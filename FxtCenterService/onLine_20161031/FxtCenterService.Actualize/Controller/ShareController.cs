using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using CAS.Entity;
using FxtCenterService.Logic;
using CAS.Entity.FxtDataCenter;
using Newtonsoft.Json;
using CAS.Common;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        ///  添加
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string AddCompanyShowData(JObject funinfo, UserCheck company)
        {

            var p = (PriviCompanyShowData)JsonConvert.DeserializeObject(funinfo.ToString(), typeof(PriviCompanyShowData));

            return ShareBL.AddCompanyShowData(p).ToString();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string UpdateCompanyShowData(JObject funinfo, UserCheck company)
        {
            
            var p = (PriviCompanyShowData)JsonConvert.DeserializeObject(funinfo.ToString(), typeof(PriviCompanyShowData));

            if (p.typecode != company.producttypecode) return "-1"; //判断产品code是否是自己的产品

            var properties = funinfo.Properties().Select(m=>m.Name.ToString());

            return ShareBL.UpdateCompanyShowData(p, properties).ToString();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCompanyShowData(JObject funinfo, UserCheck company, JObject objSinfo, JObject objInfo)
        {
            var p = (PriviCompanyShowData)JsonConvert.DeserializeObject(funinfo.ToString(), typeof(PriviCompanyShowData));

            return ShareBL.GetCompanyShowData( p, objSinfo, objInfo).ToJson();
        }
    }
}
