using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    public class DataProvider
    {
        private static DataProvider instance; // ctr R E 
         
        private string strConn = @"Data Source=DESKTOP-009B755\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";

        internal static DataProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataProvider();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private DataProvider() { }
        public DataTable ExcuteQuery(string query, object[] parameter = null)
        {

            DataTable data = new DataTable();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);

                if (parameter != null)
                {
                    query = query.Replace(',', ' ');
                    string[] listPara = query.Split(' ');
                    int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                command.Parameters.AddWithValue(item.Trim(), parameter[i]);
                                i++;
                            }
                        }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                conn.Close();
            }
            return data;
        }
        public int ExcuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);

                if (parameter != null)
                {
                    query = query.Replace(',', ' ');
                    string[] listPara = query.Split(' ');
                    
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();
                conn.Close();
            }
            return data;
        }
        public object ExcuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);

                if (parameter != null)
                {
                    query = query.Replace(',', ' ');
                    string[] listPara = query.Split(' ');


                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();
                conn.Close();
            }
            return data;
        }

    }
}
