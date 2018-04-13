using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.DB;
using System.Web;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;


namespace Kingsun.SynchronousStudy.BLL
{
    public class ScoreManagement : BaseManagement
    {
        /// <summary>
        /// 新增课程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse AddScore(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            //TbScore Score = JsonHelper.DecodeJson<TbScore>(request.Data);

            TbScore submitData = JsonHelper.DecodeJson<TbScore>(request.Data);

            #region 校验相应的数据有效性

            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.Userid))
            {
                return KingResponse.GetErrorResponse("参数错误,无用户信息", request);
            }
            if (submitData.Articleid == 0)
            {
                return KingResponse.GetErrorResponse("参数错误,无题目信息", request);
            }
            #endregion


            bool result = true;
            try
            {
                submitData.ID = Guid.NewGuid();
                result = Insert<TbScore>(submitData);
                if (!result)
                {
                    return KingResponse.GetErrorResponse("插入数据出错！", request);
                }

                var score = Select<TbScore>(submitData.ID);

                var userinfo = Select<TbUserInfo>(submitData.Userid);
                //更新用户文章信息
                if (userinfo != null)
                {
                    userinfo.Grade = submitData.Articleid;
                    Update<TbUserInfo>(userinfo);
                }
                if (score != null)
                {
                    var list = Search<TbScore>("userid='" + score.Userid + "'");
                    if (list != null)
                    {
                        score.TaskTime = list.Count;
                    }
                    else
                    {
                        score.TaskTime = 0;
                    }
                    return KingResponse.GetResponse(request, score);
                }

                return KingResponse.GetErrorResponse("返回数据出错！", request);
            }
            catch
            {
                return KingResponse.GetErrorResponse("插入数据出错！", request);
            }
        }

        /// <summary>
        /// 新增课程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse UpdScore(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            //TbScore Score = JsonHelper.DecodeJson<TbScore>(request.Data);

            var submitData = JsonHelper.DecodeJson<TbScore>(request.Data);

            #region 校验相应的数据有效性

            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("信息为空", request);
            }
            if (string.IsNullOrEmpty(submitData.Userid))
            {
                return KingResponse.GetErrorResponse("参数错误,无用户信息", request);
            }
            if (submitData.Articleid == 0)
            {
                return KingResponse.GetErrorResponse("参数错误,无题目信息", request);
            }
            #endregion

            try
            {
                var score = Select<TbScore>(submitData.ID);

                if (score != null)
                {
                    score.Articleid = submitData.Articleid;
                    score.Userid = submitData.Userid;
                    score.Filepath = submitData.Filepath;
                    score.Score = submitData.Score;

                    var result = Update(score);
                    if (!result)
                    {
                        return KingResponse.GetErrorResponse("修改数据出错！", request);
                    }

                    return KingResponse.GetResponse(request, score);
                }

                return KingResponse.GetErrorResponse("找不到更新的信息！", request);
            }
            catch
            {
                return KingResponse.GetErrorResponse("插入数据出错！", request);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryScore(KingRequest request)
        {
            PageParameter parameter = JsonHelper.DecodeJson<PageParameter>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确", request);
            }
            if (parameter.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确", request);
            }
            parameter.OrderColumns = "Createtime";
            parameter.TbNames = "tb_Score";
            parameter.IsOrderByASC = 1;
            parameter.Columns = "*";
            if (parameter.Where == null)
                parameter.Where = "";
            //IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list;
            list = parameter.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);

            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = FillData<TbScore>(ds.Tables[0])
            });
        }

        /// <summary>
        /// 根据条件返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryScoreById(KingRequest request)
        {
            TbScore parameter = JsonHelper.DecodeJson<TbScore>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.Userid == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            var list = Select<TbScore>(parameter.Userid);
            if (list != null)
            {
                return KingResponse.GetResponse(request, list);
            }

            return KingResponse.GetErrorResponse("当前查询发生异常", request);
        }

        /// <summary>
        /// 根据条件返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryScoreByUserId(KingRequest request)
        {
            TbScore parameter = JsonHelper.DecodeJson<TbScore>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.Userid == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            var list = Search<TbScore>("Userid = '" + parameter.Userid + "'");
            if (list != null)
            {
                if (list.Count > 0)
                {
                    return KingResponse.GetResponse(request, list);
                }
                return KingResponse.GetErrorResponse("没有数据", request);
            }

            return KingResponse.GetErrorResponse("当前查询发生异常", request);
        }

        public int QueryTaskTimeByUserId(string userid)
        {
            var list = Search<TbScore>("Userid = '" + userid + "'");
            if (list != null)
            {
                return list.Count;
            }
            return 0;
        }

        public IList<TbScore> QueryScoreByUserId(string userid)
        {
            var list = Search<TbScore>("Userid = '" + userid + "'");
            if (list != null)
            {
                if (list.Count > 0)
                {
                    return list;
                }
            }

            return null;
        }
    }
}
