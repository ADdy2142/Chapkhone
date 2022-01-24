using AutoMapper;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.UnitOfWork;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.WebApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Admins.Contollers
{
    [Area("Admins")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : MyControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery = "", int pageNumber = 1)
        {
            ViewBag.Title = "مدیریت محصولات";

            var products = await _unitOfWork.Products.GetAllAsync(includeProperties: new string[] { "DesignGroup" });
            if (!string.IsNullOrEmpty(searchQuery))
                products = products.Where(p => p.Title.Contains(searchQuery)).ToList();

            var viewModels = _mapper.Map<ICollection<GetProductVM>>(products);

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Title = "اضافه کردن محصول جدید";
            var designGroups = new List<SelectListItem>();
            var groups = await _unitOfWork.DesignGroups.GetAllAsync();
            foreach (var group in groups)
                designGroups.Add(new SelectListItem { Text = group.Title, Value = group.Id.ToString() });

            ViewBag.DesignGroups = designGroups;
            var viewModel = new UpsertProductVM { Id = 0 };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UpsertProductVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(viewModel);
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveAsync();

                if (viewModel.ProductImageFiles != null)
                {
                    var productImages = new List<ProductImage>();
                    foreach (var image in viewModel.ProductImageFiles)
                    {
                        var fileName = await CopyImageAsync(image);
                        productImages.Add(new ProductImage { ImageName = fileName, ProductId = product.Id });
                    }

                    await _unitOfWork.ProducImages.AddRangeAsync(productImages);
                    await _unitOfWork.SaveAsync();
                }

                if (viewModel.SliderImageFile != null)
                {
                    var fileName = await CopyImageAsync(viewModel.SliderImageFile);
                    var productImage = new ProductImage { ImageName = fileName, ProductId = product.Id, ShowInSlider = true };
                    await _unitOfWork.ProducImages.AddAsync(productImage);
                    await _unitOfWork.SaveAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "اضافه کردن محصول جدید";
            var designGroups = new List<SelectListItem>();
            var groups = await _unitOfWork.DesignGroups.GetAllAsync();
            foreach (var group in groups)
                designGroups.Add(new SelectListItem { Text = group.Title, Value = group.Id.ToString() });

            ViewBag.DesignGroups = designGroups;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _unitOfWork.Products.FindAsync(id.Value);
            if (product == null)
                return NotFound();

            ViewBag.Title = "ویرایش محصول";
            var designGroups = new List<SelectListItem>();
            var groups = await _unitOfWork.DesignGroups.GetAllAsync();
            foreach (var group in groups)
                designGroups.Add(new SelectListItem { Text = group.Title, Value = group.Id.ToString(), Selected = product.DesignGroupId == group.Id });

            ViewBag.DesignGroups = designGroups;
            var viewModel = _mapper.Map<UpsertProductVM>(product);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpsertProductVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(viewModel);
                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "ویرایش محصول";
            var designGroups = new List<SelectListItem>();
            var groups = await _unitOfWork.DesignGroups.GetAllAsync();
            foreach (var group in groups)
                designGroups.Add(new SelectListItem { Text = group.Title, Value = group.Id.ToString() });

            ViewBag.DesignGroups = designGroups;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _unitOfWork.Products.GetFirstOrDefaultAsync(p => p.Id == id.Value, includeProperties: new string[] { "ProductImages" });
            if (product == null)
                return NotFound();

            if (product.ProductImages.Count > 0)
            {
                foreach (var productImage in product.ProductImages)
                {
                    if (!string.IsNullOrEmpty(productImage.ImageName))
                        DeleteImage(productImage.ImageName);
                }

                _unitOfWork.ProducImages.DeleteRange(product.ProductImages);
            }

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("[area]/[controller]/{productId}/{productTitle}/[action]")]
        public async Task<IActionResult> ProductImages(int? productId)
        {
            if (productId == null)
                return BadRequest();

            var product = await _unitOfWork.Products.GetFirstOrDefaultAsync(p => p.Id == productId.Value, includeProperties: new string[] { "ProductImages", "ProductImages.Product" });
            if (product == null)
                return NotFound();

            ViewBag.Title = $"لیست تصاویر محصول {product.Title}";
            var viewModel = _mapper.Map<GetProductVM>(product);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductImage(AddProductImageVM viewModel)
        {
            if (viewModel.ProductImages != null)
            {
                var productImages = new List<ProductImage>();
                foreach (var image in viewModel.ProductImages)
                {
                    var productImage = _mapper.Map<ProductImage>(viewModel);
                    productImage.ImageName = await CopyImageAsync(image);
                    productImages.Add(productImage);
                }

                await _unitOfWork.ProducImages.AddRangeAsync(productImages);
                await _unitOfWork.SaveAsync();
            }

            if (viewModel.SliderImage != null)
            {
                var productImage = _mapper.Map<ProductImage>(viewModel);
                productImage.ImageName = await CopyImageAsync(viewModel.SliderImage);
                productImage.ShowInSlider = true;
                await _unitOfWork.ProducImages.AddAsync(productImage);
                await _unitOfWork.SaveAsync();
            }

            var product = await _unitOfWork.Products.FindAsync(viewModel.ProductId);
            return RedirectToAction(nameof(ProductImages), new { productId = product.Id, productTitle = product.Title });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProductImage(int? id)
        {
            if (id == null)
                return BadRequest();

            var productImage = await _unitOfWork.ProducImages.GetFirstOrDefaultAsync(pi => pi.Id == id.Value, includeProperties: new string[] { "Product" });
            if (productImage == null)
                return NotFound();

            if (!string.IsNullOrEmpty(productImage.ImageName))
                DeleteImage(productImage.ImageName);

            var product = productImage.Product;

            _unitOfWork.ProducImages.Delete(productImage);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(ProductImages), new { productId = product.Id, productTitle = product.Title });
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
                var products = await _unitOfWork.Products.GetAllAsync();
                foreach (var product in products)
                {
                    product.Discount = viewModel.Discount;

                    await _unitOfWork.Products.UpdateAsync(product);
                    await _unitOfWork.SaveAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "اعمال تخفیف روی تمام محصولات";

            return View(viewModel);
        }
    }
}