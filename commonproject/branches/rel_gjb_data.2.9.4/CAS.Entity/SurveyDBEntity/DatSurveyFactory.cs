using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库查勘工业子表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_Survey_Factory")]
    public class DatSurveyFactory : BaseTO
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
        private string _propertycompanies;
        /// <summary>
        /// 物业管理公司
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
        private string _outwall;
        /// <summary>
        /// 外墙装修
        /// </summary>
        public string outwall
        {
            get { return _outwall; }
            set { _outwall = value; }
        }
        private string _firstfloorhigh;
        /// <summary>
        /// 首层层高
        /// </summary>
        public string firstfloorhigh
        {
            get { return _firstfloorhigh; }
            set { _firstfloorhigh = value; }
        }
        private string _otherfloorhigh;
        /// <summary>
        /// 其它层高
        /// </summary>
        public string otherfloorhigh
        {
            get { return _otherfloorhigh; }
            set { _otherfloorhigh = value; }
        }
        private decimal? _occupygroundarea;
        /// <summary>
        /// 占地面积
        /// </summary>
        public decimal? occupygroundarea
        {
            get { return _occupygroundarea; }
            set { _occupygroundarea = value; }
        }
        private string _factorynumber;
        /// <summary>
        /// 厂房栋数
        /// </summary>
        public string factorynumber
        {
            get { return _factorynumber; }
            set { _factorynumber = value; }
        }
        private string _dormitory;
        /// <summary>
        /// 宿舍栋数
        /// </summary>
        public string dormitory
        {
            get { return _dormitory; }
            set { _dormitory = value; }
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
        private string _usestatus;
        /// <summary>
        /// 使用现状
        /// </summary>
        public string usestatus
        {
            get { return _usestatus; }
            set { _usestatus = value; }
        }
        private string _infloorhigh;
        /// <summary>
        /// 查勘物业层高
        /// </summary>
        public string infloorhigh
        {
            get { return _infloorhigh; }
            set { _infloorhigh = value; }
        }
        private string _buidlingstandard;
        /// <summary>
        /// 查勘厂房建筑标准
        /// </summary>
        public string buidlingstandard
        {
            get { return _buidlingstandard; }
            set { _buidlingstandard = value; }
        }
        private string _decorationnewprobability;
        /// <summary>
        /// 装修成新率
        /// </summary>
        public string decorationnewprobability
        {
            get { return _decorationnewprobability; }
            set { _decorationnewprobability = value; }
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
        private string _toiletsground;
        /// <summary>
        /// 卫生间地面
        /// </summary>
        public string toiletsground
        {
            get { return _toiletsground; }
            set { _toiletsground = value; }
        }
        private string _toiletswall;
        /// <summary>
        /// 卫生间墙面
        /// </summary>
        public string toiletswall
        {
            get { return _toiletswall; }
            set { _toiletswall = value; }
        }
        private string _toiletstop;
        /// <summary>
        /// 卫生间天花
        /// </summary>
        public string toiletstop
        {
            get { return _toiletstop; }
            set { _toiletstop = value; }
        }
        private string _dormitorysumfloor;
        /// <summary>
        /// 宿舍总层数
        /// </summary>
        public string dormitorysumfloor
        {
            get { return _dormitorysumfloor; }
            set { _dormitorysumfloor = value; }
        }
        private string _floorroom;
        /// <summary>
        /// 宿舍每层房间数
        /// </summary>
        public string floorroom
        {
            get { return _floorroom; }
            set { _floorroom = value; }
        }
        private string _corridortype;
        /// <summary>
        /// 宿舍走廊类型
        /// </summary>
        public string corridortype
        {
            get { return _corridortype; }
            set { _corridortype = value; }
        }
        private string _houseshape;
        /// <summary>
        /// 宿舍户型
        /// </summary>
        public string houseshape
        {
            get { return _houseshape; }
            set { _houseshape = value; }
        }
        private string _dormitoryistoilets;
        /// <summary>
        /// 宿舍是否带卫生间
        /// </summary>
        public string dormitoryistoilets
        {
            get { return _dormitoryistoilets; }
            set { _dormitoryistoilets = value; }
        }
        private string _dormitoryiskitchen;
        /// <summary>
        /// 宿舍是否带厨房
        /// </summary>
        public string dormitoryiskitchen
        {
            get { return _dormitoryiskitchen; }
            set { _dormitoryiskitchen = value; }
        }
        private string _dormitoryisterrace;
        /// <summary>
        /// 宿舍是否带阳台
        /// </summary>
        public string dormitoryisterrace
        {
            get { return _dormitoryisterrace; }
            set { _dormitoryisterrace = value; }
        }
        private string _goodselevator;
        /// <summary>
        /// 厂房货梯数量
        /// </summary>
        public string goodselevator
        {
            get { return _goodselevator; }
            set { _goodselevator = value; }
        }
        private string _goodselevatorbrand;
        /// <summary>
        /// 货梯品牌
        /// </summary>
        public string goodselevatorbrand
        {
            get { return _goodselevatorbrand; }
            set { _goodselevatorbrand = value; }
        }
        private string _hangcar;
        /// <summary>
        /// 厂房天车数量
        /// </summary>
        public string hangcar
        {
            get { return _hangcar; }
            set { _hangcar = value; }
        }
        private string _hangcartype;
        /// <summary>
        /// 厂房天车吨位
        /// </summary>
        public string hangcartype
        {
            get { return _hangcartype; }
            set { _hangcartype = value; }
        }
        private string _gourdhang;
        /// <summary>
        /// 电葫芦吊数量
        /// </summary>
        public string gourdhang
        {
            get { return _gourdhang; }
            set { _gourdhang = value; }
        }
        private string _gourdhangtype;
        /// <summary>
        /// 电葫芦吊吨位
        /// </summary>
        public string gourdhangtype
        {
            get { return _gourdhangtype; }
            set { _gourdhangtype = value; }
        }
        private string _firesystems;
        /// <summary>
        /// 是否有管道消防
        /// </summary>
        public string firesystems
        {
            get { return _firesystems; }
            set { _firesystems = value; }
        }
        private string _spraysystems;
        /// <summary>
        /// 是否有自动喷淋系统
        /// </summary>
        public string spraysystems
        {
            get { return _spraysystems; }
            set { _spraysystems = value; }
        }
        private string _smokesystems;
        /// <summary>
        /// 是否有烟感报警系统
        /// </summary>
        public string smokesystems
        {
            get { return _smokesystems; }
            set { _smokesystems = value; }
        }
        private string _intelligentsystems;
        /// <summary>
        /// 是否有智能报警系统
        /// </summary>
        public string intelligentsystems
        {
            get { return _intelligentsystems; }
            set { _intelligentsystems = value; }
        }
        private string _centerrefrigeration;
        /// <summary>
        /// 是否有中央空调
        /// </summary>
        public string centerrefrigeration
        {
            get { return _centerrefrigeration; }
            set { _centerrefrigeration = value; }
        }
        private string _network;
        /// <summary>
        /// 是否有网络
        /// </summary>
        public string network
        {
            get { return _network; }
            set { _network = value; }
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
        /// 离汽车站距离
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
        private string _trafficmanager;
        /// <summary>
        /// 是否有交通管制
        /// </summary>
        public string trafficmanager
        {
            get { return _trafficmanager; }
            set { _trafficmanager = value; }
        }
        private string _sideproject;
        /// <summary>
        /// 周边工业区
        /// </summary>
        public string sideproject
        {
            get { return _sideproject; }
            set { _sideproject = value; }
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
        private string _factortyground;
        /// <summary>
        /// 厂房装修 地面
        /// </summary>
        public string factortyground
        {
            get { return _factortyground; }
            set { _factortyground = value; }
        }
        private string _factortywall;
        /// <summary>
        /// 厂房装修 内墙
        /// </summary>
        public string factortywall
        {
            get { return _factortywall; }
            set { _factortywall = value; }
        }
        private string _factortytop;
        /// <summary>
        /// 厂房装修 天花
        /// </summary>
        public string factortytop
        {
            get { return _factortytop; }
            set { _factortytop = value; }
        }
        private string _dormground;
        /// <summary>
        /// 宿舍装修 地面
        /// </summary>
        public string dormground
        {
            get { return _dormground; }
            set { _dormground = value; }
        }
        private string _dormwall;
        /// <summary>
        /// 宿舍装修 内墙
        /// </summary>
        public string dormwall
        {
            get { return _dormwall; }
            set { _dormwall = value; }
        }
        private string _dormtop;
        /// <summary>
        /// 宿舍装修 天花
        /// </summary>
        public string dormtop
        {
            get { return _dormtop; }
            set { _dormtop = value; }
        }
        private string _dormoutwall;
        /// <summary>
        /// 宿舍装修 外墙
        /// </summary>
        public string dormoutwall
        {
            get { return _dormoutwall; }
            set { _dormoutwall = value; }
        }
        private string _factortyoutwall;
        /// <summary>
        /// 厂房装修 外墙
        /// </summary>
        public string factortyoutwall
        {
            get { return _factortyoutwall; }
            set { _factortyoutwall = value; }
        }
        private string _roadrank;
        /// <summary>
        /// 公路级别
        /// </summary>
        public string roadrank
        {
            get { return _roadrank; }
            set { _roadrank = value; }
        }
        private string _trafficremark;
        /// <summary>
        /// 交通备注
        /// </summary>
        public string trafficremark
        {
            get { return _trafficremark; }
            set { _trafficremark = value; }
        }
        private string _greendegree;
        /// <summary>
        /// 绿化程度
        /// </summary>
        public string greendegree
        {
            get { return _greendegree; }
            set { _greendegree = value; }
        }
        private string _cleandegree;
        /// <summary>
        /// 卫生条件
        /// </summary>
        public string cleandegree
        {
            get { return _cleandegree; }
            set { _cleandegree = value; }
        }
        private string _lightingdegree;
        /// <summary>
        /// 采光条件
        /// </summary>
        public string lightingdegree
        {
            get { return _lightingdegree; }
            set { _lightingdegree = value; }
        }
        private string _winddegree;
        /// <summary>
        /// 通风
        /// </summary>
        public string winddegree
        {
            get { return _winddegree; }
            set { _winddegree = value; }
        }
        private string _airdegree;
        /// <summary>
        /// 空气质量
        /// </summary>
        public string airdegree
        {
            get { return _airdegree; }
            set { _airdegree = value; }
        }
        private string _noisedegree;
        /// <summary>
        /// 噪声污染
        /// </summary>
        public string noisedegree
        {
            get { return _noisedegree; }
            set { _noisedegree = value; }
        }
        private decimal? _structarea;
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal? structarea
        {
            get { return _structarea; }
            set { _structarea = value; }
        }
        private string _factortykind;
        /// <summary>
        /// 厂房类型
        /// </summary>
        public string factortykind
        {
            get { return _factortykind; }
            set { _factortykind = value; }
        }
        private string _factortyspanlen;
        /// <summary>
        /// 跨度(距离)
        /// </summary>
        public string factortyspanlen
        {
            get { return _factortyspanlen; }
            set { _factortyspanlen = value; }
        }
        private string _factortyspancount;
        /// <summary>
        /// 跨度(个数)
        /// </summary>
        public string factortyspancount
        {
            get { return _factortyspancount; }
            set { _factortyspancount = value; }
        }
        private string _factortypillarlen;
        /// <summary>
        /// 柱间距(距离)
        /// </summary>
        public string factortypillarlen
        {
            get { return _factortypillarlen; }
            set { _factortypillarlen = value; }
        }
        private string _factortypillarcount;
        /// <summary>
        /// 柱间距(个数)
        /// </summary>
        public string factortypillarcount
        {
            get { return _factortypillarcount; }
            set { _factortypillarcount = value; }
        }
        private string _industrybuilding;
        /// <summary>
        /// 工业楼栋
        /// </summary>
        public string industrybuilding
        {
            get { return _industrybuilding; }
            set { _industrybuilding = value; }
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
        private string _industrytype;
        /// <summary>
        /// 工业类型
        /// </summary>
        public string industrytype
        {
            get { return _industrytype; }
            set { _industrytype = value; }
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