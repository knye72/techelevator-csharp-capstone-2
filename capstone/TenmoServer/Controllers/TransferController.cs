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

            User updatedUser = transferDao.UpdateTransfer(fromUser.UserId, user);
            //have to somehow update updatedUser's balance by using Math method
            return updatedUser;
        }
        public Transfer Math(User fromUser, User toUser, decimal amount)
        {
            Transfer transfer = new Transfer();
            
            int transferStatus = 2;

            //retrieve balance from first user from database, subtract amount from first user, add amount to second user, show balance of first user
            
            if (fromUser.Balance >= amount)
            {
                decimal fromUserBalance = transferDao.GetBalance(fromUser.Username) - amount;
                decimal toUserBalance = transferDao.GetBalance(toUser.Username) + amount;
                UpdateUser(fromUser.Username);
                UpdateUser(toUser.Username);
            }
            else
            {
                transferStatus = 3;
            }
            AddTransfer(transfer);
            return transfer;

        }
        [HttpPost()]
        public ActionResult<Transfer> AddTransfer(Transfer transfer)
        {
            Transfer added = transferDao.CreateTransfer(transfer);
            return Created($"/users/{added.Id}", added);
        }
        //STEPS FOR SENDING MONEY
        // 1) call method that gets user balance
        // 2) call method that does the math
        // 3) call method that updates the users' balances


    } 
}

