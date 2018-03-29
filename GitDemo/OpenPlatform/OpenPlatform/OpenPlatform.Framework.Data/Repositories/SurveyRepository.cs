using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Domain.Models;
using OpenPlatform.Domain.Repositories;
using OpenPlatform.Framework.Data.SQL;

namespace OpenPlatform.Framework.Data.Repositories
{
    public class SurveyRepository : ISurveyRepository
    {
        public IQueryable<int> GetCompanyId(string idNum)
        {
            //var strSql = SurveySql.GetCompanyId;

            //using (var conn = Dapper.SqlConnection())
            //{
            //    return conn.Query<int>(strSql, new { idNum = "%" + idNum + "%" }).AsQueryable();
            //}

            return null;
        }

        public IQueryable<Surveyfiles> GetPicturesById(long objectId)
        {
            //var strSql = SurveySql.GetPicturesById;

            //using (var conn = Dapper.SqlConnection())
            //{
            //    return conn.Query<Surveyfiles>(strSql, new { objectId }).AsQueryable();
            //}
            return null;
        }

        public IQueryable<SurveyInfoDto> GetSurveyInfoById(long objectId)
        {

            return null;
            //var strSql = SurveySql.GetSurveyInfoById;

            //using (var conn = Dapper.SqlConnection())
            //{
            //    var query = conn.Query<SurveyInfoDto>(strSql, new { objectId }).AsQueryable();

            //    var list = new List<SurveyInfoDto>();
            //    //foreach (var item in query)
            //    //{
            //    //    var customFields = JsonConvert.DeserializeObject<List<CustomFields>>(item.CustomFields);
            //    //    var s = new SurveyInfoDto();
            //    //    foreach (var c in customFields)
            //    //    {
            //    //        if (c.c.Trim() == "装修信息")
            //    //        {
            //    //            foreach (var v in c.v)
            //    //            {
            //    //                if (v.c.Trim() == "装修价值")
            //    //                {
            //    //                    if (string.IsNullOrEmpty(v.v))
            //    //                    {
            //    //                        s.DecorationValue = null;
            //    //                    }
            //    //                    else
            //    //                    {
            //    //                        s.DecorationValue = Convert.ToDecimal(v.v);
            //    //                    }
            //    //                }

            //    //            }
            //    //        }
            //    //        if (c.c.Trim() == "交通条件")
            //    //        {
            //    //            foreach (var v in c.v)
            //    //            {
            //    //                if (v.c.Trim() == "公交线路数量")
            //    //                {
            //    //                    s.BusLineNum = v.s;
            //    //                }
            //    //                if (v.c.Trim() == "房产位置")
            //    //                {
            //    //                    s.HousingLocation = v.s;
            //    //                }
            //    //            }
            //    //        }
            //    //        if (c.c.Trim() == "周边配套")
            //    //        {
            //    //            foreach (var v in c.v)
            //    //            {
            //    //                if (v.c.Trim() == "公共配套设施数量")
            //    //                {
            //    //                    s.PublicFacilitiesNum = v.s;
            //    //                }

            //    //            }
            //    //        }
            //    //        if (c.c.Trim() == "个人信息")
            //    //        {
            //    //            foreach (var v in c.v)
            //    //            {
            //    //                if (v.c.Trim() == "婚姻状况")
            //    //                {
            //    //                    s.MaritalStatus = v.s;
            //    //                }
            //    //                if (v.c.Trim() == "有无子女")
            //    //                {
            //    //                    s.HasChildren = v.s;
            //    //                }
            //    //                if (v.c.Trim() == "融资目的")
            //    //                {
            //    //                    s.FinancingPurpose = v.s;
            //    //                }

            //    //            }
            //    //        }
            //    //        if (c.c.Trim() == "房产信息")
            //    //        {
            //    //            foreach (var v in c.v)
            //    //            {
            //    //                if (v.c.Trim() == "使用情况")
            //    //                {
            //    //                    s.Usage = v.s;
            //    //                }
            //    //            }
            //    //        }
            //    //    }

            //    //    s.IsSurvey = item.IsSurvey;
            //    //    s.Surveyor = item.Surveyor;
            //    //    s.SurveyBeginTime = item.SurveyBeginTime;
            //    //    s.SurveyEndTime = item.SurveyEndTime;
            //    //    s.IdNum = item.IdNum;

            //    //    list.Add(s);
            //    //}


            //    return list.AsQueryable();
        }
    }
}

