using QuanLyQuanCafeTest.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafeTest.DAO
{
    public class BillDAO
    {
        private BillDAO() { }

        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        /// <summary>
        /// Thành công: bill ID
        /// Thất bại: -1
        /// </summary>
        /// <param name="idTable"></param>
        /// <returns></returns>
        public int GetUncheckBillIdByTableId(int idTable)
        {
            string query = "select * from Bill where idTable = " + idTable + " AND status = 0";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }
            else
            {
                return -1;
            }
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "update Bill set dateCheckOut = GETDATE(), status = 1, " + "discount = " + discount + ", totalPrice = " + totalPrice +" where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec InsertBill @idTable", new object[] { id });
        }

        public DataTable GetListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec GetListBillByDate @checkIn , @checkOut", new object[]{checkIn, checkOut});
        }

        public int GetMaxBillId()
        {
            try
            {
                string query = "select Max(id) from Bill";
                return (int)DataProvider.Instance.ExecuteScalar(query);
            }
            catch
            {
                return 1;
            }
        }

        public DataTable GetListBillByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return DataProvider.Instance.ExecuteQuery("exec GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut, pageNum });
        }

        public int GetNumBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("exec GetNumBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
    }
}
