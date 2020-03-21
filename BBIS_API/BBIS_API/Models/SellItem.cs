using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BBIS_API.Models
{
    public class SellItem
    {
        [Key]
        public long SellID { get; set; }
        
        [Required]
        [Range(0.01, 999.99)]
        public int SellAmount { get; set; }
        
        [Required]
        [Range(0.01, 999.99)]
        public double SellPriceTotal { get; set; }
        
        [Required]
        public bool DiscountApplied { get; set; }
        
        [Required]
        public DateTime SellDate { get; private set; }

        public long ProductID { get; set; }
        
        public ProductItem ProductObj { get; set; }

        public SellItem() { SellDate = DateTime.Now; }
    }
}
