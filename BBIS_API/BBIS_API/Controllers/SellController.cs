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
    public class SellController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public SellController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region HTTPPost Add Sell
        [HttpPost]
        [ActionName("AddSell")]
        public async Task<ActionResult<SellItem>> PostSellItem([FromBody]SellItem sellItem, [FromQuery]long productID)
        {
            try
            {
                var productExists = await DbAccessClass.ProductIDExists(productID, _context);
                var product = await DbAccessClass.GetProduct(productID, _context);

                if (!productExists)
                    throw new Exception("Product not found");

                var availableStock = product.StockAmount - sellItem.Quantity;

                if (availableStock < 0)
                    throw new Exception("Not enough quantity available in stock");

                var verifyPrice = product.SellPrice * sellItem.Quantity;
                var verifyWithDiscount = DbAccessClass.CalculateSubtotal(verifyPrice, product.Discount);

                if (sellItem.ContainerReturned && verifyWithDiscount != sellItem.TotalCost)
                    throw new Exception("Error in total");
                else if (!sellItem.ContainerReturned && verifyPrice != sellItem.TotalCost)
                    throw new Exception("Error in total");

                var change = sellItem.Payed - sellItem.TotalCost;

                if (change < 0)
                    throw new Exception("Not enough payment");

                await DbAccessClass.AddSell(sellItem, product, _context);

                return CreatedAtAction("GetSell", new { id = sellItem.SellID }, new JsonResult("Subtotal: £" + change));
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Product not found" => NotFound(new JsonResult(ex.Message)),
                    "Not enough quantity available in stock" => StatusCode(417, new JsonResult(ex.Message)),
                    "Error in total" => StatusCode(409, new JsonResult(ex.Message)),
                    "Not enough payment" => StatusCode(406, new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
        }
        #endregion

        #region HTTPGet ListSells, GetSells & SubTotal
        [HttpGet]
        [ActionName("ListSells")]
        public async Task<ActionResult<IEnumerable<SellItem>>> ListSells()
        {
            var sell = await DbAccessClass.ListSell(_context);

            return Ok(_mapper.Map<IEnumerable<SellGet>>(sell));
        }

        [HttpGet]
        [ActionName("GetSell")]
        public async Task<ActionResult> GetSellItems([FromBody]long sellID)
        {
            try
            {
                var sellExists = await DbAccessClass.SellIDExists(sellID, _context);

                if (!sellExists)
                    throw new Exception("This sell does not exists");

                var getSell = await DbAccessClass.GetSell(sellID, _context);

                return Ok(_mapper.Map<SellGet>(getSell));
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "Sell does not exist": return NotFound(new JsonResult(ex.Message));
                    default: return BadRequest(new JsonResult(ex.Message));
                }
            }
        }

        [HttpGet]
        [ActionName("GetSubTotalSell")]
        public async Task<ActionResult> GetSubtotal([FromHeader]SellSubtotal preSubtotal, [FromQuery]long productID)
        {
            try
            {
                var productExists = await DbAccessClass.ProductIDExists(productID, _context);

                if (!productExists)
                    throw new Exception("Product not found");

                var product = await DbAccessClass.GetProduct(productID, _context);
                var stockAvailability = product.StockAmount - preSubtotal.Quantity;

                if (stockAvailability < 0)
                    throw new Exception("Not enough quantity available");

                var subtotal = product.SellPrice * preSubtotal.Quantity;

                if (product.Returnable && preSubtotal.ContainerReturned)
                    subtotal = DbAccessClass.CalculateSubtotal(subtotal, product.Discount);

                return Ok(new JsonResult(subtotal));

            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Product not found" => NotFound(new JsonResult(ex.Message)),
                    "Not enough quantity available in stock" => StatusCode(417, new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
        }

        #endregion

        #region HTTPPut Update Sell
        [HttpPut]
        [ActionName("UpdateSell")]
        public async Task<ActionResult> UpdateSell([FromBody]SellItem sellUpdate)
        {
            try
            {
                var sellExists = await DbAccessClass.SellIDExists(sellUpdate.SellID, _context);

                if (!sellExists)
                    throw new Exception("Sell not found");

                var sell = await DbAccessClass.GetSell(sellUpdate.SellID, _context);
                await DbAccessClass.UpdateSell(sellUpdate, sell, _context);

                return Ok(new JsonResult("Sell updated successfully"));
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Sell not found" => NotFound(new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
        }
        #endregion

        #region HTTPDelete Delete Sell
        [HttpDelete]
        [ActionName("EliminateSell")]
        public async Task<ActionResult> DeleteOrder([FromBody]long sellID)
        {
            try
            {
                var sellExists = await DbAccessClass.SellIDExists(sellID, _context);

                if (!sellExists)
                    throw new Exception("Sell not found");

                var sell = await DbAccessClass.GetSell(sellID, _context);

                await DbAccessClass.DeleteSell(sell, _context);

                return Ok(new JsonResult("Sell deleted successfully"));
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Sell not found" => NotFound(new JsonResult(ex.Message)),
                    _ => BadRequest(new JsonResult(ex.Message)),
                };
            }
        }
        #endregion
    }
}
