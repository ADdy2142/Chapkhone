using Chapkhone.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Infrastructure
{
    public class MyControllerBase : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public string CustomerUserName
        {
            get
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                return userName;
            }
        }

        public string ImageRootPath => Path.Combine(_webHostEnvironment.WebRootPath, "images");

        public MyControllerBase(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> CopyImageAsync(IFormFile file)
        {
            var guid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            var fileName = guid + extension;
            var fullPath = Path.Combine(ImageRootPath, fileName);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
                return fileName;
            }
        }

        public void DeleteImage(string fileName)
        {
            var fullPath = Path.Combine(ImageRootPath, fileName);
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}