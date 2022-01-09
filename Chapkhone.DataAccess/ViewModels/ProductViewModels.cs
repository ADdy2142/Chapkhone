using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetProductViewModel
    {
        public int Id { get; set; }
        public int DesignGroupId { get; set; }
        public string Title { get; set; }
        public string DesignGroupTitle { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string ImageUrl { get; set; }
        public double FinalPrice { get; set; }
    }
}