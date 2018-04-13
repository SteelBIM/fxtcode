using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Model;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class RankingInformationController : ApiController
    {

        /// <summary>
        /// 获取已发布视频排行信息
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
//        public HttpResponseMessage SendShortMessages([FromBody]KingRequest request)
//        {
//            TB_UserVideoDetails submitData = JsonHelper.DecodeJson<TB_UserVideoDetails>(request.Data);

//            if (submitData.VersionID <= 0)
//            {
//                return GetErrorResult("版本ID不能小于0！");
//            }
//            if (string.IsNullOrEmpty(submitData.VideoID.ToString()))
//            {
//                return GetErrorResult("视频ID不能为空！");
//            }

//            string sql = string.Format(@"  SELECT   a.CreateTime ,
//                                                    a.NumberOfOraise ,
//                                                    a.TotalScore ,
//                                                    b.UserName ,
//                                                    b.UserImage
//                                          FROM      [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
//                                                    LEFT JOIN dbo.Tb_UserInfo b ON b.UserID = a.UserID
//                                          WHERE     a.VideoID = '{0}'  AND b.IsUser=1
//                                                    AND a.VersionID = '{1}'", submitData.VideoID, submitData.VersionID);

//            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

//            List<object> list = new List<object>();
//            if (ds.Tables[0].Rows.Count > 0 && null != ds.Tables[0])
//            {
//                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
//                {
//                    object objects = new
//                    {
//                        CreateTime = ds.Tables[0].Rows[i]["CreateTime"],
//                        NumberOfOraise = ds.Tables[0].Rows[i]["NumberOfOraise"],
//                        TotalScore =  ds.Tables[0].Rows[i]["TotalScore"],
//                        UserName = ds.Tables[0].Rows[i]["UserName"],
//                        UserImage = ds.Tables[0].Rows[i]["UserImage"]
//                    };
//                    list.Add(objects);
//                }
//                return GetResult(list);
//            }
//            else
//            {
//                return GetErrorResult("没有对应的数据！");
//            }

//        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage test()
        {
            TB_UserVideoDetails submitData = new TB_UserVideoDetails(); //JsonHelper.DecodeJson<TB_UserVideoDetails>(request.data);
            submitData.VersionID = 1;
            submitData.VideoID = 7750;
            if (submitData.VersionID <= 0)
            {
                return GetErrorResult("版本ID不能小于0！");
            }
            if (string.IsNullOrEmpty(submitData.VideoID.ToString()))
            {
                return GetErrorResult("视频ID不能为空！");
            }

            string sql = string.Format(@"  SELECT   a.CreateTime ,
                                                    a.NumberOfOraise ,
                                                    a.TotalScore ,
                                                    b.UserName ,
                                                    b.UserImage
                                          FROM      [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                                    LEFT JOIN ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo b ON b.UserID = a.UserID
                                          WHERE     a.VideoID = '{0}' AND b.IsUser=1
                                                    AND a.VersionID = '{1}'", submitData.VideoID, submitData.VersionID);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            List<object> list = new List<object>();
            if (ds.Tables[0].Rows.Count > 0 && null != ds.Tables[0])
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    object objects = new
                    {
                        CreateTime = ds.Tables[0].Rows[i]["CreateTime"],
                        NumberOfOraise = ds.Tables[0].Rows[i]["NumberOfOraise"],
                        TotalScore = ds.Tables[0].Rows[i]["TotalScore"],
                        UserName = ds.Tables[0].Rows[i]["UserName"],
                        UserImage = ds.Tables[0].Rows[i]["UserImage"]
                    };
                    list.Add(objects);
                }
                return GetResult(list);
            }
            else
            {
                return GetErrorResult("没有对应的数据！");
            }

        }


        private HttpResponseMessage GetErrorResult(string message)
        {
            object obj = new { Success = false, data = "", message = message };
            return KingsunResponse.toJson(obj);
        }
        private HttpResponseMessage GetResult(object Data, string message = "")
        {
            object obj = new { Success = true, data = Data, message = message };
            return KingsunResponse.toJson(obj);
        }
    }

}
