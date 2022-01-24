using Chapkhone.DataAccess.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SiteNotification
    {
        public int Id { get; set; }
        public string ImageNameXL { get; set; }
        public string ImageNameLG { get; set; }
        public string ImageNameMD { get; set; }
        public string ImageNameSM { get; set; }
        public bool IsDefault { get; set; }

        public string ImageUrlXL => Urls.SiteUrl + "/images/" + ImageNameXL;
        public string ImageUrlLG => Urls.SiteUrl + "/images/" + ImageNameLG;
        public string ImageUrlMD => Urls.SiteUrl + "/images/" + ImageNameMD;
        public string ImageUrlSM => Urls.SiteUrl + "/images/" + ImageNameSM;
    }
}