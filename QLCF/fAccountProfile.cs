using QLCF.DAO;
using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCF
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        internal Account LoginAccount
        {
            get
            {
                return loginAccount;
            }

            set
            {
                loginAccount = value;
                changeAccount(LoginAccount);
            }
        }
        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }

        void updateAccount()
        {
            string password = txtPass.Text;
            string displayName = txtDisplayName.Text;
            string userName = txtUseName.Text;
            string newPassword = txtNewPassword.Text;
            string reEnterPW = txtEnterRePassword.Text;

            if (!newPassword.Equals(reEnterPW))
            {
                MessageBox.Show("Xác nhận mật khẩu không trùng khớp. Vui lòng nhập lại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, password, newPassword, displayName))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (UpdateAccount != null)
                    {
                        UpdateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }
        private event EventHandler<AccountEvent> UpdateAccount;
        public event EventHandler<AccountEvent> updateAccount1
        {
            add { UpdateAccount += value; }
            remove { UpdateAccount += value; }
        }

        void changeAccount(Account acc)
        {
            txtUseName.Text = LoginAccount.UserName;
            txtDisplayName.Text = LoginAccount.DisplayName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateAccount();
        }
    }
    public class AccountEvent:EventArgs
    {
        private Account acc;

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
        public Account Acc
        {
            get
            {
                return acc;
            }

            set
            {
                acc = value;
            }
        }
    }
}
