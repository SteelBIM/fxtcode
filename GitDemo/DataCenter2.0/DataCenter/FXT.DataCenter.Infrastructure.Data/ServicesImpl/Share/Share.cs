using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class Share : IShare
    {

        //public bool IsExport(int cityId, int fxtCompanyId)
        //{
        //    var strSql = @"select IsExportHose from CompanyProduct with(nolock) where CityId=@CityId and CompanyId=@FxtCompanyId and ProductTypeCode = 1003002 and GETDATE() < OverDate and Valid = 1";

        //    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
        //    {
        //        var ret = conn.Query<int>(strSql, new { cityId, fxtCompanyId }).AsQueryable().FirstOrDefault();

        //        return ret == 1;
        //    }
        //}

        //public void GetVQDataParent(int cityId, int fxtCompanyId, out int parentShowDataCompanyId, out int parentProductTypeCode)
        //{
        //    //除西安市外
        //    var strSql = @"select ParentProductTypeCode,ParentShowDataCompanyId from CompanyProduct with(nolock) where CompanyId = @fxtCompanyId and CityId = @CityId and CityId <> 147 and ProductTypeCode = 1003036 and GETDATE() < OverDate and Valid = 1";

        //    SqlParameter[] parameter = { new SqlParameter("@CityId", SqlDbType.Int), new SqlParameter("@fxtCompanyId", SqlDbType.Int) };
        //    parameter[0].Value = cityId;
        //    parameter[1].Value = fxtCompanyId;

        //    DBHelperSql.ConnectionString = ConfigurationHelper.FxtUserCenter;
        //    var dt = DBHelperSql.ExecuteDataTable(strSql, parameter);
        //    if (dt.Rows.Count == 0)
        //    {
        //        parentShowDataCompanyId = 0; parentProductTypeCode = 0;
        //    }
        //    else
        //    {
        //        parentShowDataCompanyId = int.Parse(dt.Rows[0]["ParentShowDataCompanyId"].ToString());
        //        parentProductTypeCode = int.Parse(dt.Rows[0]["ParentProductTypeCode"].ToString());
        //    }
        //}
    }
}
