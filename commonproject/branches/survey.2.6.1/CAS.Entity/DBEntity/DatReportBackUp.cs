using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Report_BackUp")]
    public class DatReportBackUp : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _reportid;
        /// <summary>
        /// 报告Id
        /// </summary>
        public long reportid
        {
            get { return _reportid; }
            set { _reportid = value; }
        }
        private bool? _isbackup;
        /// <summary>
        /// 是否已归档
        /// </summary>
        public bool? isbackup
        {
            get { return _isbackup; }
            set { _isbackup = value; }
        }
        private string _backupcontent;
        /// <summary>
        /// 归档内容
        /// </summary>
        public string backupcontent
        {
            get { return _backupcontent; }
            set { _backupcontent = value; }
        }
        private int? _backupuserid;
        /// <summary>
        /// 归档人
        /// </summary>
        public int? backupuserid
        {
            get { return _backupuserid; }
            set { _backupuserid = value; }
        }
        private DateTime? _backupdate;
        /// <summary>
        /// 归档日期
        /// </summary>
        public DateTime? backupdate
        {
            get { return _backupdate; }
            set { _backupdate = value; }
        }
        private DateTime _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool _isdatacomplete;
        /// <summary>
        /// 归档资料已齐全
        /// </summary>
        public bool isdatacomplete
        {
            get { return _isdatacomplete; }
            set { _isdatacomplete = value; }
        }

        private int? _totalpages;
        /// <summary>
        /// 归档人
        /// </summary>
        public int? totalpages
        {
            get { return _totalpages; }
            set { _totalpages = value; }
        }
    }
}
