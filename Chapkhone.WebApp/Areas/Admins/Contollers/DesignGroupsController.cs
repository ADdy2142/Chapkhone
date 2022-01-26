using AutoMapper;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Admins.Contollers
{
    [Area("Admins")]
    [Authorize(Roles = "Admin")]
    public class DesignGroupsController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DesignGroupsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "مدیریت گروه های طراحی";
            var designGroups = await _unitOfWork.DesignGroups.GetAllAsync(includeProperties: new string[] { "Products" });
            var viewModels = _mapper.Map<ICollection<GetDesignGroupVM>>(designGroups);

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Title = "افزودن گروه جدید";
            return View(new UpsertDesignGroupVM { Id = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UpsertDesignGroupVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var designGroup = _mapper.Map<DesignGroup>(viewModel);
                if (viewModel.DesignGroupImage != null)
                    designGroup.ImageName = await CopyImageAsync(viewModel.DesignGroupImage);

                await _unitOfWork.DesignGroups.AddAsync(designGroup);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Title = "افزودن گروه جدید";
                return RedirectToAction(nameof(Add), viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.GetFirstOrDefaultAsync(dg => dg.Id == id.Value);
            if (designGroup == null)
                return NotFound();

            ViewBag.Title = "ویرایش گروه";
            var viewModel = _mapper.Map<UpsertDesignGroupVM>(designGroup);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpsertDesignGroupVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var designGroup = _mapper.Map<DesignGroup>(viewModel);
                var designGroupInDb = await _unitOfWork.DesignGroups.FindAsync(designGroup.Id);
                if (designGroupInDb == null)
                    return NotFound();

                designGroupInDb.Title = designGroup.Title;
                designGroupInDb.ShortDescription = designGroup.ShortDescription;
                designGroupInDb.UnitPrice = designGroup.UnitPrice;
                designGroupInDb.Discount = designGroup.Discount;
                if (viewModel.DesignGroupImage != null)
                {
                    if (!string.IsNullOrEmpty(designGroupInDb.ImageName))
                        DeleteImage(designGroupInDb.ImageName);

                    designGroupInDb.ImageName = await CopyImageAsync(viewModel.DesignGroupImage);
                }

                await _unitOfWork.DesignGroups.UpdateAsync(designGroupInDb);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Title = "ویرایش گروه";
                return RedirectToAction(nameof(Add), viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.FindAsync(id.Value);
            if (designGroup == null)
                return NotFound();

            if (!string.IsNullOrEmpty(designGroup.ImageName))
                DeleteImage(designGroup.ImageName);

            _unitOfWork.DesignGroups.Delete(designGroup);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Discount()
        {
            ViewBag.Title = "تخفیف کلی";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Discount(GeneralDiscountVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var designGroups = await _unitOfWork.DesignGroups.GetAllAsync();
                foreach (var designGroup in designGroups)
                {
                    designGroup.Discount = viewModel.Discount;

                    await _unitOfWork.DesignGroups.UpdateAsync(designGroup);
                    await _unitOfWork.SaveAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "اعمال تخفیف روی تمام محصولات";

            return View(viewModel);
        }
    }
}