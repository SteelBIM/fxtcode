using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Survey")]
    public class DatSurvey : BaseTO
    {
        private long _sid;
        /// <summary>
        /// 查勘基础信息ID
        /// </summary>
        [SQLField("sid", EnumDBFieldUsage.PrimaryKey, true)]
        public long sid
        {
            get { return _sid; }
            set { _sid = value; }
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
        private int _subcompanyid = 0;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int _typecode = 1031001;
        /// <summary>
        /// 查勘对象类型1031001
        /// </summary>
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private int _userid;
        /// <summary>
        /// 查勘员ID
        /// </summary>
        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 查勘员真实名
        /// </summary>
        [SQLReadOnly]
        public string username { get; set; }

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
        private long _objectid;
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int? _areaid;
        /// <summary>
        /// 行政区
        /// </summary>
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
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
        private string _contactname;
        /// <summary>
        /// 查勘联系人
        /// </summary>
        public string contactname
        {
            get { return _contactname; }
            set { _contactname = value; }
        }
        private string _contactphone;
        /// <summary>
        /// 查勘联系人电话
        /// </summary>
        public string contactphone
        {
            get { return _contactphone; }
            set { _contactphone = value; }
        }
        private string _bankcompanyname;
        /// <summary>
        /// 银行
        /// </summary>
        public string bankcompanyname
        {
            get { return _bankcompanyname; }
            set { _bankcompanyname = value; }
        }
        private string _bankdepartmentname;
        /// <summary>
        /// 分行支行
        /// </summary>
        public string bankdepartmentname
        {
            get { return _bankdepartmentname; }
            set { _bankdepartmentname = value; }
        }
        private string _bankqueryuser;
        /// <summary>
        /// 客户经理
        /// </summary>
        public string bankqueryuser
        {
            get { return _bankqueryuser; }
            set { _bankqueryuser = value; }
        }
        private string _bankphone;
        /// <summary>
        /// 银行经办人电话
        /// </summary>
        public string bankphone
        {
            get { return _bankphone; }
            set { _bankphone = value; }
        }
        private int? _workers;
        /// <summary>
        /// 业务人员
        /// </summary>
        public int? workers
        {
            get { return _workers; }
            set { _workers = value; }
        }
        private string _workersphone;
        /// <summary>
        /// 业务人员电话
        /// </summary>
        public string workersphone
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
        private DateTime? _begintime;
        /// <summary>
        /// 开始查勘时间
        /// </summary>
        public DateTime? begintime
        {
            get { return _begintime; }
            set { _begintime = value; }
        }
        private DateTime? _completetime;
        /// <summary>
        /// 上传、完成时间
        /// </summary>
        public DateTime? completetime
        {
            get { return _completetime; }
            set { _completetime = value; }
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
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int? _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
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
        private int _valid = 1;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
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
        private string _customfields;
        /// <summary>
        /// 查勘自定义字段值
        /// </summary>
        public string customfields
        {
            get { return _customfields; }
            set { _customfields = value; }
        }
        private int? _templateid;
        /// <summary>
        /// 查勘模板ID
        /// </summary>
        public int? templateid
        {
            get { return _templateid; }
            set { _templateid = value; }
        }
        private long _entrustid = 0;
        /// <summary>
        /// 业务编号，委托编号
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private int _biztype = 1026001;
        /// <summary>
        /// 业务类型
        /// </summary>
        public int biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int _createtype = 6057001;
        /// <summary>
        /// 查勘来源
        /// </summary>
        public int createtype
        {
            get { return _createtype; }
            set { _createtype = value; }
        }
        private int _surveyclass = 6052001;
        /// <summary>
        /// 查勘等级：
        /// </summary>
        public int surveyclass
        {
            get { return _surveyclass; }
            set { _surveyclass = value; }
        }
        private int _statecode = 5016020;
        /// <summary>
        /// 查勘状态
        /// </summary>
        public int statecode
        {
            get { return _statecode; }
            set { _statecode = value; }
        }
        [SQLReadOnly]
        public string statecodename { get; set; }
        private int? _assignuserid;
        /// <summary>
        /// 分配人
        /// </summary>
        public int? assignuserid
        {
            get { return _assignuserid; }
            set { _assignuserid = value; }
        }
        private DateTime? _assigndate;
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime? assigndate
        {
            get { return _assigndate; }
            set { _assigndate = value; }
        }
        private string _assignphone;
        /// <summary>
        /// 分配人电话
        /// </summary>
        public string assignphone
        {
            get { return _assignphone; }
            set { _assignphone = value; }
        }
        private DateTime? _canceldate;
        /// <summary>
        /// 撤销时间
        /// </summary>
        public DateTime? canceldate
        {
            get { return _canceldate; }
            set { _canceldate = value; }
        }
        private int? _canceluserid;
        /// <summary>
        /// 撤销人
        /// </summary>
        public int? canceluserid
        {
            get { return _canceluserid; }
            set { _canceluserid = value; }
        }
        private string _cancelreason;
        /// <summary>
        /// 撤销原因
        /// </summary>
        public string cancelreason
        {
            get { return _cancelreason; }
            set { _cancelreason = value; }
        }
        private int _imagecount = 0;
        /// <summary>
        /// 图片数
        /// </summary>
        public int imagecount
        {
            get { return _imagecount; }
            set { _imagecount = value; }
        }
        private int _videocount = 0;
        /// <summary>
        /// 视频数
        /// </summary>
        public int videocount
        {
            get { return _videocount; }
            set { _videocount = value; }
        }
        private int _gps = 0;
        /// <summary>
        /// GPS定位
        /// </summary>
        public int gps
        {
            get { return _gps; }
            set { _gps = value; }
        }
        private bool _marktime = true;
        /// <summary>
        /// 是否要打水印
        /// </summary>
        public bool marktime
        {
            get { return _marktime; }
            set { _marktime = value; }
        }
        private int _planorder = 0;
        /// <summary>
        /// 排序
        /// </summary>
        public int planorder
        {
            get { return _planorder; }
            set { _planorder = value; }
        }
        private string _surveyremarks;
        /// <summary>
        /// 查勘备注
        /// </summary>
        public string surveyremarks
        {
            get { return _surveyremarks; }
            set { _surveyremarks = value; }
        }
        private bool _issms = false;
        public bool issms
        {
            get { return _issms; }
            set { _issms = value; }
        }

        //当前机构作为第三方，所用信息
        private int? _outcompanyid;
        /// <summary>
        /// 外部companyid
        /// </summary>
        public int? outcompanyid
        {
            get { return _outcompanyid; }
            set { _outcompanyid = value; }
        }
        private long? _outobjectid;
        /// <summary>
        /// 外部  objectid
        /// </summary>
        public long? outobjectid
        {
            get { return _outobjectid; }
            set { _outobjectid = value; }
        }       
    }
}