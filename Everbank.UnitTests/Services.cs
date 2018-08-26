using Microsoft.VisualStudio.TestTools.UnitTesting;
using Everbank.Repositories;
using Everbank.Repositories.Contracts;
using Everbank.Repositories.Utilities;
using Everbank.Service;
using Everbank.Service.Contracts;

namespace Everbank.UnitTests
{
    [TestClass]
    public class Services
    {
        [TestMethod]
        public void AuthenticateUser()
        {   
            string emailAddress = "  authTest@testing.com ";
            string password = "password123";
            string hashedPassword = UserUtilities.HashString("password123");
            UserRepository userRepository = new UserRepository();
            User newUser = userRepository.AddUser(emailAddress, hashedPassword, "AuthTester1");
            UserService UserService = new UserService();
            ServiceResponse response = UserService.AuthenticateUser(emailAddress, password);
            User authenticatedUser = response.ResponseObject as User;
            Assert.IsNotNull(authenticatedUser, "User was not successfully authenticated.");
            Assert.AreEqual(UserUtilities.ConformString(emailAddress), authenticatedUser.EmailAddress, "User's email address is not properly conformed for storage.");
            Assert.IsNull(response.Messages, "Error messages were returned from authentication method.");
        }

        [TestMethod]
        public void CreateUser()
        {
            string emailAddress = "  createUserTest@testing.com ";
            string password = "password123";
            string firstName = "CreateUserTester1";
            UserService userService = new UserService();
            ServiceResponse response = userService.CreateUser(emailAddress, password, firstName);
            User newUser = response.ResponseObject as User;
            Assert.IsNotNull(newUser, "Newly created user is null.");
            Assert.IsNull(response.Messages, "Error messages were returned from user creation method");
        }
    }
}