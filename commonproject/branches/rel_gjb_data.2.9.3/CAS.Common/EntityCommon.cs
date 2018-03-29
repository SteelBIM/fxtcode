using CAS.Entity.DBEntity;
using System.Collections.Generic;

namespace CAS.Common
{
    public class EntityCommon
    {

    }

    /// <summary>
    /// 菜单
    /// </summary>
    public class Menus
    {
        public string ID;
        public string Class;
        public Link[] Links;
    }

    /// <summary>
    /// 链接
    /// </summary>
    public class Link
    {
        private string href;
        /// <summary>
        /// 链接指向
        /// </summary>
        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        private string target = null;
        /// <summary>
        /// 链接打开的目标
        /// </summary>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        private string id;
        /// <summary>
        /// 链接ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private int index;
        /// <summary>
        /// 链接显示顺序
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private string name;
        /// <summary>
        /// 链接名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string caption;
        /// <summary>
        /// 链接显示
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        private string title = null;
        /// <summary>
        /// 链接提示
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string _class = null;
        /// <summary>
        /// 链接样式
        /// </summary>
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private bool isSelected = false;
        /// <summary>
        /// 是否为选中状态
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
    }

    public class DictionaryHelper
    {
        private static List<SYSProvince> _provincelist;
        public static List<SYSProvince> GetProvinceList()
        {
            if (_provincelist != null)
                return _provincelist;
            _provincelist = new List<SYSProvince>() {new  SYSProvince { provinceid = 1, provincename = "北京直辖市", alias = "北京" },
            new  SYSProvince { provinceid = 2, provincename = "上海直辖市", alias = "上海" },
            new  SYSProvince { provinceid = 3, provincename = "天津直辖市", alias = "天津" },
            new  SYSProvince { provinceid = 4, provincename = "重庆直辖市", alias = "重庆" },
            new  SYSProvince { provinceid = 5, provincename = "广东省", alias = "广东" },
            new  SYSProvince { provinceid = 6, provincename = "黑龙江省", alias = "黑龙江" },
            new  SYSProvince { provinceid = 7, provincename = "吉林省", alias = "吉林" },
            new  SYSProvince { provinceid = 8, provincename = "辽宁省", alias = "辽宁" },
            new  SYSProvince { provinceid = 9, provincename = "河北省", alias = "河北" },
            new  SYSProvince { provinceid = 10, provincename = "河南省", alias = "河南" },
            new  SYSProvince { provinceid = 11, provincename = "湖北省", alias = "湖北" },
            new  SYSProvince { provinceid = 12, provincename = "湖南省", alias = "湖南" },
            new  SYSProvince { provinceid = 13, provincename = "浙江省", alias = "浙江" },
            new  SYSProvince { provinceid = 14, provincename = "江苏省", alias = "江苏" },
            new  SYSProvince { provinceid = 15, provincename = "安徽省", alias = "安徽" },
            new  SYSProvince { provinceid = 16, provincename = "贵州省", alias = "贵州" },
            new  SYSProvince { provinceid = 17, provincename = "福建省", alias = "福建" },
            new  SYSProvince { provinceid = 18, provincename = "四川省", alias = "四川" },
            new  SYSProvince { provinceid = 19, provincename = "山东省", alias = "山东" },
            new  SYSProvince { provinceid = 20, provincename = "山西省", alias = "山西" },
            new  SYSProvince { provinceid = 21, provincename = "新疆维吾尔自治区", alias = "新疆" },
            new  SYSProvince { provinceid = 22, provincename = "内蒙古自治区", alias = "内蒙古" },
            new  SYSProvince { provinceid = 23, provincename = "西藏自治区", alias = "西藏" },
            new  SYSProvince { provinceid = 24, provincename = "青海省", alias = "青海" },
            new  SYSProvince { provinceid = 25, provincename = "宁夏回族自治区", alias = "宁夏" },
            new  SYSProvince { provinceid = 26, provincename = "陕西省", alias = "陕西" },
            new  SYSProvince { provinceid = 27, provincename = "甘肃省", alias = "甘肃" },
            new  SYSProvince { provinceid = 28, provincename = "江西省", alias = "江西" },
            new  SYSProvince { provinceid = 29, provincename = "云南省", alias = "云南" },
            new  SYSProvince { provinceid = 30, provincename = "广西壮族自治区", alias = "广西" },
            new  SYSProvince { provinceid = 31, provincename = "海南省", alias = "海南" },
            new  SYSProvince { provinceid = 32, provincename = "香港特区", alias = "香港" },
            new  SYSProvince { provinceid = 33, provincename = "澳门特区", alias = "澳门" },
            new  SYSProvince { provinceid = 34, provincename = "台湾省", alias = "台湾" }};
            return _provincelist;
        }

        private static List<SYSCity> _citylist;
        public static List<SYSCity> GetCityList()
        {
            if (_citylist != null)
                return _citylist;
            _citylist = new List<SYSCity>() {new  SYSCity {cityid=1,cityname="北京市",alias="北京",provinceid=1},
                new  SYSCity {cityid=2,cityname="上海市",alias="上海",provinceid=2},
                new  SYSCity {cityid=3,cityname="天津市",alias="天津",provinceid=3},
                new  SYSCity {cityid=4,cityname="重庆市",alias="重庆",provinceid=4},
                new  SYSCity {cityid=5,cityname="台北市",alias="台北",provinceid=34},
                new  SYSCity {cityid=6,cityname="深圳市",alias="深圳",provinceid=5},
                new  SYSCity {cityid=7,cityname="广州市",alias="广州",provinceid=5},
                new  SYSCity {cityid=8,cityname="东莞市",alias="东莞",provinceid=5},
                new  SYSCity {cityid=9,cityname="佛山市",alias="佛山",provinceid=5},
                new  SYSCity {cityid=10,cityname="珠海市",alias="珠海",provinceid=5},
                new  SYSCity {cityid=11,cityname="惠州市",alias="惠州",provinceid=5},
                new  SYSCity {cityid=12,cityname="江门市",alias="江门",provinceid=5},
                new  SYSCity {cityid=13,cityname="中山市",alias="中山",provinceid=5},
                new  SYSCity {cityid=14,cityname="清远市",alias="清远",provinceid=5},
                new  SYSCity {cityid=15,cityname="河源市",alias="河源",provinceid=5},
                new  SYSCity {cityid=16,cityname="韶关市",alias="韶关",provinceid=5},
                new  SYSCity {cityid=17,cityname="肇庆市",alias="肇庆",provinceid=5},
                new  SYSCity {cityid=18,cityname="汕头市",alias="汕头",provinceid=5},
                new  SYSCity {cityid=20,cityname="湛江市",alias="湛江",provinceid=5},
                new  SYSCity {cityid=21,cityname="云浮市",alias="云浮",provinceid=5},
                new  SYSCity {cityid=22,cityname="阳江市",alias="阳江",provinceid=5},
                new  SYSCity {cityid=23,cityname="哈尔滨市",alias="哈尔滨",provinceid=6},
                new  SYSCity {cityid=24,cityname="佳木斯市",alias="佳木斯",provinceid=6},
                new  SYSCity {cityid=25,cityname="吉林市",alias="吉林",provinceid=7},
                new  SYSCity {cityid=26,cityname="长春市",alias="长春",provinceid=7},
                new  SYSCity {cityid=27,cityname="大连市",alias="大连",provinceid=8},
                new  SYSCity {cityid=28,cityname="沈阳市",alias="沈阳",provinceid=8},
                new  SYSCity {cityid=29,cityname="鞍山市",alias="鞍山",provinceid=8},
                new  SYSCity {cityid=31,cityname="锦州市",alias="锦州",provinceid=8},
                new  SYSCity {cityid=32,cityname="抚顺市",alias="抚顺",provinceid=8},
                new  SYSCity {cityid=33,cityname="营口市",alias="营口",provinceid=8},
                new  SYSCity {cityid=34,cityname="辽阳市",alias="辽阳",provinceid=8},
                new  SYSCity {cityid=35,cityname="铁岭市",alias="铁岭",provinceid=8},
                new  SYSCity {cityid=36,cityname="三河市",alias="三河",provinceid=9},
                new  SYSCity {cityid=37,cityname="石家庄市",alias="石家庄",provinceid=9},
                new  SYSCity {cityid=38,cityname="邯郸市",alias="邯郸",provinceid=9},
                new  SYSCity {cityid=39,cityname="保定市",alias="保定",provinceid=9},
                new  SYSCity {cityid=40,cityname="张家口市",alias="张家口",provinceid=9},
                new  SYSCity {cityid=41,cityname="承德市",alias="承德",provinceid=9},
                new  SYSCity {cityid=42,cityname="唐山市",alias="唐山",provinceid=9},
                new  SYSCity {cityid=43,cityname="廊坊市",alias="廊坊",provinceid=9},
                new  SYSCity {cityid=44,cityname="沧州市",alias="沧州",provinceid=9},
                new  SYSCity {cityid=47,cityname="秦皇岛市",alias="秦皇岛",provinceid=9},
                new  SYSCity {cityid=48,cityname="郑州市",alias="郑州",provinceid=10},
                new  SYSCity {cityid=49,cityname="商丘市",alias="商丘",provinceid=10},
                new  SYSCity {cityid=50,cityname="漯河市",alias="漯河",provinceid=10},
                new  SYSCity {cityid=51,cityname="洛阳市",alias="洛阳",provinceid=10},
                new  SYSCity {cityid=52,cityname="安阳市",alias="安阳",provinceid=10},
                new  SYSCity {cityid=53,cityname="开封市",alias="开封",provinceid=10},
                new  SYSCity {cityid=54,cityname="平顶山市",alias="平顶山",provinceid=10},
                new  SYSCity {cityid=55,cityname="驻马店市",alias="驻马店",provinceid=10},
                new  SYSCity {cityid=56,cityname="济源市",alias="济源",provinceid=10},
                new  SYSCity {cityid=57,cityname="信阳市",alias="信阳",provinceid=10},
                new  SYSCity {cityid=58,cityname="新乡市",alias="新乡",provinceid=10},
                new  SYSCity {cityid=59,cityname="宜昌市",alias="宜昌",provinceid=11},
                new  SYSCity {cityid=60,cityname="武汉市",alias="武汉",provinceid=11},
                new  SYSCity {cityid=61,cityname="襄阳市",alias="襄樊",provinceid=11},
                new  SYSCity {cityid=62,cityname="黄石市",alias="黄石",provinceid=11},
                new  SYSCity {cityid=63,cityname="孝感市",alias="孝感",provinceid=11},
                new  SYSCity {cityid=64,cityname="鄂州市",alias="鄂州",provinceid=11},
                new  SYSCity {cityid=65,cityname="荆州市",alias="荆州",provinceid=11},
                new  SYSCity {cityid=66,cityname="仙桃市",alias="仙桃",provinceid=11},
                new  SYSCity {cityid=67,cityname="长沙市",alias="长沙",provinceid=12},
                new  SYSCity {cityid=68,cityname="衡阳市",alias="衡阳",provinceid=12},
                new  SYSCity {cityid=69,cityname="岳阳市",alias="岳阳",provinceid=12},
                new  SYSCity {cityid=70,cityname="益阳市",alias="益阳",provinceid=12},
                new  SYSCity {cityid=71,cityname="株洲市",alias="株洲",provinceid=12},
                new  SYSCity {cityid=72,cityname="郴州市",alias="郴州",provinceid=12},
                new  SYSCity {cityid=73,cityname="常德市",alias="常德",provinceid=12},
                new  SYSCity {cityid=74,cityname="杭州市",alias="杭州",provinceid=13},
                new  SYSCity {cityid=75,cityname="宁波市",alias="宁波",provinceid=13},
                new  SYSCity {cityid=76,cityname="温州市",alias="温州",provinceid=13},
                new  SYSCity {cityid=77,cityname="金华市",alias="金华",provinceid=13},
                new  SYSCity {cityid=80,cityname="台州市",alias="台州",provinceid=13},
                new  SYSCity {cityid=81,cityname="泰安市",alias="泰安",provinceid=13},
                new  SYSCity {cityid=83,cityname="湖州市",alias="湖州",provinceid=13},
                new  SYSCity {cityid=84,cityname="南京市",alias="南京",provinceid=14},
                new  SYSCity {cityid=85,cityname="无锡市",alias="无锡",provinceid=14},
                new  SYSCity {cityid=86,cityname="宜兴市",alias="宜兴",provinceid=14},
                new  SYSCity {cityid=87,cityname="徐州市",alias="徐州",provinceid=14},
                new  SYSCity {cityid=88,cityname="苏州市",alias="苏州",provinceid=14},
                new  SYSCity {cityid=89,cityname="扬州市",alias="扬州",provinceid=14},
                new  SYSCity {cityid=90,cityname="镇江市",alias="镇江",provinceid=14},
                new  SYSCity {cityid=91,cityname="南通市",alias="南通",provinceid=14},
                new  SYSCity {cityid=92,cityname="淮安市",alias="淮安",provinceid=14},
                new  SYSCity {cityid=93,cityname="常州市",alias="常州",provinceid=14},
                new  SYSCity {cityid=94,cityname="连云港市",alias="连云港",provinceid=14},
                new  SYSCity {cityid=97,cityname="泰州市",alias="泰州",provinceid=14},
                new  SYSCity {cityid=98,cityname="合肥市",alias="合肥",provinceid=15},
                new  SYSCity {cityid=99,cityname="滁州市",alias="滁州",provinceid=15},
                new  SYSCity {cityid=100,cityname="马鞍山市",alias="马鞍山",provinceid=15},
                new  SYSCity {cityid=101,cityname="芜湖市",alias="芜湖",provinceid=15},
                new  SYSCity {cityid=102,cityname="阜阳市",alias="阜阳",provinceid=15},
                new  SYSCity {cityid=103,cityname="黄山市",alias="黄山",provinceid=15},
                new  SYSCity {cityid=104,cityname="淮南市",alias="淮南",provinceid=15},
                new  SYSCity {cityid=106,cityname="贵阳市",alias="贵阳",provinceid=16},
                new  SYSCity {cityid=107,cityname="遵义市",alias="遵义",provinceid=16},
                new  SYSCity {cityid=108,cityname="铜仁地区",alias="铜仁",provinceid=16},
                new  SYSCity {cityid=109,cityname="福州市",alias="福州",provinceid=17},
                new  SYSCity {cityid=110,cityname="厦门市",alias="厦门",provinceid=17},
                new  SYSCity {cityid=111,cityname="泉州市",alias="泉州",provinceid=17},
                new  SYSCity {cityid=113,cityname="漳州市",alias="漳州",provinceid=17},
                new  SYSCity {cityid=115,cityname="成都市",alias="成都",provinceid=18},
                new  SYSCity {cityid=116,cityname="南充市",alias="南充",provinceid=18},
                new  SYSCity {cityid=117,cityname="德阳市",alias="德阳",provinceid=18},
                new  SYSCity {cityid=119,cityname="泸州市",alias="泸州",provinceid=18},
                new  SYSCity {cityid=120,cityname="济南市",alias="济南",provinceid=19},
                new  SYSCity {cityid=121,cityname="青岛市",alias="青岛",provinceid=19},
                new  SYSCity {cityid=122,cityname="烟台市",alias="烟台",provinceid=19},
                new  SYSCity {cityid=123,cityname="威海市",alias="威海",provinceid=19},
                new  SYSCity {cityid=124,cityname="潍坊市",alias="潍坊",provinceid=19},
                new  SYSCity {cityid=125,cityname="淄博市",alias="淄博",provinceid=19},
                new  SYSCity {cityid=127,cityname="东营市",alias="东营",provinceid=19},
                new  SYSCity {cityid=128,cityname="菏泽市",alias="菏泽",provinceid=19},
                new  SYSCity {cityid=130,cityname="丽水市",alias="丽水",provinceid=19},
                new  SYSCity {cityid=131,cityname="临沂市",alias="临沂",provinceid=19},
                new  SYSCity {cityid=132,cityname="日照市",alias="日照",provinceid=19},
                new  SYSCity {cityid=133,cityname="聊城市",alias="聊城",provinceid=19},
                new  SYSCity {cityid=134,cityname="太原市",alias="太原",provinceid=20},
                new  SYSCity {cityid=135,cityname="大同市",alias="大同",provinceid=20},
                new  SYSCity {cityid=136,cityname="乌鲁木齐市",alias="乌鲁木齐",provinceid=21},
                new  SYSCity {cityid=137,cityname="喀什地区",alias="喀什",provinceid=21},
                new  SYSCity {cityid=139,cityname="克拉玛依市",alias="克拉玛依",provinceid=21},
                new  SYSCity {cityid=140,cityname="赤峰市",alias="赤峰",provinceid=22},
                new  SYSCity {cityid=141,cityname="呼伦贝尔市",alias="呼仑贝尔",provinceid=22},
                new  SYSCity {cityid=142,cityname="鄂尔多斯市",alias="鄂尔多斯",provinceid=22},
                new  SYSCity {cityid=143,cityname="呼和浩特市",alias="呼和浩特",provinceid=22},
                new  SYSCity {cityid=144,cityname="包头市",alias="包头",provinceid=22},
                new  SYSCity {cityid=145,cityname="西宁市",alias="西宁",provinceid=24},
                new  SYSCity {cityid=146,cityname="银川市",alias="银川",provinceid=25},
                new  SYSCity {cityid=147,cityname="西安市",alias="西安",provinceid=26},
                new  SYSCity {cityid=148,cityname="汉中市",alias="汉中",provinceid=26},
                new  SYSCity {cityid=149,cityname="咸阳市",alias="咸阳",provinceid=26},
                new  SYSCity {cityid=150,cityname="兰州市",alias="兰州",provinceid=27},
                new  SYSCity {cityid=151,cityname="南昌市",alias="南昌",provinceid=28},
                new  SYSCity {cityid=152,cityname="景德镇市",alias="景德镇",provinceid=28},
                new  SYSCity {cityid=153,cityname="吉安市",alias="吉安",provinceid=28},
                new  SYSCity {cityid=154,cityname="赣州市",alias="赣州",provinceid=28},
                new  SYSCity {cityid=155,cityname="九江市",alias="九江",provinceid=28},
                new  SYSCity {cityid=157,cityname="昆明市",alias="昆明",provinceid=29},
                new  SYSCity {cityid=158,cityname="保山市",alias="宝山",provinceid=29},
                new  SYSCity {cityid=159,cityname="南宁市",alias="南宁",provinceid=30},
                new  SYSCity {cityid=160,cityname="桂林市",alias="桂林",provinceid=30},
                new  SYSCity {cityid=161,cityname="贵港市",alias="贵港",provinceid=30},
                new  SYSCity {cityid=162,cityname="百色市",alias="百色",provinceid=30},
                new  SYSCity {cityid=163,cityname="玉林市",alias="玉林",provinceid=30},
                new  SYSCity {cityid=164,cityname="贺州市",alias="贺州",provinceid=30},
                new  SYSCity {cityid=165,cityname="海口市",alias="海口",provinceid=31},
                new  SYSCity {cityid=166,cityname="三亚市",alias="三亚",provinceid=31},
                new  SYSCity {cityid=168,cityname="文昌市",alias="文昌",provinceid=31},
                new  SYSCity {cityid=169,cityname="琼海市",alias="琼海",provinceid=31},
                new  SYSCity {cityid=170,cityname="万宁市",alias="万宁",provinceid=31},
                new  SYSCity {cityid=171,cityname="五指山市",alias="五指山",provinceid=31},
                new  SYSCity {cityid=172,cityname="东方市",alias="东方",provinceid=31},
                new  SYSCity {cityid=173,cityname="儋州市",alias="儋州",provinceid=31},
                new  SYSCity {cityid=174,cityname="临高县",alias="临高县",provinceid=31},
                new  SYSCity {cityid=175,cityname="澄迈县",alias="澄迈县",provinceid=31},
                new  SYSCity {cityid=176,cityname="定安县",alias="定安县",provinceid=31},
                new  SYSCity {cityid=177,cityname="屯昌县",alias="屯昌县",provinceid=31},
                new  SYSCity {cityid=178,cityname="昌江黎族自治县",alias="昌江县",provinceid=31},
                new  SYSCity {cityid=179,cityname="白沙黎族自治县",alias="白沙县",provinceid=31},
                new  SYSCity {cityid=180,cityname="琼中黎族苗族自治县",alias="琼中县",provinceid=31},
                new  SYSCity {cityid=181,cityname="陵水黎族自治县",alias="陵水县",provinceid=31},
                new  SYSCity {cityid=182,cityname="保亭黎族苗族自治县",alias="保亭县",provinceid=31},
                new  SYSCity {cityid=183,cityname="乐东黎族自治县",alias="乐东县",provinceid=31},
                new  SYSCity {cityid=184,cityname="西南中沙群岛办事处（县级）",alias="西南中沙群岛",provinceid=31},
                new  SYSCity {cityid=185,cityname="香港",alias="香港",provinceid=32},
                new  SYSCity {cityid=186,cityname="澳门",alias="澳门",provinceid=33},
                new  SYSCity {cityid=187,cityname="绍兴市",alias="绍兴",provinceid=13},
                new  SYSCity {cityid=188,cityname="衢州市",alias="衢州",provinceid=13},
                new  SYSCity {cityid=189,cityname="嘉兴市",alias="嘉兴",provinceid=13},
                new  SYSCity {cityid=190,cityname="湘潭市",alias="湘潭",provinceid=12},
                new  SYSCity {cityid=191,cityname="六安市",alias="六安",provinceid=15},
                new  SYSCity {cityid=192,cityname="曲靖市",alias="曲靖",provinceid=29},
                new  SYSCity {cityid=193,cityname="西双版纳傣族自治州",alias="西双版纳",provinceid=29},
                new  SYSCity {cityid=194,cityname="红河哈尼族彝族自治州",alias="红河",provinceid=29},
                new  SYSCity {cityid=195,cityname="丽江市",alias="丽江",provinceid=29},
                new  SYSCity {cityid=196,cityname="五家渠市",alias="五家渠",provinceid=21},
                new  SYSCity {cityid=197,cityname="柳州市",alias="柳州",provinceid=30},
                new  SYSCity {cityid=198,cityname="梧州市",alias="梧州",provinceid=30},
                new  SYSCity {cityid=199,cityname="北海市",alias="北海",provinceid=30},
                new  SYSCity {cityid=200,cityname="防城港市",alias="防城港",provinceid=30},
                new  SYSCity {cityid=201,cityname="钦州市",alias="钦州",provinceid=30},
                new  SYSCity {cityid=202,cityname="河池市",alias="河池",provinceid=30},
                new  SYSCity {cityid=203,cityname="来宾市",alias="来宾",provinceid=30},
                new  SYSCity {cityid=204,cityname="崇左市",alias="崇左",provinceid=30},
                new  SYSCity {cityid=205,cityname="十堰市",alias="十堰",provinceid=11},
                new  SYSCity {cityid=206,cityname="敦化市",alias="敦化",provinceid=7},
                new  SYSCity {cityid=207,cityname="滨州市",alias="滨州",provinceid=19},
                new  SYSCity {cityid=208,cityname="安顺市",alias="安顺",provinceid=16},
                new  SYSCity {cityid=209,cityname="毕节地区",alias="毕节",provinceid=16},
                new  SYSCity {cityid=210,cityname="六盘水市",alias="六盘水",provinceid=16},
                new  SYSCity {cityid=211,cityname="黔东南苗族侗族自治州",alias="黔东南苗族侗族自治州",provinceid=16},
                new  SYSCity {cityid=212,cityname="黔南布依族苗族自治州",alias="黔南布依族苗族自治州",provinceid=16},
                new  SYSCity {cityid=213,cityname="黔西南布依族苗族自治州",alias="黔西南布依族苗族自治州",provinceid=16},
                new  SYSCity {cityid=214,cityname="茂名市",alias="茂名",provinceid=5},
                new  SYSCity {cityid=215,cityname="潮州市",alias="潮州",provinceid=5},
                new  SYSCity {cityid=216,cityname="揭阳市",alias="揭阳",provinceid=5},
                new  SYSCity {cityid=217,cityname="梅州市",alias="梅州",provinceid=5},
                new  SYSCity {cityid=218,cityname="汕尾市",alias="汕尾",provinceid=5},
                new  SYSCity {cityid=219,cityname="大庆市",alias="大庆",provinceid=6},
                new  SYSCity {cityid=220,cityname="大兴安岭地区",alias="大兴安岭地区",provinceid=6},
                new  SYSCity {cityid=221,cityname="鹤岗市",alias="鹤岗",provinceid=6},
                new  SYSCity {cityid=222,cityname="黑河市",alias="黑河",provinceid=6},
                new  SYSCity {cityid=223,cityname="鸡西市",alias="鸡西",provinceid=6},
                new  SYSCity {cityid=224,cityname="牡丹江市",alias="牡丹江",provinceid=6},
                new  SYSCity {cityid=225,cityname="七台河市",alias="七台河",provinceid=6},
                new  SYSCity {cityid=226,cityname="齐齐哈尔市",alias="齐齐哈尔",provinceid=6},
                new  SYSCity {cityid=227,cityname="双鸭山市",alias="双鸭山",provinceid=6},
                new  SYSCity {cityid=228,cityname="绥化市",alias="绥化",provinceid=6},
                new  SYSCity {cityid=229,cityname="伊春市",alias="伊春",provinceid=6},
                new  SYSCity {cityid=230,cityname="白城市",alias="白城",provinceid=7},
                new  SYSCity {cityid=231,cityname="白山市",alias="白山",provinceid=7},
                new  SYSCity {cityid=232,cityname="辽源市",alias="辽源",provinceid=7},
                new  SYSCity {cityid=233,cityname="四平市",alias="四平",provinceid=7},
                new  SYSCity {cityid=234,cityname="松原市",alias="松原",provinceid=7},
                new  SYSCity {cityid=235,cityname="通化市",alias="通化",provinceid=7},
                new  SYSCity {cityid=236,cityname="延边朝鲜族自治州",alias="延边朝鲜族自治州",provinceid=7},
                new  SYSCity {cityid=237,cityname="本溪市",alias="本溪",provinceid=8},
                new  SYSCity {cityid=238,cityname="朝阳市",alias="朝阳",provinceid=8},
                new  SYSCity {cityid=239,cityname="丹东市",alias="丹东",provinceid=8},
                new  SYSCity {cityid=240,cityname="阜新市",alias="阜新",provinceid=8},
                new  SYSCity {cityid=241,cityname="葫芦岛市",alias="葫芦岛",provinceid=8},
                new  SYSCity {cityid=242,cityname="盘锦市",alias="盘锦",provinceid=8},
                new  SYSCity {cityid=244,cityname="衡水市",alias="衡水",provinceid=9},
                new  SYSCity {cityid=245,cityname="邢台市",alias="邢台",provinceid=9},
                new  SYSCity {cityid=246,cityname="鹤壁市",alias="鹤壁",provinceid=10},
                new  SYSCity {cityid=247,cityname="焦作市",alias="焦作",provinceid=10},
                new  SYSCity {cityid=248,cityname="南阳市",alias="南阳",provinceid=10},
                new  SYSCity {cityid=249,cityname="濮阳市",alias="濮阳",provinceid=10},
                new  SYSCity {cityid=250,cityname="三门峡市",alias="三门峡",provinceid=10},
                new  SYSCity {cityid=251,cityname="许昌市",alias="许昌",provinceid=10},
                new  SYSCity {cityid=252,cityname="周口市",alias="周口",provinceid=10},
                new  SYSCity {cityid=253,cityname="恩施土家族苗族自治州",alias="恩施土家族苗族自治州",provinceid=11},
                new  SYSCity {cityid=254,cityname="黄冈市",alias="黄冈",provinceid=11},
                new  SYSCity {cityid=255,cityname="荆门市",alias="荆门",provinceid=11},
                new  SYSCity {cityid=256,cityname="潜江市",alias="潜江",provinceid=11},
                new  SYSCity {cityid=257,cityname="神农架林区",alias="神农架林区",provinceid=11},
                new  SYSCity {cityid=258,cityname="随州市",alias="随州",provinceid=11},
                new  SYSCity {cityid=259,cityname="天门市",alias="天门",provinceid=11},
                new  SYSCity {cityid=260,cityname="咸宁市",alias="咸宁",provinceid=11},
                new  SYSCity {cityid=261,cityname="怀化市",alias="怀化",provinceid=12},
                new  SYSCity {cityid=262,cityname="娄底市",alias="娄底",provinceid=12},
                new  SYSCity {cityid=263,cityname="邵阳市",alias="邵阳",provinceid=12},
                new  SYSCity {cityid=264,cityname="湘西土家族苗族自治州",alias="湘西土家族苗族自治州",provinceid=12},
                new  SYSCity {cityid=265,cityname="永州市",alias="永州",provinceid=12},
                new  SYSCity {cityid=266,cityname="张家界市",alias="张家界",provinceid=12},
                new  SYSCity {cityid=267,cityname="舟山市",alias="舟山",provinceid=13},
                new  SYSCity {cityid=268,cityname="宿迁市",alias="宿迁",provinceid=14},
                new  SYSCity {cityid=269,cityname="盐城市",alias="盐城",provinceid=14},
                new  SYSCity {cityid=270,cityname="安庆市",alias="安庆",provinceid=15},
                new  SYSCity {cityid=271,cityname="蚌埠市",alias="蚌埠",provinceid=15},
                new  SYSCity {cityid=272,cityname="亳州市",alias="亳州",provinceid=15},
                new  SYSCity {cityid=273,cityname="池州市",alias="池州",provinceid=15},
                new  SYSCity {cityid=274,cityname="淮北市",alias="淮北",provinceid=15},
                new  SYSCity {cityid=275,cityname="宿州市",alias="宿州",provinceid=15},
                new  SYSCity {cityid=276,cityname="铜陵市",alias="铜陵",provinceid=15},
                new  SYSCity {cityid=277,cityname="宣城市",alias="宣城",provinceid=15},
                new  SYSCity {cityid=278,cityname="龙岩市",alias="龙岩",provinceid=17},
                new  SYSCity {cityid=279,cityname="南平市",alias="南平",provinceid=17},
                new  SYSCity {cityid=280,cityname="宁德市",alias="宁德",provinceid=17},
                new  SYSCity {cityid=281,cityname="莆田市",alias="莆田",provinceid=17},
                new  SYSCity {cityid=282,cityname="三明市",alias="三明",provinceid=17},
                new  SYSCity {cityid=283,cityname="阿坝藏族羌族自治州",alias="阿坝藏族羌族自治州",provinceid=18},
                new  SYSCity {cityid=284,cityname="巴中市",alias="巴中",provinceid=18},
                new  SYSCity {cityid=285,cityname="达州市",alias="达州",provinceid=18},
                new  SYSCity {cityid=286,cityname="甘孜藏族自治州",alias="甘孜藏族自治州",provinceid=18},
                new  SYSCity {cityid=287,cityname="广安市",alias="广安",provinceid=18},
                new  SYSCity {cityid=288,cityname="广元市",alias="广元",provinceid=18},
                new  SYSCity {cityid=289,cityname="乐山市",alias="乐山",provinceid=18},
                new  SYSCity {cityid=290,cityname="凉山彝族自治州",alias="凉山彝族自治州",provinceid=18},
                new  SYSCity {cityid=291,cityname="眉山市",alias="眉山",provinceid=18},
                new  SYSCity {cityid=292,cityname="绵阳市",alias="绵阳",provinceid=18},
                new  SYSCity {cityid=293,cityname="内江市",alias="内江",provinceid=18},
                new  SYSCity {cityid=294,cityname="攀枝花市",alias="攀枝花",provinceid=18},
                new  SYSCity {cityid=295,cityname="遂宁市",alias="遂宁",provinceid=18},
                new  SYSCity {cityid=296,cityname="雅安市",alias="雅安",provinceid=18},
                new  SYSCity {cityid=297,cityname="宜宾市",alias="宜宾",provinceid=18},
                new  SYSCity {cityid=298,cityname="资阳市",alias="资阳",provinceid=18},
                new  SYSCity {cityid=299,cityname="自贡市",alias="自贡",provinceid=18},
                new  SYSCity {cityid=300,cityname="德州市",alias="德州",provinceid=19},
                new  SYSCity {cityid=301,cityname="济宁市",alias="济宁",provinceid=19},
                new  SYSCity {cityid=302,cityname="莱芜市",alias="莱芜",provinceid=19},
                new  SYSCity {cityid=303,cityname="枣庄市",alias="枣庄",provinceid=19},
                new  SYSCity {cityid=304,cityname="长治市",alias="长治",provinceid=20},
                new  SYSCity {cityid=305,cityname="晋城市",alias="晋城",provinceid=20},
                new  SYSCity {cityid=306,cityname="晋中市",alias="晋中",provinceid=20},
                new  SYSCity {cityid=307,cityname="临汾市",alias="临汾",provinceid=20},
                new  SYSCity {cityid=308,cityname="吕梁市",alias="吕梁",provinceid=20},
                new  SYSCity {cityid=309,cityname="朔州市",alias="朔州",provinceid=20},
                new  SYSCity {cityid=310,cityname="忻州市",alias="忻州",provinceid=20},
                new  SYSCity {cityid=311,cityname="阳泉市",alias="阳泉",provinceid=20},
                new  SYSCity {cityid=312,cityname="运城市",alias="运城",provinceid=20},
                new  SYSCity {cityid=313,cityname="阿克苏地区",alias="阿克苏地区",provinceid=21},
                new  SYSCity {cityid=314,cityname="阿拉尔市",alias="阿拉尔",provinceid=21},
                new  SYSCity {cityid=315,cityname="阿勒泰地区",alias="阿勒泰地区",provinceid=21},
                new  SYSCity {cityid=316,cityname="巴音郭楞蒙古自治州",alias="巴音郭楞蒙古自治州",provinceid=21},
                new  SYSCity {cityid=317,cityname="北屯市",alias="北屯",provinceid=21},
                new  SYSCity {cityid=318,cityname="博尔塔拉蒙古自治州",alias="博尔塔拉蒙古自治州",provinceid=21},
                new  SYSCity {cityid=319,cityname="昌吉回族自治州",alias="昌吉回族自治州",provinceid=21},
                new  SYSCity {cityid=320,cityname="哈密地区",alias="哈密地区",provinceid=21},
                new  SYSCity {cityid=321,cityname="和田地区",alias="和田地区",provinceid=21},
                new  SYSCity {cityid=322,cityname="克孜勒苏柯尔克孜自治州",alias="克孜勒苏柯尔克孜自治州",provinceid=21},
                new  SYSCity {cityid=323,cityname="石河子市",alias="石河子",provinceid=21},
                new  SYSCity {cityid=324,cityname="塔城地区",alias="塔城地区",provinceid=21},
                new  SYSCity {cityid=325,cityname="铁门关市",alias="铁门关",provinceid=21},
                new  SYSCity {cityid=326,cityname="图木舒克市",alias="图木舒克",provinceid=21},
                new  SYSCity {cityid=327,cityname="吐鲁番地区",alias="吐鲁番地区",provinceid=21},
                new  SYSCity {cityid=328,cityname="伊犁哈萨克自治州",alias="伊犁哈萨克自治州",provinceid=21},
                new  SYSCity {cityid=329,cityname="阿拉善盟",alias="阿拉善盟",provinceid=22},
                new  SYSCity {cityid=330,cityname="巴彦淖尔市",alias="巴彦淖尔",provinceid=22},
                new  SYSCity {cityid=331,cityname="通辽市",alias="通辽",provinceid=22},
                new  SYSCity {cityid=332,cityname="乌海市",alias="乌海",provinceid=22},
                new  SYSCity {cityid=333,cityname="乌兰察布市",alias="乌兰察布",provinceid=22},
                new  SYSCity {cityid=334,cityname="锡林郭勒盟",alias="锡林郭勒盟",provinceid=22},
                new  SYSCity {cityid=335,cityname="兴安盟",alias="兴安盟",provinceid=22},
                new  SYSCity {cityid=336,cityname="阿里地区",alias="阿里地区",provinceid=23},
                new  SYSCity {cityid=337,cityname="昌都地区",alias="昌都地区",provinceid=23},
                new  SYSCity {cityid=338,cityname="拉萨市",alias="拉萨",provinceid=23},
                new  SYSCity {cityid=339,cityname="林芝地区",alias="林芝地区",provinceid=23},
                new  SYSCity {cityid=340,cityname="那曲地区",alias="那曲地区",provinceid=23},
                new  SYSCity {cityid=341,cityname="日喀则地区",alias="日喀则地区",provinceid=23},
                new  SYSCity {cityid=342,cityname="山南地区",alias="山南地区",provinceid=23},
                new  SYSCity {cityid=343,cityname="果洛藏族自治州",alias="果洛藏族自治州",provinceid=24},
                new  SYSCity {cityid=344,cityname="海北藏族自治州",alias="海北藏族自治州",provinceid=24},
                new  SYSCity {cityid=345,cityname="海东市",alias="海东",provinceid=24},
                new  SYSCity {cityid=346,cityname="海南藏族自治州",alias="海南藏族自治州",provinceid=24},
                new  SYSCity {cityid=347,cityname="海西蒙古族藏族自治州",alias="海西蒙古族藏族自治州",provinceid=24},
                new  SYSCity {cityid=348,cityname="黄南藏族自治州",alias="黄南藏族自治州",provinceid=24},
                new  SYSCity {cityid=349,cityname="玉树藏族自治州",alias="玉树藏族自治州",provinceid=24},
                new  SYSCity {cityid=350,cityname="固原市",alias="固原",provinceid=25},
                new  SYSCity {cityid=351,cityname="石嘴山市",alias="石嘴山",provinceid=25},
                new  SYSCity {cityid=352,cityname="吴忠市",alias="吴忠",provinceid=25},
                new  SYSCity {cityid=353,cityname="中卫市",alias="中卫",provinceid=25},
                new  SYSCity {cityid=354,cityname="安康市",alias="安康",provinceid=26},
                new  SYSCity {cityid=355,cityname="宝鸡市",alias="宝鸡",provinceid=26},
                new  SYSCity {cityid=356,cityname="商洛市",alias="商洛",provinceid=26},
                new  SYSCity {cityid=357,cityname="铜川市",alias="铜川",provinceid=26},
                new  SYSCity {cityid=358,cityname="渭南市",alias="渭南",provinceid=26},
                new  SYSCity {cityid=359,cityname="延安市",alias="延安",provinceid=26},
                new  SYSCity {cityid=360,cityname="榆林市",alias="榆林",provinceid=26},
                new  SYSCity {cityid=361,cityname="白银市",alias="白银",provinceid=27},
                new  SYSCity {cityid=362,cityname="定西市",alias="定西",provinceid=27},
                new  SYSCity {cityid=363,cityname="甘南藏族自治州",alias="甘南藏族自治州",provinceid=27},
                new  SYSCity {cityid=364,cityname="嘉峪关市",alias="嘉峪关",provinceid=27},
                new  SYSCity {cityid=365,cityname="金昌市",alias="金昌",provinceid=27},
                new  SYSCity {cityid=366,cityname="酒泉市",alias="酒泉",provinceid=27},
                new  SYSCity {cityid=367,cityname="临夏回族自治州",alias="临夏回族自治州",provinceid=27},
                new  SYSCity {cityid=368,cityname="陇南市",alias="陇南",provinceid=27},
                new  SYSCity {cityid=369,cityname="平凉市",alias="平凉",provinceid=27},
                new  SYSCity {cityid=370,cityname="庆阳市",alias="庆阳",provinceid=27},
                new  SYSCity {cityid=371,cityname="天水市",alias="天水",provinceid=27},
                new  SYSCity {cityid=372,cityname="武威市",alias="武威",provinceid=27},
                new  SYSCity {cityid=373,cityname="张掖市",alias="张掖",provinceid=27},
                new  SYSCity {cityid=374,cityname="抚州市",alias="抚州",provinceid=28},
                new  SYSCity {cityid=375,cityname="萍乡市",alias="萍乡",provinceid=28},
                new  SYSCity {cityid=376,cityname="上饶市",alias="上饶",provinceid=28},
                new  SYSCity {cityid=377,cityname="新余市",alias="新余",provinceid=28},
                new  SYSCity {cityid=378,cityname="宜春市",alias="宜春",provinceid=28},
                new  SYSCity {cityid=379,cityname="鹰潭市",alias="鹰潭",provinceid=28},
                new  SYSCity {cityid=380,cityname="楚雄彝族自治州",alias="楚雄彝族自治州",provinceid=29},
                new  SYSCity {cityid=381,cityname="大理白族自治州",alias="大理白族自治州",provinceid=29},
                new  SYSCity {cityid=382,cityname="德宏傣族景颇族自治州",alias="德宏傣族景颇族自治州",provinceid=29},
                new  SYSCity {cityid=383,cityname="迪庆藏族自治州",alias="迪庆藏族自治州",provinceid=29},
                new  SYSCity {cityid=385,cityname="临沧市",alias="临沧",provinceid=29},
                new  SYSCity {cityid=386,cityname="怒江傈僳族自治州",alias="怒江傈僳族自治州",provinceid=29},
                new  SYSCity {cityid=387,cityname="普洱市",alias="普洱",provinceid=29},
                new  SYSCity {cityid=388,cityname="文山壮族苗族自治州",alias="文山壮族苗族自治州",provinceid=29},
                new  SYSCity {cityid=389,cityname="玉溪市",alias="玉溪",provinceid=29},
                new  SYSCity {cityid=390,cityname="昭通市",alias="昭通",provinceid=29},
                new  SYSCity {cityid=391,cityname="南沙群岛",alias="南沙群岛",provinceid=31},
                new  SYSCity {cityid=392,cityname="三沙市",alias="三沙",provinceid=31},
                new  SYSCity {cityid=393,cityname="西沙群岛",alias="西沙群岛",provinceid=31},
                new  SYSCity {cityid=394,cityname="中沙群岛岛礁及海域",alias="中沙群岛岛礁及海域",provinceid=31}
            };
            return _citylist;
        }

        private static List<SYSArea> _arealist;
        /// <summary>
        /// 区域，如果为空，从接口读取，并保存在static变量中
        /// </summary>
        public static List<SYSArea> AreaList { get { return _arealist; } set { _arealist = value; } }
    }
}
