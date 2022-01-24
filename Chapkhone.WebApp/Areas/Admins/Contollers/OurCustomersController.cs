using AutoMapper;
using Chapkhone.DataAccess.Models;
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
    public class OurCustomersController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OurCustomersController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "مدیریت مشتریان ما";

            var ourCustomers = await _unitOfWork.OurCustomers.GetAllAsync();
            var viewModels = _mapper.Map<ICollection<GetOurCustomerVM>>(ourCustomers);

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Title = "اضافه کردن مشتری جدید";

            return View(new UpsertOurCustomerVM { Id = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UpsertOurCustomerVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var ourCustomer = _mapper.Map<OurCustomer>(viewModel);
                if (viewModel.Logo != null)
                    ourCustomer.LogoName = await CopyImageAsync(viewModel.Logo);

                await _unitOfWork.OurCustomers.AddAsync(ourCustomer);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "اضافه کردن مشتری جدید";

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var ourCustomer = await _unitOfWork.OurCustomers.FindAsync(id.Value);
            if (ourCustomer == null)
                return NotFound();

            ViewBag.Title = "ویرایش مشتری";
            var viewModel = _mapper.Map<UpsertOurCustomerVM>(ourCustomer);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpsertOurCustomerVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var ourCustomer = _mapper.Map<OurCustomer>(viewModel);
                var ourCustomerInDb = await _unitOfWork.OurCustomers.FindAsync(ourCustomer.Id);
                if (ourCustomerInDb == null)
                    return NotFound();

                if (viewModel.Logo != null)
                {
                    if (!string.IsNullOrEmpty(ourCustomerInDb.LogoName))
                        DeleteImage(ourCustomerInDb.LogoName);

                    ourCustomer.LogoName = await CopyImageAsync(viewModel.Logo);
                }

                ourCustomerInDb.LogoName = ourCustomer.LogoName;

                await _unitOfWork.OurCustomers.UpdateAsync(ourCustomerInDb);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "ویرایش مشتری";

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var ourCustomer = await _unitOfWork.OurCustomers.FindAsync(id.Value);
            if (ourCustomer == null)
                return NotFound();

            if (!string.IsNullOrEmpty(ourCustomer.LogoName))
                DeleteImage(ourCustomer.LogoName);

            _unitOfWork.OurCustomers.Delete(ourCustomer);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}