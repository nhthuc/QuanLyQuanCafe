using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    class CategoryDAO
    {
        private static CategoryDAO instance;
        internal static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new CategoryDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private CategoryDAO() { }
        public List<Category> GetlistCategory()
        {
            List<Category> listCategory = new List<Category>();

            DataTable data = DataProvider.Instance.ExcuteQuery("SELECT * FROM dbo.FoodCategory");

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }

            return listCategory;
        }

        public Category GetCategoryById(int id)
        {
            Category category = null;

            string query = "SELECT * FROM dbo.FoodCategory WHERE id = " + id;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;
        }
    }
}
