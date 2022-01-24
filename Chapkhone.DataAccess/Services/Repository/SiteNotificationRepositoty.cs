using Chapkhone.DataAccess.Context;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Services.Repository
{
    public class SiteNotificationRepositoty : BaseRepository<SiteNotification>, ISiteNotificationRepository
    {
        private readonly ChapkhoneContext _context;

        public SiteNotificationRepositoty(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(SiteNotification siteNotification)
        {
            var siteNotificationInDb = await FindAsync(siteNotification.Id);
            if (siteNotificationInDb != null)
            {
                siteNotificationInDb.ImageNameXL = siteNotification.ImageNameXL;
                siteNotificationInDb.ImageNameLG = siteNotification.ImageNameLG;
                siteNotificationInDb.ImageNameMD = siteNotification.ImageNameMD;
                siteNotificationInDb.ImageNameSM = siteNotification.ImageNameSM;
            }
        }
    }
}