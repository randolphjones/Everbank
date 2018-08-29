using System;
using System.Text;
using System.Collections.Generic;
using Everbank.Repositories;
using Everbank.Repositories.Contracts;
using Everbank.Repositories.Utilities;
using Everbank.Service.Contracts;
using System.Security.Cryptography;

namespace Everbank.Service
{
    public class UserService
    {
        UserRepository userRepository = new UserRepository();
        ///<summary>
        /// Attempts to Authenticate the user and if successful, returns the user's information
        ///</summary>
        public ServiceResponse AuthenticateUser(string emailAddress, string password)
        {
            try
            {
                User user = userRepository.GetUser(emailAddress);
                if (user != null)
                {
                    string conformedPassword = UserUtilities.ConformString(password);
                    string hashedPassword = UserUtilities.HashString(conformedPassword);
                    if (hashedPassword == user.Password)
                    {
                        return new ServiceResponse() {
                            ResponseObject = user,
                        };
                    }
                }
                Message errorMessage = new Message() {
                    Text = "The email address or password that you provided were incorrect. Please try again.",
                    Type = MessageType.ERROR,
                };
                return new ServiceResponse() {
                    Messages = new List<Message>() { errorMessage },
                };
            }
            catch
            {
                Message errorMessage = new Message() {
                    Text = "There was an error accessing your user profile. Please try again. If the error continues then please contact us at 123-456-7890.",
                    Type = MessageType.ERROR,
                };
                return new ServiceResponse() {
                    Messages = new List<Message>() { errorMessage },
                };
            }
        }

        ///<summary>
        /// Attempts to create a user and if successful, returns the new user
        ///</summary>
        public ServiceResponse CreateUser(string emailAddress, string password, string firstName)
        {
            bool isPasswordComplex = UserUtilities.CheckPasswordComplexity(password);
            if (!isPasswordComplex)
            {
                Message errorMessage = new Message() {
                    Text = "Please choose a password that is 8 of more characters and contains at least one letter and one number.",
                    Type = MessageType.WARN,
                };
                return new ServiceResponse() {
                    Messages = new List<Message> { errorMessage },
                };
            }

            string conformedEmail = UserUtilities.ConformString(emailAddress);
            bool isEmailValid = UserUtilities.CheckEmailValidity(conformedEmail);
            if (!isEmailValid)
            {
                Message errorMessage = new Message() {
                    Text = "Please supply a valid email address.",
                    Type = MessageType.WARN,
                };
                return new ServiceResponse() {
                    Messages = new List<Message> { errorMessage },
                };
            }

            try {
                UserRepository userRepository = new UserRepository();
                User existingUser = userRepository.GetUser(emailAddress);
                if (existingUser == null)
                {
                    string conformedPassword = UserUtilities.ConformString(password);
                    string hashedPassword = UserUtilities.HashString(conformedPassword);
                    User newUser = userRepository.AddUser(emailAddress, hashedPassword, firstName);
                    return new ServiceResponse() {
                        ResponseObject = newUser,
                    };
                }
                else
                {
                    Message errorMessage = new Message() {
                        Text = "There is an existing account at this email address. Please try logging in with these credentials.",
                        Type = MessageType.WARN,
                    };
                    return new ServiceResponse() {
                        Messages = new List<Message>() { errorMessage },
                    };
                }
            }
            catch
            {
                Message errorMessage = new Message() {
                    Text = "There was an error creating your profile. Please try again. If the error continues then please contact us at 123-456-7890.",
                    Type = MessageType.ERROR,
                };
                return new ServiceResponse() {
                    Messages = new List<Message>() { errorMessage },
                };
            }
        }
    }
}
