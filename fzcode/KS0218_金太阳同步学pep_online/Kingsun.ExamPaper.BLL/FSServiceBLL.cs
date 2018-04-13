using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.ExamPaper.Model;
using Kingsun.SynchronousStudy.Common;
using NPOI.OpenXmlFormats.Dml;
using JsonHelper = Kingsun.ExamPaper.Common.JsonHelper;

namespace Kingsun.ExamPaper.BLL
{
    public class FsServiceBll
    {
        StuAnswerBLL catalogbll = new StuAnswerBLL();
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper();
        public void ExecuteOTExampaper2DB()
        {
            var listCount = listRedis.Count("StuAnswerLisExampaper_OT");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            string kingsun_sz = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_ot"].ConnectionString;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("StuAnswerLisExampaper_OT");
                try
                {
                    Exampaper2DbModel data = JsonHelper.DecodeJson<Exampaper2DbModel>(model);
                    if (data != null)
                    {
                        catalogbll.UploadStuAnswerList(data.UserID, data.CatalogID, data.TotalScore,
                            data.AnswerList, kingsun_sz);
                    }

                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "学习报告同步异常,异常数据：" + model);
                }
            }
        }
        public void ExecuteSZExampaper2DB()
        {
            var listCount = listRedis.Count("StuAnswerLisExampaper_SZ");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            string kingsun_sz = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_sz"].ConnectionString;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("StuAnswerLisExampaper_SZ");
                try
                {
                    Exampaper2DbModel data = JsonHelper.DecodeJson<Exampaper2DbModel>(model);
                    if (data != null)
                    {
                        catalogbll.UploadStuAnswerList(data.UserID, data.CatalogID, data.TotalScore,
                            data.AnswerList, kingsun_sz);
                    }

                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "学习报告同步异常,异常数据：" + model);
                }
            }
        }
        public void ExecuteBJExampaper2DB()
        {
            var listCount = listRedis.Count("StuAnswerLisExampaper_BJ");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            string kingsun_sz = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("StuAnswerLisExampaper_BJ");
                try
                {
                    Exampaper2DbModel data = JsonHelper.DecodeJson<Exampaper2DbModel>(model);
                    if (data != null)
                    {
                        catalogbll.UploadStuAnswerList(data.UserID, data.CatalogID, data.TotalScore,
                            data.AnswerList, kingsun_sz);
                    }

                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "学习报告同步异常,异常数据：" + model);
                }
            }
        }
        public void ExecuteRJExampaper2DB()
        {
            var listCount = listRedis.Count("StuAnswerLisExampaper_RJ");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            string kingsun_sz = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_rjpep"].ConnectionString;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("StuAnswerLisExampaper_RJ");
                try
                {
                    Exampaper2DbModel data = JsonHelper.DecodeJson<Exampaper2DbModel>(model);
                    if (data != null)
                    {
                        catalogbll.UploadStuAnswerList(data.UserID, data.CatalogID, data.TotalScore,
                            data.AnswerList, kingsun_sz);
                    }

                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "学习报告同步异常,异常数据：" + model);
                }
            }
        }
    }
}
