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
    public class ArticleManagement : BaseManagement
    {
        /// <summary>
        /// 新增课程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse AddArticle(KingRequest request)
        {
            //反序列化，得到客户端提交的数据
            //TbArticle article = JsonHelper.DecodeJson<TbArticle>(request.Data);

            IList<TbArticle> submitData = JsonHelper.DecodeJson<List<TbArticle>>(request.Data);

            #region 校验相应的数据有效性

            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("上次的信息为空", request);
            }

            #endregion


            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            DelAllArticle();
            bool result = true;
            try
            {
                foreach (var item in submitData)
                {
                    var result1 = dbManage.Insert(item);
                    if (!result1)
                    {
                        result = false;
                        break;
                    }
                }

                if (!result)
                {
                    return KingResponse.GetErrorResponse("插入数据出错！", request);
                }
                return KingResponse.GetResponse(request, "插入数据完成");
            }
            catch
            {
                dbManage.Rollback();
                //return KingResponse.GetErrorResponse("插入数据出错，提示：" + _operatorError, request);
            }
            return KingResponse.GetErrorResponse("插入数据出错，提示：" + _operatorError, request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryArticle(KingRequest request)
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
            parameter.OrderColumns = "APeriod";
            parameter.TbNames = "tb_Article";
            parameter.IsOrderByASC = 1;
            parameter.Columns = "*";
            if (parameter.Where == null)
                parameter.Where = "";
            //IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = parameter.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);

            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = FillData<TbArticle>(ds.Tables[0])
            });
        }

        /// <summary>
        /// 根据条件返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryArticleById(KingRequest request)
        {
            TbArticle parameter = JsonHelper.DecodeJson<TbArticle>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.ID == 0)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }

            var list = Select<TbArticle>(parameter.ID);
            if (list != null)
            {
                return KingResponse.GetResponse(request, list);
            }
            return KingResponse.GetErrorResponse("没有数据", request);
        }

        /// <summary>
        /// 根据条件返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryArticleByWhere(KingRequest request)
        {
            TbArticle parameter = JsonHelper.DecodeJson<TbArticle>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            var list = Search<TbArticle>(parameter.AContent);
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

        public int DelAllArticle()
        {
            string sql = "turncate table tb_Article;";
            var ds = ExecuteSql(sql);

            if (ds != null)
            {
                return 1;
            }
            return 0;

        }

        public KingResponse DelAllArticle(KingRequest request)
        {
            string sql = "turncate table tb_Article;";
            var ds = ExecuteSql(sql);

            return KingResponse.GetResponse(request,"成功");

        }

        public TbArticle SelArticleBy(int id)
        {
            if (id != 0)
            {
                var acticle = Select<TbArticle>(id);
                if (acticle != null)
                {
                    return acticle;
                }
            }
            return null;
        }

    }
}
