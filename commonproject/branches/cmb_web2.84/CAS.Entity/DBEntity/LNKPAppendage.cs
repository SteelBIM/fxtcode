using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;
//创建人:曾智磊,日期:2014-06-26
namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.LNK_P_Appendage")]
    public class LNKPAppendage : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _appendagecode;
        public int appendagecode
        {
            get { return _appendagecode; }
            set { _appendagecode = value; }
        }
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private decimal? _area;
        public decimal? area
        {
            get { return _area; }
            set { _area = value; }
        }
        private string _p_aname;
        public string p_aname
        {
            get { return _p_aname; }
            set { _p_aname = value; }
        }
        private bool _isinner;
        public bool isinner
        {
            get { return _isinner; }
            set { _isinner = value; }
        }
        private int? _cityid;
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _classcode;
        /// <summary>
        /// 配套等级
        /// </summary>
        public int? classcode
        {
            get { return _classcode; }
            set { _classcode = value; }
        }
        private decimal? _distance;
        /// <summary>
        /// 距离
        /// </summary>
        public decimal? distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
        private int? _unitcode;
        /// <summary>
        /// 距离单位
        /// </summary>
        public int? unitcode
        {
            get { return _unitcode; }
            set { _unitcode = value; }
        }
    }
}
