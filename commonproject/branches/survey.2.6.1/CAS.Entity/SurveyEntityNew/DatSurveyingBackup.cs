using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 查勘中备份 实体类
    /// </summary>

    [Serializable]
    [TableAttribute("dbo.Dat_Surveying_Backup")]
    public class DatSurveyingBackup:BaseTO
    {
        private long _backid;
        [SQLField("backid", EnumDBFieldUsage.PrimaryKey, true)]
        public long backid
        {
            get { return _backid; }
            set { _backid = value; }
        }
        private long _sid;
        /// <summary>
        /// 查勘基础信息ID
        /// </summary>
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
        private int? _subcompanyid;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        public int? subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int _typecode;
        /// <summary>
        /// 查勘对象类型1031001
        /// </summary>
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _userid;
        /// <summary>
        /// 查勘员ID
        /// </summary>
        public string userid
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
        /// 业主
        /// </summary>
        public string contactname
        {
            get { return _contactname; }
            set { _contactname = value; }
        }
        private string _contactphone;
        /// <summary>
        /// 业主电话
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
        private DateTime? _createtime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
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
        private decimal? _buildingarea;
        /// <summary>
        /// 面积
        /// </summary>
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private int? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int? valid
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
        private int _templateid;
        /// <summary>
        /// 查勘模板ID
        /// </summary>
        public int templateid
        {
            get { return _templateid; }
            set { _templateid = value; }
        }
        private long? _entrustid;
        /// <summary>
        /// 业务编号，委托编号
        /// </summary>
        public long? entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private int? _biztype;
        /// <summary>
        /// 业务类型
        /// </summary>
        public int? biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int? _createtype;
        /// <summary>
        /// 查勘来源
        /// </summary>
        public int? createtype
        {
            get { return _createtype; }
            set { _createtype = value; }
        }
        private int? _surveyclass;
        /// <summary>
        /// 查勘等级：
        /// </summary>
        public int? surveyclass
        {
            get { return _surveyclass; }
            set { _surveyclass = value; }
        }
        private int? _statecode;
        /// <summary>
        /// 查勘状态
        /// </summary>
        public int? statecode
        {
            get { return _statecode; }
            set { _statecode = value; }
        }
        private string _assignuserid;
        /// <summary>
        /// 分配人
        /// </summary>
        public string assignuserid
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
        private string _canceluserid;
        /// <summary>
        /// 撤销人
        /// </summary>
        public string canceluserid
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
        private int? _imagecount;
        /// <summary>
        /// 图片数
        /// </summary>
        public int? imagecount
        {
            get { return _imagecount; }
            set { _imagecount = value; }
        }
        private int? _videocount;
        /// <summary>
        /// 视频数
        /// </summary>
        public int? videocount
        {
            get { return _videocount; }
            set { _videocount = value; }
        }
        private int? _gps;
        /// <summary>
        /// GPS定位
        /// </summary>
        public int? gps
        {
            get { return _gps; }
            set { _gps = value; }
        }
        private int? _planorder;
        /// <summary>
        /// 排序
        /// </summary>
        public int? planorder
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
        private bool? _issms;
        /// <summary>
        /// 是否发送短信
        /// </summary>
        public bool? issms
        {
            get { return _issms; }
            set { _issms = value; }
        }
        private int? _forcompanyid;
        /// <summary>
        /// 银行版存银行ID，企业版/个人版存自己公司ID
        /// </summary>
        public int? forcompanyid
        {
            get { return _forcompanyid; }
            set { _forcompanyid = value; }
        }
        private int? _systypecode;
        /// <summary>
        /// 数据来源类型
        /// </summary>
        public int? systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int? _phototemplateid;
        /// <summary>
        /// 照片模版ID
        /// </summary>
        public int? phototemplateid
        {
            get { return _phototemplateid; }
            set { _phototemplateid = value; }
        }
        private string _subcompanyname;
        /// <summary>
        /// 分支机构名称
        /// </summary>
        public string subcompanyname
        {
            get { return _subcompanyname; }
            set { _subcompanyname = value; }
        }
        private string _username;
        /// <summary>
        /// 查勘员名称
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _areaname;
        /// <summary>
        /// 行政区名称
        /// </summary>
        public string areaname
        {
            get { return _areaname; }
            set { _areaname = value; }
        }
        private string _workersname;
        /// <summary>
        /// 业务人员名称
        /// </summary>
        public string workersname
        {
            get { return _workersname; }
            set { _workersname = value; }
        }
        private string _createusername;
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string createusername
        {
            get { return _createusername; }
            set { _createusername = value; }
        }
        private string _assignusername;
        /// <summary>
        /// 分配人名称
        /// </summary>
        public string assignusername
        {
            get { return _assignusername; }
            set { _assignusername = value; }
        }
        private string _cancelusername;
        /// <summary>
        /// 撤销人名称
        /// </summary>
        public string cancelusername
        {
            get { return _cancelusername; }
            set { _cancelusername = value; }
        }
        private long? _tempsid;
        public long? tempsid
        {
            get { return _tempsid; }
            set { _tempsid = value; }
        }

        /// <summary>
        /// 查勘key
        /// </summary>
        private string _surveykey;
        public string surveykey {
            get { return _surveykey; }
            set { _surveykey = value; }
        }

        private string _uppttype;
        /// <summary>
        /// 上传平台类型
        /// </summary>
        public string uppttype
        {
            get { return _uppttype; }
            set { _uppttype = value; }
        }

        /// <summary>
        /// 真实查勘状态
        /// </summary>
        [SQLReadOnly]
        public string surveystate { get; set; }


        public string phototype { get; set; }
        /// <summary>
        /// 是否定位
        /// </summary>
        [SQLReadOnly]
        public int isugps { get; set; }

        public string locaddr { get; set; }

    }
}
