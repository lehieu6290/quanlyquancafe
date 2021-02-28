using QuanLyQuanCafeTest.DAO;
using QuanLyQuanCafeTest.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafeTest
{
    public partial class TableManagerForm : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public TableManagerForm(Account loginAccount)
        {
            InitializeComponent();

            this.LoginAccount = loginAccount;

            LoadTable();
            LoadCategory();
            LoadCompoBoxTable(switchTableComboBox);
        }

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> categoeyList = CategoryDAO.Instance.GetListCategory();
            categoryComboBox.DataSource = categoeyList;
            categoryComboBox.DisplayMember = "Name";
        }

        void LoadFoodByCategoryId(int id)
        {
            List<Food> foodList = FoodDAO.Instance.GetFoodByCategoryId(id);
            foodComboBox.DataSource = foodList;
            foodComboBox.DisplayMember = "Name";
        }

        private void LoadTable()
        {
            tableFlowLayoutPanel.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTabelList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.tableWidth, Height = TableDAO.tableHeight };
                btn.Text = item.Name + "\n\n" + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Peru;
                        break;
                    default:
                        btn.BackColor = Color.GreenYellow;
                        break;
                }
                tableFlowLayoutPanel.Controls.Add(btn);
            }
        }

        void ShowBill(int idTable)
        {
            billListView.Items.Clear();
            List<QuanLyQuanCafeTest.DTO.Menu> billInfoList = MenuDAO.Instance.GetMenuByTableId(idTable);
            float totalPrice = 0;
            foreach (QuanLyQuanCafeTest.DTO.Menu item in billInfoList)
            {
                ListViewItem list = new ListViewItem(item.FoodName.ToString());
                list.SubItems.Add(item.Count.ToString());
                list.SubItems.Add(item.Price.ToString());
                list.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                billListView.Items.Add(list);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            totalPriceTextBox.Text = totalPrice.ToString("c", culture);
        }

        #region Events
        private void btn_Click(object sender, EventArgs e)
        {
            int idTable = ((sender as Button).Tag as Table).ID;
            billListView.Tag = (sender as Button).Tag;
            ShowBill(idTable);
        }

        private void TableManagerForm_Load(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfileForm f = new AccountProfileForm(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName  + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminForm f = new AdminForm();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.EditTable += f_EditTable;
            f.InsertTable += f_InsertTable;
            f.DeleteTable += f_DeleteTable;
            f.ShowDialog();
        }

        void f_DeleteTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        void f_InsertTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        void f_EditTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodByCategoryId((categoryComboBox.SelectedItem as Category).Id);
            if (billListView.Tag != null)
            {
                ShowBill((billListView.Tag as Table).ID);
            }
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodByCategoryId((categoryComboBox.SelectedItem as Category).Id);
            if (billListView.Tag != null)
            {
                ShowBill((billListView.Tag as Table).ID);
            }
            LoadTable();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodByCategoryId((categoryComboBox.SelectedItem as Category).Id);

            if (billListView.Tag != null)
            {
                ShowBill((billListView.Tag as Table).ID);
            }
            
        }
        #endregion

        private void categoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cd = sender as ComboBox;

            if (cd.SelectedItem == null)
                return;

            Category selected = cd.SelectedItem as Category;
            id = selected.Id;

            LoadFoodByCategoryId(id);
        }

        private void AddFoodBtn_Click(object sender, EventArgs e)
        {
            Table table = billListView.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Chọn bàn để thêm món", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIdByTableId(table.ID);
            int idFood = (foodComboBox.SelectedItem as Food).Id;
            int count = (int)foodCountNumericUpDown.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                idBill = BillDAO.Instance.GetMaxBillId();
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }

            ShowBill(table.ID);
            LoadTable();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void checkOutBtn_Click(object sender, EventArgs e)
        {
            Table table = billListView.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Chọn bàn để thanh toán", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIdByTableId(table.ID);
            int discount = (int)disCountNumericUpDown.Value;
            double totalPrice = Convert.ToDouble(totalPriceTextBox.Text.Split(',')[0]) * 1000;
            double finalPrice = totalPrice - (totalPrice * discount / 100);

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho {0}\nTổng tiền: {1}\nGiảm giá: {2}\nTổng tiền sau khi giảm giá: {3}", table.Name, totalPrice, totalPrice * discount / 100, finalPrice), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }

        public void LoadCompoBoxTable(ComboBox cb){
            cb.DataSource = TableDAO.Instance.LoadTabelList();
            cb.DisplayMember = "Name";
        }

        private void switchTableBtn_Click(object sender, EventArgs e)
        {
            
            if (billListView.Tag == null)
            {
                MessageBox.Show("Mời chọn bàn để chuyển", "Thông báo", MessageBoxButtons.OK);
                return;
                
            }

            int id1 = (billListView.Tag as Table).ID;
            int id2 = (switchTableComboBox.SelectedItem as Table).ID;

            if ((switchTableComboBox.SelectedItem as Table).Status.Equals("Có người"))
            {
                MessageBox.Show("Phải chọn bàn trống", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show(string.Format("Bạn có chắc chuyển từ {0} qua {1}", (billListView.Tag as Table).Name, (switchTableComboBox.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkOutBtn_Click(this, new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFoodBtn_Click(this, new EventArgs());
        }
    }
}
