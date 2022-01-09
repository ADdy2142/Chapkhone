using AutoMapper;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Chapkhone.WebApp.Controllers
{
    public class HomeController : MyControllerBase
    {
        private readonly IMapper _mapper;

        public HomeController(IMapper mapper, UserManager<Customer> userManager) : base(userManager)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "صفحه اصلی";

            var viewModel = new IndexViewModel();

            return View(viewModel);
        }
    }
}