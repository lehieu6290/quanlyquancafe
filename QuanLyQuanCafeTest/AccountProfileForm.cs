using QuanLyQuanCafeTest.DAO;
using QuanLyQuanCafeTest.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafeTest
{
    public partial class AccountProfileForm : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(LoginAccount); }
        }

        public AccountProfileForm(Account loginAccount)
        {
            InitializeComponent();

            this.LoginAccount = loginAccount;
        }

        void ChangeAccount(Account loginAccount)
        {
            userNameTextbox.Text = loginAccount.UserName;
            displayNameTextBox.Text = loginAccount.DisplayName;

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add
            {
                updateAccount += value;
            }
            remove
            {
                updateAccount -= value;
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateAccountInfo()
        {
            string displayName = displayNameTextBox.Text;
            string password = passwordTextBox.Text;
            string newPass = newPasswordTextBox.Text;
            string reNewPass = reEnterPasswordTextBox.Text;
            string userName = userNameTextbox.Text;

            if (!newPass.Equals(reNewPass))
            {
                MessageBox.Show("Mật khẩu không trùng khớp", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, password, newPass))
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK);
                    if (updateAccount != null)
                    {
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                    }
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
