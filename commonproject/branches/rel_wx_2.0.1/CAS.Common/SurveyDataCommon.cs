using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using CAS.Entity.SurveyDBEntity;
using CAS.Entity.SurveyEntityNew;

namespace CAS.Common
{
    /// <summary>
    /// 查勘调取API获取数据通用方法 caoq 2013-12-3
    /// </summary>
    public class SurveyDataCommon
    {
        #region 查勘相关
        /// <summary>
        /// 获取查勘数据
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="bankid">银行查询数据时需要传递</param>
        /// <param name="sid">查勘ID</param>
        /// <param name="entrustid">委托ID</param>
        /// <param name="objectid">委估对象ID</param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <param name="statecode"></param>
        /// <returns></returns>
        public static List<SurveyExt> FxtSurvey_GetSurvey(LoginInfoEntity logininfo, int fxtcompanyid, int bankid, long sid, long entrustid, long objectid, int systypecode, out string msg, int statecode = 0)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&bankid={9}&userid={2}&username={3}&password={4}&systypecode={5}&sid={6}&entrustid={7}&objectid={8}&statecode={10}"
                    , (sid > 0 ? "surveyinfo" : "getinfobyobj"), fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, sid, entrustid, objectid, bankid, statecode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        if (sid > 0)
                        {
                            return new List<SurveyExt>() { (JSONHelper.JSONToObject<SurveyExt>(JSONHelper.ObjectToJSON(rtn.data))) };
                        }
                        else
                        {
                            return JSONHelper.JSONStringToList<SurveyExt>(JSONHelper.ObjectToJSON(rtn.data));
                        }
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取查勘图片数据
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="sid"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<DatFiles> FxtSurvey_GetSurveyFile(LoginInfoEntity logininfo, int fxtcompanyid, long sid, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            List<DatFiles> list = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/datfiles.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&sid={6}"
                    , "list", fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, sid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONStringToList<DatFiles>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取查勘跟进
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="sid"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<DatFollowUp> FxtSurvey_GetSurveyFollow(LoginInfoEntity logininfo, long sid, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            List<DatFollowUp> list = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_follow.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&sid={6}"
                    , "list", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, sid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONStringToList<DatFollowUp>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return list;
        }
        #endregion

        #region 查勘模版相关
        /// <summary>
        /// 获取查勘模版列表
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<SYSSurveyTemplateExt> FxtSurvey_GetSurveyTemplateList(LoginInfoEntity logininfo, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            List<SYSSurveyTemplateExt> list = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_template_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}"
                    , "list", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONStringToList<SYSSurveyTemplateExt>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取查勘模版详情
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="btsid"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SYSBusinessTableSetup FxtSurvey_GetSurveyTemplate(LoginInfoEntity logininfo, int btsid, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            SYSBusinessTableSetup model = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_template_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&btsid={6}"
                    , "details", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, btsid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONToObject<SYSBusinessTableSetup>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 修改查勘模版详情
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="btsid"></param>
        /// <param name="fieldscontent"></param>
        /// <param name="excelpath"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_UpdateSurveyTemplate(LoginInfoEntity logininfo, int btsid, string fieldscontent, string excelpath, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_template_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&btsid={6}&fieldscontent={7}&excelpath={8}"
                    , "update", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, btsid, fieldscontent, excelpath), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                    /*
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.GetJson(null, 1, "删除成功", null);
                    }
                    else
                    {
                        msg = "-1";
                    }*/
                }
            }
            return null;
        }

        /// <summary>
        /// 获取查勘模版字段
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="typecode">类型</param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<SYSBusinessTableFieldsSetup> FxtSurvey_GetSurveyFieldsList(LoginInfoEntity logininfo, int typecode, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            List<SYSBusinessTableFieldsSetup> list = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_template_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&typecode={6}"
                    , "fieldlist", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, typecode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONStringToList<SYSBusinessTableFieldsSetup>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取查勘模版字段详情
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="typecode">业务类型</param>
        /// <param name="btfsid"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SYSBusinessTableFieldsSetup FxtSurvey_GetSurveyFields(LoginInfoEntity logininfo, int typecode, int btfsid, int systypecode, out string msg)
        {

            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            SYSBusinessTableFieldsSetup model = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_template_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&typecode={6}&btfsid={7}"
                    , "fielddetails", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, typecode, btfsid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONToObject<SYSBusinessTableFieldsSetup>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 新增/修改查勘模版字段
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="typecode">业务类型</param>
        /// <param name="btfsid"></param>
        /// <param name="systypecode"></param>
        /// <param name="unit"></param>
        /// <param name="showname"></param>
        /// <param name="fieldvalues"></param>
        /// <param name="fieldtype"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_AddSurveyFields(LoginInfoEntity logininfo, int typecode, int btfsid, string unit, string showname, string fieldvalues, int fieldtype, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_template_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&typecode={6}&btfsid={7}&unit={8}&showname={9}&fieldvalues={10}&fieldtype={11}"
                    , "addfield", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, typecode, btfsid, unit, showname, fieldvalues, fieldtype), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return msg;
        }

        /// <summary>
        /// 删除查勘模版字段
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="btfsid"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_DeleteSurveyFields(LoginInfoEntity logininfo, int btfsid, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/survey_template_info.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&btfsid={6}"
                    , "deletefield", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, btfsid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return msg;
        }
        #endregion

        #region 查勘修改
        /// <summary>
        /// 修改查勘模版详情
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="sid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_SurveyMarkMap(LoginInfoEntity logininfo, long sid, double x, double y, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/surveyupdate.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&sid={6}&x={7}&y={8}"
                    , "markingmap", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, sid, x, y), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return null;
        }

        /// <summary>
        /// 新增查勘图片
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="sid"></param>
        /// <param name="filesize"></param>
        /// <param name="photopath"></param>
        /// <param name="imgtype"></param>
        /// <param name="surveytypecode"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_SurveyAddFile(LoginInfoEntity logininfo, long sid, string filesize, string photopath, string imgtype, string surveytypecode, string remark, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/datfiles.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&sid={6}&filesize={7}&photopath={8}&imgtype={9}&surveytypecode={10}&remark={11}"
                    , "add", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, sid, filesize, photopath, imgtype, surveytypecode, remark), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return null;
        }

        /// <summary>
        /// 删除查勘图片
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="sid"></param>
        /// <param name="ids"></param>
        /// <param name="photopath"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_SurveyDeleteFile(LoginInfoEntity logininfo, long sid, string ids, string photopath, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/datfiles.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&ids={6}&photopath={7}&sid={8}"
                    , "delete", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, ids, photopath, sid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return null;
        }

        /// <summary>
        /// 下载查勘图片
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="sid"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_SurveyDownloadFile(LoginInfoEntity logininfo, long sid, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/datfiles.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&sid={6}"
                    , "download", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, sid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return null;
        }

        #endregion

        #region 查勘照片模版相关
        /// <summary>
        /// 获取查勘照片模版列表
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="companyid">公司ID(银行的数据传递查勘所属银行ID(forcompanyid))</param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<SurveyImageTemplateExt> FxtSurvey_GetSurveyPhotoTemplateList(LoginInfoEntity logininfo, int companyid, int cityid, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            List<SurveyImageTemplateExt> list = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/phototemplates.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&cityid={6}"
                    , "list", companyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, cityid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONStringToList<SurveyImageTemplateExt>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取查勘照片模版详情
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="id"></param>
        /// <param name="systypecode"></param>
        /// <param name="containdetail">是否包含每页类型</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SurveyImageTemplateExt FxtSurvey_GetSurveyPhotoTemplate(LoginInfoEntity logininfo, int id, bool containdetail, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/phototemplates.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&id={6}"
                    , (containdetail ? "detailsall" : "details"), logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, id), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        return JSONHelper.JSONToObject<SurveyImageTemplateExt>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 删除查勘照片模版
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="btfsid"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_DeleteSurveyPhotoTemplate(LoginInfoEntity logininfo, int btfsid, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/phototemplates.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&btfsid={6}"
                    , "delete", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, btfsid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return msg;
        }

        /// <summary>
        /// 更新查勘照片模版
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="surveytype"></param>
        /// <param name="name"></param>
        /// <param name="pagecount"></param>
        /// <param name="pagecontent"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtSurvey_ModifyPhotoTemplate(LoginInfoEntity logininfo, int surveytype, string name, int pagecount, string pagecontent, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/phototemplates.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&surveytype={6}&name={7}&pagecount={8}&pagecontent={9}"
                    , "modify", logininfo.fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, surveytype, name, pagecount, pagecontent), true);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return msg;
        }
        #endregion

        #region 字典相关
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="logininfo"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="id">类型</param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<SYSCode> FxtSurvey_GetCode(LoginInfoEntity logininfo, int fxtcompanyid, int id, int systypecode, out string msg)
        {
            //web.config设置调度中心API的地址
            string api = WebCommon.FxtSurveyCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/code.ashx";
                string str = WebCommon.APIPostBack(url, string.Format("type={0}&fxtcompanyid={1}&userid={2}&username={3}&password={4}&systypecode={5}&id={6}"
                    , "list", fxtcompanyid, logininfo.id, logininfo.username, logininfo.userpwd, systypecode, id), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        return JSONHelper.JSONStringToList<SYSCode>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return null;
        }
        #endregion

    }
}
