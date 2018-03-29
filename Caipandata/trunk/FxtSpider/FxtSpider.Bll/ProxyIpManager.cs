using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtSpider.DAL.DB;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Common;

namespace FxtSpider.Bll
{
    public static class ProxyIpManager
    {
        /// <summary>
        /// 网站代理ip状态:可用
        /// </summary>
        public const int WebProxyIpStatus1 = 1;
        /// <summary>
        /// 网站代理ip状态:不可用
        /// </summary>
        public const int WebProxyIpStatus2 = 0;
        #region (查询)

        /// <summary>
        /// 根据网站获取可用的代理IP
        /// </summary>
        /// <param name="webId"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static string GetEffectiveProxyIp(int webId,DataClass _dc=null)
        {
            //return null;
            string nowIp = null;
            DataClass dc = new DataClass(_dc);
            //View_WebJoinProxyIp obj = dc.DB.ExecuteQuery<View_WebJoinProxyIp>(string.Format("select top 1 * from View_WebJoinProxyIp where WebId={0} and Status={1} order by newId()",webId,WebProxyIpStatus1)).FirstOrDefault();// dc.DB.View_WebJoinProxyIp.Where(tbl => tbl.WebId == webId && tbl.Status == WebProxyIpStatus1).FirstOrDefault();            
            //SysData_ProxyIp obj = dc.DB.ExecuteQuery<SysData_ProxyIp>("select top 1 * from SysData_ProxyIp  order by newId()").FirstOrDefault();// dc.DB.View_WebJoinProxyIp.Where(tbl => tbl.WebId == webId && tbl.Status == WebProxyIpStatus1).FirstOrDefault();            
            string sql = new StringBuilder().Append("select top 1 * from SysData_ProxyIp as tbl1 with(nolock) where  ")
                                          .Append("not exists (select * from SysData_WebJoinProxyIp with(nolock) where tbl1.ID=ProxyIp and WebId=").Append(webId)
                                          .Append(" and [Status]=").Append(WebProxyIpStatus2)
                                          .Append(") order by newId()").ToString();
            SysData_ProxyIp obj = dc.DB.ExecuteQuery<SysData_ProxyIp>(sql).FirstOrDefault();
            if (obj != null)
            {
                nowIp = obj.Ip;
            }
            dc.Connection_Close();
            dc.Dispose();
            return nowIp;
        }
        /// <summary>
        /// 获取所有ip
        /// </summary>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static List<SysData_ProxyIp> GetAllProxyIp(DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            List<SysData_ProxyIp> list = dc.DB.SysData_ProxyIp.ToList();
            return list;
        }
        /// <summary>
        /// 根据ip获取ip信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static SysData_ProxyIp GetProxyIpByIp(string ip, DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            string sql = string.Format("select top 1 * from SysData_ProxyIp with(nolock) where Ip='{0}'", ip);
            SysData_ProxyIp existsObj = dc.DB.ExecuteQuery<SysData_ProxyIp>(sql).FirstOrDefault();
            dc.Connection_Close();
            dc.Dispose();
            return existsObj;
        }


        #endregion

        #region (更新)
        /// <summary>
        /// 将当前网站对应的代理ip设置为不可用
        /// </summary>
        /// <param name="webId"></param>
        /// <param name="ip"></param>
        public static void SetNotEffectiveProxyIp(int webId, string ip, DataClass _dc = null)
        {
            if (!string.IsNullOrEmpty(ip))
            {
                DataClass dc = new DataClass(_dc);

                string sql = new StringBuilder().Append("select top 1 * from SysData_WebJoinProxyIp with(nolock) where WebId=")
                    .Append(webId).Append(" and ProxyIp in (select ID from SysData_ProxyIp with(nolock) where Ip='").Append(ip).Append("')").ToString();
                SysData_WebJoinProxyIp obj = dc.DB.ExecuteQuery<SysData_WebJoinProxyIp>(sql).FirstOrDefault();
                if (obj == null)
                {
                    SysData_ProxyIp ipObj = GetProxyIpByIp(ip, dc);
                    if (ipObj != null)
                    {
                        obj = new SysData_WebJoinProxyIp { WebId = webId, ProxyIp = ipObj.ID, CreateTime = DateTime.Now, Status = WebProxyIpStatus2 };
                        obj = WebJoinProxyIpManager.Insert(obj, dc);
                    }
                }
                else
                {
                    obj.Status = WebProxyIpStatus2;
                    WebJoinProxyIpManager.Update(obj, dc);
                }
                //dc.DB.SubmitChanges();
                dc.Connection_Close();
                dc.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ipArea">ip所在地区,比如:广东省广州市电信</param>
        /// <param name="existsObj">已经存在或者插入成功的数据</param>
        /// <param name="checkExists">是否监测数据是否存在</param>
        /// <param name="message"></param>
        /// <param name="_dc"></param>
        /// <returns>1:成功,2:失败-ip已经存在,0:失败-其他失败</returns>
        public static int InsertProxyIp(string ip, string ipArea, out SysData_ProxyIp existsObj, out string message, bool checkExists=true, DataClass _dc = null)
        {
            existsObj = null;
            message = "";
            ip = ip.TrimBlank();
            if (string.IsNullOrEmpty(ip)||string.IsNullOrEmpty(ip.ToLower().Replace("http://","")))
            {
                message = "ip不能为空";
                return 0;
            }
            if (string.IsNullOrEmpty(ip.TrimBlank()))
            {
                message = "ip所在地区不能为空";
                return 0;
            }
            ip = ip.Trim();
            DataClass dc = new DataClass(_dc);
            ip = ip.ToLower().Replace("http://", "");
            //如果需要检测数据是否存在
            if (checkExists)
            {
                existsObj = GetProxyIpByIp(ip, dc);
                if (existsObj != null)
                {
                    dc.Connection_Close();
                    dc.Dispose();
                    message = "ip已存在";
                    return 2;
                }
            }
            SysData_ProxyIp ipObj = new SysData_ProxyIp { CreateTime = DateTime.Now, Ip = ip, IpArea=ipArea };
            ipObj=Insert(ipObj, dc);
            //dc.DB.SysData_ProxyIp.InsertOnSubmit(ipObj);
            //dc.DB.SubmitChanges();
            dc.Connection_Close();
            dc.Dispose();
            existsObj = ipObj;
            return 1;
        }
        /// <summary>
        /// 删除所有无效IP
        /// </summary>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static bool DeleteNotEffectiveProxyIp(DataClass _dc=null)
        {

            DataClass dc = new DataClass(_dc);
            string sql = @"update SysData_ProxyIp with(rowlock) set IpArea='删除' where id in (select ProxyIp from SysData_WebJoinProxyIp)
                        delete SysData_WebJoinProxyIp with(rowlock)
                        delete SysData_ProxyIp with(rowlock) where IpArea='删除'";
            dc.DB.ExecuteCommand(sql);
            dc.Connection_Close();
            return true;

        }


        public static SysData_ProxyIp Insert(SysData_ProxyIp obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.SysData_ProxyIp_Insert(obj.Ip,obj.CreateTime,obj.IpArea, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
        #endregion
    }
}
