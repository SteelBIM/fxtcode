using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Framework.Redis;
using CBSS.IBS.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using CBSS.UserOrder.Contract.DataModel;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        /// <summary>
        /// 初中英语学习报告入库
        /// </summary>
        public void JuniorEnglishReport2DB()
        {
            var listCount = redisList.Count("StudyReport");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            for (int i = 0; i < Count; i++)
            {
                var model = redisList.RemoveStartFromList("StudyReport");
                try
                {
                    KeyAndType keyAndType = model.FromJson<KeyAndType>();
                    switch (keyAndType.Type)
                    {
                        case TypeOfReport.WordRead:
                            WordRead2DB(keyAndType.key);
                            break;
                        case TypeOfReport.WordDictation:
                            WordDictation2DB(keyAndType.key);
                            break;
                        case TypeOfReport.ArticleRead:
                            ArticleRead2DB(keyAndType.key);
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "学习报告入库异常,异常数据：" + model, ex);
                }
            }
        }


        public void JuniorEnglishSpokenRecord2DB()
        {
            var listCount = redisList.Count("UserSpokenPaperRecord");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            for (int i = 0; i < Count; i++)
            {
                var model = redisList.RemoveStartFromList("UserSpokenPaperRecord");
                try
                {
                    Rds_UserTopicSetAnswerModel topicset = model.FromJson<Rds_UserTopicSetAnswerModel>();
                    TopicSet2DB(topicset);

                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "学习报告入库异常,异常数据：" + model, ex);
                }
            }
        }

        private void TopicSet2DB(Rds_UserTopicSetAnswerModel topicset)
        {
            if (topicset != null)
            {
                var dbrecord = tbxRecordRepository
                    .SelectSearch<UserSpokenPaperRecord>(a =>
                        a.StuID == topicset.studentId && a.PaperID == topicset.id).FirstOrDefault();
                if (dbrecord != null)
                {
                    if (topicset.stuScore >= dbrecord.StuScore)
                    {
                        dbrecord.StuScore = topicset.stuScore.Value;
                        dbrecord.Score = topicset.score.Value;
                        dbrecord.StuAnswer = topicset.ToJson();
                        tbxRecordRepository.Update(dbrecord);
                    }
                }
                else
                {
                    dbrecord = new UserSpokenPaperRecord();
                    dbrecord.ID = Guid.NewGuid();
                    dbrecord.PaperID = topicset.id;
                    dbrecord.StuID = topicset.studentId;
                    dbrecord.StuScore = topicset.stuScore.Value;
                    dbrecord.Score = topicset.score.Value;
                    dbrecord.StuAnswer = topicset.ToJson();
                    tbxRecordRepository.Insert(dbrecord);
                }
            }
        }


        /// <summary>
        ///课文朗读入库
        /// </summary>
        private void ArticleRead2DB(string key)
        {
            List<Rds_UserArticleReadRecord> articleRead = redis.Get<List<Rds_UserArticleReadRecord>>("Rds_UserArticleReadRecord_" + key.Substring(0, 2), key);
            if (articleRead != null)
            {
                articleRead.ForEach(a =>
                {
                    var dbrecord = tbxRecordRepository
                     .SelectSearch<UserArticleReadRecord>(x =>
                         x.UserID == a.UserID && x.CatalogID == a.CatalogID && x.ModuleID == a.CatalogID).FirstOrDefault();
                    if (dbrecord != null)
                    {
                        if (dbrecord.AvgScore <= a.AvgScore)
                        {
                            tbxRecordRepository.Delete<UserSentenceRecordItem>(x =>
                                x.UserArticleReadRecordID.Equals(dbrecord.UserArticleReadRecordID));
                            dbrecord.UserID = key.ToInt();
                            dbrecord.AnswerSoundUrl = a.AnswerSoundUrl;
                            dbrecord.AvgScore = a.AvgScore;
                            dbrecord.CatalogID = a.CatalogID;
                            dbrecord.Completeness = a.Completeness;
                            dbrecord.CorrectRate = a.CorrectRate;
                            dbrecord.DoDate = a.DoDate;
                            dbrecord.Fluency = a.Fluency;
                            dbrecord.ModuleID = a.ModuleID;
                            dbrecord.Sort = a.Sort;
                            dbrecord.UserArticleReadRecordID = Guid.NewGuid();
                            List<UserSentenceRecordItem> listsentence = new List<UserSentenceRecordItem>();
                            a.Sentences.ForEach(xy =>
                            {
                                UserSentenceRecordItem sentence = new UserSentenceRecordItem();
                                sentence.ID = Guid.NewGuid();
                                sentence.Answer = xy.Answer;
                                sentence.Score = xy.Score;
                                sentence.Text = xy.Text;
                                sentence.UserArticleReadRecordID = dbrecord.UserArticleReadRecordID;
                                listsentence.Add(sentence);
                            });
                            if (listsentence.Count > 0)
                            {
                                tbxRecordRepository.InsertBatch(listsentence);
                            }

                            tbxRecordRepository.Update(dbrecord);
                        }
                    }
                    else
                    {
                        dbrecord = new UserArticleReadRecord();
                        dbrecord.UserID = a.UserID;
                        dbrecord.AnswerSoundUrl = a.AnswerSoundUrl;
                        dbrecord.AvgScore = a.AvgScore;
                        dbrecord.CatalogID = a.CatalogID;
                        dbrecord.Completeness = a.Completeness;
                        dbrecord.CorrectRate = a.CorrectRate;
                        dbrecord.Sort = a.Sort;
                        dbrecord.DoDate = a.DoDate;
                        dbrecord.Fluency = a.Fluency;
                        dbrecord.ModuleID = a.ModuleID;
                        //    dbrecord.StressedReadRate = articleRead.StressedReadRate;
                        dbrecord.UserArticleReadRecordID = Guid.NewGuid();
                        List<UserSentenceRecordItem> listsentence = new List<UserSentenceRecordItem>();
                        a.Sentences.ForEach(ax =>
                        {
                            UserSentenceRecordItem sentence = new UserSentenceRecordItem();
                            sentence.ID = Guid.NewGuid();
                            sentence.Answer = ax.Answer;
                            sentence.Score = ax.Score;
                            sentence.Text = ax.Text;
                            sentence.UserArticleReadRecordID = dbrecord.UserArticleReadRecordID;
                            listsentence.Add(sentence);
                        });
                        if (listsentence.Count > 0)
                        {
                            tbxRecordRepository.InsertBatch(listsentence);
                        }

                        tbxRecordRepository.Insert(dbrecord);


                    }
                });
              
               


            }
        }
        /// <summary>
        /// 单词听写入库
        /// </summary>
        private void WordDictation2DB(string key)
        {
            Rds_UserWordDictationRecord wordDictation = redis.Get<Rds_UserWordDictationRecord>("Rds_UserWordDictationRecord_"+key.Substring(0,2), key);
            if (wordDictation != null)
            {
                var dbRecord = tbxRecordRepository.SelectSearch<UserWordDictationRecord>(a =>
                   a.UserID == wordDictation.UserID && a.CatalogID == wordDictation.CatalogID && a.ModuleID == wordDictation.ModuleID).FirstOrDefault();
                if (dbRecord != null)
                {
                    if (wordDictation.Score >= dbRecord.Score)
                    {
                        tbxRecordRepository.Delete<UserWordDictationRecordItem>(a =>
                           a.UserWordDictationRecordID.Equals(dbRecord.UserWordDictationRecordID));
                        dbRecord.UserID = wordDictation.UserID;
                        dbRecord.CatalogID = wordDictation.CatalogID;
                        dbRecord.DoDate = wordDictation.DoDate;
                        dbRecord.ModuleID = wordDictation.ModuleID;
                        dbRecord.RightCount = wordDictation.RightCount;
                        dbRecord.Score = wordDictation.Score;
                        dbRecord.UserWordDictationRecordID = Guid.NewGuid();
                        dbRecord.WordsCount = wordDictation.WordsCount;
                        List<UserWordDictationRecordItem> list = new List<UserWordDictationRecordItem>();
                        wordDictation.Words.ForEach(a =>
                        {
                            UserWordDictationRecordItem item = new UserWordDictationRecordItem();
                            item.ID = Guid.NewGuid();
                            item.Answer = a.Answer;
                            item.IsRight = a.IsRight;
                            item.Text = a.Text;
                            item.UserWordDictationRecordID = dbRecord.UserWordDictationRecordID;
                            list.Add(item);
                        });
                        if (list.Count > 0)
                        {
                            tbxRecordRepository.InsertBatch(list);
                        }
                        tbxRecordRepository.Update(dbRecord);
                    }
                }
                else
                {
                    dbRecord = new UserWordDictationRecord();
                    dbRecord.UserID = wordDictation.UserID;
                    dbRecord.CatalogID = wordDictation.CatalogID;
                    dbRecord.DoDate = wordDictation.DoDate;
                    dbRecord.ModuleID = wordDictation.ModuleID;
                    dbRecord.RightCount = wordDictation.RightCount;
                    dbRecord.Score = wordDictation.Score;
                    dbRecord.UserWordDictationRecordID = Guid.NewGuid();
                    dbRecord.WordsCount = wordDictation.WordsCount;
                    List<UserWordDictationRecordItem> list = new List<UserWordDictationRecordItem>();
                    wordDictation.Words.ForEach(a =>
                    {
                        UserWordDictationRecordItem item = new UserWordDictationRecordItem();
                        item.ID = Guid.NewGuid();
                        item.Answer = a.Answer;
                        item.IsRight = a.IsRight;
                        item.Text = a.Text;
                        item.UserWordDictationRecordID = dbRecord.UserWordDictationRecordID;
                        list.Add(item);
                    });
                    if (list.Count > 0)
                    {
                        tbxRecordRepository.InsertBatch(list);
                    }
                    tbxRecordRepository.Insert(dbRecord);
                }

            }
        }
        /// <summary>
        /// 单词跟读入库
        /// </summary>
        private void WordRead2DB(string key)
        {
            Rds_UserWordReadRecord wordRead = redis.Get<Rds_UserWordReadRecord>("Rds_UserWordReadRecord_"+key.Substring(0,2), key);
            if (wordRead != null)
            {
                var dbrecord = tbxRecordRepository
                    .SelectSearch<UserWordReadRecord>(a => a.UserID == wordRead.UserID && a.CatalogID == wordRead.CatalogID && a.ModuleID == wordRead.ModuleID)
                    .FirstOrDefault();
                if (dbrecord != null)
                {
                    if (dbrecord.AvgScore <= wordRead.AvgScore)
                    {
                       tbxRecordRepository.Delete<UserWordReadRecordItem>(x =>
                            x.UserWordReadRecordID.Equals(dbrecord.UserWordReadRecordID));
                        dbrecord.UserID = wordRead.UserID;
                        dbrecord.AvgScore = wordRead.AvgScore;
                        dbrecord.CatalogID = wordRead.CatalogID;
                        dbrecord.DoDate = wordRead.DoDate;
                        dbrecord.ModuleID = wordRead.ModuleID;
                        dbrecord.UserWordReadRecordID = Guid.NewGuid();
                        List<UserWordReadRecordItem> list = new List<UserWordReadRecordItem>();
                        wordRead.Words.ForEach(a =>
                        {
                            UserWordReadRecordItem item = new UserWordReadRecordItem();
                            item.Answer = a.Answer;
                            item.Text = a.Text;
                            item.Score = a.Score;
                            item.AnswerSoundUrl = a.AnswerSoundUrl;
                            item.ID = Guid.NewGuid();
                            item.UserWordReadRecordID = dbrecord.UserWordReadRecordID;
                            list.Add(item);
                        });
                        if (list.Count > 0)
                        {
                            tbxRecordRepository.InsertBatch(list);
                        }
                        tbxRecordRepository.Update(dbrecord);
                    }
                }
                else
                {
                    dbrecord = new UserWordReadRecord();
                    dbrecord.UserID = wordRead.UserID;
                    dbrecord.AvgScore = wordRead.AvgScore;
                    dbrecord.CatalogID = wordRead.CatalogID;
                    dbrecord.DoDate = wordRead.DoDate;
                    dbrecord.ModuleID = wordRead.ModuleID;
                    dbrecord.UserWordReadRecordID = Guid.NewGuid();
                    List<UserWordReadRecordItem> list = new List<UserWordReadRecordItem>();
                    wordRead.Words.ForEach(a =>
                    {
                        UserWordReadRecordItem item = new UserWordReadRecordItem();
                        item.Answer = a.Answer;
                        item.Text = a.Text;
                        item.Score = a.Score;
                        item.AnswerSoundUrl = a.AnswerSoundUrl;
                        item.ID = Guid.NewGuid();
                        item.UserWordReadRecordID = dbrecord.UserWordReadRecordID;
                        list.Add(item);
                    });
                    if (list.Count > 0)
                    {
                        tbxRecordRepository.InsertBatch(list);
                    }
                    tbxRecordRepository.Insert(dbrecord);
                }


            }
        }



        public void OrderInfo2YXDB() 
        {
           /* var listCount = redisList.Count("CBSSUserPayOrder");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            for (int i = 0; i < Count; i++)
            {
                var model = redisList.RemoveStartFromList("CBSSUserPayOrder");
                try
                {
                    UserPayOrder order = model.FromJson<UserPayOrder>();
                    if (order != null)
                    {
                        switch (order.Type)
                        {
                            case 1:
                                InsertOrder2YX(order);
                                break;
                            case 2:
                                UpdateOrder2YX(order);
                                break;
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "学习报告入库异常,异常数据：" + model, ex);
                }
            }*/
        }


        public void ExecuteLearningReport()
        {
            var listCount = redisList.Count("LearningReportQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            using (var Redis = RedisManager.GetClient(0, "Tbx"))
            {
                for (int i = 0; i < Count; i++)
                {
                    var model = Redis.RemoveStartFromList("LearningReportQueue");
                    try
                    {
                        Rds_RedisVideoInfo data = model.FromJson<Rds_RedisVideoInfo>();
                        switch ((ModuleTypeEnum)data.ModuleType.ToInt())
                        {
                            case ModuleTypeEnum.InterestDebbing:
                                ExecuteInterestDebbingLearningReport(data);
                                break;
                            case ModuleTypeEnum.ExamPaper:
                                ExecuteExampaperLearningReport(data);
                                break;
                            case ModuleTypeEnum.HearResources:
                                ExecuteHearResourcesLearningReport(data);
                                break;

                            //优学
                            case ModuleTypeEnum.YXInterestDebbing:
                                ExecuteInterestDebbingLearningReport(data, 4);
                                break;
                            case ModuleTypeEnum.YXExamPaper:
                                ExecuteExampaperLearningReport(data, 5);
                                break;
                            case ModuleTypeEnum.YXHearResources:
                                ExecuteHearResourcesLearningReport(data, 6);
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        Log4NetHelper.Error(LoggerType.FsExceptionLog, "学习报告入库异常,异常数据：" + model, ex);
                    }
                }
            }
        }


        #region 班级变更后学习报告跟随变更

        /// <summary>
        /// 学生关系变更后，学习报告数据变更
        /// </summary>
        public void ExecuteStudentClassChangeLearningReport()
        {
            var listCount = redis.Count("StudentClassRelationKey");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            using (var Redis = RedisManager.GetClient(0,"Tbx"))
            {
                for (int i = 0; i < Count; i++)
                {
                    var model = Redis.RemoveStartFromList("StudentClassRelationKey");
                    try
                    {
                        if (!string.IsNullOrEmpty(model))
                        {
                            StudentClassRelationKey data = model.FromJson<StudentClassRelationKey>();

                            switch (data.type)
                            {

                                case 1: //绑定
                                    AddNewLearningReport(data);
                                    break;
                                case 2: //解绑
                                    CheckAndUpdateLearningReport(data);
                                    break;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetHelper.Error(LoggerType.FsExceptionLog, "班级变更的学习报告变更失败！data="+model, ex);
                    }
                }
            }

        }

        /// <summary>
        /// 查询原班级是否有学习报告  以及转移学生是否为班级最后一份学习报告
        /// </summary>
        /// <param name="data"></param>
        private void CheckAndUpdateLearningReport(StudentClassRelationKey data)
        {

            //通过原班级ID查询原班级现有学生
            var classinfo = ibsService.GetClassUserRelationByClassId(data.ClassID);
            if (classinfo != null)
            {
                data.ClassID = classinfo.ClassNum;
                bool InterestDebbingState = false;
                bool ExamPaperState = false;
                bool HearResourcesState = false;
                //遍历学生查询学生是否有学习报告
                classinfo.ClassStuList.ForEach(a =>
                {

                    var InterestRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + a.StuID.ToString().Substring(0, 2),
                     a.StuID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
                    //若原班级有学生有学习报告则无需处理
                    if (InterestRecord != null)
                    {
                        InterestDebbingState = true;
                    }

                    //若原班级有学生有学习报告则无需处理
                    var ExamRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + a.StuID.ToString().Substring(0, 2),
                     a.StuID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.ExamPaper);
                    if (ExamRecord != null)
                    {
                        ExamPaperState = true;
                    }

                    var HearResources = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + a.StuID.ToString().Substring(0, 2),
                        a.StuID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.HearResources);
                    //若原班级有学生有学习报告则无需处理
                    if (HearResources != null)
                    {
                        HearResourcesState = true;
                    }

                });

                //如果原班级无人产生说说看报告则删除转移学生产生的记录
                if (!HearResourcesState)
                {
                    var hearResources = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                        data.UserID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.HearResources);
                    if (hearResources != null)
                    {
                        hearResources.detail.ForEach(x =>
                        {
                            redis.Remove("Rds_StudyReport_Book", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.HearResources + "_" + x.BookID);
                            if (x.SecondTitleID > 0)
                            {
                                redis.Remove("Rds_StudyReport_Module",
                                    data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.HearResources + "_" + x.BookID + "_" +
                                    x.FirstTitleID + x.SecondTitleID);
                            }
                            else
                            {
                                redis.Remove("Rds_StudyReport_Module",
                                    data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.HearResources + "_" + x.BookID + "_" +
                                    x.FirstTitleID);
                            }

                        });

                    }
                    redis.Remove("Rds_StudyReport_Class", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.HearResources);

                }

                //如果原班级无人产生报告则删除转移学生产生的记录
                if (!InterestDebbingState)
                {
                    var learningRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                    data.UserID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
                    if (learningRecord != null)
                    {
                        learningRecord.detail.ForEach(x =>
                        {
                            redis.Remove("Rds_StudyReport_Book", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID);
                            if (x.SecondTitleID > 0)
                            {
                                redis.Remove("Rds_StudyReport_Module",
                            data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID + "_" +
                            x.FirstTitleID + x.SecondTitleID);
                            }
                            else
                            {
                                redis.Remove("Rds_StudyReport_Module",
                           data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID + "_" +
                           x.FirstTitleID);
                            }

                        });

                    }
                    redis.Remove("Rds_StudyReport_Class", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);

                }

                //如果原班级无人产生报告则删除转移学生产生的记录
                if (!ExamPaperState)
                {
                    redis.Remove("Rds_StudyReport_Class", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.ExamPaper);
                    var learningRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                   data.UserID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
                    if (learningRecord != null)
                    {
                        learningRecord.detail.ForEach(x =>
                        {
                            redis.Remove("Rds_StudyReport_Book", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID);

                        });

                    }
                }
            }
        }



        /// <summary>
        /// 班级变更后，新增学习报告记录
        /// </summary>
        /// <param name="data"></param>
        private void AddNewLearningReport(StudentClassRelationKey data)
        {
            var classinfo = ibsService.GetClassUserRelationByClassId(data.ClassID);
            if (classinfo != null)
            {
                data.ClassID = classinfo.ClassNum;
            }
            else
            {
                return;
            }

            #region 趣配音
            var learningRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                       data.UserID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
            if (learningRecord != null)
            {
                if (learningRecord.detail.Count > 0)
                {
                    Rds_RedisVideoInfo info = new Rds_RedisVideoInfo();
                    info.ClassID = data.ClassID;
                    #region 修改班级报告
                    //更新或新增班级报告
                    CreateOrUpdateRds_StudyReport_Class(info, (int)ModuleTypeEnum.InterestDebbing);
                    #endregion

                    learningRecord.detail.ForEach(a =>
                    {

                        info.BookId = a.BookID.ToString();
                        info.VideoNumber = a.VideoNumber.ToString();
                        #region 修改书本报告
                        //更新或新增书本报告
                        CreateOrUpdateRds_StudyReport_Book(info, (int)ModuleTypeEnum.InterestDebbing);
                        #endregion

                        #region 修改模块报告


                        var title = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + a.BookID,
                                a.FirstTitleID + "_" + a.SecondTitleID);
                        if (title != null)
                        {
                            info.FirstTitle = title.FirstTitle;
                            info.SecondTitle = title.SecondTitle;
                            info.FirstModular = title.FirstModular;
                        }


                        info.FirstTitleID = a.FirstTitleID > 0 ? a.FirstTitleID.ToString() : "";
                        info.FirstModularID = a.FirstModularID > 0 ? a.FirstModularID.ToString() : "";
                        info.SecondTitleID = a.SecondTitleID > 0 ? a.SecondTitleID.ToString() : "";
                        CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(info, (int)ModuleTypeEnum.InterestDebbing);
                        #endregion
                    });
                }

            }
            #endregion

            #region 单元测试
            var examRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                       data.UserID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.ExamPaper);
            if (examRecord != null)
            {
                if (examRecord.detail.Count > 0)
                {
                    Rds_RedisVideoInfo info = new Rds_RedisVideoInfo();
                    info.ClassID = data.ClassID;

                    #region 修改班级报告

                    //更新或新增班级报告
                    CreateOrUpdateRds_StudyReport_Class(info, (int)ModuleTypeEnum.ExamPaper);

                    #endregion

                    examRecord.detail.ForEach(a =>
                    {

                        info.BookId = a.BookID.ToString();
                        #region 修改书本报告

                        //更新或新增书本报告
                        CreateOrUpdateRds_StudyReport_Book(info, (int)ModuleTypeEnum.ExamPaper);

                        #endregion
                    });

                }
            }

            #endregion

            #region 说说看
            var HearResources = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                data.UserID + "_" + (int)OldSubjectTypeEnum.English + "_" + (int)ModuleTypeEnum.HearResources);
            if (HearResources != null)
            {
                if (HearResources.detail.Count > 0)
                {
                    Rds_RedisVideoInfo info = new Rds_RedisVideoInfo();
                    info.ClassID = data.ClassID;
                    #region 修改班级报告
                    //更新或新增班级报告
                    CreateOrUpdateRds_StudyReport_Class(info, (int)ModuleTypeEnum.HearResources);
                    #endregion

                    HearResources.detail.ForEach(a =>
                    {

                        info.BookId = a.BookID.ToString();
                        info.VideoNumber = a.VideoNumber.ToString();
                        #region 修改书本报告
                        //更新或新增书本报告
                        CreateOrUpdateRds_StudyReport_Book(info, (int)ModuleTypeEnum.HearResources);
                        #endregion

                        #region 修改模块报告


                        var title = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + a.BookID,
                                a.FirstTitleID + "_" + a.SecondTitleID);
                        if (title != null)
                        {
                            info.FirstTitle = title.FirstTitle;
                            info.SecondTitle = title.SecondTitle;
                            info.FirstModular = title.FirstModular;
                        }


                        info.FirstTitleID = a.FirstTitleID > 0 ? a.FirstTitleID.ToString() : "";
                        info.FirstModularID = a.FirstModularID > 0 ? a.FirstModularID.ToString() : "";
                        info.SecondTitleID = a.SecondTitleID > 0 ? a.SecondTitleID.ToString() : "";
                        CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(info, (int)ModuleTypeEnum.HearResources);
                        #endregion
                    });
                }

            }

            #endregion
        }

        #endregion

        #region 定时服务，生成学习报告

        /// <summary>
        /// 优学说说看学习报告
        /// </summary>
        private void ExecuteHearResourcesLearningReport(Rds_RedisVideoInfo data, int ModuleType)
        {
            var user = ibsService.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
            if (user != null)
            {
                var classinfo = user.ClassSchList.FirstOrDefault();
                if (classinfo != null)
                {
                    var ibsclass = ibsService.GetClassUserRelationByClassId(classinfo.ClassID);
                    if (ibsclass != null)
                    {
                        data.ClassID = ibsclass.ClassNum;
                    }
                }
            }
            if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
            {

                CreateOrUpdateRds_StudyReport_Class(data, ModuleType);
                CreateOrUpdateRds_StudyReport_Book(data, ModuleType);
                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, ModuleType);
            }
            ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
            CreateOrUpdateRds_StudyReport_ModuleTitleForHearResources(data, ModuleType);
        }

        /// <summary>
        /// 说说看学习报告
        /// </summary>
        private void ExecuteHearResourcesLearningReport(Rds_RedisVideoInfo data)
        {
            var user = ibsService.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
            if (user != null)
            {
                var classinfo = user.ClassSchList.FirstOrDefault();
                if (classinfo != null)
                {
                    var ibsclass = ibsService.GetClassUserRelationByClassId(classinfo.ClassID);
                    if (ibsclass != null)
                    {
                        data.ClassID = ibsclass.ClassNum;
                    }
                }
            }
            if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
            {

                CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.HearResources);
                CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.HearResources);
                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, (int)ModuleTypeEnum.HearResources);
            }
            ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
            CreateOrUpdateRds_StudyReport_ModuleTitleForHearResources(data, (int)ModuleTypeEnum.HearResources);
        }


        /// <summary>
        // 优学单元测试（暂无）
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteExampaperLearningReport(Rds_RedisVideoInfo data, int ModuleType)
        {

            /* var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
             if (user != null)
             {
                 var classinfo = user.ClassSchList.FirstOrDefault();
                 if (classinfo != null)
                 {
                     var ibsclass = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                     if (ibsclass != null)
                     {
                         data.ClassID = ibsclass.ClassNum;
                     }
                 }
             }

             if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
             {*/

            //CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.ExamPaper);
            // CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.ExamPaper);
            try
            {
                CreateOrUpdateRds_StudyReport_ModuleTitleForExamPaper(data, ModuleType);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "ExecuteExampaperLearningReport! data=" + data.ToJson(), ex);
            }



            //}
        }

        /// <summary>
        // 单元测试
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteExampaperLearningReport(Rds_RedisVideoInfo data)
        {

            /* var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
             if (user != null)
             {
                 var classinfo = user.ClassSchList.FirstOrDefault();
                 if (classinfo != null)
                 {
                     var ibsclass = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                     if (ibsclass != null)
                     {
                         data.ClassID = ibsclass.ClassNum;
                     }
                 }
             }

             if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
             {*/

            //CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.ExamPaper);
            // CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.ExamPaper);

            try
            {
                CreateOrUpdateRds_StudyReport_ModuleTitleForExamPaper(data, (int)ModuleTypeEnum.ExamPaper);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "ExecuteExampaperLearningReport! data=" + data.ToJson(), ex);
            }


            //}
        }


        /// <summary>
        /// 优学趣配音
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteInterestDebbingLearningReport(Rds_RedisVideoInfo data, int ModuleType)
        {
            try
            {
                //更新或新增班级报告
                CreateOrUpdateRds_StudyReport_Class(data, ModuleType);

                //更新或新增书本报告
                CreateOrUpdateRds_StudyReport_Book(data, ModuleType);

                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, ModuleType);

                ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
                CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing(data, ModuleType);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "ExecuteInterestDebbingLearningReport! data=" + data.ToJson(), ex);
            }
        }


        /// <summary>
        /// 趣配音
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteInterestDebbingLearningReport(Rds_RedisVideoInfo data)
        {
            try
            {
                //更新或新增班级报告
                CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.InterestDebbing);

                //更新或新增书本报告
                CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.InterestDebbing);

                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, (int)ModuleTypeEnum.InterestDebbing);

                ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
                CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing(data, (int)ModuleTypeEnum.InterestDebbing);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "ExecuteInterestDebbingLearningReport! data=" + data.ToJson(), ex);
            }
        }


        /// <summary>
        /// 单元测试报告新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleTitleForExamPaper(Rds_RedisVideoInfo data, int ModuleType)
        {
            var learningRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                data.UserId + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType);
            if (learningRecord != null)
            {
                var record =
                    learningRecord.detail.FirstOrDefault(
                        a =>
                            a.BookID == Convert.ToInt32(data.BookId) &&
                            a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                if (record != null)
                {
                    if (double.Parse(data.TotalScore) > record.BestScore)
                    {
                        //具体操作
                        record.BestScore = double.Parse(data.TotalScore);
                        record.CreateTime = data.CreateTime;
                        record.VideoNumber = data.VideoNumber.ToInt();
                        record.VideoID = data.VideoID;
                        record.DubbingNum = data.DubbingNum;
                    }
                    else
                    {
                        record.DubbingNum += 1;
                    }
                }
                else
                {
                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                    {
                        //具体操作
                        BestScore = double.Parse(data.TotalScore),
                        CreateTime = data.CreateTime,
                        VideoNumber = data.VideoNumber.ToInt(),
                        VideoID = data.VideoID,
                        BookID = data.BookId.ToInt(),
                        DubbingNum = data.DubbingNum
                    });
                }
            }
            else
            {
                learningRecord = new Rds_StudyReport_ModuleTitle();
                learningRecord.UserID = data.UserId.ToInt();
                learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                {
                    //具体操作
                    BestScore = double.Parse(data.TotalScore),
                    CreateTime = data.CreateTime,
                    VideoID = data.VideoID,
                    VideoNumber = data.VideoNumber.ToInt(),
                    BookID = data.BookId.ToInt(),
                    DubbingNum = data.DubbingNum
                });

            }
            redis.Set<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                data.UserId + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType, learningRecord);
        }

        /// <summary>
        /// 趣配音报告新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing(Rds_RedisVideoInfo data, int ModuleType)
        {
            try
            {
                var learningRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType);
                if (learningRecord != null)
                {
                    var record =
                        learningRecord.detail.FirstOrDefault(
                            a =>
                                a.BookID == Convert.ToInt32(data.BookId) &&
                                a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    if (record != null)
                    {
                        record.DubbingNum += 1;
                        if (double.Parse(data.TotalScore) > record.BestScore)
                        {
                            //具体操作
                            record.BestScore = double.Parse(data.TotalScore);
                            record.CreateTime = data.CreateTime;
                            record.VideoNumber = data.VideoNumber.ToInt();
                            record.VideoID = data.VideoID;
                        }
                    }
                    else
                    {
                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                        {
                            //具体操作
                            BestScore = double.Parse(data.TotalScore),
                            CreateTime = data.CreateTime,
                            VideoNumber = data.VideoNumber.ToInt(),
                            BookID = data.BookId.ToInt(),
                            VideoID = data.VideoID,
                            FirstTitleID = data.FirstTitleID.ToInt(),
                            SecondTitleID = data.SecondTitleID.ToInt(),
                            FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                            DubbingNum = 1,
                            SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                        });
                        var title = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                                                 data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                        if (title != null)
                        {
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModular = data.FirstModular;

                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.VideoNumber = data.VideoNumber.ToInt();
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        else
                        {
                            title = new Rds_StudyReport_BookCatalogues_BookID();
                            title.FirstTitleID = data.FirstTitleID.ToInt();
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitleID = data.SecondTitleID.ToInt();
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                            title.FirstModular = data.FirstModular;
                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        redis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                    }
                }
                else
                {
                    learningRecord = new Rds_StudyReport_ModuleTitle();
                    learningRecord.UserID = data.UserId.ToInt();
                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                    {
                        //具体操作
                        BestScore = double.Parse(data.TotalScore),
                        CreateTime = data.CreateTime,
                        VideoNumber = data.VideoNumber.ToInt(),
                        BookID = data.BookId.ToInt(),
                        VideoID = data.VideoID,
                        FirstTitleID = data.FirstTitleID.ToInt(),
                        SecondTitleID = data.SecondTitleID.ToInt(),
                        FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                        DubbingNum = 1,
                        SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                    });
                    var title = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                    if (title != null)
                    {
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    else
                    {
                        title = new Rds_StudyReport_BookCatalogues_BookID();
                        title.FirstTitleID = data.FirstTitleID.ToInt();
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitleID = data.SecondTitleID.ToInt();
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    redis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                          data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                }
                redis.Set<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType, learningRecord);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing!data=" + data.ToJson(), ex);
            }
        }



        /// <summary>
        /// 趣配音报告新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleTitleForHearResources(Rds_RedisVideoInfo data, int ModuleType)
        {
            try
            {
                var learningRecord = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType);
                if (learningRecord != null)
                {
                    //Rds_StudyReport_BookDetail record=new Rds_StudyReport_BookDetail();
                    //if (string.IsNullOrEmpty(data.SecondTitleID))
                    //{
                    //    record =
                    //        learningRecord.detail.FirstOrDefault(
                    //            a =>
                    //                a.BookID == Convert.ToInt32(data.BookId) &&
                    //                a.FirstTitleID == Convert.ToInt32(data.FirstTitleID) &&
                    //                a.SecondTitleID == 0
                    //                && a.FirstModularID == Convert.ToInt32(data.FirstModularID) &&
                    //                a.SecondModularID == Convert.ToInt32(data.SecondModularID)&&
                    //                a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    //}
                    //else
                    //{
                    //    record =
                    //        learningRecord.detail.FirstOrDefault(
                    //            a =>
                    //                a.BookID == Convert.ToInt32(data.BookId) &&
                    //                a.FirstTitleID == Convert.ToInt32(data.FirstTitleID) &&
                    //                a.SecondTitleID == Convert.ToInt32(data.SecondTitleID)
                    //                && a.FirstModularID == Convert.ToInt32(data.FirstModularID) &&
                    //                a.SecondModularID == Convert.ToInt32(data.SecondModularID)&&
                    //                a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    //}
                    int SecondTitleID = 0;
                    if (!string.IsNullOrEmpty(data.SecondTitleID))
                    {
                        SecondTitleID = Convert.ToInt32(data.SecondTitleID);
                    }


                    var record =
                            learningRecord.detail.FirstOrDefault(
                                a =>
                                    a.BookID == Convert.ToInt32(data.BookId) &&
                                    a.FirstTitleID == Convert.ToInt32(data.FirstTitleID) &&
                                    a.SecondTitleID == SecondTitleID
                                    && a.FirstModularID == Convert.ToInt32(data.FirstModularID) &&
                                    a.SecondModularID == Convert.ToInt32(data.SecondModularID) &&
                                    a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    if (record != null)
                    {
                        record.DubbingNum += 1;
                        if (double.Parse(data.TotalScore) > record.BestScore)
                        {
                            //具体操作
                            record.BestScore = double.Parse(data.TotalScore);
                            record.CreateTime = data.CreateTime;
                            record.VideoNumber = data.VideoNumber.ToInt();
                            record.VideoID = data.VideoID;
                        }
                    }
                    else
                    {
                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                        {
                            //具体操作
                            BestScore = double.Parse(data.TotalScore),
                            CreateTime = data.CreateTime,
                            VideoNumber = data.VideoNumber.ToInt(),
                            BookID = data.BookId.ToInt(),
                            VideoID = data.VideoID,
                            FirstTitleID = data.FirstTitleID.ToInt(),
                            SecondTitleID = data.SecondTitleID.ToInt(),
                            FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                            DubbingNum = 1,
                            SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                        });
                        var title = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                                                 data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                        if (title != null)
                        {
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModular = data.FirstModular;

                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt() && a.SecondModularID == Convert.ToInt32(data.SecondModularID));
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.VideoNumber = data.VideoNumber.ToInt();
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        else
                        {
                            title = new Rds_StudyReport_BookCatalogues_BookID();
                            title.FirstTitleID = data.FirstTitleID.ToInt();
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitleID = data.SecondTitleID.ToInt();
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                            title.FirstModular = data.FirstModular;
                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt() && a.SecondModularID == Convert.ToInt32(data.SecondModularID));
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        redis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                    }
                }
                else
                {
                    learningRecord = new Rds_StudyReport_ModuleTitle();
                    learningRecord.UserID = data.UserId.ToInt();
                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                    {
                        //具体操作
                        BestScore = double.Parse(data.TotalScore),
                        CreateTime = data.CreateTime,
                        VideoNumber = data.VideoNumber.ToInt(),
                        BookID = data.BookId.ToInt(),
                        VideoID = data.VideoID,
                        FirstTitleID = data.FirstTitleID.ToInt(),
                        SecondTitleID = data.SecondTitleID.ToInt(),
                        FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                        DubbingNum = 1,
                        SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                    });
                    var title = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                    if (title != null)
                    {
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt() && a.SecondModularID == Convert.ToInt32(data.SecondModularID));
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    else
                    {
                        title = new Rds_StudyReport_BookCatalogues_BookID();
                        title.FirstTitleID = data.FirstTitleID.ToInt();
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitleID = data.SecondTitleID.ToInt();
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt() && a.SecondModularID == Convert.ToInt32(data.SecondModularID));
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    redis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                          data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                }
                redis.Set<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType, learningRecord);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "CreateOrUpdateRds_StudyReport_ModuleTitleForHearResource!Data=" + data.ToJson(), ex);
            }
        }


        /// <summary>
        /// 趣配音Module表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(Rds_RedisVideoInfo data, int ModuleType)
        {
            Rds_StudyReport_Module module = null;
            if (!string.IsNullOrEmpty(data.SecondTitleID) && data.SecondTitleID != "0")
            {
                //更新或新增书本报告
                module = redis.Get<Rds_StudyReport_Module>("Rds_StudyReport_Module",
                    data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType + "_" + data.BookId + "_" +
                    data.FirstTitleID + data.SecondTitleID);
                if (module != null)
                {
                    module.Flag = 0;
                }
                else
                {
                    module = new Rds_StudyReport_Module();
                    module.ClassID = data.ClassID;
                    module.SubjectID = (int)OldSubjectTypeEnum.English;
                    module.ModuleType = ModuleType;
                    module.FirstTitleID = data.FirstTitleID.ToInt();
                    module.FirstTitle = data.FirstTitle;
                    module.SecondTitleID = data.SecondTitleID.ToInt();
                    module.SecondTitle = data.SecondTitle;
                    module.BookID = Convert.ToInt32(data.BookId);
                    module.Flag = 0;
                }
                redis.Set<Rds_StudyReport_Module>("Rds_StudyReport_Module", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType + "_" + data.BookId + "_" + data.FirstTitleID + data.SecondTitleID, module);
            }
            else
            {
                module = redis.Get<Rds_StudyReport_Module>("Rds_StudyReport_Module",
                   data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType + "_" + data.BookId + "_" +
                   data.FirstTitleID);
                if (module != null)
                {
                    module.Flag = 0;
                }
                else
                {
                    module = new Rds_StudyReport_Module();
                    module.ClassID = data.ClassID;
                    module.SubjectID = (int)OldSubjectTypeEnum.English;
                    module.ModuleType = ModuleType;
                    module.FirstTitleID = data.FirstTitleID.ToInt();
                    module.FirstTitle = data.FirstTitle;
                    module.SecondTitleID = data.SecondTitleID.ToInt();
                    module.SecondTitle = data.SecondTitle;
                    module.BookID = data.BookId.ToInt();
                    module.Flag = 0;
                }
                redis.Set<Rds_StudyReport_Module>("Rds_StudyReport_Module", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType + "_" + data.BookId + "_" + data.FirstTitleID, module);
            }
        }


        /// <summary>
        /// 趣配音书本报告记录
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_Book(Rds_RedisVideoInfo data, int ModuleType)
        {
            //更新或新增书本报告
            var rdsBook = redis.Get<Rds_StudyReport_Book>("Rds_StudyReport_Book", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType + "_" + data.BookId);
            if (rdsBook != null)
            {
                rdsBook.Flag = 0;
            }
            else
            {
                rdsBook = new Rds_StudyReport_Book();
                rdsBook.ClassID = data.ClassID;
                rdsBook.SubjectID = (int)OldSubjectTypeEnum.English;
                rdsBook.ModuleType = ModuleType;
                rdsBook.BookID = data.BookId.ToInt();
                rdsBook.Flag = 0;
            }
            redis.Set<Rds_StudyReport_Book>("Rds_StudyReport_Book", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType + "_" + data.BookId, rdsBook);
        }

        /// <summary>
        /// 趣配音班级记录新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_Class(Rds_RedisVideoInfo data, int ModuleType)
        {

            //更新或新增班级报告
            var rdsClass = redis.Get<Rds_StudyReport_Class>("Rds_StudyReport_Class", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType);
            if (rdsClass != null)
            {
                rdsClass.Flag = 0;
            }
            else
            {
                rdsClass = new Rds_StudyReport_Class();
                rdsClass.ClassID = data.ClassID;
                rdsClass.SubjectID = (int)OldSubjectTypeEnum.English;
                rdsClass.ModuleType = ModuleType;
                rdsClass.Flag = 0;
            }
            redis.Set<Rds_StudyReport_Class>("Rds_StudyReport_Class", data.ClassID + "_" + (int)OldSubjectTypeEnum.English + "_" + ModuleType, rdsClass);
        }

        #endregion


        #region  排行榜定时任务
        public void ExcutedInterestingRank()
        {
            //RedisVideoInfo submitData = JsonHelper.DecodeJson<RedisVideoInfo>(strJson);
            var listCount = redis.Count("RankQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            using (var Redis = RedisManager.GetClient(0, "Tbx"))
            {
                for (int i = 0; i < Count; i++)
                {
                    var model = Redis.RemoveStartFromList("RankQueue");
                    if (!string.IsNullOrEmpty(model))
                    {
                        Rds_RedisVideoInfo data = model.FromJson<Rds_RedisVideoInfo>();
                        InsertVideoRedis(data);

                    }
                }
            }
        }

        private void InsertVideoRedis(Rds_RedisVideoInfo submitData)
        {
            #region 插入用户配音数据
            bool bl1 = InsertVideoInfoRedis(submitData);
            #endregion

            if (submitData.UserType != "12")
            {
                if (!string.IsNullOrEmpty(submitData.SchoolID))
                {
                    #region 校级榜插入数据
                    bool bl2 = InsertSchoolRank(submitData);
                    #endregion
                }

                if (!string.IsNullOrEmpty(submitData.ClassID))
                {
                    #region 班级榜插入数据
                    bool bl3 = InsertClassRank(submitData);
                    #endregion
                }
            }
            #region 最新榜数据
            bool bl4 = InsertNewRank(submitData);
            #endregion
        }

        /// <summary>
        /// 插入用户配音数据
        /// </summary>
        /// <param name="UserVideoID"></param>
        /// <param name="submitData"></param>
        /// <param name="tname"></param>
        /// <param name="img"></param>
        /// <param name="lst"></param>
        private bool InsertVideoInfoRedis(Rds_RedisVideoInfo redisVideoInfo)
        {
            bool bl1 = false;

            List<string> lst = new List<string>();
            Redis_IntDubb_VideoInfo videoinfo = new Redis_IntDubb_VideoInfo()
            {
                VideoId = redisVideoInfo.VideoID.ToString(),
                UserId = redisVideoInfo.UserId,
                TrueName = redisVideoInfo.TrueName,
                UserImage = redisVideoInfo.UserImage,
                TotalScore = redisVideoInfo.TotalScore,
                NumberOfOraise = lst,
                CreateTime = DateTime.Now.ToString()
            };
            string intVideoInfo = redis.Get("Redis_IntDubb_VideoInfo_" + redisVideoInfo.BookId, redisVideoInfo.VideoID.ToString());
            if (intVideoInfo == null)
            {
                bl1 = redis.Set<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + redisVideoInfo.BookId, redisVideoInfo.VideoID.ToString(), videoinfo);
                Log4NetHelper.Info(LoggerType.FsExceptionLog,"插入用户配音数据:VideoID:" + redisVideoInfo.VideoID + ";BookID:" + redisVideoInfo.BookId + "；VideoInfo:" + redisVideoInfo.ToJson() + "是否成功：" + bl1);
            }
            return bl1;
        }

        /// <summary>
        /// 最新榜插入数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="UserVideoID"></param>
        /// <param name="rank"></param>
        private bool InsertNewRank(Rds_RedisVideoInfo redisVideoInfo)
        {
            List<Redis_IntDubb_NewRank> listNewRanke = new List<Redis_IntDubb_NewRank>();
            List<Redis_IntDubb_NewRank> idn = redis.Get<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + redisVideoInfo.BookId, redisVideoInfo.VideoNumber);
            if (idn != null)
            {
                foreach (var item in idn)
                {
                    Redis_IntDubb_NewRank Ranke = new Redis_IntDubb_NewRank
                    {
                        VideoId = item.VideoId.ToString(),
                        Sort = item.Sort
                    };
                    listNewRanke.Add(Ranke);
                }
            }
            Redis_IntDubb_NewRank newRanke = new Redis_IntDubb_NewRank
            {
                VideoId = redisVideoInfo.VideoID.ToString(),
                Sort = 0
            };

            listNewRanke.Add(newRanke);
            listNewRanke = listNewRanke.OrderByDescending(i => i.VideoId).Take(200).ToList();
            bool bl6 = redis.Set<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + redisVideoInfo.BookId, redisVideoInfo.VideoNumber, listNewRanke);
            Log4NetHelper.Info(LoggerType.FsExceptionLog,"插入最新榜数据:BookID:" + redisVideoInfo.BookId + ";VideoNumber:" + redisVideoInfo.VideoNumber + ";ListNewRanke:" + redisVideoInfo.ToJson() + "是否成功：" + bl6);
            return bl6;
        }

        /// <summary>
        /// 班级榜插入数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="rel"></param>
        /// <param name="rank"></param>
        private bool InsertClassRank(Rds_RedisVideoInfo redisVideoInfo)
        {
            int c = 0;
            Redis_IntDubb_Rank rank = new Redis_IntDubb_Rank
            {
                UserId = redisVideoInfo.UserId,
                VideoId = redisVideoInfo.VideoID.ToString(),
                TotalScore = Convert.ToDouble(redisVideoInfo.TotalScore)
            };
            List<Redis_IntDubb_Rank> cRank = new List<Redis_IntDubb_Rank>();
            List<Redis_IntDubb_Rank> ClassRin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + redisVideoInfo.BookId, redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber);
            if (ClassRin != null)
            {
                foreach (var item in ClassRin)
                {
                    if (item.UserId == redisVideoInfo.UserId)
                    {
                        c = 1;
                        if (Convert.ToDouble(item.TotalScore) <= Convert.ToDouble(redisVideoInfo.TotalScore))
                        {
                            cRank.Add(rank);
                        }
                        else
                        {
                            cRank.Add(new Redis_IntDubb_Rank
                            {
                                UserId = item.UserId,
                                VideoId = item.VideoId,
                                TotalScore = item.TotalScore
                            });
                        }
                    }
                    else
                    {
                        cRank.Add(new Redis_IntDubb_Rank
                        {
                            UserId = item.UserId,
                            VideoId = item.VideoId,
                            TotalScore = item.TotalScore
                        });
                    }
                }
                if (c == 0)
                {
                    cRank.Add(rank);
                }
                cRank = cRank.OrderByDescending(i => i.TotalScore).Take(200).ToList();
                bool bl4 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + redisVideoInfo.BookId, redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber, cRank);
                Log4NetHelper.Info(LoggerType.FsExceptionLog, "插入班级榜数据：BookId:" + redisVideoInfo.BookId + ";ClassID:" + redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber + "；redisVideoInfo：" + redisVideoInfo.ToJson() + "是否成功：" + bl4);
                return bl4;
            }
            else
            {
                cRank.Add(rank);
                cRank = cRank.OrderByDescending(i => i.TotalScore).Take(200).ToList();
                bool bl5 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + redisVideoInfo.BookId, redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber, cRank);
                Log4NetHelper.Info(LoggerType.FsExceptionLog, "插入班级榜数据：BookId:" + redisVideoInfo.BookId + ";ClassID:" + redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber + "；redisVideoInfo：" + redisVideoInfo.ToJson() + "是否成功：" + bl5);
                return bl5;
            }
        }

        /// <summary>
        /// 校级榜插入数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="rel"></param>
        /// <param name="s"></param>
        /// <param name="intRank"></param>
        /// <param name="rank"></param>
        private bool InsertSchoolRank(Rds_RedisVideoInfo redisVideoInfo)
        {
            List<Redis_IntDubb_Rank> intRank = new List<Redis_IntDubb_Rank>();
            int s = 0;
            Redis_IntDubb_Rank rank = new Redis_IntDubb_Rank
            {
                UserId = redisVideoInfo.UserId,
                VideoId = redisVideoInfo.VideoID.ToString(),
                TotalScore = Convert.ToDouble(redisVideoInfo.TotalScore)
            };
            List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + redisVideoInfo.BookId, redisVideoInfo.SchoolID + "_" + redisVideoInfo.VideoNumber);
            if (rin != null)
            {
                foreach (var item in rin)
                {
                    if (item.UserId == redisVideoInfo.UserId)
                    {
                        s = 1;
                        if (Convert.ToDouble(item.TotalScore) <= Convert.ToDouble(redisVideoInfo.TotalScore))
                        {
                            intRank.Add(rank);
                        }
                        else
                        {
                            intRank.Add(new Redis_IntDubb_Rank
                            {
                                UserId = item.UserId,
                                VideoId = item.VideoId,
                                TotalScore = item.TotalScore
                            });
                        }
                    }
                    else
                    {
                        intRank.Add(new Redis_IntDubb_Rank
                        {
                            UserId = item.UserId,
                            VideoId = item.VideoId,
                            TotalScore = item.TotalScore
                        });
                    }
                }
                if (s == 0)
                {
                    intRank.Add(rank);
                }
                intRank = intRank.OrderByDescending(i => i.TotalScore).Take(200).ToList();
                bool bl2 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + redisVideoInfo.BookId, redisVideoInfo.SchoolID + "_" + redisVideoInfo.VideoNumber, intRank);
                Log4NetHelper.Info(LoggerType.FsExceptionLog, "插入校级榜数据:redisVideoInfo:" + redisVideoInfo.ToJson() + ";是否成功：" + bl2);
                return bl2;
            }
            else
            {
                intRank.Add(rank);
                intRank.OrderByDescending(i => i.TotalScore).ToList().Take(200);
                bool bl3 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + redisVideoInfo.BookId, redisVideoInfo.SchoolID + "_" + redisVideoInfo.VideoNumber, intRank);
                Log4NetHelper.Info(LoggerType.FsExceptionLog, "插入校级榜数据redisVideoInfo:" + redisVideoInfo.ToJson() + ";是否成功：" + bl3);
                return bl3;
            }

        }
        #endregion

        private void UpdateOrder2YX(UserPayOrder order)
        {
            throw new NotImplementedException();
        }

        private void InsertOrder2YX(UserPayOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
