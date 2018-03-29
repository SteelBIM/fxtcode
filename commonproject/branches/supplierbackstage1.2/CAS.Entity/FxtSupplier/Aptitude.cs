using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    /// <summary>
    /// 资质
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Aptitude")]
    public class Aptitude : BaseTO
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
        private int? _companyid;
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _aptitudetype;
        /// <summary>
        /// 资质类型
        /// </summary>
        public int aptitudetype
        {
            get { return _aptitudetype; }
            set { _aptitudetype = value; }
        }
        private string _aptitudename;
        /// <summary>
        /// 资质名称 
        /// </summary>
        public string aptitudename
        {
            get { return _aptitudename; }
            set { _aptitudename = value; }
        }
        private string _aptitudefile;
        /// <summary>
        /// 资质附件 
        /// </summary>
        public string aptitudefile
        {
            get { return _aptitudefile; }
            set { _aptitudefile = value; }
        }
        private int? _aptitudegrade;
        /// <summary>
        /// 资质等级
        /// </summary>
        public int? aptitudegrade
        {
            get { return _aptitudegrade; }
            set { _aptitudegrade = value; }
        }
        private string _gradename;
        /// <summary>
        /// 资质等级名称 
        /// </summary>
        public string gradename
        {
            get { return _gradename; }
            set { _gradename = value; }
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
        private bool? _valid=true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

}
