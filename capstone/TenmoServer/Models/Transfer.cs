using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
       
        public decimal TransferAmount { get; set; }

        public int TransferStatusId { get; set; } = 2; // 1 = pending, 2 = approved, 3 = rejected

        public int ToAccountId { get; set; }
        public int FromAccountId { get; set; } // may need to be username

        public int TransferTypeId { get; set; } // 1 = request, 2 = send
        public Transfer()
        {

        }
    }
}
