using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;


namespace FxtSpider.Bll
{
    public static class CompanyManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CompanyManager));
        public static SysData_Company GetByCompanyName(string name, DataClass _db = null)
        {
            SysData_Company com = null;
            DataClass db = new DataClass(_db);
            string sql = string.Format("select top 1 * from SysData_Company with(nolock) where CompanyName='{0}'", name);
            com = db.DB.ExecuteQuery<SysData_Company>(sql).FirstOrDefault();
            //com = db.DB.SysData_Company.Where(tbl => tbl.CompanyName == name).FirstOrDefault();
            db.Connection_Close();
            db.Dispose();
            return com;
        }
        public static SysData_Company Insert(string name, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_Company com = new SysData_Company { CompanyName = name };
            com = Insert(com, db);
            db.Connection_Close();
            db.Dispose();
            return com;
        }

        public static SysData_Company Insert(SysData_Company obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.SysData_Company_Insert(obj.CompanyName, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
    }
}
