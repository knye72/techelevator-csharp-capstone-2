using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Controllers;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{

    [Route("user")]
    [ApiController]

    public class TransferController : ControllerBase
    {
        private readonly IUserDao userDao;
        private ITransferDao transferDao;
        public TransferController(ITransferDao transferDao)
        {
            this.transferDao = transferDao;
        }

        [HttpGet("{username}")]
        public ActionResult<decimal> GetBalance(string username)
        {
            decimal balance = transferDao.GetBalance(username);

            if (balance >= 0)
            {
                return balance;
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPut("{id}")] // /users/:id 
        public ActionResult<User> UpdateUser(string username)
        {
            User fromUser = userDao.GetUser(username);
            if (fromUser == null) //if the user doesn't exist 
            {
                return NotFound(); //404
            }

            //do what you gotta do to update the thing

            User updatedUser = transferDao.UpdateAccount(fromUser);
            //have to somehow update updatedUser's balance by using Math method
            return updatedUser;
        }
        [HttpPost("transfer")]
        public Transfer SendTransfer(string fromUsername, string toUsername, decimal amount)
        {
            User fromUser = userDao.GetUser(fromUsername);
            User toUser = userDao.GetUser(toUsername);
            Transfer transfer = new Transfer();

            transfer.TransferStatusId = 2;

            //retrieve balance from first user from database, subtract amount from first user, add amount to second user, show balance of first userdecimel


            decimal fromUserInitialBalance = transferDao.GetBalance(fromUser.Username);

            if (fromUserInitialBalance >= amount)
            {
                decimal fromUserBalance = transferDao.GetBalance(fromUser.Username) - amount;
                decimal toUserBalance = transferDao.GetBalance(toUser.Username) + amount;
                UpdateUser(fromUser.Username);
                UpdateUser(toUser.Username);
                transfer.FromAccountId = transferDao.GetAccountId(fromUser.Username);
                transfer.ToAccountId = transferDao.GetAccountId(toUser.Username);
                transfer.TransferAmount = amount;

            }
            else
            {
                transfer.TransferStatusId = 3;
            }
            AddTransfer(transfer, fromUser.Username, toUser.Username);
            return transfer;

        }
        [HttpPost()]
        public ActionResult<Transfer> AddTransfer(Transfer transfer, string fromUser, string toUser)
        {
            Transfer added = transferDao.CreateTransfer(transfer, fromUser, toUser);
            return Created($"/user/{added.TransferId}", added);
        }
        //STEPS FOR SENDING MONEY
        // 1) call method that gets user balance
        // 2) call method that does the math
        // 3) call method that updates the users' balances
        [HttpGet()]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = transferDao.GetUsers();

            if (users.Count >= 0)
            {
                return users;
            }
            else
            {
                return NotFound();
            }

        }
    }
}

