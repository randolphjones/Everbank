using System;
using Everbank.Service;
using Everbank.Repositories.Contracts;

namespace Everbank.Console
{
    public static class Controller
    {
        public static void Home()
        {
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
            string username = System.Console.ReadLine();
            System.Console.WriteLine("Enter your Password:");
            string password = System.Console.ReadLine();
            // TODO: Handle successful login
            // TODO: Handle failed login - error with email address or password
            // TODO: Get first name for retrieved user and welcome them
            string firstName = "Test string";
            System.Console.WriteLine($"Welcome back to Everbank, {firstName}.");
            // TODO: Present banking dashboard
            ShowDashboard();
        }

        private static void CreateAccount()
        {
            System.Console.WriteLine("Enter your First Name:");
            string firstName = System.Console.ReadLine();
            System.Console.WriteLine("Enter your Email Address:");
            string emailAddress = System.Console.ReadLine();
            System.Console.WriteLine("Enter a Password (8 characters minimum with at least 1 number):");
            string password = System.Console.ReadLine();
            // TODO: Validate inputs
                // TODO: Validate email address
            // TODO: Create the Account through business logic layer
            // TODO: Handle duplicate account
                // TODO: Forward to Login Page
            System.Console.WriteLine($"Account created for {firstName} with emailAddress {emailAddress}.");
            ShowDashboard();
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
                    LogoutAccount();
                    break;
                default:
                    System.Console.WriteLine("Please enter a valid option.");
                    ShowDashboard();
                    break;
            }
        }

        public static void LogoutAccount()
        {
            throw new NotImplementedException();
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
    }
}