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
        public int SellAmount { get; set; }
        public double SellPriceTotal { get; set; }
        public bool DiscountApplied { get; set; }
        public DateTime SellDate { get; private set; }
        public ProductItem ProductID { get; set; }
        public SellItem() { SellDate = DateTime.Now; }
    }
}
