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


    } 
}

