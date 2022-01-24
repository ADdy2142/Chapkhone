using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SpecificationOrderTitle
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string ShortDescription { get; set; }

        public SpecificationOrderTitleSize DesktopSize { get; set; } = SpecificationOrderTitleSize.Persent33;
        public SpecificationOrderTitleSize TabletSize { get; set; } = SpecificationOrderTitleSize.Persent50;
        public int SpecificationOrderGroupId { get; set; }

        [ForeignKey(nameof(SpecificationOrderGroupId))]
        public SpecificationOrderGroup SpecificationOrderGroup { get; set; }

        public ICollection<SpecificationOrderValue> SpecificationOrderValues { get; set; }

        public string DesktopSizeCssClass
        {
            get
            {
                string size = string.Empty;
                switch (DesktopSize)
                {
                    case SpecificationOrderTitleSize.Persent25:
                        size = "col-lg-3";
                        break;

                    case SpecificationOrderTitleSize.Persent33:
                        size = "col-lg-4";
                        break;

                    case SpecificationOrderTitleSize.Persent50:
                        size = "col-lg-6";
                        break;

                    case SpecificationOrderTitleSize.persent100:
                        size = "col-lg-12";
                        break;

                    default:
                        break;
                }

                return size;
            }
        }

        public string TabletSizeCssClass
        {
            get
            {
                string size = string.Empty;
                switch (TabletSize)
                {
                    case SpecificationOrderTitleSize.Persent25:
                        size = "col-md-3";
                        break;

                    case SpecificationOrderTitleSize.Persent33:
                        size = "col-md-4";
                        break;

                    case SpecificationOrderTitleSize.Persent50:
                        size = "col-md-6";
                        break;

                    case SpecificationOrderTitleSize.persent100:
                        size = "col-md-12";
                        break;

                    default:
                        break;
                }

                return size;
            }
        }
    }
}