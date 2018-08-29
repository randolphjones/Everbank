using System;
using System.Linq;
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
                ShowDashboard(user.Id);
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
                ShowDashboard(newUser.Id);
            }
            else
            {
                Home();
            }
        }

        private static void ShowDashboard(int userId)
        {
            System.Console.WriteLine("Would you like to (V)iew your transactions, (D)eposit funds, (W)ithdraw funds, or (L)ogout?");
            string dashboardChoice = System.Console.ReadLine().ToLower();
            switch (dashboardChoice)
            {
                case "v":
                    ViewTransactions(userId);
                    break;
                case "d":
                    DepositFunds(userId);
                    break;
                case "w":
                    WithdrawFunds(userId);
                    break;
                case "l":
                    System.Console.Write("Thank you for choosing Everbank! Have an excellent day :)");
                    break;
                default:
                    System.Console.WriteLine("Please enter a valid option.");
                    ShowDashboard(userId);
                    break;
            }
        }

        private static void ViewTransactions(int userId)
        {
            TransactionService transactionService = new TransactionService();
            ServiceResponse transactionsResponse = transactionService.GetTransactions(userId);
            HandleMessages(transactionsResponse.Messages);
            List<Transaction> transactions = transactionsResponse.ResponseObject as List<Transaction>;
            if (transactions.Count > 0)
            {
                RenderTransactionTable(transactions);
            }
            ServiceResponse balanceResponse = transactionService.GetAccountBalance(transactions);
            System.Console.WriteLine();
            HandleMessages(balanceResponse.Messages);
            decimal balance = (decimal)balanceResponse.ResponseObject;
            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"Your Account Balance is ${balance}");
            System.Console.ResetColor();
            System.Console.WriteLine();
            ShowDashboard(userId);
        }

        private static void RenderTransactionTable(List<Transaction> transactions)
        {
            List<Transaction> orderedTransactions = transactions.OrderByDescending(transaction => transaction.Time).ToList();
            string headerText = string.Format("|{0,25}|{1,25}|", "Date", "Amount");
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine(headerText);
            System.Console.ResetColor();

            orderedTransactions.ForEach(transaction => {
                string formattedAmount = string.Format("{0:C}", transaction.Amount);
                string rowText = string.Format("|{0,25}|{1,25}|", transaction.Time.ToString(), formattedAmount);
                System.Console.WriteLine(rowText);
            });
        }

        private static void DepositFunds(int userId)
        {
            System.Console.WriteLine("Please enter the amount you would like to deposit:");
            string depositInput = System.Console.ReadLine();
            decimal depositAmount;
            if (decimal.TryParse(depositInput, out depositAmount) && depositAmount > 0)
            {
                TransactionService transactionService = new TransactionService();
                ServiceResponse newDepositResponse = transactionService.CreateTransaction(userId, depositAmount);
                HandleMessages(newDepositResponse.Messages);
            }
            else
            {
                System.Console.WriteLine("You entered an invalid amount. Please try again.");
            }
            System.Console.WriteLine();
            ShowDashboard(userId);
        }

        private static void WithdrawFunds(int userId)
        {
            System.Console.WriteLine("Please enter the amount you would like to withdraw:");
            string withdrawalInput = System.Console.ReadLine();
            decimal withdrawalAmount;
            if (decimal.TryParse(withdrawalInput, out withdrawalAmount) && withdrawalAmount > 0)
            {
                TransactionService transactionService = new TransactionService();
                ServiceResponse newWithdrawalResponse = transactionService.CreateTransaction(userId, withdrawalAmount * -1);
                HandleMessages(newWithdrawalResponse.Messages);
            }
            else
            {
                System.Console.WriteLine("You entered an invalid amount. Please try again.");
            }
            System.Console.WriteLine();
            ShowDashboard(userId);
        }

        private static void HandleMessages(List<Message> messages)
        {
            if (messages != null && messages.Count > 0)
            {
                messages.ForEach(message => System.Console.WriteLine(message.Text));
            }
        }
    }
}