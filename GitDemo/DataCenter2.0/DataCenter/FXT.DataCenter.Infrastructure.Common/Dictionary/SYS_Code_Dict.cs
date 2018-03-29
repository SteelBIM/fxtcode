namespace FXT.DataCenter.Infrastructure.Common.Dictionary
{
    public struct SYS_Code_Dict
    {
        #region ID

        public const int _土地用途 = 1001;
        public const int _地下室用途 = 1217;
        public const int _案例类型 = 3001;
        public const int _土地来源 = 3002;
        public const int _成交状态 = 3003;
        public const int _买卖方式 = 3004;
        public const int _土地开发情况 = 3005;
        public const int _操作 = 7001;
        public const int _功能模块 = 7002;
        public const int _使用权性质 = 6030;
        public const int _居住用途 = 1002;
        public const int _产权形式 = 2007;
        public const int _建筑类型 = 2003;
        public const int _建筑等级 = 1012;
        public const int _小区规模 = 1210;
        public const int _户型结构 = 2005;
        public const int _户型 = 4001;
        public const int _朝向 = 2004;
        public const int _币种 = 2002;
        public const int _土地等级 = 1209;
        public const int _外墙装修 = 6058;
        public const int _装修档次 = 6026;
        public const int _建筑结构 = 2010;
        public const int _景观 = 2006;
        public const int _公司类型 = 2001;
        public const int _公司性质 = 1178;
        public const int _平面形状 = 1156;
        public const int _行业大类 = 1158;
        public const int _行业小类 = 1159;
        public const int _行业地位 = 1179;
        public const int _批量导入类型 = 1212;
        public const int _商圈等级 = 1103;
        public const int _图片类型 = 1181;
        public const int _图片类型_住宅 = 2009;
        public const int _商圈关联度 = 1106;
        public const int _交通便捷度 = 1141;
        public const int _经营方式 = 1127;
        public const int _商业细分类型 = 1120;
        public const int _经营业态大类 = 1130;
        public const int _租金方式 = 1155;
        public const int _消费定位 = 1154;
        public const int _客厅装修 = 1140;
        public const int _空调系统类型 = 1142;
        public const int _商业阻隔 = 1153;
        public const int _临路类型 = 1152;
        public const int _人流量 = 1157;
        public const int _商铺类型 = 1110;
        public const int _商铺位置类型 = 1113;
        public const int _楼栋商业类型 = 1118;
        public const int _调查方式 = 1148;
        public const int _办公楼等级 = 1105;
        public const int _办公用途 = 1101;
        public const int _办公外墙装修 = 1143;
        public const int _停车位类型 = 1116;
        public const int _办公商务配套 = 1149;
        public const int _办公区域等级 = 1104;
        public const int _楼栋位置 = 2011;
        public const int _商铺临街类型 = 1107;
        public const int _内街过道位置 = 1112;
        public const int _社区商铺类型 = 1129;
        public const int _附属房屋类型 = 1015;
        public const int _通风采光 = 1216;
        public const int _工业类型 = 6013;
        public const int _管道燃气 = 1213;
        public const int _采暖方式 = 1214;
        public const int _墙体类型 = 1215;
        public const int _装修情况 = 1125;  //毛坯、精装修、简装修
        public const int _住宅项目配套类型 = 2008;
        public const int _土地规划用途 = 1111;
        public const int _土地所有权 = 1022;
        public const int _建筑年代 = 8004;
        public const int _年龄段 = 8009;
        public const int _贷款额度 = 8008;
        public const int _性别 = 2019;
        public const int _婚姻状态 = 2020;
        public const int _学历 = 2021;
        public const int _年薪资范围 = 2022;
        public const int _常用交通工具 = 2023;
        public const int _职位 = 2024;
        public const int _噪音情况 = 2025;
        #endregion

        #region CODE

        public struct 操作
        {
            public const int 新增 = 7001001;
            public const int 修改 = 7001002;
            public const int 删除 = 7001003;
            public const int 导入 = 7001008;
            public const int 导出 = 7001009;
        }
        public struct 功能模块
        {
            public const int 土地案例 = 7002062;
            public const int 土地基础信息 = 7002060;
            public const int 土地基准地价 = 7002064;
            public const int 用户角色 = 7002051;
            public const int 角色菜单 = 7002050;
            public const int 住宅案例 = 7002009;
            public const int 楼盘别名 = 7002067;
            public const int 样本楼盘 = 7002068;
            public const int 待建楼盘 = 7002069;
            public const int 公司 = 7002070;
            public const int 楼盘 = 7002006;
            public const int 楼栋 = 7002007;
            public const int 房号 = 7002008;
            public const int 商圈 = 7002071;
            public const int 商业街 = 7002072;
            public const int 商业楼栋 = 7002073;
            public const int 商业楼层 = 7002074;
            public const int 商业房号 = 7002075;
            public const int 商业案例 = 7002076;
            public const int 商业动态价格调查 = 7002077;
            public const int 商铺 = 7002087;
            public const int 商务中心 = 7002078;
            public const int 办公楼盘 = 7002079;
            public const int 办公楼栋 = 7002080;
            public const int 办公房号 = 7002082;
            public const int 办公楼盘价格调查 = 7002083;
            public const int 办公商务配套 = 7002084;
            public const int 办公租客信息 = 7002085;
            public const int 办公案例 = 7002086;
            public const int 工业片区 = 7002088;
            public const int 工业楼盘 = 7002089;
            public const int 工业楼栋 = 7002090;
            public const int 工业房号 = 7002091;
            public const int 工业项目配套 = 7002092;
            public const int 工业动态价格调查 = 7002093;
            public const int 工业案例 = 7002094;
            public const int 工业租客信息 = 7002095;
            public const int 业主楼盘信息 = 7002096;
            public const int 业主房号信息 = 7002097;
            public const int VQ住宅基准房价 = 7002098;
            public const int VQ住宅案例均价 = 7002099;
            public const int 房产证地址 = 7002100;
        }

        public struct 数据大类
        {
            public const int 土地 = 1203001;
            public const int 住宅 = 1203002;
            public const int 商业 = 1203003;
            public const int 办公 = 1203004;
            public const int 工业 = 1203005;
            public const int 商家企业 = 1203006;
            public const int 业主信息 = 1203007;
            public const int VQ模块 = 1203008;
        }
        public struct 土地数据分类
        {
            public const int 土地基础数据 = 1204001;
            public const int 土地案例数据 = 1204002;
            public const int 土地基准地价 = 1204003;
            public const int 土地区域分析 = 1204004;
            public const int 市场背景分析 = 1204005;

        }

        public struct 住宅数据分类
        {
            public const int 住宅基础数据 = 1205001;
            public const int 住宅案例数据 = 1205002;
            public const int 楼盘别名 = 1205003;
            public const int 样本楼盘 = 1205004;
            public const int 待建楼盘 = 1205005;
            public const int 住宅区域分析 = 1205006;
            public const int 城市均价 = 1205007;
            public const int 市场背景分析 = 1205008;
            public const int 房产证地址 = 1205009;
        }

        public struct 商业数据分类
        {
            public const int 商业基础数据 = 1206001;
            public const int 商业案例数据 = 1206002;
            public const int 动态价格调查 = 1206003;
            public const int 动态价格 = 1206003;
            public const int 商业区域分析 = 1206004;
            public const int 商铺数据 = 1206005;
            public const int 市场背景分析 = 1206006;
        }

        public struct 办公数据分类
        {
            public const int 办公基础数据 = 1207001;
            public const int 办公案例数据 = 1207002;
            public const int 动态价格 = 1207003;
            public const int 区域分析 = 1207004;
            public const int 租客数据 = 1207005;
            public const int 市场背景分析 = 1207006;
        }

        public struct 工业数据分类
        {
            public const int 工业基础数据 = 1208001;
            public const int 工业案例数据 = 1208002;
            public const int 动态价格 = 1208003;
            public const int 区域分析 = 1208004;
            public const int 租客数据 = 1208005;
            public const int 市场背景分析 = 1208006;
        }

        public struct 业主信息分类
        {
            public const int 业主楼盘数据 = 1218001;
            public const int 业主房号数据 = 1218002;
        }

        public struct VQ模块分类
        {
            public const int VQ住宅基准房价 = 1219001;
            public const int VQ住宅案例均价 = 1219002;
        }

        public struct 页面权限
        {
            public const int 查看自己 = 1201001;
            public const int 查看全部 = 1201002;
            public const int 新增 = 1201003;
            public const int 修改自己 = 1201004;
            public const int 修改全部 = 1201005;
            public const int 删除自己 = 1201006;
            public const int 删除全部 = 1201007;
            public const int 导入 = 1201008;
            public const int 导出自己 = 1201009;
            public const int 导出全部 = 1201010;
            public const int 统计 = 1201011;
        }

        public struct 批量导入类型
        {
            public const int 土地信息 = 1212001;
            public const int 土地案例 = 1212002;
            public const int 楼盘案例 = 1212003;
            public const int 住宅楼盘信息 = 1212004;
            public const int 住宅楼栋信息 = 1212005;
            public const int 住宅房号信息 = 1212006;
            public const int 样本楼盘 = 1212022;
            public const int 公司商家信息 = 1212007;
            public const int 商业案例 = 1212008;
            public const int 办公案例 = 1212009;
            public const int 工业案例 = 1212010;
            public const int 商铺 = 1212011;
            public const int 商业楼层信息 = 1212012;
            public const int 商业街信息 = 1212013;
            public const int 商业楼栋信息 = 1212014;
            public const int 商业房号信息 = 1212015;
            public const int 商业动态价格调查 = 1212023;
            public const int 办公楼盘信息 = 1212016;
            public const int 办公楼栋信息 = 1212017;
            public const int 办公房号信息 = 1212018;
            public const int 办公商务配套 = 1212024;
            public const int 办公商务中心 = 1212025;
            public const int 办公动态价格 = 1212026;
            public const int 办公租客信息 = 1212027;
            public const int 工业楼盘信息 = 1212019;
            public const int 工业楼栋信息 = 1212020;
            public const int 工业房号信息 = 1212021;
            public const int 工业片区 = 1212028;
            public const int 工业项目配套 = 1212029;
            public const int 工业动态价格 = 1212030;
            public const int 工业租客信息 = 1212031;
            public const int 住宅项目配套 = 1212101;
            public const int 楼盘图片批量上传 = 1212102;
            public const int 楼栋图片批量上传 = 1212103;
            public const int VQ住宅基准房价 = 1212104;
            public const int 业主楼盘信息 = 1212032;
            public const int 业主房号信息 = 1212033;
            public const int 住宅房号系数差 = 1212105;
            public const int 楼盘别名 = 1212106;
            public const int VQ住宅案例均价 = 1212107;
            public const int 房产证地址 = 1212108;
        }

        public struct 公司类型
        {
            public const int 开发商 = 2001001;
            public const int 物管公司 = 2001004;
        }

        #endregion
    }
}
