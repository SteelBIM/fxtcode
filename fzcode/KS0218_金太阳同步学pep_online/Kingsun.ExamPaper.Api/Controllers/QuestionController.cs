using System;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.Api.Models;
using Kingsun.ExamPaper.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using JsonHelper = Kingsun.ExamPaper.Common.JsonHelper;

namespace Kingsun.ExamPaper.Api.Controllers
{
    public class QuestionController : ApiController
    {
        private BaseManagement bm = new BaseManagement();
        private StuAnswerBLL sab = new StuAnswerBLL();
        static RedisHashHelper redis = new RedisHashHelper();
        private RedisListHelper ibsredisList = new RedisListHelper();
        string appAreaVersion
        {
            get
            {
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("AppAreaVersion")))
                {
                    throw new Exception("未获取到地区版本appAreaVersion");
                }
                return System.Configuration.ConfigurationManager.AppSettings.Get("AppAreaVersion");
            }
        }

        [HttpPost]
        [HttpGet]
        public HttpResponseMessage GetBookAndCatalogListTest()
        {
          
            try
            {
                Custom_MODBook m = new Custom_MODBook();
                UserModel userModel = new UserModel();
                userModel.OtherBookID = 168;
                userModel.UserID = "454582054";
                IList<Custom_BookCatalog> list = new List<Custom_BookCatalog>();
                if (!userModel.OtherBookID.HasValue)
                {
                    return KingsunResponse.GetErrorResult("没有获取到课本信息");
                }
                if (string.IsNullOrEmpty(userModel.UserID))
                {
                    return KingsunResponse.GetErrorResult("没有获取到用户信息");
                }
                m = redis.Get<Custom_MODBook>(appAreaVersion + "_Exampaper_Custom_MODBook", userModel.OtherBookID.Value.ToString());
                if (m == null)
                {
                    m = new PracticeEveryDayBLL().GetBookInfoFromMOD(userModel.OtherBookID.Value);
                    if (m == null)
                    {
                        return KingsunResponse.GetSyncErrorResult("没有获取到教材对应的基础信息");
                    }

                    redis.Set<Custom_MODBook>(appAreaVersion + "_Exampaper_Custom_MODBook", userModel.OtherBookID.Value.ToString(), m);
                }

                list = new CatalogBLL().GetBookAndCatalogList(userModel.OtherBookID, userModel.UserID);

                if (list == null && !list.Any())
                {
                    return KingsunResponse.GetSyncErrorResult("没有获取到教材和目录信息");
                }


                //从submitModel读最新做题
                foreach (var cb in list)
                {
                    if (cb.CatalogList != null && cb.CatalogList.Any())
                    {
                        foreach (var c in cb.CatalogList)
                        {
                            var sm = redis.Get<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + c.CatalogID);
                            c.IsSubmit = sm == null ? c.IsSubmit : 1;
                            foreach (var cs in c.SecondCatalogList)
                            {
                                var csSm = redis.Get<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + cs.CatalogID);
                                cs.IsSubmit = csSm == null ? cs.IsSubmit : 1;
                            }
                        }
                    }
                }

                return KingsunResponse.GetResult(list.Any() ? list[0] : new Custom_BookCatalog());
            }
            catch (Exception ex)
            {

                return KingsunResponse.GetErrorResult(ex.Message + ex.StackTrace);
            }
        }


        /// <summary>
        /// 1.1 点击53获取课本和目录信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetBookAndCatalogList([FromBody]UserModel userModel)
        {
            try
            {
                Custom_MODBook m = new Custom_MODBook();
                IList<Custom_BookCatalog> list = new List<Custom_BookCatalog>();
                if (!userModel.OtherBookID.HasValue)
                {
                    return KingsunResponse.GetErrorResult("没有获取到课本信息");
                }
                if (string.IsNullOrEmpty(userModel.UserID))
                {
                    return KingsunResponse.GetErrorResult("没有获取到用户信息");
                }
                m = redis.Get<Custom_MODBook>(appAreaVersion + "_Exampaper_Custom_MODBook", userModel.OtherBookID.Value.ToString());
                if (m == null)
                {
                    m = new PracticeEveryDayBLL().GetBookInfoFromMOD(userModel.OtherBookID.Value);
                    if (m == null)
                    {
                        return KingsunResponse.GetSyncErrorResult("没有获取到教材对应的基础信息");
                    }

                    redis.Set<Custom_MODBook>(appAreaVersion + "_Exampaper_Custom_MODBook", userModel.OtherBookID.Value.ToString(), m);
                }

                list = new CatalogBLL().GetBookAndCatalogList(userModel.OtherBookID, userModel.UserID);

                if (list == null && !list.Any())
                {
                    return KingsunResponse.GetSyncErrorResult("没有获取到教材和目录信息");
                }


                //从submitModel读最新做题
                foreach (var cb in list)
                {
                    if (cb.CatalogList != null && cb.CatalogList.Any())
                    {
                        foreach (var c in cb.CatalogList)
                        {
                            var sm = redis.Get<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + c.CatalogID);
                            c.IsSubmit = sm == null ? c.IsSubmit : 1;
                            foreach (var cs in c.SecondCatalogList)
                            {
                                var csSm = redis.Get<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + cs.CatalogID);
                                cs.IsSubmit = csSm == null ? cs.IsSubmit : 1;
                            }
                        }
                    }
                }

                return KingsunResponse.GetResult(list.Any() ? list[0] : new Custom_BookCatalog());
            }
            catch (Exception ex)
            {

                return KingsunResponse.GetErrorResult(ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 1.2	根据CatalogID获取题目信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage GetQuestionList([FromBody]UserModel userModel)
        {
            try
            {
                IList<Custom_Question> list = null;
                if (!userModel.CatalogID.HasValue || string.IsNullOrEmpty(userModel.UserID) || !userModel.IsDo.HasValue)
                {
                    return KingsunResponse.GetErrorResult("没有获取到请求条件");
                }
                list = redis.Get<IList<Custom_Question>>(appAreaVersion + "_Exampaper_Custom_Question", userModel.CatalogID.Value.ToString());
                if (list == null)
                {
                    list = new QuestionBLL().GetQuestionList(userModel.UserID, userModel.CatalogID.Value, userModel.IsDo.Value);
                    if (list == null || !list.Any())
                    {
                        return KingsunResponse.GetErrorResult("未获取到题目列表");
                    }
                    else
                    {
                        redis.Set(appAreaVersion + "_Exampaper_Custom_Question", userModel.CatalogID.Value.ToString(), list);
                    }
                }

                foreach (var qu in list)
                {
                    qu.StuAnswer = null;
                }

                //从submitModel读最新做题
                var sm = redis.Get<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + userModel.CatalogID);
                if (userModel.IsDo == 0 && !string.IsNullOrWhiteSpace(userModel.UserID))
                {
                    foreach (var qu in list)
                    {//qu.StuAnswer为空是因为DB中没有记录,需要在程序中构造StuAnswer对象并赋上
                        if (sm != null && sm.AnswerList != null && sm.AnswerList.Any())
                        {
                            var q = sm.AnswerList.FirstOrDefault(o => o.QuestionID == qu.QuestionID);
                            if (q != null)
                            {
                                if (qu.StuAnswer == null)
                                {
                                    qu.StuAnswer = new Tb_StuAnswer();
                                    //基本数据
                                    qu.StuAnswer.StuAnswerID = "";
                                    qu.StuAnswer.StuCatID = "";
                                    qu.StuAnswer.StuID = sm.UserID;
                                    qu.StuAnswer.CatalogID = sm.CatalogID;
                                    qu.StuAnswer.QuestionID = qu.QuestionID;
                                    qu.StuAnswer.QuestionID = qu.ParentID;
                                }

                                //分数
                                qu.StuAnswer.Answer = q == null ? null : q.Answer;
                                qu.StuAnswer.IsRight = q == null ? 0 : q.IsRight;
                                qu.StuAnswer.Score = q == null ? 0 : (decimal)q.Score;
                            }
                        }
                    }
                }


                //redis.Set("Custom_Question_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + userModel.CatalogID.Value + "_" + userModel.IsDo.Value, list);

                return KingsunResponse.GetResult(list);
            }
            catch (Exception ex)
            {
                return KingsunResponse.GetErrorResult(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 1.2	根据CatalogID获取题目信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetQuestionListTest()
        {
            UserModel userModel = new UserModel();
            userModel.IsDo =1;
            userModel.CatalogID = 345;
            userModel.UserID = "1242345889";
            if (!userModel.CatalogID.HasValue || string.IsNullOrEmpty(userModel.UserID) || !userModel.IsDo.HasValue)
            {
                return KingsunResponse.GetErrorResult("没有获取到请求条件");
            }
            IList<Custom_Question> list = new QuestionBLL().GetQuestionList(userModel.UserID, userModel.CatalogID.Value, userModel.IsDo.Value);
            return KingsunResponse.GetResult(list);
        }



        /// <summary>
        /// 1.3 一键上传答案
        /// </summary>
        /// <param name="submitModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadStuAnswerList([FromBody]SubmitModel submitModel)
        {


            if (string.IsNullOrEmpty(submitModel.UserID) || !submitModel.CatalogID.HasValue || submitModel.AnswerList == null || submitModel.AnswerList.Count == 0)
            {
                return KingsunResponse.GetErrorResult("没有获取到上传数据");
            }

            #region  此处为了解决超级分数BUG
            //var groups = submitModel.AnswerList.GroupBy(o => o.QuestionID);
            //if (groups.Any(o => o.Count() > 1))
            //{
            //    return KingsunResponse.GetErrorResult("提交了重复的QuestionID:" + groups.First(o => o.Count() > 1).First().QuestionID);
            //}

            //var sumScore = submitModel.AnswerList.Where(o => !string.IsNullOrWhiteSpace(o.ParentID)).Sum(o => o.Score);

            //if (submitModel.TotalScore > sumScore)//总分大于小题分数之和bug
            //{
            //    submitModel.TotalScore = sumScore;
            //}
            if (submitModel.TotalScore > 100)
            {
                Log4Net.LogHelper.Error("出现超级分数:"+submitModel.ToJson());
                submitModel.TotalScore = 100;
            }
            #endregion
            try
            {
                //构建报告页接口(GetStuCatalog)的redis数据
                #region
                Custom_StuCatalog custom_StuCatalog = redis.Get<Custom_StuCatalog>(appAreaVersion + "_Exampaper_Custom_StuCatalog_" + submitModel.UserID.Substring(0, 2), submitModel.UserID + "_" + submitModel.CatalogID.Value);

                if (custom_StuCatalog == null)
                {
                    custom_StuCatalog = new Custom_StuCatalog();

                }
                //构造题目和答案
                var list = redis.Get<IList<Custom_Question>>(appAreaVersion + "_Exampaper_Custom_Question", submitModel.CatalogID.Value.ToString());//题目列表
                var questionList = new List<Custom_PQ>();
                foreach (var a in submitModel.AnswerList)
                {
                    var q = list.FirstOrDefault(o => o.QuestionID == a.QuestionID);
                    if (q == null)
                    {
                        return KingsunResponse.GetErrorResult("未找到questionid对应的题目" + a.QuestionID);
                    }
                    //if (string.IsNullOrWhiteSpace(q.ParentID))
                    //{
                    questionList.Add(new Custom_PQ
                    {
                        QuestionID = a.QuestionID,
                        Score = a.Score,
                        QuestionModel = q.QuestionModel,
                        QuestionTitle = q.QuestionTitle,
                        Sort = q.Sort,
                        ParentID = a.ParentID,
                        BestAnswer = a.Answer,
                        BestScore = a.Score,
                        BestIsRight = a.IsRight,
                    });
                    //}
                }

                custom_StuCatalog.BestTotalScore = custom_StuCatalog.BestTotalScore < submitModel.TotalScore.Value ? submitModel.TotalScore.Value : custom_StuCatalog.BestTotalScore;
                if (custom_StuCatalog.BestTotalScore <= submitModel.TotalScore.Value)//只存最优记录
                {
                    custom_StuCatalog.QuestionList = questionList;
                    custom_StuCatalog.TotalScore = submitModel.TotalScore.Value;
                }

                AsyncCustom_StuCatalog(submitModel);

                redis.Set<Custom_StuCatalog>(appAreaVersion + "_Exampaper_Custom_StuCatalog_" + submitModel.UserID.Substring(0, 2), submitModel.UserID + "_" + submitModel.CatalogID.Value, custom_StuCatalog);

                ibsredisList.LPush("StuAnswerLisExampaper_" + appAreaVersion, submitModel.ToJson());

                //var update = new StuAnswerBLL().UploadStuAnswerList(submitModel.UserID, submitModel.CatalogID.Value, submitModel.TotalScore.Value, submitModel.AnswerList);
                #endregion


            }
            catch (Exception ex)
            {            

                Log4Net.LogHelper.Error("异步写入redis报告UploadStuAnswerList, UserID=" + submitModel.UserID + "\t CatalogID=" + submitModel.CatalogID + "\t异常：" + ex.Message);
                return KingsunResponse.GetErrorResult("提交失败!" + ex.Message);
            }

            return KingsunResponse.GetResult("提交成功");
        }


        [HttpGet]
        public HttpResponseMessage Test()
        {
            string model = "{\"UserID\":\"5097600\",\"CatalogID\":82,\"TotalScore\":90.000000,\"DoDate\":\"/Date(1514390400000)/\",\"AnswerList\":[{\"QuestionID\":\"918e6d58-8593-44a3-9477-e0b6b5b17bc7\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"6\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":10.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"70603d5a-bb20-4b33-90a4-f8964751df69\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"99cbeeac-8e23-4def-9dd9-0be0ae1e18b2\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"4\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"51022beb-215b-40cb-8d1d-064b49d6e0c7\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"7\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"382e3008-edb5-4102-a4ad-c2819e7cf9ac\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"9\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"2cec3b7f-2656-4e99-bc31-2797a6f0b8d5\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"9a2cdae6-6f37-4285-a29d-fff4ea1db443\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"10\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"7f35d882-35b9-4b60-9cdc-7d0083775d61\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"1\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"a766b969-d8f9-4b7d-ac4e-4dc3cc3ef64e\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"5\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"e1508408-6047-413e-a46a-18929f939775\",\"ParentID\":\"f5956390-fcd6-4f05-9d31-1efd014960f0\",\"Answer\":\"8\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"90be8d12-8b5a-4830-9912-4ee53ac5d53a\",\"ParentID\":\"397a9b29-cf38-4752-bb5a-3b41deec36b7\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"397a9b29-cf38-4752-bb5a-3b41deec36b7\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":10.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"e7d513f0-3436-461b-926d-e321e76b8e10\",\"ParentID\":\"397a9b29-cf38-4752-bb5a-3b41deec36b7\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"cdbd789c-cc22-4e70-94bb-611c9ae2917a\",\"ParentID\":\"397a9b29-cf38-4752-bb5a-3b41deec36b7\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"91b6da65-1548-466d-8660-72e0f435e7bc\",\"ParentID\":\"397a9b29-cf38-4752-bb5a-3b41deec36b7\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"3192e0d1-ced7-401d-b2fd-4afc1478762b\",\"ParentID\":\"397a9b29-cf38-4752-bb5a-3b41deec36b7\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"d3c2e97b-f553-4110-b48a-7bf4b03946cc\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":10.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"8068836a-96d6-4d3e-8527-86678a5fdcea\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"19cdfdf0-27e7-44a3-a9ed-ed5eb5d7e457\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"1\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"5ea72d76-9223-4650-881d-710ad90c4115\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"05eb9993-dc19-45ec-9830-223e45bc0d48\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"11f751de-68ed-4aba-8ae9-013965e4c57c\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"1\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"23f150a6-28e8-47da-b0e0-82abfba6ac31\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"5051cc53-469a-4b6e-8762-6722dae6a033\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"f2e1baa5-edbf-4b9c-bbbb-838b5f0caa19\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"782c32c1-515c-40ad-9f2c-d569ae7ecb7d\",\"ParentID\":\"24d3023a-6918-4f42-87a8-49da06cf33be\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"14fb197c-a590-4e53-a5af-27f4dfa94b8d\",\"ParentID\":\"be425b60-10d0-443f-aad9-b819dab02d3d\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"be425b60-10d0-443f-aad9-b819dab02d3d\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":10.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"42dd7c0c-f50a-4488-8dfc-b82f7bd6896e\",\"ParentID\":\"be425b60-10d0-443f-aad9-b819dab02d3d\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"3344de2d-6d1a-48bf-bd89-ec38e43bbbf0\",\"ParentID\":\"be425b60-10d0-443f-aad9-b819dab02d3d\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"09964a3a-29b1-4d45-bfa8-7d60ffef382e\",\"ParentID\":\"be425b60-10d0-443f-aad9-b819dab02d3d\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"77b63f26-8fdb-4797-9afc-d76f1e6ef1d5\",\"ParentID\":\"be425b60-10d0-443f-aad9-b819dab02d3d\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"7502f802-3ac7-4f31-8329-a6cc82e9d665\",\"ParentID\":\"48163521-1628-44ff-bfd5-7b7c57cf489d\",\"Answer\":\"good\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"48163521-1628-44ff-bfd5-7b7c57cf489d\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":4.000000,\"IsRight\":0,\"CatalogID\":null},{\"QuestionID\":\"fc976218-7850-4111-b552-e2143d5e72c6\",\"ParentID\":\"48163521-1628-44ff-bfd5-7b7c57cf489d\",\"Answer\":\"He\u0027s\",\"Score\":0.000000,\"IsRight\":0,\"CatalogID\":null},{\"QuestionID\":\"0fcb25eb-a39f-49c2-be50-46b1275e31af\",\"ParentID\":\"48163521-1628-44ff-bfd5-7b7c57cf489d\",\"Answer\":\"Chinese\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"0049d652-a95c-4a57-8b4c-bc465eb0af96\",\"ParentID\":\"48163521-1628-44ff-bfd5-7b7c57cf489d\",\"Answer\":\"hfyuyi\",\"Score\":0.000000,\"IsRight\":0,\"CatalogID\":null},{\"QuestionID\":\"1274f95e-4c23-411b-9699-3cb623936e0d\",\"ParentID\":\"48163521-1628-44ff-bfd5-7b7c57cf489d\",\"Answer\":\"hgfyu\",\"Score\":0.000000,\"IsRight\":0,\"CatalogID\":null},{\"QuestionID\":\"dd39d9ff-5939-4583-955f-e8ab318c3a1d\",\"ParentID\":\"095714c1-264f-4a35-ada5-0c1e9d04ef70\",\"Answer\":\"4\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"095714c1-264f-4a35-ada5-0c1e9d04ef70\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":10.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"bcbfc3fd-d067-42e9-a6bf-8aaab2c55799\",\"ParentID\":\"095714c1-264f-4a35-ada5-0c1e9d04ef70\",\"Answer\":\"4\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"6b1de339-646d-486c-9348-0868b68af1b9\",\"ParentID\":\"095714c1-264f-4a35-ada5-0c1e9d04ef70\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"10c3fc7e-f216-43e2-a4ca-90aba8df331a\",\"ParentID\":\"095714c1-264f-4a35-ada5-0c1e9d04ef70\",\"Answer\":\"3\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"fa5ac7f4-33a9-47fa-a18e-a0dd4d3917c8\",\"ParentID\":\"095714c1-264f-4a35-ada5-0c1e9d04ef70\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"9f0040b5-ba1c-41bb-86e4-92714f0f90de\",\"ParentID\":\"021a568e-d27a-4adc-832d-ebabf7eeef44\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"021a568e-d27a-4adc-832d-ebabf7eeef44\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":8.000000,\"IsRight\":0,\"CatalogID\":null},{\"QuestionID\":\"67072816-7da6-4cb8-9797-afd3dd21d1b7\",\"ParentID\":\"021a568e-d27a-4adc-832d-ebabf7eeef44\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"43ae2341-4082-4a29-8296-d8983a2ffdee\",\"ParentID\":\"021a568e-d27a-4adc-832d-ebabf7eeef44\",\"Answer\":\"3\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"f122f776-a3cf-44c1-90ed-c236369a6748\",\"ParentID\":\"021a568e-d27a-4adc-832d-ebabf7eeef44\",\"Answer\":\"2\",\"Score\":0.000000,\"IsRight\":0,\"CatalogID\":null},{\"QuestionID\":\"e670c37f-03f5-42cd-b3d2-93ecad58339d\",\"ParentID\":\"021a568e-d27a-4adc-832d-ebabf7eeef44\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"703736ae-014b-4d54-a906-ed141fabf725\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"1\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":10.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"09672b79-3aeb-48c1-93d0-9215b53e455f\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"fc4b9bc5-543b-4d36-b4de-4ce65bcf1b2a\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"1\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"7a465c69-bf03-44aa-9a7d-73cf5847cd01\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"99a10992-4511-425c-9091-b8aefeb348d8\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"3a5f65c9-63cd-41a5-8c8f-47ee0325faf3\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"1\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"ca4a5ede-933e-493b-898a-2270b0086581\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"38024d27-5e70-4524-836b-445bd0e8de7b\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"3\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"9323f13f-f0af-4f33-b30b-70bb9bf77643\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"dc3014a5-15f6-4e0d-b4fe-4a4f4be32605\",\"ParentID\":\"d9f2bc57-e042-4170-802c-9b2bb4a15ad6\",\"Answer\":\"2\",\"Score\":1.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"297aad92-64a0-4edb-ab05-beda11e9d695\",\"ParentID\":\"cde6ebe9-eee0-453f-9db7-7962eee72674\",\"Answer\":\"C\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"cde6ebe9-eee0-453f-9db7-7962eee72674\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":10.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"6f2cb6b1-6e59-4011-975f-ddedb795b5b6\",\"ParentID\":\"cde6ebe9-eee0-453f-9db7-7962eee72674\",\"Answer\":\"D\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"323c0105-a603-4666-9e6a-457b2822fb35\",\"ParentID\":\"cde6ebe9-eee0-453f-9db7-7962eee72674\",\"Answer\":\"E\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"9436140f-2b75-4d6d-a0b1-91321f2d197b\",\"ParentID\":\"cde6ebe9-eee0-453f-9db7-7962eee72674\",\"Answer\":\"B\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"d92a795b-0890-4c94-990d-309ce14989b3\",\"ParentID\":\"cde6ebe9-eee0-453f-9db7-7962eee72674\",\"Answer\":\"A\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"4646d5dd-35d9-4134-b125-389012dba653\",\"ParentID\":\"58520812-aac9-4dda-b218-4f9bf348d5fa\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"58520812-aac9-4dda-b218-4f9bf348d5fa\",\"ParentID\":\"\",\"Answer\":\"\",\"Score\":8.000000,\"IsRight\":0,\"CatalogID\":null},{\"QuestionID\":\"faa62b76-c12c-4843-8a57-e9a6c56e38bb\",\"ParentID\":\"58520812-aac9-4dda-b218-4f9bf348d5fa\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"5e2f6d4c-f3dd-455b-9375-dabf9e71a167\",\"ParentID\":\"58520812-aac9-4dda-b218-4f9bf348d5fa\",\"Answer\":\"1\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"a68464f9-e25d-46d0-9244-d37fc009cb73\",\"ParentID\":\"58520812-aac9-4dda-b218-4f9bf348d5fa\",\"Answer\":\"2\",\"Score\":2.000000,\"IsRight\":1,\"CatalogID\":null},{\"QuestionID\":\"9626f61d-b879-415e-b6d3-2f9cbc943a18\",\"ParentID\":\"58520812-aac9-4dda-b218-4f9bf348d5fa\",\"Answer\":\"1\",\"Score\":0.000000,\"IsRight\":0,\"CatalogID\":null}]}";
            Exampaper2DbModel data = JsonHelper.DecodeJson<Exampaper2DbModel>(model);
            new StuAnswerBLL().UploadStuAnswerList(data.UserID, data.CatalogID, data.TotalScore, data.AnswerList);
            return KingsunResponse.GetResult("提交成功");
        }
        /// <summary>
        /// 初始化MODEL
        /// </summary>
        /// <param name="submitModel"></param>
        /// <returns></returns>


        /// <summary>
        /// 异步redis写入报告
        /// </summary>
        /// <param name="agrs"></param>
        public bool AsyncCustom_StuCatalog(object agrs)
        {
            SubmitModel submitModel = null;
            try
            {
                if (agrs is SubmitModel)
                {
                    submitModel = (SubmitModel)agrs;

                    var sm = redis.Get<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + submitModel.UserID.Substring(0, 2), submitModel.UserID + "_" + submitModel.CatalogID);
                    if (sm == null)
                    {
                        submitModel.DoDate = DateTime.Now.Date;
                        return redis.Set<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + submitModel.UserID.Substring(0, 2), submitModel.UserID + "_" + submitModel.CatalogID, submitModel);
                    }
                    else
                    {
                        if (sm.TotalScore <= submitModel.TotalScore)
                        {
                            sm.AnswerList = submitModel.AnswerList;
                            sm.DoDate = DateTime.Now.Date;
                            sm.TotalScore = submitModel.TotalScore;

                            return redis.Set<SubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + submitModel.UserID.Substring(0, 2), submitModel.UserID + "_" + submitModel.CatalogID, sm);
                        }

                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("异步写入redis报告UploadStuAnswerList, UserID=" + submitModel.UserID + "\t CatalogID=" + submitModel.CatalogID + "\t异常：" + ex.Message);

                return false;
            }
        }
        /// <summary>
        /// 1.3 一键上传答案
        /// </summary>
        /// <param name="submitModel"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UploadStuAnswerListTest()
        {
            SubmitModel submitModel = new SubmitModel();
            submitModel.UserID = "98702742";
            submitModel.CatalogID = 2;
            IList<Custom_StuAnswer> list = new List<Custom_StuAnswer>();
            Custom_StuAnswer stus = new Custom_StuAnswer
              {
                  QuestionID = "fa528d00-c4c1-4c5f-b43e-e0da1f2a911f",
                  ParentID = "98c860e0-5248-4f5e-8f9b-2b291e6084c1",
                  Answer = "1",
                  Score = 1,
                  IsRight = 1
              };
            list.Add(stus);
            submitModel.AnswerList = list;

            submitModel.TotalScore = (decimal)95.0;
            //if (string.IsNullOrEmpty(submitModel.UserID) || !submitModel.CatalogID.HasValue || submitModel.AnswerList == null || submitModel.AnswerList.Count == 0)
            //{
            //    return KingsunResponse.GetErrorResult("没有获取到上传数据");
            //}

            UploadStuAnswerList(submitModel);
            //if (new StuAnswerBLL().UploadStuAnswerList(submitModel.UserID, submitModel.CatalogID.Value, submitModel.TotalScore.Value, submitModel.AnswerList))
            //{
            //    return KingsunResponse.GetResult("提交成功");
            //}
            //else
            //{
            //    return KingsunResponse.GetErrorResult("提交失败");
            //}
            return KingsunResponse.GetResult("");
        }

        /// <summary>
        /// 1.4 获取报告
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetStuCatalog([FromBody]UserModel userModel)
        {
            Custom_StuCatalog _m = new Custom_StuCatalog();
            if (!userModel.CatalogID.HasValue || string.IsNullOrEmpty(userModel.UserID))
            {
                return KingsunResponse.GetErrorResult("没有获取到请求条件");
            }
            Custom_StuCatalog custom_StuCatalog = redis.Get<Custom_StuCatalog>(appAreaVersion + "_Exampaper_Custom_StuCatalog_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + userModel.CatalogID.Value);
            if (custom_StuCatalog == null)
            {
                custom_StuCatalog = new StuCatalogBLL().GetStuCatalog(userModel.UserID, userModel.CatalogID.Value);
                if (custom_StuCatalog == null)
                {
                    return KingsunResponse.GetErrorResult("未获取到成绩报告!");
                }
                redis.Set<Custom_StuCatalog>(appAreaVersion + "_Exampaper_Custom_StuCatalog_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + userModel.CatalogID.Value, custom_StuCatalog);
            }

            //从submitModel读最新做题
            var sm = redis.Get<CSubmitModel>(appAreaVersion + "_Exampaper_SubmitModel_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + userModel.CatalogID);
            if (sm != null)//更新分数
            {
                custom_StuCatalog.TotalScore = sm.TotalScore.HasValue?sm.TotalScore.Value.CutDoubleWithN(1):0;//最近分数
                custom_StuCatalog.DoDate = sm.DoDate.HasValue ? sm.DoDate.Value : DateTime.Now;
                if (custom_StuCatalog.BestTotalScore < sm.TotalScore || custom_StuCatalog.BestTotalScore <= 0)//没做过或者低于最近总分
                {
                    custom_StuCatalog.BestTotalScore = sm.TotalScore.HasValue ? sm.TotalScore.Value.CutDoubleWithN(1) : 0;//最近分数
                }
                else
                {
                    custom_StuCatalog.BestTotalScore = custom_StuCatalog.BestTotalScore.CutDoubleWithN(1);
                }
            }
            if (sm != null && custom_StuCatalog.QuestionList != null && sm.AnswerList != null && sm.AnswerList.Any())
            {
                foreach (var q in custom_StuCatalog.QuestionList)
                {
                    var qAnswer = sm.AnswerList.FirstOrDefault(o => o.QuestionID == q.QuestionID);
                    q.Score = qAnswer != null ? qAnswer.Score.CutDoubleWithN(1) : q.Score;
                }
            }

            //处理排序（前端app要求小题在前，大题在后）
            //if (custom_StuCatalog.QuestionList != null && custom_StuCatalog.QuestionList.Any())
            //{
            //    List<Custom_PQ> qList = new List<Custom_PQ>();
            //    var parents = custom_StuCatalog.QuestionList.Where(o => string.IsNullOrWhiteSpace(o.ParentID) || o.ParentID == "NULL").OrderBy(o => o.Sort).ToList();
            //    foreach (var p in parents)
            //    {
            //        qList.AddRange(custom_StuCatalog.QuestionList.Where(o => o.ParentID == p.QuestionID).OrderBy(o=>o.Sort).ToList());
            //        qList.Add(p);
            //    }
            //    custom_StuCatalog.QuestionList = qList;
            //}


            redis.Set<Custom_StuCatalog>(appAreaVersion + "_Exampaper_Custom_StuCatalog_" + userModel.UserID.Substring(0, 2), userModel.UserID + "_" + userModel.CatalogID.Value, custom_StuCatalog);

            return KingsunResponse.GetResult(custom_StuCatalog);
        }

        /// <summary>
        /// 1.5 获取某天报告
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetDoDateStuCatalog([FromBody]UserModel userModel)
        {
            DateTime date = DateTime.Parse(userModel.DoDate);
            TimeSpan ts = (DateTime.Now).Subtract(date);
            if (ts.Days > 30)
            {
                return KingsunResponse.GetErrorResult("只能获取30天内学校报告");
            }
            else
            {
                date = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            }
            return KingsunResponse.GetResult(new StuCatalogBLL().GetDoDateStuCatalog(userModel.UserID, date));
        }

        /// <summary>
        /// 1.6 班级圈周学校报告
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetClassStuCatalogWeek([FromBody]UserModel userModel)
        {
            return KingsunResponse.GetResult(new StuCatalogBLL().GetClassStuCatalogWeek(userModel.UserID.ToString()));
        }

        /// <summary>
        /// 1.5 获取最新资源包
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetLatestResource([FromBody]UserModel userModel)
        {
            if (!userModel.OtherBookID.HasValue)
            {
                return KingsunResponse.GetSyncErrorResult("没有获取到课本信息");
            }
            Custom_MODBook modBook = new PracticeEveryDayBLL().GetBookInfoFromMOD(userModel.OtherBookID.Value);
            if (modBook == null)
            {
                return KingsunResponse.GetSyncErrorResult("没有获取到教材对应的基础信息");
            }
            QTb_Resource res = new ResourceBLL().GetResource(modBook.EditionID, modBook.GradeID, modBook.BookReel);
            if (res != null)
            {
                return KingsunResponse.GetSyncResult(new { ResUrl = res.ResUrl, ResMD5 = res.ResMD5, ResVersion = res.ResVersion });
            }
            else
            {
                return KingsunResponse.GetSyncErrorResult("无可用的资源包");
            }
        }
        /// <summary>
        /// 1.6 判断并获取有更新的资源包
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUpdatedResource([FromBody]UserModel userModel)
        {
            if (!userModel.OtherBookID.HasValue)
            {
                return KingsunResponse.GetSyncErrorResult("没有获取到课本信息");
            }
            if (string.IsNullOrEmpty(userModel.ResVersion))
            {
                return KingsunResponse.GetSyncErrorResult("没有获取到资源包版本信息");
            }
            Custom_MODBook modBook = new PracticeEveryDayBLL().GetBookInfoFromMOD(userModel.OtherBookID.Value);
            if (modBook == null)
            {
                return KingsunResponse.GetSyncErrorResult("没有获取到教材对应的基础信息");
            }
            QTb_Resource res = new ResourceBLL().GetResource(modBook.EditionID, modBook.GradeID, modBook.BookReel);
            if (res != null && res.ResVersion != userModel.ResVersion)
            {
                return KingsunResponse.GetSyncResult(new { ResUrl = res.ResUrl, ResMD5 = res.ResMD5, ResVersion = res.ResVersion });
            }
            else
            {
                return KingsunResponse.GetSyncErrorResult("已是最新资源包");
            }
        }

        /// <summary>
        /// 上传音频
        /// </summary>
        /// <param name="saModel"></param>
        /// <returns></returns>
        public HttpResponseMessage UploadAudios([FromBody]StuAnswerModel saModel)
        {
            if (string.IsNullOrEmpty(saModel.StudentID))
            {
                return KingsunResponse.GetErrorResult("没有获取到学生信息哦！");
            }
            if (saModel.UrlList == null || saModel.UrlList.Count == 0)
            {
                return KingsunResponse.GetErrorResult("没有获取到要保存的录音哦！");
            }
            IList<string> listUrl = new List<string>();
            foreach (string strUrl in saModel.UrlList)
            {
                listUrl.Add(Common.UploadAudio.UploadAudioFile(strUrl, saModel.StudentID, 1));
            }
            if (listUrl.Count > 0)
            {
                return KingsunResponse.GetResult(listUrl);
            }
            else
            {
                return KingsunResponse.GetErrorResult("保存失败！");
            }
        }

        /// <summary>
        /// 上传音频（安卓端）
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AndroidUploadAudios([FromBody]AndroidModel read)
        {
            string test = read.FormData;
            StuAnswerModel t = Kingsun.SynchronousStudy.Common.JsonHelper.DecodeJson<StuAnswerModel>(test);
            return UploadAudios(t);
        }

        /// <summary>
        /// 学生提交答案模板
        /// </summary>
        public class StuAnswerModel
        {
            public string StudentID { get; set; }
            public string StuTaskID { get; set; }
            public string ParentID { get; set; }
            public string QuestionID { get; set; }
            public decimal SpendTime { get; set; }
            public string StuAnswer { get; set; }
            public int IsRight { get; set; }
            public int Score { get; set; }
            public int Award { get; set; }
            public string AnswerSystem { get; set; }
            public IList<string> UrlList { get; set; }
            public IList<SplitAnswer> SplitAnswerList { get; set; }
            public IList<StuAnswer> StuAnswerList { get; set; }
        }

        /// <summary>
        /// 保存拆分题型答案
        /// </summary>
        public class SplitAnswer
        {
            public string QuestionID { get; set; }
            public string ParentID { get; set; }
            public int IsRight { get; set; }
        }
        /// <summary>
        /// 保存一屏多题的小题答案
        /// </summary>
        public class StuAnswer
        {
            public string QuestionID { get; set; }
            public string Answer { get; set; }
            public int IsRight { get; set; }
        }

    }
}
