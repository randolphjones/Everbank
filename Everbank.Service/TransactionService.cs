using System;
using System.Collections.Generic;
using System.Linq;
using Everbank.Service.Contracts;
using Everbank.Repositories.Contracts;
using Everbank.Repositories;

namespace Everbank.Service
{
    public class TransactionService
    {
        TransactionRepository transactionRepository = new TransactionRepository();
        public ServiceResponse CreateTransaction(int userId, decimal amount)
        {
            if (amount == 0)
            {
                Message invalidAmountMessage = new Message() {
                    Text = "Please enter a valid amount.",
                    Type = MessageType.WARN,
                };
                return new ServiceResponse() {
                    Messages = new List<Message>() { invalidAmountMessage },
                };
            }
            try
            {
                ServiceResponse balanceResponse = GetAccountBalance(userId);
                decimal balance = (decimal)balanceResponse.ResponseObject;
                decimal newBalance = balance + amount;
                if (newBalance >= 0)
                {
                    Transaction newTransaction = transactionRepository.AddTransaction(userId, amount);
                    string actionType = amount > 0 ? "deposited" : "withdrawn";
                    Message successMessage = new Message() {
                        Text = $"You have successfully {actionType} ${Math.Abs(amount)}. Your new balance is ${newBalance}.",
                        Type = MessageType.SUCCESS,
                    };
                    return new ServiceResponse() {
                        ResponseObject = newTransaction,
                        Messages = new List<Message>() { successMessage },
                    };
                }
                else
                {
                    Message insufficientFundsMessage = new Message() {
                        Text = "You do not have sufficient funds to make this withdrawal. Please deposit more funds or withdraw a smaller amount.",
                        Type = MessageType.WARN,
                    };
                    return new ServiceResponse() {
                        Messages = new List<Message>() { insufficientFundsMessage },
                    };
                }
            }
            catch
            {
                Message errorMessage = new Message() {
                    Text = "There was an error with your transaction. Please try again. If the error continues then please contact us at 123-456-7890.",
                    Type = MessageType.ERROR,
                };
                return new ServiceResponse() {
                    Messages = new List<Message>() { errorMessage },
                };
            }
        }
        public ServiceResponse GetTransactions(int userId)
        {
            try
            {
                List<Transaction> transactions = transactionRepository.GetTransactions(userId);
                ServiceResponse serviceResponse = new ServiceResponse() {
                    ResponseObject = transactions
                };
                if (transactions.Count == 0)
                {
                    Message emptyLedgerMessage = new Message() {
                        Text = "You currently do not have any transactions. Begin by depositing some funds into your account.",
                        Type = MessageType.INFO,
                    };
                    serviceResponse.Messages = new List<Message>() { emptyLedgerMessage };
                }
                return serviceResponse;
            }
            catch
            {
                Message errorMessage = new Message() {
                    Text = "There was an error accessing your transaction history. Please try again. If the error continues then please contact us at 123-456-7890.",
                    Type = MessageType.ERROR,
                };
                return new ServiceResponse() {
                    Messages = new List<Message>() { errorMessage },
                };
            }
        }

        public ServiceResponse GetAccountBalance(int userId)
        {
            try
            {
                List<Transaction> transactions = transactionRepository.GetTransactions(userId);
                return GetAccountBalance(transactions);
            }
            catch
            {
                Message errorMessage = new Message() {
                    Text = "There was an error accessing your account balance. Please try again. If the error continues then please contact us at 123-456-7890.",
                    Type = MessageType.ERROR,
                };
                return new ServiceResponse() {
                    Messages = new List<Message>() { errorMessage },
                };
            }
        }

        public ServiceResponse GetAccountBalance(List<Transaction> transactions)
        {
            decimal total = transactions.Select(transaction => transaction.Amount).Sum();
            return new ServiceResponse() {
                ResponseObject = total,
            };
        }
    }
}
