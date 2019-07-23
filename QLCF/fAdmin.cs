using QLCF.DAO;
using QLCF.DTO;
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

namespace QLCF
{
    public partial class fAdmin : Form
    {
        BindingSource FoodList = new BindingSource(); // tránh mất kết nối khi load lại Binding
        BindingSource AccountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            dgvShowFood.DataSource = FoodList;
            dgvAccount.DataSource = AccountList;

            LoadFood();
            LoadDate();
            LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
            AddfoodBinding();
            LoadCategoryIntoCmb(cmbFoodCategory);
            AddAccountBinding();
            LoadAccount();
        }
        #region methods
        void AddAccountBinding()
        {
            txtAccountUseName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtAccountDisplyName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccType.DataBindings.Add(new Binding("Value", dgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            AccountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDate()
        {
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvBill.DataSource = BillDAO.Instance.GetBillByDate(checkIn, checkOut);
        }

        void LoadFood()
        {
            
            FoodList.DataSource = FoodDAO.Instance.GetListFood();
            //FoodList.DataSource = DataProvider.Instance.ExcuteQuery(query);
            //dgvShowFood.Columns["ID"].Width = 25; // thiết lập chiều rộng của cột
            dgvShowFood.Columns["id Danh mục"].Visible= false; // ẩn cột id danh mục
        }
        void AddfoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dgvShowFood.DataSource, "Tên món ăn",true,DataSourceUpdateMode.Never)); // lỗi binding load food khi chỉnh sửa bên combobox
            txtFoodID.DataBindings.Add(new Binding("Text", dgvShowFood.DataSource, "ID", true, DataSourceUpdateMode.Never)); // 1 luồng từ list view 
            nmFoodPrice.DataBindings.Add(new Binding("value", dgvShowFood.DataSource, "Giá", true, DataSourceUpdateMode.Never));
            
        }
        void LoadCategoryIntoCmb(ComboBox cmb)
        {
            cmb.DataSource = CategoryDAO.Instance.GetlistCategory();
            cmb.DisplayMember = "Name"; // hiển thị trường tên

        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Bạn đã thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Bạn đã thêm tài khoản thất bại");
            }
            LoadAccount();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.EditAccount(userName, displayName, type))
            {
                MessageBox.Show("Bạn đã chỉnh sửa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Bạn đã chỉnh sửa tài khoản thất bại");
            }
            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xóa tài khoản khi đang đang nhập");
                return;
            }
            if (AccountDAO.Instance.DELETEAccount(userName))
            {
                MessageBox.Show("Bạn đã Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Bạn đã xóa tài khoản thất bại");
            }
            LoadAccount();
        }
        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPass(userName))
            {
                MessageBox.Show("Bạn đã phục hồi mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Bạn đã phục hồi mật khẩu thất bại");
            }
        }
        #endregion

        #region events
        private void btnBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadFood();
        }

        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            //Sự kiện FoodId thay đổi thì lấy IdCategory từ DataGridView để cập nhật lại cmbCategory
            if (dgvShowFood.SelectedCells.Count > 0)
            {
                int id = (int)dgvShowFood.SelectedCells[0].OwningRow.Cells["id Danh mục"].Value;

                Category category = CategoryDAO.Instance.GetCategoryById(id);
                cmbFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;
                foreach (Category item in cmbFoodCategory.Items)
                {
                    if (item.Id == category.Id)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
                cmbFoodCategory.SelectedIndex = index;
            }

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string foodName = txtFoodName.Text;
            int categoryId = (cmbFoodCategory.SelectedItem as Category).Id; // lấy id từ combobox
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(foodName, categoryId, price))
            {
                MessageBox.Show("Bạn đã thêm món thành công", "Thành công");
                LoadFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs()); // gọi lại event mới tạo
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm món ăn");
            }
        }

        private void btnUpdateFood_Click(object sender, EventArgs e)
        {
            string foodName = txtFoodName.Text;
            int categoryId = (cmbFoodCategory.SelectedItem as Category).Id; // lấy id từ combobox
            float price = (float)nmFoodPrice.Value;
            int idFood = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(idFood, foodName, categoryId, price))
            {
                MessageBox.Show("Bạn đã sửa món thành công", "Thành công");
                LoadFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Lỗi khi Sửa món ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.DELETEFood(idFood))
            {
                MessageBox.Show("Bạn đã xóa món thành công", "Thành công");
                LoadFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Lỗi khi xóa món ăn");
            }
        }

        //tạo event để load lại bàn ăn
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood += value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood += value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood += value; }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FoodList.DataSource = FoodDAO.Instance.SearchFoodByName(txtSearchNameFood.Text);
            dgvShowFood.Columns["id Danh mục"].Visible = false;
        }

        #endregion

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtAccountUseName.Text;
            string displayName = txtAccountDisplyName.Text;
            int type = (int)nmAccType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtAccountUseName.Text;
            DeleteAccount(userName);
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            string userName = txtAccountUseName.Text;
            string displayName = txtAccountDisplyName.Text;
            int type = (int)nmAccType.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txtAccountUseName.Text;
            ResetPass(userName);
        }
    }
}
