using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_HumanHouse
    {
        [DisplayName("城市")]
        public string CityName { get; set; }
        [DisplayName("*行政区")]
        public string AreaName { get; set; }
        [DisplayName("*楼盘名称")]
        public string ProjectName { get; set; }
        [DisplayName("*楼栋名称")]
        public string BuildingName { get; set; }
        [DisplayName("*房号名称")]
        public string HouseName { get; set; }

        [DisplayName("*产权人")]
        public string Name { get; set; }
        [DisplayName("房产证号")]
        public string PropertyNumber { get; set; }
        [DisplayName("产权人占有份额_百分比")]
        [RegularExpression(@"^[1-9]{2}(\.[0-9]+)?|0\.[1-9][0-9]+", ErrorMessage = "必须两位数值")]
        public decimal? OwnerProportion { get; set; }
        [DisplayName("共有人")]
        public string Share { get; set; }
        [ExcelExportIgnore]
        public int? IsMortgage { get; set; }
        [DisplayName("是否抵押")]
        public string IsMortgageName { get; set; }
        [ExcelExportIgnore]
        public int? IsSealUp { get; set; }
        [DisplayName("是否查封")]
        public string IsSealUpName { get; set; }
        [ExcelExportIgnore]
        public int? RightCode { get; set; }
        [DisplayName("产权形式")]
        public string RightCodeName { get; set; }
        [DisplayName("建筑面积_平方米")]
        public decimal? BuildingArea { get; set; }
        [ExcelExportIgnore]
        public int? BuildingStructureCode { get; set; }
        [DisplayName("建筑结构")]
        public string BuildingStructureCodeName { get; set; }
        [ExcelExportIgnore]
        public int? StructureCode { get; set; }
        [DisplayName("户型结构")]
        public string StructureCodeName { get; set; }
        [ExcelExportIgnore]
        public int? IsParking { get; set; }
        [DisplayName("是否有车位")]
        public string IsParkingName { get; set; }
        [DisplayName("附属物业")]
        public string SubHouse { get; set; }
        [ExcelExportIgnore]
        public int? BuildingDate { get; set; }
        [DisplayName("建筑年代")]
        public string BuildingDateName { get; set; }
        [ExcelExportIgnore]
        public int? ZhuangXiu { get; set; }
        [DisplayName("装修档次")]
        public string ZhuangXiuName { get; set; }
        [ExcelExportIgnore]
        public int? IsLoaned { get; set; }
        [DisplayName("是否贷款")]
        public string IsLoanedName { get; set; }
        [ExcelExportIgnore]
        public int? LoanedLines { get; set; }
        [DisplayName("贷款额度")]
        public string LoanedLinesName { get; set; }
        [DisplayName("贷款期限_月")]
        [RegularExpression(@"^[0-9]+", ErrorMessage = "必须是数字类型")]
        public int? LoanedDeadline { get; set; }
        [ExcelExportIgnore]
        public int? IsSelf { get; set; }
        [DisplayName("是否自住")]
        public string IsSelfName { get; set; }

        [ExcelExportIgnore]
        public int? Sex { get; set; }
        [DisplayName("性别")]
        public string SexName { get; set; }
        [DisplayName("年龄")]
        [RegularExpression(@"^[0-9]+", ErrorMessage = "必须是数字类型")]
        public int? Age { get; set; }
        [ExcelExportIgnore]
        public int? AgeGroup { get; set; }
        [DisplayName("年龄段")]
        public string AgeGroupName { get; set; }
        [DisplayName("籍贯")]
        public string Origin { get; set; }
        [ExcelExportIgnore]
        public DateTime? Birthday { get; set; }
        [DisplayName("出生年月")]
        public string BirthdayName { get; set; }
        [DisplayName("身份证")]
        public string IDCard { get; set; }
        [ExcelExportIgnore]
        public int? Marriage { get; set; }
        [DisplayName("婚姻状态")]
        public string MarriageName { get; set; }
        [DisplayName("手机号码")]
        [RegularExpression(@"^[0-9]+", ErrorMessage = "请输入正确格式")]
        public string Telephone { get; set; }
        [ExcelExportIgnore]
        public int? Education { get; set; }
        [DisplayName("学历")]
        public string EducationName { get; set; }
        [ExcelExportIgnore]
        public int? Occupation { get; set; }
        [DisplayName("行业")]
        public string OccupationName { get; set; }
        [ExcelExportIgnore]
        public int? Position { get; set; }
        [DisplayName("职位")]
        public string PositionName { get; set; }
        [DisplayName("所在公司")]
        public string Company { get; set; }
        [ExcelExportIgnore]
        public int? Salary { get; set; }
        [DisplayName("年薪范围")]
        public string SalaryName { get; set; }
        [ExcelExportIgnore]
        public int? Transportation { get; set; }
        [DisplayName("常用交通工具")]
        public string TransportationName { get; set; }
        [RegularExpression(@"^[0-9]+", ErrorMessage = "必须是数字类型")]
        [DisplayName("家庭成员总数")]
        public int? FamilyNum { get; set; }
        [RegularExpression(@"^[0-9]+", ErrorMessage = "必须是数字类型")]
        [DisplayName("购房数")]
        public int? Houses { get; set; }
        [DisplayName("备注")]
        public string Remark { get; set; }

        [ExcelExportIgnore]
        public long HumanId { get; set; }
        [ExcelExportIgnore]
        public int CityId { get; set; }
        [ExcelExportIgnore]
        public int AreaId { get; set; }
        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [ExcelExportIgnore]
        public long BuildingId { get; set; }
        [ExcelExportIgnore]
        public long HouseId { get; set; }
        [ExcelExportIgnore]
        public int? IsGroup { get; set; }
        [ExcelExportIgnore]
        public int FxtcompanyId { get; set; }
        [ExcelExportIgnore]
        public string Creator { get; set; }
        [ExcelExportIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelExportIgnore]
        public string Saver { get; set; }
        [ExcelExportIgnore]
        public DateTime? SaveTime { get; set; }
        [ExcelExportIgnore]
        public int? Valid { get; set; }
    }
}
