using BBIS_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBIS_API.DbAccess
{
    public static class DbAccessClass
    {
        #region Product
        public static async Task<bool> ProductIDExists(long productID, DatabaseContext _context)
        {
            var product = await _context.ProductItems.AnyAsync(e => e.ProductId == productID);

            return (product) ? true : false;
        }

        public static async Task<bool> ProductExists(ProductItem product, DatabaseContext _context)
        {
            IQueryable<ProductItem> ProductQuery = from Product
                                                   in _context.ProductItems
                                                   where Product.Brand == product.Brand
                                                   && Product.Flavour == product.Flavour
                                                   && Product.ContainerType == product.ContainerType
                                                   select Product;
            if (ProductQuery != null)
            {
                ProductItem selectedProduct = await ProductQuery.FirstOrDefaultAsync();

                if (selectedProduct == null)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public static async Task<ProductItem> GetProduct(long productID, DatabaseContext _context)
        {
            var product = await _context.ProductItems.FindAsync(productID);

            return product;
        }

        public static async Task<ProductItem> AddProduct(ProductItem productItem, DatabaseContext _context)
        {
            _context.ProductItems.Add(productItem);
            await _context.SaveChangesAsync();

            return productItem;
        }

        public static async Task UpdateProduct(ProductItem productItem, DatabaseContext _context)
        {
            _context.Entry(productItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteProduct(ProductItem productItem, DatabaseContext _context)
        {
            _context.ProductItems.Remove(productItem);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Order
        public static async Task<bool> OrderIDExists(long orderID, DatabaseContext _context)
        {
            var order = await _context.OrderItems.AnyAsync(e => e.OrderID == orderID);

            return (order) ? true : false;
        }

        public static async Task<bool> OrderExists(OrderItem orderItem, DatabaseContext _context)
        {

            //If the product is the same,

            //and if the order date is the same day/month/year,

            //return true,

            //else return false;
            return false;
        }

        public static async Task<OrderItem> GetOrder(long orderID, DatabaseContext _context)
        {
            var order = await _context.OrderItems.FindAsync(orderID);

            return order;
        }

        public static async Task AddOrder(OrderItem orderItem, ProductItem product, DatabaseContext _context)
        {
            OrderItem newOrder = new OrderItem
            {
                StockAmount = orderItem.StockAmount,
                WarehousePrice = orderItem.WarehousePrice,
                Product = product
            };

            _context.OrderItems.AddAsync(newOrder);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Sell
        public static async Task<bool> SellIDExists(long sellID, DatabaseContext _context)
        {
            var sell = await _context.SellItems.AnyAsync(e => e.SellID == sellID);

            return (sell) ? true : false;
        }

        public static async Task<SellItem> GetSell(long sellID, DatabaseContext _context)
        {
            var sell = await _context.SellItems.FindAsync(sellID);

            return sell;
        }
        #endregion
    }
}
