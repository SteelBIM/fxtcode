using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kingsun.AppLibrary.Model;
using Kingsun.DB;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL.Management
{
    public class ModuleConfigManagement : BaseManagement
    {

        /// <summary>
        /// 获取目录模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryModuleConfigList(KingRequest request)
        {
            PageParameter param = JsonHelper.DecodeJson<PageParameter>(request.Data);
            #region 验证相关数据有效性
            if (param == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空");
            }
            if (param.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确");
            }
            if (param.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确");
            }
            #endregion
            param.OrderColumns = "ID";
            param.TbNames = "TB_ModuleConfiguration";
            param.IsOrderByASC = 1;
            param.Columns = "*";
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = param.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);
            if (ds == null || ds.Tables.Count < 1)
            {

                return KingResponse.GetErrorResponse("执行存储过程失败，" + _operatorError);
            }
            object obj = new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = FillData<TB_ModuleConfiguration>(ds.Tables[0])
            };
            return KingResponse.GetResponse(request, obj);
        }

        public KingResponse DeleteModuleConfig(KingRequest request)
        {
            TB_ModuleConfiguration versonData = JsonHelper.DecodeJson<TB_ModuleConfiguration>(request.Data);
            try
            {
                if (Delete<TB_ModuleConfiguration>(versonData.ID))
                    return KingResponse.GetResponse(request, "删除成功！");
                else
                    return KingResponse.GetErrorResponse("删除失败" + _operatorError, request);
            }
            catch { return KingResponse.GetErrorResponse("删除失败" + _operatorError, request); }
        }

        public KingResponse ModifyModule(KingRequest request)
        {
            TB_ModuleConfiguration Tmc = JsonHelper.DecodeJson<TB_ModuleConfiguration>(request.Data);
            var fc = Select<TB_ModuleConfiguration>(Tmc.ID);
            if (fc == null)
            {
                return KingResponse.GetErrorResponse("找不到此目录信息");
            }
            if (fc.SecondTitleID!=null)
            fc.SecondTitle = Tmc.SecondTitle;
            if (fc.FirstTitle != Tmc.FirstTitle)
            {
                var MCList = Search<TB_ModuleConfiguration>("FirstTitileID='" + fc.FirstTitileID + "'");
                int i;
                if (MCList != null)
                {
                    for (i = 0; i < MCList.Count; i++)
                    {
                        MCList[i].FirstTitle = Tmc.FirstTitle;
                        Update<TB_ModuleConfiguration>(MCList[i]);
                    }
                }
            }
            fc.FirstTitle = Tmc.FirstTitle;
            if (Update<TB_ModuleConfiguration>(fc))
            {
                return KingResponse.GetResponse(request, "更新成功");
            }
            return KingResponse.GetErrorResponse("更新失败" + _operatorError, request);
        }


        public KingResponse QueryModule(KingRequest request)
        {
            TB_ModuleConfiguration p = JsonHelper.DecodeJson<TB_ModuleConfiguration>(request.Data);
            var fee = Select<TB_ModuleConfiguration>(p.ID);
            return KingResponse.GetResponse(request, fee);

        }
    }
}
