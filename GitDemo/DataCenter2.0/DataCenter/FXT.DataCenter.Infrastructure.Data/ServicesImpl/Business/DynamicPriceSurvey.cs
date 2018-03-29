using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class DynamicPriceSurvey : IDynamicPriceSurvey
    {

        public IQueryable<Dat_P_B_Price_Biz> GetDynamicPriceSurveys(Dat_P_B_Price_Biz dynamicPriceSurvey, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(dynamicPriceSurvey.CityId, dynamicPriceSurvey.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = dynamicPriceSurvey.FxtCompanyId.ToString();
            if (self) comId = dynamicPriceSurvey.FxtCompanyId.ToString();

            var strSql = @"select pb.*,p.ProjectName,b.BuildingName,s.CodeName as RentTypeName,s1.CodeName as SurveyTypeName
from FxtData_Biz.dbo.Dat_P_B_Price_Biz pb with(nolock)
left join FxtData_Biz.dbo.Dat_Project_Biz p with(nolock) on p.ProjectId = pb.ProjectId
left join FxtData_Biz.dbo.Dat_Building_Biz b with(nolock) on b.BuildingId = pb.BuildingId
left join FxtDataCenter.dbo.sys_code s with(nolock) on s.code = pb.RentTypeCode
left join FxtDataCenter.dbo.sys_code s1 with(nolock) on s1.code = pb.SurveyTypeCode
where pb.Valid= 1 and  pb.CityId = @cityId and pb.FxtCompanyId in (" + comId + ")";

            if (!string.IsNullOrWhiteSpace(dynamicPriceSurvey.ProjectName)) strSql += "  and p.ProjectName like '%'+@ProjectName+'%'";

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.projectId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                totalCount = conn.Query<int>(totalCountSql, dynamicPriceSurvey).FirstOrDefault();
                return conn.Query<Dat_P_B_Price_Biz>(pagenatedSql, dynamicPriceSurvey).AsQueryable();
            }


        }

        public Dat_P_B_Price_Biz GetDynamicPriceSurveyById(int id)
        {
            var strSql = @"select pb.* from FxtData_Biz.dbo.Dat_P_B_Price_Biz pb with(nolock)
where Id= @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<Dat_P_B_Price_Biz>(strSql, new { id }).FirstOrDefault();
            }
        }


        public int GetDynamicPriceSurveyId(long projectId, long buildingId, int cityId, int fxtCompanyId)
        {
            var strSql = @"select pb.id from FxtData_Biz.dbo.Dat_P_B_Price_Biz pb with(nolock)
where ProjectId=@projectId and BuildingId= @buildingId and CityId=@cityId and FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<int>(strSql, new { projectId, buildingId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddDynamicPriceSurvey(Dat_P_B_Price_Biz dynamicPriceSurvey)
        {
            var strSql = @"insert into FxtData_Biz.dbo.Dat_P_B_Price_Biz (cityid,projectid,buildingid,renttypecode,avgrent,avgsaleprice,rent1,rent2,saleprice1,saleprice2,tenantarea,vacantarea,vacantrate,rentsalerate,managerprice,surveydate,surveyuser,surveytypecode,fxtcompanyid,creator,createtime) 
values(@cityid,@projectid,@buildingid,@renttypecode,@avgrent,@avgsaleprice,@rent1,@rent2,@saleprice1,@saleprice2,@tenantarea,@vacantarea,@vacantrate,@rentsalerate,@managerprice,@surveydate,@surveyuser,@surveytypecode,@fxtcompanyid,@creator,@createtime)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }

        public int UpdateDynamicPriceSurvey(Dat_P_B_Price_Biz dynamicPriceSurvey)
        {
            var strSql = @"update FxtData_Biz.dbo.Dat_P_B_Price_Biz  set projectid = @projectid,buildingid = @buildingid,renttypecode = @renttypecode,avgrent = @avgrent,avgsaleprice = @avgsaleprice,rent1 = @rent1,rent2 = @rent2,saleprice1 = @saleprice1,saleprice2 = @saleprice2,tenantarea = @tenantarea,vacantarea = @vacantarea,vacantrate = @vacantrate,rentsalerate = @rentsalerate,managerprice = @managerprice,surveydate = @surveydate,surveyuser = @surveyuser,surveytypecode = @surveytypecode,savedatetime = @savedatetime,saveuser = @saveuser
where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }

        public int DeleteDynamicPriceSurvey(int id)
        {
            var strSql = @"update FxtData_Biz.dbo.Dat_P_B_Price_Biz  set valid = 0 where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Execute(strSql, new { id });
            }
        }

        #region 公共

        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[CaseTable],s.BizCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),FxtDataCenter.dbo.[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<AccessTable>(sql, new { cityid, fxtcompanyid }).FirstOrDefault();
                ptable = query == null ? "" : query.ProjectTable;
                ctable = query == null ? "" : query.CaseTable;
                btable = query == null ? "" : query.BuildingTable;
                comId = query == null ? "" : query.BizCompanyId;
            }

        }

        private class AccessTable
        {
            public string ProjectTable { get; set; }
            public string CaseTable { get; set; }
            public string BuildingTable { get; set; }
            public string BizCompanyId { get; set; }
        }

        #endregion
    }
}
