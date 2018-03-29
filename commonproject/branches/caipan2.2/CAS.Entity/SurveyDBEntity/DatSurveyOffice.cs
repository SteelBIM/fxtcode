using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库查勘办公子表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_Survey_Office")]
    public class DatSurveyOffice : BaseTO
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
        /// 楼盘名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _buildingname;
        /// <summary>
        /// 楼栋名称
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
        /// 房号名称
        /// </summary>
        public string housename
        {
            get { return _housename; }
            set { _housename = value; }
        }
        private string _totalfloors;
        /// <summary>
        /// 总楼层
        /// </summary>
        public string totalfloors
        {
            get { return _totalfloors; }
            set { _totalfloors = value; }
        }
        private string _officeisfloors;
        /// <summary>
        /// 办公总层数
        /// </summary>
        public string officeisfloors
        {
            get { return _officeisfloors; }
            set { _officeisfloors = value; }
        }
        private string _estimatefloor;
        /// <summary>
        /// 评估楼层
        /// </summary>
        public string estimatefloor
        {
            get { return _estimatefloor; }
            set { _estimatefloor = value; }
        }
        private string _buidlingstruct;
        /// <summary>
        /// 建筑结构
        /// </summary>
        public string buidlingstruct
        {
            get { return _buidlingstruct; }
            set { _buidlingstruct = value; }
        }
        private string _completetime;
        /// <summary>
        /// 竣工日期
        /// </summary>
        public string completetime
        {
            get { return _completetime; }
            set { _completetime = value; }
        }
        private string _podium;
        /// <summary>
        /// 裙楼
        /// </summary>
        public string podium
        {
            get { return _podium; }
            set { _podium = value; }
        }
        private string _basement;
        /// <summary>
        /// 地下室
        /// </summary>
        public string basement
        {
            get { return _basement; }
            set { _basement = value; }
        }
        private string _structnew;
        /// <summary>
        /// 结构成新率
        /// </summary>
        public string structnew
        {
            get { return _structnew; }
            set { _structnew = value; }
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
        private string _propertycompany;
        /// <summary>
        /// 物业公司
        /// </summary>
        public string propertycompany
        {
            get { return _propertycompany; }
            set { _propertycompany = value; }
        }
        private string _propertyprice;
        /// <summary>
        /// 管理费
        /// </summary>
        public string propertyprice
        {
            get { return _propertyprice; }
            set { _propertyprice = value; }
        }
        private string _rent;
        /// <summary>
        /// 租金
        /// </summary>
        public string rent
        {
            get { return _rent; }
            set { _rent = value; }
        }
        private string _upparkingspaces;
        /// <summary>
        /// 地上车位
        /// </summary>
        public string upparkingspaces
        {
            get { return _upparkingspaces; }
            set { _upparkingspaces = value; }
        }
        private string _downparkingspaces;
        /// <summary>
        /// 地下车位
        /// </summary>
        public string downparkingspaces
        {
            get { return _downparkingspaces; }
            set { _downparkingspaces = value; }
        }
        private string _stopcarconvenient;
        /// <summary>
        /// 停车便捷度
        /// </summary>
        public string stopcarconvenient
        {
            get { return _stopcarconvenient; }
            set { _stopcarconvenient = value; }
        }
        private string _parkingspacesratio;
        /// <summary>
        /// 车位面积比
        /// </summary>
        public string parkingspacesratio
        {
            get { return _parkingspacesratio; }
            set { _parkingspacesratio = value; }
        }
        private string _inhousingprobability;
        /// <summary>
        /// 入住率
        /// </summary>
        public string inhousingprobability
        {
            get { return _inhousingprobability; }
            set { _inhousingprobability = value; }
        }
        private string _officelv;
        /// <summary>
        /// 写字楼等级
        /// </summary>
        public string officelv
        {
            get { return _officelv; }
            set { _officelv = value; }
        }
        private string _businesssupportingout;
        /// <summary>
        /// 商业配套(外部)
        /// </summary>
        public string businesssupportingout
        {
            get { return _businesssupportingout; }
            set { _businesssupportingout = value; }
        }
        private string _businesssupportingin;
        /// <summary>
        /// 商业配套(内部)
        /// </summary>
        public string businesssupportingin
        {
            get { return _businesssupportingin; }
            set { _businesssupportingin = value; }
        }
        private string _outwall;
        /// <summary>
        /// 外墙装修
        /// </summary>
        public string outwall
        {
            get { return _outwall; }
            set { _outwall = value; }
        }
        private string _lobbyhigh;
        /// <summary>
        /// 大堂层高
        /// </summary>
        public string lobbyhigh
        {
            get { return _lobbyhigh; }
            set { _lobbyhigh = value; }
        }
        private string _lobbytop;
        /// <summary>
        /// 大堂天花装修
        /// </summary>
        public string lobbytop
        {
            get { return _lobbytop; }
            set { _lobbytop = value; }
        }
        private string _lobbyinwall;
        /// <summary>
        /// 大堂内墙装修
        /// </summary>
        public string lobbyinwall
        {
            get { return _lobbyinwall; }
            set { _lobbyinwall = value; }
        }
        private string _lobbyground;
        /// <summary>
        /// 大堂地面装修
        /// </summary>
        public string lobbyground
        {
            get { return _lobbyground; }
            set { _lobbyground = value; }
        }
        private string _corridorhigh;
        /// <summary>
        /// 走廊高度
        /// </summary>
        public string corridorhigh
        {
            get { return _corridorhigh; }
            set { _corridorhigh = value; }
        }
        private string _corridortop;
        /// <summary>
        /// 走廊天花
        /// </summary>
        public string corridortop
        {
            get { return _corridortop; }
            set { _corridortop = value; }
        }
        private string _corridorinwall;
        /// <summary>
        /// 走廊内墙
        /// </summary>
        public string corridorinwall
        {
            get { return _corridorinwall; }
            set { _corridorinwall = value; }
        }
        private string _corridorground;
        /// <summary>
        /// 走廊地面
        /// </summary>
        public string corridorground
        {
            get { return _corridorground; }
            set { _corridorground = value; }
        }
        private string _buidlingshape;
        /// <summary>
        /// 建筑形态
        /// </summary>
        public string buidlingshape
        {
            get { return _buidlingshape; }
            set { _buidlingshape = value; }
        }
        private string _inenterprise;
        /// <summary>
        /// 进驻知名企业
        /// </summary>
        public string inenterprise
        {
            get { return _inenterprise; }
            set { _inenterprise = value; }
        }
        private string _applicationflag;
        /// <summary>
        /// 与法定用途是否一致
        /// </summary>
        public string applicationflag
        {
            get { return _applicationflag; }
            set { _applicationflag = value; }
        }
        private string _usestatus;
        /// <summary>
        /// 使用现状
        /// </summary>
        public string usestatus
        {
            get { return _usestatus; }
            set { _usestatus = value; }
        }
        private string _layerhigh;
        /// <summary>
        /// 层高
        /// </summary>
        public string layerhigh
        {
            get { return _layerhigh; }
            set { _layerhigh = value; }
        }
        private string _landscape;
        /// <summary>
        /// 景观
        /// </summary>
        public string landscape
        {
            get { return _landscape; }
            set { _landscape = value; }
        }
        private string _decorationnew;
        /// <summary>
        /// 装修成新率
        /// </summary>
        public string decorationnew
        {
            get { return _decorationnew; }
            set { _decorationnew = value; }
        }
        private string _decorationlv;
        /// <summary>
        /// 装修档次
        /// </summary>
        public string decorationlv
        {
            get { return _decorationlv; }
            set { _decorationlv = value; }
        }
        private string _ground;
        /// <summary>
        /// 地面
        /// </summary>
        public string ground
        {
            get { return _ground; }
            set { _ground = value; }
        }
        private string _door;
        /// <summary>
        /// 门
        /// </summary>
        public string door
        {
            get { return _door; }
            set { _door = value; }
        }
        private string _window;
        /// <summary>
        /// 窗
        /// </summary>
        public string window
        {
            get { return _window; }
            set { _window = value; }
        }
        private string _wall;
        /// <summary>
        /// 墙
        /// </summary>
        public string wall
        {
            get { return _wall; }
            set { _wall = value; }
        }
        private string _singlebathroom;
        /// <summary>
        /// 有无独立卫生间
        /// </summary>
        public string singlebathroom
        {
            get { return _singlebathroom; }
            set { _singlebathroom = value; }
        }
        private string _publicbathroom;
        /// <summary>
        /// 有无公共卫生间
        /// </summary>
        public string publicbathroom
        {
            get { return _publicbathroom; }
            set { _publicbathroom = value; }
        }
        private string _fireelevator;
        /// <summary>
        /// 消防梯数量
        /// </summary>
        public string fireelevator
        {
            get { return _fireelevator; }
            set { _fireelevator = value; }
        }
        private string _fireelevatorbrand;
        /// <summary>
        /// 消防梯品牌
        /// </summary>
        public string fireelevatorbrand
        {
            get { return _fireelevatorbrand; }
            set { _fireelevatorbrand = value; }
        }
        private string _lineelevator;
        /// <summary>
        /// 垂直电梯数量
        /// </summary>
        public string lineelevator
        {
            get { return _lineelevator; }
            set { _lineelevator = value; }
        }
        private string _lineelevatorbrand;
        /// <summary>
        /// 垂直电梯品牌
        /// </summary>
        public string lineelevatorbrand
        {
            get { return _lineelevatorbrand; }
            set { _lineelevatorbrand = value; }
        }
        private string _handrailelevator;
        /// <summary>
        /// 扶手电梯数量
        /// </summary>
        public string handrailelevator
        {
            get { return _handrailelevator; }
            set { _handrailelevator = value; }
        }
        private string _handrailelevatorbrand;
        /// <summary>
        /// 扶手电梯品牌
        /// </summary>
        public string handrailelevatorbrand
        {
            get { return _handrailelevatorbrand; }
            set { _handrailelevatorbrand = value; }
        }
        private string _tourismelevator;
        /// <summary>
        /// 观光电梯数量
        /// </summary>
        public string tourismelevator
        {
            get { return _tourismelevator; }
            set { _tourismelevator = value; }
        }
        private string _tourismelevatorbrand;
        /// <summary>
        /// 观光电梯品牌
        /// </summary>
        public string tourismelevatorbrand
        {
            get { return _tourismelevatorbrand; }
            set { _tourismelevatorbrand = value; }
        }
        private string _firehydrant;
        /// <summary>
        /// 有无消防栓
        /// </summary>
        public string firehydrant
        {
            get { return _firehydrant; }
            set { _firehydrant = value; }
        }
        private string _spraysystems;
        /// <summary>
        /// 有无自动喷淋系统
        /// </summary>
        public string spraysystems
        {
            get { return _spraysystems; }
            set { _spraysystems = value; }
        }
        private string _smokesystems;
        /// <summary>
        /// 有无烟感报警系统
        /// </summary>
        public string smokesystems
        {
            get { return _smokesystems; }
            set { _smokesystems = value; }
        }
        private string _intelligentsystems;
        /// <summary>
        /// 有无智能系统
        /// </summary>
        public string intelligentsystems
        {
            get { return _intelligentsystems; }
            set { _intelligentsystems = value; }
        }
        private string _centerrefrigeration;
        /// <summary>
        /// 有无中央空调
        /// </summary>
        public string centerrefrigeration
        {
            get { return _centerrefrigeration; }
            set { _centerrefrigeration = value; }
        }
        private string _network;
        /// <summary>
        /// 有无网络接线
        /// </summary>
        public string network
        {
            get { return _network; }
            set { _network = value; }
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
        private string _busdistance;
        /// <summary>
        /// 公交站台距离
        /// </summary>
        public string busdistance
        {
            get { return _busdistance; }
            set { _busdistance = value; }
        }
        private string _subwaydistance;
        /// <summary>
        /// 地铁站距离
        /// </summary>
        public string subwaydistance
        {
            get { return _subwaydistance; }
            set { _subwaydistance = value; }
        }
        private string _bussubwayroad;
        /// <summary>
        /// 公交及地铁线路
        /// </summary>
        public string bussubwayroad
        {
            get { return _bussubwayroad; }
            set { _bussubwayroad = value; }
        }
        private string _trafficmanagement;
        /// <summary>
        /// 是否有交通管制
        /// </summary>
        public string trafficmanagement
        {
            get { return _trafficmanagement; }
            set { _trafficmanagement = value; }
        }
        private string _sideofficeproperty;
        /// <summary>
        /// 周边办公物业
        /// </summary>
        public string sideofficeproperty
        {
            get { return _sideofficeproperty; }
            set { _sideofficeproperty = value; }
        }
        private string _sideproperty;
        /// <summary>
        /// 周边其它物业
        /// </summary>
        public string sideproperty
        {
            get { return _sideproperty; }
            set { _sideproperty = value; }
        }
        private string _purposecode;
        /// <summary>
        /// 物业实际用途
        /// </summary>
        public string purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private string _orientations;
        /// <summary>
        /// 朝向
        /// </summary>
        public string orientations
        {
            get { return _orientations; }
            set { _orientations = value; }
        }
        private string _officetop;
        /// <summary>
        /// 办公室天花装修
        /// </summary>
        public string officetop
        {
            get { return _officetop; }
            set { _officetop = value; }
        }
        private string _officeground;
        /// <summary>
        /// 办公室地面装修
        /// </summary>
        public string officeground
        {
            get { return _officeground; }
            set { _officeground = value; }
        }
        private string _officeinwall;
        /// <summary>
        /// 办公室内墙装修
        /// </summary>
        public string officeinwall
        {
            get { return _officeinwall; }
            set { _officeinwall = value; }
        }
        private string _toilettop;
        /// <summary>
        /// 卫生间天花装修
        /// </summary>
        public string toilettop
        {
            get { return _toilettop; }
            set { _toilettop = value; }
        }
        private string _toiletground;
        /// <summary>
        /// 卫生间地面装修
        /// </summary>
        public string toiletground
        {
            get { return _toiletground; }
            set { _toiletground = value; }
        }
        private string _toiletinwall;
        /// <summary>
        /// 卫生间内墙装修
        /// </summary>
        public string toiletinwall
        {
            get { return _toiletinwall; }
            set { _toiletinwall = value; }
        }
        private string _fillergrade;
        /// <summary>
        /// 卫生间装修档次
        /// </summary>
        public string fillergrade
        {
            get { return _fillergrade; }
            set { _fillergrade = value; }
        }
        private string _cleangrade;
        /// <summary>
        /// 卫生间清洁程度
        /// </summary>
        public string cleangrade
        {
            get { return _cleangrade; }
            set { _cleangrade = value; }
        }
        private string _officebizcenter;
        /// <summary>
        /// 商务中心
        /// </summary>
        public string officebizcenter
        {
            get { return _officebizcenter; }
            set { _officebizcenter = value; }
        }
        private string _scompletetime;
        /// <summary>
        /// 查勘完成时间
        /// </summary>
        public string scompletetime
        {
            get { return _scompletetime; }
            set { _scompletetime = value; }
        }
    }
}