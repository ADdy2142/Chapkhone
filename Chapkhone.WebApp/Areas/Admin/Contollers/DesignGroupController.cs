using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chapkhone.WebApp.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class DesignGroupController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignGroupController(IUnitOfWork unitOfWork, UserManager<Customer> userManager) : base(userManager)
        {
            _unitOfWork = unitOfWork;
        }

        [ActionName("index")]
        public IActionResult Index()
        {
            ViewBag.Title = "مدیریت گروه های طراحی";
            return View();
        }
    }
}