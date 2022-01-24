using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Admins.Contollers
{
    [Area("Admins")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userName = null)
        {
            ViewBag.Title = "مدیریت سفارشات";

            var orders = await _unitOfWork.Orders.GetAllAsync(o => o.OrderDate < DateTime.Now.AddDays(-30), includeProperties: new string[] { "SpecificationOrders.SpecificationOrderImages" });
            if (orders != null && orders.Count > 0)
            {
                foreach (var order in orders)
                {
                    foreach (var specificationOrder in order.SpecificationOrders)
                    {
                        var specificationOrderImages = new List<SpecificationOrderImage>();
                        foreach (var specificationOrderImage in specificationOrder.SpecificationOrderImages)
                        {
                            if (!string.IsNullOrEmpty(specificationOrderImage.ImageName))
                                DeleteImage(specificationOrderImage.ImageName);

                            specificationOrderImages.Add(specificationOrderImage);
                        }

                        if (specificationOrderImages.Count > 0)
                        {
                            _unitOfWork.SpecificationOrderImages.DeleteRange(specificationOrderImages);
                            await _unitOfWork.SaveAsync();
                        }
                    }

                    if (!order.IsFinalized)
                    {
                        _unitOfWork.Orders.Delete(order);
                        await _unitOfWork.SaveAsync();
                    }
                }
            }

            if (string.IsNullOrEmpty(userName))
                orders = await _unitOfWork.Orders.GetAllAsync(filter: o => o.TransactionStatus, orderBy: o => o.OrderByDescending(o => o), includeProperties: new string[] { "Customer", "SpecificationOrders" });
            else
                orders = await _unitOfWork.Orders.GetAllAsync(filter: o => o.TransactionStatus && (o.Customer.PhoneNumber.Contains(userName) || o.Customer.Email.Contains(userName) || o.Customer.FirstName.Contains(userName) || o.Customer.LastName.Contains(userName)), orderBy: o => o.OrderByDescending(o => o), includeProperties: new string[] { "Customer", "SpecificationOrders" });

            return View(orders);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{orderId}")]
        public async Task<IActionResult> OrderDetails(int? orderId)
        {
            if (orderId == null)
                return BadRequest();

            var order = await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.Id == orderId.Value, includeProperties: new string[] { "Customer", "SpecificationOrders.SpecificationOrderValues.SpecificationOrderTitle.SpecificationOrderGroup.DesignGroup", "SpecificationOrders.SpecificationOrderImages" });
            if (order == null)
                return NotFound();

            if (!order.IsVisitedByAdmin)
            {
                order.IsVisitedByAdmin = true;

                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveAsync();
            }

            ViewBag.Title = "جزئیات سفارش";

            return View(order);
        }
    }
}