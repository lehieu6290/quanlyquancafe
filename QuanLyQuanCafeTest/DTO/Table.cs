using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafeTest.DTO
{
    public class Table
    {
        public Table(int id, string name, string status)
        {
            this.id = id;
            this.name = name;
            this.status = status;
        }

        public Table(DataRow row)
        {
            this.id = (int)row["id"];
            this.name = row["name"].ToString();
            this.status = row["status"].ToString();
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
