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
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public OrdersController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region HTTPPost
        [HttpPost]
        [ActionName("AddOrder")]
        public async Task<ActionResult<OrderItem>> PostOrderItem([FromBody]OrderItem orderItem, [FromQuery]long productID)
        {
            try
            {
                var productExists = await DbAccessClass.ProductIDExists(productID, _context);

                if (!productExists)
                    throw new Exception("Product does not exist");

                var product = await DbAccessClass.GetProduct(productID, _context);
                var orderExits = await DbAccessClass.OrderExists(product.ProductID, _context);

                if (orderExits)
                    throw new Exception("This order already exists");

                var newOrder = DbAccessClass.AddOrder(orderItem, product, _context);

                //If this return does not work just do Ok(newOrder); and make the AddOrder into a Task which returns a OrderItem
                return CreatedAtAction("GetOrder", new { id = orderItem.OrderID }, Ok("200"));

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        #endregion

        #region HTTPGet ListOrders & GetProduct
        [HttpGet]
        [ActionName("ListOrders")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> ListOrderItems()
        {
            var orders = await DbAccessClass.ListOrders(_context);

            return Ok(_mapper.Map<IEnumerable<OrderGet>>(orders));
        }

        [HttpGet]
        [ActionName("GetOrder")]
        public async Task<ActionResult> GetOrderItem([FromBody]long orderID)
        {
            try
            {
                var orderExists = await DbAccessClass.OrderIDExists(orderID, _context);

                if (!orderExists)
                    throw new Exception("This order does not exist");

                var getOrder = await DbAccessClass.GetOrder(orderID, _context);

                return Ok(_mapper.Map<OrderGet>(getOrder));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region HTTPPut UpdateOrder
        [HttpPut]
        [ActionName("UpdateOrder")]
        public async Task<IActionResult> PutOrderItem([FromBody]OrderItem orderItem)
        {
            try
            {
                await DbAccessClass.UpdateOrder(orderItem, _context);
                return Ok("200");
            }
            catch (Exception ex)
            {
                if (!await DbAccessClass.ProductIDExists(orderItem.OrderID, _context))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region HTTPDelete EliminateOrder
        [HttpDelete]
        [ActionName("EliminateOrder")]
        public async Task<ActionResult> DeleteOrder([FromBody]long orderID)
        {
            var orderExists = await DbAccessClass.OrderIDExists(orderID, _context);

            if (!orderExists)
                return BadRequest("Not done");

            var order = await DbAccessClass.GetOrder(orderID, _context);
            var product = await DbAccessClass.GetProduct(order.Product.ProductID, _context);
            System.Diagnostics.Debug.WriteLine("THIS IS THE PRODUCT IS FOR THIS ORDER " + product);
            await DbAccessClass.DeleteOrder(order, _context);

            return Ok("200");
        }
        #endregion
    }
}
