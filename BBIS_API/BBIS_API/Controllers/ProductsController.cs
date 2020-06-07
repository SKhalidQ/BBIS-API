﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BBIS_API.Models;
using System.Diagnostics;
using BBIS_API.DbAccess;

namespace BBIS_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProductsController(DatabaseContext context)
        {
            _context = context;
        }

        #region Main Menu
        [HttpGet]
        [ActionName("Status")]
        public string Hello()
        {
            HttpContext.Response.StatusCode = 200;
            return "API is Active";
        }
        #endregion

        #region HTTPPost AddProduct
        [HttpPost]
        [ActionName("AddProduct")]
        public async Task<ActionResult<ProductItem>> PostProductItem([FromBody]ProductItem productItem)
        {
            var foundProduct = await DbAccessClass.ProductExists(productItem, _context);

            if (foundProduct)
            {
                return StatusCode(400, "Already Exists");
            }

            var newProduct = await DbAccessClass.AddProduct(productItem, _context);

            return CreatedAtAction("GetProduct", new { id = newProduct.ProductId }, "Done");
        }
        #endregion

        #region HTTPGet ListProducts & GetProduct
        [HttpGet]
        [ActionName("ListProducts")]
        public async Task<ActionResult<IEnumerable<ProductItem>>> GetProductItems()
        {
            return await _context.ProductItems.ToListAsync();
        }

        [HttpGet]
        [ActionName("GetProduct")]
        public async Task<ActionResult<ProductItem>> GetProductItem([FromBody]long id)
        {
            var productItem = await _context.ProductItems.FindAsync(id);

            //return (productItem == null) ? productItem : productItem; //Tenary does not return NotFound();

            if (productItem == null)
            {
                return NotFound("Product Does Not Exist");
            }

            return productItem;
        }

        #endregion

        #region HTTPPut Update Information
        [HttpPut]
        [ActionName("UpdateInfo")]
        public async Task<IActionResult> PutProductItem([FromBody]ProductUpdate productUpdate)
        {
            try
            {
                var product = await DbAccessClass.GetProduct(productUpdate.ProductId, _context);
                
                await DbAccessClass.UpdateProduct(productUpdate, product, _context);
                return StatusCode(200, "Done");
            }
            catch (Exception e)
            {
                if (!await DbAccessClass.ProductIDExists(productUpdate.ProductId, _context))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }

        }
        #endregion

        #region HTTPDelete EliminateProduct
        [HttpDelete]
        [ActionName("EliminateProduct")]
        public async Task<ActionResult<ProductItem>> DeleteProduct([FromBody]long productID)
        {
            var productItem = await DbAccessClass.ProductIDExists(productID, _context);
            
            if (!productItem)
                return BadRequest("Not done");

            var product = await DbAccessClass.GetProduct(productID, _context);
            DbAccessClass.DeleteProduct(product, _context);

            return Ok("Done");
        }
        #endregion

    }
}
