using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Policy;

namespace Capstone.DAO
{
    public class CookoutSqlDao : ICookoutDao
    {
        private readonly string connectionString;

        public CookoutSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Cookout> GetListOfCookouts()
        {
            IList<Cookout> cookouts = new List<Cookout>();
            string sql = "SELECT * FROM cookout";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Cookout nextCookout = MapRowToCookout(reader);
                        cookouts.Add(nextCookout);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return cookouts;
        }

        public IList<Cookout> GetListOfCookoutsByHostId(int hostId)
        {
            IList<Cookout> cookouts = new List<Cookout>();
            string sql = "SELECT * FROM cookout " +
                "WHERE host_id = @host_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@host_id", hostId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Cookout nextCookout = MapRowToCookout(reader);
                        cookouts.Add(nextCookout);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return cookouts;
        }

        public IList<Cookout> GetListOfCookoutsByChefId(int chefId)
        {
            IList<Cookout> cookouts = new List<Cookout>();
            string sql = "SELECT * FROM cookout " +
                "WHERE chef_id = @chef_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@chef_id", chefId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Cookout nextCookout = MapRowToCookout(reader);
                        cookouts.Add(nextCookout);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return cookouts;
        }



        public IList<Cookout> GetInvitedCookouts(string email)
        {
            IList<Cookout> cookouts = new List<Cookout>();
            string sql = "SELECT * FROM cookout as c " +
                "JOIN invites as i ON c.cookout_id = i.cookout_id " +
                "WHERE i.invite_email = @email";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Cookout nextCookout = MapRowToCookout(reader);
                        cookouts.Add(nextCookout);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return cookouts;
        }

        public Cookout GetCookoutByCookoutId(int id)
        {
            Cookout cookout = null;

            string sql = "SELECT * FROM cookout " +
                "WHERE cookout_id = @cookout_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cookout_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cookout = MapRowToCookout(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return cookout;
        }

        public Cookout CreateCookout(Cookout cookout)
        {
            Cookout newCookout = null;
            string sql = "INSERT INTO cookout (number_of_attendees, host_id, chef_id, date_of_event, street_address, city, state_abbreviation, zipcode, cookout_name, description) " +
                "OUTPUT INSERTED.cookout_id " +
                "VALUES (@number_of_attendees, @host_id, @chef_id, @date_of_event, @street_address, @city, @state_abbreviation, @zipcode, @cookout_name, @description)";

            try
            {
                int newCookoutId;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@number_of_attendees", cookout.NumberOfAttendees);
                    cmd.Parameters.AddWithValue("@host_id", cookout.HostId);
                    cmd.Parameters.AddWithValue("@chef_id", cookout.ChefId);
                    cmd.Parameters.AddWithValue("@date_of_event", cookout.DateOfEvent);
                    cmd.Parameters.AddWithValue("@street_address", cookout.Location.StreetAddress);
                    cmd.Parameters.AddWithValue("@city", cookout.Location.City);
                    cmd.Parameters.AddWithValue("@state_abbreviation", cookout.Location.StateAbbreviation);
                    cmd.Parameters.AddWithValue("@zipcode", cookout.Location.Zipcode);
                    cmd.Parameters.AddWithValue("@cookout_name", cookout.CookoutName);
                    cmd.Parameters.AddWithValue("@description", cookout.Description);

                    newCookoutId = Convert.ToInt32(cmd.ExecuteScalar());

                }

                newCookout = GetCookoutByCookoutId(newCookoutId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return newCookout;
        }

        public Cookout UpdateCookout(Cookout cookout)
        {
            Cookout updateCookout = null;

            string sql = "UPDATE cookout SET number_of_attendees = @number_of_attendees, host_id = @host_id, chef_id = @chef_id, date_of_event = @date_of_event, street_address = @street_address, city = @city, state_abbreviation = @state_abbreviation, zipcode = @zipcode, cookout_name = @cookout_name, description = @description " +
                "WHERE cookout_id = @cookout_id";

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cookout_id", cookout.CookoutID);
                    cmd.Parameters.AddWithValue("@number_of_attendees", cookout.NumberOfAttendees);
                    cmd.Parameters.AddWithValue("@host_id", cookout.HostId);
                    cmd.Parameters.AddWithValue("@chef_id", cookout.ChefId);
                    cmd.Parameters.AddWithValue("@date_of_event", cookout.DateOfEvent);
                    cmd.Parameters.AddWithValue("@street_address", cookout.Location.StreetAddress);
                    cmd.Parameters.AddWithValue("@city", cookout.Location.City);
                    cmd.Parameters.AddWithValue("@state_abbreviation", cookout.Location.StateAbbreviation);
                    cmd.Parameters.AddWithValue("@zipcode", cookout.Location.Zipcode);
                    cmd.Parameters.AddWithValue("@cookout_name", cookout.CookoutName);
                    cmd.Parameters.AddWithValue("@description", cookout.Description);

                    cmd.ExecuteNonQuery();

                }
                updateCookout = GetCookoutByCookoutId(cookout.CookoutID);
            }
            catch (SqlException ex)
            {
                throw new DaoException("An SQL Error Occurred", ex);
            }

            return updateCookout;
        }

        public IList<Order> GetOrdersByCookoutId(int cookoutId)
        {
            IList<Order> orders = new List<Order>();
            

            string sql = "SELECT * FROM orders as o " +
                "JOIN menu_order as mo ON o.order_id = mo.order_id " +
                "JOIN menu_items as mi ON mi.item_id = mo.item_id " +
                "WHERE o.cookout_id = @cookout_id";

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
                        bool has = orders.Any(o => o.OrderId == nextOrder.OrderId);
                        if (has)
                        {
                            Order order = orders.Single(item => item.OrderId == nextOrder.OrderId);
                            order.MenuItems.Add(nextOrder.MenuItems[0]);
                        }
                        else
                        {
                            orders.Add(nextOrder);
                        }
                        
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return orders;
        }

        public IList<MenuItem> GetMenuItemsForCookout(int cookoutId)
        {
            IList<MenuItem> items = new List<MenuItem>();
            string sql = "SELECT * FROM menu_items " +
                "WHERE cookout_id = @cookout_id";

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

        public IList<Order> GetOrdersByEmailForCookout(int cookoutId, string email)
        {
            IList<Order> orders = new List<Order>();

            string sql = "SELECT * FROM orders as o " +
                "JOIN menu_order as mo ON o.order_id = mo.order_id " +
                "JOIN menu_items as mi ON mo.item_id = mi.item_id " +
                "WHERE o.cookout_id = @cookout_id AND email = @email";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cookout_id", cookoutId);
                    cmd.Parameters.AddWithValue("@email", email);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Order nextOrder = MapRowToOrder(reader);
                        bool has = orders.Any(o => o.OrderId == nextOrder.OrderId);
                        if (has)
                        {
                            Order order = orders.Single(item => item.OrderId == nextOrder.OrderId);
                            order.MenuItems.Add(nextOrder.MenuItems[0]);
                        }
                        else
                        {
                            orders.Add(nextOrder);
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return orders;
        }



        private Cookout MapRowToCookout(SqlDataReader reader)
        {
            Cookout cookout = new Cookout();
            Address address = new Address();

            address.City = Convert.ToString(reader["city"]);
            address.StateAbbreviation = Convert.ToString(reader["state_abbreviation"]);
            address.StreetAddress = Convert.ToString(reader["street_address"]);
            address.Zipcode = Convert.ToInt32(reader["zipcode"]);

            cookout.CookoutName = Convert.ToString(reader["cookout_name"]);
            cookout.Description = Convert.ToString(reader["description"]);
            cookout.CookoutID = Convert.ToInt32(reader["cookout_id"]);
            cookout.NumberOfAttendees = Convert.ToInt32(reader["number_of_attendees"]);
            cookout.HostId = Convert.ToInt32(reader["host_id"]);
            cookout.ChefId = Convert.ToInt32(reader["chef_id"]);
            cookout.DateOfEvent = Convert.ToDateTime(reader["date_of_event"]);



            cookout.Location = address;

            return cookout;
        }

        private Order MapRowToOrder(SqlDataReader reader)
        {
            Order order = new Order();
            List<MenuItem> orders = new List<MenuItem>();
            order.OrderId = Convert.ToInt32(reader["order_id"]);
            order.CookoutId = Convert.ToInt32(reader["cookout_id"]);
            order.IsCompleted = Convert.ToBoolean(reader["is_completed"]);
            order.Email = Convert.ToString(reader["email"]);

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

            orders.Add(item);
            order.MenuItems = orders;
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
