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
        decimal GetBalanceByAccount(int accountId);
        Account UpdateAccount(Account account);
        int GetAccountId(string username);
        Account GetAccount(int accountId);
        Transfer CreateTransfer(Transfer transfer);
        List<User> GetUsers();
        List<Transfer> GetTransfersByUser(string username);
    }
}
