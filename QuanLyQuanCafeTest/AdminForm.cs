using QuanLyQuanCafeTest.DAO;
using QuanLyQuanCafeTest.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafeTest
{
    public partial class AdminForm : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();

        public Account loginAccount;

        public AdminForm()
        {
            InitializeComponent();
            MyLoad();
        }

        void MyLoad()
        {
            foodDataGridView.DataSource = foodList;
            accountDataGridView.DataSource = accountList;
            categoryDataGridView.DataSource = categoryList;
            tableDataGridView.DataSource = tableList;

            LoadDateTimePicker();
            LoadListBillByDate(fromDateDateTimePicker.Value, toDateDateTimePicker.Value);
            LoadListFood();
            LoadAccount();
            LoadListCategory();
            LoadListTable();

            AddFoodBinding();
            AddAccountBinding();
            AddCategoryBinding();
            AddTableBinding();
            LoadCategoryIntoCombobox(foodCategoryComboBox);
        }

        void AddAccountBinding()
        {
            usernameTextBox.DataBindings.Add(new Binding("Text",  accountDataGridView.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            displayNameTextBox.DataBindings.Add(new Binding("Text", accountDataGridView.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            accountType.DataBindings.Add(new Binding("Value", accountDataGridView.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        private void foodTabPage_Click(object sender, EventArgs e)
        {

        }

        void LoadDateTimePicker()
        {
            DateTime today = DateTime.Now;
            fromDateDateTimePicker.Value = new DateTime(today.Year, today.Month, 1);
            toDateDateTimePicker.Value = fromDateDateTimePicker.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            billDataGridView.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
        }

        private void viewBillBtn_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(fromDateDateTimePicker.Value, toDateDateTimePicker.Value);
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void AddFoodBinding()
        {
            foodNameTextBox.DataBindings.Add(new Binding("Text", foodDataGridView.DataSource, "Name", true, DataSourceUpdateMode.Never));
            foodIDTextbox.DataBindings.Add(new Binding("Text", foodDataGridView.DataSource, "ID", true, DataSourceUpdateMode.Never));
            foodPriceNumericUpDown.DataBindings.Add(new Binding("Value", foodDataGridView.DataSource, "Price", true, DataSourceUpdateMode.Never));        
        }

        void AddCategoryBinding()
        {
            categoryNameTextBox.DataBindings.Add(new Binding("Text", categoryDataGridView.DataSource, "Name", true, DataSourceUpdateMode.Never));
            categoryIDTextBox.DataBindings.Add(new Binding("Text", categoryDataGridView.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }

        void AddTableBinding()
        {
            tableNameTextBox.DataBindings.Add(new Binding("Text", tableDataGridView.DataSource, "Name", true, DataSourceUpdateMode.Never));
            tableIDTextBox.DataBindings.Add(new Binding("Text", tableDataGridView.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTabelList();
        }

        private void showFoodBtn_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void foodIDTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (foodDataGridView.SelectedCells.Count > 0)
                {
                    int id = (int)foodDataGridView.SelectedCells[0].OwningRow.Cells["IDCategory"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    foodCategoryComboBox.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in foodCategoryComboBox.Items)
                    {
                        if (item.Id == category.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    foodCategoryComboBox.SelectedIndex = index;
                }
            }catch{ }
        }

        private void addFoodBtn_Click(object sender, EventArgs e)
        {
            string name = foodNameTextBox.Text;
            int categoryId = (foodCategoryComboBox.SelectedItem as Category).Id;
            float price = (float)foodPriceNumericUpDown.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryId, price))
            {
                MessageBox.Show("Thêm món thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListFood();

                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void editFoodBtn_Click(object sender, EventArgs e)
        {
            string name = foodNameTextBox.Text;
            int categoryId = (foodCategoryComboBox.SelectedItem as Category).Id;
            float price = (float)foodPriceNumericUpDown.Value;
            int id = Convert.ToInt32(foodIDTextbox.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryId, price))
            {
                MessageBox.Show("Sửa món thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListFood();

                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void deleteFoodBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(foodIDTextbox.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListFood();

                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add
            {
                insertTable += value;
            }

            remove
            {
                insertTable -= value;
            }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add
            {
                deleteTable += value;
            }

            remove
            {
                deleteTable -= value;
            }
        }

        private event EventHandler editTable;
        public event EventHandler EditTable
        {
            add
            {
                editTable += value;
            }

            remove
            {
                editTable -= value;
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }

        private void searchFoodBtn_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(searchFoodNameTextBox.Text);
        }

        private void showAccountBtn_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButtons.OK);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm tài khoản", "Thông báo", MessageBoxButtons.OK);
            }
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công", "Thông báo", MessageBoxButtons.OK);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa tài khoản", "Thông báo", MessageBoxButtons.OK);
            }
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Đây là tài khoản đang đăng nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công", "Thông báo", MessageBoxButtons.OK);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa tài khoản", "Thông báo", MessageBoxButtons.OK);
            }
        }

        void ResetPass(string username)
        {
            if (AccountDAO.Instance.ResetPass(username))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công", "Thông báo", MessageBoxButtons.OK);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi đặt lại mật khẩu tài khoản", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void addAccountBtn_Click(object sender, EventArgs e)
        {
            string userName = usernameTextBox.Text;
            string displayName = displayNameTextBox.Text;
            int type = (int)accountType.Value;
            AddAccount(userName, displayName, type);
        }

        private void editAccountBtn_Click(object sender, EventArgs e)
        {
            string userName = usernameTextBox.Text;
            string displayName = displayNameTextBox.Text;
            int type = (int)accountType.Value;
            EditAccount(userName, displayName, type);
        }

        private void deleteAccountBtn_Click(object sender, EventArgs e)
        {
            string userName = usernameTextBox.Text;
            DeleteAccount(userName);
        }

        private void resetPasswordBtn_Click(object sender, EventArgs e)
        {
            string userName = usernameTextBox.Text;
            ResetPass(userName);
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            //this.GetListBillByDateForReportTableAdapter.Fill(this.QuanLyQuanCafeTestDataSet.GetListBillByDateForReport, fromDateDateTimePicker.Value, toDateDateTimePicker.Value);
            //this.rpViewer.RefreshReport();
            //this.rpViewer.RefreshReport();
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txtPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDate(fromDateDateTimePicker.Value, toDateDateTimePicker.Value);
            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
            {
                lastPage++;
            }

            txtPageBill.Text = lastPage.ToString();
        }

        private void txtPageBill_TextChanged(object sender, EventArgs e)
        {
            billDataGridView.DataSource = BillDAO.Instance.GetListBillByDateAndPage(fromDateDateTimePicker.Value, toDateDateTimePicker.Value, Convert.ToInt32(txtPageBill.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            if (page > 1)
                page--;

            txtPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillByDate(fromDateDateTimePicker.Value, toDateDateTimePicker.Value);

            if (page <= sumRecord / 10)
                page++;

            txtPageBill.Text = page.ToString();
        }

        private void AdminForm_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'QuanLyQuanCafeTestDataSet.GetListBillByDate' table. You can move, or remove it, as needed.
            //this.GetListBillByDateTableAdapter.Fill(this.QuanLyQuanCafeTestDataSet.GetListBillByDate);
            //this.GetListBillByDateForReportTableAdapter.Fill(this.QuanLyQuanCafeTestDataSet.GetListBillByDateForReport, fromDateDateTimePicker.Value, toDateDateTimePicker.Value);
            //this.rpViewer.RefreshReport();
            //this.rpViewer.RefreshReport();
        }

        private void rpViewer_Load(object sender, EventArgs e)
        {

        }

        private void AdminForm_Load_2(object sender, EventArgs e)
        {

            //this.rpViewer.RefreshReport();
        }

        private void AdminForm_Load_3(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'QuanLyQuanCafeTestDataSet.GetListBillByDateForReport' table. You can move, or remove it, as needed.
            this.GetListBillByDateForReportTableAdapter.Fill(this.QuanLyQuanCafeTestDataSet.GetListBillByDateForReport, fromDateDateTimePicker.Value, toDateDateTimePicker.Value);

            //this.rpViewer.RefreshReport();
            this.rpViewer.RefreshReport();
        }

        private void showCategoryBtn_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void editCategoryBtn_Click(object sender, EventArgs e)
        {
            string name = categoryNameTextBox.Text;
            int id = Convert.ToInt32(categoryIDTextBox.Text);

            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListCategory();
                LoadListFood();
                LoadCategoryIntoCombobox(foodCategoryComboBox);
                //if (updateFood != null)
                //{
                //    updateFood(this, new EventArgs());
                //}
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void addCategoryBtn_Click(object sender, EventArgs e)
        {
            string name = categoryNameTextBox.Text;

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListCategory();
                LoadListFood();
                LoadCategoryIntoCombobox(foodCategoryComboBox);
                //if (insertFood != null)
                //{
                //    insertFood(this, new EventArgs());
                //}
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void deleteCategoryBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(categoryIDTextBox.Text);

            if (MessageBox.Show("Khi xóa danh mục sẽ xóa hết các món trong danh mục. Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa danh mục thành công", "Thông báo", MessageBoxButtons.OK);
                    LoadListCategory();
                    LoadListFood();
                    LoadCategoryIntoCombobox(foodCategoryComboBox);

                    //if (deleteFood != null)
                    //{
                    //    deleteFood(this, new EventArgs());
                    //}
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa danh mục", "Thông báo", MessageBoxButtons.OK);
                }
            }

            
        }

        private void showTableBtn_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void editTableBtn_Click(object sender, EventArgs e)
        {
            string name = tableNameTextBox.Text;
            int id = Convert.ToInt32(tableIDTextBox.Text);

            if (TableDAO.Instance.UpdateTable(id, name))
            {
                MessageBox.Show("Sửa bàn thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListTable();
                
                if (editTable != null)
                {
                    editTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void deleteTableBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tableIDTextBox.Text);
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListTable();

                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void addTableBtn_Click(object sender, EventArgs e)
        {
            string name = tableNameTextBox.Text;

            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListTable();
                
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void toDateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
