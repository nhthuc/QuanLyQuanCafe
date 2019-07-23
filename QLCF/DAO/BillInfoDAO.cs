using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        internal static BillInfoDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new BillInfoDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private BillInfoDAO() { }

        public void DeleteBillInfoByIdFood(int id)
        {
            DataProvider.Instance.ExcuteQuery("DELETE dbo.BillInfo WHERE idFood = " + id);
        }
        public List<BillInfo> GetlistBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExcuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill = " + id);

            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExcuteNonQuery("USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
    }
}
