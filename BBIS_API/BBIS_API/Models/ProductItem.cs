using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BBIS_API.Models
{
    public class ProductItem
    {
        [Key]
        public long ProductId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Value for {0} has a max limit of {1} characters.")]
        public string Brand { get; set; }
        
        [Required]
        [StringLength(60, ErrorMessage = "Value for {0} has a max limit of {1} characters.")]
        public string Flavour { get; set; }
        
        [Required]
        public bool Alcoholic { get; set; }
        
        [Required]
        [StringLength(10, ErrorMessage = "Value for {0} has a max limit of {1} characters.")]
        public string ContainerType { get; set; }
        
        [Required]
        public bool Returnable { get; set; }
        
        [Range(0, 999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int StockAmount { get; set; }
        
        [Required]
        [Range(0.01, 999.99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellPrice { get; set; }
        
        [Required]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Discount { get; set; }

        public ICollection<OrderItem> OrdersList { get; set; } = new List<OrderItem>();

        public ICollection<SellItem> SalesList { get; set; } = new List<SellItem>();

    }

    public class ProductGet
    {
        public long ProductId { get; set; }
        public string Brand { get; set; }
        public string Flavour { get; set; }
        public bool Alcoholic { get; set; }
        public string ContainerType { get; set; }
        public bool Returnable { get; set; }
        public int StockAmount { get; set; }
        public decimal SellPrice { get; set; }
        public int Discount { get; set; }
    }

    public class ProductUpdate
    {
        public long ProductId { get; set; }
        public bool Returnable { get; set; }

        [Range(-1, 999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int StockAmount { get; set; }

        [Range(-1, 999.99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal SellPrice { get; set; }

        [Range(-1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Discount { get; set; }
    }
}

