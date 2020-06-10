using BBIS_API.Models;
using Microsoft.AspNetCore.Mvc;
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
        #region APIStatus Check

        public static async Task<bool> DatabaseCheck(DatabaseContext _context)
        {
            _context.Database.EnsureCreated();

            var isConected = await _context.Database.CanConnectAsync();

            return (isConected) ? true : false;
        }

        #endregion

        #region Product
        public static async Task<ProductItem> AddProduct(ProductItem productItem, DatabaseContext _context)
        {
            _context.ProductItems.Add(productItem);
            await _context.SaveChangesAsync();

            return productItem;
        }
        
        public static async Task<ProductItem> GetProduct(long productID, DatabaseContext _context)
        {
            return await _context.ProductItems.FindAsync(productID);
        }

        public static async Task<ProductGet> GetOnlyProduct(long productID, DatabaseContext _context)
        {
            var product = await _context.ProductItems.FindAsync(productID);

            return GetSecretProduct(product);
        }

        public static async Task<ActionResult<IEnumerable<ProductGet>>> ListProducts(DatabaseContext _context)
        {
            return await _context.ProductItems.Select(x => GetSecretProduct(x)).ToListAsync();
        }

        private static ProductGet GetSecretProduct(ProductItem productItem) => new ProductGet
        {
            ProductId = productItem.ProductId,
            Brand = productItem.Brand,
            Flavour = productItem.Flavour,
            Alcoholic = productItem.Alcoholic,
            ContainerType = productItem.ContainerType,
            Returnable = productItem.Returnable,
            StockAmount = productItem.StockAmount,
            SellPrice = productItem.SellPrice,
            Discount = productItem.Discount
        };

        public static async Task UpdateProduct(ProductUpdate updatedProduct, ProductItem productItem, DatabaseContext _context)
        {
            if (updatedProduct.SellPrice != -1)
                productItem.SellPrice = updatedProduct.SellPrice;
            if (updatedProduct.Discount != -1)
                productItem.Discount = updatedProduct.Discount;
            if (updatedProduct.StockAmount != -1)
                productItem.StockAmount = updatedProduct.StockAmount;

            productItem.Returnable = updatedProduct.Returnable;

            _context.Entry(productItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
        
        public static async Task<bool> DeleteProduct(ProductItem productItem, DatabaseContext _context)
        {
            _context.ProductItems.Remove(productItem);
            await _context.SaveChangesAsync();

            return true;
        }
        
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

                return (selectedProduct == null) ? false : true;
            }

            return false;
        }
        
        #endregion

        #region Order
        public static async Task AddOrder(OrderItem orderItem, ProductItem product, DatabaseContext _context)
        {
            product.OrdersList.Add(new OrderItem
            {
                StockAmount = orderItem.StockAmount,
                WarehousePrice = orderItem.WarehousePrice
            });

            product.StockAmount += orderItem.StockAmount;
            await _context.SaveChangesAsync();
        }

        public static async Task<OrderItem> GetOrder(long orderID, DatabaseContext _context)
        {
            return await _context.OrderItems.Include(x => GetSecretProduct(x.Product)).FirstOrDefaultAsync(x => x.OrderID == orderID);
        }

        public static async Task<ActionResult<IEnumerable<OrderItem>>> ListOrders(DatabaseContext _context)
        {
            return await _context.OrderItems.Include(x => GetSecretProduct(x.Product)).ToListAsync();

            //return await _context.OrderItems.Include(x => x.Product).ToListAsync();
        }

        public static async Task UpdateOrder(OrderItem orderItem, DatabaseContext _context)
        {
            _context.Entry(orderItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteOrder(OrderItem orderItem, DatabaseContext _context)
        {
            var orderWithProduct = await _context.OrderItems.Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderID == orderItem.OrderID);

            orderWithProduct.Product.StockAmount -= orderItem.StockAmount;

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public static async Task<bool> OrderIDExists(long orderID, DatabaseContext _context)
        {
            var order = await _context.OrderItems.AnyAsync(e => e.OrderID == orderID);

            return (order) ? true : false;
        }

        public static async Task<bool> OrderExists(OrderItem orderItem, DatabaseContext _context)
        {
            IQueryable<OrderItem> OrderQuery = from Order
                                               in _context.OrderItems
                                               where Order.Product.ProductId == orderItem.Product.ProductId
                                               && Order.OrderDate == orderItem.OrderDate
                                               select Order;

            if (OrderQuery != null)
            {
                OrderItem SelectedOrder = await OrderQuery.FirstOrDefaultAsync();

                return (SelectedOrder == null) ? false : true;
            }

            return false;
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
