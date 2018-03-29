using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Data
{
    public class TransactionHelper
    {
        private bool ExistParent
        {
            get;
            set;
        }
        public ITransaction Tr
        {
            get;
            set;
        }
        public TransactionHelper(MSSQLDBDAL DB,ITransaction tr = null)
        {
            if (tr == null)
            {
                this.Tr = DB.BeginTransaction();
                this.ExistParent = false;
            }
            else
            {
                this.Tr = tr;
                this.ExistParent = true;
            }
        }

        public void Commit()
        {
            if (!this.ExistParent)
            {
                this.Tr.Commit();
                this.Tr.Dispose();
            }
        }
        public void Rollback()
        {
            if (!this.ExistParent)
            {
                this.Tr.Rollback();
                this.Tr.Dispose();
            }
        }
    }
}
