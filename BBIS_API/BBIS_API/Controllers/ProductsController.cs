using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductsController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region HTTPPost AddProduct
        [HttpPost]
        [ActionName("AddProduct")]
        public async Task<ActionResult<ProductItem>> PostProductItem([FromBody]ProductItem productItem)
        {
            try
            {
                var foundProduct = await DbAccessClass.ProductExists(productItem, _context);

                if (foundProduct)
                    throw new Exception("Product already exists");

                if (productItem.Discount > 100 || productItem.Discount < 0)
                    throw new Exception("Percentage out of range");

                var newProduct = await DbAccessClass.AddProduct(productItem, _context, _mapper);

                return CreatedAtAction("GetProduct", new { id = newProduct.ProductID }, Ok("Product added successfully"));
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Product already exists" => StatusCode(422, new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
        }
        #endregion

        #region HTTPGet ListProducts & GetProduct
        [HttpGet]
        [ActionName("ListProducts")]
        public async Task<ActionResult<IEnumerable<ProductGet>>> GetProductItems()
        {
            try
            {
                return await DbAccessClass.ListProducts(_context, _mapper);
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResult(ex.Message));
            }
        }

        [HttpGet]
        [ActionName("GetProduct")]
        public async Task<ActionResult<ProductGet>> GetProductItem([FromBody]long productID)
        {
            try
            {
                var productItem = await DbAccessClass.GetOnlyProduct(productID, _context, _mapper);

                if (productItem == null)
                    throw new Exception("Product not found");

                return productItem;
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Product not found" => NotFound(new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
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
                    throw new Exception("Product not found");

                var product = await DbAccessClass.GetProduct(productUpdate.ProductID, _context);

                await DbAccessClass.UpdateProduct(productUpdate, product, _context);
                return Ok(new JsonResult("Product updated successfully"));
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Product not found" => NotFound(new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }

        }
        #endregion

        #region HTTPDelete EliminateProduct
        [HttpDelete]
        [ActionName("EliminateProduct")]
        public async Task<ActionResult<ProductItem>> DeleteProduct([FromBody]long productID)
        {
            try
            {
                var productItem = await DbAccessClass.ProductIDExists(productID, _context);

                if (!productItem)
                    throw new Exception("Product not found");

                var product = await DbAccessClass.GetProduct(productID, _context);
                await DbAccessClass.DeleteProduct(product, _context);

                return Ok(new JsonResult("Product deleted successfully"));
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Product not found" => NotFound(new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
        }
        #endregion

    }
}
