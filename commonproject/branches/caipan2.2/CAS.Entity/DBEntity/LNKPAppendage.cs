using System;
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
        private string _appendagecodename;
        [SQLReadOnly]
        public string appendagecodename
        {
            get { return _appendagecodename; }
            set { _appendagecodename = value; }
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
        private string _isinnername;
        [SQLReadOnly]
        public string isinnername
        {
            get { return _isinnername; }
            set { _isinnername = value; }
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
        private string _classcodename;
        [SQLReadOnly]
        public string classcodename
        {
            get { return _classcodename; }
            set { _classcodename = value; }
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

        private int? _peitaoid;
        /// <summary>
        /// 配套
        /// </summary>
        public int? peitaoid
        {
            get { return _peitaoid; }
            set { _peitaoid = value; }
        }

        private int? _schoolid;
        /// <summary>
        /// 学校
        /// </summary>
        public int? schoolid
        {
            get { return _schoolid; }
            set { _schoolid = value; }
        }

        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        [SQLReadOnly]
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }

        private decimal? _x;
        /// <summary>
        /// x
        /// </summary>
        [SQLReadOnly]
        public decimal? x
        {
            get { return _x; }
            set { _x = value; }
        }

        private decimal? _y;
        /// <summary>
        /// y
        /// </summary>
        [SQLReadOnly]
        public decimal? y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
