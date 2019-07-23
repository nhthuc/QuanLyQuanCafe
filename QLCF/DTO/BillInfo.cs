using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DTO
{
    public class BillInfo
    {
        private int count;
        private int foodId;
        private int billId;
        private int id;

        public BillInfo (int id, int billId, int foodId, int count)
        {
            this.Id = id;
            this.BillId = billId;
            this.FoodId = foodId;
            this.Count = count;
        }

        public BillInfo (DataRow row)
        {
            this.Id = (int)row["id"];
            this.BillId = (int)row["idBill"];
            this.FoodId = (int)row["idFood"];
            this.Count = (int)row["count"];
        }
        public int BillId
        {
            get
            {
                return billId;
            }

            set
            {
                billId = value;
            }
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

        public int FoodId
        {
            get
            {
                return foodId;
            }

            set
            {
                foodId = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
            }
        }
    }
}
