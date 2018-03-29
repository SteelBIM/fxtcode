using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models.DTO
{
    public class ProjectCase_WorkLoad
    {
        public string UserName { get; set; }
        public int Count { get; set; }
    }

    public class ProjectCase_Statist
    {
        [DisplayName("楼盘ID")]
        public int ProjectId { get; set; }
        [DisplayName("行政区")]
        public string AreaName { get; set; }
        [DisplayName("楼盘名称")]
        public string ProjectName { get; set; }
        [DisplayName("主用途")]
        public string PurposeCodeName { get; set; }
        [DisplayName("主建筑物类型")]
        public string pBuildingTypeCodeName { get; set; }

        [DisplayName("案例总数")]
        public int casecount { get; set; }
        [DisplayName("fxt案例总数")]
        public int casecount_fxt { get; set; }
        [DisplayName("pg案例总数")]
        public int casecount_nfxt { get; set; }

        [DisplayName("案例均价")]
        public decimal caseprice { get; set; }
        [DisplayName("fxt案例均价")]
        public decimal caseprice_fxt { get; set; }
        [DisplayName("pg通案例均价")]
        public decimal caseprice_nfxt { get; set; }

        [DisplayName("低层案例")]
        public int LowerCount { get; set; }
        [DisplayName("fxt低层案例")]
        public int LowerCount_fxt { get; set; }
        [DisplayName("pg低层案例")]
        public int LowerCount_nfxt { get; set; }

        [DisplayName("低层均价")]
        public decimal lowerprice { get; set; }
        [DisplayName("fxt低层均价")]
        public decimal lowerprice_fxt { get; set; }
        [DisplayName("pg低层均价")]
        public decimal lowerprice_nfxt { get; set; }

        [DisplayName("多层案例")]
        public int MultiCount { get; set; }
        [DisplayName("fxt多层案例")]
        public int MultiCount_fxt { get; set; }
        [DisplayName("pg多层案例")]
        public int MultiCount_nfxt { get; set; }

        [DisplayName("多层均价")]
        public decimal multiprice { get; set; }
        [DisplayName("fxt多层均价")]
        public decimal multiprice_fxt { get; set; }
        [DisplayName("pg多层均价")]
        public decimal multiprice_nfxt { get; set; }

        [DisplayName("小高层案例")]
        public int SmallHighCount { get; set; }
        [DisplayName("fxt小高层案例")]
        public int SmallHighCount_fxt { get; set; }
        [DisplayName("pg小高层案例")]
        public int SmallHighCount_nfxt { get; set; }

        [DisplayName("小高层均价")]
        public decimal smallhighprice { get; set; }
        [DisplayName("fxt小高层均价")]
        public decimal smallhighprice_fxt { get; set; }
        [DisplayName("pg小高层均价")]
        public decimal smallhighprice_nfxt { get; set; }

        [DisplayName("高层案例")]
        public int HighCount { get; set; }
        [DisplayName("fxt高层案例")]
        public int HighCount_fxt { get; set; }
        [DisplayName("pg高层案例")]
        public int HighCount_nfxt { get; set; }

        [DisplayName("高层均价")]
        public decimal highprice { get; set; }
        [DisplayName("fxt高层均价")]
        public decimal highprice_fxt { get; set; }
        [DisplayName("pg高层均价")]
        public decimal highprice_nfxt { get; set; }

        [DisplayName("空建筑类型案例")]
        [ExcelExportIgnore]
        public int NBuildingTypeCount { get; set; }
        [DisplayName("空建筑类型均价")]
        [ExcelExportIgnore]
        public decimal nbuildingtypeprice { get; set; }
    }

    public class ProjectCase_AvePrice
    {
        [DisplayName("案例时间")]
        public string CaseDate { get; set; }

        [DisplayName("行政区")]
        public string AreaName { get; set; }

        [DisplayName("片区")]
        public string SubAreaName { get; set; }

        [DisplayName("楼盘ID")]
        public long ProjectId { get; set; }

        [DisplayName("楼盘")]
        public string ProjectName { get; set; }

        [DisplayName("建筑年代")]
        public string BuildingDateName { get; set; }

        [DisplayName("案例条数")]
        public string Num { get; set; }

        [DisplayName("均价")]
        public string AvgPrice { get; set; }

        [DisplayName("涨跌幅_百分比")]
        public string PricePercent { get; set; }


    }

    public class ProjectCase_ProjectEValue
    {
        [DisplayName("行政区")]
        public string AreaName { get; set; }

        [DisplayName("楼盘")]
        public string ProjectName { get; set; }

        [DisplayName("楼盘地址")]
        public string Address { get; set; }

        [DisplayName("是否可估")]
        public string PIsEValue { get; set; }

        [DisplayName("可估系数")]
        public string PWeight { get; set; }

        [DisplayName("案例条数")]
        public string CaseCount { get; set; }

        [DisplayName("估价结果")]
        public string PEPrice { get; set; }

        [DisplayName("不可估原因")]
        public string Remark { get; set; }
    }

    public class ProjectCase_BuildingEValue
    {
        [DisplayName("行政区")]
        public string AreaName { get; set; }

        [DisplayName("楼盘名称")]
        public string ProjectName { get; set; }

        [DisplayName("楼盘地址")]
        public string Address { get; set; }

        [DisplayName("楼栋名称")]
        public string BuildingName { get; set; }

        [DisplayName("案例条数")]
        public string CaseCount { get; set; }

        [DisplayName("楼盘估价结果")]
        public string PEPrice { get; set; }

        [DisplayName("楼栋估价结果")]
        public string BEPrice { get; set; }

        [DisplayName("不可估原因")]
        public string Remark { get; set; }
    }

    public class MisMatchProjectDTO
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string ProjectName { get; set; }
        public int Num { get; set; }
        public bool IsWaitProject { get; set; }
    }
}
