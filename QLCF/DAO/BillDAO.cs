using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    //nhiệm vụ của class lấy bill từ id table
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new BillDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private BillDAO() { }
        
        public int GetUncheckBillIdByTable(int id)
        {
            DataTable data = DataProvider.Instance.ExcuteQuery("SELECT * FROM dbo.Bill WHERE idTable = "+ id +" AND Status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
                //thất bại trả về -1 ... thành công trả về bill ID
            }
            return -1;
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "UPDATE dbo.Bill SET DateCheckOut = GETDATE(), Status = 1, "+ "discount = " + discount + ", totalPrice = " + totalPrice + " WHERE id = " + id;
            DataProvider.Instance.ExcuteNonQuery(query);
        }

        public DataTable GetBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExcuteQuery("USP_GetListDateByDate @checkIn, @checkOut", new object[] { checkIn, checkOut });
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExcuteNonQuery("USP_InsertBill @idTable", new object[] { id });
        }

        public int GetMaxIdBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExcuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {

                return 1;
            }
        }
    }
}
