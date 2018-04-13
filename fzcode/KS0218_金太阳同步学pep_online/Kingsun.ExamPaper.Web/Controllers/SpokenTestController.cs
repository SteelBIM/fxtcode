using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Kingsun.ExamPaper.Web.Controllers
{
    public class SpokenTestController : Controller
    {
        private StuCatalogBLL stuCataBll = new StuCatalogBLL();
        private StuAnswerBLL stuAnserBll = new StuAnswerBLL();
        private CatalogBLL catalogBll = new CatalogBLL();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string SubmitAnswer(string queList)
        {
            string UserId = Request.Cookies["UserId"].Value;
            var answers = JsonHelper.DecodeJson<List<ExamPaper.Model.Tb_StuAnswer>>(queList);
            var dailyAnswers = JsonHelper.DecodeJson<List<ExamPaper.Model.Tb_StuAnswer_Month>>(queList);
            string isSuccess = "";
            WebClient web = new WebClient();

            for (int i = 0; i < answers.Count; i++)
            {
                var guid = Guid.NewGuid();
                string filename = Server.MapPath("/App_Data/" + guid + ".mp3");
                web.DownloadFile(answers[i].BestAnswer??answers[i].Answer, filename);

                var ossResult = Kingsun.SynchronousStudy.Common.OSSHelper.PutObject("synchronousstudy", "SynchronousStudy/KouyuCepingjuan/" + guid + ".mp3", filename);
                string mp3Url = "http://synchronousstudy.oss-cn-shenzhen.aliyuncs.com/SynchronousStudy/KouyuCepingjuan/" + guid + ".mp3";
                answers[i].Answer = mp3Url;
                answers[i].BestAnswer = mp3Url;
                dailyAnswers[i].Answer = mp3Url;
                dailyAnswers[i].BestAnswer = mp3Url;
                if (ossResult)
                    System.IO.File.Delete(filename);
            }

            if (answers.Count > 0)
            {
                isSuccess = stuAnserBll.SubmitStuBestAnswer(answers, UserId);
                isSuccess = stuAnserBll.SubmitStuDailyBestAnswer(dailyAnswers, UserId);
            }
            if (isSuccess == "提交成功")
            {
                var cata = catalogBll.GetNextCatalog(answers[0].CatalogID.Value);
                string type = "";
                if (cata.CatalogName.Contains("单词"))
                {
                    type = "word";
                }
                else if (cata.CatalogName.Contains("句子"))
                {
                    type = "sentence";
                }
                else if (cata.CatalogName.Contains("课文"))
                {
                    type = "story";
                }
                else
                {
                    type = "sound";
                }
                return string.Format("DoQuestion.aspx?CatalogId={0}&UnitCatalogId={1}&type={2}", cata.CatalogID, cata.ParentID, type);
            }
            return null;
        }


    }
}
