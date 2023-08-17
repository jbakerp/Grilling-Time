using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;
using System.Security.Policy;

namespace Capstone.DAO
{
    public class OrderSqlDao : IOrderDao
    {
        private readonly string connectionString;

        public OrderSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Order> GetAllOrdersByCookout(int id)
        {
            IList<Order> orders = new List<Order>();

            string sql = "SELECT * FROM orders " +
                "JOIN menu_order mu ON orders.order_id = mu.order_id " +
                "JOIN menu_items mi ON mu.item_id = mi.item_id " +
                "WHERE orders.cookout_id = @cookout_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cookout_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Order nextOrder = MapRowToOrder(reader);
                        orders.Add(nextOrder);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return orders;
        }

        public Order GetOrderByOrderId(int orderId)
        {
            Order order = null;

            string sql = "SELECT * FROM orders " +
                "JOIN menu_order mu ON orders.order_id = mu.order_id " +
                "JOIN menu_items mi ON mu.item_id = mi.item_id " +
                "WHERE orders.order_id = @order_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    SqlDataReader reader = cmd.ExecuteReader();

                     while(reader.Read())
                     {
                        MenuItem nextMenuItem = new MenuItem();
                        if(order != null)
                        {
                            nextMenuItem = MapRowToMenuItem(reader);
                            order.MenuItems.Add(nextMenuItem);
                        }
                        else
                        {
                            order = MapRowToOrder(reader);
                        }
                        
                        
                     }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return order;
        }

        public IList<Order> GetIncompleteOrCompleteOrders(bool isComplete, int cookoutId)
        {
            IList <Order> orders = new List<Order>();
            string sql = "";

            if (isComplete)
            {
                sql = "SELECT * FROM orders " +
                    "JOIN menu_order mu ON orders.order_id = mu.order_id " +
                    "JOIN menu_items mi ON mu.item_id = mi.item_id " +
                    "WHERE is_completed = 1 AND cookout_id = @cookout_id";
            }
            else
            {
               sql = "SELECT * FROM orders " +
                    "JOIN menu_order mu ON orders.order_id = mu.order_id " +
                    "JOIN menu_items mi ON mu.item_id = mi.item_id " +
                    "WHERE is_completed = 0 AND cookout_id = @cookout_id";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cookout_id", cookoutId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Order nextOrder = MapRowToOrder(reader);
                        orders.Add(nextOrder);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return orders;
        }

        public Order CreateOrder(Order newOrder)
        {

            Order addedOrder = null;

            string sql1 = "INSERT INTO orders (cookout_id, is_completed, email) " +
                "OUTPUT INSERTED.order_id " +
                "VALUES (@cookout_id, @is_completed, @email)";

            string sql2 = "INSERT INTO menu_order (item_id, order_id) " +
                "VALUES (@item_id, @order_id)";

            try
            {
                int newOrderId;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql1, conn);
                    cmd.Parameters.AddWithValue("@cookout_id", newOrder.CookoutId);
                    cmd.Parameters.AddWithValue("@is_completed", newOrder.IsCompleted);
                    cmd.Parameters.AddWithValue("@email", newOrder.Email);

                    newOrderId = Convert.ToInt32(cmd.ExecuteScalar());

                    foreach(MenuItem item in newOrder.MenuItems)
                    {
                        cmd = new SqlCommand(sql2, conn);
                        cmd.Parameters.AddWithValue("@item_id", item.ItemID);
                        cmd.Parameters.AddWithValue("@order_id", newOrderId);
                        cmd.ExecuteNonQuery();
                    }


                }

                addedOrder = GetOrderByOrderId(newOrderId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return addedOrder;
        }

        public int DeleteOrderByOrderId(int orderId)
        {
            int rowsDeleted;
            string sql1 = "DELETE FROM menu_order " +
                "WHERE order_id = @order_id";

            string sql2 = "DELETE FROM order " +
                "WHERE order_id = @order_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql1, conn);
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(sql2, conn);
                    cmd.Parameters.AddWithValue("@order_id", orderId);

                    rowsDeleted = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("An SQL Error Occurred", ex);
            }

            return rowsDeleted;
        }

        public Order UpdateOrder(Order order)
        {
            Order updatedOrder = null;

            string sql = "UPDATE orders SET cookout_id = @cookout_id, is_completed = @is_completed, email = @email " +
                "WHERE order_id = @order_id";

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cookout_id", order.CookoutId);
                    cmd.Parameters.AddWithValue("@is_completed", order.IsCompleted);
                    cmd.Parameters.AddWithValue("@email", order.Email);
                    cmd.Parameters.AddWithValue("@order_id", order.OrderId);

                    cmd.ExecuteNonQuery();

                }
                updatedOrder = GetOrderByOrderId(order.OrderId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("An SQL Error Occurred", ex);
            }

            return updatedOrder;
        }

        public IList<MenuItem> GetAllItemByOrderId(int id)
        {
            IList<MenuItem> items = new List<MenuItem>();
            string sql = "SELECT * FROM menu_items as mu " +
                "JOIN menu_order as mo ON mu.item_id = mo.item_id " +
                "WHERE order_id = @order_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@order_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        MenuItem nextMenuItem = MapRowToMenuItem(reader);
                        items.Add(nextMenuItem);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return items;
        }



        private Order MapRowToOrder(SqlDataReader reader)
        {
            Order order = new Order();
            MenuItem item = new MenuItem();

            order.OrderId = Convert.ToInt32(reader["order_id"]);
            order.CookoutId = Convert.ToInt32(reader["cookout_id"]);
            order.IsCompleted = Convert.ToBoolean(reader["is_completed"]);
            order.Email = Convert.ToString(reader["email"]);
            item = MapRowToMenuItem(reader);
            order.MenuItems.Add(item);
            return order;
        }
        private MenuItem MapRowToMenuItem(SqlDataReader reader)
        {
            MenuItem item = new MenuItem();
            item.ItemID = Convert.ToInt32(reader["item_id"]);
            item.Name = Convert.ToString(reader["name"]);
            item.Description = Convert.ToString(reader["description"]);
            item.Price = Convert.ToDecimal(reader["price"]);
            item.Image = Convert.ToString(reader["image"]);
            item.IsVegan = Convert.ToBoolean(reader["is_vegan"]);
            item.IsVegetarian = Convert.ToBoolean(reader["is_vegetarian"]);
            item.IsGlutenFree = Convert.ToBoolean(reader["is_gluten_free"]);
            item.IsAvailable = Convert.ToBoolean(reader["is_available"]);
            item.CookoutId = Convert.ToInt32(reader["cookout_id"]);
            item.IsGuestBrought = Convert.ToBoolean(reader["is_guest_brought"]);

            return item;
        }


    }


}
