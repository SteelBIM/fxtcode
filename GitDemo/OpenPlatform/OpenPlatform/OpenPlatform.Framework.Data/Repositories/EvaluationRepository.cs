using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using OpenPlatform.Domain.Models;
using OpenPlatform.Domain.Repositories;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Framework.Data.SQL;

namespace OpenPlatform.Framework.Data.Repositories
{
    public class EvaluationRepository : IEvaluationRepository
    {

        public IQueryable<long> GetAppraiseStatusById(string id, int fxtCompanyId)
        {
            var strSql = EvaluationSql.GetAppraiseStatusById;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<long>(strSql, new { id, fxtCompanyId }).AsQueryable();

            }
        }

        public IQueryable<long> GetEoId(string id)
        {
            var strSql = EvaluationSql.GetEoIdById;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<long>(strSql, new { id }).AsQueryable();
            }
        }

        public IQueryable<EvaluationDto> GetEntrustAppraiseById(string id)
        {
            var strSql = EvaluationSql.GetEntrustAppraiseById;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<EvaluationDto>(strSql, new { id }).AsQueryable();
            }
        }


        public IQueryable<EntrustObjectDto> GetEntrustObjectById(long id)
        {
            var strSql = EvaluationSql.GetEntrustObjectById;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<EntrustObjectDto>(strSql, new { EAId = id }).AsQueryable();
            }
        }

        public IQueryable<PropertyInfoDto> GetPropertyBuyerById(long objectId)
        {
            var strSql = EvaluationSql.GetPropertyOwnerById;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<PropertyInfoDto>(strSql, new { EOId = objectId }).AsQueryable();
            }
        }

        public IQueryable<string> GetPathListById(long objectId)
        {
            var strSql = EvaluationSql.GetPathListById;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<string>(strSql, new { EOId = objectId }).AsQueryable();
            }
        }

        public IQueryable<BuyerInfoDto> GetBuyerInfoById(long objectId)
        {
            var strSql = EvaluationSql.GetBuyerInfoById;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<BuyerInfoDto>(strSql, new { EOId = objectId }).AsQueryable();
            }
        }


        public int AddEntrust(Evaluation4GjbDto ed)
        {
            var strSql = EvaluationSql.InsertEntrust;

            var obj = new
            {
                v_fxtcompanyid = ed.FxtCompanyId,
                v_gjbentrustid = ed.GjbEntrustId,
                v_entrustcensusregister = ed.EntrustCensusRegister,
                v_entrustphone = ed.EntrustPhone,
                v_entrustidnum = ed.EntrustIDNum,
                v_clientcontact = ed.ClientContact,
                v_clientcontactphone = ed.ClientContactPhone,
                v_buyingtype = ed.BuyingType,
                v_lendingbank = ed.LendingBank,
                v_guarantor4mortgage = ed.Guarantor4Mortgage,
                v_appraiseagency = ed.AppraiseAgency,
                v_appraiser = ed.Appraiser,
                v_assigner = ed.Assigner,
                v_applicationstatus = ed.ApplicationStatus,
                v_appraisestatus = ed.AppraiseStatus,
                v_valid = ed.Valid,
                v_entrustandpropertyownerrelation = ed.EntrustAndPropertyOwnerRelation,
                v_borrowerispropertyowner = ed.BorrowerIsPropertyOwner,
                v_appraisepurpose = ed.EvaluationPurpose,
                v_realestateagency = ed.RealEstateAgency,
                v_realestatebroker = ed.RealEstateBroker,
                v_businessstatecode = ed.BusinessStateCode

            };

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Execute(strSql, obj, commandType: CommandType.StoredProcedure);
            }
        }

        public int AddEntrustObject(EntrustObject4GjbDto eo)
        {
            var strSql = EvaluationSql.InsertEntrustObject;

            var obj = new
            {
                v_gjbobjid = eo.GjbObjId,
                v_gjbentrustid = eo.EntrustId,
                v_houseid = eo.HouseId,
                v_buildingid = eo.BuildingId,
                v_projectid = eo.ProjectId,
                v_cityid = eo.CityId,
                v_areaid = eo.AreaId,
                v_address = eo.Address,
                v_landvalueintermsofperunitfloor = eo.LandValueInTermsOfPerUnitFloor,
                v_projectname = eo.ProjectName,
                v_buildingname = eo.BuildingName,
                v_buildingstructure = eo.BuildingStructure,
                v_totalfloor = eo.TotalFloor,
                v_housename = eo.HouseName,
                v_floor = eo.Floor,
                v_roomnum = eo.RoomNum,
                v_balconynum = eo.BalconyNum,
                v_buildingarea = eo.BuildingArea,
                v_landarea = eo.LandArea,
                v_practicalarea = eo.PracticalArea,
                v_fitment = eo.Fitment,
                v_objectfullname = eo.ObjectFullName,
                v_trandate = eo.TranDate,
                v_propertycertificateregisteprice = eo.PropertyCertificateRegistePrice,
                v_propertycertificateregistedate = eo.PropertyCertificateRegisteDate,
                v_prepareloanamount = eo.PrepareLoanAmount,
                v_tranprice = eo.TranPrice,
                v_propertycertificatenum = eo.PropertyCertificateNum,
                v_landcertificatedate = eo.LandCertificateDate,
                v_landcertificatearea = eo.LandCertificateArea,
                v_landcertificateaddress = eo.LandCertificateAddress,
                v_isfirstbuy = eo.IsFirstBuy,
                v_surveyor = eo.Surveyor,
                v_issurvey = eo.IsSurvey,
                v_surveybegintime = eo.SurveyBeginTime,
                v_surveyendtime = eo.SurveyEndTime,
                v_financingpurpose = eo.FinancingPurpose,
                v_usage = eo.Usage,
                v_decorationvalue = eo.DecorationValue,
                v_buslinenum = eo.BusLineNum,
                v_housinglocation = eo.HousingLocation,
                v_publicfacilitiesnum = eo.PublicFacilitiesNum,
                v_autoprice = eo.AutoPrice,
                v_mainhouseunitprice = eo.MainHouseUnitPrice,
                v_mainhousetotalprice = eo.MainHouseTotalPrice,
                v_outbuildingtotalprice = eo.OutbuildingTotalPrice,
                v_landunitprice = eo.LandUnitPrice,
                v_landtotalprice = eo.LandTotalPrice,
                v_appraisetotalprice = eo.AppraiseTotalPrice,
                v_valuedate = eo.ValueDate,
                v_buildingdate = eo.BuildingDate,
                v_valid = eo.Valid,
                v_fxtcompanyid = eo.FxtCompanyId,
                v_downpayment = eo.DownPayment

            };

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Execute(strSql, obj, commandType: CommandType.StoredProcedure);
            }
        }

        public int AddProperty(PropertyInfo pi)
        {
            float rightPercent = 0;
            if (string.IsNullOrEmpty(pi.RightPercent))
            {
                rightPercent = 0;
            }
            else
            {
                var r = pi.RightPercent.Split('%');
                float p;
                var b = float.TryParse(r[0], out p);
                if (b)
                {
                    rightPercent = p / 100;
                }
            }

            var strSql = EvaluationSql.InsertProperty;
            var obj = new
            {
                v_gjbobjid = pi.GjbObjId,
                v_personname = pi.PersonName,
                v_idnum = pi.IdNum,
                v_rightpercent = rightPercent,
                v_phone1 = pi.Phone,
                v_contacts = pi.Contacts,
                v_relation = pi.Relation,
                v_contractphone = pi.ContractPhone,
                v_maritalstatus = pi.MaritalStatus,
                v_haschildren = pi.HasChildren,
                v_isfirstcall = pi.IsFirstCall,
                v_fxtcompanyid = pi.FxtCompanyId,
                v_ownercensusregister = pi.OwnerCensusRegister
            };

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Execute(strSql, obj, commandType: CommandType.StoredProcedure);
            }
        }

        public int AddBuyer(BuyerInfo bi)
        {
            var strSql = EvaluationSql.InsertBuyer;
            var obj = new
            {
                v_gjbobjid = bi.GJBObjId,
                v_buyername = bi.BuyerName,
                v_idnum = bi.BuyerIdNum,
                v_phone = bi.BuyerPhone,
                v_isfirstcall = bi.IsFirstCall,
                v_fxtcompanyid = bi.FxtCompanyId,
                v_buyercensusregister = bi.BuyerCensusRegister
            };

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Execute(strSql, obj, commandType: CommandType.StoredProcedure);
            }
        }

        public int AddPictures(Surveyfiles sf)
        {
            var strSql = EvaluationSql.InsertPicture;

            var obj = new
            {
                v_gjbobjid = sf.GJBObjId,
                v_name = sf.Name,
                v_path = sf.Path,
                v_uptime = default(DateTime),
                v_smallimgpath = sf.Smallimgpath,
                v_annextypecode = sf.Annextypecode,
                v_annextypesubcode = sf.Annextypesubcode,
                v_imagetype = sf.Imagetype,
                v_filesize = sf.Filesize,
                v_flietypecode = sf.Flietypecode,
                v_filesubtypecode = sf.Filesubtypecode,
                v_createdate = default(DateTime),
                v_remark = "",
                v_filecreatedate = default(DateTime),
                v_fxtcompanyid = sf.FxtCompanyId
            };


            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Execute(strSql, obj, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
