using AutoMapper;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Admins.Contollers
{
    [Area("Admins")]
    [Authorize(Roles = "Admin")]
    public class CustomerCommentsController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerCommentsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "مدیریت نظرات مشتریان";

            var customerComments = await _unitOfWork.CustomerComments.GetAllAsync(cc => !cc.AllowedToShow);
            var viewModels = _mapper.Map<ICollection<GetCustomerCommentVM>>(customerComments);

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var customerComment = await _unitOfWork.CustomerComments.GetFirstOrDefaultAsync(cc => cc.Id == id.Value, includeProperties: new string[] { "Customer", "Order" });
            if (customerComment == null)
                return NotFound();

            ViewBag.Title = "جزئیات نظر مشتری";

            return View(customerComment);
        }

        [HttpGet]
        public async Task<IActionResult> GivePermission(int? id)
        {
            if (id == null)
                return BadRequest();

            var customerComment = await _unitOfWork.CustomerComments.FindAsync(id.Value);
            if (customerComment == null)
                return NotFound();

            customerComment.AllowedToShow = true;

            await _unitOfWork.CustomerComments.UpdateAsync(customerComment);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var customerComment = await _unitOfWork.CustomerComments.FindAsync(id.Value);
            if (customerComment == null)
                return NotFound();

            _unitOfWork.CustomerComments.Delete(customerComment);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}