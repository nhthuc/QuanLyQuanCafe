using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        internal static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new AccountDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
     private AccountDAO() { }

        public bool Login(string userName, string passWord)
        {
            string query = "EXEC dbo.USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExcuteQuery(query, new object[] {userName, passWord});

            return result.Rows.Count > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExcuteQuery("SELECT UserName,DisplayName,Type FROM dbo.Account ");
        }
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExcuteQuery("SELECT * FROM dbo.Account WHERE UserName = '" + userName +"'");
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public bool UpdateAccount(string userName, string passWord, string newPass, string displayName)
        {
            int result = DataProvider.Instance.ExcuteNonQuery("USP_UpdateAccount @userName , @displayName , @passWord , @newPassWord", new object[] {userName, displayName, passWord, newPass });
            return result > 0;
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("INSERT INTO dbo.Account( UserName, DisplayName, Type ) VALUES ( N'{0}', N'{1}', {2})", userName, displayName, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }

        public bool EditAccount(string userName, string displayName, int type)
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{1}', Type = {2} WHERE UserName = N'{0}'", userName, displayName, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }

        public bool DELETEAccount(string userName)
        {
            
            string query = string.Format("DELETE dbo.Account WHERE UserName = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }
        public bool ResetPass(string userName)
        {
            string query = string.Format("UPDATE dbo.Account SET Password = N'0' WHERE UserName = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }
    }
}
