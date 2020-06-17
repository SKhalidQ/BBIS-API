using BBIS_API.DbAccess;
using BBIS_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        #region HTTPPost AddProduct
        [HttpPost]
        [ActionName("AddProduct")]
        public async Task<ActionResult<ProductItem>> PostProductItem([FromBody]ProductItem productItem)
        {
            var foundProduct = await DbAccessClass.ProductExists(productItem, _context);

            if (foundProduct)
                return StatusCode(400, "Not done");

            var newProduct = await DbAccessClass.AddProduct(productItem, _context);

            return CreatedAtAction("GetProduct", new { id = newProduct.ProductID }, Ok("Done"));
        }
        #endregion

        #region HTTPGet ListProducts & GetProduct
        [HttpGet]
        [ActionName("ListProducts")]
        public async Task<ActionResult<IEnumerable<ProductGet>>> GetProductItems()
        {
            return await DbAccessClass.ListProducts(_context);
        }

        [HttpGet]
        [ActionName("GetProduct")]
        public async Task<ActionResult<ProductGet>> GetProductItem([FromBody]long productID)
        {
            var productItem = await DbAccessClass.GetOnlyProduct(productID, _context);

            if (productItem == null)
                return NotFound("Product Does Not Exist");

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
                var productExists = await DbAccessClass.ProductIDExists(productUpdate.ProductID, _context);

                if (!productExists)
                    throw new Exception("Not done");

                var product = await DbAccessClass.GetProduct(productUpdate.ProductID, _context);

                await DbAccessClass.UpdateProduct(productUpdate, product, _context);
                return Ok("Done");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
            await DbAccessClass.DeleteProduct(product, _context);

            return Ok("Done");
        }
        #endregion

    }
}
