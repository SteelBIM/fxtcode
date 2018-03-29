using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    /// <summary>
    /// 联系人
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.LinkMen")]
    public class LinkMen : BaseTO
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
        private int _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _truename;
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename
        {
            get { return _truename; }
            set { _truename = value; }
        }
        private string _mobile;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        private string _telephone;
        /// <summary>
        /// 电话
        /// </summary>
        public string telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }
        private string _email;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _remark;
        /// <summary>
        /// 说明
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private bool? _valid =true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _responsibleobject;
        /// <summary>
        /// 负责项目
        /// </summary>
        public string responsibleobject
        {
            get { return _responsibleobject; }
            set { _responsibleobject = value; }
        }
    }

}
