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
        [Range(1, 999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.00, 999.99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }

        [Required]
        public bool ContainerReturned { get; set; }

        [Required]
        [Range(0.01, 999.99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Payed { get; set; }

        [Required]
        public DateTime SellDate { get; private set; }

        public ProductItem Product { get; set; }

        public SellItem() { SellDate = DateTime.Now; }
    }

    public class SellGet
    {
        [Required]
        public long SellID { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public bool ContainerReturned { get; set; }
        public DateTime SellDate { get; private set; }
        public ProductGet Product { get; set; }
        public SellGet() { SellDate = DateTime.Now; }
    }

    public class SellSubtotal
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool ContainerReturned { get; set; }
    }
}