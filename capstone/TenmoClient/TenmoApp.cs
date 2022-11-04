using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;


namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                ShowBalance(tenmoApiService.Username);
                // View your current balance
            }

            if (menuSelection == 2)
            {
                // View your past transfers
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                List<ApiUser> userList = tenmoApiService.ListUsers();
                ShowUserList(userList);
                Console.Write("enter username for user being sent money: ");
                SendTransfer transfer = new SendTransfer();
                transfer.ToUsername = Console.ReadLine();
                Console.Write("enter the amount of TEBucks you want to send to the selected user: ");
                transfer.TransferAmount = Convert.ToDecimal(Console.ReadLine());
                transfer.TransferTypeId = 2;
                transfer.FromUsername = tenmoApiService.Username;
                transfer.FromAccountId = tenmoApiService.GetAccountNumber(transfer.FromUsername);
                transfer.ToAccountId = tenmoApiService.GetAccountNumber(transfer.ToUsername);
                TransferFunds(transfer.FromAccountId, transfer.ToAccountId, transfer.TransferAmount, transfer.TransferTypeId, transfer.TransferStatusId);
                // Send TE bucks
            }

            if (menuSelection == 5)
            {
                List<ApiUser> userList = tenmoApiService.ListUsers();
                ShowUserList(userList);
                Console.Write("enter username for user who should be sending you money: ");
                SendTransfer requestTransfer = new SendTransfer();
                requestTransfer.FromUsername = Console.ReadLine();
                Console.Write("enter the amount of TEBucks you want to receieve from the selected user: ");
                requestTransfer.TransferAmount = Convert.ToDecimal(Console.ReadLine());
                requestTransfer.TransferTypeId = 1;
                requestTransfer.TransferStatusId = 1;
                requestTransfer.ToUsername = tenmoApiService.Username;
                requestTransfer.FromAccountId = tenmoApiService.GetAccountNumber(requestTransfer.FromUsername);
                requestTransfer.ToAccountId = tenmoApiService.GetAccountNumber(requestTransfer.ToUsername);
                TransferFunds(requestTransfer.FromAccountId, requestTransfer.ToAccountId, requestTransfer.TransferAmount, requestTransfer.TransferTypeId, requestTransfer.TransferStatusId);

                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }
        private void ShowBalance(string username)
        {
            
            try
            {
                decimal balance = tenmoApiService.GetBalance(username);
                if(balance >= 0)
                { 
                    Console.WriteLine(balance);
                }
                
            }
            catch(Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        private void ShowUserList(List<ApiUser> users)
        {
            try
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Users");
                Console.WriteLine("--------------------------------------------");
                foreach (ApiUser user in users)
                {
                    Console.WriteLine(user.UserId + ": " + user.Username);
                }
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }
        //private void ShowUserList()
        //{
        //    try
        //    {
        //        List<ApiUser> userList = tenmoApiService.ListUsers();
        //        if (userList != null)
        //        {
        //            Console.Write(userList.);
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        console.PrintError(ex.Message);
        //    }
        //    console.Pause();
        //}
        private void TransferFunds(int fromAccountId, int toAccountId, decimal amount, int transferTypeId, int transferStatusId)
        {
            try
            {
                SendTransfer transfer = tenmoApiService.Transfer(fromAccountId, toAccountId, amount, transferTypeId, transferStatusId);
            }
            catch(Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }
    }
}
