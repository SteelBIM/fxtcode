using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.DTO
{
    public class SurveyInfoDto
    {

        public long Sid { get; set; }
        public int FxtCompanyId { get; set; }

        public string BuildingStructure { get; set; }
        public string TotalFloor { get; set; }
        public string Floor { get; set; }
        public string RoomNum { get; set; }
        public string BalconyNum { get; set; }

        public string Surveyor { get; set; }
        public int IsSurvey { get; set; }
        public string CustomFields { get; set; }
        public string FinancingPurpose { get; set; }
        public string Usage { get; set; }
        public decimal? DecorationValue { get; set; }
        public string BusLineNum { get; set; }
        public string HousingLocation { get; set; }
        public string PublicFacilitiesNum { get; set; }
        public string MaritalStatus { get; set; }
        public string HasChildren { get; set; }
        public decimal? PrepareLoanAmount { get; set; }
        public string IdNum { get; set; }
        public DateTime? SurveyBeginTime { get; set; }
        public DateTime? SurveyEndTime { get; set; }

        #region 新增字段
        public string Number { get; set; }
        public string CompleteTime { get; set; }
        public string SurveyHouseAge { get; set; }
        public string Wall { get; set; }
        public string PropertyPrice { get; set; }
        public string Villa { get; set; }
        public string AverageHouse { get; set; }
        public string NotAverageHouse { get; set; }
        public string GreenEnvironment { get; set; }
        public string AirQuality { get; set; }
        public string HouseStruct { get; set; }
        public string HouseName { get; set; }
        public string HasKitchen { get; set; }
        public decimal? Terrace { get; set; }
        public string SingleBalcony { get; set; }
        public string SingleBalconyArea { get; set; }
        public string TallBalcony { get; set; }
        public string TallBalconyArea { get; set; }
        public decimal? Roof { get; set; }
        public decimal? Garden { get; set; }
        public string StructNewProbability { get; set; }
        public string LayerHigh { get; set; }
        public string Ventilation { get; set; }
        public string NoisePollution { get; set; }
        public string DecorationProbabilit { get; set; }
        public string LvYear { get; set; }
        public string ParlorCeiling { get; set; }
        public string ParlorWall { get; set; }
        public string ParlorGround { get; set; }
        public string BedroomCeiling { get; set; }
        public string BedroomWall { get; set; }
        public string BedroomGround { get; set; }
        public string KitchenCeiling { get; set; }
        public string KitchenWall { get; set; }
        public string KitchenGround { get; set; }
        public string KitchenDesk { get; set; }
        public string SurveyHouseKitchenCupboards { get; set; }
        public string ToiletsCeiling { get; set; }
        public string ToiletsWall { get; set; }
        public string ToiletsGround { get; set; }
        public string ToiletsHealth { get; set; }
        public string ToiletsBath { get; set; }
        public string Toilet { get; set; }
        public string BigDoor { get; set; }
        public string InDoor { get; set; }
        public string RoomDoor { get; set; }
        public string Window { get; set; }
        public string IntelligentSystems { get; set; }
        public string SmokeSystems { get; set; }
        public string SpraySystems { get; set; }
        public string GasSystems { get; set; }
        public string IntercomSystems { get; set; }
        public string Broadband { get; set; }
        public string Cabletelevision { get; set; }
        public string Phone { get; set; }
        public string Heating { get; set; }
        public string ClientElevator { get; set; }
        public string CabinBrand { get; set; }
        public string LadderBrand { get; set; }
        public string IsUpCarLocation { get; set; }
        public string IsDownCarLocation { get; set; }
        public string IsCar { get; set; }
        public string CarOccupy { get; set; }
        public string Movement { get; set; }
        public string Club { get; set; }
        public string HealthCente { get; set; }
        public string PostOffice { get; set; }
        public string Bank { get; set; }
        public string Market { get; set; }
        public string HighSchool { get; set; }
        public string PrimarySchool { get; set; }
        public string Nursery { get; set; }
        public string RoadName { get; set; }
        public string RoadWidht { get; set; }
        public string TrafficConvenient { get; set; }
        public string BusDistance { get; set; }
        public string Metro { get; set; }
        public string SubwayDistance { get; set; }
        public string RoadTrafficFlow { get; set; }
        public string TrafficManagement { get; set; }
        public string Side { get; set; }
        public string Environment { get; set; }
        public float? Loclat { get; set; }
        public float? Loclng { get; set; }
        #endregion

    }


    //n:ID或名称 name
    //c:显示的名称 caption
    //l:logo url logo
    //d:描述 desc
    //t:类型 type
    //v:值或列表值 value
    //s:结果值 show
    //r:必填项 required
    //f:系统字段 field
    //u:单位 unit
    //"t":文本 text
    //"n":数字 number
    //"s":下拉框 select
    //"r":单选框 radio
    //"b":bool选择框
    //"c":多选框 checkbox
    //"m":通过测距仪输入数据框   measurement
    public class CustomFields
    {
        public string d { get; set; }
        public string l { get; set; }
        public string c { get; set; }
        public object v { get; set; }
    }

    public class CustomFieldValue
    {
        public string g { get; set; }
        public string c { get; set; }
        public string t { get; set; }
        public string u { get; set; }
        public string m { get; set; }
        public string i { get; set; }
        public string r { get; set; }
        public string n { get; set; }
        public string v { get; set; }
        public string f { get; set; }
        public string s { get; set; }
    }
}
