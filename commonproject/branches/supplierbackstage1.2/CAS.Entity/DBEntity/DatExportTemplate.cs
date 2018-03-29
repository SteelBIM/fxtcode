using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ExportTemplate")]
    public class DatExportTemplate : BaseTO
    {
        private int _etid;
        [SQLField("etid", EnumDBFieldUsage.PrimaryKey, true)]
        public int etid
        {
            get { return _etid; }
            set { _etid = value; }
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
    }
}