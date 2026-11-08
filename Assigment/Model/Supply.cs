using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment.Model
{
    public class Supply
    {
        [Key]
        public int SupplyId { get; set; }
        [Required]
        public int SupplierId { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("SupplierId")]
       public virtual Supplier Supplier { get; set; }
        public virtual ICollection<SupplyDetail> SupplyDetails { get; set; }


    }
}
