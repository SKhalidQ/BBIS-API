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
    [Route("api/ProductItems")]
    [ApiController]
    public class ProductItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProductItemsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ProductItems
        // This is to Read
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductItem>>> GetProductItems()
        {
            return await _context.ProductItems.ToListAsync();
        }

        // GET: api/ProductItems/5
        // This is to Read a specific data by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductItem>> GetProductItem(long id)
        {
            var productItem = await _context.ProductItems.FindAsync(id);

            if (productItem == null)
            {
                return NotFound("Product not found");
            }

            return productItem;
        }

        // PUT: api/ProductItems/5
        // This is to Update
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductItem(long id, ProductItem productItem)
        {
            if (id != productItem.ProductId)
            {
                return BadRequest("The requested product does not exist");
            }

            _context.Entry(productItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductIDExists(id))
                {
                    return NotFound("Product not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductItems
        // This is to Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProductItem>> PostProductItem(ProductItem productItem)
        {          
            if(ProductExists(productItem))
            {
                ModelState.AddModelError("Brand" + "Flavour", "This product already exists");
                return BadRequest("This product already exists");
            }
            else
            {
                _context.ProductItems.Add(productItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProductItem", new { id = productItem.ProductId }, productItem);
            }

        }

        // DELETE: api/ProductItems/5
        // This is to Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductItem>> DeleteProductItem(long id)
        {
            var productItem = await _context.ProductItems.FindAsync(id);
            if (productItem == null)
            {
                return NotFound();
            }

            _context.ProductItems.Remove(productItem);
            await _context.SaveChangesAsync();

            return productItem;
        }

        private bool ProductIDExists(long id)
        {
            return _context.ProductItems.Any(e => e.ProductId == id);
        }

        private bool ProductExists(ProductItem productItem)
        {
        
            return _context.ProductItems.Any(x => x.Brand == productItem.Brand)
                && _context.ProductItems.Any(y => y.Flavour == productItem.Flavour);
        }
    }
}
