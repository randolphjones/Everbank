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
    public class DashboardHelper
    {
        public DashboardHelper()
        {

        }
        private UserService userService = new UserService();
        private TransactionService transactionService = new TransactionService();
        private MessageHelper messageHelper = new MessageHelper();

        ///<summary>
        /// Brings together the presentation components needed to build the Dashboard View Model
        ///</summary>
        public DashboardModel BuildDashboardModel(User user, List<Message> messages)
        {
            ServiceResponse transactionsResponse = transactionService.GetTransactions(user.Id);
            messageHelper.AppendResponseMessages(messages, transactionsResponse);
            List<Transaction> transactions = transactionsResponse.ResponseObject as List<Transaction>;
            
            ServiceResponse balanceResponse = transactionService.GetAccountBalance(transactions);
            messageHelper.AppendResponseMessages(messages, balanceResponse);
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
