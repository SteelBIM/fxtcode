using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.Infrastructure.Common.Common;
using System.Collections.Generic;

namespace FXT.DataCenter.Domain.Models
{
    [Serializable]
    public class DAT_Project
    {
        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [DisplayName("*楼盘名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string projectname { get; set; }

        [DisplayName("片区")]
        public string SubAreaName { get; set; }

        [DisplayName("楼盘别名")]
        public string othername { get; set; }

        [DisplayName("楼盘地址")]
        public string address { get; set; }

        [ExcelExportIgnore]
        public int? purposecode { get; set; }
        [DisplayName("*主用途")]
        public string PurposeName
        {
            get
            {

                if (purposecode != null)
                {
                    if (purposecode == -1)
                    {
                        return "";
                    }
                    switch (purposecode)
                    {
                        case 1001001:
                            return "居住";
                        case 1001002:
                            return "居住(别墅)";
                        case 1001003:
                            return "居住(洋房)";
                        case 1001004:
                            return "商业";
                        case 1001005:
                            return "办公";
                        case 1001006:
                            return "工业";
                        case 1001007:
                            return "商业、居住";
                        case 1001008:
                            return "商业、办公";
                        case 1001009:
                            return "办公、居住";
                        case 1001010:
                            return "停车场";
                        case 1001011:
                            return "酒店";
                        case 1001012:
                            return "加油站";
                        case 1001013:
                            return "综合";
                        case 1001014:
                            return "其他";
                        default:
                            return purposecode.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("宗地号")]
        public string fieldno { get; set; }

        [DisplayName("土地起始日期")]
        public DateTime? startdate { get; set; }

        [DisplayName("土地终止日期")]
        public DateTime? startenddate { get; set; }

        [DisplayName("土地使用年限")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? usableyear { get; set; }

        [DisplayName("主建筑物类型")]
        [ExcelExportIgnore]
        public int? buildingtypecode { get; set; }
        [DisplayName("主建筑物类型")]
        public string BuildingtypeName
        {
            get
            {
                if (buildingtypecode != null)
                {
                    if (buildingtypecode == -1)
                    {
                        return "";
                    }
                    switch (buildingtypecode)
                    {
                        case 2003001:
                            return "低层";
                        case 2003002:
                            return "多层";
                        case 2003003:
                            return "小高层";
                        case 2003004:
                            return "高层";
                        default:
                            return buildingtypecode.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("容积率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? cubagerate { get; set; }

        [DisplayName("绿化率")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? greenrate { get; set; }

        [DisplayName("占地面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? landarea { get; set; }

        [DisplayName("建筑面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? buildingarea { get; set; }

        [DisplayName("可销售面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? salablearea { get; set; }

        [DisplayName("封顶日期")]
        public DateTime? coverdate { get; set; }

        [DisplayName("竣工日期")]
        public DateTime? enddate { get; set; }

        [DisplayName("开盘日期")]
        public DateTime? saledate { get; set; }

        [DisplayName("开工日期")]
        public DateTime? buildingdate { get; set; }

        [DisplayName("入伙日期")]
        public DateTime? joindate { get; set; }

        [DisplayName("项目均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? averageprice { get; set; }

        [DisplayName("开盘均价")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? saleprice { get; set; }

        [DisplayName("总栋数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? buildingnum { get; set; }

        [DisplayName("总套数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? totalnum { get; set; }

        [DisplayName("车位数")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? parkingnumber { get; set; }

        [DisplayName("车位描述")]
        public string parkingdesc { get; set; }

        [DisplayName("开发商")]
        public string DeveCompanyName { get; set; }

        [DisplayName("物管公司")]
        public string ManagerCompanyName { get; set; }

        [DisplayName("物管费")]
        public string managerprice { get; set; }

        [DisplayName("物管电话")]
        public string managertel { get; set; }

        [DisplayName("项目概况")]
        public string detail { get; set; }

        [DisplayName("东")]
        public string east { get; set; }

        [DisplayName("西")]
        public string west { get; set; }

        [DisplayName("南")]
        public string south { get; set; }

        [DisplayName("北")]
        public string north { get; set; }

        [DisplayName("经度")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? x { get; set; }

        [DisplayName("纬度")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? y { get; set; }

        [DisplayName("内部认购日期")]
        public DateTime? innersaledate { get; set; }

        [DisplayName("产权形式")]
        [ExcelExportIgnore]
        public int? rightcode { get; set; }
        [DisplayName("产权形式")]
        public string RightName
        {
            get
            {
                if (rightcode != null)
                {
                    if (rightcode == -1)
                    {
                        return "";
                    }
                    switch (rightcode)
                    {
                        case 2007001:
                            return "商品房";
                        case 2007002:
                            return "微利房";
                        case 2007003:
                            return "福利房";
                        case 2007004:
                            return "军产房";
                        case 2007005:
                            return "集资房";
                        case 2007006:
                            return "自建房";
                        case 2007007:
                            return "经济适用房";
                        case 2007008:
                            return "小产权房";
                        case 2007009:
                            return "限价房";
                        case 2007010:
                            return "解困房";
                        case 2007011:
                            return "宅基地";
                        case 2007012:
                            return "房改房";
                        case 2007013:
                            return "平改房";
                        case 2007014:
                            return "回迁房";
                        case 2007015:
                            return "安置房";
                        default:
                            return rightcode.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("办公面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? officearea { get; set; }

        [DisplayName("商业面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? businessarea { get; set; }

        [DisplayName("工业面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? industryarea { get; set; }

        [DisplayName("其他用途面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? otherarea { get; set; }

        [DisplayName("土地规划用途")]
        [ExcelExportIgnore]
        public string planpurpose { get; set; }

        [DisplayName("土地规划用途")]
        [ExcelExportIgnore]
        public List<string> Description { get; set; }

        [DisplayName("土地规划用途")]
        public string opValue { get; set; }

        [DisplayName("价格调查时间")]
        public DateTime? pricedate { get; set; }

        [DisplayName("价格修正系数")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? weight { get; set; }

        [DisplayName("是否完成基础数据")]
        [ExcelExportIgnore]
        public int? iscomplete { get; set; }
        [DisplayName("是否完成基础数据")]
        public string iscompleteName
        {
            get
            {
                if (iscomplete != null)
                {
                    if (iscomplete == -1)
                    {
                        return "";
                    }
                    switch (iscomplete)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return iscomplete.ToString();
                    }
                }
                return "";
            }
        }

        [ExcelExportIgnore]
        public int? isevalue { get; set; }
        [DisplayName("是否可估")]
        public string isevalueName
        {
            get
            {
                if (isevalue != null)
                {
                    if (isevalue == -1)
                    {
                        return "";
                    }
                    switch (isevalue)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return isevalue.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("拼音简写")]
        public string pinyin { get; set; }

        [DisplayName("楼盘名称全拼")]
        public string pinyinall { get; set; }

        [DisplayName("比例尺")]
        public int? xyscale { get; set; }

        [ExcelExportIgnore]
        public int? isempty { get; set; }
        [DisplayName("是否空楼盘")]
        public string isemptyName
        {
            get
            {
                if (isempty != null)
                {
                    if (isempty == -1)
                    {
                        return "";
                    }
                    switch (isempty)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return isempty.ToString();
                    }
                }
                return "";
            }
        }

        [ExcelExportIgnore]
        public int? BuildingQuality { get; set; }
        [DisplayName("建筑质量")]
        public string BuildingQualityName
        {
            get
            {
                if (BuildingQuality != null)
                {
                    if (BuildingQuality == -1)
                    {
                        return "";
                    }
                    switch (BuildingQuality)
                    {
                        case 1012001:
                            return "优";
                        case 1012002:
                            return "良";
                        case 1012003:
                            return "一般";
                        case 1012004:
                            return "差";
                        case 1012005:
                            return "很差";
                        default:
                            return BuildingQuality.ToString();
                    }
                }
                return "";
            }
        }

        [ExcelExportIgnore]
        public int? HousingScale { get; set; }
        [DisplayName("小区规模")]
        public string HousingScaleName
        {
            get
            {
                if (HousingScale != null)
                {
                    if (HousingScale == -1)
                    {
                        return "";
                    }
                    switch (HousingScale)
                    {
                        case 1210001:
                            return "10万㎡以下";
                        case 1210002:
                            return "10~20万㎡";
                        case 1210003:
                            return "20~50万㎡";
                        case 1210004:
                            return "50~100万㎡";
                        case 1210005:
                            return "100万㎡以上";
                        default:
                            return HousingScale.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("楼栋备注")]
        public string BuildingDetail { get; set; }

        [DisplayName("房号备注")]
        public string HouseDetail { get; set; }

        [DisplayName("地下室用途")]
        public string BasementPurpose { get; set; }

        [ExcelExportIgnore]
        public int? ManagerQuality { get; set; }
        [DisplayName("物业管理质量")]
        public string ManagerQualityName
        {
            get
            {
                if (ManagerQuality != null)
                {
                    if (ManagerQuality == -1)
                    {
                        return "";
                    }
                    switch (ManagerQuality)
                    {
                        case 1012001:
                            return "优";
                        case 1012002:
                            return "良";
                        case 1012003:
                            return "一般";
                        case 1012004:
                            return "差";
                        case 1012005:
                            return "很差";
                        default:
                            return ManagerQuality.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("设备设施")]
        public string Facilities { get; set; }

        [ExcelExportIgnore]
        public int? AppendageClass { get; set; }
        [DisplayName("配套等级")]
        public string AppendageClassName
        {
            get
            {
                if (AppendageClass != null)
                {
                    if (AppendageClass == -1)
                    {
                        return "";
                    }
                    switch (AppendageClass)
                    {
                        case 1012001:
                            return "优";
                        case 1012002:
                            return "良";
                        case 1012003:
                            return "一般";
                        case 1012004:
                            return "差";
                        case 1012005:
                            return "很差";
                        default:
                            return AppendageClass.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("区域分析")]
        public string RegionalAnalysis { get; set; }

        [DisplayName("有利因素")]
        public string Wrinkle { get; set; }

        [DisplayName("不利因素")]
        public string Aversion { get; set; }

        [DisplayName("数据来源")]
        public string SourceName { get; set; }

        [DisplayName("实际楼栋数")]
        public int BuildingNumber { get; set; }

        [DisplayName("实际总套数")]
        public int HouseNumber { get; set; }

        [DisplayName("创建用户")]
        public string creator { get; set; }
        [DisplayName("创建日期")]
        public DateTime? createtime { get; set; }
        [DisplayName("评估机构")]
        [ExcelExportIgnore]
        public string CompanyName { get; set; }

        #region 扩展字段
        [ExcelExportIgnore]
        public int cityid { get; set; }
        [ExcelExportIgnore]
        public string CityName { get; set; }

        [ExcelExportIgnore]
        public int projectid { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        [ExcelExportIgnore]
        public int areaid { get; set; }
        [ExcelExportIgnore]
        public int? subareaid { get; set; }

        [ExcelExportIgnore]
        public int DeveCompanyId { get; set; }
        [ExcelExportIgnore]
        public int ManagerCompanyId { get; set; }

        [DisplayName("修改价格")]
        [ExcelExportIgnore]
        public string ModifyPrice { get; set; }

        [DisplayName("坐标集合")]
        [ExcelExportIgnore]
        public string LngOrLat { get; set; }

        [DisplayName("目标名称ID")]
        [ExcelExportIgnore]
        public int projectIdTo { get; set; }

        [DisplayName("目标行政区ID")]
        [ExcelExportIgnore]
        public int AreaIdTo { get; set; }

        //[DisplayName("实际楼栋数")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        //[ExcelExportIgnore]
        //public int? buildingnumCount { get; set; }

        //[DisplayName("实际总套数")]
        //[ExcelExportIgnore]
        //public int? totalnumCount { get; set; }

        private int _valid = 1;
        [ExcelExportIgnore]
        public int valid { get { return _valid; } set { _valid = value; } }
        [ExcelExportIgnore]
        public int fxtcompanyid { get; set; }
        [ExcelExportIgnore]
        public string fxtcompanyname { get; set; }
        [ExcelExportIgnore]
        public int belongcompanyid { get; set; }
        [DisplayName("数据建设机构")]
        public string belongcompanyname { get; set; }
        [ExcelExportIgnore]
        public DateTime? updatedatetime { get; set; }
        [ExcelExportIgnore]
        public DateTime savedatetime { get; set; }
        [ExcelExportIgnore]
        public string saveuser { get; set; }
        [ExcelExportIgnore]
        public int totalid { get; set; }
        [ExcelExportIgnore]
        public int? arealineid { get; set; }
        [ExcelExportIgnore]
        public string oldid { get; set; }

        #endregion
    }
}
