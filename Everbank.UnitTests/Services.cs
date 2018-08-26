using Microsoft.VisualStudio.TestTools.UnitTesting;
using Everbank.Repositories;
using Everbank.Repositories.Contracts;
using Everbank.Repositories.Utilities;
using Everbank.Service;
using Everbank.Service.Contracts;
using System.Collections.Generic;

namespace Everbank.UnitTests
{
    [TestClass]
    public class Services
    {
        [TestMethod]
        public void AuthenticateUser()
        {   
            string emailAddress = "  authTest@testing.com ";
            string password = "password123";
            string hashedPassword = UserUtilities.HashString("password123");
            UserRepository userRepository = new UserRepository();
            User newUser = userRepository.AddUser(emailAddress, hashedPassword, "AuthTester1");
            UserService UserService = new UserService();
            ServiceResponse response = UserService.AuthenticateUser(emailAddress, password);
            User authenticatedUser = response.ResponseObject as User;
            Assert.IsNotNull(authenticatedUser, "User was not successfully authenticated.");
            Assert.AreEqual(UserUtilities.ConformString(emailAddress), authenticatedUser.EmailAddress, "User's email address is not properly conformed for storage.");
            Assert.IsNull(response.Messages, "Error messages were returned from authentication method.");
        }

        [TestMethod]
        public void CreateUser()
        {
            string emailAddress = "  createUserTest@testing.com ";
            string password = "password123";
            string firstName = "CreateUserTester1";
            UserService userService = new UserService();
            ServiceResponse response = userService.CreateUser(emailAddress, password, firstName);
            User newUser = response.ResponseObject as User;
            Assert.IsNotNull(newUser, "Newly created user is null.");
            Assert.IsNull(response.Messages, "Error messages were returned from user creation method");
        }

        [TestMethod]
        public void GetTransactions()
        {
            TransactionService transactionService = new TransactionService();
            ServiceResponse response = transactionService.GetTransactions(1);
            List<Transaction> transactions = response.ResponseObject as List<Transaction>;
            Assert.IsTrue(transactions.Count > 0, "User 1 is getting empty transaction set.");
            Assert.IsNull(response.Messages, "Error messages were returned from get transactions method");
            
            ServiceResponse emptyResponse = transactionService.GetTransactions(2);
            List<Transaction> emptyTransactions = emptyResponse.ResponseObject as List<Transaction>;
            Assert.IsTrue(emptyTransactions.Count == 0, "User 2 is not getting empty transaction set.");
            Assert.IsNotNull(emptyResponse.Messages, "User 2 is not getting the 'empty transaction' message.");
        }

        [TestMethod]
        public void GetAccountBalance()
        {
            TransactionService transactionService = new TransactionService();
            ServiceResponse response = transactionService.GetAccountBalance(1);
            decimal balance = (decimal)response.ResponseObject;
            Assert.AreEqual(1350, balance, "User 1 does not show the correct account balance");

            ServiceResponse zeroResponse = transactionService.GetAccountBalance(2);
            decimal zeroBalance = (decimal)zeroResponse.ResponseObject;
            Assert.AreEqual(0, zeroBalance, "User 2 does not show the correct account balance");
        }

        [TestMethod]
        public void AddTransaction()
        {
            TransactionService transactionService = new TransactionService();

            ServiceResponse startingBalanceResponse = transactionService.GetAccountBalance(1);
            decimal startingBalance = (decimal)startingBalanceResponse.ResponseObject;

            ServiceResponse depositResponse = transactionService.CreateTransaction(1, 500);
            Transaction newDeposit = depositResponse.ResponseObject as Transaction;
            Assert.IsNotNull(newDeposit, "User 1 can not successfully make a deposit.");
            Assert.IsTrue(depositResponse.Messages.Count > 0, "User 1 did not get successful deposit message.");

            ServiceResponse postDepositBalanceResponse = transactionService.GetAccountBalance(1);
            decimal postDepositBalance = (decimal)postDepositBalanceResponse.ResponseObject;
            Assert.AreEqual(startingBalance + 500, postDepositBalance, "User 1 does not show the correct account balance after deposit.");

            ServiceResponse withdrawalResponse = transactionService.CreateTransaction(1, -500);
            Transaction newWithdrawal = withdrawalResponse.ResponseObject as Transaction;
            Assert.IsNotNull(newWithdrawal, "User 1 can not successfully make a withdrawal.");
            Assert.IsTrue(withdrawalResponse.Messages.Count > 0, "User 1 did not get successful withdrawal message.");

            ServiceResponse postWithdrawalBalanceResponse = transactionService.GetAccountBalance(1);
            decimal postWithdrawalBalance = (decimal)postWithdrawalBalanceResponse.ResponseObject;
            Assert.AreEqual(startingBalance, postWithdrawalBalance, "User 1 does not show the correct account balance after withdrawal.");

            ServiceResponse overdraftResponse = transactionService.CreateTransaction(1, postWithdrawalBalance * -2);
            Transaction overdraftTransaction = overdraftResponse.ResponseObject as Transaction;
            Assert.IsNull(overdraftTransaction, "User 1 is able to overdraft account.");
            Assert.IsTrue(overdraftResponse.Messages.Count > 0, "User 1 did not get overdraft warning.");
        }
    }
}