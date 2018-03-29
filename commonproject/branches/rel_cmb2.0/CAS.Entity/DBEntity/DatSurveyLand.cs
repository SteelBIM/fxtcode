using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Survey_Land")]
    public class DatSurveyLand : BaseTO
    {
        private long _sid;
        /// <summary>
        /// 查勘基础表ID
        /// </summary>
        [SQLField("sid", EnumDBFieldUsage.PrimaryKey)]
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
        private string _projectname;
        /// <summary>
        /// 宗地名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _owner;
        /// <summary>
        /// 权利人
        /// </summary>
        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        private string _ownernature;
        /// <summary>
        /// 所以权性质
        /// </summary>
        public string ownernature
        {
            get { return _ownernature; }
            set { _ownernature = value; }
        }
        private string _useoriginally;
        /// <summary>
        /// 使用权来源
        /// </summary>
        public string useoriginally
        {
            get { return _useoriginally; }
            set { _useoriginally = value; }
        }
        private string _useryear;
        /// <summary>
        /// 使用年限
        /// </summary>
        public string useryear
        {
            get { return _useryear; }
            set { _useryear = value; }
        }
        private string _begintime;
        /// <summary>
        /// 土地起始时间
        /// </summary>
        public string begintime
        {
            get { return _begintime; }
            set { _begintime = value; }
        }
        private string _endtime;
        /// <summary>
        /// 土地终止时间
        /// </summary>
        public string endtime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }
        private string _parcelnum;
        /// <summary>
        /// 宗地编号
        /// </summary>
        public string parcelnum
        {
            get { return _parcelnum; }
            set { _parcelnum = value; }
        }
        private string _parcelarea;
        /// <summary>
        /// 宗地面积
        /// </summary>
        public string parcelarea
        {
            get { return _parcelarea; }
            set { _parcelarea = value; }
        }
        private string _purpose;
        /// <summary>
        /// 用途
        /// </summary>
        public string purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        private string _east;
        /// <summary>
        /// 四至-东
        /// </summary>
        public string east
        {
            get { return _east; }
            set { _east = value; }
        }
        private string _south;
        /// <summary>
        /// 四至-南
        /// </summary>
        public string south
        {
            get { return _south; }
            set { _south = value; }
        }
        private string _western;
        /// <summary>
        /// 四至-西
        /// </summary>
        public string western
        {
            get { return _western; }
            set { _western = value; }
        }
        private string _north;
        /// <summary>
        /// 四至-北
        /// </summary>
        public string north
        {
            get { return _north; }
            set { _north = value; }
        }
        private string _landlv;
        /// <summary>
        /// 土地等级
        /// </summary>
        public string landlv
        {
            get { return _landlv; }
            set { _landlv = value; }
        }
        private string _landshape;
        /// <summary>
        /// 地块形状
        /// </summary>
        public string landshape
        {
            get { return _landshape; }
            set { _landshape = value; }
        }
        private string _landsituation;
        /// <summary>
        /// 地势
        /// </summary>
        public string landsituation
        {
            get { return _landsituation; }
            set { _landsituation = value; }
        }
        private string _developprobability;
        /// <summary>
        /// 开发程度
        /// </summary>
        public string developprobability
        {
            get { return _developprobability; }
            set { _developprobability = value; }
        }
        private string _cubagerate;
        /// <summary>
        /// 容积率
        /// </summary>
        public string cubagerate
        {
            get { return _cubagerate; }
            set { _cubagerate = value; }
        }
        private string _coverrate;
        /// <summary>
        /// 覆盖率
        /// </summary>
        public string coverrate
        {
            get { return _coverrate; }
            set { _coverrate = value; }
        }
        private string _baseequipment;
        /// <summary>
        /// 市政基础设施完备度
        /// </summary>
        public string baseequipment
        {
            get { return _baseequipment; }
            set { _baseequipment = value; }
        }
        private string _publicequipment;
        /// <summary>
        /// 公共设施完备度
        /// </summary>
        public string publicequipment
        {
            get { return _publicequipment; }
            set { _publicequipment = value; }
        }
        private string _businesslv;
        /// <summary>
        /// 商业等级
        /// </summary>
        public string businesslv
        {
            get { return _businesslv; }
            set { _businesslv = value; }
        }
        private string _contract;
        /// <summary>
        /// 有无《国有土地使用权出让合同》
        /// </summary>
        public string contract
        {
            get { return _contract; }
            set { _contract = value; }
        }
        private string _landusecertificate;
        /// <summary>
        /// 有无《国有土地使用证》
        /// </summary>
        public string landusecertificate
        {
            get { return _landusecertificate; }
            set { _landusecertificate = value; }
        }
        private string _houseprove;
        /// <summary>
        /// 有无《房地产证》
        /// </summary>
        public string houseprove
        {
            get { return _houseprove; }
            set { _houseprove = value; }
        }
        private string _planlicenseprove;
        /// <summary>
        /// 有无《建设用地规划许可证》
        /// </summary>
        public string planlicenseprove
        {
            get { return _planlicenseprove; }
            set { _planlicenseprove = value; }
        }
        private string _importantletters;
        /// <summary>
        /// 有无《国土局规划设计要点复函》
        /// </summary>
        public string importantletters
        {
            get { return _importantletters; }
            set { _importantletters = value; }
        }
        private string _governmentapprove;
        /// <summary>
        /// 有无《政府批文》
        /// </summary>
        public string governmentapprove
        {
            get { return _governmentapprove; }
            set { _governmentapprove = value; }
        }
        private string _ownerprove;
        /// <summary>
        /// 有无《付清地价款证明》
        /// </summary>
        public string ownerprove
        {
            get { return _ownerprove; }
            set { _ownerprove = value; }
        }
        private string _redfigure;
        /// <summary>
        /// 有无《红线图》
        /// </summary>
        public string redfigure
        {
            get { return _redfigure; }
            set { _redfigure = value; }
        }
        private string _parcelfigure;
        /// <summary>
        /// 有无《宗地图》
        /// </summary>
        public string parcelfigure
        {
            get { return _parcelfigure; }
            set { _parcelfigure = value; }
        }
        private string _buidlingflatfigure;
        /// <summary>
        /// 有无《建筑平面图》
        /// </summary>
        public string buidlingflatfigure
        {
            get { return _buidlingflatfigure; }
            set { _buidlingflatfigure = value; }
        }
        private string _otherprove;
        /// <summary>
        /// 其他协议、合同、证明文件
        /// </summary>
        public string otherprove
        {
            get { return _otherprove; }
            set { _otherprove = value; }
        }
        private string _groundbuidlingnumber;
        /// <summary>
        /// 地上建筑物栋数
        /// </summary>
        public string groundbuidlingnumber
        {
            get { return _groundbuidlingnumber; }
            set { _groundbuidlingnumber = value; }
        }
        private string _groundbuidingfloor;
        /// <summary>
        /// 地上建筑物层数
        /// </summary>
        public string groundbuidingfloor
        {
            get { return _groundbuidingfloor; }
            set { _groundbuidingfloor = value; }
        }
        private string _sumbuidlingarea;
        /// <summary>
        /// 总建筑面积
        /// </summary>
        public string sumbuidlingarea
        {
            get { return _sumbuidlingarea; }
            set { _sumbuidlingarea = value; }
        }
        private string _groundbuidingstruct;
        /// <summary>
        /// 地上建筑物建筑结构
        /// </summary>
        public string groundbuidingstruct
        {
            get { return _groundbuidingstruct; }
            set { _groundbuidingstruct = value; }
        }
        private string _groundbuidinguse;
        /// <summary>
        /// 地上建筑物用途
        /// </summary>
        public string groundbuidinguse
        {
            get { return _groundbuidinguse; }
            set { _groundbuidinguse = value; }
        }
        private string _sideroadtype;
        /// <summary>
        /// 临路类型
        /// </summary>
        public string sideroadtype
        {
            get { return _sideroadtype; }
            set { _sideroadtype = value; }
        }
        private string _busdistance;
        /// <summary>
        /// 离汽车站台距离
        /// </summary>
        public string busdistance
        {
            get { return _busdistance; }
            set { _busdistance = value; }
        }
        private string _traindistance;
        /// <summary>
        /// 离火车站距离
        /// </summary>
        public string traindistance
        {
            get { return _traindistance; }
            set { _traindistance = value; }
        }
        private string _harbordistance;
        /// <summary>
        /// 离港口距离
        /// </summary>
        public string harbordistance
        {
            get { return _harbordistance; }
            set { _harbordistance = value; }
        }
        private string _airportdistance;
        /// <summary>
        /// 离机场距离
        /// </summary>
        public string airportdistance
        {
            get { return _airportdistance; }
            set { _airportdistance = value; }
        }
        private string _trafficconvenient;
        /// <summary>
        /// 交通便捷度
        /// </summary>
        public string trafficconvenient
        {
            get { return _trafficconvenient; }
            set { _trafficconvenient = value; }
        }
        private string _busstationdistance;
        /// <summary>
        /// 离公交站台距离
        /// </summary>
        public string busstationdistance
        {
            get { return _busstationdistance; }
            set { _busstationdistance = value; }
        }
        private string _subwaystationdistance;
        /// <summary>
        /// 离地铁站距离
        /// </summary>
        public string subwaystationdistance
        {
            get { return _subwaystationdistance; }
            set { _subwaystationdistance = value; }
        }
        private string _bussubwayline;
        /// <summary>
        /// 公交及地铁线路
        /// </summary>
        public string bussubwayline
        {
            get { return _bussubwayline; }
            set { _bussubwayline = value; }
        }
        private string _trafficmanagement;
        /// <summary>
        /// 有无交通管制
        /// </summary>
        public string trafficmanagement
        {
            get { return _trafficmanagement; }
            set { _trafficmanagement = value; }
        }
        private string _sideproperty;
        /// <summary>
        /// 周边物业
        /// </summary>
        public string sideproperty
        {
            get { return _sideproperty; }
            set { _sideproperty = value; }
        }
    }
}