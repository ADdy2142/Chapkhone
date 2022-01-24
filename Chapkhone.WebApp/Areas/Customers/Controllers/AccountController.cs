using AutoMapper;
using Chapkhone.DataAccess.Constants;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZarinPal.Class;

namespace Chapkhone.WebApp.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Authorize]
    public class AccountController : MyControllerBase
    {
        private readonly Payment _payment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Customer> userManager, SignInManager<Customer> signInManager, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;

            var expose = new Expose();
            _payment = expose.CreatePayment();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.Title = "داشبورد";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "داشبورد",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                }
            };

            var customer = await _userManager.FindByNameAsync(CustomerUserName);
            var viewModel = _mapper.Map<CustomerDetailsVM>(customer);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeCustomerImage(IFormFile customerImage)
        {
            if (customerImage != null)
            {
                var customer = await _userManager.FindByNameAsync(CustomerUserName);
                if (customer == null)
                    return NotFound();

                if (!string.IsNullOrEmpty(customer.ImageName) && customer.ImageName != "default-user-image.png")
                    DeleteImage(customer.ImageName);

                customer.ImageName = await CopyImageAsync(customerImage);
                await _userManager.UpdateAsync(customer);
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> EditPersonalInfo(bool emailExists = false)
        {
            ViewBag.Title = "ویرایش اطلاعات شخصی";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "ویرایش اطلاعات شخصی",
                    Url = Url.Action("EditPersonalInfo", "Account", new { area = "Customers" })
                }
            };

            var customer = await _userManager.FindByNameAsync(CustomerUserName);
            var viewModel = _mapper.Map<EditCustomerPersonalInfoVM>(customer);
            ViewBag.EmailExists = emailExists;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPersonalInfo(EditCustomerPersonalInfoVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var customerByEmail = await _userManager.FindByEmailAsync(viewModel.Email);
                if (customerByEmail != null)
                    return RedirectToAction(nameof(EditPersonalInfo), new { emailExists = true });

                var customerInDb = await _userManager.FindByNameAsync(CustomerUserName);
                if (customerInDb == null)
                    return NotFound();

                var customer = _mapper.Map<Customer>(viewModel);
                customerInDb.FirstName = customer.FirstName;
                customerInDb.LastName = customer.LastName;
                customerInDb.Email = customer.Email;

                await _userManager.UpdateAsync(customerInDb);

                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Title = "ویرایش اطلاعات شخصی";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "ویرایش اطلاعات شخصی",
                    Url = Url.Action("EditPersonalInfo", "Account", new { area = "Customers" })
                }
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            ViewBag.Title = "تغییر کلمه عبور";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "تغییر کلمه عبور",
                    Url = Url.Action("ChangePassword", "Account", new { area = "Customers" })
                }
            };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeCustomerPasswordVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = await _userManager.FindByNameAsync(CustomerUserName);
                if (customer == null)
                    return NotFound();

                var result = await _userManager.ChangePasswordAsync(customer, viewModel.CurrentPassword, viewModel.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(customer, false);
                    return RedirectToAction(nameof(Dashboard));
                }
                else
                    ModelState.AddModelError("WrongPassword", "کمله عبور شما اشتباه است. لطفا دوباره کلمه عبور خود را بررسی کنید.");
            }

            ViewBag.Title = "تغییر کلمه عبور";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "تغییر کلمه عبور",
                    Url = Url.Action("ChangePassword", "Account", new { area = "Customers" })
                }
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ShoppingCart(PaymentStatus? paymentStatus)
        {
            var customer = await _userManager.FindByNameAsync(CustomerUserName);
            if (customer == null)
                return NotFound();

            var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.CustomerId == customer.Id && !o.IsFinalized, includeProperties: new string[] { "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });

            ViewBag.Title = "سبد خرید";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "سبد خرید",
                    Url = Url.Action("ShoppingCart", "Account", new { area = "Customers" })
                }
            };

            if (paymentStatus != null)
                ViewBag.PaymentStatus = paymentStatus == PaymentStatus.Failure ? "مشتری گرامی درخواست شما با خطا مواجه شد. لطفا چند دقیقه دیگر دوباره امتحان کنید." : "مشتری گرامی درخواست شما با موفقیت انجام شد.";

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteShoppingCartItem(int? id)
        {
            if (id == null)
                return BadRequest();

            var specificationOrder = await _unitOfWork.SpecificationOrders.GetFirstOrDefaultAsync(so => so.Id == id.Value, includeProperties: new string[] { "SpecificationOrderValues", "SpecificationOrderImages" });
            if (specificationOrder == null)
                return NotFound();

            var orderId = specificationOrder.OrderId;

            if (specificationOrder.SpecificationOrderImages.Any())
            {
                foreach (var specificationOrderImage in specificationOrder.SpecificationOrderImages)
                {
                    if (!string.IsNullOrEmpty(specificationOrderImage.ImageName))
                        DeleteImage(specificationOrderImage.ImageName);
                }

                _unitOfWork.SpecificationOrderImages.DeleteRange(specificationOrder.SpecificationOrderImages);
            }

            if (specificationOrder.SpecificationOrderValues.Any())
                _unitOfWork.SpecificationOrderValues.DeleteRange(specificationOrder.SpecificationOrderValues);

            _unitOfWork.SpecificationOrders.Delete(specificationOrder);
            await _unitOfWork.SaveAsync();

            var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.Id == orderId, includeProperties: new string[] { "Customer", "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });
            if (order.SpecificationOrders.Any())
            {
                order.TotalPrice = order.SpecificationOrders.Sum(so => so.TotalPrice);
                order.TotalDiscount = order.SpecificationOrders.Sum(so => so.TotalDiscount);

                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                _unitOfWork.Orders.Delete(order);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction(nameof(ShoppingCart));
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var customer = await _userManager.FindByNameAsync(CustomerUserName);
            if (customer == null)
                return NotFound();

            var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => !o.IsFinalized, includeProperties: new string[] { "Customer", "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });
            if (order == null)
                return NotFound();

            string callbackUrl = Urls.SiteUrl + Url.Action("TransactionStatus", "Account", new { area = "Customers", orderId = order.Id });
            var result = await _payment.Request(new Dto.Payment.DtoRequest
            {
                Amount = order.FinalPrice,
                Description = "سفارش طراحی",
                CallbackUrl = callbackUrl,
                MerchantId = Variables.MerchantId,
                Email = customer.Email,
                Mobile = customer.PhoneNumber
            }, ZarinPal.Class.Payment.Mode.sandbox);

            if (result.Status == 100)
            {
                string authority = result.Authority;
                //string gatewayUrl = "https://www.zarinpal.com/pg/StartPay/" + authority;
                string gatewayUrl = "https://sandbox.zarinpal.com/pg/StartPay/" + authority;
                return Redirect(gatewayUrl);
            }

            return RedirectToAction(nameof(ShoppingCart), new { paymentStatus = PaymentStatus.Failure });
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{orderId}")]
        public async Task<IActionResult> Payment(int? orderId)
        {
            if (orderId == null)
                return BadRequest();

            var customer = await _userManager.FindByNameAsync(CustomerUserName);
            if (customer == null)
                return NotFound();

            var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.Id == orderId.Value, includeProperties: new string[] { "Customer", "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });
            if (order == null)
                return NotFound();

            string callbackUrl = Urls.SiteUrl + Url.Action("TransactionStatus", "Account", new { area = "Customers", orderId = order.Id });
            var result = await _payment.Request(new Dto.Payment.DtoRequest
            {
                Amount = order.FinalPrice,
                Description = "سفارش طراحی",
                CallbackUrl = callbackUrl,
                MerchantId = Variables.MerchantId,
                Email = customer.Email,
                Mobile = customer.PhoneNumber
            }, ZarinPal.Class.Payment.Mode.sandbox);

            if (result.Status == 100)
            {
                string authority = result.Authority;
                //string gatewayUrl = "https://www.zarinpal.com/pg/StartPay/" + authority;
                string gatewayUrl = "https://sandbox.zarinpal.com/pg/StartPay/" + authority;
                return Redirect(gatewayUrl);
            }

            return RedirectToAction(nameof(ShoppingCart), new { paymentStatus = PaymentStatus.Failure });
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{orderId}")]
        public async Task<IActionResult> TransactionStatus(int? orderId, string authority, string status)
        {
            if (orderId == null)
                return BadRequest();

            var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.Id == orderId.Value, includeProperties: new string[] { "Customer", "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });
            if (order == null)
                return NotFound();

            var verification = await _payment.Verification(new Dto.Payment.DtoVerification
            {
                Amount = order.FinalPrice,
                Authority = authority,
                MerchantId = Variables.MerchantId
            }, ZarinPal.Class.Payment.Mode.sandbox);

            order.IsFinalized = true;

            if (!string.IsNullOrEmpty(authority) && !string.IsNullOrEmpty(status) && string.Equals(status, "ok", StringComparison.OrdinalIgnoreCase))
            {
                if (verification.Status == 100)
                {
                    order.TransactionStatus = true;

                    await _unitOfWork.Orders.UpdateAsync(order);
                    await _unitOfWork.SaveAsync();
                }

                ViewBag.RefId = verification.RefId;

                return View(order.TransactionStatus);
            }

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveAsync();

            ViewBag.Title = "وضعیت تراکنش";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "وضعیت تراکنش",
                    Url = Url.Action("TransactionStatus", "Account", new { area = "Customers", orderId = order.Id, authority = authority, status = status })
                }
            };

            return View(order.TransactionStatus);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var customer = await _userManager.FindByNameAsync(CustomerUserName);
            if (customer == null)
                return NotFound();

            ViewBag.Title = "سفارش های من";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "سفارش های من",
                    Url = Url.Action("Orders", "Account", new { area = "Customers" })
                }
            };

            var orders = await _unitOfWork.Orders.GetAllAsync(o => o.CustomerId == customer.Id && o.IsFinalized, includeProperties: new string[] { "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });

            return View(orders);
        }

        [HttpGet]
        [Route("[area]/[controller]/orders/[action]/{orderId}")]
        public async Task<IActionResult> OrderDetails(int? orderId)
        {
            if (orderId == null)
                return BadRequest();

            var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.Id == orderId.Value, includeProperties: new string[] { "Customer", "CustomerComment", "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });
            if (order == null)
                return NotFound();

            ViewBag.Title = "جزئیات سفارش";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "سفارش های من",
                    Url = Url.Action("Orders", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "جزئیات سفارش",
                    Url = Url.Action("OrderDetails", "Account", new { area = "Customers", orderId = order.Id })
                }
            };

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> RegisterComment(int? id)
        {
            if (id == null)
                return BadRequest();

            var order = await _unitOfWork.Orders.FindAsync(id.Value);
            if (order == null)
                return NotFound();

            var viewModel = new UpsertCustomerCommentVM { OrderId = order.Id };

            ViewBag.Title = "ثبت نظر";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "ثبت نظر",
                    Url = Url.Action("RegisterComment", "Account", new { area = "Customers", id = order.Id })
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterComment(UpsertCustomerCommentVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = await _userManager.FindByNameAsync(CustomerUserName);
                if (customer == null)
                    return NotFound();

                var order = await _unitOfWork.Orders.FindAsync(viewModel.OrderId);
                if (order == null)
                    return NotFound();

                var customerComment = _mapper.Map<CustomerComment>(viewModel);
                customerComment.Id = 0;
                customerComment.CustomerId = customer.Id;
                customerComment.OrderId = order.Id;
                await _unitOfWork.CustomerComments.AddAsync(customerComment);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Title = "ثبت نظر";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home", new { area = "" })
                },
                new BreadCrumb
                {
                    Title = "پروفایل",
                    Url = Url.Action("Dashboard", "Account", new { area = "Customers" })
                },
                new BreadCrumb
                {
                    Title = "ثبت نظر",
                    Url = Url.Action("RegisterComment", "Account", new { area = "Customers", id = viewModel.OrderId })
                }
            };

            return View(viewModel);
        }
    }
}