using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConecctionString)
        {
            connectionString = dbConecctionString;
        }

        public decimal GetBalance(string username)
        {
            decimal balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM account JOIN tenmo_user ON tenmo_user.user_id = account.user_id WHERE username = @username", conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        balance = Convert.ToDecimal(reader["balance"]);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return balance;
        }
        public decimal GetBalanceByAccount(int accountId)
        {
            decimal balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM account  WHERE account_id = @account_id", conn);
                    cmd.Parameters.AddWithValue("@account_id", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        balance = Convert.ToDecimal(reader["balance"]);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return balance;
        }
        public int GetAccountId(string username)
        {
            int accountId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id FROM account JOIN tenmo_user ON tenmo_user.user_id = account.user_id WHERE username = @username", conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        accountId = Convert.ToInt32(reader["account_id"]);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return accountId;
        }
        public Account GetAccount(int accountId)
        {
            Account account = new Account();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM account WHERE account_id = @account_id", conn);
                    cmd.Parameters.AddWithValue("@account_id", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account.AccountId = Convert.ToInt32(reader["account_id"]);
                        account.UserId = Convert.ToInt32(reader["user_id"]);
                        account.Balance = Convert.ToDecimal(reader["balance"]);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return account;
        }
        public User GetUserFromReader(SqlDataReader reader)
        {
            User u = new User()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
               
            };

            return u;
        }

        public List<User> GetUsers()
        {
            List<User> returnUsers = new List<User>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT user_id, username FROM tenmo_user", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User u = GetUserFromReader(reader);
                        returnUsers.Add(u);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnUsers;
        }
        public Transfer CreateTransfer(Transfer transfer)
        {
            int newTransferId;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //int fromAccountid = GetAccountId();

                SqlCommand cmd = new SqlCommand("INSERT INTO transfer(transfer_id transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                    "OUTPUT INSERTED.transfer_id VALUES(@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);", conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                cmd.Parameters.AddWithValue("@account_from", transfer.FromAccountId);
                cmd.Parameters.AddWithValue("@account_to", transfer.ToAccountId);
                cmd.Parameters.AddWithValue("@amount", transfer.TransferAmount);

                //ExecuteScalar() since we expect the id back from the query
                newTransferId = Convert.ToInt32(cmd.ExecuteScalar()); //need to do the conversion to a data type that C# understands 


            }

            Transfer newTransfer = GetTransfer(newTransferId); //we wrote a method for getting a specific transfer from the DB, so let's use it
            return newTransfer;
        }
        public Transfer GetTransfer(int transferId)
        {
            Transfer transfer = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM transfer WHERE transfer_id = @transfer_id", conn);
                cmd.Parameters.AddWithValue("@transfer_id", transferId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) //if only one row is expected back, just check to see if it can be read, don't need a whole while loop for one row 
                {
                    transfer = CreateTransferFromReader(reader);
                }

            }

            return transfer;
        }
        private Transfer CreateTransferFromReader(SqlDataReader reader) //make a transfer object out of row of SQL data
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.TransferAmount = Convert.ToDecimal(reader["amount"]);
            transfer.FromAccountId = Convert.ToInt32(reader["account_from"]);
            transfer.ToAccountId = Convert.ToInt32(reader["account_to"]);

            return transfer;
        }
        public Account UpdateAccount(Account account)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE account SET balance = @balance WHERE user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@balance", account.Balance);
                cmd.Parameters.AddWithValue("@user_id", account.UserId);


                cmd.ExecuteNonQuery(); //don't bother saving the number of rows changed anywhere, just exec the query
            }
            return account;
        }
    }
}
