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
        public double WarehousePrice { get; set; }
        public int StockAmount { get; set; }
        public DateTime OrderDate { get; private set; }
        public ProductItem ProductID { get; set; }
        public OrderItem() { OrderDate = DateTime.Now; }
    }
}
