using System;
using CAS.Entity.BaseDAModels;

namespace GJB.Entity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Appraisers")]
    public class DatAppraisers : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _reportid;
        /// <summary>
        /// 报告编号
        /// </summary>
        public long reportid
        {
            get { return _reportid; }
            set { _reportid = value; }
        }
        private int _userid;
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _usernumber;
        /// <summary>
        /// 注册号
        /// </summary>
        public string usernumber
        {
            get { return _usernumber; }
            set { _usernumber = value; }
        }
        private int _biztype;
        /// <summary>
        /// 2018005、2018006
        /// </summary>
        public int biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int _appraisertype;
        /// <summary>
        /// 估价师类型
        /// </summary>
        public int appraisertype
        {
            get { return _appraisertype; }
            set { _appraisertype = value; }
        }
        private int _valid;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _certtypecode;
        /// <summary>
        /// 证书类型
        /// </summary>
        public int certtypecode
        {
            get { return _certtypecode; }
            set { _certtypecode = value; }
        }

        private string _newaddflag;
        public string newaddflag
        {
            get { return _newaddflag; }
            set { _newaddflag = value; }
        }
    }
}