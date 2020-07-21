using AutoMapper;
using BBIS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                _context.Database.EnsureCreated();

                var isConected = await _context.Database.CanConnectAsync();
                var createAdmin = await AddAdminUser(_context);

                return (isConected) ? true : throw new Exception();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> ClearDatabase(DatabaseContext _context)
        {
            try
            {
                var deleteDatabase = await _context.Database.EnsureDeletedAsync();
                var createDatabase = await _context.Database.EnsureCreatedAsync();
                var isConnected = await _context.Database.CanConnectAsync();

                var createAdmin = await AddAdminUser(_context);

                return (deleteDatabase && createDatabase && isConnected && createAdmin) ? true : throw new Exception();
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Product
        public static async Task<ProductItem> AddProduct(ProductItem productItem, DatabaseContext _context, IMapper _mapper)
        {
            _context.ProductItems.Add(productItem);
            await _context.SaveChangesAsync();

            return productItem;
        }

        public static async Task<ProductItem> GetProduct(long productID, DatabaseContext _context)
        {
            return await _context.ProductItems.FindAsync(productID);
        }

        public static async Task<ProductGet> GetOnlyProduct(long productID, DatabaseContext _context, IMapper _mapper)
        {
            var product = await _context.ProductItems.FindAsync(productID);

            return _mapper.Map<ProductGet>(product);
        }

        public static async Task<ActionResult<IEnumerable<ProductGet>>> ListProducts(DatabaseContext _context, IMapper _mapper)
        {
            return await _context.ProductItems.Select(x => _mapper.Map<ProductGet>(x)).ToListAsync();
        }

        public static async Task UpdateProduct(ProductUpdate updatedProduct, ProductItem productItem, DatabaseContext _context)
        {
            if (updatedProduct.SellPrice != -1)
                productItem.SellPrice = updatedProduct.SellPrice;
            if (updatedProduct.Discount != -1)
                productItem.Discount = updatedProduct.Discount;

            productItem.Returnable = updatedProduct.Returnable;

            _context.Entry(productItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteProduct(ProductItem productItem, DatabaseContext _context)
        {

            _context.RemoveRange(_context.OrderItems.Where(x => x.Product.ProductID == productItem.ProductID));
            _context.RemoveRange(_context.SellItems.Where(x => x.Product.ProductID == productItem.ProductID));

            _context.ProductItems.Remove(productItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public static async Task<bool> ProductIDExists(long productID, DatabaseContext _context)
        {
            var product = await _context.ProductItems.AnyAsync(e => e.ProductID == productID);

            return (product) ? true : false;
        }

        public static async Task<bool> ProductExists(ProductItem product, DatabaseContext _context)
        {
            return await _context.ProductItems.AnyAsync(x => x.Brand.ToLower() == product.Brand.ToLower()
                                                          && x.Flavour.ToLower() == product.Flavour.ToLower()
                                                          && x.ContainerType.ToLower() == product.ContainerType.ToLower());
        }
        #endregion

        #region Order
        public static async Task AddOrder(OrderItem orderItem, ProductItem product, DatabaseContext _context)
        {
            product.OrdersList.Add(new OrderItem
            {
                QuantityOrdered = orderItem.QuantityOrdered,
                TotalCost = orderItem.TotalCost
            });

            product.StockAmount += orderItem.QuantityOrdered;
            await _context.SaveChangesAsync();
        }

        public static async Task<OrderItem> GetOrder(long orderID, DatabaseContext _context)
        {
            return await _context.OrderItems.Include(x => x.Product)
                                            .FirstOrDefaultAsync(x => x.OrderID == orderID);
        }

        public static async Task<IEnumerable<OrderItem>> ListOrders(DatabaseContext _context)
        {
            return await _context.OrderItems.Include(x => x.Product).ToListAsync();
        }

        public static async Task UpdateOrder(OrderUpdate updatedOrder, OrderItem orderItem, DatabaseContext _context)
        {
            if (updatedOrder.TotalCost != -1)
                orderItem.TotalCost = updatedOrder.TotalCost;

            if (updatedOrder.QuantityOrdered != -1)
            {
                orderItem.Product.StockAmount -= orderItem.QuantityOrdered;
                orderItem.Product.StockAmount += updatedOrder.QuantityOrdered;

                orderItem.QuantityOrdered = updatedOrder.QuantityOrdered;
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteOrder(OrderItem orderItem, DatabaseContext _context)
        {
            try
            {
                if (orderItem.Product.StockAmount - orderItem.QuantityOrdered < 0)
                    throw new Exception("Stock amount drops below 0");

                orderItem.Product.StockAmount -= orderItem.QuantityOrdered;

                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> OrderIDExists(long orderID, DatabaseContext _context)
        {
            var order = await _context.OrderItems.AnyAsync(e => e.OrderID == orderID);

            return (order) ? true : false;
        }

        public static async Task<bool> OrderExists(long productID, DatabaseContext _context)
        {
            return await _context.OrderItems.AnyAsync(x => x.Product.ProductID == productID
                                                        && x.OrderDate/*.Date*/ == DateTime.Now.Date);
        }
        #endregion

        #region Sell
        public static async Task AddSell(SellItem sellItem, ProductItem product, DatabaseContext _context)
        {
            product.SalesList.Add(new SellItem
            {
                Quantity = sellItem.Quantity,
                TotalCost = sellItem.TotalCost,
                Paid = sellItem.Paid,
                ContainerReturned = sellItem.ContainerReturned
            });

            product.StockAmount -= sellItem.Quantity;
            await _context.SaveChangesAsync();
        }

        public static async Task<SellItem> GetSell(long sellID, DatabaseContext _context)
        {
            return await _context.SellItems.Include(x => x.Product)
                               .FirstOrDefaultAsync(x => x.SellID == sellID);
        }

        public static async Task<IEnumerable<SellItem>> ListSell(DatabaseContext _context)
        {
            return await _context.SellItems.Include(x => x.Product).ToListAsync();
        }

        public static async Task<bool> SellIDExists(long sellID, DatabaseContext _context)
        {
            return (await _context.SellItems.AnyAsync(e => e.SellID == sellID)) ? true : false;
        }

        public static async Task<bool> DeleteSell(SellItem sellItem, DatabaseContext _context)
        {
            try
            {
                sellItem.Product.StockAmount += sellItem.Quantity;

                _context.SellItems.Remove(sellItem);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static async Task UpdateSell(SellUpdate updateSell, SellItem sellItem, DatabaseContext _context)
        {
            if (updateSell.Quantity != -1)
                sellItem.Quantity = updateSell.Quantity;
            if (updateSell.TotalCost != -1)
                sellItem.TotalCost = updateSell.TotalCost;

            sellItem.ContainerReturned = updateSell.ContainerReturned;

            _context.Entry(sellItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public static decimal CalculateSubtotal(decimal subtotal, int discount)
        {
            subtotal *= (1 - ((decimal)discount / 100));

            return Math.Round(subtotal, 2);
        }
        #endregion

        #region User
        public static async Task<bool> VerifyUser(string username, string password, DatabaseContext _context)
        {
            try
            {
                IQueryable<User> userQuery = from user
                                             in _context.UserItems
                                             where user.Username == username
                                             && user.Password == password
                                             select user;

                User selectedUser = await userQuery.FirstOrDefaultAsync();

                return (selectedUser == null) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            //await Task.WhenAll();
            //return false;
        }

        public static async Task<bool> UserExists(string username, DatabaseContext _context)
        {
            try
            {
                IQueryable<User> userQuery = from user
                                             in _context.UserItems
                                             where user.Username.ToLower() == username.ToLower()
                                             select user;

                User selectedUser = await userQuery.FirstOrDefaultAsync();

                return (selectedUser == null) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            //await Task.WhenAll();
            //return false;
        }

        private static async Task<bool> AddAdminUser(DatabaseContext _context)
        {
            try
            {
                User User = new User()
                {
                    UserID = new Guid(),
                    Username = "Admin",
                    Password = "@dm1n"
                };

                var userExists = await UserExists(User.Username, _context);

                if (!userExists)
                {
                    await _context.UserItems.AddAsync(User);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<User> GetUser(string username, DatabaseContext _context)
        {
            try
            {
                IQueryable<User> userQuery = from User
                                             in _context.UserItems
                                             where User.Username == username
                                             select User;

                User selectedUser = await userQuery.FirstOrDefaultAsync();

                return selectedUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> ChangePassword(User User, string newPassword, DatabaseContext _context)
        {
            try
            {
                User.Password = newPassword;

                _context.Entry(User).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


    }
}
