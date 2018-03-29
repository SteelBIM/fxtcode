using System;
using CAS.Entity.BaseDAModels;
namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_OwnerInfo_BONAFinancial")]
    public class DatObjectOwnerInfoBONAFinancial : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _objectid;
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private string _owner;
        /// <summary>
        /// 产权人
        /// </summary>
        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        private string _ownerphone;
        /// <summary>
        /// 产权人电话
        /// </summary>
        public string ownerphone
        {
            get { return _ownerphone; }
            set { _ownerphone = value; }
        }
        private string _owneridcard;
        /// <summary>
        /// 产权人身份证
        /// </summary>
        public string owneridcard
        {
            get { return _owneridcard; }
            set { _owneridcard = value; }
        }
        private string _ownercity;
        /// <summary>
        /// 所在城市
        /// </summary>
        public string ownercity
        {
            get { return _ownercity; }
            set { _ownercity = value; }
        }

        private string _ownerregioer;
        /// <summary>
        /// 产权人户籍
        /// </summary>
        public string ownerregioer
        {
            get { return _ownerregioer; }
            set { _ownerregioer = value; }
        }
        private string _ismarried;
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string ismarried
        {
            get { return _ismarried; }
            set { _ismarried = value; }
        }
        private string _ishavechildren;
        /// <summary>
        /// 有无子女
        /// </summary>
        public string ishavechildren
        {
            get { return _ishavechildren; }
            set { _ishavechildren = value; }
        }
        private string _owneridtype;
        /// <summary>
        /// 证件类型
        /// </summary>
        public string owneridtype
        {
            get { return _owneridtype; }
            set { _owneridtype = value; }
        }


        private int? _valid;
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
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
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }

        private string _ownership;
        /// <summary>
        /// 产权人所占比例
        /// </summary>
        public string ownership
        {
            get { return _ownership; }
            set { _ownership = value; }
        }
        
    }

}
