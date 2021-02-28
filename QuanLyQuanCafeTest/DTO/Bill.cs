using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafeTest.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime dateCheckIn, DateTime dateCheckOut, int status, int discount) 
        {
            this.id = id;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.status = status;
            this.discount = discount;
        }

        public Bill(DataRow row)
        {
            this.id = (int)row["id"];
            this.dateCheckIn = (DateTime?)row["dateCheckIn"];
            if (row["dateCheckOut"].ToString() != "")
                this.dateCheckOut = (DateTime?)row["dateCheckOut"];
            this.status = (int)row["status"];

            if(row["discount"].ToString() != "")
                this.Discount = (int)row["discount"];
        }

        private int status;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private DateTime? dateCheckOut;

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }

        private DateTime? dateCheckIn;

        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int discount;

        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }
    }
}
