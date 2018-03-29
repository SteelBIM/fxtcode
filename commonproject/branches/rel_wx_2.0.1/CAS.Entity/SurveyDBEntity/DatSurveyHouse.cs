using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库查勘住宅子表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_Survey_House")]
    public class DatSurveyHouse : BaseTO
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
        /// 房讯通公司ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
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
        private string _estimatefloor;
        /// <summary>
        /// 评估楼层
        /// </summary>
        public string estimatefloor
        {
            get { return _estimatefloor; }
            set { _estimatefloor = value; }
        }
        private string _number;
        /// <summary>
        /// 层户数
        /// </summary>
        public string number
        {
            get { return _number; }
            set { _number = value; }
        }
        private string _completetime;
        /// <summary>
        /// 竣工时间
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
        private string _basementfloor;
        /// <summary>
        /// 地下室层数
        /// </summary>
        public string basementfloor
        {
            get { return _basementfloor; }
            set { _basementfloor = value; }
        }
        private string _wall;
        /// <summary>
        /// 外墙装修
        /// </summary>
        public string wall
        {
            get { return _wall; }
            set { _wall = value; }
        }
        private string _structnewprobability;
        /// <summary>
        /// 结构成新率
        /// </summary>
        public string structnewprobability
        {
            get { return _structnewprobability; }
            set { _structnewprobability = value; }
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
        /// 物管费
        /// </summary>
        public string propertyprice
        {
            get { return _propertyprice; }
            set { _propertyprice = value; }
        }
        private string _villa;
        /// <summary>
        /// 小区别墅栋数
        /// </summary>
        public string villa
        {
            get { return _villa; }
            set { _villa = value; }
        }
        private string _averagehouse;
        /// <summary>
        /// 小区普通住宅栋数
        /// </summary>
        public string averagehouse
        {
            get { return _averagehouse; }
            set { _averagehouse = value; }
        }
        private string _notaveragehouse;
        /// <summary>
        /// 小区非普通住宅栋数
        /// </summary>
        public string notaveragehouse
        {
            get { return _notaveragehouse; }
            set { _notaveragehouse = value; }
        }
        private string _greenenvironment;
        /// <summary>
        /// 绿化环境
        /// </summary>
        public string greenenvironment
        {
            get { return _greenenvironment; }
            set { _greenenvironment = value; }
        }
        private string _noisepollution;
        /// <summary>
        /// 噪声污染
        /// </summary>
        public string noisepollution
        {
            get { return _noisepollution; }
            set { _noisepollution = value; }
        }
        private string _airquality;
        /// <summary>
        /// 空气质量
        /// </summary>
        public string airquality
        {
            get { return _airquality; }
            set { _airquality = value; }
        }
        private string _isupcarlocation;
        /// <summary>
        /// 地上车位
        /// </summary>
        public string isupcarlocation
        {
            get { return _isupcarlocation; }
            set { _isupcarlocation = value; }
        }
        private string _isdowncarlocation;
        /// <summary>
        /// 地下车位
        /// </summary>
        public string isdowncarlocation
        {
            get { return _isdowncarlocation; }
            set { _isdowncarlocation = value; }
        }
        private string _iscar;
        /// <summary>
        /// 车位是否充足
        /// </summary>
        public string iscar
        {
            get { return _iscar; }
            set { _iscar = value; }
        }
        private string _caroccupy;
        /// <summary>
        /// 车户比
        /// </summary>
        public string caroccupy
        {
            get { return _caroccupy; }
            set { _caroccupy = value; }
        }
        private string _status;
        /// <summary>
        /// 使用现状
        /// </summary>
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        private string _direction;
        /// <summary>
        /// 朝向
        /// </summary>
        public string direction
        {
            get { return _direction; }
            set { _direction = value; }
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
        private string _decorationprobabilit;
        /// <summary>
        /// 装修成新率
        /// </summary>
        public string decorationprobabilit
        {
            get { return _decorationprobabilit; }
            set { _decorationprobabilit = value; }
        }
        private string _lv;
        /// <summary>
        /// 装修档次
        /// </summary>
        public string lv
        {
            get { return _lv; }
            set { _lv = value; }
        }
        private string _housecount;
        /// <summary>
        /// 房
        /// </summary>
        public string housecount
        {
            get { return _housecount; }
            set { _housecount = value; }
        }
        private string _hallcount;
        /// <summary>
        /// 厅
        /// </summary>
        public string hallcount
        {
            get { return _hallcount; }
            set { _hallcount = value; }
        }
        private string _balconynumber;
        /// <summary>
        /// 阳台
        /// </summary>
        public string balconynumber
        {
            get { return _balconynumber; }
            set { _balconynumber = value; }
        }
        private string _bathroomcount;
        /// <summary>
        /// 卫
        /// </summary>
        public string bathroomcount
        {
            get { return _bathroomcount; }
            set { _bathroomcount = value; }
        }
        private string _haskitchen;
        /// <summary>
        /// 厨房
        /// </summary>
        public string haskitchen
        {
            get { return _haskitchen; }
            set { _haskitchen = value; }
        }
        private string _housestruct;
        /// <summary>
        /// 户型结构
        /// </summary>
        public string housestruct
        {
            get { return _housestruct; }
            set { _housestruct = value; }
        }
        private string _singlebalcony;
        /// <summary>
        /// 单层阳台
        /// </summary>
        public string singlebalcony
        {
            get { return _singlebalcony; }
            set { _singlebalcony = value; }
        }
        private string _tallbalcony;
        /// <summary>
        /// 高挑阳台
        /// </summary>
        public string tallbalcony
        {
            get { return _tallbalcony; }
            set { _tallbalcony = value; }
        }
        private decimal? _terrace;
        /// <summary>
        /// 露台面积
        /// </summary>
        public decimal? terrace
        {
            get { return _terrace; }
            set { _terrace = value; }
        }
        private decimal? _roof;
        /// <summary>
        /// 天台露台
        /// </summary>
        public decimal? roof
        {
            get { return _roof; }
            set { _roof = value; }
        }
        private decimal? _garden;
        /// <summary>
        /// 入户花园露台
        /// </summary>
        public decimal? garden
        {
            get { return _garden; }
            set { _garden = value; }
        }
        private string _toiletshealth;
        /// <summary>
        /// 洗手间-卫生洁具
        /// </summary>
        public string toiletshealth
        {
            get { return _toiletshealth; }
            set { _toiletshealth = value; }
        }
        private string _toiletsbath;
        /// <summary>
        /// 洗手间-浴具
        /// </summary>
        public string toiletsbath
        {
            get { return _toiletsbath; }
            set { _toiletsbath = value; }
        }
        private string _toiletsground;
        /// <summary>
        /// 洗手间-地面
        /// </summary>
        public string toiletsground
        {
            get { return _toiletsground; }
            set { _toiletsground = value; }
        }
        private string _toilet;
        /// <summary>
        /// 洗手间-座便器
        /// </summary>
        public string toilet
        {
            get { return _toilet; }
            set { _toilet = value; }
        }
        private string _toiletswall;
        /// <summary>
        /// 洗手间-墙面
        /// </summary>
        public string toiletswall
        {
            get { return _toiletswall; }
            set { _toiletswall = value; }
        }
        private string _toiletsceiling;
        /// <summary>
        /// 洗手间-天花
        /// </summary>
        public string toiletsceiling
        {
            get { return _toiletsceiling; }
            set { _toiletsceiling = value; }
        }
        private string _kitchendesk;
        /// <summary>
        /// 厨房-工作台
        /// </summary>
        public string kitchendesk
        {
            get { return _kitchendesk; }
            set { _kitchendesk = value; }
        }
        private string _kitchenground;
        /// <summary>
        /// 厨房-地面
        /// </summary>
        public string kitchenground
        {
            get { return _kitchenground; }
            set { _kitchenground = value; }
        }
        private string _kitchenwall;
        /// <summary>
        /// 厨房-墙面
        /// </summary>
        public string kitchenwall
        {
            get { return _kitchenwall; }
            set { _kitchenwall = value; }
        }
        private string _kitchenceiling;
        /// <summary>
        /// 厨房-天花
        /// </summary>
        public string kitchenceiling
        {
            get { return _kitchenceiling; }
            set { _kitchenceiling = value; }
        }
        private string _bedroomground;
        /// <summary>
        /// 卧室-地面
        /// </summary>
        public string bedroomground
        {
            get { return _bedroomground; }
            set { _bedroomground = value; }
        }
        private string _bedroomwall;
        /// <summary>
        /// 卧室-墙面
        /// </summary>
        public string bedroomwall
        {
            get { return _bedroomwall; }
            set { _bedroomwall = value; }
        }
        private string _bedroomceiling;
        /// <summary>
        /// 卧室-天花
        /// </summary>
        public string bedroomceiling
        {
            get { return _bedroomceiling; }
            set { _bedroomceiling = value; }
        }
        private string _parlorground;
        /// <summary>
        /// 客厅-地面
        /// </summary>
        public string parlorground
        {
            get { return _parlorground; }
            set { _parlorground = value; }
        }
        private string _parlorwall;
        /// <summary>
        /// 客厅-墙面
        /// </summary>
        public string parlorwall
        {
            get { return _parlorwall; }
            set { _parlorwall = value; }
        }
        private string _parlorceiling;
        /// <summary>
        /// 客厅-天花
        /// </summary>
        public string parlorceiling
        {
            get { return _parlorceiling; }
            set { _parlorceiling = value; }
        }
        private string _bigdoor;
        /// <summary>
        /// 入户门
        /// </summary>
        public string bigdoor
        {
            get { return _bigdoor; }
            set { _bigdoor = value; }
        }
        private string _indoor;
        /// <summary>
        /// 内门
        /// </summary>
        public string indoor
        {
            get { return _indoor; }
            set { _indoor = value; }
        }
        private string _roomdoor;
        /// <summary>
        /// 房门
        /// </summary>
        public string roomdoor
        {
            get { return _roomdoor; }
            set { _roomdoor = value; }
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
        private string _clientelevator;
        /// <summary>
        /// 客梯
        /// </summary>
        public string clientelevator
        {
            get { return _clientelevator; }
            set { _clientelevator = value; }
        }
        private string _fireelevator;
        /// <summary>
        /// 消防梯
        /// </summary>
        public string fireelevator
        {
            get { return _fireelevator; }
            set { _fireelevator = value; }
        }
        private string _cabinbrand;
        /// <summary>
        /// 客梯品牌
        /// </summary>
        public string cabinbrand
        {
            get { return _cabinbrand; }
            set { _cabinbrand = value; }
        }
        private string _ladderbrand;
        /// <summary>
        /// 消防梯品牌
        /// </summary>
        public string ladderbrand
        {
            get { return _ladderbrand; }
            set { _ladderbrand = value; }
        }
        private string _intelligentsystems;
        /// <summary>
        /// 智能系统
        /// </summary>
        public string intelligentsystems
        {
            get { return _intelligentsystems; }
            set { _intelligentsystems = value; }
        }
        private string _smokesystems;
        /// <summary>
        /// 烟感报警
        /// </summary>
        public string smokesystems
        {
            get { return _smokesystems; }
            set { _smokesystems = value; }
        }
        private string _spraysystems;
        /// <summary>
        /// 自动喷淋
        /// </summary>
        public string spraysystems
        {
            get { return _spraysystems; }
            set { _spraysystems = value; }
        }
        private string _gassystems;
        /// <summary>
        /// 管道燃气
        /// </summary>
        public string gassystems
        {
            get { return _gassystems; }
            set { _gassystems = value; }
        }
        private string _intercomsystems;
        /// <summary>
        /// 对讲系统
        /// </summary>
        public string intercomsystems
        {
            get { return _intercomsystems; }
            set { _intercomsystems = value; }
        }
        private string _broadband;
        /// <summary>
        /// 宽带
        /// </summary>
        public string broadband
        {
            get { return _broadband; }
            set { _broadband = value; }
        }
        private string _cabletelevision;
        /// <summary>
        /// 有线电视
        /// </summary>
        public string cabletelevision
        {
            get { return _cabletelevision; }
            set { _cabletelevision = value; }
        }
        private string _phone;
        /// <summary>
        /// 电话
        /// </summary>
        public string phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        private string _heating;
        /// <summary>
        /// 暖气
        /// </summary>
        public string heating
        {
            get { return _heating; }
            set { _heating = value; }
        }
        private string _movement;
        /// <summary>
        /// 运动场所
        /// </summary>
        public string movement
        {
            get { return _movement; }
            set { _movement = value; }
        }
        private string _club;
        /// <summary>
        /// 会所
        /// </summary>
        public string club
        {
            get { return _club; }
            set { _club = value; }
        }
        private string _healthcenter;
        /// <summary>
        /// 社康中心
        /// </summary>
        public string healthcenter
        {
            get { return _healthcenter; }
            set { _healthcenter = value; }
        }
        private string _postoffice;
        /// <summary>
        /// 邮局
        /// </summary>
        public string postoffice
        {
            get { return _postoffice; }
            set { _postoffice = value; }
        }
        private string _bank;
        /// <summary>
        /// 银行
        /// </summary>
        public string bank
        {
            get { return _bank; }
            set { _bank = value; }
        }
        private string _market;
        /// <summary>
        /// 商场/菜市场
        /// </summary>
        public string market
        {
            get { return _market; }
            set { _market = value; }
        }
        private string _highschool;
        /// <summary>
        /// 中学
        /// </summary>
        public string highschool
        {
            get { return _highschool; }
            set { _highschool = value; }
        }
        private string _primaryschool;
        /// <summary>
        /// 小学
        /// </summary>
        public string primaryschool
        {
            get { return _primaryschool; }
            set { _primaryschool = value; }
        }
        private string _nursery;
        /// <summary>
        /// 幼儿园
        /// </summary>
        public string nursery
        {
            get { return _nursery; }
            set { _nursery = value; }
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
        private string _trafficmanagement;
        /// <summary>
        /// 是否有交通管制
        /// </summary>
        public string trafficmanagement
        {
            get { return _trafficmanagement; }
            set { _trafficmanagement = value; }
        }
        private string _busdistance;
        /// <summary>
        /// 离公交站台距离
        /// </summary>
        public string busdistance
        {
            get { return _busdistance; }
            set { _busdistance = value; }
        }
        private string _subwaydistance;
        /// <summary>
        /// 离地铁站距离
        /// </summary>
        public string subwaydistance
        {
            get { return _subwaydistance; }
            set { _subwaydistance = value; }
        }
        private string _highway;
        /// <summary>
        /// 公交线路
        /// </summary>
        public string highway
        {
            get { return _highway; }
            set { _highway = value; }
        }
        private string _side;
        /// <summary>
        /// 周边住宅
        /// </summary>
        public string side
        {
            get { return _side; }
            set { _side = value; }
        }
        private string _environment;
        /// <summary>
        /// 小区及周边环境
        /// </summary>
        public string environment
        {
            get { return _environment; }
            set { _environment = value; }
        }
        private string _metro;
        /// <summary>
        /// 地铁线路
        /// </summary>
        public string metro
        {
            get { return _metro; }
            set { _metro = value; }
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
        private string _ventilation;
        /// <summary>
        /// 通风采光
        /// </summary>
        public string ventilation
        {
            get { return _ventilation; }
            set { _ventilation = value; }
        }
        private string _lvyear;
        /// <summary>
        /// 装修年代
        /// </summary>
        public string lvyear
        {
            get { return _lvyear; }
            set { _lvyear = value; }
        }
        private string _surveyhousetarget;
        /// <summary>
        /// 查勘目的：撰写评估报告  价格核定
        /// </summary>
        public string surveyhousetarget
        {
            get { return _surveyhousetarget; }
            set { _surveyhousetarget = value; }
        }
        private string _sub_business_content;
        /// <summary>
        /// 裙楼经营内容
        /// </summary>
        public string sub_business_content
        {
            get { return _sub_business_content; }
            set { _sub_business_content = value; }
        }
        private string _survey_house_age;
        /// <summary>
        /// 楼龄
        /// </summary>
        public string survey_house_age
        {
            get { return _survey_house_age; }
            set { _survey_house_age = value; }
        }
        private string _singlebalcony_area;
        /// <summary>
        /// 单层阳台面积
        /// </summary>
        public string singlebalcony_area
        {
            get { return _singlebalcony_area; }
            set { _singlebalcony_area = value; }
        }
        private string _tallbalcony_area;
        /// <summary>
        /// 高挑阳台面积
        /// </summary>
        public string tallbalcony_area
        {
            get { return _tallbalcony_area; }
            set { _tallbalcony_area = value; }
        }
        private string _road_width;
        /// <summary>
        /// 路宽
        /// </summary>
        public string road_width
        {
            get { return _road_width; }
            set { _road_width = value; }
        }
        private string _road_name;
        /// <summary>
        /// 路名
        /// </summary>
        public string road_name
        {
            get { return _road_name; }
            set { _road_name = value; }
        }
        private string _road_traffic_flow;
        /// <summary>
        /// 道路及车流量
        /// </summary>
        public string road_traffic_flow
        {
            get { return _road_traffic_flow; }
            set { _road_traffic_flow = value; }
        }
        private string _survey_house_kitchen_cupboards;
        public string survey_house_kitchen_cupboards
        {
            get { return _survey_house_kitchen_cupboards; }
            set { _survey_house_kitchen_cupboards = value; }
        }
    }
}