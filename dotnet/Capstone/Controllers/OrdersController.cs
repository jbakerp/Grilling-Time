using Capstone.DAO;
using Capstone.Logic;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private IOrderDao OrderDao;
        
        public OrdersController(IOrderDao orderDao)
        {
            this.OrderDao = orderDao;
        }


        [HttpGet("cookout/{cookoutId}/orders/{id}")]
        [AllowAnonymous]
        public Order GetOrderByOrderId(int id)
        {
            return OrderDao.GetOrderByOrderId(id);
        }

        [HttpPost("cookout/{cookoutId}/orders")]
        [AllowAnonymous]
        public ActionResult<Order> CreateOrder(Order order, int cookoutId)
        {
            Order newOrder = null;
            newOrder = OrderDao.CreateOrder(order);
            return Created($"/cookout/{cookoutId}/orders/{order.OrderId}", newOrder);
        }

        [HttpPut("cookout/{cookoutId}/orders/{id}")]
        public Order UpdateOrder(Order order, int id)
        {
            FinishedOrderSender emailSender = new FinishedOrderSender();
            emailSender.SendFinishedOrderMail(order.Email);
            return OrderDao.UpdateOrder(order);
        }

        [HttpGet("cookout/{cookoutId}/orders/{id}/items")]
        [AllowAnonymous]
        public ActionResult<IList<MenuItem>> GetItemsByOrder(int id)
        {
            return Ok(OrderDao.GetAllItemByOrderId(id));
        }

    }
}
