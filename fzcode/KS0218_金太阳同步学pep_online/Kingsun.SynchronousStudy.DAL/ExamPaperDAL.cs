using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.IBSLearnReport;

namespace Kingsun.SynchronousStudy.DAL
{
    public class ExamPaperDAL
    {
        static RedisHashHelper hashRedis = new RedisHashHelper();
        readonly BaseManagement _bm = new BaseManagement();
        readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];
        private int ModelType = 2;//redis数据类型(1:趣配音,2:单元测试，3：说说看)
        private int Subject = 3;//学科（3：英语）


        /// <summary>
        /// 获取班级期末测评卷做题人数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookId">书籍ID</param>
        /// <param name="classShortId">班级ID</param>
        /// <param name="pageNumber">页码</param>
        /// <returns></returns>
        public List<ExamPaperModel> GetExamPaperList_bak(string bookId, string sbclass, int ClassNum)
        {
            try
            {
                string sql = string.Format(@"
                    select 
	                    a.CatalogID,a.CatalogName,COUNT(*) QuestionNum,{2} ClassNum
                    from FZ_ExamPaper.dbo.QTb_Catalog a 
                    left join FZ_ExamPaper.dbo.Tb_StuCatalog b on a.CatalogID=b.CatalogID 
                    where a.BookID={0} and a.ParentID <> 0 and StuID IN ({1}) 
                    group by a.CatalogID,a.CatalogName", bookId, sbclass.ToString(), ClassNum);
                DataSet dsExam = SqlHelper.ExecuteDataset(SqlHelper.ExamConnectionString, CommandType.Text, sql);
                List<ExamPaperModel> examList = JsonHelper.DataSetToIList<ExamPaperModel>(dsExam, 0);
                return examList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取班级期末测评卷做题人数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookId">书籍ID</param>
        /// <param name="classShortId">班级ID</param>
        /// <param name="pageNumber">页码</param>
        /// <returns></returns>
        public List<ExamPaperModel> GetExamPaperList(string bookId, IBS_ClassUserRelation classinfo, int ClassNum, string classId)
        {
            string sql = string.Format(@" SELECT CatalogID,CatalogName FROM QTb_Catalog WHERE BookID='{0}' AND ParentID<>0", bookId);
            DataSet dsExam = SqlHelper.ExecuteDataset(SqlHelper.ExamConnectionString, CommandType.Text, sql);
            List<ExamPaperModel> examList = JsonHelper.DataSetToIList<ExamPaperModel>(dsExam, 0);
            List<ExamPaperModel> exaModel = new List<ExamPaperModel>();
            using (var Redis = RedisManager.GetClient(0))
            {
                foreach (var item in examList)
                {
                    int cl = 0;
                    try
                    {
                        foreach (var stu in classinfo.ClassStuList)
                        {
                            string value = Redis.GetValueFromHash("Rds_StudyReport_ModuleTitle_" + stu.StuID.ToString().Substring(0, 2), stu.StuID + "_" + Subject + "_" + ModelType);
                            if (!string.IsNullOrEmpty(value))
                            {
                                Rds_StudyReport_ModuleTitle study = JsonHelper.DecodeJson<Rds_StudyReport_ModuleTitle>(value);
                                foreach (var userinfo in study.detail)
                                {
                                    if (userinfo.BookID == bookId.ToInt() && userinfo.VideoNumber == item.CatalogID)
                                    {
                                        cl++;
                                    }
                                }
                            }

                        }
                        ExamPaperModel vlist = new ExamPaperModel()
                          {
                              ClassNum = ClassNum,
                              QuestionNum = cl,
                              CatalogID = Convert.ToInt32(item.CatalogID),
                              CatalogName = item.CatalogName,
                          };
                        exaModel.Add(vlist);

                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + item.CatalogID);
                    }
                }
                return exaModel;

            }
        }

        /// <summary>
        /// 获取班级期末测评成绩单
        /// </summary>
        /// <param name="request"></param>
        /// <param name="catalogId">测评卷ID</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        public HttpResponseMessage GetUserExamInfo(string classId, string catalogId, int pageNumber, IBS_ClassUserRelation userClass)
        {

            List<UserExamInfo> userexaminfo = new List<UserExamInfo>();
            UserExamInfo uvi;
            double count = 0;
            double maxScore = 0;
            double minScore = 100;
            int cl = 0;
            string sql = string.Format(@"  SELECT BookID FROM QTb_Catalog WHERE CatalogID='{0}'", catalogId);
            DataSet dsExam = SqlHelper.ExecuteDataset(SqlHelper.ExamConnectionString, CommandType.Text, sql);
            if (dsExam.Tables.Count > 0)
            {

                if (userClass != null)
                {
                    foreach (var item in userClass.ClassStuList)
                    {
                        Rds_StudyReport_ModuleTitle module = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + item.StuID.ToString().Substring(0, 2), item.StuID + "_" + Subject + "_" + ModelType);

                        uvi = new UserExamInfo();
                        if (module != null)
                        {
                            Rds_StudyReport_BookDetail rdsBookDetail = module.detail.FirstOrDefault(i => i.BookID == Convert.ToInt32(dsExam.Tables[0].Rows[0]["BookID"].ToString())&& i.VideoNumber == catalogId.ToInt());
                            if (rdsBookDetail != null)
                            {
                                cl++;
                                if (Convert.ToDouble(rdsBookDetail.BestScore) > maxScore)
                                {
                                    maxScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                                }
                                if (Convert.ToDouble(rdsBookDetail.BestScore) <= minScore)
                                {
                                    minScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                                }
                                count += rdsBookDetail.BestScore.CutDoubleWithN(1);

                                string imgUrl = item.IsEnableOss != 0 ? _getOssFilesUrl + item.UserImage : _getFilesUrl + "?FileID=" + item.UserImage;
                                uvi.UserId = module.UserID.ToString(); //返回UserId
                                uvi.UserImg = imgUrl;
                                uvi.UserName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                                uvi.ShowName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                                uvi.AnswerNum = rdsBookDetail.DubbingNum;
                                uvi.StuCatID = rdsBookDetail.VideoID;
                                uvi.IsStudy = true;
                                uvi.CreateTime = rdsBookDetail.CreateTime;
                                uvi.Score = rdsBookDetail.BestScore.CutDoubleWithN(1);
                            }
                            else
                            {
                                uvi.UserId = item.StuID.ToString();
                                uvi.UserImg = item.IsEnableOss != 0 ? _getOssFilesUrl + item.UserImage : _getFilesUrl + "?FileID=" + item.UserImage;
                                uvi.UserName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                                uvi.ShowName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                                uvi.Score = 0.00;
                                uvi.IsStudy = false;
                            }
                        }
                        else
                        {
                            uvi.UserId = item.StuID.ToString();
                            uvi.UserImg = string.IsNullOrEmpty(item.UserImage)? "00000000-0000-0000-0000-000000000000":item.UserImage;
                            uvi.UserName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                            uvi.ShowName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                            uvi.Score = 0.00;
                            uvi.IsStudy = false;
                        }

                        userexaminfo.Add(uvi);
                    }
                }
            }
            if (cl == 0)
            {
                minScore = 0;
            }
            //以时间为单位，降序排列
            userexaminfo = userexaminfo.OrderByDescending(i => i.IsStudy).ThenByDescending(i => i.Score).Skip(pageNumber * 10).Take(10).ToList();
            object obj =
                new
                {
                    AverageScore = count <= 0 ? "0" : (count / cl).ToString("0.0"),
                    HighestScore = Convert.ToDouble(maxScore.ToString("0.0")),
                    LowestScore = Convert.ToDouble(minScore.ToString("0.0")),
                    Students = userexaminfo
                };
            return JsonHelper.GetResult(obj, "操作成功");//返回信息 
        }

        /// <summary>
        /// 获取班级期末测评成绩单
        /// </summary>
        /// <param name="request"></param>
        /// <param name="catalogId">测评卷ID</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        public HttpResponseMessage GetUserExamInfo_bak(string classId, string catalogId, int pageNumber, List<UserClass> userClass)
        {
            List<UInfo> stulist = GetStuListByClassShortId(classId, userClass);//匹配学生
            if (stulist == null || stulist.Count == 0) { return JsonHelper.GetErrorResult("该班级没有学生"); }

            string sql = string.Format(@"SELECT  StuCatID,StuID,CatalogID,BestTotalScore,DoDate,AnswerNum FROM [Tb_StuCatalog] WHERE CatalogID ='{0}'", catalogId);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ExamConnectionString, CommandType.Text, sql);
            List<Tb_StuCatalog> ts = JsonHelper.DataSetToIList<Tb_StuCatalog>(ds, 0);
            List<UserExamInfo> userexaminfo = new List<UserExamInfo>();
            UserExamInfo uvi;
            double count = 0;
            double maxScore = 0;
            double minScore = 100;
            int cl = 0;

            foreach (var item in stulist)
            {
                uvi = new UserExamInfo();
                if (ts != null)
                {
                    IList<Tb_StuCatalog> uslist = ts.OrderBy(i => i.DoDate).Where(i => i.StuID.ToString() == item.UserID).Take(1).ToList();
                    if (uslist.Count == 0)
                    {
                        uvi.UserId = item.UserID;
                        uvi.UserImg = string.IsNullOrEmpty(item.UserImg)? "00000000-0000-0000-0000-000000000000":item.UserImg;
                        uvi.UserName = string.IsNullOrEmpty(item.UserName) ? "暂未填写" : item.UserName;
                        uvi.ShowName = string.IsNullOrEmpty(item.TrueName) ? "暂未填写" : item.TrueName;
                        uvi.Score = 0.00;
                        uvi.IsStudy = false;
                    }
                    else
                    {
                        cl++;
                        foreach (var usItem in uslist)
                        {
                            if (Convert.ToDouble(usItem.BestTotalScore) > maxScore)
                            {
                                maxScore = Convert.ToDouble(usItem.BestTotalScore.ToString("0.0"));
                            }
                            if (Convert.ToDouble(usItem.BestTotalScore) <= minScore)
                            {
                                minScore = Convert.ToDouble(usItem.BestTotalScore.ToString("0.0"));
                            }
                            count += Convert.ToDouble(usItem.BestTotalScore.ToString("0.0"));

                            string imgUrl = string.IsNullOrEmpty(item.UserImg)
                                ? "00000000-0000-0000-0000-000000000000"
                                : item.UserImg;
                            uvi.UserId = usItem.StuID;//返回UserId
                            uvi.UserImg = imgUrl;
                            uvi.UserName = string.IsNullOrEmpty(item.UserName) ? "暂未填写" : item.UserName;
                            uvi.ShowName = string.IsNullOrEmpty(item.TrueName) ? "暂未填写" : item.TrueName;
                            uvi.AnswerNum = usItem.AnswerNum;
                            uvi.StuCatID = usItem.StuCatID;
                            uvi.IsStudy = true;
                            uvi.CreateTime = usItem.DoDate.ToString("yyyy.MM.dd");
                            uvi.Score = Convert.ToDouble(usItem.BestTotalScore.ToString("0.0"));
                        }
                    }
                }
                else
                {
                    uvi.UserId = item.UserID;
                    uvi.UserImg = string.IsNullOrEmpty(item.UserImg)
                        ? "00000000-0000-0000-0000-000000000000"
                        : item.UserImg;
                    uvi.UserName = string.IsNullOrEmpty(item.UserName)?"暂未填写":item.UserName;
                    uvi.ShowName = string.IsNullOrEmpty(item.TrueName) ? "暂未填写" : item.TrueName;
                    uvi.Score = 0.00;
                    uvi.IsStudy = false;
                }

                userexaminfo.Add(uvi);
            }
            if (cl == 0)
            {
                minScore = 0;
            }
            //以时间为单位，降序排列
            userexaminfo = userexaminfo.OrderByDescending(i => i.IsStudy).ThenByDescending(i => i.Score).Skip(pageNumber * 10).Take(10).ToList();
            object obj =
                new
                {
                    AverageScore = count <= 0 ? "0" : (count / cl).ToString("0.0"),
                    HighestScore = maxScore,
                    LowestScore = minScore,
                    Students = userexaminfo
                };
            return JsonHelper.GetResult(obj, "操作成功");//返回信息 
        }

        /// <summary>
        /// 通过班级ID查询班级下的所有学生
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        private List<UInfo> GetStuListByClassShortId(string classId, List<UserClass> userClass)
        {
            List<UInfo> stuList = new List<UInfo>();
            string sql = string.Format(@" SELECT b.TrueName ,
                                                b.UserName ,
                                                b.UserID ,
                                                b.NickName ,
                                                b.UserImage,
                                                b.IsEnableOss
                                                from ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo b where b.IsUser=1 ");

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UInfo ui = new UInfo
                    {
                        UserName = ds.Tables[0].Rows[i]["UserName"].ToString(),
                        UserID = ds.Tables[0].Rows[i]["UserID"].ToString(),
                        UserImg = ds.Tables[0].Rows[i]["IsEnableOss"].ToString() != "0" ? _getOssFilesUrl + ds.Tables[0].Rows[i]["UserImage"] : _getFilesUrl + "?FileID=" + ds.Tables[0].Rows[i]["UserImage"]
                    };
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TrueName"].ToString()))
                    {
                        ui.TrueName = ds.Tables[0].Rows[i]["TrueName"].ToString();
                    }
                    else
                    {
                        ui.TrueName = "暂未填写";
                    }

                    ui.IsEnableOss = Convert.ToInt32(ds.Tables[0].Rows[i]["IsEnableOss"].ToString());
                    stuList.Add(ui);
                }
            }
            if (userClass.Count > 0)
            {
                stuList = (from u in userClass
                           join b in stuList on u.UserID.ToString() equals b.UserID into userid
                           from b in userid.DefaultIfEmpty()
                           where u.UserID == Convert.ToInt32(b.UserID)
                           select new UInfo
                           {
                               UserID = u.UserID.ToString(),
                               IsEnableOss = b == null ? 0 : b.IsEnableOss,
                               TrueName = b == null ? "" : b.TrueName,
                               UserImg = b == null ? "" : b.UserImg,
                               UserName = b == null ? "" : b.UserName
                           }).ToList<UInfo>();
            }

            return stuList.OrderBy(i => i.UserID).ToList();
        }
    }
    public class UserIDModel
    {
        public string UserID { get; set; }
    }

    public class ExamPaperModel
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
        public int QuestionNum { get; set; }

        public int ClassNum { get; set; }
    }
}
