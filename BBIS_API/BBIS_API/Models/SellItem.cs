using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBIS_API.Models
{
    public class SellItem
    {
        [Key]
        public long SellID { get; set; }

        [Required]
        [Range(0.01, 999.99)]
        public int QuantitySold { get; set; }

        [Required]
        [Range(0.01, 999.99)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }

        [Required]
        public bool DiscountApplied { get; set; }

        [Required]
        public DateTime SellDate { get; private set; }

        public ProductItem Product { get; set; }

        public SellItem() { SellDate = DateTime.Now; }
    }

    public class SellGet
    {
        public long SellID { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalCost { get; set; }
        public bool DiscountApplied { get; set; }
        public DateTime SellDate { get; private set; }
        public ProductGet Product { get; set; }
        public SellGet() { SellDate = DateTime.Now; }

    }
}