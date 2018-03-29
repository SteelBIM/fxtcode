using System;
using CAS.Entity.BaseDAModels;
namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_FileUploadSetting")]
    public class SYSFileUploadSetting : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _objecttypecode;
        /// <summary>
        /// 报告阶段
        /// </summary>
        public string objecttypecode
        {
            get { return _objecttypecode; }
            set { _objecttypecode = value; }
        }
        private string _objecttypetext;
        /// <summary>
        /// 报告阶段名称
        /// </summary>
        public string objecttypetext
        {
            get { return _objecttypetext; }
            set { _objecttypetext = value; }
        }
        private string _reporttypecode;
        /// <summary>
        /// 报型类型
        /// </summary>
        public string reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _reporttypetext;
        /// <summary>
        /// 报型类型名称
        /// </summary>
        public string reporttypetext
        {
            get { return _reporttypetext; }
            set { _reporttypetext = value; }
        }
        private string _filetypecode;
        /// <summary>
        /// 附件类型
        /// </summary>
        public string filetypecode
        {
            get { return _filetypecode; }
            set { _filetypecode = value; }
        }
        private string _filetypetext;
        /// <summary>
        /// 附件类型名称
        /// </summary>
        public string filetypetext
        {
            get { return _filetypetext; }
            set { _filetypetext = value; }
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
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
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
    }

}
