using AutoMapper;
using Chapkhone.DataAccess.Constants;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Controllers
{
    public class HomeController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Customer> _userManager;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Customer> userManager, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "صفحه اصلی";

            var siteNotification = await _unitOfWork.SiteNotifications.GetFirstOrDefaultAsync(sn => sn.IsDefault);
            var slideShowProducts = await _unitOfWork.ProducImages.GetAllAsync(pi => pi.ShowInSlider, includeProperties: new string[] { "Product.DesignGroup" });
            var newProducts = await _unitOfWork.Products.GetAllAsync(orderBy: p => p.OrderByDescending(p => p), includeProperties: new string[] { "DesignGroup", "ProductImages" });
            newProducts = newProducts.Take(10).ToList();
            var customerComments = await _unitOfWork.CustomerComments.GetAllAsync(cc => cc.AllowedToShow, orderBy: cc => cc.OrderByDescending(cc => cc), includeProperties: new string[] { "Customer" });
            customerComments = customerComments.Take(4).ToList();
            var ourCustomers = await _unitOfWork.OurCustomers.GetAllAsync();
            var viewModel = new MainPageVM
            {
                SiteNotification = siteNotification,
                SlideShowProducts = slideShowProducts,
                NewProducts = newProducts,
                CustomerComments = customerComments,
                OurCustomers = ourCustomers
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DesignOrder()
        {
            var groups = await _unitOfWork.DesignGroups.GetAllAsync();
            ViewBag.Title = "سفارش اختصاصی";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home")
                },
                new BreadCrumb
                {
                    Title = "سفارش اختصاصی",
                    Url = Url.Action("DesignOrder", "Home")
                }
            };

            return View(groups);
        }

        [HttpGet]
        [Route("[controller]/[action]/{designGroupId}")]
        [Authorize]
        public async Task<IActionResult> OrderDetails(int? designGroupId, string error = null)
        {
            if (designGroupId == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.GetFirstOrDefaultAsync(dg => dg.Id == designGroupId.Value, includeProperties: new string[] { "SpecificationOrderGroups", "SpecificationOrderGroups.SpecificationOrderTitles" });
            if (designGroup == null)
                return NotFound();

            ViewBag.Title = $"سفارش {designGroup.Title}";
            ViewBag.Error = error;
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home")
                },
                new BreadCrumb
                {
                    Title = "سفارش اختصاصی",
                    Url = Url.Action("DesignOrder", "Home")
                },
                new BreadCrumb
                {
                    Title = $"سفارش {designGroup.Title}",
                    Url = Url.Action("OrderDetails", "Home", new { designGroupId = designGroup.Id })
                }
            };

            var viewModel = new GetOrderDetailsVM
            {
                DesignGroupId = designGroup.Id,
                UnitPrice = designGroup.FinalPrice,
                UnitPriceType = designGroup.UnitPriceType,
                DesignGroupImageUrl = designGroup.ImageUrl,
                SpecificationOrderGroups = _mapper.Map<ICollection<SpecificationOrderGroupVM>>(designGroup.SpecificationOrderGroups)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(GetOrderDetailsVM viewModel)
        {
            var designGroup = await _unitOfWork.DesignGroups.FindAsync(viewModel.DesignGroupId);
            if (designGroup == null)
                return NotFound();

            string errorMessage = string.Empty;
            switch (designGroup.UnitPriceType)
            {
                case UnitPriceType.m2:
                    if (viewModel.OrderDetailsUnitPrice.Width <= 0 || viewModel.OrderDetailsUnitPrice.Height <= 0)
                    {
                        errorMessage = "طول یا ارتفاع وارد شده قابل قبول نیست.";
                        ModelState.AddModelError("InvalidWidthOrHeight", errorMessage);
                    }
                    break;

                case UnitPriceType.kg:
                    if (viewModel.OrderDetailsUnitPrice.Weight <= 0)
                    {
                        errorMessage = "وزن وارد شده قابل قبول نیست.";
                        ModelState.AddModelError("InvalidWeight", errorMessage);
                    }
                    break;

                case UnitPriceType.qty:
                    if (viewModel.OrderDetailsUnitPrice.Qty <= 0)
                    {
                        errorMessage = "تعداد وارد شده قابل قبول نیست.";
                        ModelState.AddModelError("InvalidWeight", errorMessage);
                    }
                    break;

                default:
                    break;
            }

            if (ModelState.IsValid)
            {
                var customer = await _userManager.FindByNameAsync(CustomerUserName);
                if (customer == null)
                    return NotFound();

                var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => !o.IsFinalized);
                if (order == null)
                {
                    order = new Order { CustomerId = customer.Id, OrderDate = DateTime.Now };
                    await _unitOfWork.Orders.AddAsync(order);
                    await _unitOfWork.SaveAsync();
                }

                var product = await _unitOfWork.Products.FindAsync(viewModel.ProductId);
                double totalDiscount = 0;
                double totalPrice = 0;

                switch (designGroup.UnitPriceType)
                {
                    case UnitPriceType.m2:
                        var width = viewModel.OrderDetailsUnitPrice.Width;
                        var height = viewModel.OrderDetailsUnitPrice.Height;
                        var m2 = (double)width / 100 * (double)height / 100;
                        totalDiscount = (designGroup.UnitPrice - designGroup.FinalPrice) * m2;
                        totalPrice = designGroup.UnitPrice * m2;
                        break;

                    case UnitPriceType.kg:
                        var weight = viewModel.OrderDetailsUnitPrice.Weight / 1000;
                        if (product != null)
                        {
                            totalDiscount = (product.Price - product.FinalPrice) * weight;
                            totalPrice = product.Price * weight;
                        }
                        else
                        {
                            totalDiscount = (designGroup.UnitPrice - designGroup.FinalPrice) * weight;
                            totalPrice = designGroup.UnitPrice * weight;
                        }
                        break;

                    case UnitPriceType.qty:
                        var qty = viewModel.OrderDetailsUnitPrice.Qty;
                        if (product != null)
                        {
                            totalDiscount = (product.Price - product.FinalPrice) * qty;
                            totalPrice = product.Price * qty;
                        }
                        else
                        {
                            totalDiscount = (designGroup.UnitPrice - designGroup.FinalPrice) * qty;
                            totalPrice = designGroup.UnitPrice * qty;
                        }
                        break;

                    default:
                        break;
                }

                var specificationOrder = new SpecificationOrder();
                specificationOrder.Title = viewModel.Title;
                specificationOrder.Width = viewModel.OrderDetailsUnitPrice.Width;
                specificationOrder.Height = viewModel.OrderDetailsUnitPrice.Height;
                specificationOrder.Weight = viewModel.OrderDetailsUnitPrice.Weight;
                specificationOrder.Qty = viewModel.OrderDetailsUnitPrice.Qty == 0 ? 1 : viewModel.OrderDetailsUnitPrice.Qty;
                specificationOrder.Description = viewModel.Description;
                specificationOrder.OrderId = order.Id;
                specificationOrder.ProductId = product?.Id ?? 0;
                specificationOrder.DesignGroupId = product?.DesignGroupId ?? viewModel.DesignGroupId;
                specificationOrder.TotalPrice = (int)totalPrice;
                specificationOrder.TotalDiscount = (int)totalDiscount;

                await _unitOfWork.SpecificationOrders.AddAsync(specificationOrder);
                await _unitOfWork.SaveAsync();

                if (viewModel.SpecificationOrderValues != null)
                {
                    foreach (var specificationOrderValue in viewModel.SpecificationOrderValues)
                    {
                        var specificationOrderTitle = await _unitOfWork.SpecificationOrderTitles.FindAsync(specificationOrderValue.SpecificationOrderTitleId);
                        if (specificationOrderTitle == null)
                            return BadRequest();

                        var newSpecificationOrderValue = new SpecificationOrderValue
                        {
                            SpecificationOrderId = specificationOrder.Id,
                            SpecificationOrderTitleId = specificationOrderTitle.Id,
                            Value = specificationOrderValue.Value
                        };

                        await _unitOfWork.SpecificationOrderValues.AddAsync(newSpecificationOrderValue);
                        await _unitOfWork.SaveAsync();
                    }
                }

                if (viewModel.CustomerImages != null)
                {
                    var specificationOrderImages = new List<SpecificationOrderImage>();
                    foreach (var image in viewModel.CustomerImages)
                    {
                        var specificationOrderImage = new SpecificationOrderImage();
                        specificationOrderImage.ImageName = await CopyImageAsync(image);
                        specificationOrderImage.SpecificationOrderId = specificationOrder.Id;
                        specificationOrderImages.Add(specificationOrderImage);
                    }

                    await _unitOfWork.SpecificationOrderImages.AddRangeAsync(specificationOrderImages);
                    await _unitOfWork.SaveAsync();
                }

                order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.Id == order.Id, includeProperties: new string[] { "Customer", "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup" });
                order.TotalPrice = order.SpecificationOrders.Sum(so => so.TotalPrice);
                order.TotalDiscount = order.SpecificationOrders.Sum(so => so.TotalDiscount);

                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveAsync();

                return RedirectToAction("ShoppingCart", "Account", new { Area = "Customers" });
            }

            return RedirectToAction(nameof(OrderDetails), new { designGroupId = designGroup.Id, error = errorMessage });
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            ViewBag.Title = "درباره ما";
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home")
                },
                new BreadCrumb
                {
                    Title = "درباره ما",
                    Url = Url.Action("AboutUs", "Home")
                }
            };

            return View();
        }

        [HttpGet]
        [Route("404")]
        public IActionResult NotFoundError()
        {
            ViewBag.Title = "صفحه مورد نظر یافت نشد.";
            return View();
        }
    }
}