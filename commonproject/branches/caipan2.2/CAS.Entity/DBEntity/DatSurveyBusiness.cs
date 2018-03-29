using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Survey_Business")]
    public class DatSurveyBusiness : BaseTO
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
        /// 城市id
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构id
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
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
        private string _businessfloor;
        /// <summary>
        /// 商业楼层
        /// </summary>
        public string businessfloor
        {
            get { return _businessfloor; }
            set { _businessfloor = value; }
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
        private string _buildingstruct;
        /// <summary>
        /// 建筑结构
        /// </summary>
        public string buildingstruct
        {
            get { return _buildingstruct; }
            set { _buildingstruct = value; }
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
        private string _podiumfloor;
        /// <summary>
        /// 裙楼层数
        /// </summary>
        public string podiumfloor
        {
            get { return _podiumfloor; }
            set { _podiumfloor = value; }
        }
        private string _basement;
        /// <summary>
        /// 地下室层数
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
        private string _layerhigh;
        /// <summary>
        /// 层高
        /// </summary>
        public string layerhigh
        {
            get { return _layerhigh; }
            set { _layerhigh = value; }
        }
        private string _east;
        /// <summary>
        /// 四至东
        /// </summary>
        public string east
        {
            get { return _east; }
            set { _east = value; }
        }
        private string _south;
        /// <summary>
        /// 四至南
        /// </summary>
        public string south
        {
            get { return _south; }
            set { _south = value; }
        }
        private string _western;
        /// <summary>
        /// 四至西
        /// </summary>
        public string western
        {
            get { return _western; }
            set { _western = value; }
        }
        private string _north;
        /// <summary>
        /// 四至北
        /// </summary>
        public string north
        {
            get { return _north; }
            set { _north = value; }
        }
        private string _propertycompanies;
        /// <summary>
        /// 物管公司
        /// </summary>
        public string propertycompanies
        {
            get { return _propertycompanies; }
            set { _propertycompanies = value; }
        }
        private string _propertyprice;
        /// <summary>
        /// 物业管理费
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
        /// 地上车位数
        /// </summary>
        public string upparkingspaces
        {
            get { return _upparkingspaces; }
            set { _upparkingspaces = value; }
        }
        private string _downparkingspaces;
        /// <summary>
        /// 地下车位数
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
        private string _shoppingstopcar;
        /// <summary>
        /// 是否购物免费停车
        /// </summary>
        public string shoppingstopcar
        {
            get { return _shoppingstopcar; }
            set { _shoppingstopcar = value; }
        }
        private string _businesslv;
        /// <summary>
        /// 商业级别
        /// </summary>
        public string businesslv
        {
            get { return _businesslv; }
            set { _businesslv = value; }
        }
        private string _experiencemethod;
        /// <summary>
        /// 经营方式
        /// </summary>
        public string experiencemethod
        {
            get { return _experiencemethod; }
            set { _experiencemethod = value; }
        }
        private string _buildingshape;
        /// <summary>
        /// 建筑形态
        /// </summary>
        public string buildingshape
        {
            get { return _buildingshape; }
            set { _buildingshape = value; }
        }
        private string _inbusiness;
        /// <summary>
        /// 进驻商家
        /// </summary>
        public string inbusiness
        {
            get { return _inbusiness; }
            set { _inbusiness = value; }
        }
        private string _businesslocate;
        /// <summary>
        /// 商业定位
        /// </summary>
        public string businesslocate
        {
            get { return _businesslocate; }
            set { _businesslocate = value; }
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
        private string _commercialactivities;
        /// <summary>
        /// 商业业态
        /// </summary>
        public string commercialactivities
        {
            get { return _commercialactivities; }
            set { _commercialactivities = value; }
        }
        private string _operatecontent;
        /// <summary>
        /// 经营内容
        /// </summary>
        public string operatecontent
        {
            get { return _operatecontent; }
            set { _operatecontent = value; }
        }
        private string _peopleflow;
        /// <summary>
        /// 人流量
        /// </summary>
        public string peopleflow
        {
            get { return _peopleflow; }
            set { _peopleflow = value; }
        }
        private string _peopledensity;
        /// <summary>
        /// 居住人口密度
        /// </summary>
        public string peopledensity
        {
            get { return _peopledensity; }
            set { _peopledensity = value; }
        }
        private string _peoplequality;
        /// <summary>
        /// 居住人口素质
        /// </summary>
        public string peoplequality
        {
            get { return _peoplequality; }
            set { _peoplequality = value; }
        }
        private string _consumptionlv;
        /// <summary>
        /// 消费档次
        /// </summary>
        public string consumptionlv
        {
            get { return _consumptionlv; }
            set { _consumptionlv = value; }
        }
        private string _shownature;
        /// <summary>
        /// 昭示性
        /// </summary>
        public string shownature
        {
            get { return _shownature; }
            set { _shownature = value; }
        }
        private string _trafficmainroad;
        /// <summary>
        /// 交通主干道
        /// </summary>
        public string trafficmainroad
        {
            get { return _trafficmainroad; }
            set { _trafficmainroad = value; }
        }
        private string _trafficagainroad;
        /// <summary>
        /// 次干道
        /// </summary>
        public string trafficagainroad
        {
            get { return _trafficagainroad; }
            set { _trafficagainroad = value; }
        }
        private string _lifemainroad;
        /// <summary>
        /// 生活型主干道
        /// </summary>
        public string lifemainroad
        {
            get { return _lifemainroad; }
            set { _lifemainroad = value; }
        }
        private string _mixmainroad;
        /// <summary>
        /// 混合型主干道
        /// </summary>
        public string mixmainroad
        {
            get { return _mixmainroad; }
            set { _mixmainroad = value; }
        }
        private string _communityroad;
        /// <summary>
        /// 小区内支路
        /// </summary>
        public string communityroad
        {
            get { return _communityroad; }
            set { _communityroad = value; }
        }
        private string _trafficmanagement;
        /// <summary>
        /// 是否交通管制
        /// </summary>
        public string trafficmanagement
        {
            get { return _trafficmanagement; }
            set { _trafficmanagement = value; }
        }
        private string _planehigh;
        /// <summary>
        /// 比路面高
        /// </summary>
        public string planehigh
        {
            get { return _planehigh; }
            set { _planehigh = value; }
        }
        private string _planelow;
        /// <summary>
        /// 比路面低
        /// </summary>
        public string planelow
        {
            get { return _planelow; }
            set { _planelow = value; }
        }
        private string _temporaryroaddistance;
        /// <summary>
        /// 临路肩距离
        /// </summary>
        public string temporaryroaddistance
        {
            get { return _temporaryroaddistance; }
            set { _temporaryroaddistance = value; }
        }
        private string _isgreenseparate;
        /// <summary>
        /// 有无绿化隔离带
        /// </summary>
        public string isgreenseparate
        {
            get { return _isgreenseparate; }
            set { _isgreenseparate = value; }
        }
        private string _lookroadcode;
        /// <summary>
        /// 临街类型
        /// </summary>
        public string lookroadcode
        {
            get { return _lookroadcode; }
            set { _lookroadcode = value; }
        }
        private string _oneroad;
        /// <summary>
        /// 一面临街
        /// </summary>
        public string oneroad
        {
            get { return _oneroad; }
            set { _oneroad = value; }
        }
        private string _towroad;
        /// <summary>
        /// 两面临街
        /// </summary>
        public string towroad
        {
            get { return _towroad; }
            set { _towroad = value; }
        }
        private string _storeshape;
        /// <summary>
        /// 铺面形状
        /// </summary>
        public string storeshape
        {
            get { return _storeshape; }
            set { _storeshape = value; }
        }
        private string _streetwidth;
        /// <summary>
        /// 商铺开间
        /// </summary>
        public string streetwidth
        {
            get { return _streetwidth; }
            set { _streetwidth = value; }
        }
        private string _streeheight;
        /// <summary>
        /// 商铺进深
        /// </summary>
        public string streeheight
        {
            get { return _streeheight; }
            set { _streeheight = value; }
        }
        private string _streefloor;
        /// <summary>
        /// 商铺层高
        /// </summary>
        public string streefloor
        {
            get { return _streefloor; }
            set { _streefloor = value; }
        }
        private string _isstreemezzanine;
        /// <summary>
        /// 商铺有无夹层
        /// </summary>
        public string isstreemezzanine
        {
            get { return _isstreemezzanine; }
            set { _isstreemezzanine = value; }
        }
        private string _isstreebasement;
        /// <summary>
        /// 商铺有无地下室
        /// </summary>
        public string isstreebasement
        {
            get { return _isstreebasement; }
            set { _isstreebasement = value; }
        }
        private string _storeinescalatorsdistance;
        /// <summary>
        /// 内铺离主入口或扶手电梯距离
        /// </summary>
        public string storeinescalatorsdistance
        {
            get { return _storeinescalatorsdistance; }
            set { _storeinescalatorsdistance = value; }
        }
        private string _storeincorridortype;
        /// <summary>
        /// 内铺临走廊形状
        /// </summary>
        public string storeincorridortype
        {
            get { return _storeincorridortype; }
            set { _storeincorridortype = value; }
        }
        private string _storeinfaceshape;
        /// <summary>
        /// 内铺铺面形状
        /// </summary>
        public string storeinfaceshape
        {
            get { return _storeinfaceshape; }
            set { _storeinfaceshape = value; }
        }
        private string _storeinshape;
        /// <summary>
        /// 内铺形状
        /// </summary>
        public string storeinshape
        {
            get { return _storeinshape; }
            set { _storeinshape = value; }
        }
        private string _storeinheight;
        /// <summary>
        /// 内铺进深
        /// </summary>
        public string storeinheight
        {
            get { return _storeinheight; }
            set { _storeinheight = value; }
        }
        private string _storeinfloorheight;
        /// <summary>
        /// 内铺层高
        /// </summary>
        public string storeinfloorheight
        {
            get { return _storeinfloorheight; }
            set { _storeinfloorheight = value; }
        }
        private string _decorationprobabilitynew;
        /// <summary>
        /// 装修成新率
        /// </summary>
        public string decorationprobabilitynew
        {
            get { return _decorationprobabilitynew; }
            set { _decorationprobabilitynew = value; }
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
        private string _outwall;
        /// <summary>
        /// 外墙装修
        /// </summary>
        public string outwall
        {
            get { return _outwall; }
            set { _outwall = value; }
        }
        private string _ceiling;
        /// <summary>
        /// 天花
        /// </summary>
        public string ceiling
        {
            get { return _ceiling; }
            set { _ceiling = value; }
        }
        private string _inwall;
        /// <summary>
        /// 内墙装修
        /// </summary>
        public string inwall
        {
            get { return _inwall; }
            set { _inwall = value; }
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
        private string _verticalelevator;
        /// <summary>
        /// 垂直电梯数量
        /// </summary>
        public string verticalelevator
        {
            get { return _verticalelevator; }
            set { _verticalelevator = value; }
        }
        private string _verticalelevatorbrand;
        /// <summary>
        /// 垂直电梯品牌
        /// </summary>
        public string verticalelevatorbrand
        {
            get { return _verticalelevatorbrand; }
            set { _verticalelevatorbrand = value; }
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
        private string _watchelevator;
        /// <summary>
        /// 观光电梯数量
        /// </summary>
        public string watchelevator
        {
            get { return _watchelevator; }
            set { _watchelevator = value; }
        }
        private string _watchelevatorbrand;
        /// <summary>
        /// 观光电梯品牌
        /// </summary>
        public string watchelevatorbrand
        {
            get { return _watchelevatorbrand; }
            set { _watchelevatorbrand = value; }
        }
        private string _firebolt;
        /// <summary>
        /// 有无消防栓
        /// </summary>
        public string firebolt
        {
            get { return _firebolt; }
            set { _firebolt = value; }
        }
        private string _intelligentsystems;
        /// <summary>
        /// 有无自动喷淋系统
        /// </summary>
        public string intelligentsystems
        {
            get { return _intelligentsystems; }
            set { _intelligentsystems = value; }
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
        private string _alarmsystems;
        /// <summary>
        /// 有无智能报警系统
        /// </summary>
        public string alarmsystems
        {
            get { return _alarmsystems; }
            set { _alarmsystems = value; }
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
        /// 离公交站点距离
        /// </summary>
        public string busstationdistance
        {
            get { return _busstationdistance; }
            set { _busstationdistance = value; }
        }
        private string _subwaystationdistance;
        /// <summary>
        /// 离地铁站点距离
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
        private string _sidebusinessproperty;
        /// <summary>
        /// 周边商业物业
        /// </summary>
        public string sidebusinessproperty
        {
            get { return _sidebusinessproperty; }
            set { _sidebusinessproperty = value; }
        }
        private string _sideotherproperty;
        /// <summary>
        /// 周边其它物业
        /// </summary>
        public string sideotherproperty
        {
            get { return _sideotherproperty; }
            set { _sideotherproperty = value; }
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
        private string _survey_business_type;
        public string survey_business_type
        {
            get { return _survey_business_type; }
            set { _survey_business_type = value; }
        }
        private string _property_company_verify;
        public string property_company_verify
        {
            get { return _property_company_verify; }
            set { _property_company_verify = value; }
        }
        private string _doorplate_verify;
        public string doorplate_verify
        {
            get { return _doorplate_verify; }
            set { _doorplate_verify = value; }
        }
        private string _location_plan_verify;
        public string location_plan_verify
        {
            get { return _location_plan_verify; }
            set { _location_plan_verify = value; }
        }
        private string _business_license_verify;
        public string business_license_verify
        {
            get { return _business_license_verify; }
            set { _business_license_verify = value; }
        }
        private string _side_shop_verify;
        public string side_shop_verify
        {
            get { return _side_shop_verify; }
            set { _side_shop_verify = value; }
        }
        private string _main_shop;
        public string main_shop
        {
            get { return _main_shop; }
            set { _main_shop = value; }
        }
        private string _shop_size;
        public string shop_size
        {
            get { return _shop_size; }
            set { _shop_size = value; }
        }
        private string _vacancy_rate;
        public string vacancy_rate
        {
            get { return _vacancy_rate; }
            set { _vacancy_rate = value; }
        }
        private string _business_price;
        public string business_price
        {
            get { return _business_price; }
            set { _business_price = value; }
        }
        private string _side_shop_state;
        public string side_shop_state
        {
            get { return _side_shop_state; }
            set { _side_shop_state = value; }
        }
        private string _toilet_land;
        public string toilet_land
        {
            get { return _toilet_land; }
            set { _toilet_land = value; }
        }
        private string _toilet_wall;
        public string toilet_wall
        {
            get { return _toilet_wall; }
            set { _toilet_wall = value; }
        }
        private string _toilet_top;
        public string toilet_top
        {
            get { return _toilet_top; }
            set { _toilet_top = value; }
        }
        private string _toilet_facility;
        public string toilet_facility
        {
            get { return _toilet_facility; }
            set { _toilet_facility = value; }
        }
        private string _road_name;
        public string road_name
        {
            get { return _road_name; }
            set { _road_name = value; }
        }
        private string _road_width;
        public string road_width
        {
            get { return _road_width; }
            set { _road_width = value; }
        }
        private string _subway_name;
        public string subway_name
        {
            get { return _subway_name; }
            set { _subway_name = value; }
        }
        private string _orientations;
        public string orientations
        {
            get { return _orientations; }
            set { _orientations = value; }
        }
        private string _side_kindergarten;
        public string side_kindergarten
        {
            get { return _side_kindergarten; }
            set { _side_kindergarten = value; }
        }
        private string _side_bank;
        public string side_bank
        {
            get { return _side_bank; }
            set { _side_bank = value; }
        }
        private string _side_primary_school;
        public string side_primary_school
        {
            get { return _side_primary_school; }
            set { _side_primary_school = value; }
        }
        private string _side_middle_school;
        public string side_middle_school
        {
            get { return _side_middle_school; }
            set { _side_middle_school = value; }
        }
        private string _side_marketplace;
        public string side_marketplace
        {
            get { return _side_marketplace; }
            set { _side_marketplace = value; }
        }
        private string _side_post_office;
        public string side_post_office
        {
            get { return _side_post_office; }
            set { _side_post_office = value; }
        }
        private string _side_sports;
        public string side_sports
        {
            get { return _side_sports; }
            set { _side_sports = value; }
        }
        private string _side_club;
        public string side_club
        {
            get { return _side_club; }
            set { _side_club = value; }
        }
        private string _side_hospital;
        public string side_hospital
        {
            get { return _side_hospital; }
            set { _side_hospital = value; }
        }
        private string _side_condition;
        public string side_condition
        {
            get { return _side_condition; }
            set { _side_condition = value; }
        }
        private string _road_horizontal;
        public string road_horizontal
        {
            get { return _road_horizontal; }
            set { _road_horizontal = value; }
        }
        private string _road_next;
        public string road_next
        {
            get { return _road_next; }
            set { _road_next = value; }
        }
        private string _shop_one_two;
        public string shop_one_two
        {
            get { return _shop_one_two; }
            set { _shop_one_two = value; }
        }
        private string _side_road_distance;
        public string side_road_distance
        {
            get { return _side_road_distance; }
            set { _side_road_distance = value; }
        }
        private string _in_shop_long;
        public string in_shop_long
        {
            get { return _in_shop_long; }
            set { _in_shop_long = value; }
        }
        private string _business_road_traffic_flow;
        public string business_road_traffic_flow
        {
            get { return _business_road_traffic_flow; }
            set { _business_road_traffic_flow = value; }
        }
    }
}