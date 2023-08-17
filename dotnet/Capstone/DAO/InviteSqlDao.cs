using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.Security.Models;
using System.Net;
using System.Net.Mail;

namespace Capstone.DAO
{
    public class InviteSqlDao : IInviteDao
    {
        private readonly string connectionString;

        public InviteSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public IList<Invite> GetAllInvites()
        {
            List<Invite> inviteList = new List<Invite>();

            string sql = "SELECT invite_id, invite_email, person_name, is_sent, cookout_id, response_status FROM invites";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Invite invite = MapRowToInvite(reader);
                        inviteList.Add(invite);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return inviteList;
        }
        public IList<Invite> GetInvitesByEmail(string email)
        {
            List<Invite> invites = new List<Invite>();

            string sql = "SELECT invite_id, invite_email, person_name, is_sent, cookout_id, response_status FROM invites " +
                "WHERE invite_email = @invite_email";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@invite_email", email);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Invite invite = MapRowToInvite(reader);
                        invites.Add(invite);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql Exception occurred", ex);
            }
            return invites;
        }
        public Invite GetInviteByEmailAddress(int cookoutId, string email)
        {
            Invite invite = null;

            string sql = "SELECT invite_id, invite_email, person_name, is_sent, cookout_id, response_status FROM invites " +
                "WHERE invite_email = @invite_email AND cookout_id = @cookout_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@invite_email", email);
                    cmd.Parameters.AddWithValue("@cookout_id", cookoutId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        invite = MapRowToInvite(reader);
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new DaoException("SQL Exception occurred", ex);
            }
            return invite;
        }
        public Invite GetInviteByUserId(int userId)
        {
            Invite invite = null;

            string sql = "SELECT invite_id, invite_email, person_name, is_sent, cookout_id, response_status FROM invites WHERE invite_id = " +
                "(SELECT invite_id, user_id FROM invite_user WHERE user_id = @user_id)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        invite = MapRowToInvite(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL Exception occurred", ex);
            }
            return invite;
        }
        public Invite GetInviteById(int inviteId)
        {
            Invite invite = null;

            string sql = "SELECT invite_id, invite_email, person_name, is_sent, cookout_id, response_status FROM invites WHERE invite_id = @invite_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@invite_id", inviteId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        invite = MapRowToInvite(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL Exception occurred", ex);
            }
            return invite;
        }

        public IList<Invite> GetInvitesByCookoutId(int cookoutId)
        {
            List<Invite> invites = new List<Invite>();

            string sql = "SELECT invite_id, invite_email, person_name, is_sent, cookout_id, response_status FROM invites WHERE cookout_id = @cookout_id";

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
                        Invite invite = MapRowToInvite(reader);
                        invites.Add(invite);
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new DaoException("SQL Exception Occurred", ex);
            }
            return invites;
        }
        public Invite UpdateInvite(Invite inviteToUpdate)
        {
            Invite updatedInvite = null;

            string sql = "UPDATE invites SET response_status = @response_status " +
                "WHERE invite_id = @invite_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    //cmd.Parameters.AddWithValue("@invite_email", inviteToUpdate.InviteEmail);
                    //cmd.Parameters.AddWithValue("@person_name", inviteToUpdate.PersonName);
                    //cmd.Parameters.AddWithValue("@is_sent", inviteToUpdate.IsSent);
                    //cmd.Parameters.AddWithValue("@cookout_id", inviteToUpdate.CookoutId);
                    cmd.Parameters.AddWithValue("@response_status", inviteToUpdate.ResponseStatus);
                    cmd.Parameters.AddWithValue("@invite_id", inviteToUpdate.InviteId);

                    cmd.ExecuteNonQuery();
                }

                updatedInvite = GetInviteById(inviteToUpdate.InviteId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return updatedInvite;
        }
        public Invite CreateNewInvite(Invite newInvite)
        {
            string sql1 = "INSERT INTO invites (invite_email, person_name, is_sent, cookout_id, response_status) " +
                "OUTPUT INSERTED.invite_id " +
                "VALUES (@invite_email, @person_name, @is_sent, @cookout_id, @response_status)";

            string sql2 = "INSERT INTO invite_user (invite_id, user_id) " +
                "VALUES (@invite_id, @user_id)";
            try
            {
                int newInviteId;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql1, conn);
                    cmd.Parameters.AddWithValue("@invite_email", newInvite.InviteEmail);
                    cmd.Parameters.AddWithValue("@person_name", newInvite.PersonName);
                    cmd.Parameters.AddWithValue("@is_sent", newInvite.IsSent);
                    cmd.Parameters.AddWithValue("@cookout_id", newInvite.CookoutId);
                    cmd.Parameters.AddWithValue("@response_status", newInvite.ResponseStatus);

                    newInviteId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                newInvite = GetInviteById(newInviteId);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql2, conn);
                    cmd.Parameters.AddWithValue("@invite_id", newInvite.InviteId);

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return newInvite;
        }

        private Invite MapRowToInvite(SqlDataReader reader)
        {
            Invite invite = new Invite();
            invite.InviteId = Convert.ToInt32(reader["invite_id"]);
            invite.InviteEmail = Convert.ToString(reader["invite_email"]);
            invite.PersonName = Convert.ToString(reader["person_name"]);
            invite.IsSent = Convert.ToBoolean(reader["is_sent"]);
            invite.CookoutId = Convert.ToInt32(reader["cookout_id"]);
            invite.ResponseStatus = Convert.ToInt32(reader["response_status"]);
            return invite;
        }
    }
}
