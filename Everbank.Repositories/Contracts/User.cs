using System;

namespace Everbank.Repositories.Contracts
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}