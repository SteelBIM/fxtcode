using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Application.Services
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
        /// 产权形式
        /// </summary>
        public const int RIGHTCODE_ID = 2007;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public const int BUILDINGTYPECODE_ID = 2003;
        /// <summary>
        /// 等级
        /// </summary>
        public const int LEVELCODE_ID = 1012;
        /// <summary>
        /// 户型结构
        /// </summary>
        public const int STRUCTURECODE_ID = 1012;
        /// <summary>
        /// 通风采光
        /// </summary>
        public const int VDCODE_ID = 1216;
        /// <summary>
        /// 噪音情况
        /// </summary>
        public const int NOISE_ID = 2025;
        /// <summary>
        /// 房号用途
        /// </summary>
        public const int HOUSEPURPOSECODE_ID = 1002;

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
        public static readonly List<SYSCode> STATECODE_LIST = new List<SYSCode>();
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

        #region 建筑类型(CODE)
        //BUILDINGTYPE
        public const int BUILDINGTYPECODE_1 = 2003001;//低层
        public const int BUILDINGTYPECODE_2 = 2003002;//多层
        public const int BUILDINGTYPECODE_3 = 2003003;//小高层
        public const int BUILDINGTYPECODE_4 = 2003004;//高层
        #endregion                   CODE

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
        public const int COMPANYTYPECODE_1 = SYSCodeApi.COMPANYTYPECODE_1;
        /// <summary>
        /// 公司类型-物业管理
        /// </summary>
        public const int COMPANYTYPECODE_4 = SYSCodeApi.COMPANYTYPECODE_4;
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
        /// 照片类型-其他
        /// </summary>
        public const int PHOTOTYPECODE_9 = 2009009;
        #endregion

        #region 户型结构
        public const int STRUCTURECODE_1 = 2005001;//平面
        public const int STRUCTURECODE_2 = 2005002;//跃式
        public const int STRUCTURECODE_3 = 2005003;//复式
        public const int STRUCTURECODE_4 = 2005004;//错层
        public const int STRUCTURECODE_5 = 2005005;//LOFT
        #endregion

        #region 通风采光
        public const int VDCODE_1 = 1216001;//全明通透
        public const int VDCODE_2 = 1216002;//采光欠佳
        public const int VDCODE_3 = 1216003;//通风欠佳
        public const int VDCODE_4 = 1216004;//通风采光欠佳
        #endregion

        #region 噪音情况
        public const int NOISE_1 = 2025001;//安静
        public const int NOISE_2 = 2025002;//较安静
        public const int NOISE_3 = 2025003;//微吵
        public const int NOISE_4 = 2025004;//较吵
        public const int NOISE_5 = 2025005;//很吵
        #endregion


        #region 等级
        public const int LEVELCODE_1 = 1012001;//优
        public const int LEVELCODE_2 = 1012002;//良
        public const int LEVELCODE_3 = 1012003;//一般
        public const int LEVELCODE_4 = 1012004;//差
        public const int LEVELCODE_5 = 1012005;//很差
        #endregion

        static SYSCodeManager()
        {
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_1, CodeName = "待分配", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_2, CodeName = "已分配", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_3, CodeName = "已接受", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_4, CodeName = "查勘中", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_5, CodeName = "已查勘", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_6, CodeName = "自审通过", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_7, CodeName = "自审不通过", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_8, CodeName = "审核通过", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_9, CodeName = "审核不通过", ID = STATECODE_ID, CodeType = "任务状态" });
            STATECODE_LIST.Add(new SYSCode { Code = STATECODE_10, CodeName = "已入库", ID = STATECODE_ID, CodeType = "任务状态" });
        }
        /// <summary>
        /// 建筑类型code列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> BuildingTypeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = BUILDINGTYPECODE_1, CodeName  = "低层",  ID = BUILDINGTYPECODE_ID, CodeType = "建筑类型" },
                new SYSCode { Code = BUILDINGTYPECODE_2, CodeName  = "多层",  ID = BUILDINGTYPECODE_ID, CodeType = "建筑类型" },
                new SYSCode { Code = BUILDINGTYPECODE_3, CodeName  = "小高层",ID = BUILDINGTYPECODE_ID, CodeType = "建筑类型" },
                new SYSCode { Code = BUILDINGTYPECODE_4, CodeName  = "高层",  ID = BUILDINGTYPECODE_ID, CodeType = "建筑类型" }
             };
            return list;
        }
        /// <summary>
        /// 产权形式code列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> RightCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = RIGHTCODE_1, CodeName  = "商品房",       ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_2, CodeName  = "微利房",    ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_3, CodeName  = "福利房",    ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_4, CodeName  = "军产房",       ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_5, CodeName  = "集资房",       ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_6, CodeName  = "自建房",     ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_7, CodeName  = "经济适用房", ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_8, CodeName  = "小产权房",   ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_9, CodeName  = "限价房",     ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_10, CodeName = "解困房",     ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_11, CodeName = "宅基地",     ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_12, CodeName = "房改房",     ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_13, CodeName = "平改房",     ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_14, CodeName = "回迁房",     ID = RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = RIGHTCODE_15, CodeName = "安置房",     ID = RIGHTCODE_ID, CodeType = "产权形式" }
            };
            return list;
        }
        /// <summary>
        /// 土地用途code列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> PurposeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = PURPOSECODE_1, CodeName  = "居住",       ID = PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = PURPOSECODE_2, CodeName  = "居住(别墅)", ID = PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = PURPOSECODE_3, CodeName  = "居住(洋房)", ID = PURPOSECODE_ID, CodeType = "土地用途" },
            //new SYSCode { Code = PURPOSECODE_4, CodeName  = "商业",       ID = PURPOSECODE_ID, CodeType = "土地用途" },
            //new SYSCode { Code = PURPOSECODE_5, CodeName  = "办公",       ID = PURPOSECODE_ID, CodeType = "土地用途" },
            //new SYSCode { Code = PURPOSECODE_6, CodeName  = "工业",       ID = PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = PURPOSECODE_7, CodeName  = "商业、居住", ID = PURPOSECODE_ID, CodeType = "土地用途" },
            //new SYSCode { Code = PURPOSECODE_8, CodeName  = "商业、办公", ID = PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = PURPOSECODE_9, CodeName  = "办公、居住", ID = PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = PURPOSECODE_10, CodeName = "停车场",     ID = PURPOSECODE_ID, CodeType = "土地用途" },
            //new SYSCode { Code = PURPOSECODE_11, CodeName = "酒店",       ID = PURPOSECODE_ID, CodeType = "土地用途" },
            //new SYSCode { Code = PURPOSECODE_12, CodeName = "加油站",     ID = PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = PURPOSECODE_13, CodeName = "综合",       ID = PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = PURPOSECODE_14, CodeName = "其他",       ID = PURPOSECODE_ID, CodeType = "土地用途" }
            };
            return list;
        }

        /// <summary>
        /// 房号用途
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> HousePurposeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = HOUSEPURPOSECODE_1, CodeName  = "普通住宅",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_2, CodeName  = "非普通住宅",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_3, CodeName  = "公寓",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_4, CodeName  = "酒店式公寓",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_5, CodeName  = "独立别墅",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_6, CodeName  = "联排别墅",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_7, CodeName  = "叠加别墅",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_8, CodeName  = "双拼别墅",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_9, CodeName  = "旅馆",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_10, CodeName  = "花园洋房",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_11, CodeName  = "老洋房",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_12, CodeName  = "新式里弄",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_13, CodeName  = "旧式里弄",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_14, CodeName  = "商业",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_15, CodeName  = "办公",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_16, CodeName  = "厂房",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_17, CodeName  = "酒店",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_18, CodeName  = "仓库",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_19, CodeName  = "车位",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_20, CodeName  = "综合",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_21, CodeName  = "商住",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_22, CodeName  = "其他",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_23, CodeName  = "经济适用房",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_24, CodeName  = "补差商品住房",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_25, CodeName  = "地下室,储藏室",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_26, CodeName  = "车库",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = HOUSEPURPOSECODE_27, CodeName  = "别墅",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            };
            return list;
        }

        /// <summary>
        /// 户型结构code列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> StructureCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = STRUCTURECODE_1, CodeName  = "平面",       ID = STRUCTURECODE_ID, CodeType = "户型结构" },
            new SYSCode { Code = STRUCTURECODE_2, CodeName  = "跃式",       ID = STRUCTURECODE_ID, CodeType = "户型结构" },
            new SYSCode { Code = STRUCTURECODE_3, CodeName  = "复式",       ID = STRUCTURECODE_ID, CodeType = "户型结构" },
            new SYSCode { Code = STRUCTURECODE_4, CodeName  = "错层",       ID = STRUCTURECODE_ID, CodeType = "户型结构" },
            new SYSCode { Code = STRUCTURECODE_5, CodeName  = "LOFT",       ID = STRUCTURECODE_ID, CodeType = "户型结构" },
            };
            return list;
        }

        /// <summary>
        /// 通风采光code列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> VDCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = VDCODE_1, CodeName  = "全明通透",       ID = VDCODE_ID, CodeType = "通风采光" },
            new SYSCode { Code = VDCODE_2, CodeName  = "采光欠佳",       ID = VDCODE_ID, CodeType = "通风采光" },
            new SYSCode { Code = VDCODE_3, CodeName  = "通风欠佳",       ID = VDCODE_ID, CodeType = "通风采光" },
            new SYSCode { Code = VDCODE_4, CodeName  = "通风采光欠佳",       ID = VDCODE_ID, CodeType = "通风采光" },
            };
            return list;
        }

        /// <summary>
        /// 噪音情况code列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> NoiseManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = NOISE_1, CodeName  = "安静",       ID = NOISE_ID, CodeType = "噪音情况" },
            new SYSCode { Code = NOISE_2, CodeName  = "较安静",       ID = NOISE_ID, CodeType = "噪音情况" },
            new SYSCode { Code = NOISE_3, CodeName  = "微吵",       ID = NOISE_ID, CodeType = "噪音情况" },
            new SYSCode { Code = NOISE_4, CodeName  = "较吵",       ID = NOISE_ID, CodeType = "噪音情况" },
            new SYSCode { Code = NOISE_5, CodeName  = "很吵",       ID = NOISE_ID, CodeType = "噪音情况" },
            };
            return list;
        }

        /// <summary>
        /// 等级code列表
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> LevelManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = LEVELCODE_1, CodeName  = "优",       ID = LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = LEVELCODE_2, CodeName  = "良",       ID = LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = LEVELCODE_3, CodeName  = "一般",       ID = LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = LEVELCODE_4, CodeName  = "差",       ID = LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = LEVELCODE_5, CodeName  = "很差",       ID = LEVELCODE_ID, CodeType = "等级" },
            };
            return list;
        }


        #region 权限FunctionCode查询
        /// <summary>
        /// 获取所有查询相关functioncode
        /// </summary>
        /// <returns></returns>
        public static int[] GetViewFunctionCodes()
        {
            return new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 };
        }
        #endregion

        /// <summary>
        /// 获取配套code
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetAppendageCodeList()
        {
            //int[] ints = new int[] { APPENDAGECODE_6, APPENDAGECODE_13, APPENDAGECODE_14, APPENDAGECODE_15, APPENDAGECODE_16, APPENDAGECODE_17, APPENDAGECODE_19 };
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2008);
            //List<SYSCode> list2 = list.Where(obj => ints.Contains(obj.Code)).ToList();
            return list;
        }
        /// <summary>
        /// 获取建筑类型(结构)code
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> GetStructureCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2010);
            return list;
        }
        /// <summary>
        /// 获取楼栋位置code
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetBuildingLocationCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2011);
            return list;
        }
        /// <summary>
        /// 获取朝向code
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> GetFrontCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2004);
            return list;
        }
        /// <summary>
        /// 获取户型code
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> GetHouseTypeCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(4001);
            return list;
        }
        /// <summary>
        /// 获取等级code
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetClassCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(1012);
            return list;
        }
        /// <summary>
        /// 获取景观code
        /// </summary>
        /// <returns></returns>
        public static List<SYSCode> GetSightCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2006);
            return list;
        }
        /// <summary>
        /// 获取图片类型Code
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetPhotoTypeCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2009).Where(m => m.Code < 2009010).ToList();
            return list;
        }
        /// <summary>
        /// 楼栋外墙
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetWallCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(6058);
            return list;
        }
        ///<summary>
        /// 户型结构
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetHouseStructureCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(2005);
            return list;
        }
        ///<summary>
        /// 用途
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetHousePurposeCodeList()
        {
            List<SYSCode> list = SYSCodeApi.GetSYSCodeById(1002);
            return list;
        }
        /// <summary>
        /// 获取所有状态code
        /// </summary>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static IList<SYSCode> GetAllotStatusCodeList(string username, string signname, List<UserCenter_Apps> appList)
        {
            List<SYSCode> list = DataCenterCodeApi.GetCodeById(1035, username, signname, appList);
            return list;
        }
        /// <summary>
        /// 根据code获取任务状态
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static SYSCode GetAllotStatusCode(int code)
        {
            return STATECODE_LIST.Where(obj => obj.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 停车状况
        /// </summary>
        /// <returns></returns>
        public static IList<SYSCode> GetAllParkingStatusList()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = 1056001, CodeName  = "充裕", ID = 1056, CodeType = "停车状况" },
            new SYSCode { Code = 1056002, CodeName  = "够用", ID = 1056, CodeType = "停车状况" },
            new SYSCode { Code = 1056003, CodeName  = "稍紧张", ID = 1056, CodeType = "停车状况" },
            new SYSCode { Code = 1056004, CodeName  = "紧张", ID = 1056, CodeType = "停车状况" },
            new SYSCode { Code = 1056005, CodeName = "很紧张",     ID = 1056, CodeType = "停车状况" },
            };
            return list;
        }
    }
}
