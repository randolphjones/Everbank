using System;
using Everbank.Service;
using Everbank.Service.Contracts;
using Everbank.Repositories.Contracts;
using System.Collections.Generic;

namespace Everbank.Console
{
    public static class Controller
    {
        public static void Home()
        {
            System.Console.WriteLine("Welcome to Everbank!");
            System.Console.WriteLine("Would you like to (L)ogin or Create a (N)ew Account?");
            string loginOption = System.Console.ReadLine().ToLower();
            switch (loginOption)
            {
                case "l":
                    LoginAccount();
                    break;
                case "n":
                    CreateAccount();
                    break;
                default:
                    System.Console.WriteLine("Please enter a valid option.");
                    Home();
                    break;
            }
        }

        private static void LoginAccount()
        {
            System.Console.WriteLine("Enter your Email Address:");
            string emailAddress = System.Console.ReadLine();
            System.Console.WriteLine("Enter your Password:");
            string password = System.Console.ReadLine();
            UserService userService = new UserService();
            ServiceResponse response = userService.AuthenticateUser(emailAddress, password);
            HandleMessages(response.Messages);
            User user = response.ResponseObject as User;
            if (user != null)
            {
                System.Console.WriteLine($"Welcome back to Everbank, {user.FirstName}.");
                ShowDashboard();
            }
            else
            {
                Home();
            }
        }

        private static void CreateAccount()
        {
            System.Console.WriteLine("Enter your First Name:");
            string firstName = System.Console.ReadLine();
            System.Console.WriteLine("Enter your Email Address:");
            string emailAddress = System.Console.ReadLine();
            System.Console.WriteLine("Enter a Password (8 characters minimum with at least 1 number):");
            string password = System.Console.ReadLine();
            UserService userService = new UserService();
            ServiceResponse response = userService.CreateUser(emailAddress, password, firstName);
            HandleMessages(response.Messages);
            User newUser = response.ResponseObject as User;
            if (newUser != null)
            {
                System.Console.WriteLine($"Account created for {newUser.FirstName} with email address {newUser.EmailAddress}.");
                ShowDashboard();
            }
            else
            {
                Home();
            }
        }

        private static void ShowDashboard()
        {
            System.Console.WriteLine("Would you like to (V)iew your transactions, (D)eposit funds, (W)ithdraw funds, or (L)ogout?");
            string dashboardChoice = System.Console.ReadLine().ToLower();
            switch (dashboardChoice)
            {
                case "v":
                    ViewTransactions();
                    break;
                case "d":
                    DepositFunds();
                    break;
                case "w":
                    WithdrawFunds();
                    break;
                case "l":
                    Home();
                    break;
                default:
                    System.Console.WriteLine("Please enter a valid option.");
                    ShowDashboard();
                    break;
            }
        }

        public static void ViewTransactions()
        {
            throw new NotImplementedException();
        }

        public static void DepositFunds()
        {
            throw new NotImplementedException();
        }

        public static void WithdrawFunds()
        {
            // TODO: Enforce balance before allowing the withdrawal
            throw new NotImplementedException();
        }

        public static void HandleMessages(List<Message> messages)
        {
            if (messages != null && messages.Count > 0)
            {
                messages.ForEach(message => System.Console.WriteLine(message.Text));
            }
        }
    }
}