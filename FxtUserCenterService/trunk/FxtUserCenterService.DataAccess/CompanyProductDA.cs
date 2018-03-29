using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Entity;
using FxtUserCenterService.Entity.InheritClass;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace FxtUserCenterService.DataAccess
{
    public class CompanyProductDA : Base
    {
        private static readonly string openplatformContextStr = ConfigurationManager.ConnectionStrings["OpenplatformContext"].ToString();//默认数据库

        /// <summary>
        /// 根据公司id和产品code获取信息(caoq 2013-7-12)
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="producttypecode">产品code</param>
        /// <param name="signname">公司标识</param>
        /// <param name="isvalid">是否有效产品 1:仅查询有效产品</param>
        /// <param name="companycode">公司编码</param>
        /// <returns></returns>
        public static List<CompanyProduct> Get(int companyid, string producttypecode, string signname, int isvalid, string companycode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"select p.*
                from dbo.CompanyProduct p with(nolock)
                join dbo.companyinfo c with(nolock) on c.companyid=p.companyid
                where 1=1 ";
            if (companyid > 0)
            {
                sql += " and p.CompanyId=@companyid";
                parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(producttypecode))
            {
                sql += " and p.ProductTypeCode in (" + producttypecode + ")";
            }
            if (!string.IsNullOrEmpty(signname))
            {
                sql += " and c.signname=@signname";
                parameters.Add(SqlHelper.GetSqlParameter("@signname", signname, SqlDbType.VarChar));
            }
            if (isvalid == 1)
            {
                sql += " and OverDate > getdate()";
            }

            if (!string.IsNullOrEmpty(companycode))
            {
                sql += " and c.companycode=@companycode";
                parameters.Add(SqlHelper.GetSqlParameter("@companycode", companycode, SqlDbType.VarChar));
            }

            return ExecuteToEntityList<CompanyProduct>(sql, CommandType.Text, parameters);
        }


        /// <summary>
        /// 修改产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话(hody 2014-04-24)
        /// </summary>
        /// <param name="logoPath">CAS产品LOGO</param>
        /// <param name="smallLogoPath">CAS产品小LOGO</param>
        /// <param name="telephone">对外显示的产品名</param>
        /// <param name="titleName">产品联系电话</param>
        /// <returns></returns>
        public static int UpdateProductPartialInfo(string logoPath, string smallLogoPath, string telephone, string titleName, int companyid, int systypecode, string bgpic, string homepage, string twodimensionalcode)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = SQLName.CompanyProduct.UpdateProductPartialInfo;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@logopath", logoPath, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@smalllogopath", smallLogoPath, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@telephone", telephone, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@titlename", titleName, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@homepage", homepage, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@twodimensionalcode", twodimensionalcode, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@bgpic", bgpic, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@systypecode", systypecode, SqlDbType.Int));
            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 根据WebUrl查询产品信息
        /// </summary>
        /// <param name="weburl">网址</param>
        /// <param name="weburl1">备用网址</param>
        /// <returns></returns>
        public static InheritCompanyProduct GetProductInfoByWebUrl(string weburl, string weburl1)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQLName.CompanyProduct.GetProductInfoByWebUrl;
            parameters.Add(SqlHelper.GetSqlParameter("@weburl", weburl, SqlDbType.VarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@weburl1", weburl1, SqlDbType.VarChar));
            return ExecuteToEntity<InheritCompanyProduct>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 产品授权是否已存在
        /// 20150814 wb add
        /// 说明：1.companyid, producttypecode, cityid此三字体为组合主键
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static bool CheckedCompanyProductExists(int companyid, int producttypecode, int cityid)
        {
            string sqlExists = string.Format(
                        "select CompanyId from CompanyProduct where CompanyId={0} and ProductTypeCode={1} and CityId={2}",
                        companyid, producttypecode, cityid
                    );

            int res = Convert.ToInt32(ExecuteScalar(sqlExists, CommandType.Text));
            if (res > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 获取CompanyProduct List根据companyid、producttypecodes、cityid
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecodes"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static List<CompanyProduct> GetCompanyProductList(int companyid, int[] producttypecodes, int cityid)
        {
            if (companyid <= 0 || producttypecodes.Length == 0)
                return null;

            string pro = (producttypecodes == null || producttypecodes.Length == 0) ? "" : string.Join(",", producttypecodes.Select(i => i.ToString()).ToArray());

            string sql = string.Format(
                        "select * from CompanyProduct where CompanyId={0} and ProductTypeCode in ({1}) and CityId={2}",
                        companyid, pro, cityid
                    );

            return ExecuteToEntityList<CompanyProduct>(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 云查勘升级
        /// 20150814 wb add
        /// </summary>
        /// <param name="companyid">客户</param>
        /// <param name="producttypecode">需要升级的产品编码</param>
        /// <param name="newproducttypecode">升级后的产品编码</param>
        public static int UpdateCompanyProductPtc(int companyid, int producttypecode, int newproducttypecode)
        {
            if (companyid <= 0 || producttypecode <= 0 || newproducttypecode <= 0)
                throw new Exception("云查勘升级：参数有误");

            string sql = string.Format("update CompanyProduct set ProductTypeCode={2} where CompanyId={0} and producttypecode={1} and AppAbbreviation='yck'",
                companyid, producttypecode);

            return ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 根据产品code、公司ID查询已开通城市权限
        /// zhoub 20160531
        /// </summary>
        /// <param name="producttypecode">产品code</param>
        /// <param name="companyid">公司ID</param>
        /// <returns></returns>
        public static List<CompanyProduct> GetCompanyProductByCodeAndCompanyIdAndCityId(int producttypecode, int companyid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = "select * from fxtusercenter.dbo.companyproduct where producttypecode =@producttypecode and companyid =@companyid";
            parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", producttypecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            return ExecuteToEntityList<CompanyProduct>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 流量配置
        /// zhoub 20160621
        /// </summary>
        /// <param name="companyid">公司ID</param>
        /// <returns></returns>
        public static int FlowControlConfig(int companyid)
        {
            int result = 0;
            MySqlConnection myConn = new MySqlConnection(openplatformContextStr);
            MySqlCommand myComm = new MySqlCommand("add_data_for_flow_control_config_prc", myConn);//(Client_Str);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.StoredProcedure;
                MySqlParameter myParameter;
                myParameter = new MySqlParameter("?v_companyid", MySqlDbType.Int32);
                myParameter.Value = companyid;
                myParameter.Direction = ParameterDirection.Input;
                myComm.Parameters.Add(myParameter);
                result = myComm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                myComm.Connection.Close();
                myComm.Dispose();
            }
            finally
            {
                myComm.Connection.Close();
                myComm.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 查询公司是否包含该产品和城市
        /// zhoub 20160914
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static List<CompanyProduct> GetCompanyProductByCompanyidAndProductTypeCode(int companyid, int producttypecode, int cityid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"select top 1 CompanyId,IsDeleteTrue,IsExportHose,parentproducttypecode,parentshowdatacompanyid from FxtUserCenter.dbo.CompanyProduct where CompanyId = @companyid and ProductTypeCode = @producttypecode and (CityId = 0 or CityId = @cityid) and StartDate <= GETDATE() and OverDate >= GETDATE() and Valid = 1 order by CityId desc";
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", producttypecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            return ExecuteToEntityList<CompanyProduct>(sql, CommandType.Text, parameters);
        }
    }
}
