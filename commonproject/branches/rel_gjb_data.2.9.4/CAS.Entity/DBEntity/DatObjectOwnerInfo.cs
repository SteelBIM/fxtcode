using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_OwnerInfo")]
    public class DatObjectOwnerInfo : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long? _objectid;
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long? objectid
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
        private string _owneridtype;
        /// <summary>
        /// 产权人身份证所属地区
        /// </summary>
        public string owneridtype
        {
            get { return _owneridtype; }
            set { _owneridtype = value; }
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
        private string _ownerphone;
        /// <summary>
        /// 产权人电话
        /// </summary>
        public string ownerphone
        {
            get { return _ownerphone; }
            set { _ownerphone = value; }
        }
        private string _ownership;
        /// <summary>
        /// 所有权比例
        /// </summary>
        public string ownership
        {
            get { return _ownership; }
            set { _ownership = value; }
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
        private string _ownerregistrationno;
        /// <summary>
        /// 产权人工商登记号
        /// </summary>
        public string ownerregistrationno
        {
            get { return _ownerregistrationno; }
            set { _ownerregistrationno = value; }
        }
        private string _owneraddress;
        /// <summary>
        /// 产权人地址
        /// </summary>
        public string owneraddress
        {
            get { return _owneraddress; }
            set { _owneraddress = value; }
        }
        private string _ownerlegal;
        /// <summary>
        /// 产权人法人代表
        /// </summary>
        public string ownerlegal
        {
            get { return _ownerlegal; }
            set { _ownerlegal = value; }
        }
        private int? _valid;
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _newaddflag;
        public string newaddflag
        {
            get { return _newaddflag; }
            set { _newaddflag = value; }
        }
    }

}
