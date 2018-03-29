using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_House")]
    public class DatObjectHouse : BaseTO
    {
        private long _objectid;
        /// <summary>
        /// 住宅基础委估对象ID
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
        private string _purpose = "";
        /// <summary>
        /// 用途((1002001))
        /// </summary>
        public string purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        private string _innerbuildingarea;
        /// <summary>
        /// 套内面积
        /// </summary>
        public string innerbuildingarea
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
        private string _front;
        /// <summary>
        /// 朝向
        /// </summary>
        public string front
        {
            get { return _front; }
            set { _front = value; }
        }
        private string _sight;
        /// <summary>
        /// 景观
        /// </summary>
        public string sight
        {
            get { return _sight; }
            set { _sight = value; }
        }
        private int? _balconynumber;
        /// <summary>
        /// 阳台数量
        /// </summary>
        public int? balconynumber
        {
            get { return _balconynumber; }
            set { _balconynumber = value; }
        }
        private decimal? _gardenarea;
        /// <summary>
        /// 花园面积
        /// </summary>
        public decimal? gardenarea
        {
            get { return _gardenarea; }
            set { _gardenarea = value; }
        }
        private int? _housecount;
        /// <summary>
        /// 房间数量
        /// </summary>
        public int? housecount
        {
            get { return _housecount; }
            set { _housecount = value; }
        }
        private int? _hallcount;
        /// <summary>
        /// 厅数量
        /// </summary>
        public int? hallcount
        {
            get { return _hallcount; }
            set { _hallcount = value; }
        }
        private int? _bathroomcount;
        /// <summary>
        /// 卫生间数量
        /// </summary>
        public int? bathroomcount
        {
            get { return _bathroomcount; }
            set { _bathroomcount = value; }
        }
        private int _haskitchen = 0;
        /// <summary>
        /// 有无厨房
        /// </summary>
        public int haskitchen
        {
            get { return _haskitchen; }
            set { _haskitchen = value; }
        }

        private string _fitment = "";
        /// <summary>
        /// 装修类型
        /// </summary>
        public string fitment
        {
            get { return _fitment; }
            set { _fitment = value; }
        }
        private decimal? _oldprice;
        /// <summary>
        /// 原购价
        /// </summary>
        public decimal? oldprice
        {
            get { return _oldprice; }
            set { _oldprice = value; }
        }
        private decimal? _saleprice;
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal? saleprice
        {
            get { return _saleprice; }
            set { _saleprice = value; }
        }
        private decimal? _prepareloanamount;
        /// <summary>
        /// 拟贷金额
        /// </summary>
        public decimal? prepareloanamount
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
        private int _firstbuye = 0;
        /// <summary>
        /// 是否首次购房
        /// </summary>
        public int firstbuye
        {
            get { return _firstbuye; }
            set { _firstbuye = value; }
        }
        private int _onlylivingroom;
        /// <summary>
        /// 是否唯一生活用房
        /// </summary>
        public int onlylivingroom
        {
            get { return _onlylivingroom; }
            set { _onlylivingroom = value; }
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
        private string _landarea;
        /// <summary>
        /// 土地面积
        /// </summary>
        public string landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
        private decimal? _landunitprice=0;
        /// <summary>
        /// 土地单价
        /// </summary>
        public decimal? landunitprice
        {
            get { return _landunitprice; }
            set { _landunitprice = value; }
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
        private decimal? _subhousealltotalprice;
        /// <summary>
        /// 所有附属房屋总价
        /// </summary>
        public decimal? subhousealltotalprice
        {
            get { return _subhousealltotalprice; }
            set { _subhousealltotalprice = value; }
        }

        private string _landpurpose = "";
        /// <summary>
        /// 土地用途
        /// </summary>
        public string landpurpose
        {
            get { return _landpurpose; }
            set { _landpurpose = value; }
        }
        private string _landstatutorypurpose = "";
        /// <summary>
        /// 土地证载用途
        /// </summary>
        public string landstatutorypurpose
        {
            get { return _landstatutorypurpose; }
            set { _landstatutorypurpose = value; }
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
        private string _housetypecode = "";
        /// <summary>
        /// 户型
        /// </summary>
        public string housetypecode
        {
            get { return _housetypecode; }
            set { _housetypecode = value; }
        }

    }
}