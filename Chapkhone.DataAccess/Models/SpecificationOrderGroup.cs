using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SpecificationOrderGroup
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public int DesignGroupId { get; set; }

        [ForeignKey(nameof(DesignGroupId))]
        public DesignGroup DesignGroup { get; set; }

        public ICollection<SpecificationOrderTitle> SpecificationOrderTitles { get; set; }
    }
}