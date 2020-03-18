using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(300)]
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
        public double SellPrice { get; set; }
        
        [Required]
        public int Discount { get; set; }
        
        public ProductItem() { }
    }
}
