using System;
using System.Data;
using Everbank.Mocking;

namespace Everbank.Repositories 
{
    public class UserRepository : BaseRepository
    {
        public UserRepository() : base()
        {

        }

        public Contracts.User GetUser()
        {
            throw new NotImplementedException();
        }

        public bool AddUser()
        {
            throw new NotImplementedException();
        }
    }
}