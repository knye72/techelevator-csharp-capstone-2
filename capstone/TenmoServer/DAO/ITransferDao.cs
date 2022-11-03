using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Security;
using TenmoServer.Controllers;
using TenmoServer.Models;
using TenmoServer.DAO;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        decimal GetBalance(string username);
        User UpdateAccount(User user);
        int GetAccountId(string username);
        Transfer CreateTransfer(Transfer transfer, string fromUsername, string toUsername);
        List<User> GetUsers();
    }
}
