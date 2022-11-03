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
        //public SendTransfer SendMoney()
        //{

        //}
    }
}
