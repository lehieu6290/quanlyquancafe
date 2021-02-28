using QuanLyQuanCafeTest.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafeTest.DAO
{
    public class AccountDAO
    {
        private AccountDAO() { }

        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO();  return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }

        public bool Login(string userName, string password)
        {
            //byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            //byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);

            //string hashPass = "";
            //foreach (byte item in hashData)
            //{
            //    hashPass += item;
            //}

            string query = "exec Login @userName , @passWord";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[]{userName, password});

            return data.Rows.Count > 0;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Account where userName = '" + userName + "'");
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }

        public bool UpdateAccount(string userName, string displayName, string password, string newPassword)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec UpdateAccount @userName , @displayName , @password , @newPassword", new object[] { userName, displayName, password, newPassword });
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select userName, displayName, type from Account");
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("insert into Account (userName, displayName, type) values(N'{0}', N'{1}', {2})", userName, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string userName, string displayName, int type)
        {
            string query = string.Format("update Account set displayName = N'{1}', type = {2} where userName = N'{0}'", userName, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string userName)
        {
            string query = string.Format("delete Account where userName = N'{0}'", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool ResetPass(string username)
        {
            string query = string.Format("update Account set passWord = 0 where userName = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
