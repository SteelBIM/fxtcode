using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common.Model;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.App.Common
{
    public class ObjectToJson
    {
        public static HttpResponseMessage GetErrorResult(string message)
        {
            object obj = new { Success = false, data = "", Message = message, SystemTime = DateTime.Now };
            return KingsunResponse.toJson(obj);
        }

        public static HttpResponseMessage GetResult(object Data, string message = "")
        {
            object obj = new { Success = true, data = Data, Message = message, SystemTime = DateTime.Now };
            return KingsunResponse.toJson(obj);
        }

        //public static HttpResponseMessage GetResult(object data, int num = 0, string message = "")
        //{
        //    object obj = new { Success = true, data = data, Num = num, Message = message };
        //    return KingsunResponse.toJson(obj);
        //}

        public static HttpResponseMessage GetJsonResult(string result = "")
        {
            return KingsunResponse.toJson(result);
        }

        /// <summary>
        /// 获取TB_ModuleConfiguration 的结构
        /// </summary>
        /// <param name="listconfig"></param>
        /// <returns></returns>
        public static HttpResponseMessage ListToJson(List<TB_ModuleConfiguration> listconfig)
        {
            List<object> listResult = new List<object>();
            //把bookid和FirstTitileID重复的去除
            List<TB_ModuleConfiguration> listconfigs = listconfig.Where((x, i) => listconfig.FindIndex(z => z.BookID == x.BookID && z.FirstTitileID == x.FirstTitileID && z.State == 0) == i).ToList();
            List<object> configlist = new List<object>();
            string mStart = "";
            string mEnd = "";
            foreach (TB_ModuleConfiguration configes in listconfigs)
            {
                List<object> listobject = new List<object>();
                foreach (TB_ModuleConfiguration configobj in listconfig)
                {
                    if (configes.BookID == configobj.BookID && configes.FirstTitileID == configobj.FirstTitileID)
                    {
                        mStart += configobj.StartingPage + ",";
                        mEnd += configobj.EndingPage + ",";
                        if (configes.SecondTitleID == null)
                        {
                            //listobject.Add(null);
                            //listobject.Add("");
                        }
                        else
                        {
                            object objects = new
                            {
                                SecondTitleID = configobj.SecondTitleID,
                                SecondTitle = configobj.SecondTitle,
                                StartingPage = configobj.StartingPage,
                                EndingPage = configobj.EndingPage,
                            };
                            listobject.Add(objects);
                        }
                    }

                }
                if (listobject == null)
                {
                    object objs = new
                    {
                        BookID = configes.BookID,
                        TeachingNaterialName = configes.TeachingNaterialName,
                        FirstTitileID = configes.FirstTitileID,
                        FirstTitle = configes.FirstTitle,
                        configlist = ""
                    };
                    configlist.Add(objs);
                }
                else
                {
                    string[] start = mStart.Trim(',').Split(',');
                    string[] end = mEnd.Trim(',').Split(',');
                    object objs = new
                    {
                        BookID = configes.BookID,
                        TeachingNaterialName = configes.TeachingNaterialName,
                        FirstTitileID = configes.FirstTitileID,
                        FirstTitle = configes.FirstTitle,
                        StartingPage = Convert.ToInt32(start[0]),
                        EndingPage = Convert.ToInt32(end[end.Length - 1]),
                        configlist = listobject
                    };
                    configlist.Add(objs);
                }
                mStart = "";
                mEnd = "";
            }
            return GetResult(configlist);
        }




    }
}