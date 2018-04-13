﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Kingsun.ExamPaper.Wechat.Models;

namespace Kingsun.ExamPaper.Wechat.api
{
    /// <summary>
    /// 创建菜单
    /// </summary>
    public class CreateMenu
    {
        JavaScriptSerializer Jss = new JavaScriptSerializer();

        #region 发布菜单
        /// <summary>
        /// 发布菜单
        /// </summary>
        /// <param name="MenuJson">配置的菜单json数据</param>
        /// <param name="AppID">AppID</param>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns>返回0成功否则错误码</returns>
        public string MenuCreate(string MenuJson, string AppID, string AppSecret)
        {
            string setMenuUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            setMenuUrl = string.Format(setMenuUrl, BasicApi.GetTokenSession(AppID, AppSecret));//获取token、拼凑url
            string respText = CommonMethod.WebRequestPostOrGet(setMenuUrl, MenuJson);
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            return respDic["errcode"].ToString();//返回0发布成功
        }
        #endregion
    }
}