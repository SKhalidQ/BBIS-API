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
    [Route("api/OrderItems")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OrderItemsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(long id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        // PUT: api/OrderItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(long id, OrderItem orderItem)
        {
            if (id != orderItem.OrderID)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderIDExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            if (OrderExists(orderItem))
            {
                ModelState.AddModelError("Order", "This order already exists");
                return BadRequest("This order already exists");
            }
            else
            {
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderID }, orderItem);
            }
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderItem>> DeleteOrderItem(long id)
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

        private bool OrderIDExists(long id)
        {
            return _context.OrderItems.Any(e => e.OrderID == id);
        }
        private bool OrderExists(OrderItem OrderItem)
        {
            return _context.OrderItems.Any(e => e.OrderDate == OrderItem.OrderDate);
        }

    }
}
