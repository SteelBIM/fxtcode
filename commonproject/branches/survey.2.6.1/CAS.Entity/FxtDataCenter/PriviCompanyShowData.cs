using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter
{
    [Serializable]
    [TableAttribute("FxtDataCenter.dbo.Privi_Company_ShowData ")]
    public class PriviCompanyShowData : BaseTO
    {
        private long _id;
        /// <summary>
        /// 评估机构之间的数据关系表
        /// </summary>
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _fxtcompanyid;
        [SQLField("fxtcompanyid", EnumDBFieldUsage.PrimaryKey)]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _cityid;
        [SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _showcompanyid;
        /// <summary>
        /// 本机构可应用数据的机构ID，例如：25,101,102
        /// </summary>
        public string showcompanyid
        {
            get { return _showcompanyid; }
            set { _showcompanyid = value; }
        }
        private int _typecode = 0;
        /// <summary>
        /// 应用类型，暂时未定
        /// </summary>
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _casecompanyid;
        public string casecompanyid
        {
            get { return _casecompanyid; }
            set { _casecompanyid = value; }
        }
        private string _bizcompanyid;
        /// <summary>
        /// 商业基础数据
        /// </summary>
        public string bizcompanyid
        {
            get { return _bizcompanyid; }
            set { _bizcompanyid = value; }
        }
        private string _bizcasecompanyid;
        /// <summary>
        /// 商业案例数据
        /// </summary>
        public string bizcasecompanyid
        {
            get { return _bizcasecompanyid; }
            set { _bizcasecompanyid = value; }
        }
        private string _officecompanyid;
        /// <summary>
        /// 办公基础数据
        /// </summary>
        public string officecompanyid
        {
            get { return _officecompanyid; }
            set { _officecompanyid = value; }
        }
        private string _officecasecompanyid;
        /// <summary>
        /// 办公案例数据
        /// </summary>
        public string officecasecompanyid
        {
            get { return _officecasecompanyid; }
            set { _officecasecompanyid = value; }
        }
        private string _landcompanyid;
        /// <summary>
        /// 土地基础数据
        /// </summary>
        public string landcompanyid
        {
            get { return _landcompanyid; }
            set { _landcompanyid = value; }
        }
        private string _landcasecompanyid;
        /// <summary>
        /// 土地案例数据
        /// </summary>
        public string landcasecompanyid
        {
            get { return _landcasecompanyid; }
            set { _landcasecompanyid = value; }
        }
        private string _industrycompanyid;
        /// <summary>
        /// 工业基础数据
        /// </summary>
        public string industrycompanyid
        {
            get { return _industrycompanyid; }
            set { _industrycompanyid = value; }
        }
        private string _industrycasecompanyid;
        /// <summary>
        /// 工业案例数据
        /// </summary>
        public string industrycasecompanyid
        {
            get { return _industrycasecompanyid; }
            set { _industrycasecompanyid = value; }
        }
        private string _stampcompanyid;
        /// <summary>
        /// VQ盖章数据
        /// </summary>
        public string stampcompanyid
        {
            get { return _stampcompanyid; }
            set { _stampcompanyid = value; }
        }

        //公司名称
        public string CompanyName { get; set; }


    }
}
