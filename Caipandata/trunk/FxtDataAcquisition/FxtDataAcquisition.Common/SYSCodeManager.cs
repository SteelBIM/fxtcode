using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Common
{
    public static class SYSCodeManager
    {
        #region (状态code)
        /// <summary>
        /// 
        /// </summary>
        public const int STATECODE_ID = 1035;
        /// <summary>
        /// 土地用途
        /// </summary>
        public const int PURPOSECODE_ID = 1001;
        /// <summary>
        /// 照片类型
        /// </summary>
        public const int PHOTOTYPECODE_ID = 2009;

        #region 楼盘
        /// <summary>
        /// 小区规模
        /// </summary>
        public const int HOUSINGSCALECODE_ID = 1210;

        /// <summary>
        /// 土地规划用途
        /// </summary>
        public const int PLANPURPOSECODE_ID = 1111;
        #endregion

        #region 楼栋

        /// <summary>
        /// 等级
        /// </summary>
        public const int LEVELCODE_ID = 1012;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public const int BUILDINGTYPECODE_ID = 2003;
        /// <summary>
        /// 建筑结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_ID = 2010;
        /// <summary>
        /// 楼栋位置
        /// </summary>
        public const int LOCATIONCODE_ID = 2011;
        /// <summary>
        /// 外墙装修
        /// </summary>
        public const int WALLCODE_ID = 6058;
        /// <summary>
        /// 产权形式
        /// </summary>
        public const int RIGHTCODE_ID = 2007;
        /// <summary>
        /// 内部装修
        /// </summary>
        public const int INNERFITMENTCODE_ID = 6026;
        /// <summary>
        /// 管道燃气
        /// </summary>
        public const int PIPELINEGASCODE_ID = 1213;
        /// <summary>
        /// 采暖方式
        /// </summary>
        public const int HEATINGMODECODE_ID = 1214;
        /// <summary>
        /// 墙体类型
        /// </summary>
        public const int WALLTYPECODE_ID = 1215;
        /// <summary>
        /// 户型面积
        /// </summary>
        public const int BHOUSETYPECODE_ID = 2016;
        #endregion

        #region 房号
        /// <summary>
        /// 房号用途
        /// </summary>
        public const int HOUSEPURPOSECODE_ID = 1002;
        /// <summary>
        /// 房号朝向
        /// </summary>
        public const int HOUSEFRONTCODE_ID = 2004;
        /// <summary>
        /// 房号景观
        /// </summary>
        public const int HOUSESIGHTCODE_ID = 2006;
        /// <summary>
        /// 通风采光
        /// </summary>
        public const int VDCODE_ID = 1216;
        /// <summary>
        /// 户型结构
        /// </summary>
        public const int STRUCTURECODE_ID = 1012;
        /// <summary>
        /// 噪音情况
        /// </summary>
        public const int NOISE_ID = 2025;
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public const int SUBHOUSETYPE_ID = 1015;
        /// <summary>
        /// 户型
        /// </summary>
        public const int HOUSETYPECODE_ID = 4001;
        /// <summary>
        /// 装修
        /// </summary>
        public const int FITMENTCODE_ID = 6026;
        #endregion

        /// <summary>
        /// 数据状态-待分配
        /// </summary>
        public const int STATECODE_1 = 1035001;
        /// <summary>
        /// 数据状态-已分配
        /// </summary>
        public const int STATECODE_2 = 1035002;
        /// <summary>
        /// 数据状态-已接收
        /// </summary>
        public const int STATECODE_3 = 1035003;
        /// <summary>
        /// 数据状态-查勘中
        /// </summary>
        public const int STATECODE_4 = 1035004;
        /// <summary>
        /// 数据状态-已查勘
        /// </summary>
        public const int STATECODE_5 = 1035005;
        /// <summary>
        /// 数据状态-自审通过
        /// </summary>
        public const int STATECODE_6 = 1035006;
        /// <summary>
        /// 数据状态-自审不通过
        /// </summary>
        public const int STATECODE_7 = 1035007;
        /// <summary>
        /// 数据状态-审核通过
        /// </summary>
        public const int STATECODE_8 = 1035008;
        /// <summary>
        /// 数据状态-审核不通过
        /// </summary>
        public const int STATECODE_9 = 1035009;
        /// <summary>
        /// 数据状态-已入库
        /// </summary>
        public const int STATECODE_10 = 1035010;
        #endregion

        #region 土地用途(CODE)
        public const int PURPOSECODE_1 = 1001001;//	居住
        public const int PURPOSECODE_2 = 1001002;//	居住(别墅)
        public const int PURPOSECODE_3 = 1001003;//	居住(洋房)
        public const int PURPOSECODE_4 = 1001004;//	商业
        public const int PURPOSECODE_5 = 1001005;//	办公
        public const int PURPOSECODE_6 = 1001006;//	工业
        public const int PURPOSECODE_7 = 1001007;//	商业、居住
        public const int PURPOSECODE_8 = 1001008;//	商业、办公
        public const int PURPOSECODE_9 = 1001009;//	办公、居住
        public const int PURPOSECODE_10 = 1001010;//停车场
        public const int PURPOSECODE_11 = 1001011;//酒店
        public const int PURPOSECODE_12 = 1001012;//加油站
        public const int PURPOSECODE_13 = 1001013;//综合
        public const int PURPOSECODE_14 = 1001014;//其他
        #endregion

        #region 楼盘

        #region 小区规模
        /// <summary>
        /// 10万㎡以下
        /// </summary>
        public const int HOUSINGSCALECODE_1 = 1210001;
        /// <summary>
        /// 10~20万㎡
        /// </summary>
        public const int HOUSINGSCALECODE_2 = 1210002;
        /// <summary>
        /// 20~50万㎡
        /// </summary>
        public const int HOUSINGSCALECODE_3 = 1210003;
        /// <summary>
        /// 50~100万㎡
        /// </summary>
        public const int HOUSINGSCALECODE_4 = 1210004;
        /// <summary>
        /// 100万㎡以上
        /// </summary>
        public const int HOUSINGSCALECODE_5 = 1210005;
        #endregion
        #region 土地规划用途
        /// <summary>
        /// 居住用地
        /// </summary>
        public const int PLANPURPOSECODE_1 = 1111001;
        /// <summary>
        /// 商业、居住用地
        /// </summary>
        public const int PLANPURPOSECODE_2 = 1111002;
        /// <summary>
        /// 商业用地
        /// </summary>
        public const int PLANPURPOSECODE_3 = 1111003;
        /// <summary>
        /// 商业、办公用地
        /// </summary>
        public const int PLANPURPOSECODE_4 = 1111004;
        /// <summary>
        /// 办公用地
        /// </summary>
        public const int PLANPURPOSECODE_5 = 1111005;
        /// <summary>
        /// 批发零售用地
        /// </summary>
        public const int PLANPURPOSECODE_6 = 1111006;
        /// <summary>
        /// 住宿餐饮用地
        /// </summary>
        public const int PLANPURPOSECODE_7 = 1111007;
        /// <summary>
        /// 商务金融用地
        /// </summary>
        public const int PLANPURPOSECODE_8 = 1111008;
        /// <summary>
        /// 其他商服用地
        /// </summary>
        public const int PLANPURPOSECODE_9 = 1111009;
        /// <summary>
        /// 工业用地
        /// </summary>
        public const int PLANPURPOSECODE_10 = 1111010;
        /// <summary>
        /// 仓储用地
        /// </summary>
        public const int PLANPURPOSECODE_11 = 1111011;
        /// <summary>
        /// 采矿用地
        /// </summary>
        public const int PLANPURPOSECODE_12 = 1111012;
        /// <summary>
        /// 工矿仓储用地
        /// </summary>
        public const int PLANPURPOSECODE_13 = 1111013;
        /// <summary>
        /// 公共管理与公共服务用地
        /// </summary>
        public const int PLANPURPOSECODE_14 = 1111014;
        /// <summary>
        /// 机关团体用地
        /// </summary>
        public const int PLANPURPOSECODE_15 = 1111015;
        /// <summary>
        /// 新闻出版用地
        /// </summary>
        public const int PLANPURPOSECODE_16 = 1111016;
        /// <summary>
        /// 科教用地
        /// </summary>
        public const int PLANPURPOSECODE_17 = 1111017;
        /// <summary>
        /// 医卫慈善用地
        /// </summary>
        public const int PLANPURPOSECODE_18 = 1111018;
        /// <summary>
        /// 文体娱乐用地
        /// </summary>
        public const int PLANPURPOSECODE_19 = 1111019;
        /// <summary>
        /// 公共设施用地
        /// </summary>
        public const int PLANPURPOSECODE_20 = 1111020;
        /// <summary>
        /// 公园与绿地
        /// </summary>
        public const int PLANPURPOSECODE_21 = 1111021;
        /// <summary>
        /// 风景名胜设施用地
        /// </summary>
        public const int PLANPURPOSECODE_22 = 1111022;
        /// <summary>
        /// 特殊用地
        /// </summary>
        public const int PLANPURPOSECODE_23 = 1111023;
        /// <summary>
        /// 交通运输用地
        /// </summary>
        public const int PLANPURPOSECODE_24 = 1111024;
        /// <summary>
        /// 公路用地
        /// </summary>
        public const int PLANPURPOSECODE_25 = 1111025;
        /// <summary>
        /// 街巷用地
        /// </summary>
        public const int PLANPURPOSECODE_26 = 1111026;
        /// <summary>
        /// 机场用地
        /// </summary>
        public const int PLANPURPOSECODE_27 = 1111027;
        /// <summary>
        /// 港口码头用地
        /// </summary>
        public const int PLANPURPOSECODE_28 = 1111028;
        /// <summary>
        /// 管道运输用地
        /// </summary>
        public const int PLANPURPOSECODE_29 = 1111029;
        /// <summary>
        /// 城镇住宅用地
        /// </summary>
        public const int PLANPURPOSECODE_30 = 1111030;
        /// <summary>
        /// 农村宅基地
        /// </summary>
        public const int PLANPURPOSECODE_31 = 1111031;
        /// <summary>
        /// 其他
        /// </summary>
        public const int PLANPURPOSECODE_32 = 1111032;
        #endregion

        #endregion

        #region 楼栋

        #region 等级
        public const int LEVELCODE_1 = 1012001;//优
        public const int LEVELCODE_2 = 1012002;//良
        public const int LEVELCODE_3 = 1012003;//一般
        public const int LEVELCODE_4 = 1012004;//差
        public const int LEVELCODE_5 = 1012005;//很差
        #endregion
        #region 建筑类型
        /// <summary>
        /// 低层
        /// </summary>
        public const int BUILDINGTYPECODE_1 = 2003001;
        /// <summary>
        /// 多层
        /// </summary>
        public const int BUILDINGTYPECODE_2 = 2003002;
        /// <summary>
        /// 小高层
        /// </summary>
        public const int BUILDINGTYPECODE_3 = 2003003;
        /// <summary>
        /// 高层
        /// </summary>
        public const int BUILDINGTYPECODE_4 = 2003004;
        #endregion
        #region 建筑结构
        /// <summary>
        /// 砖木结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_1 = 2010001;
        /// <summary>
        /// 砖混结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_2 = 2010002;
        /// <summary>
        /// 框架结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_3 = 2010003;
        /// <summary>
        /// 框剪结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_4 = 2010004;
        /// <summary>
        /// 框筒结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_5 = 2010005;
        /// <summary>
        /// 钢结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_6 = 2010006;
        /// <summary>
        /// 钢混结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_7 = 2010007;
        /// <summary>
        /// 混合结构
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_8 = 2010008;
        /// <summary>
        /// 内浇外砌
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_9 = 2010009;
        /// <summary>
        /// 内浇外挂
        /// </summary>
        public const int BUILDINGSTRUCTURECODE_10 = 20100010;
        #endregion
        #region 楼栋位置
        /// <summary>
        /// 无特别因素
        /// </summary>
        public const int LOCATIONCODE_1 = 2011001;
        /// <summary>
        /// 临公园、绿地
        /// </summary>
        public const int LOCATIONCODE_2 = 2011002;
        /// <summary>
        /// 临江、河、湖
        /// </summary>
        public const int LOCATIONCODE_3 = 2011003;
        /// <summary>
        /// 临噪音源(路、桥、工厂)
        /// </summary>
        public const int LOCATIONCODE_4 = 2011004;
        /// <summary>
        /// 临垃圾站、医院
        /// </summary>
        public const int LOCATIONCODE_5 = 2011005;
        /// <summary>
        /// 临变电站、高压线
        /// </summary>
        public const int LOCATIONCODE_6 = 2011006;
        /// <summary>
        /// 临其他不利因素
        /// </summary>
        public const int LOCATIONCODE_7 = 2011007;
        /// <summary>
        /// 临小区中庭
        /// </summary>
        public const int LOCATIONCODE_8 = 2011008;
        #endregion
        #region 外墙装修
        /// <summary>
        /// 涂料
        /// </summary>
        public const int WALLCODE_1 = 6058001;
        /// <summary>
        /// 马赛克
        /// </summary>
        public const int WALLCODE_2 = 6058002;
        /// <summary>
        /// 条形砖
        /// </summary>
        public const int WALLCODE_3 = 6058003;
        /// <summary>
        /// 玻璃幕墙
        /// </summary>
        public const int WALLCODE_4 = 6058004;
        /// <summary>
        /// 铝复板
        /// </summary>
        public const int WALLCODE_5 = 6058005;
        /// <summary>
        /// 大理石
        /// </summary>
        public const int WALLCODE_6 = 6058006;
        /// <summary>
        /// 花岗石
        /// </summary>
        public const int WALLCODE_7 = 6058007;
        /// <summary>
        /// 瓷片
        /// </summary>
        public const int WALLCODE_8 = 6058008;
        /// <summary>
        /// 墙砖
        /// </summary>
        public const int WALLCODE_9 = 6058009;
        /// <summary>
        /// 水刷石
        /// </summary>
        public const int WALLCODE_10 = 60580010;
        /// <summary>
        /// 清水墙
        /// </summary>
        public const int WALLCODE_11 = 60580011;
        /// <summary>
        /// 其他
        /// </summary>
        public const int WALLCODE_12 = 60580012;
        /// <summary>
        /// 水泥砂浆
        /// </summary>
        public const int WALLCODE_13 = 60580013;
        #endregion
        #region 内部装修
        /// <summary>
        /// 豪华
        /// </summary>
        public const int INNERFITMENTCODE_1 = 6026001;
        /// <summary>
        /// 高档
        /// </summary>
        public const int INNERFITMENTCODE_2 = 6026002;
        /// <summary>
        /// 中档
        /// </summary>
        public const int INNERFITMENTCODE_3 = 6026003;
        /// <summary>
        /// 普通
        /// </summary>
        public const int INNERFITMENTCODE_4 = 6026004;
        /// <summary>
        /// 简易
        /// </summary>
        public const int INNERFITMENTCODE_5 = 6026005;
        /// <summary>
        /// 毛坯
        /// </summary>
        public const int INNERFITMENTCODE_6 = 6026006;
        #endregion
        #region 管道燃气
        /// <summary>
        /// 管道天然气
        /// </summary>
        public const int PIPELINEGASCODE_1 = 1213001;
        /// <summary>
        /// 管道煤气
        /// </summary>
        public const int PIPELINEGASCODE_2 = 1213002;
        /// <summary>
        /// 无
        /// </summary>
        public const int PIPELINEGASCODE_3 = 1213003;
        #endregion
        #region 采暖方式
        /// <summary>
        /// 集中供暖
        /// </summary>
        public const int HEATINGMODECODE_1 = 1214001;
        /// <summary>
        /// 独户采暖
        /// </summary>
        public const int HEATINGMODECODE_2 = 1214002;
        /// <summary>
        /// 无
        /// </summary>
        public const int HEATINGMODECODE_3 = 1214003;
        #endregion
        #region 墙体类型
        /// <summary>
        /// 加厚墙体
        /// </summary>
        public const int WALLTYPECODE_1 = 1215001;
        /// <summary>
        /// 普通墙体
        /// </summary>
        public const int WALLTYPECODE_2 = 1215002;
        /// <summary>
        /// 不详
        /// </summary>
        public const int WALLTYPECODE_3 = 1215003;
        #endregion
        #region 户型面积
        /// <summary>
        /// 小户型
        /// </summary>
        public const int BHOUSETYPECODE_1 = 2016001;
        /// <summary>
        /// 大户型
        /// </summary>
        public const int BHOUSETYPECODE_2 = 2016002;
        /// <summary>
        /// 复式户型
        /// </summary>
        public const int BHOUSETYPECODE_3 = 2016003;
        /// <summary>
        /// 特殊户型
        /// </summary>
        public const int BHOUSETYPECODE_4 = 2016004;
        #endregion

        #endregion

        #region 房号

        #region 房号用途
        public const int HOUSEPURPOSECODE_1 = 1002001;//普通住宅
        public const int HOUSEPURPOSECODE_2 = 1002002;//非普通住宅
        public const int HOUSEPURPOSECODE_3 = 1002003;//公寓
        public const int HOUSEPURPOSECODE_4 = 1002004;//酒店式公寓
        public const int HOUSEPURPOSECODE_5 = 1002005;//独立别墅
        public const int HOUSEPURPOSECODE_6 = 1002006;//联排别墅
        public const int HOUSEPURPOSECODE_7 = 1002007;//叠加别墅
        public const int HOUSEPURPOSECODE_8 = 1002008;//双拼别墅
        public const int HOUSEPURPOSECODE_9 = 1002009;//旅馆
        public const int HOUSEPURPOSECODE_10 = 1002010;//花园洋房
        public const int HOUSEPURPOSECODE_11 = 1002011;//老洋房
        public const int HOUSEPURPOSECODE_12 = 1002012;//新式里弄
        public const int HOUSEPURPOSECODE_13 = 1002013;//旧式里弄
        public const int HOUSEPURPOSECODE_22 = 1002022;//其他
        public const int HOUSEPURPOSECODE_23 = 1002023;//经济适用房
        public const int HOUSEPURPOSECODE_24 = 1002024;//补差商品住房
        public const int HOUSEPURPOSECODE_25 = 1002025;//地下室,储藏室
        public const int HOUSEPURPOSECODE_26 = 1002026;//车库
        public const int HOUSEPURPOSECODE_27 = 1002027;//别墅
        #endregion

        #region 朝向
        public const int HOUSEFRONTCODE_1 = 2004001;//东
        public const int HOUSEFRONTCODE_2 = 2004002;//南
        public const int HOUSEFRONTCODE_3 = 2004003;//西
        public const int HOUSEFRONTCODE_4 = 2004004;//北
        public const int HOUSEFRONTCODE_5 = 2004005;//东南
        public const int HOUSEFRONTCODE_6 = 2004006;//东北
        public const int HOUSEFRONTCODE_7 = 2004007;//西南
        public const int HOUSEFRONTCODE_8 = 2004008;//西北
        #endregion

        #region 景观
        public const int HOUSESIGHTCODE_1 = 2006001;//公园景观
        public const int HOUSESIGHTCODE_2 = 2006002;//绿地景观
        public const int HOUSESIGHTCODE_3 = 2006003;//小区景观
        public const int HOUSESIGHTCODE_4 = 2006004;//街景
        public const int HOUSESIGHTCODE_5 = 2006005;//市景
        public const int HOUSESIGHTCODE_6 = 2006006;//海景
        public const int HOUSESIGHTCODE_7 = 2006007;//山景
        public const int HOUSESIGHTCODE_8 = 2006008;//江景
        public const int HOUSESIGHTCODE_9 = 2006009;//湖景
        public const int HOUSESIGHTCODE_10 = 20060010;//无特别景观
        public const int HOUSESIGHTCODE_11 = 20060011;//小区绿地
        public const int HOUSESIGHTCODE_12 = 20060012;//河景
        public const int HOUSESIGHTCODE_13 = 20060013;//有建筑物遮挡
        public const int HOUSESIGHTCODE_14 = 20060014;//临高架桥
        public const int HOUSESIGHTCODE_15 = 20060015;//临铁路
        public const int HOUSESIGHTCODE_16 = 20060016;//临其他厌恶设施
        #endregion

        #region 通风采光
        public const int VDCODE_1 = 1216001;//全明通透
        public const int VDCODE_2 = 1216002;//采光欠佳
        public const int VDCODE_3 = 1216003;//通风欠佳
        public const int VDCODE_4 = 1216004;//通风采光欠佳
        #endregion

        #region 户型结构
        public const int STRUCTURECODE_1 = 2005001;//平面
        public const int STRUCTURECODE_2 = 2005002;//跃式
        public const int STRUCTURECODE_3 = 2005003;//复式
        public const int STRUCTURECODE_4 = 2005004;//错层
        public const int STRUCTURECODE_5 = 2005005;//LOFT
        #endregion

        #region 噪音情况
        public const int NOISE_1 = 2025001;//安静
        public const int NOISE_2 = 2025002;//较安静
        public const int NOISE_3 = 2025003;//微吵
        public const int NOISE_4 = 2025004;//较吵
        public const int NOISE_5 = 2025005;//很吵
        #endregion

        #region 户型
        public const int HOUSETYPECODE_1 = 4001001;//单房
        public const int HOUSETYPECODE_2 = 4001002;//单身公寓
        public const int HOUSETYPECODE_3 = 4001003;//一室一厅
        public const int HOUSETYPECODE_4 = 4001004;//两室一厅
        public const int HOUSETYPECODE_5 = 4001005;//两室两厅
        public const int HOUSETYPECODE_6 = 4001006;//三室一厅
        public const int HOUSETYPECODE_7 = 4001007;//三室两厅
        public const int HOUSETYPECODE_8 = 4001008;//四室一厅
        public const int HOUSETYPECODE_9 = 4001009;//四室两厅
        public const int HOUSETYPECODE_10 = 40010010;//四室三厅
        public const int HOUSETYPECODE_11 = 40010011;//五室
        public const int HOUSETYPECODE_12 = 40010012;//六室
        public const int HOUSETYPECODE_13 = 40010013;//七室及以上
        public const int HOUSETYPECODE_14 = 40010014;//一室两厅
        public const int HOUSETYPECODE_15 = 40010015;//两室零厅
        public const int HOUSETYPECODE_16 = 40010016;//三室零厅
        public const int HOUSETYPECODE_17 = 40010017;//四室四厅
        #endregion

        #region 附属房屋类型
        public const int SUBHOUSETYPE_1 = 1015001;//地下室
        public const int SUBHOUSETYPE_2 = 1015002;//杂物间
        public const int SUBHOUSETYPE_3 = 1015003;//车库
        public const int SUBHOUSETYPE_4 = 1015004;//摩托车库
        public const int SUBHOUSETYPE_5 = 1015005;//下房
        public const int SUBHOUSETYPE_6 = 1015006;//储藏室
        public const int SUBHOUSETYPE_7 = 1015007;//阁楼
        public const int SUBHOUSETYPE_8 = 1015008;//厦子
        public const int SUBHOUSETYPE_9 = 1015009;//附房
        public const int SUBHOUSETYPE_10 = 10150010;//夹层
        public const int SUBHOUSETYPE_11 = 10150011;//地下车库
        public const int SUBHOUSETYPE_12 = 10150012;//车位
        #endregion

        #region 装修
        public const int FITMENTCODE_1 = 6026001;//豪华
        public const int FITMENTCODE_2 = 6026002;//高档
        public const int FITMENTCODE_3 = 6026003;//中档
        public const int FITMENTCODE_4 = 6026004;//普通
        public const int FITMENTCODE_5 = 6026005;//简易
        public const int FITMENTCODE_6 = 6026006;//毛坯
        #endregion

        #endregion

        #region 产权形式(CODE)
        public const int RIGHTCODE_1 = 2007001;//商品房
        public const int RIGHTCODE_2 = 2007002;//微利房
        public const int RIGHTCODE_3 = 2007003;//福利房
        public const int RIGHTCODE_4 = 2007004;//军产房
        public const int RIGHTCODE_5 = 2007005;//集资房
        public const int RIGHTCODE_6 = 2007006;//自建房
        public const int RIGHTCODE_7 = 2007007;//经济适用房
        public const int RIGHTCODE_8 = 2007008;//小产权房
        public const int RIGHTCODE_9 = 2007009;//限价房
        public const int RIGHTCODE_10 = 2007010;//解困房
        public const int RIGHTCODE_11 = 2007011;//宅基地
        public const int RIGHTCODE_12 = 2007012;//房改房
        public const int RIGHTCODE_13 = 2007013;//平改房
        public const int RIGHTCODE_14 = 2007014;//回迁房
        public const int RIGHTCODE_15 = 2007015;//回迁房
        #endregion

        #region (数据类型code)
        /// <summary>
        /// 数据类型-楼盘
        /// </summary>
        public const int DATATYPECODE_1 = 1034001;
        /// <summary>
        /// 数据类型-楼栋
        /// </summary>
        public const int DATATYPECODE_2 = 1034002;
        /// <summary>
        /// 数据类型-楼层
        /// </summary>
        public const int DATATYPECODE_3 = 1034003;
        /// <summary>
        /// 数据类型-房号
        /// </summary>
        public const int DATATYPECODE_4 = 1034004;
        #endregion

        #region (操作权限Code(ID:1301))
        /// <summary>
        /// 查看自己
        /// </summary>
        public const int FunOperCode_1 = 1301001;
        /// <summary>
        /// 查看小组
        /// </summary>
        public const int FunOperCode_2 = 1301002;
        /// <summary>
        /// 查看全部
        /// </summary>
        public const int FunOperCode_3 = 1301003;
        /// <summary>
        /// 新增
        /// </summary>
        public const int FunOperCode_4 = 1301004;
        /// <summary>
        /// 修改自己
        /// </summary>
        public const int FunOperCode_5 = 1301005;
        /// <summary>
        /// 修改小组
        /// </summary>
        public const int FunOperCode_6 = 1301006;
        /// <summary>
        /// 修改全部
        /// </summary>
        public const int FunOperCode_7 = 1301007;
        /// <summary>
        /// 删除自己
        /// </summary>
        public const int FunOperCode_8 = 1301008;
        /// <summary>
        /// 删除全部
        /// </summary>
        public const int FunOperCode_9 = 1301009;
        /// <summary>
        /// 导入
        /// </summary>
        public const int FunOperCode_10 = 1301010;
        /// <summary>
        /// 导出自己
        /// </summary>
        public const int FunOperCode_11 = 1301011;
        /// <summary>
        /// 导出小组
        /// </summary>
        public const int FunOperCode_12 = 1301012;
        /// <summary>
        /// 导出全部
        /// </summary>
        public const int FunOperCode_13 = 1301013;
        /// <summary>
        /// 分配任务
        /// </summary>
        public const int FunOperCode_14 = 1301014;
        /// <summary>
        /// 撤销任务
        /// </summary>
        public const int FunOperCode_15 = 1301015;
        /// <summary>
        /// 撤销查勘
        /// </summary>
        public const int FunOperCode_16 = 1301016;
        /// <summary>
        /// 重新分配查勘
        /// </summary>
        public const int FunOperCode_17 = 1301017;
        /// <summary>
        /// 审核自己
        /// </summary>
        public const int FunOperCode_18 = 1301018;
        /// <summary>
        /// 审核小组
        /// </summary>
        public const int FunOperCode_19 = 1301019;
        /// <summary>
        /// 审核全部
        /// </summary>
        public const int FunOperCode_20 = 1301020;
        /// <summary>
        /// 数据库调出
        /// </summary>
        public const int FunOperCode_21 = 1301021;
        /// <summary>
        /// 删除类型操作
        /// </summary>
        public static readonly int[] FunOperCodes_Delete = new int[] { FunOperCode_8, FunOperCode_9 };
        /// <summary>
        /// 修改类型操作
        /// </summary>
        public static readonly int[] FunOperCodes_Update = new int[] { FunOperCode_5, FunOperCode_6, FunOperCode_7 };
        /// <summary>
        /// 审核类型操作
        /// </summary>
        public static readonly int[] FunOperCodes_Audit = new int[] { FunOperCode_18, FunOperCode_19, FunOperCode_20 };
        #endregion

        #region(公司类型Code(ID:2001))
        /// <summary>
        /// 公司类型-开发商
        /// </summary>
        public const int COMPANYTYPECODE_1 = 2001001;
        /// <summary>
        /// 公司类型-物业管理
        /// </summary>
        public const int COMPANYTYPECODE_4 = 2001004;
        #endregion

        #region (配套Code(ID:2008))
        /// <summary>
        /// 学校
        /// </summary>
        public const int APPENDAGECODE_6 = 2008006;
        /// <summary>
        /// 道路通达度
        /// </summary>
        public const int APPENDAGECODE_13 = 2008013;
        /// <summary>
        /// 停车状况
        /// </summary>
        public const int APPENDAGECODE_14 = 2008014;
        /// <summary>
        /// 污染状况
        /// </summary>
        public const int APPENDAGECODE_15 = 2008015;
        /// <summary>
        /// 人文环境
        /// </summary>
        public const int APPENDAGECODE_16 = 2008016;
        /// <summary>
        /// 自然环境
        /// </summary>
        public const int APPENDAGECODE_17 = 2008017;
        /// <summary>
        /// 厌恶设施
        /// </summary>
        public const int APPENDAGECODE_19 = 2008019;
        #endregion

        #region (页面权限承载类型(ID:1202))
        /// <summary>
        /// 菜单
        /// </summary>
        public const int MENUTYPECODE_1 = 1202001;
        /// <summary>
        /// 页面
        /// </summary>
        public const int MENUTYPECODE_2 = 1202002;
        #endregion

        #region (照片类型Code(ID:2009))
        /// <summary>
        /// 照片类型-logo
        /// </summary>
        public const int PHOTOTYPECODE_1 = 2009001;
        /// <summary>
        /// 照片类型-标准层平面图
        /// </summary>
        public const int PHOTOTYPECODE_2 = 2009002;
        /// <summary>
        /// 照片类型-户型图
        /// </summary>
        public const int PHOTOTYPECODE_3 = 2009003;
        /// <summary>
        /// 照片类型-实景图
        /// </summary>
        public const int PHOTOTYPECODE_4 = 2009004;
        /// <summary>
        /// 照片类型-外立面图
        /// </summary>
        public const int PHOTOTYPECODE_5 = 2009005;
        /// <summary>
        /// 照片类型-位置图
        /// </summary>
        public const int PHOTOTYPECODE_6 = 2009006;
        /// <summary>
        /// 照片类型-效果图
        /// </summary>
        public const int PHOTOTYPECODE_7 = 2009007;
        /// <summary>
        /// 照片类型-总平面图
        /// </summary>
        public const int PHOTOTYPECODE_8 = 2009008;
        /// <summary>
        /// 照片类型-其他
        /// </summary>
        public const int PHOTOTYPECODE_9 = 2009009;
        #endregion
    }
}
