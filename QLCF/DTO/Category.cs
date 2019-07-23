using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DTO
{
    class Category
    {
        string name;
        int id;

        public Category(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
        }
        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
}
