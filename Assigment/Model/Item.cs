using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment.Model
{
    public class Item
    {
        [Key]
        public int ItemId {  get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Details { get; set; }
        public double Price { get; set; }
        public string MasureingUnite { get; set; }

        public double ReOrederLevel {  get; set; }
        [NotMapped]

        public double StockValue { get; set; }

    }
}
