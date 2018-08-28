using System;
using System.Collections.Generic;
using Everbank.Repositories.Contracts;

namespace Everbank.Web.Models
{
    public class DashboardModel : PageModel
    {
        public User User { get; set; }
        public decimal AccountBalance { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}