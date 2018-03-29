using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    /// <summary>
    /// 业务受理
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Business")]
    public class Business : BaseTO
    {
        private int _businessid;
        /// <summary>
        /// 业务ID
        /// </summary>
        [SQLField("businessid", EnumDBFieldUsage.PrimaryKey, true)]
        public int businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private int? _service;
        /// <summary>
        /// 服务类型
        /// </summary>
        public int? service
        {
            get { return _service; }
            set { _service = value; }
        }
        private string _projectname;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _customercompanyfullname;
        /// <summary>
        /// 委估机构名称
        /// </summary>
        public string customercompanyfullname
        {
            get { return _customercompanyfullname; }
            set { _customercompanyfullname = value; }
        }
        private string _remark;
        /// <summary>
        /// 需求描述
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _valid;
        /// <summary>
        /// 业务状态：0-撤销；1-有效；2-结束
        /// </summary>
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int? _businessscopetype;
        /// <summary>
        /// 业务范围类型。1:全国供应商都可以受理；2：整个城市；3：具体某供应商受理
        /// </summary>
        public int? businessscopetype
        {
            get { return _businessscopetype; }
            set { _businessscopetype = value; }
        }
        private string _clientname;
        /// <summary>
        /// 委托方联系人
        /// </summary>
        public string clientname
        {
            get { return _clientname; }
            set { _clientname = value; }
        }
        private string _clientcontact;
        /// <summary>
        /// 委托方联系人联系方式
        /// </summary>
        public string clientcontact
        {
            get { return _clientcontact; }
            set { _clientcontact = value; }
        }
        private string _clientemail;
        /// <summary>
        /// 委托方业务联系人的邮箱
        /// </summary>
        public string clientemail
        {
            get { return _clientemail; }
            set { _clientemail = value; }
        }
        private DateTime? _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _createusername;
        /// <summary>
        /// 业务委托人
        /// </summary>
        public string createusername
        {
            get { return _createusername; }
            set { _createusername = value; }
        }
    }


}
