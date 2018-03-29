using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtSpider.DAL.DB;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Common;
using log4net;

namespace FxtSpider.Bll
{
    public static class WebJoinProxyIpManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(WebJoinProxyIpManager));
        /// <summary>
        /// 获取网站代理IP列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <param name="count"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static List<View_WebJoinProxyIp> GetAllViewWebJoinProxyIp(int start, int pageSize, bool isGetCount, out int count,DataClass _dc=null)
        {
            count = 0;
            DataClass dc=new DataClass (_dc);
            var query = dc.DB.View_WebJoinProxyIp;
            if (isGetCount)
            {
                count = query.Count();
            }
            List<View_WebJoinProxyIp> list = query.OrderByDescending(tbl => tbl.ID).Skip(pageSize * (start - 1)).Take(pageSize)
                    .ToList<View_WebJoinProxyIp>();
            dc.Connection_Close();
            dc.Dispose();
            return list;
        }
        public static List<View_WebJoinProxyIp> GetViewWebJoinProxyIp(long[] ids, DataClass _dc = null)
        {
            if(ids==null||ids.Length<1)
            {
                return new List<View_WebJoinProxyIp> ();
            }
            DataClass dc = new DataClass(_dc);
            List<View_WebJoinProxyIp> list = dc.DB.View_WebJoinProxyIp.Where(tbl => ids.Contains(tbl.ID)).ToList<View_WebJoinProxyIp>();
            dc.Connection_Close();
            dc.Dispose();
            return list; 
        }
        /// <summary>
        /// 给网站插入代理IP(如果ip在ip表里存在则读取后插入到网站ip表,否则新增ip)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ipArea">ip所在地区,比如:广东省广州市电信</param>
        /// <param name="webIds"></param>
        /// <param name="addList">新增成功的网站代理ip信息</param>
        /// <param name="message"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static int InsertWebJoinProxyIp(string ip, string ipArea, int[] webIds, out List<SysData_WebJoinProxyIp> addList, out string message, DataClass _dc = null)
        {
            message = "";
            addList = new List<SysData_WebJoinProxyIp>();
            if (string.IsNullOrEmpty(ip) || webIds == null || webIds.Length < 1)
            {
                message = "请输入ip和网站";
                return 0;
            }
            DataClass dc = new DataClass(_dc);
            try
            {
                SysData_ProxyIp ipObj = null;
                int result = ProxyIpManager.InsertProxyIp(ip, ipArea,out ipObj, out message, _dc : dc);
                if (ipObj == null)
                {
                    dc.Connection_Close();
                    dc.Dispose();
                    message = "ip不能为空";
                    return 0;
                }
                List<SysData_WebJoinProxyIp> list = dc.DB.SysData_WebJoinProxyIp.Where(tbl => tbl.ProxyIp == ipObj.ID && webIds.Contains(tbl.WebId)).ToList();
                if (list.Count == webIds.Length)
                {
                    dc.Connection_Close();
                    dc.Dispose();
                    message = "此网站代理IP已存在";
                    return 0;
                }
                foreach (int webId in webIds)
                {
                    if (list.Where(p => p.WebId == webId).FirstOrDefault() == null)
                    {
                        SysData_WebJoinProxyIp wipObj = new SysData_WebJoinProxyIp();
                        wipObj.ProxyIp = ipObj.ID;
                        wipObj.WebId = webId;
                        wipObj.Status = ProxyIpManager.WebProxyIpStatus1;
                        wipObj.CreateTime = DateTime.Now;
                        addList.Add(wipObj);
                    }
                }
                if (addList != null && addList.Count > 0)
                {
                    dc.DB.SysData_WebJoinProxyIp.InsertAllOnSubmit<SysData_WebJoinProxyIp>(addList);
                    dc.DB.SubmitChanges();
                }
                dc.Connection_Close();
                dc.Dispose();
            }
            catch (Exception ex)
            {
                dc.Connection_Close();
                dc.Dispose();
                message = "系统异常";
                log.Error(string.Format("(插入网站代理ip失败)InsertWebJoinProxyIp(string ip={0}, int[] webIds={1},out string message, DataClass _dc = null)"
                    , ip == null ? "null" : null, webIds == null ? 0 : webIds.Length),
                   ex
                   );
                return 0;
            }
            return 1;
        }

        public static SysData_WebJoinProxyIp Insert(SysData_WebJoinProxyIp obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.SysData_WebJoinProxyIp_Insert(obj.WebId,obj.ProxyIp,obj.Status,obj.CreateTime, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }

        public static SysData_WebJoinProxyIp Update(SysData_WebJoinProxyIp obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                db.DB.SysData_WebJoinProxyIp_Update(obj.WebId, obj.ProxyIp, obj.Status, obj.CreateTime, obj.ID);
            }
            return obj;
        }
    }
}
