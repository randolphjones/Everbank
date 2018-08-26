using System;
using System.Data;

namespace Everbank.Mocking
{
    public class ApplicationData
    {
        private static ApplicationData _applicationData = new ApplicationData();
        DataSet _dataSet = new DataSet();
        public DataSet DataSet
        {
            get {
                return _dataSet;
            }
        }
        DataTable _users;
        DataTable _transactions;
        ApplicationData ()
        {
            _users = _dataSet.Tables.Add("everbank_users");
            _users.Columns.Add("email_address");
            _users.Columns.Add("password");
            _users.Columns.Add("first_name");
            DataColumn primaryUserColumn = _users.Columns.Add("uid", typeof(int));
            _users.PrimaryKey = new DataColumn[] { primaryUserColumn };
            _transactions = _dataSet.Tables.Add("everbank_transactions");
            _transactions.Columns.Add("time");
            _transactions.Columns.Add("amount");
            _transactions.Columns.Add("user_id");
            DataColumn primaryTransactionColumn = _transactions.Columns.Add("uid", typeof(int));
            _transactions.PrimaryKey = new DataColumn[] { primaryTransactionColumn };
            FillData();
        }

        public static ApplicationData GetInstance()
        {
            return _applicationData;
        }

        private void FillData()
        {
            InsertUser("test@test.com", "EF92B778BAFE771E89245B89ECBC08A44A4E166C06659911881F383D4473E94F", "Tester1");
            InsertTransaction(1, 200, DateTime.Now);
            // TODO: Fill the DataSet from fixtures in a flat file
        }

        private void InsertUser(string emailAddress, string password, string firstName)
        {
            DataRow newRow = _users.NewRow();
            newRow["uid"] = _users.Rows.Count + 1;
            newRow["email_address"] = emailAddress;
            newRow["first_name"] = firstName;
            newRow["password"] = password;
            _users.Rows.Add(newRow);
        }
        private void InsertTransaction(int userId, decimal amount, DateTime time)
        {
            DataRow newRow = _transactions.NewRow();
            newRow["uid"] = _transactions.Rows.Count + 1;
            newRow["user_id"] = userId;
            newRow["amount"] = amount;
            newRow["time"] = time;
        }
    }
}