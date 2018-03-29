using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.DAL.DB
{
    public class DataClass
    {
        private bool ExistParent
        {
            get;
            set;
        }
        public DataClassesDataContext DB
        {
            get;
            set;
        }
        public DataClass(DataClass dc = null)
        {
            if (dc == null)
            {
                this.DB = new  DataClassesDataContext();
                this.ExistParent = false;
            }
            else
            {
                this.DB = dc.DB;
                this.ExistParent = true;
            }
        }
        public DataClass(DataClassesDataContext db)
        {
            if (db == null)
            {
                this.DB = new DataClassesDataContext();
                this.ExistParent = false;
            }
            else
            {
                this.DB = db;
                this.ExistParent = true;
            }
        }

        public void SubmitChanges()
        {
            if (!this.ExistParent)
            {
                this.DB.SubmitChanges();
            }
        }

        /// <summary>
        /// 摘要:
        ///  关闭与数据库的连接。这是关闭任何打开连接的首选方法。
        /// 异常:
        ///   System.Data.Common.DbException:
        ///     在打开连接时出现连接级别的错误。
        /// </summary>
        public void Connection_Close()
        {
            if (!this.ExistParent)
            {
                this.DB.Connection.Close();
            }
        }
        /// <summary>
        /// 摘要:
        ///     释放由 System.Data.Linq.DataContext 使用的所有资源。
        /// </summary>
        public void Dispose()
        {
            if (!this.ExistParent)
            {
                this.DB.Dispose();
            }
        }
    }
}
