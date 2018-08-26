using Microsoft.VisualStudio.TestTools.UnitTesting;
using Everbank.Repositories;
using Everbank.Repositories.Contracts;
using Everbank.Repositories.Utilities;

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
    }
}
