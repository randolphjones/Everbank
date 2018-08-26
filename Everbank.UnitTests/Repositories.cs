using Microsoft.VisualStudio.TestTools.UnitTesting;
using Everbank.Repositories;
using Everbank.Repositories.Contracts;
using Everbank.Repositories.Utilities;
using System.Collections.Generic;

namespace Everbank.UnitTests
{
    [TestClass]
    public class Repositories
    {
        [TestMethod]
        public void GetUser()
        {
            UserRepository userRepository = new UserRepository();
            User user = userRepository.GetUser("    tEst@teST.com   ");
            ValidateUser(user);
        }

        [TestMethod]
        public void AddUser()
        {
            UserRepository userRepository = new UserRepository();
            string hashedPassword = UserUtilities.HashString("password123");
            User newUser = userRepository.AddUser("secondTestUser@test.com", hashedPassword, "Tester2");
            ValidateUser(newUser);
        }

        public void ValidateUser (User user)
        {
            Assert.IsNotNull(user, "User is null");
            Assert.AreNotEqual(0, user.Id, "User ID is incorrectly set to 0");
            Assert.IsFalse(string.IsNullOrEmpty(user.Password), "User password is null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(user.EmailAddress), "User email address is null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(user.FirstName), "User user first name is null or empty");
        }

        [TestMethod]
        public void CheckPasswordComplexity()
        {
            Assert.IsTrue(UserUtilities.CheckPasswordComplexity("password123"), "Password complexity standards not correctly enforced.");
            Assert.IsFalse(UserUtilities.CheckPasswordComplexity("1234"), "Password complexity standards not correctly enforced.");
            Assert.IsFalse(UserUtilities.CheckPasswordComplexity("abc"), "Password complexity standards not correctly enforced.");
            Assert.IsFalse(UserUtilities.CheckPasswordComplexity("abc123"), "Password complexity standards not correctly enforced.");
        }

        [TestMethod]
        public void CheckEmailValidity()
        {
            Assert.IsTrue(UserUtilities.CheckEmailValidity("test@test"), "Email address validity standards not correctly enforced.");
            Assert.IsTrue(UserUtilities.CheckEmailValidity("test@test.com"), "Email address validity standards not correctly enforced.");
            Assert.IsTrue(UserUtilities.CheckEmailValidity("test.test@test.com"), "Email address validity standards not correctly enforced.");
            Assert.IsFalse(UserUtilities.CheckEmailValidity("@test"), "Email address validity standards not correctly enforced.");
            Assert.IsFalse(UserUtilities.CheckEmailValidity("test.com"), "Email address validity standards not correctly enforced.");
            Assert.IsFalse(UserUtilities.CheckEmailValidity("test"), "Email address validity standards not correctly enforced.");
        }

        [TestMethod]
        public void GetTransactions()
        {
            TransactionRepository transactionRepository = new TransactionRepository();
            List<Transaction> transactions = transactionRepository.GetTransactions(1);
            Assert.IsTrue(transactions.Count > 0, "Transactions set is returned empty");
        }

        [TestMethod]
        public void GetTransaction()
        {
            TransactionRepository transactionRepository = new TransactionRepository();
            Transaction transaction = transactionRepository.GetTransaction(1);
            Assert.IsNotNull(transaction, "Transaction by unique id is returning null.");
        }

        [TestMethod]
        public void AddTransaction()
        {
            decimal amount = 500;
            int userId = 1;
            TransactionRepository transactionRepository = new TransactionRepository();

            Transaction transaction = transactionRepository.AddTransaction(userId, amount);
            Assert.IsNotNull(transaction, "Add transaction is returning a null object.");
            Assert.IsTrue(transaction.Amount == amount, "Added Transaction amount does not match.");
            Assert.IsTrue(transaction.UserId == userId, "Added Transaction user id does not match.");

            // Reversing the transaction ensures that it doesn't interfere with any other unit tests
            Transaction reverseTransaction = transactionRepository.AddTransaction(userId, amount * -1);
            Assert.IsNotNull(reverseTransaction, "Add transaction is returning a null object.");
            Assert.IsTrue(reverseTransaction.Amount == amount * -1, "Added Transaction amount does not match.");
            Assert.IsTrue(reverseTransaction.UserId == userId, "Added Transaction user id does not match.");
        }
    }
}
