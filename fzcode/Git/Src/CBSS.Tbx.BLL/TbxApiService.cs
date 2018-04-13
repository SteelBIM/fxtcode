using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using CBSS.IBS.Contract;
using CBSS.IBS.Contract.IBSResource;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
      //  private int ModelType = 1;//redis数据类型(1:趣配音,2:单元测试，3：说说看,4优学趣配音，5优学单元测试，6优学说说看)
        private int Subject = 3;//学科（3：英语）
       

        public APIResponse GetModResource(EnglishResourceModel model)
        {
            var module = GetModuleList(o => o.ModuleID == model.moduleID).FirstOrDefault();
            switch ((MODSourceTypeEnum)model.type)
            {
                case MODSourceTypeEnum.EBook:
                    return GetPrimaryEBook(model);

                case MODSourceTypeEnum.Listen:
                    return GetPrimaryListen(model);

                case MODSourceTypeEnum.HearResource:
                    return GetSecondModuleHearResource(model);

                case MODSourceTypeEnum.IntestingDubbing:
                    return GetIntestingDubbing(model);
                default:
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到资源);
            }
        }


        public APIResponse GetBookSecretKey(int marketBookId)
        {
            var book = repository.SelectSearch<MarketBook>(o => o.MarketBookID == marketBookId).FirstOrDefault();

            if (book == null) return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到课本);

            return ibsService.GetBookSecretKey(book.MODBookID);
        }
        public APIResponse GetArticle(EnglishResourceModel param)
        {
            var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();

            var book = GetMarketBookByID(cata.MarketBookID.Value);
            var response = ibsService.GetArticle(book.MODBookID, 1003, cata.MODBookCatalogID.Value);

            var d = response.Data as List<FollowRead>;


            return APIResponse.GetResponse(response.Data);
        }

        public APIResponse GetIntestingDubbing(EnglishResourceModel param)
        {
            var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();

            var book = GetMarketBookByID(cata.MarketBookID.Value);
            var response = ibsService.GetDubbingByCataId(cata.MODBookCatalogID.Value,param);

            var d = response.Data as List<V_DubbingInfo>;


            return APIResponse.GetResponse(response.Data);
        }

        public APIResponse GetFollowRead(EnglishResourceModel param)
        {

            var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();

            var book = GetMarketBookByID(cata.MarketBookID.Value);
            var response = ibsService.GetFollowRead(book.MODBookID, 1002, cata.MODBookCatalogID.Value);
            return APIResponse.GetResponse(response.Data);
        }

        /// <summary>
        /// 通过书本ID和一级目录ID查询一级目录下的的所有二级目录说说看资源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public APIResponse GetSecondModuleHearResource(EnglishResourceModel param)
        {
            List<v_HearResources> hearResource = new List<v_HearResources>();
            Dictionary<int, int> list = new Dictionary<int, int>();
            var cata = GetMarketBookCatalogsList(o => o.ParentCatalogID == param.FirstTitleID).ToList();
            if (cata != null)
            {
                cata.ForEach(a => { list.Add((int)a.MODBookCatalogID, a.MarketBookCatalogID); });
            }

            var book = GetMarketBookByID(param.bookId);
            var response = ibsService.GetHearResource(book.MODBookID, list, param);
            return APIResponse.GetResponse(response.Data);
        }

        public APIResponse GetHearResource(EnglishResourceModel param)
        {
            MarketBookCatalog cata;
            if (!string.IsNullOrEmpty(param.SecondTitleID) && param.SecondTitleID != "0")
            {
                int SecondTitleID = param.SecondTitleID.ToInt();
                cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == SecondTitleID).FirstOrDefault();
            }
            else
            {
                cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.FirstTitleID).FirstOrDefault();
            }

            var book = GetMarketBookByID((int)cata.MarketBookID);
            if (param.SecondModularID > 0)
            {
                var response = ibsService.GetHearResource(book.MODBookID, (int)cata.MODBookCatalogID, param.SecondModularID, param);
                return APIResponse.GetResponse(response.Data);
            }
            else
            {
                var response = ibsService.GetHearResource(book.MODBookID, (int)cata.MODBookCatalogID, param);
                return APIResponse.GetResponse(response.Data);
            }

        }

        public APIResponse GetPrimaryEBook(EnglishResourceModel param)
        {
            //   var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();

            var book = GetMarketBookByID(param.bookId);
            var response = ibsService.GetPrimaryEBook(book.MODBookID, param.secretKeyId);

            return APIResponse.GetResponse(response.Data);
        }

        public APIResponse GetPrimaryListen(EnglishResourceModel param)
        {
            //var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();

            var book = GetMarketBookByID(param.bookId);
            var response = ibsService.GetPrimaryListen(book.MODBookID, param.secretKeyId);
            return APIResponse.GetResponse(response.Data);
        }
        public APIResponse GetEBook(EnglishResourceModel param)
        {
            //   var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();

            var book = GetMarketBookByID(param.bookId);
            var response = ibsService.GetEBook(book.MODBookID, param.secretKeyId);

            return APIResponse.GetResponse(response.Data);
        }

        public APIResponse GetListen(EnglishResourceModel param)
        {
            //var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();

            var book = GetMarketBookByID(param.bookId);
            var response = ibsService.GetListen(book.MODBookID, param.secretKeyId);
            return APIResponse.GetResponse(response.Data);
        }
        /// <summary>
        /// 新目标单词
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public APIResponse GetRJXMBWords(EnglishResourceModel param)
        {
            var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();
            param.size = param.size == 0 ? 30 : param.size;
            if (cata == null) return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到目录);
            //新目标用父级目录ID查询单词：
            cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == cata.ParentCatalogID).FirstOrDefault();
            if (cata == null) return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到目录);

            var book = GetMarketBookByID(cata.MarketBookID.Value);//查出book

            var response = ibsService.GetWordsByCatalogId(param.secretKeyId, cata.MODBookCatalogID.Value, param.size);
            if (book == null) return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到课本);
            var allBookWords = ibsService.GetWordsByBookId(null, book.MODBookID, param.size);

            var data = allBookWords.Data as WordsByBookId;
            List<Word> studiedWords = new List<Word>();//学过的单词（当前单元之前的单词）
            if (data == null || data.words == null || !data.words.Any())
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到资源);
            }
            foreach (var w in data.words)
            {
                if (w.Key < cata.MODBookCatalogID)
                {
                    studiedWords.AddRange(w.Value.ToJson().ToObject<List<Word>>());
                }
            }

            if (studiedWords.Count < 2)//前面单元的单词少于2个，则放开限制，取全书单词
            {
                foreach (var w in data.words)
                {
                    if (w.Key >= cata.MODBookCatalogID)
                    {
                        studiedWords.AddRange(w.Value.ToJson().ToObject<List<Word>>());
                    }
                }
            }

            Random rand = new Random();
            var currentWords = response.Data as Words;//当前单元单词       
            var currentWordsCopy = currentWords.words != null ? currentWords.words.ToJson().ToObject<List<Word>>() : new List<Word>();
            studiedWords = studiedWords.Where(o => !currentWords.words.Select(w => w.wordId).Contains(o.wordId)).ToList();
            foreach (var c in currentWords.words)
            {
                do
                {
                    c.encryptImgUrl = c.encryptImgUrl.Split(',')[0];
                    var r1 = rand.Next(0, studiedWords.Count / 2);
                    var r2 = rand.Next(studiedWords.Count / 2, studiedWords.Count);

                    var randWord1 = studiedWords[r1];
                    var randWord2 = studiedWords[r2];
                    var otherWords = currentWordsCopy.Where(o => o.wordId != c.wordId && o.wordId != randWord1.wordId && o.wordId != randWord2.wordId).ToList();//当前单元单词（除了当前单词和另外两个随机单词）
                    var currentRandWord = otherWords[rand.Next(0, otherWords.Count)];
                    c.encryptImgUrl += string.Format(",{0},{1},{2}", randWord1.encryptImgUrl, randWord2.encryptImgUrl, currentRandWord.encryptImgUrl);
                    c.originImgUrl += string.Format(",{0},{1},{2}", randWord1.originImgUrl, randWord2.originImgUrl, currentRandWord.originImgUrl);
                    c.encryptSentenceImgUrl = string.Format("{0},{1},{2},{3}", c.sentences[0].encryptImgUrl, randWord1.sentences[0].encryptImgUrl, randWord2.sentences[0].encryptImgUrl, currentRandWord.sentences[0].encryptImgUrl);
                    if (c.similarWordList != null && c.similarWordList.Any())
                    {
                        c.similarWords = string.Join("; ", c.similarWordList);
                    }
                }
                while (c.encryptImgUrl.Split(',').Distinct().Count() < c.encryptImgUrl.Split(',').Count() || c.encryptSentenceImgUrl.Split(',').Distinct().Count() < c.encryptSentenceImgUrl.Split(',').Count());//检查重复，有则重新分配
            }


            return APIResponse.GetResponse(response.Data);
        }
        public APIResponse GetEWords(EnglishResourceModel param)
        {
            var cata = GetMarketBookCatalogsList(o => o.MarketBookCatalogID == param.catalogueId).FirstOrDefault();
            param.size = param.size == 0 ? 30 : param.size;
            if (cata == null) return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到目录);
            var book = GetMarketBookByID(cata.MarketBookID.Value);//查出book
            var response = ibsService.GetWordsByCatalogId(param.secretKeyId, cata.MODBookCatalogID.Value, param.size);
            if (book == null) return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到课本);
            var allBookWords = ibsService.GetWordsByBookId(null, book.MODBookID, param.size);

            var data = allBookWords.Data as WordsByBookId;
            List<Word> studiedWords = new List<Word>();//学过的单词（当前单元之前的单词）
            if (data == null || data.words == null || !data.words.Any())
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到资源);
            }
            foreach (var w in data.words)
            {
                if (w.Key < cata.MODBookCatalogID)
                {
                    studiedWords.AddRange(w.Value.ToJson().ToObject<List<Word>>());
                }
            }

            if (studiedWords.Count < 2)//前面单元的单词少于2个，则放开限制，取全书单词
            {
                foreach (var w in data.words)
                {
                    if (w.Key >= cata.MODBookCatalogID)
                    {
                        studiedWords.AddRange(w.Value.ToJson().ToObject<List<Word>>());
                    }
                }
            }

            Random rand = new Random();
            var currentWords = response.Data as Words;//当前单元单词       
            var currentWordsCopy = currentWords.words != null ? currentWords.words.ToJson().ToObject<List<Word>>() : new List<Word>();
            studiedWords = studiedWords.Where(o => !currentWords.words.Select(w => w.wordId).Contains(o.wordId)).ToList();
            foreach (var c in currentWords.words)
            {
                do
                {
                    c.encryptImgUrl = c.encryptImgUrl.Split(',')[0];
                    var r1 = rand.Next(0, studiedWords.Count / 2);
                    var r2 = rand.Next(studiedWords.Count / 2, studiedWords.Count);

                    var randWord1 = studiedWords[r1];
                    var randWord2 = studiedWords[r2];
                    var otherWords = currentWordsCopy.Where(o => o.wordId != c.wordId && o.wordId != randWord1.wordId && o.wordId != randWord2.wordId).ToList();//当前单元单词（除了当前单词和另外两个随机单词）
                    var currentRandWord = otherWords[rand.Next(0, otherWords.Count)];
                    c.encryptImgUrl += string.Format(",{0},{1},{2}", randWord1.encryptImgUrl, randWord2.encryptImgUrl, currentRandWord.encryptImgUrl);
                    c.originImgUrl += string.Format(",{0},{1},{2}", randWord1.originImgUrl, randWord2.originImgUrl, currentRandWord.originImgUrl);
                    c.encryptSentenceImgUrl = string.Format("{0},{1},{2},{3}", c.sentences[0].encryptImgUrl, randWord1.sentences[0].encryptImgUrl, randWord2.sentences[0].encryptImgUrl, currentRandWord.sentences[0].encryptImgUrl);
                    //  c.similarWords = c.similarWords.Replace(";", "; ");
                    if (c.similarWordList != null && c.similarWordList.Any())
                    {
                        c.similarWords = string.Join("; ", c.similarWordList);
                    }
                }
                while (c.encryptImgUrl.Split(',').Distinct().Count() < c.encryptImgUrl.Split(',').Count() || c.encryptSentenceImgUrl.Split(',').Distinct().Count() < c.encryptSentenceImgUrl.Split(',').Count());//检查重复，有则重新分配
            }


            return APIResponse.GetResponse(response.Data);
        }

        public APIResponse GetExercise(EnglishResourceModel param)
        {
            var book = GetMarketBookByID(param.bookId);//查询mod bookid

            if (book == null || book.MODBookID <= 0)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到课本);
            }

            var response = ibsService.GetExerciseAudio(book.MODBookID);
            return response;
        }

        public APIResponse GetTopicSets(EnglishResourceModel param)
        {
            return null;
        }

        public APIResponse GetClassInfoByTeacherId(string appId, string classid, List<ClassInfoList> classList)
        {
            //List<string> strlist = new List<string>(classid.Split(','));
            //List<TBX_UserClass> ibs = new List<TBX_UserClass>();
            //strlist.ForEach(a =>
            //{
            //    List<TBX_StudentCount> count = new List<TBX_StudentCount>();
            //    var userclass = ibsService.GetUserClassRelationByNum(a, out count);
            //    userclass.ForEach(x =>
            //    {
            //        if (ibs.FirstOrDefault(y => y.ClassID == x.ClassID && y.UserID == x.UserID) == null)
            //        {
            //            ibs.Add(x);
            //        }

            //    });
            //});
            //Log4Net.LogHelper.Info(ibs.ToJson());
            return GetClassInfoByTeacherId(classList);
        }

        /// <summary>
        /// 根据老师ID查询班级信息
        /// </summary>
        /// <returns></returns>
        public APIResponse GetClassInfoByTeacherId(List<ClassInfoList> classList)
        {
            List<ClassInfoList> returnCinfoList = ClassOrder(classList);//排序后的班级新

            object obj = new { ClassList = returnCinfoList.OrderByDescending(i => i.ClassName) };//返回信息
            return APIResponse.GetResponse(obj);
        }

        /// <summary>
        /// 班级排序
        /// </summary>
        /// <param name="list"></param>
        private List<ClassInfoList> ClassOrder(List<ClassInfoList> list)
        {
            List<ClassInfoList> classList = list;
            List<ClassInfoList> returnList = new List<ClassInfoList>();
            if (classList != null && classList.Count > 0)
            {
                string[] gradeArr = { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" };
                for (int i = 0, length = gradeArr.Length; i < length; i++)
                {
                    returnList.AddRange(classList.Where(t => t.ClassName.IndexOf(gradeArr[i], StringComparison.Ordinal) > -1));
                }
            }
            return returnList;
        }

        public APIResponse GetJuniorGradeNumByClassId(string appId, string classId)
        {
            int ModelType = 1;//redis数据类型(1:趣配音,2:单元测试，3：说说看)
            int Subject = 3;//学科（3：英语）
            var appBooks = repository.SelectSearch<MarketBook, AppMarketBook>((m, a) => a.AppID == appId, (m, a) => a.MarketBookID == m.MarketBookID).ToList();

            List<JuniorGradeInfoList> listobj = new List<JuniorGradeInfoList>();
            for (int i = 0; i < appBooks.Count; i++)
            {
                var book = appBooks[i];
                JuniorGradeInfoList obj = new JuniorGradeInfoList();
                try
                {
                    string value = redis.Get("Rds_StudyReport_Book", classId + "_" + Subject + "_" + ModelType + "_" + book.MarketBookID);
                    if (!string.IsNullOrEmpty(value))
                    {
                        int bookId = Convert.ToInt32(book.MarketBookID);
                        string bookName = string.IsNullOrEmpty(book.MarketBookName) ? book.MODBookName : book.MarketBookName;
                        obj = new JuniorGradeInfoList { BookId = bookId, BookName = bookName, IsStudy = true };
                    }
                    else
                    {
                        int bookId = Convert.ToInt32(book.MarketBookID);
                        string bookName = string.IsNullOrEmpty(book.MarketBookName) ? book.MODBookName : book.MarketBookName;
                        obj = new JuniorGradeInfoList { BookId = bookId, BookName = bookName, IsStudy = false };
                    }
                    Rds_StudyReport_Book study = value.ToObject<Rds_StudyReport_Book>();
                    if (study != null)
                    {
                        study.Flag = 1;
                        try
                        {
                            string ve = study.ToJson();
                            redis.Set("Rds_StudyReport_Book", classId + "_" + Subject + "_" + ModelType + "_" + book.MarketBookID, ve);
                        }
                        catch (Exception ex)
                        {
                            Log4NetHelper.Error(LoggerType.ApiExceptionLog, "错误：HashID为：Rds_StudyReport_Book|pairs为：" + classId + "_" + Subject + "_" + ModelType + "_" + book.MarketBookID, ex);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.ApiExceptionLog, "错误：HashID为：Rds_StudyReport_Class|Key为：" + classId + "_" + Subject + "_" + ModelType + "_" + book.MarketBookID, ex);
                }

                listobj.Add(obj);
            }
            if (listobj.Count > 0)
            {
                return APIResponse.GetResponse(CurriculumOrder(listobj));
            }
            else
            {
                return APIResponse.GetErrorResponse("数据不存在！");
            }
        }

        /// <summary>
        /// 年级排序
        /// </summary>
        /// <param name="list"></param>
        private List<JuniorGradeInfoList> CurriculumOrder(List<JuniorGradeInfoList> list)
        {
            List<JuniorGradeInfoList> returnList = new List<JuniorGradeInfoList>();
            if (list != null && list.Count > 0)
            {
                string[] gradeArr = { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" };
                for (int i = 0, length = gradeArr.Length; i < length; i++)
                {
                    returnList.AddRange(list.Where(t => t.BookName.IndexOf(gradeArr[i], StringComparison.Ordinal) > -1));
                }
            }
            return returnList;
        }

        /// <summary>
        /// 根据书籍id、班级Id获取单元模块下学习人数
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="classId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public APIResponse GetUnitLearningByBookId(string bookId, string classId, int pageNumber)
        {
            List<TBX_StudentCount> stuCount = new List<TBX_StudentCount>();
            var userClassList = ibsService.GetUserClassRelationByNum(classId, out stuCount);
            return GetUnitLearningByBookId(bookId, classId, pageNumber, userClassList,1);
        }

        /// <summary>
        /// 根据书籍id、班级Id获取单元模块下学习人数
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="classId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public APIResponse GetYxUnitLearningByBookId(string bookId, string classId, int pageNumber)
        {
            List<TBX_StudentCount> stuCount = new List<TBX_StudentCount>();
            var userClassList = ibsService.GetUserClassRelationByNum(classId, out stuCount);
            return GetUnitLearningByBookId(bookId, classId, pageNumber, userClassList,4);
        }

        /// <summary>
        ///说说看 根据书籍Id和班级Id查询单元学习人数
        /// </summary>
        /// <param name="bookId">书籍ID</param>
        /// <param name="classId">班级ID</param>
        /// <param name="pageNumber">分页页码</param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public APIResponse GetUnitLearningByBookId(string bookId, string classId, int pageNumber, string ModelType)
        {
            var intBookId = int.Parse(bookId);
            var catalogs = repository.SelectSearch<MarketBookCatalog>(o => o.MarketBookID == intBookId);
            if (!catalogs.Any())
            {
                return APIResponse.GetErrorResponse("无目录数据！");
            }

            List<UnitLearningInfoList> unit = new List<UnitLearningInfoList>();
            UnitLearningInfoList ull;

            foreach (var item in catalogs)
            {
                var pCata = new MarketBookCatalog();
                if (item.ParentCatalogID > 0)
                {
                    pCata = repository.FirstOrDefault<MarketBookCatalog>(o => o.MarketBookCatalogID == item.ParentCatalogID);
                }
                else
                {
                    pCata = item;
                }
                string FirstTitle = !string.IsNullOrEmpty(pCata.MarketBookCatalogName) ? pCata.MarketBookCatalogName : pCata.MODBookCatalogName;
                string secondTitle = !string.IsNullOrEmpty(item.MarketBookCatalogName) ? item.MarketBookCatalogName : item.MODBookCatalogName;
                ull = new UnitLearningInfoList();
                string modName = "";
                string uniName = "";
                string muName = "";
                modName = FirstTitle;
                if (secondTitle != null)
                {
                    uniName = secondTitle;
                }
                if (modName != "" && uniName != "")
                {
                    muName = modName + "/" + uniName;
                }
                if (modName != "" && uniName == "")
                {
                    muName = modName;
                }
                string Mkey = "";
                if (item.MarketBookCatalogID > 0)
                {
                    Mkey = classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + pCata.MarketBookCatalogID + item.MarketBookCatalogID;
                }
                else
                {
                    Mkey = classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + item.MarketBookCatalogID;
                }
                string value = redis.Get("Rds_StudyReport_Module", Mkey);
                if (!string.IsNullOrEmpty(value))
                {
                    ull.FirstTitleID = pCata.MarketBookCatalogID;
                    ull.SecondTitleID = item.MarketBookCatalogID;
                    ull.Catalague = muName;
                    ull.IsStudy = true;
                    unit.Add(ull);
                }
                else
                {
                    ull.Catalague = muName;
                    ull.FirstTitleID = pCata.MarketBookCatalogID;
                    ull.SecondTitleID = item.MarketBookCatalogID;
                    ull.IsStudy = false;
                    unit.Add(ull);
                }
            }

            if (unit.Count <= 0)
            {
                return APIResponse.GetErrorResponse("暂无数据！");
            }
            else
            {
                unit = unit.OrderBy(i => i.FirstTitleID).ThenBy(i => i.SecondTitleID).Skip(pageNumber * 10).Take(10).ToList();
                return APIResponse.GetResponse(unit);
            }
        }

        /// <summary>
        /// 根据书籍Id和班级Id查询单元学习人数
        /// </summary>
        /// <param name="bookId">书籍ID</param>
        /// <param name="classId">班级ID</param>
        /// <param name="pageNumber">分页页码</param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public APIResponse GetUnitLearningByBookId(string bookId, string classId, int pageNumber, List<TBX_UserClass> userClass,int ModelType)
        {
            var intBookId = int.Parse(bookId);
            var catalogs = repository.SelectSearch<MarketBookCatalog>(o => o.MarketBookID == intBookId);
            if (!catalogs.Any())
            {
                return APIResponse.GetErrorResponse("无目录数据！");
            }

            List<UnitLearningInfoList> unit = new List<UnitLearningInfoList>();
            UnitLearningInfoList ull;

            foreach (var item in catalogs)
            {
                var pCata = new MarketBookCatalog();
                if (item.ParentCatalogID > 0)
                {
                    pCata = repository.FirstOrDefault<MarketBookCatalog>(o => o.MarketBookCatalogID == item.ParentCatalogID);
                }
                else
                {
                    pCata = item;
                }
                string FirstTitle = !string.IsNullOrEmpty(pCata.MarketBookCatalogName) ? pCata.MarketBookCatalogName : pCata.MODBookCatalogName;
                string secondTitle = !string.IsNullOrEmpty(item.MarketBookCatalogName) ? item.MarketBookCatalogName : item.MODBookCatalogName;
                ull = new UnitLearningInfoList();
                string modName = "";
                string uniName = "";
                string muName = "";
                modName = FirstTitle;
                if (secondTitle != null)
                {
                    uniName = secondTitle;
                }
                if (modName != "" && uniName != "")
                {
                    muName = modName + "/" + uniName;
                }
                if (modName != "" && uniName == "")
                {
                    muName = modName;
                }
                string Mkey = "";
                if (item.MarketBookCatalogID > 0)
                {
                    Mkey = classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + pCata.MarketBookCatalogID + item.MarketBookCatalogID;
                }
                else
                {
                    Mkey = classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + item.MarketBookCatalogID;
                }
                string value = redis.Get("Rds_StudyReport_Module", Mkey);
                if (!string.IsNullOrEmpty(value))
                {
                    ull.FirstTitleID = pCata.MarketBookCatalogID;
                    ull.SecondTitleID = item.MarketBookCatalogID;
                    ull.Catalague = muName;
                    ull.IsStudy = true;
                    unit.Add(ull);
                }
                else
                {
                    ull.Catalague = muName;
                    ull.FirstTitleID = pCata.MarketBookCatalogID;
                    ull.SecondTitleID = item.MarketBookCatalogID;
                    ull.IsStudy = false;
                    unit.Add(ull);
                }
            }

            if (unit.Count <= 0)
            {
                return APIResponse.GetErrorResponse("暂无数据！");
            }
            else
            {
                unit = unit.OrderBy(i => i.FirstTitleID).ThenBy(i => i.SecondTitleID).Skip(pageNumber * 10).Take(10).ToList();
                return APIResponse.GetResponse(unit);
            }
        }

        public APIResponse GetVideoDetailsByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId)
        {
            IBS_ClassUserRelation userClassList = ibsService.GetClassUserRelationByClassOtherId(classId, 1);
            return GetVideoDetailsByModuleId(bookId, classId, firstTitleId, secondTitleId, userClassList);
        }

        public APIResponse GetYxVideoDetailsByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId)
        {
            IBS_ClassUserRelation userClassList = ibsService.GetClassUserRelationByClassOtherId(classId, 1);
            return GetVideoDetailsByModuleId(bookId, classId, firstTitleId, secondTitleId, userClassList);
        }

        /// <summary>
        /// 根据班级Id、册别、目录统计趣配音学习情况
        /// </summary>
        /// <param name="bookId">书籍id</param>
        /// <param name="classId">班级ID</param>
        /// <param name="firstTitleId">单元ID</param>
        /// <param name="secondTitleId">模块ID</param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public APIResponse GetVideoDetailsByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId, IBS_ClassUserRelation userClassList)
        {
            int ModelType = 1;
            var intBookId = int.Parse(bookId);
            System.Linq.Expressions.Expression<Func<MarketBookCatalog, bool>> expression = o => 1 == 1;

            if (secondTitleId != "0" || !string.IsNullOrEmpty(secondTitleId))
            {
                //where = string.Format(" AND FirstTitleID = '{0}'  AND SecondTitleID = '{1}' ", firstTitleId, secondTitleId);
                int sTitleId = int.Parse(secondTitleId);
                expression = o => o.MarketBookCatalogID == sTitleId;
            }
            else
            {
                //   where = string.Format(" AND FirstTitleID = '{0}' ", firstTitleId);
                int fTitleId = int.Parse(firstTitleId);
                expression = o => o.MarketBookCatalogID == fTitleId;
            }

            var catalog = repository.FirstOrDefault<MarketBookCatalog>(expression);

            List<VideoDetailsList> videoList = new List<VideoDetailsList>();

            var vList = GetModResource(new EnglishResourceModel { catalogueId = catalog.MarketBookCatalogID, type = (int)MODSourceTypeEnum.IntestingDubbing }).Data as List<Dubbing>;

            foreach (var v in vList)
            {
                int cl = 0;
                try
                {
                    foreach (var stu in userClassList.ClassStuList)
                    {
                        string value = redis.Get("Rds_StudyReport_ModuleTitle_" + stu.StuID.ToString().Substring(0, 2), stu.StuID + "_" + Subject + "_" + ModelType);
                        if (!string.IsNullOrEmpty(value))
                        {
                            Rds_StudyReport_ModuleTitle study = value.ToObject<Rds_StudyReport_ModuleTitle>();
                            foreach (var userinfo in study.detail)
                            {
                                if (userinfo.BookID == bookId.ToInt() && userinfo.VideoNumber == v.seq)
                                {
                                    cl++;
                                }
                            }
                        }
                    }
                    VideoDetailsList vlist = new VideoDetailsList()
                    {
                        ClassStudentCount = userClassList.ClassStuList.Count,
                        StudentStudyCount = cl,
                        ModuleId = 2,
                        ModuleName = "趣配音",
                        BookID = bookId.ToInt(),
                        VideoNumber = Convert.ToInt32(v.seq),
                        VideoTitle = v.title
                    };
                    videoList.Add(vlist);
                }

                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.ApiExceptionLog, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + v.seq, ex);
                }
            }
            return APIResponse.GetResponse(videoList.OrderBy(i => i.VideoNumber));
        }

        public APIResponse GetClassStudyDetailsByClassId(string classId, string bookId, int videoNumber, int pageNumber,int  modelType)
        {
            IBS_ClassUserRelation userClassList = ibsService.GetClassUserRelationByClassOtherId(classId, 1);
            return GetClassStudyDetailsByClassId(classId, bookId, videoNumber, pageNumber, userClassList,modelType);
        }

        public APIResponse GetYxClassStudyDetailsByClassId(string classId, string bookId, int videoNumber, int pageNumber,int modelType)
        {
            IBS_ClassUserRelation userClassList = ibsService.GetClassUserRelationByClassOtherId(classId, 1);
            return GetClassStudyDetailsByClassId(classId, bookId, videoNumber, pageNumber, userClassList, modelType);
        }


        /// <summary>
        /// 根据书籍ID,班级ID获取班级详细学习情况
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="bookId">书籍ID</param>
        /// <param name="videoNumber">视频序号</param>
        /// <param name="pageNumber">分页代码</param>
        /// <returns></returns>
        public APIResponse GetClassStudyDetailsByClassId(string classId, string bookId, int videoNumber, int pageNumber, IBS_ClassUserRelation userClass,int ModelType)
        {
            double count = 0;
            double maxScore = 0;
#pragma warning disable CS0219 // 变量“sort”已被赋值，但从未使用过它的值
            int sort = 1;
#pragma warning restore CS0219 // 变量“sort”已被赋值，但从未使用过它的值
            double minScore = 100;
            int cl = 0;
            string ImgUrl = "";
            UserVideosInfo uvi;
            List<UserVideosInfo> uservideoinfo = new List<UserVideosInfo>();
            if (userClass != null)
            {
                foreach (var item in userClass.ClassStuList)
                {
                    Rds_StudyReport_ModuleTitle module = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + item.StuID.ToString().Substring(0, 2), item.StuID + "_" + Subject + "_" + ModelType);
                    uvi = new UserVideosInfo();
                    if (module != null)
                    {
                        Rds_StudyReport_BookDetail rdsBookDetail = module.detail.FirstOrDefault(i => i.BookID == bookId.ToInt() && i.VideoNumber == videoNumber);
                        if (rdsBookDetail != null)
                        {
                            Rds_StudyReport_BookCatalogues_BookID BookCatalogues = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + bookId, rdsBookDetail.FirstTitleID + "_" + rdsBookDetail.SecondTitleID + "_" + ModelType);
                            if (BookCatalogues != null)
                            {
                                foreach (var vi in BookCatalogues.Videos)
                                {
                                    if (vi.VideoNumber == videoNumber)
                                    {
                                        ImgUrl = vi.IsEnableOss != 0
                                            ? _getOssFilesUrl + vi.VideoImageAddress
                                            : _getFilesUrl + "?FileID=" + vi.VideoImageAddress;
                                    }
                                }
                            }
                            rdsBookDetail.BestScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                            cl++;
                            if (Convert.ToDouble(rdsBookDetail.BestScore) > maxScore)
                            {
                                maxScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                            }
                            if (Convert.ToDouble(rdsBookDetail.BestScore) <= minScore)
                            {
                                minScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                            }
                            count += Convert.ToDouble(rdsBookDetail.BestScore.ToString("0.0"));

                            if (!string.IsNullOrEmpty(item.StuName))
                            {
                                uvi.UserName = item.StuName;
                            }
                            else
                            {
                                uvi.UserName = "暂未填写";
                            }

                            string imgUrl = item.IsEnableOss != 0 ? _getOssFilesUrl + item.UserImage : _getFilesUrl + "?FileID=" + item.UserImage;
                            uvi.UserId = (int)item.StuID;//返回UserId
                            if (item.UserImage != null) uvi.UserImg = imgUrl;

                            uvi.DubTimes = rdsBookDetail.DubbingNum;
                            uvi.VedioId = Convert.ToInt32(rdsBookDetail.VideoID);
                            uvi.IsStudy = true;
                            uvi.CreateTime = rdsBookDetail.CreateTime;
                            uvi.Score = rdsBookDetail.BestScore.ToString("0.0");
                        }
                        else
                        {
                            uvi.UserId = Convert.ToInt32(item.StuID);
                            uvi.UserImg = item.IsEnableOss != 0
                                            ? _getOssFilesUrl + item.UserImage
                                            : _getFilesUrl + "?FileID=" + item.UserImage;
                            uvi.UserName = item.StuName;
                            uvi.Score = "0.0";
                            uvi.IsStudy = false;
                        }
                    }
                    else
                    {
                        uvi.UserId = Convert.ToInt32(item.StuID);
                        uvi.UserImg = item.IsEnableOss != 0
                                          ? _getOssFilesUrl + item.UserImage
                                          : _getFilesUrl + "?FileID=" + item.UserImage;
                        uvi.UserName = item.StuName;
                        uvi.Score = "0.0";
                        uvi.IsStudy = false;
                    }
                    uservideoinfo.Add(uvi);
                }
            }
            if (cl == 0)
            {
                minScore = 0;
            }
            //以时间为单位，降序排列
            uservideoinfo = uservideoinfo.OrderByDescending(i => i.IsStudy).ThenByDescending(i => Convert.ToDouble(i.Score)).Skip(pageNumber * 10).Take(10).ToList();
            object obj =
                new
                {
                    AverageScore = count <= 0 ? "0" : (count / cl).ToString("0.0"),
                    HighestScore = maxScore,
                    LowestScore = minScore,
                    ImgUrl = ImgUrl,
                    Students = uservideoinfo
                };
            return APIResponse.GetResponse(obj);//返回信息 

        }
        /// <summary>
        /// 获取取模Url地址
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public APIResponse GetAPIDistributeUrl(long UserID, int ModelID)
        {
            APIReturnData responseData = new APIReturnData();
            try
            {
                #region 是否分发判断
                string filepath = "Config/APIDistributeConfig.xml";
                string APIDistributeSwitch = XMLHelper.GetAppSetting(filepath, "APIDistributeSwitch");
                if (string.IsNullOrEmpty(APIDistributeSwitch) || APIDistributeSwitch != "1")//只有BaseAPI才设置开关,分流API不设置开关
                {
                    return APIResponse.GetErrorResponse("");
                }
                #endregion

                #region 读取分发Url列表
                var urls = XMLHelper.GetAppSettingList(filepath, "APIDistributeConfig", "HearResourceAPI");
                if (urls == null || urls.Count == 0)
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误);
                }
                #endregion

                #region 返回取模分发url
                int distributenum = (int)(UserID % urls.Count);//前两位取模
                string distributeurl = urls[distributenum];
                return APIResponse.GetResponse(distributeurl);
                #endregion
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误, LogLevelEnum.Error, ex);
            }
        }

        public APIResponse GetHearResourcesByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId, IBS_ClassUserRelation userClassList, string ModelType)
        {
            var intBookId = int.Parse(bookId);
            System.Linq.Expressions.Expression<Func<MarketBookCatalog, bool>> expression = o => 1 == 1;

            if (secondTitleId != "0" || !string.IsNullOrEmpty(secondTitleId))
            {
                //where = string.Format(" AND FirstTitleID = '{0}'  AND SecondTitleID = '{1}' ", firstTitleId, secondTitleId);
                int sTitleId = int.Parse(secondTitleId);
                expression = o => o.MarketBookCatalogID == sTitleId;
            }
            else
            {
                //   where = string.Format(" AND FirstTitleID = '{0}' ", firstTitleId);
                int fTitleId = int.Parse(firstTitleId);
                expression = o => o.MarketBookCatalogID == fTitleId;
            }

            var catalog = repository.FirstOrDefault<MarketBookCatalog>(expression);

            List<VideoDetailsList> videoList = new List<VideoDetailsList>();
            var vList = GetModResource(new EnglishResourceModel { catalogueId = catalog.MarketBookCatalogID, type = (int)MODSourceTypeEnum.IntestingDubbing }).Data as List<CBSS.Tbx.Contract.ViewModel.v_HearResources>;
            foreach (var v in vList)
            {
                int cl = 0;
#pragma warning disable CS0219 // 变量“Userid”已被赋值，但从未使用过它的值
                long Userid = 0;
#pragma warning restore CS0219 // 变量“Userid”已被赋值，但从未使用过它的值
                try
                {
                    foreach (var stu in userClassList.ClassStuList)
                    {
                        string value = redis.Get("Rds_StudyReport_ModuleTitle_" + stu.StuID.ToString().Substring(0, 2), stu.StuID + "_" + Subject + "_" + ModelType);
                        if (!string.IsNullOrEmpty(value))
                        {
                            Rds_StudyReport_ModuleTitle study = value.ToObject<Rds_StudyReport_ModuleTitle>();
                            var rdsBookDetail = study.detail.FirstOrDefault(i => i.BookID == bookId.ToInt() && i.FirstTitleID == v.FirstTitleID && i.SecondTitleID == v.SecondTitleID
                                                                                 && i.FirstModularID == v.FirstModularID && i.SecondModularID == v.SecondModularID);
                            if (rdsBookDetail != null)
                            {
                                cl++;
                            }
                        }
                    }

                    VideoDetailsList vlist = new VideoDetailsList()
                    {
                        ClassStudentCount = userClassList.ClassStuList.Count,
                        StudentStudyCount = cl,
                        ModuleId = 3,
                        ModuleName = "说说看",
                        BookID = bookId.ToInt(),
                        VideoNumber = Convert.ToInt32(v.SerialNumber),
                        VideoTitle = "说说看"
                    };
                    videoList.Add(vlist);

                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.ApiExceptionLog, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + v.SerialNumber, ex);
                }
            }
            return APIResponse.GetResponse(videoList.OrderBy(i => i.VideoNumber));
        }

        /// <summary>
        /// 说说看根据书籍ID,班级ID获取班级详细学习情况,
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="bookId">书籍ID</param>
        /// <param name="videoNumber">视频序号</param>
        /// <param name="pageNumber">分页代码</param>
        /// <returns></returns>
        public APIResponse GetHearLeaningClassStudyDetailsByClassId(string bookId, int FirstTitleID, int SecondTitleID, int FirstModularID, int videoNumber, int pageNumber, IBS_ClassUserRelation userClass, string ModelType, string AppID)
        {
            double count = 0;
            double maxScore = 0;
#pragma warning disable CS0219 // 变量“sort”已被赋值，但从未使用过它的值
            int sort = 1;
#pragma warning restore CS0219 // 变量“sort”已被赋值，但从未使用过它的值
            double minScore = 100;
            int cl = 0;
            string ImgUrl = "";
            UserVideosInfo uvi;
            List<UserVideosInfo> uservideoinfo = new List<UserVideosInfo>();
            if (userClass != null)
            {
                foreach (var item in userClass.ClassStuList)
                {
                    Rds_StudyReport_ModuleTitle module = redis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + item.StuID.ToString().Substring(0, 2), item.StuID + "_" + Subject + "_" + ModelType);
                    uvi = new UserVideosInfo();
                    if (module != null)
                    {
                        Rds_StudyReport_BookDetail rdsBookDetail = module.detail.FirstOrDefault(i => i.BookID == bookId.ToInt() && i.FirstTitleID == FirstTitleID && i.SecondTitleID == SecondTitleID && i.FirstModularID == FirstModularID && i.SecondModularID == videoNumber);
                        if (rdsBookDetail != null)
                        {
                            Rds_StudyReport_BookCatalogues_BookID BookCatalogues = redis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + bookId, rdsBookDetail.FirstTitleID + "_" + rdsBookDetail.SecondTitleID + "_" + ModelType);
                            if (BookCatalogues != null)
                            {
                                foreach (var vi in BookCatalogues.Videos)
                                {
                                    if (vi.SecondModularID == videoNumber)
                                    {
                                        ImgUrl = vi.IsEnableOss != 0
                                            ? _getOssFilesUrl + vi.VideoImageAddress
                                            : _getFilesUrl + "?FileID=" + vi.VideoImageAddress;
                                    }
                                }
                            }
                            rdsBookDetail.BestScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
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

                            if (!string.IsNullOrEmpty(item.StuName))
                            {
                                uvi.UserName = item.StuName;
                            }
                            else
                            {
                                uvi.UserName = "暂未填写";
                            }

                            string imgUrl = item.IsEnableOss != 0 ? _getOssFilesUrl + item.UserImage : _getFilesUrl + "?FileID=" + item.UserImage;
                            uvi.UserId = (int)item.StuID;//返回UserId
                            if (item.UserImage != null) uvi.UserImg = imgUrl;

                            uvi.DubTimes = rdsBookDetail.DubbingNum;
                            uvi.VedioId = Convert.ToInt32(rdsBookDetail.VideoID);
                            uvi.IsStudy = true;
                            uvi.CreateTime = rdsBookDetail.CreateTime;
                            uvi.Score = rdsBookDetail.BestScore.ToString("0.0");
                            uvi.LinkURL = LinkUrl + "?UserID=" + item.StuID + "&BookID=" + rdsBookDetail.BookID + "&FirstTitleID=" + rdsBookDetail.FirstTitleID + "&SecondTitleID=" + rdsBookDetail.SecondTitleID + "&FirstModularID=" + rdsBookDetail.FirstModularID + "&SecondModularID=" + rdsBookDetail.SecondModularID + "&AppID=" + AppID;
                        }
                        else
                        {
                            uvi.UserId = Convert.ToInt32(item.StuID);
                            uvi.UserImg = item.IsEnableOss != 0
                                            ? _getOssFilesUrl + item.UserImage
                                            : _getFilesUrl + "?FileID=" + item.UserImage;
                            uvi.UserName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                            uvi.Score = "0.0";
                            uvi.IsStudy = false;
                        }
                    }
                    else
                    {
                        uvi.UserId = Convert.ToInt32(item.StuID);
                        uvi.UserImg = item.IsEnableOss != 0
                                          ? _getOssFilesUrl + item.UserImage
                                          : _getFilesUrl + "?FileID=" + item.UserImage;
                        uvi.UserName = string.IsNullOrEmpty(item.StuName) ? "暂未填写" : item.StuName;
                        uvi.Score = "0.0";
                        uvi.IsStudy = false;
                    }
                    uservideoinfo.Add(uvi);
                }
            }
            if (cl == 0)
            {
                minScore = 0;
            }
            //以时间为单位，降序排列
            uservideoinfo = uservideoinfo.OrderByDescending(i => i.IsStudy).ThenByDescending(i => i.Score).Skip(pageNumber * 10).Take(10).ToList();
            object obj =
                new
                {
                    AverageScore = count <= 0 ? "0" : (count / cl).ToString("0.0"),
                    HighestScore = maxScore,
                    LowestScore = minScore,
                    ImgUrl = ImgUrl,
                    Students = uservideoinfo
                };
            return APIResponse.GetResponse(obj);//返回信息 

        }
    }
}
