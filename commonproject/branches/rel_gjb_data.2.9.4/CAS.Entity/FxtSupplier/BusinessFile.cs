using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.BusinessFile")]
    public class BusinessFile : BaseTO
    {
        private int _id;
        /// <summary>
        /// id
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int? _businessid;
        /// <summary>
        /// 业务Id
        /// </summary>
        public int? businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private string _filepath;
        /// <summary>
        /// 附加路径
        /// </summary>
        public string filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        private string _filename;
        /// <summary>
        /// 附加名称
        /// </summary>
        public string filename
        {
            get { return _filename; }
            set { _filename = value; }
        }
        private bool? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int? _filetype;
        /// <summary>
        /// 附件类型(1:业务附件，2:业务提交附件，3：支付依据附件)
        /// </summary>
        public int? filetype
        {
            get { return _filetype; }
            set { _filetype = value; }
        }
    }

}
