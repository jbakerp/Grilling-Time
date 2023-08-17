using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class MenuItemSqlDao : IMenuItemDao
    {
        private readonly string connectionString;

        public MenuItemSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public MenuItem GetMenuItemByItemId(int id)
        {
            MenuItem item = null;

            string sql = "SELECT * FROM menu_items " +
                "WHERE item_id = @item_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@item_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        item = MapRowToMenuItem(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return item;
        }

        public IList<MenuItem> GetMenuItems()
        {
            IList<MenuItem> items = new List<MenuItem>();
            string sql = "SELECT * FROM menu_items";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
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

        public MenuItem CreateMenuItem(MenuItem item)
        {
            MenuItem newItem = null;

            string sql = "INSERT INTO menu_items (name, description, price, image, is_vegan, is_vegetarian, is_gluten_free, is_available, cookout_id, is_guest_brought) " +
                "OUTPUT INSERTED.item_id " +
                "VALUES(@name, @description, @price, @image, @is_vegan, @is_vegetarian, @is_gluten_free, @is_available, @cookout_id, @is_guest_brought)";

            try
            {
                int newItemId;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.Parameters.AddWithValue("@price", item.Price);
                    cmd.Parameters.AddWithValue("@image", item.Image);
                    cmd.Parameters.AddWithValue("@is_vegan", item.IsVegan);
                    cmd.Parameters.AddWithValue("@is_vegetarian", item.IsVegetarian);
                    cmd.Parameters.AddWithValue("@is_gluten_free", item.IsGlutenFree);
                    cmd.Parameters.AddWithValue("@is_available", item.IsAvailable);
                    cmd.Parameters.AddWithValue("@cookout_id", item.CookoutId);
                    cmd.Parameters.AddWithValue("@is_guest_brought", item.IsGuestBrought);

                    newItemId = Convert.ToInt32(cmd.ExecuteScalar());

                }

                newItem = GetMenuItemByItemId(newItemId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return newItem;
        }

        public MenuItem UpdateMenuItem(MenuItem item)
        {
            MenuItem updatedItem = null;

            string sql = "UPDATE menu_items SET name = @name, description = @description, price = @price, image = @image, is_vegan = @is_vegan, is_vegetarian = @is_vegetarian, is_gluten_free = @is_gluten_free, is_available = @is_available, cookout_id = @cookout_id, is_guest_brought = @is_guest_brought " +
                "WHERE item_id = @item_id";

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@item_id", item.ItemID);
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.Parameters.AddWithValue("@price", item.Price);
                    cmd.Parameters.AddWithValue("@image", item.Image);
                    cmd.Parameters.AddWithValue("@is_vegan", item.IsVegan);
                    cmd.Parameters.AddWithValue("@is_vegetarian", item.IsVegetarian);
                    cmd.Parameters.AddWithValue("@is_gluten_free", item.IsGlutenFree);
                    cmd.Parameters.AddWithValue("@is_available", item.IsAvailable);
                    cmd.Parameters.AddWithValue("@cookout_id", item.CookoutId);
                    cmd.Parameters.AddWithValue("@is_guest_brough", item.IsGuestBrought);
                    cmd.ExecuteNonQuery();

                }
                updatedItem = GetMenuItemByItemId(item.ItemID);
            }
            catch (SqlException ex)
            {
                throw new DaoException("An SQL Error Occurred", ex);
            }

            return updatedItem;
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
