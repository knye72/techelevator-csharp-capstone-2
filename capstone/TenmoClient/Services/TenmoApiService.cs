using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...

        public int GetAccountNumber(string username)
        {
            RestRequest request = new RestRequest($"user/account/{username}");
            IRestResponse<int> response = client.Get<int>(request);

            CheckForError(response);
            return response.Data;
        }

        public decimal GetBalance(string username)
        {
            
            RestRequest request = new RestRequest($"user/{username}");
            IRestResponse<decimal> response = client.Get<decimal>(request);

            CheckForError(response);
            return response.Data;
        }

        public SendTransfer Transfer(int fromAccountId, int toAccountId, decimal amount, int transferTypeId, int transferStatusId)
        {
            SendTransfer transfer = new SendTransfer();

            transfer.ToAccountId = toAccountId;
            transfer.FromAccountId = fromAccountId;
            transfer.TransferAmount = amount;
            transfer.TransferTypeId = transferTypeId;
            transfer.TransferStatusId = transferStatusId;

            RestRequest request = new RestRequest($"user/transfer");
            request.AddJsonBody(transfer);//change
            IRestResponse<SendTransfer> response = client.Post<SendTransfer>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<ApiUser> ListUsers()
        {
            RestRequest request = new RestRequest($"user");
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<SendTransfer> ListTransfer(string username)
        {
            RestRequest request = new RestRequest($"user/transfer/{username}");
            
            IRestResponse<List<SendTransfer>> response = client.Get<List<SendTransfer>>(request);

            CheckForError(response);
            return response.Data;
        }
    }
}
