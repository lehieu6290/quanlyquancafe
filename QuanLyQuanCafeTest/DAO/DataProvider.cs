using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafeTest.DAO
{
    public class DataProvider
    {
        private DataProvider() { }

        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=QuanLyQuanCafeTest;Integrated Security=True";

        //Lay ra du lieu
        public DataTable ExecuteQuery(string query, object[] paramater = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (paramater != null)
                {
                    int i = 0;
                    string[] listParamater = query.Split(' ');
                    foreach (string item in listParamater)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connection.Close();
            }
            

            return data;
        }

        //Kiem tra so dong thanh cong
        public int ExecuteNonQuery(string query, object[] paramater = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (paramater != null)
                {
                    int i = 0;
                    string[] listParamater = query.Split(' ');
                    foreach (string item in listParamater)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }

            return data;
        }

        //Dem so dong lay ra
        public object ExecuteScalar(string query, object[] paramater = null)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (paramater != null)
                {
                    int i = 0;
                    string[] listParamater = query.Split(' ');
                    foreach (string item in listParamater)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();

                connection.Close();
            }

            return data;
        }
    }
}
