using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_Land
    {
        public DAT_Land() {
            this.pageIndex = 1;
            this.pageSize = 30;
        }
        private long _landid;
        /// <summary>
        /// 土地基础信息表
        /// </summary>
        //[SQLField("landid", EnumDBFieldUsage.PrimaryKey, true)]
        [DisplayName("土地ID")]
        [ExcelExportIgnore]
        public long landid
        {
            get { return _landid; }
            set { _landid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        [DisplayName("评估机构")]
        [ExcelExportIgnore]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        [DisplayName("城市")]
        [ExcelExportIgnore]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _areaid;
        /// <summary>
        /// 行政区ID
        /// </summary>
        [DisplayName("行政区")]
        [ExcelExportIgnore]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private int? _subareaid;
        /// <summary>
        /// 片区ID
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("片区")]
        public int? subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private string _landno;
        /// <summary>
        /// 宗地号
        /// </summary>
        [DisplayName("宗地号")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string landno
        {
            get { return _landno; }
            set { _landno = value; }
        }
        private string _fieldno;
        /// <summary>
        /// 土地使用证号
        /// </summary>
        [DisplayName("土地使用证号")]
        //[Required(ErrorMessage = "{0}不能为空")]
        public string fieldno
        {
            get { return _fieldno; }
            set { _fieldno = value; }
        }
        private string _mapno;
        /// <summary>
        /// 图号
        /// </summary>
        [DisplayName("图号")]
        public string mapno
        {
            get { return _mapno; }
            set { _mapno = value; }
        }
        private string _landname;
        /// <summary>
        /// 土地名称
        /// </summary>
        [DisplayName("土地名称")]
        //[Required(ErrorMessage = "{0}不能为空")]
        public string landname
        {
            get { return _landname; }
            set { _landname = value; }
        }
        private string _address;
        /// <summary>
        /// 土地位置
        /// </summary>
        [DisplayName("土地位置")]
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }

        #region 使用权类型、性质
        /// <summary>
        /// 使用权性质
        /// </summary>
        [DisplayName("使用权性质")]
        public string UserTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 使用权类型
        /// </summary>
        [DisplayName("使用权类型")]
        public string LandTypeName
        {
            get;
            set;
        }
        #endregion
        private int? _landtypecode;
        /// <summary>
        /// 使用权类型
        /// </summary>
        [DisplayName("使用权类型")]
        [ExcelExportIgnore]
        public int? landtypecode
        {
            get { return _landtypecode; }
            set { _landtypecode = value; }
        }
        private int? _usetypecode;
        /// <summary>
        /// 使用权性质
        /// </summary>
        [DisplayName("使用权性质")]
        [ExcelExportIgnore]
        public int? usetypecode
        {
            get { return _usetypecode; }
            set { _usetypecode = value; }
        }
        private DateTime? _startdate;
        /// <summary>
        /// 土地使用起始日期
        /// </summary>
        [DisplayName("土地使用起始日期")]
        public DateTime? startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }
        private DateTime? _enddate;
        /// <summary>
        /// 土地使用结束日期
        /// </summary>
        [DisplayName("土地使用结束日期")]
        public DateTime? enddate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }
        private int? _useyear;
        /// <summary>
        /// 土地使用年限
        /// </summary>
        [DisplayName("土地使用年限")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是正整数")]
        [Range(1, int.MaxValue, ErrorMessage = "{0}必须大于或者等于{1}")]
        public int? useyear
        {
            get { return _useyear; }
            set { _useyear = value; }
        }
        private string _planpurpose;
        /// <summary>
        /// 规划用途
        /// </summary>
        [DisplayName("规划用途")]
        public string planpurpose
        {
            get
            {

                return _planpurpose;
            }
            set { _planpurpose = value; }
        }
        private string _factpurpose;
        /// <summary>
        /// 实际用途
        /// </summary>
        [DisplayName("实际用途")]
        public string factpurpose
        {
            get { return _factpurpose; }
            set { _factpurpose = value; }
        }
        private decimal? _landarea;
        /// <summary>
        /// 土地面积
        /// </summary>
        [DisplayName("土地面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
        private decimal? _buildingarea;
        /// <summary>
        /// 规划总建筑面积
        /// </summary>
        [DisplayName("规划总建筑面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private decimal? _cubagerate;
        /// <summary>
        /// 容积率
        /// </summary>
        [DisplayName("容积率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? cubagerate
        {
            get { return _cubagerate; }
            set { _cubagerate = value; }
        }
        private decimal? _maxcubagerate;
        /// <summary>
        /// 最大容积率
        /// </summary>
        [DisplayName("最大容积率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? maxcubagerate
        {
            get { return _maxcubagerate; }
            set { _maxcubagerate = value; }
        }
        private decimal? _mincubagerate;
        /// <summary>
        /// 最小容积率
        /// </summary>
        [DisplayName("最小容积率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? mincubagerate
        {
            get { return _mincubagerate; }
            set { _mincubagerate = value; }
        }
        private decimal? _coverage;
        /// <summary>
        /// 覆盖率
        /// </summary>
        [DisplayName("覆盖率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? coverage
        {
            get { return _coverage; }
            set { _coverage = value; }
        }
        private decimal? _maxcoverage;
        /// <summary>
        /// 最大覆盖率
        /// </summary>
        [DisplayName("最大覆盖率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? maxcoverage
        {
            get { return _maxcoverage; }
            set { _maxcoverage = value; }
        }
        private decimal? _greenrage;
        /// <summary>
        /// 绿化率
        /// </summary>
        [DisplayName("绿化率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? greenrage
        {
            get { return _greenrage; }
            set { _greenrage = value; }
        }
        private decimal? _mingreenrage;
        /// <summary>
        /// 最小绿化率
        /// </summary>
        [DisplayName("最小绿化率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? mingreenrage
        {
            get { return _mingreenrage; }
            set { _mingreenrage = value; }
        }
        private int? _landshapecode;
        /// <summary>
        /// 土地形状
        /// </summary>
        [DisplayName("土地形状")]
        [ExcelExportIgnore]
        public int? landshapecode
        {
            get { return _landshapecode; }
            set { _landshapecode = value; }
        }
        #region 扩展字段 
        /// <summary>
        /// 土地形状
        /// </summary>
        [DisplayName("土地形状")]
        public string LandshapeName{ get; set; }
        #endregion
        private int? _developmentcode;
        /// <summary>
        /// 开发程度
        /// </summary>
        [DisplayName("开发程度")]
        [ExcelExportIgnore]
        public int? developmentcode
        {
            get { return _developmentcode; }
            set { _developmentcode = value; }
        }
        #region 扩展字段
        /// <summary>
        /// 开发程度
        /// </summary>
        [DisplayName("开发程度")]
        public string DevelopmentName { get; set; }
        #endregion
        private string _landusestatus;
        /// <summary>
        /// 土地利用状况
        /// </summary>
        [DisplayName("土地利用状况")]
        public string landusestatus
        {
            get { return _landusestatus; }
            set { _landusestatus = value; }
        }
        private int? _landclass;
        /// <summary>
        /// 土地等级
        /// </summary>
        [DisplayName("土地等级")]
        [ExcelExportIgnore]
        public int? landclass
        {
            get { return _landclass; }
            set { _landclass = value; }
        }
        private int? _heightlimited;
        /// <summary>
        /// 建筑物限高
        /// </summary>
        [DisplayName("建筑物限高")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是正整数")]
        public int? heightlimited
        {
            get { return _heightlimited; }
            set { _heightlimited = value; }
        }
        private string _planlimited;
        /// <summary>
        /// 规划限制
        /// </summary>
        [DisplayName("规划限制")]
        public string planlimited
        {
            get { return _planlimited; }
            set { _planlimited = value; }
        }
        private string _east;
        /// <summary>
        /// 四至 东
        /// </summary>
        [DisplayName("四至 东")]
        public string east
        {
            get { return _east; }
            set { _east = value; }
        }
        private string _west;
        /// <summary>
        /// 四至 西
        /// </summary>
        [DisplayName("四至 西")]
        public string west
        {
            get { return _west; }
            set { _west = value; }
        }
        private string _south;
        /// <summary>
        /// 四至 南
        /// </summary>
        [DisplayName("四至 南")]
        public string south
        {
            get { return _south; }
            set { _south = value; }
        }
        private string _north;
        /// <summary>
        /// 四至 北
        /// </summary>
        [DisplayName("四至 北")]
        public string north
        {
            get { return _north; }
            set { _north = value; }
        }
        private int? _landownerid;
        /// <summary>
        /// 土地所有者
        /// </summary>
        [DisplayName("土地所有者")]
        [ExcelExportIgnore]
        public int? landownerid
        {
            get { return _landownerid; }
            set { _landownerid = value; }
        }
        private int? _landuseid;
        /// <summary>
        /// 土地使用者
        /// </summary>
        [DisplayName("土地使用者")]
        [ExcelExportIgnore]
        public int? landuseid
        {
            get { return _landuseid; }
            set { _landuseid = value; }
        }
        private decimal? _businesscenterdistance;
        /// <summary>
        /// 距离商服中心距离
        /// </summary>
        [DisplayName("距离商服中心距离")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,9}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? businesscenterdistance
        {
            get { return _businesscenterdistance; }
            set { _businesscenterdistance = value; }
        }
        private string _traffic;
        /// <summary>
        /// 交通条件
        /// </summary>
        [DisplayName("交通条件")]
        public string traffic
        {
            get { return _traffic; }
            set { _traffic = value; }
        }
        private string _infrastructure;
        /// <summary>
        /// 基础设施状况
        /// </summary>
        [DisplayName("基础设施状况")]
        public string infrastructure
        {
            get { return _infrastructure; }
            set { _infrastructure = value; }
        }
        private string _publicservice;
        /// <summary>
        /// 公用设施状况
        /// </summary>
        [DisplayName("公用设施状况")]
        public string publicservice
        {
            get { return _publicservice; }
            set { _publicservice = value; }
        }
        private int? _environmentcode;
        /// <summary>
        /// 环境质量
        /// </summary>
        [DisplayName("环境质量")]
        [ExcelExportIgnore]
        public int? environmentcode
        {
            get { return _environmentcode; }
            set { _environmentcode = value; }
        }
        #region 扩展字段
        /// <summary>
        /// 环境质量
        /// </summary>
        [DisplayName("环境质量")]
        public string EnvironmentName { get; set; }
        #endregion
        private DateTime? _licencedate;
        /// <summary>
        /// 许可时间
        /// </summary>
        [DisplayName("许可时间")]
        public DateTime? licencedate
        {
            get { return _licencedate; }
            set { _licencedate = value; }
        }
        private string _landdetail;
        /// <summary>
        /// 宗地自身条件
        /// </summary>
        [DisplayName("宗地自身条件")]
        public string landdetail
        {
            get { return _landdetail; }
            set { _landdetail = value; }
        }
        private decimal? _weight = 1M;
        /// <summary>
        /// 地价指数
        /// </summary>
        [DisplayName("地价指数")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,9}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        private decimal? _coefficient;
        /// <summary>
        /// 综合修正系数
        /// </summary>
        [DisplayName("综合修正系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,9}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? coefficient
        {
            get { return _coefficient; }
            set { _coefficient = value; }
        }
        private decimal? _x;
        /// <summary>
        /// 经度
        /// </summary>
        [DisplayName("经度")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,19}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? x
        {
            get { return _x; }
            set { _x = value; }
        }
        private decimal? _y;
        /// <summary>
        /// 纬度
        /// </summary>
        [DisplayName("纬度")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,19}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? y
        {
            get { return _y; }
            set { _y = value; }
        }
        private int? _xyscale;
        /// <summary>
        /// 经纬度比例
        /// </summary>
        [DisplayName("经纬度比例")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? xyscale
        {
            get { return _xyscale; }
            set { _xyscale = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _creator;
        /// <summary>
        /// 创建者
        /// </summary>
        [DisplayName("创建者")]
        [ExcelExportIgnore]
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private DateTime? _savedate;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DisplayName("最后修改时间")]
        public DateTime? savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuser;
        /// <summary>
        /// 最后修改人
        /// </summary>
        [DisplayName("最后修改人")]
        [ExcelExportIgnore]
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 是否有效
        /// </summary>
        [DisplayName("是否有效")]
        [ExcelExportIgnore]
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        #region 扩展字段
        /// <summary>
        /// 土地用途
        /// </summary>
        [DisplayName("土地用途")]
        [ExcelExportIgnore]
        public List<string> Description { get; set; }
        /// <summary>
        /// 评估机构名称
        /// </summary>
        [DisplayName("评估机构名称")]
        [ExcelExportIgnore]
        public string CompanyName { get; set; }

        /// <summary>
        /// 行政区名称
        /// </summary>
        [DisplayName("行政区名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string AreaName { get; set; }

        /// <summary>
        /// 片区名称
        /// </summary>
        [DisplayName("片区名称")]
        public string SubAreaName { get; set; }

        /// <summary>
        /// 地图标注集合
        /// </summary>
        [DisplayName("地图标注集合")]
        [ExcelExportIgnore]
        public string LngOrLat { get; set; }

        /// <summary>
        /// 土地等级code名称
        /// </summary>
        [DisplayName("土地等级名称")]
        public string LandClassName { get; set; }

        /// <summary>
        /// 土地用途code名称
        /// </summary>
        [DisplayName("土地用途名称")]
        [ExcelExportIgnore]
        public string LandPurposeName { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [DisplayName("城市名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string CityName { get; set; }

        /// <summary>
        /// 土地用途集合
        /// </summary>
        [DisplayName("土地用途集合")]
        [ExcelExportIgnore]
        public string opValue { get; set; }

        /// <summary>
        /// 命令 
        /// self自己
        /// all所有
        /// </summary>
        [DisplayName("命令")]
        [ExcelExportIgnore]
        public bool command { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        [DisplayName("页索引")]
        [ExcelExportIgnore]
        public int pageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        [DisplayName("页大小")]
        [ExcelExportIgnore]
        public int pageSize { get; set; }

        /// <summary>
        /// 地价公布结束时间
        /// </summary>
        [DisplayName("地价公布结束时间")]
        [ExcelExportIgnore]
        public DateTime? priceenddate { get; set; }



        /// <summary>
        /// 土地基础信息表
        /// 土地所有者
        /// </summary>
        [DisplayName("土地所有者")]
        [ExcelExportIgnore]
        public string LandOwnerName { get; set; }
        /// <summary>
        /// 土地基础信息表
        /// 土地使用者
        /// </summary>
        [DisplayName("土地使用者")]
        [ExcelExportIgnore]
        public string LandUseName { get; set; }
        #endregion
    }
}
