using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [Table("dbo.Dat_DataBaseExportTemplate")]
    public class DatDataBaseExportTemplate : BaseTO
    {
        private int _dbetid;
        [SQLField("dbetid", EnumDBFieldUsage.PrimaryKey, true)]
        public int dbetid
        {
            get { return _dbetid; }
            set { _dbetid = value; }
        }
        private string _name;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _databasecolumn;
        /// <summary>
        /// 列名
        /// </summary>
        public string databasecolumn
        {
            get { return _databasecolumn; }
            set { _databasecolumn = value; }
        }
        private int _createduserid;
        /// <summary>
        /// 创建用户
        /// </summary>
        public int createduserid
        {
            get { return _createduserid; }
            set { _createduserid = value; }
        }
        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int? _typecode;
        /// <summary>
        /// 数据主体类型(报告、预评、询价、基础数据等)
        /// </summary>
        public int? typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private int? _subtypecode;
        /// <summary>
        /// 数据主体子类型(主要用于typecode为基础数据时下的住宅等子类型)
        /// </summary>
        public int? subtypecode
        {
            get { return _subtypecode; }
            set { _subtypecode = value; }
        }
    }

}
