using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_Office")]
    public class DatObjectOffice : BaseTO
    {
        private long _objectid;
        /// <summary>
        /// 办公基础委估对象ID
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
        private int _totalfloor = 0;
        /// <summary>
        /// 总楼层
        /// </summary>
        public int totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private string _purpose = "";
        /// <summary>
        /// 用途((1002015))
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
        private string _structure;
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
        private string _statutorypurpose = "";
        /// <summary>
        /// 法定用途
        /// </summary>
        public string statutorypurpose
        {
            get { return _statutorypurpose; }
            set { _statutorypurpose = value; }
        }
        private string _officelevel = "";
        /// <summary>
        /// 写字楼等级6049001
        /// </summary>
        public string officelevel
        {
            get { return _officelevel; }
            set { _officelevel = value; }
        }

        private int _commondecorationlevel = 0;
        /// <summary>
        /// 公共装修等级6034001
        /// </summary>
        public int commondecorationlevel
        {
            get { return _commondecorationlevel; }
            set { _commondecorationlevel = value; }
        }
        private string _managercompany = "";
        /// <summary>
        /// 物业管理公司
        /// </summary>
        public string managercompany
        {
            get { return _managercompany; }
            set { _managercompany = value; }
        }
        private decimal _managerprice = 0M;
        /// <summary>
        /// 管理费,元.月/平方米
        /// </summary>
        public decimal managerprice
        {
            get { return _managerprice; }
            set { _managerprice = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _savedate = DateTime.Now;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuserid;
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string saveuserid
        {
            get { return _saveuserid; }
            set { _saveuserid = value; }
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
        private int _landpurpose = 0;
        /// <summary>
        /// 土地用途
        /// </summary>
        public int landpurpose
        {
            get { return _landpurpose; }
            set { _landpurpose = value; }
        }
        private int _landstatutorypurpose = 0;
        /// <summary>
        /// 土地证载用途
        /// </summary>
        public int landstatutorypurpose
        {
            get { return _landstatutorypurpose; }
            set { _landstatutorypurpose = value; }
        }



        private decimal _oldprice = 0M;
        public decimal oldprice
        {
            get { return _oldprice; }
            set { _oldprice = value; }
        }
        private decimal _saleprice = 0M;
        public decimal saleprice
        {
            get { return _saleprice; }
            set { _saleprice = value; }
        }
        private decimal _prepareloanamount = 0M;
        public decimal prepareloanamount
        {
            get { return _prepareloanamount; }
            set { _prepareloanamount = value; }
        }
        private int _expiryfiveyear = 0;
        public int expiryfiveyear
        {
            get { return _expiryfiveyear; }
            set { _expiryfiveyear = value; }
        }
        private decimal _rent = 0M;
        public decimal rent
        {
            get { return _rent; }
            set { _rent = value; }
        }
        private decimal? _occupancyrate;
        public decimal? occupancyrate
        {
            get { return _occupancyrate; }
            set { _occupancyrate = value; }
        }
        private string _fitment = "";
        public string fitment
        {
            get { return _fitment; }
            set { _fitment = value; }
        }

    }

}
