using AutoMapper;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Admins.Contollers
{
    [Area("Admins")]
    [Authorize(Roles = "Admin")]
    public class SpecificationOrdersController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecificationOrdersController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region SpecificationOrderGroup

        [HttpGet]
        [Route("[area]/[controller]/[action]/{designGroupId}")]
        public async Task<IActionResult> OrderGroups(int? designGroupId)
        {
            if (designGroupId == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.GetFirstOrDefaultAsync(group => group.Id == designGroupId.Value, includeProperties: "SpecificationOrderGroups");
            if (designGroup == null)
                return NotFound();

            ViewBag.Title = "مدیریت مشخصات سفارش";
            ViewBag.DesignGroupId = designGroupId.Value;
            ViewBag.DesignGroupTitle = designGroup.Title;
            var viewModels = _mapper.Map<ICollection<GetSpecificationOrderGroupVM>>(designGroup.SpecificationOrderGroups);

            return View(viewModels);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{designGroupId}")]
        public async Task<IActionResult> AddOrderGroup(int? designGroupId)
        {
            if (designGroupId == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.FindAsync(designGroupId.Value);
            if (designGroup == null)
                return NotFound();

            ViewBag.Title = "اضافه کردن گروه مشخصات سفارش";
            var viewModel = new UpsertSpecificationOrderGroupVM { Id = 0, DesignGroupId = designGroup.Id };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrderGroup(UpsertSpecificationOrderGroupVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var specificationOrderGroup = _mapper.Map<SpecificationOrderGroup>(viewModel);
                var designGroup = await _unitOfWork.DesignGroups.FindAsync(viewModel.DesignGroupId);
                if (designGroup == null)
                    return NotFound();

                specificationOrderGroup.DesignGroup = designGroup;
                await _unitOfWork.SpecificationOrderGroups.AddAsync(specificationOrderGroup);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(OrderGroups), new { designGroupId = viewModel.DesignGroupId });
            }
            else
            {
                ViewBag.Title = "اضافه کردن گروه مشخصات سفارش";
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditOrderGroup(int? id)
        {
            if (id == null)
                return BadRequest();

            var specificationOrderGroup = await _unitOfWork.SpecificationOrderGroups.FindAsync(id.Value);
            if (specificationOrderGroup == null)
                return NotFound();

            ViewBag.Title = $"ویرایش گروه {specificationOrderGroup.Title}";
            var viewModel = _mapper.Map<UpsertSpecificationOrderGroupVM>(specificationOrderGroup);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrderGroup(UpsertSpecificationOrderGroupVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var specificationOrderGroup = _mapper.Map<SpecificationOrderGroup>(viewModel);
                var specificationOrderGroupInDb = await _unitOfWork.SpecificationOrderGroups.FindAsync(specificationOrderGroup.Id);
                if (specificationOrderGroupInDb == null)
                    return NotFound();

                specificationOrderGroupInDb.Title = specificationOrderGroup.Title;
                await _unitOfWork.SpecificationOrderGroups.UpdateAsync(specificationOrderGroupInDb);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(OrderGroups), new { designGroupId = viewModel.DesignGroupId });
            }
            else
            {
                var specificationOrderGroup = await _unitOfWork.DesignGroups.FindAsync(viewModel.Id);
                ViewBag.Title = $"ویرایش گروه {specificationOrderGroup.Title}";
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrderGroup(int? id)
        {
            if (id == null)
                return BadRequest();

            var specificationOrderGroup = await _unitOfWork.SpecificationOrderGroups.FindAsync(id.Value);
            if (specificationOrderGroup == null)
                return NotFound();

            var designGroupId = specificationOrderGroup.DesignGroupId;
            _unitOfWork.SpecificationOrderGroups.Delete(specificationOrderGroup);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(OrderGroups), new { designGroupId = designGroupId });
        }

        #endregion SpecificationOrderGroup

        #region SpecificationOrderTitle

        [HttpGet]
        [Route("[area]/[controller]/[action]/{orderGroupId}")]
        public async Task<IActionResult> OrderTitles(int? orderGroupId)
        {
            if (orderGroupId == null)
                return BadRequest();

            var specificationOrderGroup = await _unitOfWork.SpecificationOrderGroups.GetFirstOrDefaultAsync(group => group.Id == orderGroupId.Value, includeProperties: "SpecificationOrderTitles");
            if (specificationOrderGroup == null)
                return NotFound();

            ViewBag.Title = "مدیریت مشخصات سفارش";
            ViewBag.OrderGroupId = specificationOrderGroup.Id;
            ViewBag.OrderGroupTitle = specificationOrderGroup.Title;
            var viewModels = _mapper.Map<ICollection<GetSpecificationOrderTitleVM>>(specificationOrderGroup.SpecificationOrderTitles);

            return View(viewModels);
        }

        [HttpGet]
        [Route("[area]/[controller]/[action]/{orderGroupId}")]
        public async Task<IActionResult> AddOrderTitle(int? orderGroupId)
        {
            if (orderGroupId == null)
                return BadRequest();

            var specificationOrderGroup = await _unitOfWork.SpecificationOrderGroups.FindAsync(orderGroupId.Value);
            if (specificationOrderGroup == null)
                return NotFound();

            ViewBag.Title = "اضافه کردن عنوان مشخصات سفارش";
            var viewModel = new UpsertSpecificationOrderTitleVM { Id = 0, SpecificationOrderGroupId = specificationOrderGroup.Id };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrderTitle(UpsertSpecificationOrderTitleVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var specificationOrderTitle = _mapper.Map<SpecificationOrderTitle>(viewModel);
                var specificationOrderGroup = await _unitOfWork.SpecificationOrderGroups.FindAsync(viewModel.SpecificationOrderGroupId);
                if (specificationOrderGroup == null)
                    return NotFound();

                specificationOrderTitle.SpecificationOrderGroup = specificationOrderGroup;
                await _unitOfWork.SpecificationOrderTitles.AddAsync(specificationOrderTitle);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(OrderTitles), new { orderGroupId = viewModel.SpecificationOrderGroupId });
            }
            else
            {
                ViewBag.Title = "اضافه کردن عنوان مشخصات سفارش";
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditOrderTitle(int? id)
        {
            if (id == null)
                return BadRequest();

            var specificationOrderTitle = await _unitOfWork.SpecificationOrderTitles.FindAsync(id.Value);
            if (specificationOrderTitle == null)
                return NotFound();

            ViewBag.Title = $"ویرایش عنوان {specificationOrderTitle.Title}";
            var viewModel = _mapper.Map<UpsertSpecificationOrderTitleVM>(specificationOrderTitle);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrderTitle(UpsertSpecificationOrderTitleVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var specificationOrderTitle = _mapper.Map<SpecificationOrderTitle>(viewModel);
                var specificationOrderTitleInDb = await _unitOfWork.SpecificationOrderTitles.FindAsync(specificationOrderTitle.Id);
                if (specificationOrderTitleInDb == null)
                    return NotFound();

                specificationOrderTitleInDb.Title = specificationOrderTitle.Title;
                specificationOrderTitleInDb.ShortDescription = specificationOrderTitle.ShortDescription;
                specificationOrderTitleInDb.DesktopSize = specificationOrderTitle.DesktopSize;
                specificationOrderTitleInDb.TabletSize = specificationOrderTitle.TabletSize;
                await _unitOfWork.SpecificationOrderTitles.UpdateAsync(specificationOrderTitleInDb);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(OrderTitles), new { orderGroupId = viewModel.SpecificationOrderGroupId });
            }
            else
            {
                var specificationOrderTitle = await _unitOfWork.SpecificationOrderTitles.FindAsync(viewModel.Id);
                ViewBag.Title = $"ویرایش عنوان {specificationOrderTitle.Title}";
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrderTitle(int? id)
        {
            if (id == null)
                return BadRequest();

            var specificationOrderTitle = await _unitOfWork.SpecificationOrderTitles.FindAsync(id.Value);
            if (specificationOrderTitle == null)
                return NotFound();

            var orderGroupId = specificationOrderTitle.SpecificationOrderGroupId;
            _unitOfWork.SpecificationOrderTitles.Delete(specificationOrderTitle);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(OrderTitles), new { orderGroupId = orderGroupId });
        }

        #endregion SpecificationOrderTitle
    }
}