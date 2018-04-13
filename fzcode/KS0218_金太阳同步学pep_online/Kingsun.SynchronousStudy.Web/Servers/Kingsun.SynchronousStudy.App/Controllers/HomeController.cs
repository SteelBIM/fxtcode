using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.SynchronousStudy.Pay.business;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration = int.MaxValue)]
        public ActionResult Index()
        {
            //string retString = string.Empty;
            //for (int i = 0; i < 0; i++)
            //{
            //    string serviceAddress = "http://localhost:65423/api/vedioinfo/GetVideoListTest";
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            //    request.Method = "GET";
            //    request.ContentType = "text/html;charset=UTF-8";
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    Stream myResponseStream = response.GetResponseStream();
            //    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            //    retString += myStreamReader.ReadToEnd();
            //    myStreamReader.Close();
            //    myResponseStream.Close();
            //}

            string path = Server.MapPath("/XmlFiles/APPManagement.xml");
            TB_APPManagement model = getAPPManagement("241ea176-fce7-4bd7-a65f-a7978aac1cd2", path);
            ViewData["TB_APPManagement"] = model;
            Response.Write(model);
            BaseManagement bm = new BaseManagement();
            var iList = bm.Search<TB_APPManagement>("");

            //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, @"select * from TB_APPManagement --where ID='241ea176-fce7-4bd7-a65f-a7978aac1cd2'").Tables[0];
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    bool flag_xml = XmlHelper.WriteToXml(dt, path, "APPManagements", "APPManagement");
            //    //读取
            //    XmlHelper xmlHelper = new XmlHelper(path);
            //    DataTable dt_xml = xmlHelper.GetDataSetByXml(path).Tables[0];
            //    Response.Write(dt_xml);
            //}
            return View(model);
        }
        /// <summary>
        /// 根据ID和xml路径得到TB_APPManagement
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public TB_APPManagement getAPPManagement(string ID, string path)
        {
            try
            {
                TB_APPManagement model = null;
                bool flag = false;
                //读取
                XmlHelper xmlHelper = new XmlHelper(path);
                DataTable dt_xml = xmlHelper.GetDataSetByXml(path) == null ? null : xmlHelper.GetDataSetByXml(path).Tables[0];
                if (dt_xml != null && dt_xml.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_xml.Rows)
                    {
                        if (row["ID"].Equals(ID))
                        {
                            model = new TB_APPManagement()
                            {
                                ID = row["ID"].ToString(),
                                VersionName = row["VersionName"].ToString(),
                                VersionID = Convert.ToInt32(string.IsNullOrEmpty(row["VersionID"].ToString()) ? 0 : row["VersionID"]),
                                CreatePerson = row["CreatePerson"].ToString(),
                                CreateDate = DateTime.Parse(row["CreateDate"].ToString())
                            };
                            flag = true;
                            break;
                        }
                    }
                }

                if (flag == false)
                {
                    DataTable dt = null;
                    dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, @"select * from TB_APPManagement").Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["ID"].Equals(ID))
                            {
                                model = new TB_APPManagement()
                                {
                                    ID = row["ID"].ToString(),
                                    VersionName = row["VersionName"].ToString(),
                                    VersionID = Convert.ToInt32(string.IsNullOrEmpty(row["VersionID"].ToString()) ? 0 : row["VersionID"]),
                                    CreatePerson = row["CreatePerson"].ToString(),
                                    CreateDate = DateTime.Parse(row["CreateDate"].ToString())
                                };
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag == true)
                    {
                        bool flag_xml = XmlHelper.WriteToXml(dt, path, "APPManagements", "APPManagement");
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }
}
