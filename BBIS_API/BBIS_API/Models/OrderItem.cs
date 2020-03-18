using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BBIS_API.Models
{
    public class OrderItem
    {
        [Key]
        public long OrderID { get; set; }
        
        [Required]
        [Range(0.01, 999.99)]
        public double WarehousePrice { get; set; }
        
        [Required]
        [Range(1, 9999)]
        public int StockAmount { get; set; }
        
        [Required]
        public DateTime OrderDate { get; private set; }
        
        public ProductItem ProductID { get; }
       
        public OrderItem() { OrderDate = DateTime.Now; }
    }
}
