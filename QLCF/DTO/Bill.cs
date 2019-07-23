using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DTO
{
    class Bill
    {
        public Bill(int id, DateTime? checkIn, DateTime? checkOut, int status, int discount = 0)
        {
            this.Id = id;
            this.CheckIn = checkIn;
            this.CheckOut = checkOut;
            this.Status = status;
            this.Discount = discount;
        }

        
        private int status;
        private DateTime? checkOut;
        private DateTime? checkIn;
        private int id;
        private int discount;

        public Bill(DataRow row)
        {
            // DateTime? cho phép null
            this.Id = (int)row["id"];
            this.CheckIn = (DateTime?)row["DateCheckIn"];
            //cách giải quyết khi lấy ra một ô từ bảng có bị null hay không
            DateTime? dataCheckOutTemp;
            try
            {
                dataCheckOutTemp = (DateTime?)row["DateCheckOut"];
            }
            catch (Exception)
            {
                dataCheckOutTemp = null;
            }
                
            if (dataCheckOutTemp.ToString() != "" )
                this.CheckOut = dataCheckOutTemp;

            this.Status = (int)row["Status"];
            this.Discount = (int)row["discount"];
        }
        public DateTime? CheckIn
        {
            get
            {
                return checkIn;
            }

            set
            {
                checkIn = value;
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

        public DateTime? CheckOut
        {
            get
            {
                return checkOut;
            }

            set
            {
                checkOut = value;
            }
        }

        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public int Discount
        {
            get
            {
                return discount;
            }

            set
            {
                discount = value;
            }
        }
    }
}
