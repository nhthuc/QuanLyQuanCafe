using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    class MenuDAO
    {
        private static MenuDAO instance;
        internal static MenuDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new MenuDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private MenuDAO() { }

        public List<Menu> GetListMenuIdByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            string query = "SELECT f.name, bi.count, f.price, bi.count*f.price AS TotalPrice FROM dbo.Food AS f, dbo.Bill AS b, dbo.BillInfo AS bi WHERE b.id = bi.idBill AND bi.idFood = f.id AND b.Status = 0 AND b.idTable = " + id;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
