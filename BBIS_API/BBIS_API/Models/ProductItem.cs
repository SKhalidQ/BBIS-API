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
        [StringLength(50)]
        public string Brand { get; set; }
        
        [Required]
        [StringLength(60)]
        public string Flavour { get; set; }
        
        [Required]
        public bool Alcoholic { get; set; }
        
        [Required]
        [StringLength(10)]
        public string ContainerType { get; set; }
        
        [Required]
        public bool Returnable { get; set; }
        
        [Range(0, 999)]
        public int StockAmount { get; set; }
        
        [Required]
        [Range(0.01, 999.99)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellPrice { get; set; }
        
        [Required]
        [Range(0, 100)]
        public int Discount { get; set; }

        public virtual ICollection<OrderItem> OrdersList { get; set; }
        
        public virtual ICollection<SellItem> SalesList { get; set; }

    }

    public class ProductUpdate
    {
        public long ProductId { get; set; }
        public bool Returnable { get; set; }
        public int StockAmount { get; set; }
        public decimal SellPrice { get; set; }
        public int Discount { get; set; }
    }
}

