using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;
using CAS.Entity.AttributeHelper;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Case : DATCase
    {
        #region 面积段均价
        /// <summary>
        /// 60以下
        /// </summary>
        [SQLReadOnly]
        public decimal oneStageUnitprice { get; set; }
        /// <summary>
        /// 60~90
        /// </summary>
        [SQLReadOnly]
        public decimal twoStageUnitprice { get; set; }
        /// <summary>
        /// 90~120
        /// </summary>
        [SQLReadOnly]
        public decimal threeStageUnitprice { get; set; }
        /// <summary>
        /// 120~144
        /// </summary>
        [SQLReadOnly]
        public decimal forStageUnitprice { get; set; }
        /// <summary>
        /// 144以上
        /// </summary>
        [SQLReadOnly]
        public decimal fiveStageUnitprice { get; set; }
        #endregion
        /// <summary>
        /// 案例类型
        /// </summary>
        [SQLReadOnly]
        [ExportDisplayName("类型")]
        public string codetypename { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        [SQLReadOnly]
        public string purposename { get; set; }

        /// <summary>
        ///景观
        /// </summary>
        [SQLReadOnly]
        public string sightcodename { get; set; }

        /// <summary>
        /// 货币单位
        /// </summary>
        [SQLReadOnly]
        public string moneyunitcodename { get; set; }

        /// <summary>
        /// 建筑结构
        /// </summary>
        [SQLReadOnly]
        public string structurecodename { get; set; }

        /// <summary>
        /// 建筑类型
        /// </summary>
        [SQLReadOnly]
        public string buildingtypecodename { get; set; }

        /// <summary>
        /// 户型
        /// </summary>
        [SQLReadOnly]
        public string housetypecodename { get; set; }

        /// <summary>
        /// 装修情况
         ///</summary>
        [SQLReadOnly]
        public string fitmentcodename { get; set; }

        /// <summary>
        /// 案例类型
        /// </summary>
        [SQLReadOnly]
        public string casetypecodename { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }

        /// <summary>
        /// 案例所有者
        /// </summary>
        [SQLReadOnly]
        public string fxtcompanyname { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [SQLReadOnly]
        [ExportDisplayName("楼盘名称")]
        public string projectname { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        [SQLReadOnly]
        public string frontname { get; set; }

        /// <summary>
        /// 案例地址
        /// </summary>
        [SQLReadOnly]
        public string Address { get; set; }
        /// <summary>
        /// 用途（居住用途）
        /// </summary>
        [SQLReadOnly]
        public string purposecodename { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        [SQLReadOnly]
        public string frontcodename { get; set; }

        /// <summary>
        /// 土地起始日期
        /// </summary>
        [SQLReadOnly]
        public string startdate { get; set; }

        /// <summary>
        /// 土地使用年限
        /// </summary>
        [SQLReadOnly]
        public string usableyear { get; set; }

        /// <summary>
        /// 容积率
        /// </summary>
        [SQLReadOnly]
        public string cubagerate { get; set; }

        /// <summary>
        /// 绿化率
        /// </summary>
        [SQLReadOnly]
        public string greenrate { get; set; }

        /// <summary>
        /// 有利因素
        /// </summary>
        [SQLReadOnly]
        public string wrinkle { get; set; }

        /// <summary>
        /// 不利因素
        /// </summary>
        [SQLReadOnly]
        public string aversion { get; set; }
        /// <summary>
        /// 1:估价宝案例，2：数据中心案例
        /// </summary>
        [SQLReadOnly]
        public int casesource { get; set; }
        [SQLReadOnly]
        public long gjbcaseid { get; set; }
        /// <summary>
        /// 物业名称
        /// </summary>
        [SQLReadOnly]
        public string casename
        {
            get;
            set;
        }
    }




    /// <summary>
    /// 楼盘周边案例(仅仅保存价格等简单信息，用于估算楼盘均价)
    /// </summary>
    public class Dat_AroundCase
    {
        /// <summary>
        /// 案例类型（1：周边5公里范围内建筑类型和面积段相同 2：周边5公里范围内建筑类型相同 3：行政区内建筑类型和面积段相同）
        /// </summary>
        public int datatype { get; set; }
        /// <summary>
        /// 均价
        /// </summary>
        public decimal unitprice { get; set; }
        /// <summary>
        /// 建筑面积CODE
        /// </summary>
        public int buildingareatype { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal buildingarea { get; set; }
        /// <summary>
        /// 建筑类型Code
        /// </summary>
        public int buildingtypecode { get; set; }


    }

    public class Dat_Case_Dhhy : Dat_Case
    {
        /// <summary>
        /// 案例名次
        /// </summary>
        [SQLReadOnly]
        public string CaseName
        {

            get
            {
                return projectname + buildingname + houseno;
            }
        }

        /// <summary>
        /// 设计用途
        /// </summary>
        [SQLReadOnly]
        public string PlanPurpose { get; set; }

        /// <summary>
        /// 墙体类型
        /// </summary>
        [SQLReadOnly]
        public string WallType { get; set; }

        /// <summary>
        /// 窗户
        /// </summary>
        [SQLReadOnly]
        public string Window { get; set; }

        /// <summary>
        /// 单元门
        /// </summary>
        [SQLReadOnly]
        public string UnitDoor { get; set; }

        /// <summary>
        /// 单元墙面
        /// </summary>
        [SQLReadOnly]
        public string UnitWall { get; set; }

        /// <summary>
        /// 楼梯扶手
        /// </summary>
        [SQLReadOnly]
        public string Banister { get; set; }

        /// <summary>
        /// 单元踏步
        /// </summary>
        [SQLReadOnly]
        public string UnitTread { get; set; }

        /// <summary>
        /// 入户门
        /// </summary>
        [SQLReadOnly]
        public string Doors { get; set; }

        /// <summary>
        /// 供暖方式
        /// </summary>
        [SQLReadOnly]
        public string HeatingType { get; set; }

        /// <summary>
        /// 室内格局
        /// </summary>
        [SQLReadOnly]
        public string HouseTypeDetail { get; set; }

        /// <summary>
        /// 室内装修
        /// </summary>
        [SQLReadOnly]
        public string ZhuangXiuType { get; set; }

        /// <summary>
        /// 举架
        /// </summary>
        [SQLReadOnly]
        public string JuJia { get; set; }

        /// <summary>
        /// 土地信息
        /// </summary>
        [SQLReadOnly]
        public string LandDetail { get; set; }

        /// <summary>
        /// 土地面积
        /// </summary>
        [SQLReadOnly]
        public decimal? LandArea { get; set; }

        /// <summary>
        /// 土地使用权性质
        /// </summary>
        [SQLReadOnly]
        public string LandRight { get; set; }

        /// <summary>
        /// 土地使用期限
        /// </summary>
        [SQLReadOnly]
        public string LandOver { get; set; }


        /// <summary>
        /// 四至
        /// </summary>
        [SQLReadOnly]
        public string SiZhi { get; set; }

        /// <summary>
        /// 建筑物描述
        /// </summary>
        [SQLReadOnly]
        public string BuildingRemark { get; set; }

        /// <summary>
        /// 外墙
        /// </summary>
        [SQLReadOnly]
        public string OutWall { get; set; }

        /// <summary>
        /// 暖房工程改造(是,否)
        /// </summary>
        [SQLReadOnly]
        public string NuanGai { get; set; }
        /// <summary>
        /// 临街状况(临主街,临次街,不临街)
        /// </summary>
        [SQLReadOnly]
        public string LinJie { get; set; }
        /// <summary>
        /// 区位，交通便捷度(好,差,一般)
        /// </summary>
        [SQLReadOnly]
        public string QW_JTBJD { get; set; }
        /// <summary>
        /// 区位，距离医疗机构距离(远,近,一般)
        /// </summary>
        [SQLReadOnly]
        public string QW_YLJGJL { get; set; }
        /// <summary>
        /// 区位，距离教育机构距离(远,近,一般)
        /// </summary>
        [SQLReadOnly]
        public string QW_JYJGJL { get; set; }
        /// <summary>
        /// 区位，距离服务设施距离(远,近,一般)
        /// </summary>
        [SQLReadOnly]
        public string QW_FWSSJL { get; set; }

    }
}
