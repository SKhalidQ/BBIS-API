using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal WarehousePrice { get; set; }
        
        [Required]
        [Range(1, 9999)]
        public int StockAmount { get; set; }
        
        [Required]
        public DateTime OrderDate { get; private set; }

        public virtual ProductItem ProductObj { get; set; }

        public OrderItem() { OrderDate = DateTime.Now.Date; }
    }
}
