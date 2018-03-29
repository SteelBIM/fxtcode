using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 查勘接口实体类
    /// 潘锦发 20150910
    /// </summary>
    public class DatSurveyApi
    {
        /// <summary>
        /// Id
        /// </summary>
        public long sid { get; set; }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityId
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtCompanyId
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _subcompanyid = 0;
        /// <summary>
        /// 评估机构分支机构ID
        /// </summary>
        public int subCompanyId
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int _typecode = 1031001;
        /// <summary>
        /// 查勘对象类型1031001
        /// </summary>
        public int typeCode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _userid;
        /// <summary>
        /// 查勘员用户名
        /// </summary>
        public string userId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _names;
        /// <summary>
        /// 查勘对象名称
        /// </summary>
        public string names
        {
            get { return _names; }
            set { _names = value; }
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
        /// 行政区
        /// </summary>
        public int? areaId
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private string _contactname;
        /// <summary>
        /// 业主
        /// </summary>
        public string contactName
        {
            get { return _contactname; }
            set { _contactname = value; }
        }
        private string _contactphone;
        /// <summary>
        /// 业主电话
        /// </summary>
        public string contactPhone
        {
            get { return _contactphone; }
            set { _contactphone = value; }
        }
        private string _bankcompanyname;
        /// <summary>
        /// 银行
        /// </summary>
        public string bankCompanyName
        {
            get { return _bankcompanyname; }
            set { _bankcompanyname = value; }
        }
        private string _bankdepartmentname;
        /// <summary>
        /// 分行支行
        /// </summary>
        public string bankDepartmentName
        {
            get { return _bankdepartmentname; }
            set { _bankdepartmentname = value; }
        }

        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string buildingName { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string floorNumber { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public string houseName { get; set; }

        private string _bankqueryuser;
        /// <summary>
        /// 客户经理
        /// </summary>
        public string bankQueryUser
        {
            get { return _bankqueryuser; }
            set { _bankqueryuser = value; }
        }
        private string _bankphone;
        /// <summary>
        /// 银行经办人电话
        /// </summary>
        public string bankPhone
        {
            get { return _bankphone; }
            set { _bankphone = value; }
        }
        private string _workers;
        /// <summary>
        /// 业务人员
        /// </summary>
        public string workers
        {
            get { return _workers; }
            set { _workers = value; }
        }
        private string _workersphone;
        /// <summary>
        /// 业务人员电话
        /// </summary>
        public string workersPhone
        {
            get { return _workersphone; }
            set { _workersphone = value; }
        }
        private string _remarks;
        /// <summary>
        /// 查勘备注
        /// </summary>
        public string remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        private double? _x;
        /// <summary>
        /// X坐标
        /// </summary>
        public double? x
        {
            get { return _x; }
            set { _x = value; }
        }
        private double? _y;
        /// <summary>
        /// Y坐标
        /// </summary>
        public double? y
        {
            get { return _y; }
            set { _y = value; }
        }
        private decimal? _buildingarea;
        /// <summary>
        /// 面积
        /// </summary>
        public decimal? buildingArea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private double? _loclat;
        /// <summary>
        /// 经度
        /// </summary>
        public double? loclat
        {
            get { return _loclat; }
            set { _loclat = value; }
        }
        private double? _loclng;
        /// <summary>
        /// 纬度
        /// </summary>
        public double? loclng
        {
            get { return _loclng; }
            set { _loclng = value; }
        }
        private int? _templateid;
        /// <summary>
        /// 查勘模板ID
        /// </summary>
        public int? templateId
        {
            get { return _templateid; }
            set { _templateid = value; }
        }
        private int _surveyclass = 6052001;
        /// <summary>
        /// 查勘等级：
        /// </summary>
        public int surveyClass
        {
            get { return _surveyclass; }
            set { _surveyclass = value; }
        }
        private string _assignphone;
        private long _entrustid = 0;
        /// <summary>
        /// 业务编号，委托编号
        /// </summary>
        public long entrustID
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private long _objectid;
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long objectId
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int? _systypecode;
        /// <summary>
        /// 数据来源类型
        /// </summary>
        public int? sysTypeCode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        /// <summary>
        /// 查勘员名称
        /// </summary>
        private string _username;
        public string userName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// 行政区名称
        /// </summary>
        private string _areaname;
        public string areaName
        {
            get { return _areaname; }
            set { _areaname = value; }
        }
        private string _entrust;
        /// <summary>
        /// 委托方
        /// </summary>
        public string entrust
        {
            get { return _entrust; }
            set { _entrust = value; }
        }
        /// <summary>
        /// 业务人员名称
        /// yinpc
        /// </summary>
        private string _workersname;
        public string workersName
        {
            get { return _workersname; }
            set { _workersname = value; }
        }
        /// <summary>
        /// 区分估价宝，贷前、贷后
        /// </summary>
        public int proTypeCode { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        private int? _provinceid;
        public int? provinceId
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        public string cityName { get; set; }
        public string provinceName { get; set; }
        /// <summary>
        /// 添加查勘类型
        /// </summary>
        private string _splatype;
        public string splatype
        {
            get
            {
                _splatype = System.Configuration.ConfigurationManager.AppSettings["SplaType"];
                return _splatype;
            }
            set { _splatype = value; }
        }
        /// <summary>
        /// 分支机构名称
        /// </summary>
        public string subCompanyName { get; set; }
        /// <summary>
        /// 分配人电话
        /// </summary>
        public string assignPhone { get; set; }
        /// <summary>
        /// 辅助查勘人
        /// </summary>
        public string auxiliarySurveyUser { get; set; }
        /// <summary>
        /// 辅助查勘人名称
        /// </summary>
        public string auxiliarySurveyUserName { get; set; }
        /// <summary>
        /// 表单字段
        /// </summary>
        public string customFields { get; set; }

        public string nettype { get; set; }
        /// <summary>
        /// 查勘备注
        /// </summary>
        public string surveyRemarks { get; set; }
        /// <summary>
        /// 查勘用户电话
        /// </summary>
        public string userMobile { get; set; }
    }
}