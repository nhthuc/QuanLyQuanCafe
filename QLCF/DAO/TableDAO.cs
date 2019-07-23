using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    class TableDAO
    {
        private static TableDAO instance;
        internal static TableDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new TableDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private TableDAO() { }

        public static int tableWidth = 85;
        public static int tableHeight = 85;

        public List<Table> LoadListTable()
        {
            List<Table> tableList = new List<Table>();
            DataTable data = DataProvider.Instance.ExcuteQuery("USP_TableLoad");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }
    }
}
