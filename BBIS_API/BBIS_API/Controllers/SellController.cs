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
                    throw new Exception("Not done");

                var availableStock = product.StockAmount - sellItem.QuantitySold;

                if (availableStock < 0)
                    throw new Exception("Not enough quantity available in stock");

                await DbAccessClass.AddSell(sellItem, product, _context);

                var change = /*sellItem.Payed -*/ sellItem.TotalCost;

                return CreatedAtAction("GetSell", new { id = sellItem.SellID }, change);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("GetSubTotalSell")]
        public async Task<ActionResult> GetSubtotal([FromBody]SellSubtotal preSubtotal, [FromQuery]long productID)
        {
            try
            {
                var productExists = await DbAccessClass.ProductIDExists(productID, _context);

                if (!productExists)
                    throw new Exception("Not Found");

                var product = await DbAccessClass.GetProduct(productID, _context);
                var stockAvailability = product.StockAmount - preSubtotal.Quantity;

                if (stockAvailability < 0)
                    throw new Exception("Not enough quantity available in stock");

                var subtotal = product.SellPrice * preSubtotal.Quantity;

                if (product.Returnable && preSubtotal.ContainerReturned)
                    subtotal *= 1 - ((decimal)product.Discount / 100);

                return Ok(Math.Round(subtotal, 2));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                    throw new Exception("Not done");

                var sell = await DbAccessClass.GetSell(sellUpdate.SellID, _context);
                await DbAccessClass.UpdateSell(sellUpdate, sell, _context);

                return Ok("Done");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                    throw new Exception("Not done");

                var sell = await DbAccessClass.GetSell(sellID, _context);
                var product = await DbAccessClass.GetProduct(sell.Product.ProductID, _context);

                await DbAccessClass.DeleteSell(sell, _context);

                return Ok("Done");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
