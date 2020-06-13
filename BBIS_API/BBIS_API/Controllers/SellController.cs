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
    [Route("api/[controller]")]
    [ApiController]
    public class SellController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public SellController(DatabaseContext context)
        {
            _context = context;
        }

        #region HTTPPost Add Sell

        #endregion

        #region HTTPGet ListSells & GetSells
        [HttpGet]
        [ActionName("ListSells")]
        public async Task<ActionResult<IEnumerable<SellItem>>> ListSells()
        {
            return await _context.SellItems.ToListAsync();
        }

        #endregion

        #region HTTPPut Update Sell

        #endregion

        #region HTTPDelete Delete Sell

        #endregion

        // GET: api/Sell/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SellItem>> GetSellItem(long id)
        {
            var sellItem = await _context.SellItems.FindAsync(id);

            if (sellItem == null)
            {
                return NotFound();
            }

            return sellItem;
        }

        // PUT: api/Sell/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSellItem(long id, SellItem sellItem)
        {
            if (id != sellItem.SellID)
            {
                return BadRequest();
            }

            _context.Entry(sellItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellItemExists(id))
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

        // POST: api/Sell
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SellItem>> PostSellItem(SellItem sellItem)
        {
            _context.SellItems.Add(sellItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSellItem", new { id = sellItem.SellID }, sellItem);
        }

        // DELETE: api/Sell/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SellItem>> DeleteSellItem(long id)
        {
            var sellItem = await _context.SellItems.FindAsync(id);
            if (sellItem == null)
            {
                return NotFound();
            }

            _context.SellItems.Remove(sellItem);
            await _context.SaveChangesAsync();

            return sellItem;
        }

        private bool SellItemExists(long id)
        {
            return _context.SellItems.Any(e => e.SellID == id);
        }
    }
}
