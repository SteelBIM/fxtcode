using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.AssetObject")]
    public class AssetObject : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _querytypecode;
        /// <summary>
        /// 询价类型
        /// </summary>
        public int querytypecode
        {
            get { return _querytypecode; }
            set { _querytypecode = value; }
        }
        private string _projectfullname;
        /// <summary>
        /// 资产名称
        /// </summary>
        public string projectfullname
        {
            get { return _projectfullname; }
            set { _projectfullname = value; }
        }
        private int _businessid;
        /// <summary>
        /// 业务ID
        /// </summary>
        public int businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private string _projectname;
        /// <summary>
        /// 楼盘名
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _buildingname;
        /// <summary>
        /// 楼栋名
        /// </summary>
        public string buildingname
        {
            get { return _buildingname; }
            set { _buildingname = value; }
        }
        private string _floornumber;
        /// <summary>
        /// 楼层
        /// </summary>
        public string floornumber
        {
            get { return _floornumber; }
            set { _floornumber = value; }
        }
        private string _housename;
        /// <summary>
        /// 房号
        /// </summary>
        public string housename
        {
            get { return _housename; }
            set { _housename = value; }
        }
        private int? _projectid;
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int? projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int? _buildingid;
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public int? buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private int? _houseid;
        /// <summary>
        /// 房号ID
        /// </summary>
        public int? houseid
        {
            get { return _houseid; }
            set { _houseid = value; }
        }
        private int _totalfloor;
        /// <summary>
        /// 总楼层
        /// </summary>
        public int totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private decimal? _buildingarea;
        /// <summary>
        /// 面积
        /// </summary>
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private string _housecertno;
        /// <summary>
        /// 房产证号
        /// </summary>
        public string housecertno
        {
            get { return _housecertno; }
            set { _housecertno = value; }
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
        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private int? _areaid;
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _provinceid;
        /// <summary>
        /// 省份ID
        /// </summary>
        public int? provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private decimal? _managerprice;
        /// <summary>
        /// 管理费
        /// </summary>
        public decimal? managerprice
        {
            get { return _managerprice; }
            set { _managerprice = value; }
        }
        private decimal? _rent;
        /// <summary>
        /// 租金
        /// </summary>
        public decimal? rent
        {
            get { return _rent; }
            set { _rent = value; }
        }
        private int? _assettype;
        /// <summary>
        /// 鉴价资产类型
        /// </summary>
        public int? assettype
        {
            get { return _assettype; }
            set { _assettype = value; }
        }
        private string _fieldno;
        /// <summary>
        /// 宗地号
        /// </summary>
        public string fieldno
        {
            get { return _fieldno; }
            set { _fieldno = value; }
        }
    }

}
