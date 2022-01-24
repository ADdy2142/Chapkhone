using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetSiteNotificationVM
    {
        public int Id { get; set; }
        public bool IsDefault { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpsertSiteNotificationVM
    {
        public int Id { get; set; }
        public string ImageUrlXL { get; set; }
        public string ImageUrlLG { get; set; }
        public string ImageUrlMD { get; set; }
        public string ImageUrlSM { get; set; }

        public IFormFile ImageXL { get; set; }
        public IFormFile ImageLG { get; set; }
        public IFormFile ImageMD { get; set; }
        public IFormFile ImageSM { get; set; }
    }
}