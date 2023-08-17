using Capstone.Models;
using System.Collections;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IOrderDao
    {
        public IList<Order> GetAllOrdersByCookout(int cookoutId);
        public Order GetOrderByOrderId(int orderId);
        public IList<Order> GetIncompleteOrCompleteOrders(bool isComplete, int cookoutId);
        public Order CreateOrder(Order order);
        public int DeleteOrderByOrderId(int orderId);
        public Order UpdateOrder(Order order);
        public IList<MenuItem> GetAllItemByOrderId(int id);

    }
}
