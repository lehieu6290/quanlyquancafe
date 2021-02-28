using QuanLyQuanCafeTest.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafeTest.DAO
{
    public class BillInfoDAO
    {
        private BillInfoDAO() { }

        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        public List<BillInfo> GetListBillInfo(int idBill)
        {
            List<BillInfo> billInfoList = new List<BillInfo>();

            string query = "select * from BillInfo where idBill = " + idBill;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                BillInfo billInfo = new BillInfo(item);
                billInfoList.Add(billInfo);
            }

            return billInfoList;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("exec InsertBillInfo @idBill , @idFood , @count ", new object[] { idBill, idFood, count });
        }

        public void DeleteBillInfoByFoodId(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete BillInfo where idFood = " + id);
        }
    }
}
