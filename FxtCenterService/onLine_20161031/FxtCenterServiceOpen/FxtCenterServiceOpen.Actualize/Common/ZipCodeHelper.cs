using CAS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace FxtCenterServiceOpen.Actualize.Common
{
    public class ZipCodeHelper
    {
        /// <summary>
        /// 获取城市列表信息（ZipCode，CityId）
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetZipCodeCityIdMap(string zipcode = "")
        {
            Dictionary<string, string> dicZipCodeCityIdMap = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            string sql = "select CityId,zipcode from FxtDataCenter.dbo.SYS_City";
            if (!string.IsNullOrEmpty(zipcode))
                sql += " where zipcode=@zipcode";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["FXTConnectionString"].ToString();
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (!string.IsNullOrEmpty(zipcode))
                cmd.Parameters.Add(new SqlParameter("@zipcode", zipcode));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string cityId = row["cityid"].ToString();
                    string zipCodeItem = row["zipcode"].ToString();
                    zipCodeItem = zipCodeItem == null ? "" : zipCodeItem;
                    if (!dicZipCodeCityIdMap.ContainsKey(zipCodeItem) && !string.IsNullOrEmpty(zipCodeItem))
                    {
                        dicZipCodeCityIdMap.Add(zipCodeItem, cityId);
                    }
                }
            }
            return dicZipCodeCityIdMap;
        }

        public static bool IsXml(string Path)
        {
            StreamReader sr = new StreamReader(Path);
            string strXml = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(strXml);//判断是否加载成功
                return true;//是xml文件，返回
            }
            catch
            {
                return false;//不是xml文件，返回
            }
        }
    }
}