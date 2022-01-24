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
        public async Task<IActionResult> ExclusiveOrder()
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
                    Url = Url.Action("ExclusiveOrder", "Home")
                }
            };

            return View(groups);
        }

        [HttpGet]
        [Route("[controller]/[action]/{designGroupId}")]
        [Authorize]
        public async Task<IActionResult> OrderDetails(int? designGroupId)
        {
            if (designGroupId == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.GetFirstOrDefaultAsync(dg => dg.Id == designGroupId.Value, includeProperties: new string[] { "SpecificationOrderGroups", "SpecificationOrderGroups.SpecificationOrderTitles" });
            if (designGroup == null)
                return NotFound();

            ViewBag.Title = $"سفارش {designGroup.Title}";
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
                    Url = Url.Action("ExclusiveOrder", "Home")
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
                Qty = 1,
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

                int totalDiscount = 0;
                int totalPrice = 0;

                var product = await _unitOfWork.Products.FindAsync(viewModel.ProductId);
                if (product != null)
                {
                    totalDiscount = (product.Price - product.FinalPrice) * viewModel.Qty;
                    totalPrice = product.Price * viewModel.Qty;
                }
                else
                {
                    var designGroup = await _unitOfWork.DesignGroups.FindAsync(viewModel.DesignGroupId);
                    if (designGroup == null)
                        return BadRequest();

                    totalDiscount = (designGroup.DesignPrice - designGroup.FinalPrice) * viewModel.Qty;
                    totalPrice = designGroup.DesignPrice * viewModel.Qty;
                }

                var specificationOrder = new SpecificationOrder();
                specificationOrder.Title = viewModel.Title;
                specificationOrder.Qty = viewModel.Qty;
                specificationOrder.Description = viewModel.Description;
                specificationOrder.OrderId = order.Id;
                specificationOrder.ProductId = product?.Id ?? 0;
                specificationOrder.DesignGroupId = product?.DesignGroupId ?? viewModel.DesignGroupId;
                specificationOrder.TotalPrice = totalPrice;
                specificationOrder.TotalDiscount = totalDiscount;

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
            }

            return RedirectToAction("ShoppingCart", "Account", new { Area = "Customers" });
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