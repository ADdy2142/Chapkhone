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
    public class SiteNotificationsController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SiteNotificationsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var siteNotifications = await _unitOfWork.SiteNotifications.GetAllAsync();
            var viewModels = _mapper.Map<ICollection<GetSiteNotificationVM>>(siteNotifications);

            ViewBag.Title = "مدیریت اعلان سایت";

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Title = "اضافه کردن اعلان جدید";

            return View(new UpsertSiteNotificationVM { Id = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UpsertSiteNotificationVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var siteNotification = _mapper.Map<SiteNotification>(viewModel);
                if (viewModel.ImageXL != null)
                    siteNotification.ImageNameXL = await CopyImageAsync(viewModel.ImageXL);

                if (viewModel.ImageLG != null)
                    siteNotification.ImageNameLG = await CopyImageAsync(viewModel.ImageLG);

                if (viewModel.ImageMD != null)
                    siteNotification.ImageNameMD = await CopyImageAsync(viewModel.ImageMD);

                if (viewModel.ImageSM != null)
                    siteNotification.ImageNameSM = await CopyImageAsync(viewModel.ImageSM);

                var siteNotifications = await _unitOfWork.SiteNotifications.GetAllAsync();
                if (siteNotifications != null && siteNotifications.Count == 0)
                    siteNotification.IsDefault = true;

                await _unitOfWork.SiteNotifications.AddAsync(siteNotification);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "اضافه کردن اعلان جدید";

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var siteNotification = await _unitOfWork.SiteNotifications.FindAsync(id.Value);
            if (siteNotification == null)
                return NotFound();

            ViewBag.Title = "ویرایش اعلان";
            var viewModel = _mapper.Map<UpsertSiteNotificationVM>(siteNotification);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpsertSiteNotificationVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var siteNotification = _mapper.Map<SiteNotification>(viewModel);
                var siteNotificationInDb = await _unitOfWork.SiteNotifications.FindAsync(siteNotification.Id);
                if (siteNotificationInDb == null)
                    return NotFound();

                if (viewModel.ImageXL != null)
                {
                    if (!string.IsNullOrEmpty(siteNotificationInDb.ImageNameXL))
                        DeleteImage(siteNotificationInDb.ImageNameXL);

                    siteNotification.ImageNameXL = await CopyImageAsync(viewModel.ImageXL);
                }

                if (viewModel.ImageLG != null)
                {
                    if (!string.IsNullOrEmpty(siteNotificationInDb.ImageNameLG))
                        DeleteImage(siteNotificationInDb.ImageNameLG);

                    siteNotification.ImageNameLG = await CopyImageAsync(viewModel.ImageLG);
                }

                if (viewModel.ImageMD != null)
                {
                    if (!string.IsNullOrEmpty(siteNotificationInDb.ImageNameMD))
                        DeleteImage(siteNotificationInDb.ImageNameMD);

                    siteNotification.ImageNameMD = await CopyImageAsync(viewModel.ImageMD);
                }

                if (viewModel.ImageSM != null)
                {
                    if (!string.IsNullOrEmpty(siteNotificationInDb.ImageNameSM))
                        DeleteImage(siteNotificationInDb.ImageNameSM);

                    siteNotification.ImageNameSM = await CopyImageAsync(viewModel.ImageSM);
                }

                siteNotificationInDb.ImageNameXL = siteNotification.ImageNameXL;
                siteNotificationInDb.ImageNameLG = siteNotification.ImageNameLG;
                siteNotificationInDb.ImageNameMD = siteNotification.ImageNameMD;
                siteNotificationInDb.ImageNameSM = siteNotification.ImageNameSM;
                await _unitOfWork.SiteNotifications.UpdateAsync(siteNotificationInDb);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "ویرایش اعلان";

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var siteNotification = await _unitOfWork.SiteNotifications.FindAsync(id.Value);
            if (siteNotification == null)
                return NotFound();

            if (!string.IsNullOrEmpty(siteNotification.ImageNameXL))
                DeleteImage(siteNotification.ImageNameXL);

            if (!string.IsNullOrEmpty(siteNotification.ImageNameLG))
                DeleteImage(siteNotification.ImageNameLG);

            if (!string.IsNullOrEmpty(siteNotification.ImageNameMD))
                DeleteImage(siteNotification.ImageNameMD);

            if (!string.IsNullOrEmpty(siteNotification.ImageNameSM))
                DeleteImage(siteNotification.ImageNameSM);

            _unitOfWork.SiteNotifications.Delete(siteNotification);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SaveAsDefault(int? id)
        {
            if (id == null)
                return BadRequest();

            var siteNotification = await _unitOfWork.SiteNotifications.FindAsync(id.Value);
            if (siteNotification == null)
                return NotFound();

            var defaultSiteNotification = await _unitOfWork.SiteNotifications.GetFirstOrDefaultAsync(sn => sn.IsDefault);
            if (defaultSiteNotification != null)
            {
                defaultSiteNotification.IsDefault = false;
                await _unitOfWork.SiteNotifications.UpdateAsync(defaultSiteNotification);
                await _unitOfWork.SaveAsync();
            }

            siteNotification.IsDefault = true;

            await _unitOfWork.SiteNotifications.UpdateAsync(siteNotification);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}