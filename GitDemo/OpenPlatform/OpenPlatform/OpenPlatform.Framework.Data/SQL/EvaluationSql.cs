using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Framework.Data.SQL
{
   public struct EvaluationSql
   {
      
       public const string GetAppraiseStatusById = @"SELECT ea.GJBEntrustId
            FROM Person p
            INNER JOIN Property_Owner po ON po.PersonId = p.PersonId
            INNER JOIN Entrust_Object eo ON eo.EOId = po.EOId
            RIGHT JOIN Entrust_Appraise ea ON ea.EAId = eo.EAId
            WHERE ea.AppraiseStatus=1 and ea.fxtCompanyId=@fxtCompanyId and p.IDNum = @id group by ea.EAId";

       /// 20151026
       /// 根据身份证获取委估对象ID（EOId），含产权人和购买人身份证
       public const string GetEoIdById = @"SELECT po.EOId
			FROM person p
				,property_owner po
			WHERE p.PersonId = po.PersonId
		AND p.IDNum = @id
		UNION
		SELECT b.EOId
			FROM buyer b
			WHERE b.IDNum = @id";


       /// <summary>
       /// 根据委估对象ID获取委托业务
       /// </summary>
       /// 20151026
       public const string GetEntrustAppraiseById = @"SELECT ea.EAId
			,ea.GJBEntrustId
            ,ea.FXTCompanyId
			,CASE
				ea.EntrustCensusRegister 
				WHEN 1 
				THEN '本市户籍' 
				WHEN 0 
				THEN '非本市户籍' 
				ELSE '' 
			END AS EntrustCensusRegister
			,CASE
				ea.BorrowerIsPropertyOwner 
				WHEN 1 
				THEN '是' 
				WHEN 0 
				THEN '否' 
				ELSE '' 
			END AS BorrowerIsPropertyOwner
			,ea.EntrustIDNum
			,ea.EntrustPhone
			,ea.ClientContact
			,ea.ClientContactPhone
			,CASE
				ea.BuyingType 
				WHEN 1 
				THEN '抵押' 
				WHEN 2 
				THEN '按揭' 
				ELSE '' 
			END AS BuyingType
			,ea.LendingBank
			,CASE
				ea.Guarantor4Mortgage 
				WHEN 1 
				THEN '是' 
				WHEN 0 
				THEN '否' 
				ELSE '' 
			END AS Guarantor4Mortgage
			,ea.AppraiseAgency
			,ea.Appraiser
			,ea.Assigner
			,CASE
				ea.ApplicationStatus 
				WHEN 1 
				THEN '申请后' 
				WHEN 0 
				THEN '申请前' 
				WHEN - 1 
				THEN '抵押' 
				ELSE '' 
			END AS ApplicationStatus
			,CASE
				ea.AppraiseStatus 
				WHEN 1 
				THEN '已完成' 
				WHEN 0 
				THEN '未完成' 
				ELSE '' 
			END AS AppraiseStatus
			,ea.AppraisePurpose
			,ea.RealEstateAgency
			,ea.RealEstateBroker 
		FROM entrust_appraise ea
			,entrust_object eo
		WHERE ea.EAId = eo.EAId
        AND ea.BusinessStateCode!=10013005
		AND eo.valid = 1
		AND eo.EOId in(SELECT po.EOId
			FROM person p
				,property_owner po
			WHERE p.PersonId = po.PersonId
		AND p.IDNum = @id
		UNION
		SELECT b.EOId
			FROM buyer b
			WHERE b.IDNum = @id)
		GROUP BY ea.EAId";
	   
	   
	   
	   
	   
       // public const string GetEntrustAppraiseById = @"  SELECT ea.EAId,ea.GJBEntrustId,
    // CASE ea.EntrustCensusRegister
         // WHEN 1 THEN '本市户籍'
         // WHEN 0 THEN '非本市户籍'
         // ELSE ''
       // END  as EntrustCensusRegister,
 // CASE ea.BorrowerIsPropertyOwner
         // WHEN 1 THEN '是'
         // WHEN 0 THEN '否'
         // ELSE ''
       // END  as BorrowerIsPropertyOwner,
   // ea.EntrustIDNum,
   // ea.EntrustPhone,
   // ea.ClientContact,
   // ea.ClientContactPhone,
   // CASE ea.BuyingType
        // WHEN 1 THEN '抵押'
        // WHEN 2 THEN '按揭'
        // ELSE ''
   // END as BuyingType,
   // ea.LendingBank,
   // CASE ea.Guarantor4Mortgage
        // WHEN 1 THEN '是'
        // WHEN 0 THEN '否'
        // ELSE ''
   // END as Guarantor4Mortgage,
   // ea.AppraiseAgency,
   // ea.Appraiser,
   // ea.Assigner,
  // CASE ea.ApplicationStatus
         // WHEN 1 THEN '申请后'
         // WHEN 0 THEN '申请前'
		 // WHEN -1 THEN '抵押'
         // ELSE ''
       // END  as ApplicationStatus,
        // CASE ea.AppraiseStatus
         // WHEN 1 THEN '已完成'
         // WHEN 0 THEN '未完成'
         // ELSE ''
       // END  as AppraiseStatus,
 // ea.AppraisePurpose,
// ea.RealEstateAgency,
// ea.RealEstateBroker
// FROM Person p
// INNER JOIN Property_Owner po ON po.PersonId = p.PersonId
// INNER JOIN ENTRUST_OBJECT eo ON eo.EOId = po.EOId
// RIGHT JOIN Entrust_Appraise ea ON ea.EAId = eo.EAId
// WHERE p.IDNum = @id and ea.valid = 1 and eo.valid = 1 group by ea.EAId";

       /// <summary>
       /// 根据委估对象ID获取委托对象
       /// </summary>
       /// 20151026
       public const string GetEntrustObjectById = @"SELECT eo.EOId
			,eo.GJBObjId
			,sci.GBCode AS CityCode
			,sci.CityName
			,sa.GBCode AS AreaCode
			,sa.AreaName
			,eo.Address
			,eo.LandValueInTermsOfPerUnitFloor
			,eo.ProjectName
			,eo.BuildingName
			,eo.BuildingStructure
			,eo.BuildingDate
			,eo.TotalFloor
			,eo.HouseName
			,eo.`Floor`
			,eo.RoomNum
			,eo.BalconyNum
			,eo.BuildingArea
			,eo.LandArea
			,eo.PracticalArea
			,eo.Fitment
			,eo.ObjectFullName
			,ptr.TranDate
			,pc.PropertyCertificateRegistePrice
			,pc.PropertyCertificateRegisteDate
			,ptr.PrepareLoanAmount
			,ptr.TranPrice
			,pc.PropertyCertificateNum
			,pc.LandCertificateDate
			,pc.LandCertificateArea
			,pc.LandCertificateAddress
			,CASE
				ptr.IsFirstBuy 
				WHEN 1 
				THEN '是' 
				WHEN 0 
				THEN '否' 
				ELSE '' 
			END AS IsFirstBuy
			,eo.Surveyor
			,CASE
				eo.IsSurvey 
				WHEN 1 
				THEN '是' 
				WHEN 0 
				THEN '否' 
				ELSE '' 
			END AS IsSurvey
			,eo.SurveyBeginTime
			,eo.SurveyEndTime
			,ptr.FinancingPurpose
			,eo.`Usage`
			,eo.DecorationValue
			,eo.BusLineNum
			,eo.HousingLocation
			,eo.PublicFacilitiesNum
			,CASE
				eo.OnlyLivingRoom 
				WHEN 1 
				THEN '是' 
				WHEN 0 
				THEN '否' 
				ELSE '' 
			END AS OnlyLivingRoom
			,eo.Number AS MembersOfLayer
			,eo.CompleteTime
			,eo.surveyhouseage AS SurveyHouseAge
			,eo.Wall
			,eo.Front
			,eo.Sight
			,eo.PropertyPrice AS MaterialCost
			,eo.Villa
			,eo.AverageHouse
			,eo.NotAverageHouse
			,eo.GreenEnvironment
			,eo.AirQuality
			,eo.landusetype
			,eo.HouseStruct
			,eo.HouseName
			,eo.HallCount
			,eo.BathroomCount
			,eo.HasKitchen
			,eo.Terrace
			,eo.SingleBalcony
			,eo.SingleBalconyarea AS SingleBalconyArea
			,eo.TallBalcony
			,eo.TallBalconyarea AS TallBalconyArea
			,eo.Roof
			,eo.Garden
			,eo.StructNewProbability
			,eo.LayerHigh
			,eo.Ventilation
			,eo.NoisePollution
			,eo.DecorationProbabilit
			,eo.LvYear
			,eo.ParlorCeiling
			,eo.ParlorWall
			,eo.ParlorGround
			,eo.BedroomCeiling
			,eo.BedroomWall
			,eo.BedroomGround
			,eo.KitchenCeiling
			,eo.KitchenWall
			,eo.KitchenGround
			,eo.KitchenDesk
			,eo.surveyhousekitchencupboards
			,eo.ToiletsCeiling
			,eo.ToiletsWall
			,eo.ToiletsGround
			,eo.ToiletsHealth
			,eo.ToiletsBath
			,eo.Toilet
			,eo.BigDoor
			,eo.InDoor
			,eo.RoomDoor
			,eo.Window
			,eo.IntelligentSystems
			,eo.SmokeSystems
			,eo.SpraySystems
			,eo.GasSystems
			,eo.IntercomSystems
			,eo.Broadband
			,eo.Cabletelevision
			,eo.Phone
			,eo.Heating
			,eo.ClientElevator
			,eo.CabinBrand
			,eo.LadderBrand
			,eo.IsUpCarLocation
			,eo.IsDownCarLocation
			,eo.IsCar
			,eo.CarOccupy
			,eo.Movement
			,eo.Club
			,eo.HealthCenter
			,eo.PostOffice
			,eo.Bank
			,eo.Market
			,eo.HighSchool
			,eo.PrimarySchool
			,eo.Nursery
			,eo.roadname AS RoadName
			,eo.roadwidth AS RoadWidth
			,eo.TrafficConvenient
			,eo.BusDistance
			,eo.Metro
			,eo.SubwayDistance
			,eo.roadtrafficflow
			,eo.TrafficManagement
			,eo.Side
			,eo.Environment
			,eo.loclat AS Loclat
			,eo.loclng AS Loclng
			,aop.AutoPrice
			,aop.MainHouseUnitPrice
			,aop.MainHouseTotalPrice
			,aop.OutbuildingTotalPrice
			,aop.LandUnitPrice
			,aop.LandTotalPrice
			,aop.AppraiseTotalPrice
			,aop.ValueDate
			,ptr.DownPayment 
		FROM Entrust_Object eo 
			INNER JOIN Property_Transaction_Recode ptr ON eo.EOId = ptr.EOId
			INNER JOIN Appraise_Object_Price aop ON eo.EOId = aop.EOId
			INNER JOIN Property_Certificate pc ON eo.EOId = pc.EOId
			LEFT JOIN openplatform.Sys_City sci ON eo.cityid = sci.cityid
			LEFT JOIN openplatform.Sys_Area sa ON eo.areaid = sa.areaid
		WHERE eo.EAId = @EAId
		AND eo.valid = 1";
	   
	
	
	
       // public const string GetEntrustObjectById = @"SELECT eo.EOId, eo.GJBObjId
    // ,sci.GBCode AS CityCode 
    // ,sci.CityName
    // ,sa.GBCode AS AreaCode
    // ,sa.AreaName
    // ,eo.Address
    // ,eo.LandValueInTermsOfPerUnitFloor
    // ,eo.ProjectName
    // ,eo.BuildingName
    // ,eo.BuildingStructure
    // ,eo.BuildingDate
    // ,eo.TotalFloor
    // ,eo.HouseName
    // ,eo.`Floor`
    // ,eo.RoomNum
    // ,eo.BalconyNum
    // ,eo.BuildingArea
    // ,eo.LandArea
    // ,eo.PracticalArea
    // ,eo.Fitment
    // ,eo.ObjectFullName
    // ,ptr.TranDate 
    // ,pc.PropertyCertificateRegistePrice
    // ,pc.PropertyCertificateRegisteDate
    // ,ptr.PrepareLoanAmount
    // ,ptr.TranPrice
    // ,pc.PropertyCertificateNum
    // ,pc.LandCertificateDate
    // ,pc.LandCertificateArea
    // ,pc.LandCertificateAddress
    // ,CASE ptr.IsFirstBuy 
        // WHEN 1 THEN '是'
        // WHEN 0 THEN '否'
        // ELSE ''
    // END AS IsFirstBuy 
    // ,eo.Surveyor
    // ,CASE eo.IsSurvey  
        // WHEN 1 THEN '是'
        // WHEN 0 THEN '否'
        // ELSE '' 
    // END AS IsSurvey 
    // ,eo.SurveyBeginTime
    // ,eo.SurveyEndTime
    // ,ptr.FinancingPurpose
    // ,eo.`Usage` 
    // ,eo.DecorationValue
    // ,eo.BusLineNum
    // ,eo.HousingLocation
    // ,eo.PublicFacilitiesNum
 // ,CASE eo.OnlyLivingRoom 
        // WHEN 1 THEN '是'
        // WHEN 0 THEN '否'
        // ELSE '' 
    // END AS OnlyLivingRoom 
// ,eo.Number AS MembersOfLayer
// ,eo.CompleteTime
// ,eo.surveyhouseage AS SurveyHouseAge
// ,eo.Wall
// ,eo.Front
// ,eo.Sight
// ,eo.PropertyPrice AS MaterialCost
// ,eo.Villa
// ,eo.AverageHouse
// ,eo.NotAverageHouse
// ,eo.GreenEnvironment
// ,eo.AirQuality
// ,eo.landusetype
// ,eo.HouseStruct
// ,eo.HouseName
// ,eo.HallCount
// ,eo.BathroomCount
// ,eo.HasKitchen
// ,eo.Terrace
// ,eo.SingleBalcony
// ,eo.SingleBalconyarea AS SingleBalconyArea
// ,eo.TallBalcony
// ,eo.TallBalconyarea AS TallBalconyArea
// ,eo.Roof
// ,eo.Garden
// ,eo.StructNewProbability
// ,eo.LayerHigh
// ,eo.Ventilation
// ,eo.NoisePollution
// ,eo.DecorationProbabilit
// ,eo.LvYear
// ,eo.ParlorCeiling
// ,eo.ParlorWall
// ,eo.ParlorGround
// ,eo.BedroomCeiling
// ,eo.BedroomWall
// ,eo.BedroomGround
// ,eo.KitchenCeiling
// ,eo.KitchenWall
// ,eo.KitchenGround
// ,eo.KitchenDesk
// ,eo.surveyhousekitchencupboards
// ,eo.ToiletsCeiling
// ,eo.ToiletsWall
// ,eo.ToiletsGround
// ,eo.ToiletsHealth
// ,eo.ToiletsBath
// ,eo.Toilet
// ,eo.BigDoor
// ,eo.InDoor
// ,eo.RoomDoor
// ,eo.Window
// ,eo.IntelligentSystems
// ,eo.SmokeSystems
// ,eo.SpraySystems
// ,eo.GasSystems
// ,eo.IntercomSystems
// ,eo.Broadband
// ,eo.Cabletelevision
// ,eo.Phone
// ,eo.Heating
// ,eo.ClientElevator
// ,eo.CabinBrand
// ,eo.LadderBrand
// ,eo.IsUpCarLocation
// ,eo.IsDownCarLocation
// ,eo.IsCar
// ,eo.CarOccupy
// ,eo.Movement
// ,eo.Club
// ,eo.HealthCenter
// ,eo.PostOffice
// ,eo.Bank
// ,eo.Market
// ,eo.HighSchool
// ,eo.PrimarySchool
// ,eo.Nursery
// ,eo.roadname AS RoadName
// ,eo.roadwidth AS RoadWidth
// ,eo.TrafficConvenient
// ,eo.BusDistance
// ,eo.Metro
// ,eo.SubwayDistance
// ,eo.roadtrafficflow
// ,eo.TrafficManagement
// ,eo.Side
// ,eo.Environment
// ,eo.loclat AS Loclat
// ,eo.loclng AS Loclng
    // ,aop.AutoPrice
    // ,aop.MainHouseUnitPrice
    // ,aop.MainHouseTotalPrice
    // ,aop.OutbuildingTotalPrice
    // ,aop.LandUnitPrice
    // ,aop.LandTotalPrice
    // ,aop.AppraiseTotalPrice
    // ,aop.ValueDate
    // ,ptr.DownPayment 
    // FROM Entrust_Object eo 
    // INNER JOIN Property_Transaction_Recode ptr ON eo.EOId = ptr.EOId
    // INNER JOIN Appraise_Object_Price aop ON eo.EOId = aop.EOId
    // INNER JOIN Property_Certificate pc ON eo.EOId = pc.EOId
    // LEFT JOIN Sys_City sci ON eo.cityid = sci.cityid
    // LEFT JOIN Sys_Area sa ON eo.areaid = sa.areaid
    // WHERE eo.EAId = @EAId AND eo.valid = 1";



       /// <summary>
       /// 获取产权人
       /// </summary>
       public const string GetPropertyOwnerById = @"SELECT po.RightPercent,p.PersonName,p.IDNum,p.Phone1 AS Phone,p.Contacts,p.Relation,p.ContractPhone,p.MaritalStatus,p.HasChildren,
CASE p.OwnerCensusRegister
         WHEN 1 THEN '本市户籍'
         WHEN 0 THEN '非本市户籍'
         ELSE ''
       END  as OwnerCensusRegister
FROM Property_Owner po
INNER JOIN Person p ON p.PersonId = po.PersonId
WHERE po.EOId = @EOId";

       /// <summary>
       /// 获取图片列表
       /// </summary>
       public const string GetPathListById = @"select Path from survey_files where EOId = @EOId";

       /// <summary>
       /// 获取买房人信息
       /// </summary>
       public const string GetBuyerInfoById = @"SELECT BuyerName,IDNum as BuyerIdNum,Phone as BuyerPhone,
CASE BuyerCensusRegister
         WHEN 1 THEN '本市户籍'
         WHEN 0 THEN '非本市户籍'
         ELSE ''
       END  as BuyerCensusRegister
 FROM buyer where EOId = @EOId";

       /// <summary>
       /// 插入委估业务信息
       /// </summary>
       public const string InsertEntrust = @"process_entrust_data_prc";

       /// <summary>
       /// 插入委估对象信息
       /// </summary>
       public const string InsertEntrustObject = @"process_entrustobject_data_prc";

       /// <summary>
       /// 插入产权信息
       /// </summary>
       public const string InsertProperty = @"process_property_data_prc";

       /// <summary>
       /// 插入买房人信息
       /// </summary>
       public const string InsertBuyer = @"process_buyer_data_prc";

       /// <summary>
       /// 插入照片信息
       /// </summary>
       public const string InsertPicture = @"process_files_data_prc";
   }
}
