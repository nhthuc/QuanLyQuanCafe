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
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }
        bool check = false;
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string userName = txtUseName.Text;
            string passWord = txtPassWord.Text;
            if (Login(userName, passWord))
            {
                Account loginAcc = AccountDAO.Instance.GetAccountByUserName(userName);
                fTableManager f = new fTableManager(loginAcc);
                this.Hide(); //ẩn form đăng nhập hiện tại
                f.ShowDialog();
                this.Show(); //hiện form đăng nhập lại khi tắt chương trình 
            }
            else
            {
                MessageBox.Show("sai tài khoản hoặc mật khẩu !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassWord.Focus();
            }
            
        }
        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName,passWord);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự thoát chương trình hay không?","Thông báo", MessageBoxButtons.OKCancel , MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void txtUseName_Leave(object sender, EventArgs e)
        {
            if (txtUseName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập Tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUseName.Focus();
            }
        }

        private void txtPassWord_Leave(object sender, EventArgs e)
        {
            if (txtPassWord.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassWord.Focus();
            }
        }
    }
}
