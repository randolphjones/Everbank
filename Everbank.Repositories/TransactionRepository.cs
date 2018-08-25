using System;
using System.Data;
using System.Collections.Generic;
using Everbank.Mocking;

namespace Everbank.Repositories
{
    public class TransactionRepository : BaseRepository
    {
        public TransactionRepository() : base()
        {

        }

        public List<Contracts.Transaction> GetTransactions()
        {
            throw new NotImplementedException();
        }
        public bool AddTransaction()
        {
            throw new NotImplementedException();
        }
    }
}