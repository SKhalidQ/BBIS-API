using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBIS_API.Models
{
    public class OrderItem
    {
        [Key]
        public long OrderID { get; set; }

        [Required]
        [Range(0.01, 999.99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }

        [Required]
        [Range(1, 999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int QuantityOrdered { get; set; }

        [Required]
        public DateTime OrderDate { get; private set; }

        public ProductItem Product { get; set; }

        public OrderItem() { OrderDate = DateTime.Now; }
    }

    public class OrderGet
    {
        [Required]
        public long OrderID { get; set; }
        public decimal TotalCost { get; set; }
        public int QuantityOrdered { get; set; }
        public DateTime OrderDate { get; set; }
        public ProductGet Product { get; set; }
        public OrderGet() { OrderDate = DateTime.Now.Date; }
    }

    public class OrderUpdate
    {
        [Required]
        public long OrderID { get; set; }
        public decimal TotalCost { get; set; }
        public int QuantityOrdered { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderUpdate() { OrderDate = DateTime.Now.Date; }
    }
}
