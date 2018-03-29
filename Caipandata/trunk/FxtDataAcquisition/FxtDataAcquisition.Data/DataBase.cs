using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Data
{
    public class DataBase
    {
        private bool ExistParent
        {
            get;
            set;
        }
        public MSSQLDBDAL DB
        {
            get;
            set;
        }
        public DataBase(DataBase db = null)
        {
            if (db == null)
            {
                this.DB = new MSSQLDBDAL();
                this.ExistParent = false;
            }
            else
            {
                this.DB = db.DB;
                this.ExistParent = true;
            }
        }
        public void Close()
        {
            if (!this.ExistParent)
            {
                this.ExistParent = true;
                this.DB.Close();
            }
        }
    }
}
