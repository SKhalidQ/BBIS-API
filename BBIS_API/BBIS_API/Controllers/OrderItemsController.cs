using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BBIS_API.Models;

namespace BBIS_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OrderItemsController(DatabaseContext context)
        {
            _context = context;
        }

        #region HTTPGet
        [HttpGet]
        [ActionName("GetOrder")]
        public async Task<ActionResult<OrderItem>> GetOrderItem([FromBody]long id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        [HttpGet]
        [ActionName("ListOrders")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }
        #endregion

        #region HTTPPut
        [HttpPut]
        [ActionName("UpdateOrder")]
        public async Task<IActionResult> PutOrderItem(/*[FromBody]long id,*/ OrderItem orderItem)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderIDExists(orderItem.Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(200, "Done");

            #region Old
            //if (id != orderItem.OrderID)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(orderItem).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!OrderIDExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            #endregion
        }
        #endregion

        #region HTTPPost
        [HttpPost]
        [ActionName("AddOrder")]
        public async Task<ActionResult<OrderItem>> PostOrderItem([FromBody]OrderItem orderItem, [FromQuery] long ProductId)
        {
            var Product = ProductExists(ProductId);

            if (Product)
            {
                var Order = OrderExists(orderItem);
                var GetProduct = _context.ProductItems.Find(ProductId);

                if (Order)
                {
                    ModelState.AddModelError("Order", "This order already exists");
                    return BadRequest("This order already exists");
                }

                OrderItem newOrder = new OrderItem
                {
                    StockAmount = orderItem.StockAmount,
                    WarehousePrice = orderItem.WarehousePrice,
                    Product = GetProduct
                };

                _context.OrderItems.Add(newOrder);
                await _context.SaveChangesAsync();

                //return CreatedAtAction("GetOrderItem", new { id = ProductId }, orderItem);
                return Ok(newOrder);
            }
                return BadRequest();
        }
        #endregion

        #region HTTPDelete
        [HttpDelete]
        [ActionName("EliminateOrder")]
        public async Task<ActionResult<OrderItem>> DeleteOrderItem([FromBody]long id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return orderItem;
        }
        #endregion

        #region Check if Exist
        private bool OrderIDExists(long id)
        {
            return _context.OrderItems.Any(e => e.OrderID == id);
        }
        private bool OrderExists(OrderItem OrderItem)
        {
            return _context.OrderItems.Any(e => e.OrderDate == OrderItem.OrderDate);
        }
        private bool ProductExists(long id)
        {
            return _context.ProductItems.Any(e => e.ProductId == id);
        }
        #endregion

    }
}
