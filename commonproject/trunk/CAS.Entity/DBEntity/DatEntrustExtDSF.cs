using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Entrust_ExtDSF")]
    public class DatEntrustExtDSF : BaseTO
    {
        private long _etid;
        [SQLField("etid", EnumDBFieldUsage.PrimaryKey, true)]
        public long etid
        {
            get { return _etid; }
            set { _etid = value; }
        }
        private long _entrustid;
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private string _externalstatetext;
        /// <summary>
        /// 外部状态
        /// </summary>
        public string externalstatetext
        {
            get { return _externalstatetext; }
            set { _externalstatetext = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private DateTime _changedon;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime changedon
        {
            get { return _changedon; }
            set { _changedon = value; }
        }

        private string _fileids;
        /// <summary>
        /// 附件ID集合,逗号分割
        /// </summary>
        public string fileids
        {
            get { return _fileids; }
            set { _fileids = value; }
        }
        private int? _biztype;
        /// <summary>
        /// 业务节点 2018
        /// </summary>
        public int? biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private bool? _iscomplate;
        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool? iscomplate
        {
            get { return _iscomplate; }
            set { _iscomplate = value; }
        }

        /// <summary>
        /// 预评、报告状态
        /// </summary>
        [SQLReadOnly]
        public int businessStateCode { get; set; }
    }
}