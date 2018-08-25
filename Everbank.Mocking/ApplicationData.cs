using System;
using System.Data;

namespace Everbank.Mocking
{
    public class ApplicationData
    {
        ApplicationData _applicationData = new ApplicationData();
        DataSet _dataSet = new DataSet();
        public DataSet DataSet
        {
            get {
                return _dataSet;
            }
        }
        DataTable _users;
        DataTable _transactions;
        private ApplicationData ()
        {
            _users = _dataSet.Tables.Add("everbank_users");
            _users.Columns.Add("email_address");
            _users.Columns.Add("password");
            _users.Columns.Add("first_name");
            _transactions = _dataSet.Tables.Add("everbank_transactions");
            _transactions.Columns.Add("time");
            _transactions.Columns.Add("amount");
            _transactions.Columns.Add("user_id");
            FillData();
        }

        public ApplicationData GetInstance()
        {
            return _applicationData;
        }

        private void FillData()
        {
            // TODO: Fill the DataSet from fixtures in a flat file
            InsertUser("test@test.com", "Tester1", "password123");
            InsertTransaction(1, 200, DateTime.Now);
            throw new NotImplementedException();
        }

        private void InsertUser(string emailAddress, string password, string firstName)
        {
            DataRow newRow = _users.NewRow();
            newRow["email_address"] = emailAddress;
            newRow["first_name"] = firstName;
            newRow["password"] = password;
            _users.Rows.Add(newRow);
        }
        private void InsertTransaction(int userId, decimal amount, DateTime time)
        {
            DataRow newRow = _transactions.NewRow();
            newRow["user_id"] = userId;
            newRow["amount"] = amount;
            newRow["time"] = time;
        }
    }
}