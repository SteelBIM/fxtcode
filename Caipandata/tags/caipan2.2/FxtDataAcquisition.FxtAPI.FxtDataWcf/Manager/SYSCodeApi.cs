using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager
{
    public static class SYSCodeApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(SYSCodeApi));
        /// <summary>
        /// 公司类型-开发商
        /// </summary>
        public const int COMPANYTYPECODE_1 = 2001001;
        /// <summary>
        /// 公司类型-物业管理
        /// </summary>
        public const int COMPANYTYPECODE_4 = 2001004;
        /// <summary>
        /// 楼盘主用途-居住(默认)
        /// </summary>
        public const int PROJECT_PURPOSECODE1 = 1001001;

        public static List<SYSCode> GetSYSCodeById(int id, FxtAPIClientExtend _fxtApi = null)
        {
            List<SYSCode> list = new List<SYSCode>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetSYSCodeByID";
                var para = new { id = id };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name,para.ToJSONjss(), _fxtApi: fxtApi));

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<SYSCode>();
                }
                list = JsonHelp.ParseJSONList <SYSCode>(jsonStr);
                list.DecodeField<SYSCode>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetSYSCodeById(int id,FxtAPIClientExtend _fxtApi = null)", ex);
            }
            return list;
        }

    }
}
