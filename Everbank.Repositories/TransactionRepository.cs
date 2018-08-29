using System;
using System.Data;
using System.Collections.Generic;
using Everbank.Mocking;
using Everbank.Repositories.Contracts;

namespace Everbank.Repositories
{
    public class TransactionRepository : BaseRepository
    {
        public TransactionRepository() : base()
        {

        }

        ///<summary>
        /// Get a Collection of transactions by user Id
        ///</summary>
        public List<Transaction> GetTransactions(int userId)
        {
            try
            {
                DataTable transactions = ApplicationData.GetInstance().DataSet.Tables["everbank_transactions"];
                DataRow[] results = transactions.Select($"user_id = {userId}");
                if (results.Length > 0)
                {
                    List<Transaction> transactionList = new List<Transaction>();
                    foreach (DataRow row in results)
                    {
                        Transaction transaction = new Transaction() {
                            Id = row.Field<int>("uid"),
                            Amount = row.Field<decimal>("amount"),
                            Time = row.Field<DateTime>("time"),
                            UserId = row.Field<int>("user_id")
                        };
                        transactionList.Add(transaction);
                    }
                    return transactionList;
                }
                else
                {
                    return new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        /// Gets a transaction by unique Id
        ///</summary>
        public Transaction GetTransaction(int uid)
        {
            try
            {
                DataTable transactions = ApplicationData.GetInstance().DataSet.Tables["everbank_transactions"];
                DataRow[] results = transactions.Select($"uid = {uid}");
                if (results.Length > 0)
                {
                    DataRow row = results[0];
                    Transaction transaction = new Transaction() {
                        Id = row.Field<int>("uid"),
                        Amount = row.Field<decimal>("amount"),
                        Time = row.Field<DateTime>("time"),
                        UserId = row.Field<int>("user_id")
                    };
                    return transaction;
                }
                else
                {
                    return null;
                }
            } 
            catch(Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        /// Adds a Transaction for a user and returns the new transaction
        ///</summary>
        public Transaction AddTransaction(int userId, decimal amount)
        {
            try
            {
                DataTable transactions = ApplicationData.GetInstance().DataSet.Tables["everbank_transactions"];
                DataRow newRow = transactions.NewRow();
                int uid = transactions.Rows.Count + 1;
                newRow["uid"] = uid;
                newRow["user_id"] = userId;
                newRow["amount"] = amount;
                newRow["time"] = DateTime.Now;
                transactions.Rows.Add(newRow);
                Transaction newTransaction = GetTransaction(uid);
                return newTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}