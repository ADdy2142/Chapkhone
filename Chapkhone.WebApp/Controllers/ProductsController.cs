using AutoMapper;
using Chapkhone.DataAccess.Constants;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.Utilities.Pagination;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Controllers
{
    public class ProductsController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]/[action]/{designGroupId}")]
        public async Task<IActionResult> List(int? designGroupId, int orderBy = 0, int pageIndex = 1)
        {
            if (designGroupId == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.FindAsync(designGroupId.Value);
            if (designGroup == null)
                return NotFound();

            var products = await _unitOfWork.Products.GetAllAsync(p => p.DesignGroupId == designGroup.Id, includeProperties: new string[] { "ProductImages", "DesignGroup" });
            switch (orderBy)
            {
                case 0:
                    products = products.OrderByDescending(p => p.Id).ToList();
                    break;

                case 1:
                    products = products.OrderBy(p => p.Id).ToList();
                    break;

                case 2:
                    products = products.OrderByDescending(p => p.FinalPrice).ToList();
                    break;

                case 3:
                    products = products.OrderBy(p => p.FinalPrice).ToList();
                    break;
            }

            var paginatedList = PaginatedList<Product>.Create(products, pageIndex, 12);

            ViewBag.Title = $"لیست محصولات {designGroup.Title}";
            ViewBag.DesignGroupId = designGroup.Id;
            ViewBag.SiteUrl = Urls.SiteUrl;
            ViewBag.PageIndex = pageIndex;
            ViewBag.OrderBy = orderBy;
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home")
                },
                new BreadCrumb
                {
                    Title = $"محصولات {designGroup.Title}",
                    Url = Url.Action("List", "Products", new { designGroupId = designGroup.Id, orderBy = orderBy, pageIndex = pageIndex })
                }
            };

            return View(paginatedList);
        }

        [HttpGet]
        [Route("[controller]/[action]/{designGroupId}")]
        public async Task<IActionResult> GetProducts(int? designGroupId, int orderBy = 0, int pageIndex = 1)
        {
            if (designGroupId == null)
                return BadRequest();

            var designGroup = await _unitOfWork.DesignGroups.FindAsync(designGroupId.Value);
            if (designGroup == null)
                return NotFound();

            var products = await _unitOfWork.Products.GetAllAsync(p => p.DesignGroupId == designGroup.Id, includeProperties: new string[] { "ProductImages", "DesignGroup" });
            switch (orderBy)
            {
                case 0:
                    products = products.OrderByDescending(p => p.Id).ToList();
                    break;

                case 1:
                    products = products.OrderBy(p => p.Id).ToList();
                    break;

                case 2:
                    products = products.OrderByDescending(p => p.FinalPrice).ToList();
                    break;

                case 3:
                    products = products.OrderBy(p => p.FinalPrice).ToList();
                    break;
            }

            var paginatedList = PaginatedList<Product>.Create(products, pageIndex, 12);

            return View(paginatedList);
        }

        [HttpGet]
        [Route("[controller]/{designGroupTitle}/[action]/{id}/{title}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _unitOfWork.Products.GetFirstOrDefaultAsync(p => p.Id == id.Value, includeProperties: new string[] { "ProductImages", "DesignGroup.SpecificationOrderGroups.SpecificationOrderTitles" });
            if (product == null)
                return NotFound();

            ViewBag.Title = product.Title;
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home")
                },
                new BreadCrumb
                {
                    Title = $"محصولات {product.DesignGroup.Title}",
                    Url = Url.Action("List", "Products", new { designGroupId = product.DesignGroup.Id })
                },
                new BreadCrumb
                {
                    Title = product.Title,
                    Url = Url.Action("Details", "Products", new { designGroupTitle = product.DesignGroup.Title.Replace(" ", "-"), id = product.Id, title = product.Title.Replace(" ", "-") })
                }
            };

            var viewModel = new GetOrderDetailsVM
            {
                ProductId = product.Id,
                DesignGroupId = product.DesignGroup.Id,
                Qty = 1,
                DesignGroupImageUrl = product.DesignGroup.ImageUrl,
                SpecificationOrderGroups = _mapper.Map<ICollection<SpecificationOrderGroupVM>>(product.DesignGroup.SpecificationOrderGroups)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search = null, int pageIndex = 1)
        {
            var products = new List<Product>();
            if (string.IsNullOrEmpty(search))
            {
                products = await _unitOfWork.Products.GetAllAsync(includeProperties: new string[] { "ProductImages", "DesignGroup" });
            }
            else
            {
                products = await _unitOfWork.Products.GetAllAsync(p => p.Title.Contains(search), includeProperties: new string[] { "ProductImages", "DesignGroup" });
            }

            var paginatedList = PaginatedList<Product>.Create(products, pageIndex, 12);

            ViewBag.Title = "نتایج جستجو";
            ViewBag.SearchText = search;
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb
                {
                    Title = "چاپخانه بخشی",
                    Url = Url.Action("Index", "Home")
                },
                new BreadCrumb
                {
                    Title = $"نتایج جستجو {search}".Trim(),
                    Url = Url.Action("Search", "Products", new { search = search, pageIndex = pageIndex })
                }
            };

            return View(paginatedList);
        }
    }
}