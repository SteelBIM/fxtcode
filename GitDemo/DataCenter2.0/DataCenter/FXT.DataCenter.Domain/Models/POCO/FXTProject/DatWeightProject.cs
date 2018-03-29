using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DatWeightProject
    {
        [ExcelExportIgnore]
        public long Id { get; set; }

        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }

        [ExcelExportIgnore]
        public int CityId { get; set; }

        [DisplayName("城市名称")]
        public string CityName { get; set; }

        [DisplayName("楼盘ID")]
        [Required(ErrorMessage = "楼盘不存在")]
        public int ProjectId { get; set; }

        [ExcelExportIgnore]
        public int AreaId { get; set; }

        [DisplayName("行政区")]
        public string AreaName { get; set; }

        [DisplayName("楼盘名称")]
        public string ProjectName { get; set; }

        [DisplayName("楼盘别名")]
        public string OtherName { get; set; }

        [DisplayName("楼栋建筑类型")]
        public string BuildingTypeCodeName { get; set; }

        [DisplayName("低层建筑系数")]
        [ExcelExportIgnore]
        public int? LowLayerWeight { get; set; }

        [DisplayName("多层建筑系数")]
        [ExcelExportIgnore]
        public int? MultiLayerWeight { get; set; }

        [DisplayName("小高层建筑系数")]
        [ExcelExportIgnore]
        public int? SmallHighLayerWeight { get; set; }

        [DisplayName("高层建筑系数")]
        [ExcelExportIgnore]
        public int? HighLayerWeight { get; set; }

        [DisplayName("楼盘均价")]
        public int? ProjectAvgPrice { get; set; }

        [DisplayName("低层建筑均价")]
        public int? LowLayerPrice { get; set; }

        [DisplayName("多层建筑均价")]
        public int? MultiLayerPrice { get; set; }

        [DisplayName("小高层建筑均价")]
        public int? SmallHighLayerPrice { get; set; }

        [DisplayName("高层建筑均价")]
        public int? HighLayerPrice { get; set; }

        [DisplayName("独幢别墅建筑均价")]
        public int? SingleVillaPrice { get; set; }

        [DisplayName("联排别墅建筑均价")]
        public int? PlatoonVillaPrice { get; set; }

        [DisplayName("叠加别墅建筑均价")]
        public int? SuperpositionVillaPrice { get; set; }

        [DisplayName("双拼别墅建筑均价")]
        public int? DuplexesVillaPrice { get; set; }

        [DisplayName("回迁房建筑均价")]
        public int? MoveBackHousePrice { get; set; }

        [DisplayName("更新时间")]
        public DateTime UpdateDate { get; set; }

        [DisplayName("是否过期")]
        public string IsExpire { get; set; }

        [ExcelExportIgnore]
        public int? EvaluationCompanyId { get; set; }

        [ExcelExportIgnore]
        public int BuildingNum { get; set; }

        #region 扩展字段

        [ExcelExportIgnore]
        public int Type { get; set; }

        [DisplayName("估价公司名称")]
        [ExcelExportIgnore]
        public string EvaluationCompanyName { get; set; }

        [DisplayName("修改人")]
        public string UpdateUser { get; set; }

        #endregion
    }
}
