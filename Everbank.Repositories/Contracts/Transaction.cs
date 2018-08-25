using System;

namespace Everbank.Repositories.Contracts
{
    [Serializable]
    public class Transaction 
    {   
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
    }
}