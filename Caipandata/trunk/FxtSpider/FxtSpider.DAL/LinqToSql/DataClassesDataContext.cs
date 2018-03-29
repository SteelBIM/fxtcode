using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace FxtSpider.DAL.LinqToSql
{
    public partial class VIEW_案例信息_城市表_网站表
    {
        
    }
    public partial class 案例信息
    {
        public string 用途 { get; set; }
        public string 案例类型 { get; set; }
        public string 结构 { get; set; }
        public string 建筑类型 { get; set; }
        public string 户型 { get; set; }
        public string 朝向 { get; set; }
        public string 币种 { get; set; }


        public string 面积String { get; set; }
        public string 单价String { get; set; }
        public string 总价String { get; set; }
        public string 所在楼层String { get; set; }
        public string 总楼层String { get; set; }
        public string 花园面积String { get; set; }
        public string 车位数量String { get; set; }
        public string 地下室面积String { get; set; }
    }
    public partial class DataClassesDataContext
    {
        public DataClassesDataContext() :
            base(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)
        {
            object commandTimeoutValue = ConfigurationManager.AppSettings["SqlCommandTimeout"];
            if (commandTimeoutValue != null)
            {
                if (Regex.IsMatch(Convert.ToString(commandTimeoutValue), @"^\d*$"))
                {
                    this.CommandTimeout = Convert.ToInt32(commandTimeoutValue);
                }
            }
            
            OnCreated();
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "案例信息_获取爬取数据_根据城市ID_创建时间区间")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> 案例信息_获取爬取数据_根据城市ID_创建时间区间(int 城市ID, DateTime 创建日期_开始时间, DateTime 创建日期_结束时间)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 创建日期_开始时间, 创建日期_结束时间);
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "get_案例信息_获取爬取数据_根据城市ID_创建时间区间_分页")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> get_案例信息_获取爬取数据_根据城市ID_创建时间区间_分页(int 城市ID, DateTime 创建日期_开始时间, DateTime 创建日期_结束时间, int startIndex, int endIndex)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 创建日期_开始时间, 创建日期_结束时间, startIndex,  endIndex);
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }        
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.get_案例信息_获取爬取数据_根据城市ID_创建时间区间_分页2")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> get_案例信息_获取爬取数据_根据城市ID_创建时间区间_分页2(int 城市ID,DateTime 创建日期_开始时间, DateTime 创建日期_结束时间, int startIndex, int endIndex, out int count)
        {
            count = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 创建日期_开始时间, 创建日期_结束时间, startIndex, endIndex, count);
            count = (Convert.ToInt32(result.GetParameterValue(5)));
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.案例信息_获取爬取数据_根据城市ID_网站_创建时间区间")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> 案例信息_获取爬取数据_根据城市ID_网站_创建时间区间( int 城市ID, int 网站ID,DateTime 创建日期_开始时间,DateTime 创建日期_结束时间)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 网站ID,创建日期_开始时间, 创建日期_结束时间);
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.get_案例信息_获取爬取数据_根据城市ID_网站_创建时间区间_分页")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> get_案例信息_获取爬取数据_根据城市ID_网站_创建时间区间_分页( int 城市ID, int 网站ID,  DateTime 创建日期_开始时间,DateTime 创建日期_结束时间, int startIndex, int endIndex)
        {

            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 网站ID, 创建日期_开始时间, 创建日期_结束时间, startIndex, endIndex);
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.get_案例信息_获取爬取数据_根据城市ID_网站_创建时间区间_分页2")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> get_案例信息_获取爬取数据_根据城市ID_网站_创建时间区间_分页2(int 城市ID, int 网站ID, DateTime 创建日期_开始时间, DateTime 创建日期_结束时间, int startIndex, int endIndex, out int count)
        {
            count = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 网站ID, 创建日期_开始时间, 创建日期_结束时间, startIndex, endIndex, count);
            count = (Convert.ToInt32(result.GetParameterValue(6)));
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.get_案例信息_获取爬取数据_根据多条件")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> get_案例信息_获取爬取数据_根据多条件(int 城市ID, int 网站ID, DateTime 创建日期_开始时间, DateTime 创建日期_结束时间, int startIndex, int endIndex)
        {

            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 网站ID, 创建日期_开始时间, 创建日期_结束时间, startIndex, endIndex);
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.get_案例信息_获取爬取数据_根据多条件_getCount")]
        public ISingleResult<VIEW_案例信息_城市表_网站表> get_案例信息_获取爬取数据_根据多条件_getCount(int 城市ID, int 网站ID, DateTime 创建日期_开始时间, DateTime 创建日期_结束时间, int startIndex, int endIndex, out int count)
        {
            count = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), 城市ID, 网站ID, 创建日期_开始时间, 创建日期_结束时间, startIndex, endIndex, count);
            count = (Convert.ToInt32(result.GetParameterValue(6)));
            return ((ISingleResult<VIEW_案例信息_城市表_网站表>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.案例信息_DeleteByIds")]
        public int 案例信息_DeleteByIds(string Ids)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), Ids);
            return Convert.ToInt32(result.ReturnValue);
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.案例库上传信息过滤表_DeleteByCaseIds")]
        public int 案例库上传信息过滤表_DeleteByCaseIds(string CaseIds)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), CaseIds);
            return Convert.ToInt32(result.ReturnValue);
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_Company_Insert")]
        public int SysData_Company_Insert(string companyName, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), companyName, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(1));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_CompanyArea_Insert")]
        public int SysData_CompanyArea_Insert(string companyAreaName, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), companyAreaName, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(1));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_Project_Insert")]
        public int SysData_Project_Insert(string projectName,int cityId,int webId, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), projectName,cityId,webId, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(3));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_Area_Insert")]
        public int SysData_Area_Insert(string areaName, int cityId, int webId, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), areaName, cityId, webId, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(3));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_SubArea_Insert")]
        public int SysData_SubArea_Insert(string subAreaName, int cityId, int webId, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), subAreaName, cityId, webId, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(3));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_装修_Insert")]
        public int SysData_装修_Insert(string zxName, out int nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), zxName, nowID);
            nowID = Convert.ToInt32(result.GetParameterValue(1));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.Dat_SpiderRepetitionLog_Insert")]
        public int Dat_SpiderRepetitionLog_Insert(int webId,int cityId,long repetitionCount,string date,DateTime updateTime,out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), webId,cityId,repetitionCount,date,updateTime, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(5));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.Dat_SpiderRepetitionLog_Update")]
        public int Dat_SpiderRepetitionLog_Update(int webId, int cityId, long repetitionCount, string date, DateTime updateTime, long nowID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), webId, cityId, repetitionCount, date, updateTime, nowID);
            return Convert.ToInt32(result.ReturnValue);
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.Dat_KeyValueConfig_Insert")]
        public int Dat_KeyValueConfig_Insert(string keyName, string keyValue, int webId, int cityId, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),keyName,keyValue, webId, cityId,  nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(4));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.Dat_KeyValueConfig_Update")]
        public int Dat_KeyValueConfig_Update(string keyName, string keyValue, int webId, int cityId,long nowID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), keyName, keyValue, webId, cityId, nowID);
            return Convert.ToInt32(result.ReturnValue);
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.Dat_SpiderErrorLog_Insert")]
        public int Dat_SpiderErrorLog_Insert(int webId, int cityId,string url,int errorTypeCode ,DateTime createTime ,string remark , out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), webId, cityId,url,errorTypeCode,createTime,remark, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(6));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.Dat_SpiderErrorLog_Update")]
        public int Dat_SpiderErrorLog_Update(int webId, int cityId, string url, int errorTypeCode, DateTime createTime, string remark, long nowID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), webId, cityId, url, errorTypeCode, createTime, remark, nowID);
            return Convert.ToInt32(result.ReturnValue);
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_ProxyIp_Insert")]
        public int SysData_ProxyIp_Insert(string ip, DateTime createTime, string ipArea,  out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), ip, createTime, ipArea,  nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(3));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_WebJoinProxyIp_Insert")]
        public int SysData_WebJoinProxyIp_Insert(int webId, long proxyIp, int status,DateTime createTime, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), webId, proxyIp, status,createTime, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(4));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SysData_WebJoinProxyIp_Update")]
        public int SysData_WebJoinProxyIp_Update(int webId, long proxyIp, int status, DateTime createTime, long nowID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), webId, proxyIp, status, createTime, nowID);
            return Convert.ToInt32(result.ReturnValue);
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.案例信息_Insert")]
        public int 案例信息_Insert(string 楼盘名, DateTime? 案例时间, string 行政区, string 片区, string 楼栋, string 房号,
            decimal? 面积, decimal? 单价, decimal? 总价, int? 所在楼层, int? 总楼层, string 装修, string 建筑年代,
            string 信息, string 电话, string url, string 地址, DateTime? 创建时间, string 来源, string 建筑形式, decimal? 花园面积,
            string 厅结构, int? 车位数量, string 配套设施, decimal? 地下室面积, int 城市id, int? 网站id, int? 案例类型id,
            int? 币种id, int? 朝向id, int? 户型id, int? 建筑类型id, int? 结构id, int? 用途id, int? 装修id, int 是否已进行入库整理,
            DateTime? 进行入库整理时间, int? fxtid, long? companyid, long? companyareaid, long? projectid, long? areaid,
            long? subareaid, out long nowID)
        {
            nowID = 0;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                楼盘名, 案例时间, 行政区, 片区, 楼栋, 房号, 面积, 单价, 总价, 所在楼层, 总楼层,
                 装修, 建筑年代, 信息, 电话, url, 地址, 创建时间, 来源, 建筑形式, 花园面积, 厅结构, 车位数量, 配套设施,
                地下室面积, 城市id, 网站id, 案例类型id, 币种id, 朝向id, 户型id, 建筑类型id, 结构id, 用途id, 装修id,
                是否已进行入库整理, 进行入库整理时间, fxtid, companyid, companyareaid, projectid, areaid, subareaid, nowID);
            nowID = Convert.ToInt64(result.GetParameterValue(43));
            if (nowID > 0)
            {
                return 1;
            }
            return 0;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.案例信息_Update")]
        public int 案例信息_Update(string 楼盘名, DateTime? 案例时间, string 行政区, string 片区, string 楼栋, string 房号,
            decimal? 面积, decimal? 单价, decimal? 总价, int? 所在楼层, int? 总楼层, string 装修, string 建筑年代,
            string 信息, string 电话, string url, string 地址, DateTime? 创建时间, string 来源, string 建筑形式, decimal? 花园面积,
            string 厅结构, int? 车位数量, string 配套设施, decimal? 地下室面积, int 城市id, int? 网站id, int? 案例类型id,
            int? 币种id, int? 朝向id, int? 户型id, int? 建筑类型id, int? 结构id, int? 用途id, int? 装修id, int 是否已进行入库整理,
            DateTime? 进行入库整理时间, int? fxtid, long? companyid, long? companyareaid, long? projectid, long? areaid,
            long? subareaid, long nowID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                楼盘名, 案例时间, 行政区, 片区, 楼栋, 房号, 面积, 单价, 总价, 所在楼层, 总楼层,
                 装修, 建筑年代, 信息, 电话, url, 地址, 创建时间, 来源, 建筑形式, 花园面积, 厅结构, 车位数量, 配套设施,
                地下室面积, 城市id, 网站id, 案例类型id, 币种id, 朝向id, 户型id, 建筑类型id, 结构id, 用途id, 装修id,
                是否已进行入库整理, 进行入库整理时间, fxtid, companyid, companyareaid, projectid, areaid, subareaid, nowID);
            return Convert.ToInt32(result.ReturnValue);
        }
    }
}
