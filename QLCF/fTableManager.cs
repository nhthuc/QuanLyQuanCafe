using QLCF.DAO;
using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCF
{
    public partial class fTableManager : Form
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
                changeAccount(LoginAccount.Type);
            }
        }
        
        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            LoadTable();
            LoadCategory();
        }

        #region Method
        void changeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetlistCategory();
            cmbCategory.DataSource = listCategory;
            cmbCategory.DisplayMember = "Name"; // hiển thị trường nào trong list
        }
        void LoadFoodisbyCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetlistFoodByCategoryId(id);
            cmbFood.DataSource = listFood;
            cmbFood.DisplayMember = "Name";
        }

        
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadListTable();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.tableWidth, Height = TableDAO.tableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status; // hiển thị tên bàn và status
                

                btn.Click += Btn_Click; // sự kiện khi nhấn vào bàn
                btn.Tag = item; // lấy id table
                if (item.Status == "Trống")
                {
                    btn.BackColor = Color.DeepPink;
                }
                else
                {
                    btn.BackColor = Color.BlueViolet;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void showBill(int id)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillIdInfo = MenuDAO.Instance.GetListMenuIdByTable(id);
            float totalPrice = 0;
            foreach (DTO.Menu item in listBillIdInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());

                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            //chuyển đổi đơn vị tiền tệ
            CultureInfo culture = new CultureInfo("vi-VN");
            txtTotalPrice.Text = totalPrice.ToString("c",culture);

        }
        #endregion

        #region Event
        private void Btn_Click(object sender, EventArgs e)
        {
            string status = ((sender as Button).Tag as Table).Status;
            if (status == "Trống")
                mnDisCount.ResetText();

            int tableId = ((sender as Button).Tag as Table).Id; // lấy id khi nhấn vào button thông qua thẻ tag
            lsvBill.Tag = (sender as Button).Tag;// thẻ tag để biết listView của bàn nào và để truyền idTable đi
            lbTenHoaDon.Text = "Hóa đơn bàn " + tableId.ToString();
            showBill(tableId);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.updateAccount1 += F_updateAccount1; //truyền dữ liệu từ form con qua form cha
            f.ShowDialog();
        }

        private void F_updateAccount1(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;// truyền tải khoản đang đăng nhập qua form admin
            // load lại khi nhấn vào thêm xóa sửa
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodisbyCategoryID((cmbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).Id);
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodisbyCategoryID((cmbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).Id);
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodisbyCategoryID((cmbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).Id);
        }
        #endregion

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cmb = sender as ComboBox;

            if (cmb.SelectedItem == null)
            {
                return;
            }

            Category selected = cmb.SelectedItem as Category;
            id = selected.Id;

            LoadFoodisbyCategoryID(id);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn");
                return;
            }
            //MessageBox.Show(table.Id.ToString());
            int foodId = (cmbFood.SelectedItem as Food).Id;
            int count = (int)nmFoodCount.Value;
            int idBill = BillDAO.Instance.GetUncheckBillIdByTable(table.Id);
            //MessageBox.Show(idBill.ToString());
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.Id);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), foodId, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodId, count);
            }
            showBill(table.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table; // lấy id table ở bàn hiện tại

            int idBill = BillDAO.Instance.GetUncheckBillIdByTable(table.Id);
            int discount = (int)mnDisCount.Value;
            double totalPrice = (Convert.ToDouble(txtTotalPrice.Text.Split(',')[0]) * 1000);
            double finalTotalprice = totalPrice - (totalPrice/100)*discount; 

            if (idBill != 1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc chắn muốn thanh toán bàn {0}\n Tổng tiền =Tổng tiền - (Tổng tiền / 100) x giảm giá\n => {1} - ({1} / 100) x {2} = {3}",table.Id,totalPrice,discount,finalTotalprice  ) +" hay không?", "Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalprice);
                    showBill(table.Id);
                    LoadTable();
                }
            }
        }
    }
}
