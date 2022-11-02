using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    // model for creating a send money transfer
    public class SendTransfer
    {
        public decimal StartingBalance { get; set; }
        public decimal TransferAmount { get; set; }

        public bool TransferStatus { get; set; } = true;

        public string ToUsername { get; set; }
        public string FromUsername { get; set; }
        
        public int TransferType { get; set; }
        public SendTransfer ()
        {

        }

    }
}
