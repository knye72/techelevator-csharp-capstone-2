using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    // model for creating a send money transfer
    public class SendTransfer
    {
        public int TransferId { get; set; }

        public decimal TransferAmount { get; set; }

        public int TransferStatusId { get; set; } //= 2; // 1 = pending, 2 = approved, 3 = rejected

        public string ToUsername { get; set; }
        public string FromUsername { get; set; } // may need to be username

        public int TransferTypeId { get; set; } // 1 = request, 2 = send

        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }
        public SendTransfer()
        {

        }

    }
}
