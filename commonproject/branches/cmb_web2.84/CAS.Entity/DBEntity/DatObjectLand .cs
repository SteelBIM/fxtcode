using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_Land")]
    public class DatObjectLand : BaseTO
    {
        private long _objectid;
        /// <summary>
        /// 土地基础委估对象ID
        /// </summary>
        [SQLField("objectid", EnumDBFieldUsage.PrimaryKey)]
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
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
        private string _fieldno = "";
        /// <summary>
        /// 宗地号
        /// </summary>
        public string fieldno
        {
            get { return _fieldno; }
            set { _fieldno = value; }
        }
        private string _landpurpose = "";
        /// <summary>
        /// 土地主要用途1001
        /// </summary>
        public string landpurpose
        {
            get { return _landpurpose; }
            set { _landpurpose = value; }
        }
        private DateTime? _landregisterdata;
        /// <summary>
        /// 土地登记日期
        /// </summary>
        public DateTime? landregisterdata
        {
            get { return _landregisterdata; }
            set { _landregisterdata = value; }
        }
        private DateTime? _landenddata;
        /// <summary>
        /// 土地使用截止日期
        /// </summary>
        public DateTime? landenddata
        {
            get { return _landenddata; }
            set { _landenddata = value; }
        }
        private string _statutorypurpose = "";
        /// <summary>
        /// 规划用途
        /// </summary>
        public string statutorypurpose
        {
            get { return _statutorypurpose; }
            set { _statutorypurpose = value; }
        }
        private decimal _landarea = 0M;
        /// <summary>
        /// 土地面积
        /// </summary>
        public decimal landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
        private string _certno = "";
        /// <summary>
        /// 土地使用证编号
        /// </summary>
        public string certno
        {
            get { return _certno; }
            set { _certno = value; }
        }
        private string _electronno = "";
        /// <summary>
        /// 电子监管号
        /// </summary>
        public string electronno
        {
            get { return _electronno; }
            set { _electronno = value; }
        }
        private decimal _cubagerate = 0M;
        /// <summary>
        /// 现状容积率
        /// </summary>
        public decimal cubagerate
        {
            get { return _cubagerate; }
            set { _cubagerate = value; }
        }
        private decimal _maxcubagerate = 0M;
        /// <summary>
        /// 规划最大容积率
        /// </summary>
        public decimal maxcubagerate
        {
            get { return _maxcubagerate; }
            set { _maxcubagerate = value; }
        }
        private decimal _mincubagerate = 0M;
        /// <summary>
        /// 规划最小容积率
        /// </summary>
        public decimal mincubagerate
        {
            get { return _mincubagerate; }
            set { _mincubagerate = value; }
        }
        private string _developdegree;
        /// <summary>
        /// 土地开发程度
        /// </summary>
        public string developdegree
        {
            get { return _developdegree; }
            set { _developdegree = value; }
        }
        private string _planlimit = "";
        /// <summary>
        /// 规划限制
        /// </summary>
        public string planlimit
        {
            get { return _planlimit; }
            set { _planlimit = value; }
        }
        private string _fieldname;
        /// <summary>
        /// 宗地名称
        /// </summary>
        public string fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        private string _landsourcecode;
        /// <summary>
        /// 土地来源
        /// </summary>
        public string landsourcecode
        {
            get { return _landsourcecode; }
            set { _landsourcecode = value; }
        }
        private string _landrighttypecode;
        /// <summary>
        /// 土地产权(国有、集体、个人)
        /// </summary>
        public string landrighttypecode
        {
            get { return _landrighttypecode; }
            set { _landrighttypecode = value; }
        }
        private decimal? _coverrate;
        /// <summary>
        /// 地上建筑物覆盖率
        /// </summary>
        public decimal? coverrate
        {
            get { return _coverrate; }
            set { _coverrate = value; }
        }
        private string _buildingdetail;
        /// <summary>
        /// 地上附着物描述
        /// </summary>
        public string buildingdetail
        {
            get { return _buildingdetail; }
            set { _buildingdetail = value; }
        }
        private decimal? _buildingarea1;
        /// <summary>
        /// 地上附着物总建筑面积
        /// </summary>
        public decimal? buildingarea1
        {
            get { return _buildingarea1; }
            set { _buildingarea1 = value; }
        }
        private decimal _subhousealltotalprice = 0;
        /// <summary>
        /// 所有附属房屋总价
        /// </summary>
        public decimal subhousealltotalprice
        {
            get { return _subhousealltotalprice; }
            set { _subhousealltotalprice = value; }
        }
    }
}
