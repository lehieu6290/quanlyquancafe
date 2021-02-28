using QuanLyQuanCafeTest.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafeTest.DAO
{
    public class MenuDAO
    {
        private MenuDAO() { }

        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { MenuDAO.instance = value; }
        }

        public List<Menu> GetMenuByTableId(int id)
        {
            List<Menu> menuList = new List<Menu>();

            string query = "select f.name, bi.count, f.price, f.price * bi.count as totalPrice from BillInfo as bi, Bill as b, Food as f where bi.idBill = b.id and bi.idFood = f.id and b.status = 0 and b.idTable = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                menuList.Add(menu);
            }

            return menuList;
        }
    }
}
