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

        public decimal GetBalance(string username)
        {
            
            RestRequest request = new RestRequest($"user/{username}");
            IRestResponse<decimal> response = client.Get<decimal>(request);

            CheckForError(response);
            return response.Data;
        }

        public SendTransfer Transfer(string fromUser, string toUser, decimal amount)
        {
            RestRequest request = new RestRequest($"user");
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
    }
}
