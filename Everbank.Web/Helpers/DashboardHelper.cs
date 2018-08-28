using System;
using System.Linq;
using System.Collections.Generic;
using Everbank.Web.Helpers;
using Everbank.Repositories.Contracts;
using Everbank.Service.Contracts;
using Everbank.Service;
using Everbank.Web.Models;

namespace Everbank.Web.Helpers
{
    public static class DashboardHelper
    {
        private static UserService userService = new UserService();
        private static TransactionService transactionService = new TransactionService();

        public static DashboardModel BuildDashboardModel(User user, List<Message> messages)
        {
            ServiceResponse transactionsResponse = transactionService.GetTransactions(user.Id);
            MessageHelper.AppendResponseMessages(messages, transactionsResponse);
            List<Transaction> transactions = transactionsResponse.ResponseObject as List<Transaction>;
            
            ServiceResponse balanceResponse = transactionService.GetAccountBalance(transactions);
            MessageHelper.AppendResponseMessages(messages, balanceResponse);
            decimal balance = (decimal)balanceResponse.ResponseObject;
            
            DashboardModel dashboardModel = new DashboardModel() {
                User = user,
                Transactions = transactions.OrderByDescending(transaction => transaction.Time).ToList(),
                AccountBalance = balance,
                Messages = messages,
            };
            return dashboardModel;
        }
    }
}
