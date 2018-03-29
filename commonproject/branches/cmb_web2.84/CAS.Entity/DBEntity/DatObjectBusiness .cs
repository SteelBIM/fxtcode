using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_Business")]
    public class DatObjectBusiness : BaseTO
    {
        private long _objectid;
        /// <summary>
        /// 商业基础委估对象ID
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
        private string _projectname = "";
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _buildingname = "";
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string buildingname
        {
            get { return _buildingname; }
            set { _buildingname = value; }
        }
        private string _floornumber = "";
        /// <summary>
        /// 楼层
        /// </summary>
        public string floornumber
        {
            get { return _floornumber; }
            set { _floornumber = value; }
        }
        private string _housename = "";
        /// <summary>
        /// 房号名称
        /// </summary>
        public string housename
        {
            get { return _housename; }
            set { _housename = value; }
        }
        private int _projectid = 0;
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int _buildingid = 0;
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public int buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private int _houseid = 0;
        /// <summary>
        /// 房号ID
        /// </summary>
        public int houseid
        {
            get { return _houseid; }
            set { _houseid = value; }
        }
        private string _totalfloor;
        /// <summary>
        /// 总楼层
        /// </summary>
        public string totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private string _purpose;
        /// <summary>
        /// 用途1002014
        /// </summary>
        public string purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        private decimal _innerbuildingarea = 0M;
        /// <summary>
        /// 套内面积
        /// </summary>
        public decimal innerbuildingarea
        {
            get { return _innerbuildingarea; }
            set { _innerbuildingarea = value; }
        }
        private string _structure = "";
        /// <summary>
        /// 建筑结构2010003
        /// </summary>
        public string structure
        {
            get { return _structure; }
            set { _structure = value; }
        }
        private string _buildingdate = "";
        /// <summary>
        /// 建筑年代
        /// </summary>
        public string buildingdate
        {
            get { return _buildingdate; }
            set { _buildingdate = value; }
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
        /// 土地用途1001
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
        /// 法定用途
        /// </summary>
        public string statutorypurpose
        {
            get { return _statutorypurpose; }
            set { _statutorypurpose = value; }
        }
        private decimal _oldprice = 0M;
        /// <summary>
        /// 原购价
        /// </summary>
        public decimal oldprice
        {
            get { return _oldprice; }
            set { _oldprice = value; }
        }
        private decimal _saleprice = 0M;
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal saleprice
        {
            get { return _saleprice; }
            set { _saleprice = value; }
        }
        private decimal _prepareloanamount = 0M;
        /// <summary>
        /// 拟贷金额
        /// </summary>
        public decimal prepareloanamount
        {
            get { return _prepareloanamount; }
            set { _prepareloanamount = value; }
        }
        private int _expiryfiveyear = 0;
        /// <summary>
        /// 是否满5年
        /// </summary>
        public int expiryfiveyear
        {
            get { return _expiryfiveyear; }
            set { _expiryfiveyear = value; }
        }
        private decimal _rent = 0M;
        /// <summary>
        /// 租金(元/月.平方米)
        /// </summary>
        public decimal rent
        {
            get { return _rent; }
            set { _rent = value; }
        }
        private string _shoptype = "";
        /// <summary>
        /// 商铺类型
        /// </summary>
        public string shoptype
        {
            get { return _shoptype; }
            set { _shoptype = value; }
        }
        private decimal? _shopturnoverrate;
        /// <summary>
        /// 同类商铺营业率(%)
        /// </summary>
        public decimal? shopturnoverrate
        {
            get { return _shopturnoverrate; }
            set { _shopturnoverrate = value; }
        }
        private string _shopstatenow;
        /// <summary>
        /// 商铺现状
        /// </summary>
        public string shopstatenow
        {
            get { return _shopstatenow; }
            set { _shopstatenow = value; }
        }
        private decimal? _landarea;
        /// <summary>
        /// 土地面积
        /// </summary>
        public decimal? landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
        private decimal? _landunitprice;
        /// <summary>
        /// 土地单价
        /// </summary>
        public decimal? landunitprice
        {
            get { return _landunitprice; }
            set { _landunitprice = value; }
        }
        private decimal? _floorunitprice;
        /// <summary>
        /// 楼面单价
        /// </summary>
        public decimal? floorunitprice
        {
            get { return _floorunitprice; }
            set { _floorunitprice = value; }
        }
        private string _landstatutorypurpose;
        /// <summary>
        /// 土地证载用途
        /// </summary>
        public string landstatutorypurpose
        {
            get { return _landstatutorypurpose; }
            set { _landstatutorypurpose = value; }
        }

        private decimal? _landtotalprice;
        /// <summary>
        /// 土地总价
        /// </summary>
        public decimal? landtotalprice
        {
            get { return _landtotalprice; }
            set { _landtotalprice = value; }
        }
    }
}
