using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    class FoodDAO
    {
        private static FoodDAO instance;
        internal static FoodDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new FoodDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private FoodDAO() { }
        public List<Food> GetlistFoodByCategoryId(int id)
        {
            List<Food> list = new List<Food>();

            string query = "SELECT * FROM dbo.Food WHERE idCategory = " + id;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }
        public List<Food> GetlistFood()
        {
            List<Food> list = new List<Food>();

            string query = "SELECT * FROM dbo.Food ";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        
        public DataTable SearchFoodByName(string name)
        {
            string query = string.Format("SELECT a.id AS [ID], a.name AS [Tên món ăn],b.name AS [Danh mục],b.id AS [id Danh mục], a.price AS [Giá] FROM dbo.Food AS a, dbo.FoodCategory AS b WHERE a.idCategory = b.id AND dbo.fuConvertToUnsign1(a.name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public DataTable GetListFood()
        {
            string query = "SELECT a.id AS [ID], a.name AS [Tên món ăn],b.name AS [Danh mục],b.id AS [id Danh mục], a.price AS [Giá] FROM dbo.Food AS a, dbo.FoodCategory AS b WHERE a.idCategory = b.id";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT INTO dbo.Food( name, idCategory, price ) VALUES ( N'{0}', {1}, {2})" ,name, id, price);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("UPDATE dbo.Food SET name = N'{0}', idCategory =  {1}, price = {2} WHERE id = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }

        public bool DELETEFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByIdFood(idFood);
            string query = string.Format("DELETE dbo.Food WHERE id = " + idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }
    }
}
