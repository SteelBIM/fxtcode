using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    public static class CompanyAreaManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CompanyAreaManager));
        public static SysData_CompanyArea GetByCompanyAreaName(string name,DataClass _db=null)
        {
            SysData_CompanyArea comArea = null;
            DataClass db = new DataClass(_db);
            string sql = string.Format("select top 1 * from SysData_CompanyArea with(nolock) where CompanyAreaName='{0}'", name);
            comArea = db.DB.ExecuteQuery<SysData_CompanyArea>(sql).FirstOrDefault();
            //comArea = db.DB.SysData_CompanyArea.Where(tbl => tbl.CompanyAreaName == name).FirstOrDefault();
            db.Connection_Close();
            db.Dispose();
            return comArea;
        }
        public static SysData_CompanyArea Insert(string name, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_CompanyArea comArea = new SysData_CompanyArea { CompanyAreaName = name };
            comArea=Insert(comArea, db);
            db.Connection_Close();
            db.Dispose();
            return comArea;
        }

        public static SysData_CompanyArea Insert(SysData_CompanyArea obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.SysData_CompanyArea_Insert(obj.CompanyAreaName, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
    }
}
