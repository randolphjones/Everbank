using System;
using System.Data;
using Everbank.Mocking;
using Everbank.Repositories.Contracts;
using Everbank.Repositories.Utilities;

namespace Everbank.Repositories 
{
    public class UserRepository : BaseRepository
    {
        public UserRepository() : base()
        {

        }

        ///<summary>
        /// Gets a user record from the dataset by email address
        ///</summary>
        public User GetUser(string emailAddress)
        {
            return GetUserWithQuery($"email_address = '{UserUtilities.ConformString(emailAddress)}'");
        }

        ///<summary>
        /// Gets a user record from the dataset by email address
        ///</summary>
        public User GetUser(int userId)
        {
            return GetUserWithQuery($"uid = {userId}");
        }

        ///<summary>
        /// Gets a user record by Query
        ///</summary>
        private User GetUserWithQuery(string query)
        {
            try
            {
                DataTable users = ApplicationData.GetInstance().DataSet.Tables["everbank_users"];
                DataRow[] results = users.Select(query);
                if (results.Length > 0)
                {
                    DataRow row = results[0];
                    User user = new User() {
                        EmailAddress = row.Field<string>("email_address"),
                        FirstName = row.Field<string>("first_name"),
                        Id = row.Field<int>("uid"),
                        Password = row.Field<string>("password"),
                    };
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        /// Adds a new user to the DataSet
        ///</summary>
        public User AddUser(string emailAddress, string hashedPassword, string firstName)
        {
            try
            {
                string conformedEmail = UserUtilities.ConformString(emailAddress);
                DataTable users = ApplicationData.GetInstance().DataSet.Tables["everbank_users"];
                DataRow newRow = users.NewRow();
                newRow["uid"] = users.Rows.Count + 1;
                newRow["email_address"] = conformedEmail;
                newRow["first_name"] = firstName.Trim();
                newRow["password"] = hashedPassword;
                users.Rows.Add(newRow);
                User newUser = GetUser(conformedEmail);
                return newUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}