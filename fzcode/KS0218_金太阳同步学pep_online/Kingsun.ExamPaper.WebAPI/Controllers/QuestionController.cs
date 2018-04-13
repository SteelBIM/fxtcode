using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.WebAPI.Models;
using Kingsun.ExamPaper.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Kingsun.ExamPaper.WebAPI.Controllers
{
    public class QuestionController : ApiController
    {
        /// <summary>
        /// 1.1 获取课本和目录信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetBookAndCatalogList([FromBody]UserModel userModel)
        {
            if (!userModel.OtherBookID.HasValue)
            {
                return KingsunResponse.GetErrorResult("没有获取到课本信息");
            }            
            IList<Custom_BookCatalog> list = new CatalogBLL().GetBookAndCatalogList(userModel.OtherBookID);
            return KingsunResponse.GetResult(list);
        }
        /// <summary>
        /// 1.2	根据CatalogID获取题目信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetQuestionList([FromBody]UserModel userModel)
        {
            if (!userModel.CatalogID.HasValue || string.IsNullOrEmpty(userModel.UserID) || !userModel.IsDo.HasValue)
            {
                return KingsunResponse.GetErrorResult("没有获取到请求条件");
            }
            IList<Custom_Question> list = new QuestionBLL().GetQuestionList(userModel.UserID, userModel.CatalogID.Value, userModel.IsDo.Value);
            return KingsunResponse.GetResult(list);
        }
        /// <summary>
        /// 1.3 一键上传答案(Android端)
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AndroidUploadStuAnswerList([FromBody]AndroidModel read)
        {
            string test = read.FormData;
            SubmitModel t = JsonHelper.DecodeJson<SubmitModel>(test);
            return UploadStuAnswerList(t);
        }
        /// <summary>
        /// 1.3 一键上传答案
        /// </summary>
        /// <param name="submitModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadStuAnswerList([FromBody]SubmitModel submitModel)
        {
            if (string.IsNullOrEmpty(submitModel.UserID) || !submitModel.CatalogID.HasValue)
            {
                return KingsunResponse.GetErrorResult("没有获取到上传数据");
            }
            if (new StuAnswerBLL().UploadStuAnswerList(submitModel.UserID, submitModel.CatalogID.Value, submitModel.TotalScore.Value, submitModel.AnswerList))
            {
                return KingsunResponse.GetResult("提交成功");
            }
            else
            {
                return KingsunResponse.GetErrorResult("提交失败");
            }
        }
        /// <summary>
        /// 1.4 获取报告
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetStuCatalog([FromBody]UserModel userModel)
        {
            if (!userModel.CatalogID.HasValue || string.IsNullOrEmpty(userModel.UserID))
            {
                return KingsunResponse.GetErrorResult("没有获取到请求条件");
            }
            return KingsunResponse.GetResult(new StuCatalogBLL().GetStuCatalog(userModel.UserID, userModel.CatalogID.Value));
        }
    }
}
