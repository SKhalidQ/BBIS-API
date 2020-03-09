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
        public string Brand { get; set; }
        public string Flavour { get; set; }
        public bool Alcoholic { get; set; }
        public string ContainerType { get; set; }
        public bool Returnable { get; set; }
        public int StockAmount { get; set; }
        public double SellPrice { get; set; }
        public int Discount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ProductItem() { }
    }
}
