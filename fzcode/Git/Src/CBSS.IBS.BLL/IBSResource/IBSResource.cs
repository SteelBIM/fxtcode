using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.IBS.Contract.IBSResource;
using CBSS.IBS.IBLL;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Framework.Redis;
using CBSS.Core.Log;
using CBSS.Framework.Contract;
using CBSS.Core.Config;
using System.Web;
using CBSS.Framework.Contract.Enums;
using System.Data;

namespace CBSS.IBS.BLL
{
    public partial class IBSService : IIBSService
    {

        public bool NewMod2IBSResource()
        {
            try
            {
                GetModBooks();
                GetModCatalog();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "获取书本和目录失败", ex);
            }
            //GetMODBookSubjects();

            return true;
        }

        private void GetModCatalog()
        {

            //先实现逻辑 再改批量操作
            var listBook = repository.ListAll<MODBook>();
            foreach (var book in listBook)
            {
                string url = CachedConfigContext.Current.SettingConfig.ModRequestUrl + CachedConfigContext.Current.SettingConfig.CatalogApi + "?bookId=" + book.MODBookID + "&catalogueLevel=3";
                var str = HttpHelper.ModHttpGet(url);
                BookInfoResponse response = str.FromJson<BookInfoResponse>();
                var modBook = response.data.book;
                MODCatalogRecursive(modBook.catalogues);
            }
        }
        /// <summary>
        /// 递归映射目录
        /// </summary>
        /// <param name="catalogues"></param>
        /// <returns></returns>
        private void MODCatalogRecursive(List<Catalog> catalogues)
        {
            //if (catalogues == null || !catalogues.Any())
            //{
            //    Log4NetHelper.Info(LoggerType.ServiceExceptionLog, "目录终止！");
            //}
            catalogues.ForEach(o =>
            {
                try
                {
                    var catalog = repository.SelectSearch<MODBookCatalog>(a => a.MODBookCatalogID == o.id).FirstOrDefault();
                    if (catalog != null)
                    {
                        if (catalog.MODBookCatalogName == o.title && catalog.MODBookCatalogCover == o.coverUrl
                            && catalog.MODBookID == o.bookId)
                        {

                        }
                        else
                        {
                            catalog.MODBookCatalogLevel = o.level;
                            catalog.MODBookCatalogName = o.title;
                            catalog.MODBookCatalogCover = o.coverUrl;
                            catalog.MODBookID = o.bookId;
                            catalog.pageEnd = o.end;
                            catalog.PageStart = o.start;
                            catalog.ParentId = o.parentId;
                            repository.Update(catalog);
                        }
                    }
                    else
                    {
                        MODBookCatalog log = new MODBookCatalog();
                        log.MODBookCatalogID = o.id;
                        log.MODBookCatalogLevel = o.level;
                        log.MODBookCatalogName = o.title;
                        log.MODBookCatalogCover = o.coverUrl;
                        log.MODBookID = o.bookId;
                        log.pageEnd = o.end;
                        log.PageStart = o.start;
                        log.ParentId = o.parentId;
                        repository.Insert(log);
                    }
                    MODCatalogRecursive(o.catalogues);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "GetModCatalog异常！Data=" + o.ToJson(), ex);
                }

            });

        }
        private void GetModBooks()
        {
            List<int> stateList = new List<int>();
            stateList.Add(2);
            stateList.Add(4);
            stateList.Add(8);
            //先实现逻辑 再改批量操作
            var listSubject = StringEnumHelper.ToDictionaryKeyValue<SubjectTypeEnum>();
            if (listSubject != null)
            {
                try
                {
                    string xurl = CachedConfigContext.Current.SettingConfig.ModRequestUrl + CachedConfigContext.Current.SettingConfig.VersionsApi;
                    // string url = GetModRequestUrl() + GetVersionApiUrl();
                    foreach (var s in listSubject)
                    {
                        stateList.ForEach(state =>
                        {
                            string url = xurl + "?courses=" + s.Key + "&&stage=" + state;
                            var str = HttpHelper.ModHttpGet(url);
                            VersionResponse response = str.FromJson<VersionResponse>();
                            if (response.retcode == 0)
                            {
                                List<MODVersions> classifies = new List<MODVersions>();
                                var vers = response.data.verList;

                                var keys = vers.Keys;
                                foreach (var key in keys)
                                {
                                    var verDetails = vers[key];
                                    foreach (var d in verDetails)
                                    {
                                        var c = repository.SelectSearch<MODVersions>(a => a.MODVersionId == Convert.ToInt32(d.versionId) && a.SubjectId == s.Value).FirstOrDefault();
                                        if (c != null)
                                        {
                                            if (c.MODVersionName != d.name)
                                            {
                                                c.MODVersionName = d.name;
                                                repository.Update(c);
                                                foreach (var b in d.books)
                                                {
                                                    var dbbook = repository.SelectSearch<MODBook>(a => a.MODBookID == Convert.ToInt32(b.bookId) && a.VerId == d.versionId).FirstOrDefault();
                                                    if (dbbook != null)
                                                    {
                                                        if (dbbook.MODBookName == b.name && dbbook.MODBookFullName == b.fullName && dbbook.MODBookCover == b.coverUrl)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            dbbook.MODBookName = b.name;
                                                            dbbook.Stage = b.stages;
                                                            dbbook.MODBookFullName = b.fullName;
                                                            dbbook.MODBookCover = b.coverUrl;
                                                            dbbook.GradeID = b.gradeLevel;
                                                            dbbook.ReelID = b.volume;
                                                            dbbook.PageStart = b.start;
                                                            dbbook.PageEnd = b.end;
                                                            dbbook.VerId = b.versionId;
                                                            dbbook.SubjectID = b.courseId;
                                                            dbbook.MODBookKeyId = b.secretKeyId;
                                                            dbbook.MODBookKey = b.secretKey;

                                                            repository.Update(dbbook);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dbbook = new MODBook()
                                                        {
                                                            MODBookID = b.bookId,
                                                            MODBookName = b.name,
                                                            Stage = b.stages,
                                                            MODBookFullName = b.fullName,
                                                            MODBookCover = b.coverUrl,
                                                            GradeID = b.gradeLevel,
                                                            ReelID = b.volume,
                                                            PageStart = b.start,
                                                            PageEnd = b.end,
                                                            VerId = b.versionId,
                                                            MODBookKey = b.secretKey,
                                                            MODBookKeyId = b.secretKeyId,
                                                            SubjectID = b.courseId,
                                                        };
                                                        repository.Insert(dbbook);
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                foreach (var b in d.books)
                                                {
                                                    var dbbook = repository.SelectSearch<MODBook>(a => a.MODBookID == Convert.ToInt32(b.bookId) && a.VerId == d.versionId).FirstOrDefault();
                                                    if (dbbook != null)
                                                    {
                                                        if (dbbook.MODBookName == b.name && dbbook.MODBookFullName == b.fullName && dbbook.MODBookCover == b.coverUrl)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            dbbook.MODBookName = b.name;
                                                            dbbook.Stage = b.stages;
                                                            dbbook.MODBookFullName = b.fullName;
                                                            dbbook.MODBookCover = b.coverUrl;
                                                            dbbook.GradeID = b.gradeLevel;
                                                            dbbook.ReelID = b.volume;
                                                            dbbook.PageStart = b.start;
                                                            dbbook.PageEnd = b.end;
                                                            dbbook.VerId = b.versionId;
                                                            dbbook.SubjectID = b.courseId;
                                                            dbbook.MODBookKeyId = b.secretKeyId;
                                                            dbbook.MODBookKey = b.secretKey;
                                                            repository.Update(dbbook);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dbbook = new MODBook()
                                                        {
                                                            MODBookID = b.bookId,
                                                            MODBookName = b.name,
                                                            Stage = b.stages,
                                                            MODBookFullName = b.fullName,
                                                            MODBookCover = b.coverUrl,
                                                            GradeID = b.gradeLevel,
                                                            ReelID = b.volume,
                                                            PageStart = b.start,
                                                            PageEnd = b.end,
                                                            VerId = b.versionId,
                                                            MODBookKey = b.secretKey,
                                                            MODBookKeyId = b.secretKeyId,
                                                            SubjectID = b.courseId,
                                                        };
                                                        repository.Insert(dbbook);
                                                    }


                                                }
                                            }
                                        }
                                        else
                                        {
                                            c = new MODVersions();
                                            c.SubjectId = s.Value;
                                            c.MODVersionId = Convert.ToInt32(d.versionId);
                                            c.MODVersionName = d.name;
                                            foreach (var b in d.books)
                                            {
                                                MODBook book = new MODBook()
                                                {
                                                    MODBookID = b.bookId,
                                                    MODBookName = b.name,
                                                    MODBookFullName = b.fullName,
                                                    MODBookCover = b.coverUrl,
                                                    GradeID = b.gradeLevel,
                                                    Stage = b.stages,
                                                    ReelID = b.volume,
                                                    PageStart = b.start,
                                                    PageEnd = b.end,
                                                    VerId = b.versionId,
                                                    MODBookKey = b.secretKey,
                                                    MODBookKeyId = b.secretKeyId,
                                                    SubjectID = b.courseId,
                                                };
                                                repository.Insert(book);

                                            }
                                            repository.Insert(c);
                                        }



                                        classifies.Add(c);
                                    }
                                }
                            }
                        });


                    }

                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "获取科目信息异常！", ex);

                }
            }
        }

        public bool GetMODBookSubjects()
        {
            // string url = GetModRequestUrl() + GetSubjectsApiUrl();
            string url = CachedConfigContext.Current.SettingConfig.ModRequestUrl + CachedConfigContext.Current.SettingConfig.SubjectsApi;
            try
            {
                var str = HttpHelper.ModHttpGet(url);
                SubjectResponse response = str.FromJson<SubjectResponse>();
                if (response.retcode == 0)
                {
                    List<MODSubjects> subjectList = new List<MODSubjects>();
                    var subjects = response.data.courses;
                    var keys = subjects.Keys;
                    foreach (var key in keys)
                    {
                        var dbSubject = repository.SelectSearch<MODSubjects>(a => a.MODSubjectId == long.Parse(key)).FirstOrDefault();
                        if (dbSubject != null)
                        {
                            if (dbSubject.MODSubjectName != subjects[key])
                            {
                                dbSubject.MODSubjectName = subjects[key];
                                repository.Update(dbSubject);

                            }
                        }
                        else
                        {
                            MODSubjects subject = new MODSubjects();
                            subject.MODSubjectId = long.Parse(key);
                            subject.MODSubjectName = subjects[key];
                            subject.EnumValue = (int)(SubjectTypeEnum)Enum.Parse(typeof(SubjectTypeEnum), subjects[key]);
                            subject.CreateDate = DateTime.Now;
                            subject.CreateUser = 0;
                            repository.Insert(subject);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "获取科目信息异常！", ex);
                return false;
            }


        }


        public APIResponse GetBookSubjects()
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + GetSubjectsApiUrl();
            var str = HttpHelper.ModHttpGet(url);
            SubjectResponse response = str.FromJson<SubjectResponse>();
            if (response.retcode == 0)
            {
                List<MarketClassify> classifies = new List<MarketClassify>();
                var subjects = response.data.courses;

                var keys = subjects.Keys;
                foreach (var key in keys)
                {
                    MarketClassify c = new MarketClassify();
                    c.MODID = Convert.ToInt64(key);
                    c.MarketClassifyName = subjects[key];
                    c.ModClassifyName = subjects[key];
                    c.MODType = 1;
                    c.MarketID = 1;
                    c.ParentId = 0;
                    c.MarketClassifyProperty = "学科";
                    c.MODType = 1;
                    classifies.Add(c);
                }
                result.Data = classifies;
                result.Success = true;
            }
            return result;
        }
        /// <summary>
        /// 获取版本和书
        /// </summary>
        /// <param name="subject">科目，如：语文，英语</param>
        /// <returns></returns>
        public APIResponse GetBooks(List<string> subjects, int stage)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + GetVersionApiUrl();
            foreach (var s in subjects)
            {
                if (url.Contains("?courses="))
                    url += "&courses=" + s;
                else
                    url += "?courses=" + s;
            }
            url += "&stage=" + stage;
            var str = HttpHelper.ModHttpGet(url);
            VersionResponse response = str.FromJson<VersionResponse>();
            if (response.retcode == 0)
            {
                List<V_MarketClassify> classifies = new List<V_MarketClassify>();
                var vers = response.data.verList;

                var keys = vers.Keys;
                foreach (var key in keys)
                {
                    var verDetails = vers[key];
                    foreach (var d in verDetails)
                    {
                        V_MarketClassify c = new V_MarketClassify();
                        c.MarketBooks = new List<V_MarketBook>();
                        c.MODID = Convert.ToInt64(d.versionId);
                        c.MarketClassifyName = d.name;
                        c.ModClassifyName = d.name;
                        c.MODType = 2;
                        c.MarketID = 1;
                        c.MarketClassifyProperty = "版本";
                        c.MODType = 2;

                        foreach (var b in d.books)
                        {
                            V_MarketBook book = new V_MarketBook()
                            {
                                MarketBookCover = b.coverUrl,
                                MarketBookName = b.name,
                                MODBookCover = b.coverUrl,
                                MODBookName = b.fullName,
                                CreateDate = DateTime.Now,
                                MODBookID = b.bookId,
                            };
                            c.MarketBooks.Add(book);
                        }
                        classifies.Add(c);
                    }
                }
                result.Data = classifies;
                result.Success = true;
            }
            return result;
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="catalogueLevel">目录层级</param>
        /// <returns></returns>
        public APIResponse GetCatalogs(int bookId, int catalogueLevel = 2)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + GetCatalogApiUrl() + "?bookId=" + bookId + "&catalogueLevel=" + catalogueLevel;
            var str = HttpHelper.ModHttpGet(url);
            BookInfoResponse response = str.FromJson<BookInfoResponse>();

            var modBook = response.data.book;
            var catalogues = modBook.catalogues;

            V_MarketBook book = new V_MarketBook()
            {
                MODBookID = modBook.bookId,
                CreateDate = DateTime.Now,
                //目录多层级，递归赋值
                Catalogs = CatalogRecursive(modBook.catalogues)
            };

            result.Data = book.Catalogs;

            return result;
        }
        public APIResponse GetBooksFromIbsDb(long versionId, int stage)
        {
            var books = repository.SelectSearch<MODBook>(o => o.VerId == versionId && o.Stage == stage).ToList();
            return APIResponse.GetResponse(books);
        }
        public APIResponse GetCatalogsFromDb(int modBookId)
        {
            var catas = repository.SelectSearch<MODBookCatalog>(o => o.MODBookID == modBookId).ToList();

            var parent = catas.Where(o => o.ParentId == 0).ToList();

            List<V_MarketBookCatalog> parentCatas = new List<V_MarketBookCatalog>();

            if (parent.Any())
            {
                parentCatas = parent.Select(o => new V_MarketBookCatalog
                {
                    MarketBookCatalogCover = o.MODBookCatalogCover,
                    ParentCatalogID = 0,
                    V_MarketBookCatalogs = new List<V_MarketBookCatalog>(),
                    CreateDate = DateTime.Now,
                    MarketBookCatalogName = o.MODBookCatalogName,
                    MODBookCatalogID = o.MODBookCatalogID,
                    MODBookCatalogName = o.MODBookCatalogName,
                    MODBookCatalogCover = o.MODBookCatalogCover,
                    StartPage = o.PageStart,
                    EndPage = o.pageEnd
                }).ToList();
            }
            foreach (var p in parentCatas)
            {
                RecursionCatalog(p, catas);
            }

            return APIResponse.GetResponse(parentCatas);
        }

        public void RecursionCatalog(V_MarketBookCatalog p, List<MODBookCatalog> catas)
        {
            if (catas.Any(o => o.ParentId == p.MODBookCatalogID && o.MODBookCatalogLevel < 3))
            {
                var childs = catas.Where(o => o.ParentId == p.MODBookCatalogID && o.MODBookCatalogLevel < 3);
                p.V_MarketBookCatalogs.AddRange(childs.Select(o => new V_MarketBookCatalog
                {
                    MarketBookCatalogCover = o.MODBookCatalogCover,
                    MarketBookCatalogName = o.MODBookCatalogName,
                    StartPage = o.PageStart,
                    EndPage = o.pageEnd,
                    MODBookCatalogName = o.MODBookCatalogName,
                    MODBookCatalogCover = o.MODBookCatalogCover,
                    V_MarketBookCatalogs = new List<V_MarketBookCatalog>(),
                    MODBookCatalogID = o.MODBookCatalogID,
                }));

                foreach (var p2 in p.V_MarketBookCatalogs)
                {
                    RecursionCatalog(p2, catas);
                }
            }
        }

        /// <summary>
        /// 递归映射目录
        /// </summary>
        /// <param name="catalogues"></param>
        /// <returns></returns>
        List<V_MarketBookCatalog> CatalogRecursive(List<Catalog> catalogues)
        {
            if (catalogues == null || !catalogues.Any())
            {
                return new List<V_MarketBookCatalog>();//返回空list终止递归
            }
            return catalogues.Select(o => new V_MarketBookCatalog
            {
                CreateDate = DateTime.Now,
                MarketBookCatalogCover = o.coverUrl,
                MarketBookCatalogName = o.title,
                MODBookCatalogID = o.id,
                //目录多层级，递归赋值
                V_MarketBookCatalogs = CatalogRecursive(o.catalogues),
            }).ToList();
        }

        public APIResponse GetBookSecretKey(int modBookId)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/book/getBookById?bookId={0}", modBookId);
            var str = HttpHelper.ModHttpGet(url);

            var r = str.ToObject<ComResponse<CurrentBook>>();
            if (r.data == null || r.data.book == null)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到课本);
            }
            return APIResponse.GetResponse(new { r.data.book.secretKey });
        }

        /// <summary>
        ///  根据目录ID获取单词
        /// </summary>
        /// <param name="secretKeyId">秘钥ID 可空</param>
        /// <param name="catalogueId">目录ID</param>
        /// <returns></returns>
        public APIResponse GetWordsByCatalogId(string secretKeyId, int catalogueId, int size = 30)
        {
            APIResponse result = new APIResponse();
            string url = "";
            if (!secretKeyId.IsNullOrEmpty())
            {
                url = GetModRequestUrl() +
                      $"api/book/getWordsByCatalogueId?secretKeyId={secretKeyId}&catalogueId={catalogueId}&size={size}";
            }
            else
            {
                url = GetModRequestUrl() + $"api/book/getWordsByCatalogueId?catalogueId={catalogueId}&size={size}";
            }
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<ComResponse<Words>>();
            if (response.retcode == 0)
            {
                result.Success = true;
                result.Data = response.data;
            }
            else
            {
                result.Success = false;
                result.ErrorMsg = EnumHelper.GetEnumDesc<ModErrorCodeEnum>(response.retcode);
            }

            return result;
        }
        /// <summary>
        /// 根据书本ID获取单词
        /// </summary>
        /// <param name="secretKeyId">秘钥ID 可空</param>
        /// <param name="catalogueId">书本id</param>
        /// <returns></returns>
        public APIResponse GetWordsByBookId(string secretKeyId, int bookId, int size = 30)
        {
            APIResponse result = new APIResponse();
            string url = "";
            if (!secretKeyId.IsNullOrEmpty())
            {
                url = GetModRequestUrl() +
                      $"api/book/getWordsByBookId?secretKeyId={secretKeyId}&bookId={bookId}&size={size}";
            }
            else
            {
                url = GetModRequestUrl() + $"api/book/getWordsByBookId?bookId={bookId}&size={size}";
            }

            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<ComResponse<WordsByBookId>>();
            if (response.retcode == 0)
            {
                result.Success = true;
                result.Data = response.data;
            }
            else
            {
                result.Success = false;
                result.ErrorMsg = EnumHelper.GetEnumDesc<ModErrorCodeEnum>(response.retcode);
            }
            return result;
        }


        /// <summary>
        /// 课文朗读
        /// </summary>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        public APIResponse GetArticle(int bookId, int moduleId, int modCataId)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/book/getReadFollow?bookId={0}&moduleType={1}", bookId, moduleId);
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<ComResponse<FollowReadResposne>>();

            var d = response.data.catalogues.Where(o => o.catalogueId == modCataId).ToList();

            result.Data = d;
            return result;
        }

        /// <summary>
        /// 电子书
        /// </summary>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        public APIResponse GetEBook(int bookId, string secretKeyId)
        {
            APIResponse result = new APIResponse();
            //var cata = repository.SelectSearch<MODBookCatalog>(o => o.MODBookCatalogID == modCatalogueId).FirstOrDefault();
            string url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&secretKeyId={1}&start={2}&end={3}", bookId, secretKeyId, 1, int.MaxValue);
            if (string.IsNullOrWhiteSpace(secretKeyId))
            {
                url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&start={1}&end={2}", bookId, 1, int.MaxValue);
            }
            var str = HttpHelper.ModHttpGet(url);

            var response = str.FromJson<ComResponse<Pages<EBookPiece>>>();

            //result.Data = response.data.pages;
            result.Data = response.data.pages;
            return result;
        }

        public APIResponse GetListen(int bookId, string secretKeyId)
        {
            APIResponse result = new APIResponse();
            //var cata = repository.SelectSearch<MODBookCatalog>(o => o.MODBookCatalogID == modCatalogueId).FirstOrDefault();
            string url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&secretKeyId={1}&start={2}&end={3}", bookId, secretKeyId, 1, int.MaxValue);

            if (string.IsNullOrWhiteSpace(secretKeyId))
            {
                url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&start={1}&end={2}", bookId, 1, int.MaxValue);
            }

            var str = HttpHelper.ModHttpGet(url);

            var response = str.FromJson<ComResponse<Pages<ListenPiece>>>();

            var pages = response.data.pages;
            var d = pages.Select(o => new { pageNumber = o.pageNumber, encryptSoundUrls = o.pieces.Select(s => s.encryptSoundUrl) });
            d = d.Where(o => o.encryptSoundUrls.Count() > 0);

            List<ListenModel> soundUrls = new List<ListenModel>();

            foreach (var page in d)
            {
                foreach (var s in page.encryptSoundUrls)
                {
                    soundUrls.Add(new ListenModel { pageNum = page.pageNumber, soundUrl = s });
                }
            }
            result.Data = soundUrls;
            return result;
        }



        public APIResponse GetExerciseAudio(int bookId)
        {
            APIResponse result = new APIResponse();
            //var cata = repository.SelectSearch<MODBookCatalog>(o => o.MODBookCatalogID == modCatalogueId).FirstOrDefault();
            string url = GetModRequestUrl() + string.Format("api/questionBank/getExerciseBookByBookId?bookId={0}", bookId);
            var str = HttpHelper.ModHttpGet(url);

            var response = str.FromJson<ComResponse<ExerciseBooks>>();
            response.data.exerciseBooks.ForEach(o => o.categories.ForEach(c =>
          {
              if (c.examPaper != null && !string.IsNullOrEmpty(c.examPaper.audioUrl))
                  c.children.Add(new category { name = c.examPaper.name, audioUrl = c.examPaper.audioUrl });
          }
          ));
            var exercises = response.data.exerciseBooks.Where(o => o.name.Contains("练习册")).Select(o => o.categories.Select(c => new { c.name, c.audioUrl, c.examPaper, childrens = c.children.Select(ch => new { name = ch.name, audioUrl = ch.audioUrl }).ToList() })).FirstOrDefault();

            result.Data = exercises;
            result.Success = true;
            return result;
        }

        public APIResponse GetFollowRead(int bookId, int moduleId, int modCataId)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/book/getReadFollow?bookId={0}&moduleType={1}", bookId, moduleId);
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<ComResponse<FollowReadResposne>>();
            var d = response.data.catalogues.Where(o => o.catalogueId == modCataId).ToList();

            result.Data = d;
            return result;
        }


        public APIResponse GetHearResource(int bookId, int modCataId, int moduleId, EnglishResourceModel param)
        {
            List<v_HearResources> res = new List<v_HearResources>();
            APIResponse result = new APIResponse();
            string where = "";
            string sql = @"
                    select [ID],[BookID],[FirstTitleID],[SecondTitleID],[FirstModularID],[SecondModularID],
                    [ModularEN],[SerialNumber],[TextSerialNumber] ,[RoleName],[TextDesc] ,[AudioFrequency],[Picture],[RepeatNumber]
                      from [FZ_HearResources].[dbo].[TB_HearResources] a 
                      where {0}
                      group by [ID],[BookID],[FirstTitleID],[SecondTitleID],[FirstModularID] ,[SecondModularID]
                      ,[ModularEN],[SerialNumber],[TextSerialNumber],[RoleName],[TextDesc],[AudioFrequency],[Picture],[RepeatNumber]";
            where += " and a.BookID = " + param.bookId;
            where += " and a.FirstTitleID = " + param.FirstTitleID;
            if (!string.IsNullOrEmpty(param.SecondTitleID)&&param.SecondTitleID!="0")
            {
                where += " and SecondTitleID = " + param.SecondTitleID;
            }
            where += " and a.FirstModularID = " + param.FirstModularID;
            where += " and SecondModularID = '" + (param.SecondModularID) + "'";
            sql = string.Format(sql, where);
            DataTable dt = resources.SelectDataTable(sql);
            List<v_HearResources> s = DataTableHelper<v_HearResources>.ConvertToModel(dt).ToList();
            return APIResponse.GetResponse(s);
        }

        /*
        /// <summary>
        ///
        /// </summary>
        /// <param name="bookId">书籍</param>
        /// <param name="modCataId">目录</param>
        /// <param name="moduleId">类型 1001跟读单词 1002跟读句子 1003 跟读课文 1004跟读语音</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public APIResponse GetHearResource(int bookId, int modCataId, int moduleId, EnglishResourceModel param)
        {
            List<v_HearResources> res = new List<v_HearResources>();
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/book/getReadFollow?bookId={0}&moduleType={1}", bookId, moduleId);
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<ComResponse<FollowReadResposne>>();
            List<FollowReadModule> list = new List<FollowReadModule>();
            response.data.catalogues.Where(o => o.catalogueId == modCataId).ToList().ForEach(a =>
            {
                list.AddRange(a.modules);
            });
            list.ForEach(x =>
            {
                x.contents.ForEach(a =>
                {
                    v_HearResources tb = new v_HearResources();
                    tb.BookID = param.bookId;
                    tb.FirstTitleID = param.FirstTitleID;
                    tb.SecondTitleID = param.SecondTitleID.ToInt();
                    tb.AudioFrequency = a.originSoundUrl;
                    tb.FirstModularID = param.FirstModularID;
                    tb.ModularEN = x.moduleName;
                    tb.Picture = a.coverUrl;
                    tb.RepeatNumber = 3;
                    tb.RoleName = a.role;
                    tb.SecondModularID = moduleId;
                    tb.SerialNumber = a.sort;
                    tb.TextDesc = a.content;
                    tb.TextSerialNumber = 0;
                    res.Add(tb);
                });
            });


            result.Data = res;
            return result;
        }*/

        public APIResponse GetHearResource(int bookId, int modCataId, EnglishResourceModel param)
        {
            List<v_HearResources> res = new List<v_HearResources>();
            APIResponse result = new APIResponse();
            string where = "";
            string sql = @"
                    select [ID],[BookID],[FirstTitleID],[SecondTitleID],[FirstModularID],[SecondModularID],
                    [ModularEN],[SerialNumber],[TextSerialNumber] ,[RoleName],[TextDesc] ,[AudioFrequency],[Picture],[RepeatNumber]
                      from [FZ_HearResources].[dbo].[TB_HearResources] a 
                      where {0}
                      group by [ID],[BookID],[FirstTitleID],[SecondTitleID],[FirstModularID] ,[SecondModularID]
                      ,[ModularEN],[SerialNumber],[TextSerialNumber],[RoleName],[TextDesc],[AudioFrequency],[Picture],[RepeatNumber]";
            where += " and a.BookID = " + param.bookId;
            where += " and a.FirstTitleID = " + param.FirstTitleID;
            if (!string.IsNullOrEmpty(param.SecondTitleID) && param.SecondTitleID != "0")
            {
                where += " and SecondTitleID = " + param.SecondTitleID;
            }
            sql = string.Format(sql, where);
            DataTable dt = resources.SelectDataTable(sql);
            List<v_HearResources> s = DataTableHelper<v_HearResources>.ConvertToModel(dt).ToList();
            return APIResponse.GetResponse(s);
        }
            /*
                    public APIResponse GetHearResource(int bookId, int modCataId, EnglishResourceModel param)
                    {
                        List<v_HearResources> res = new List<v_HearResources>();
                        APIResponse result = new APIResponse();
                        var listSubject = StringEnumHelper.ToDictionaryKeyValue<SecondModularEnum>();
                        List<FollowReadModule> list = new List<FollowReadModule>();
                        foreach (var s in listSubject)
                        {
                            string url = GetModRequestUrl() + string.Format("api/book/getReadFollow?bookId={0}&moduleType={1}", bookId, s.Value);
                            var str = HttpHelper.ModHttpGet(url);
                            var response = str.FromJson<ComResponse<FollowReadResposne>>();
                            response.data.catalogues.Where(o => o.catalogueId == modCataId).ToList().ForEach(a =>
                            {
                                list.AddRange(a.modules);
                            });
                            list.ForEach(x =>
                            {
                                x.contents.ForEach(a =>
                                {
                                    v_HearResources tb = new v_HearResources();
                                    tb.BookID = param.bookId;
                                    tb.FirstTitleID = param.FirstTitleID;
                                    tb.SecondTitleID = param.catalogueId;
                                    tb.AudioFrequency = a.originSoundUrl;
                                    tb.FirstModularID = param.FirstModularID;
                                    tb.ModularEN = x.moduleName;
                                    tb.Picture = a.coverUrl;
                                    tb.RepeatNumber = 3;
                                    tb.RoleName = a.role;
                                    tb.SecondModularID = s.Value;
                                    tb.SerialNumber = a.sort;
                                    tb.TextDesc = a.content;
                                    tb.TextSerialNumber = 0;
                                    res.Add(tb);
                                });
                            });
                        }

                        result.Data = res;
                        return result;
                    }*/


            public APIResponse GetHearResource(int bookId, Dictionary<int, int> catalogList, EnglishResourceModel param)
        {
            List<v_HearResources> res = new List<v_HearResources>();
            APIResponse result = new APIResponse();
            string where = "";
            string sql = @"
                    select [ID],[BookID],[FirstTitleID],[SecondTitleID],[FirstModularID],[SecondModularID],
                    [ModularEN],[SerialNumber],[TextSerialNumber] ,[RoleName],[TextDesc] ,[AudioFrequency],[Picture],[RepeatNumber]
                      from [FZ_HearResources].[dbo].[TB_HearResources] a 
                      where {0}
                      group by [ID],[BookID],[FirstTitleID],[SecondTitleID],[FirstModularID] ,[SecondModularID]
                      ,[ModularEN],[SerialNumber],[TextSerialNumber],[RoleName],[TextDesc],[AudioFrequency],[Picture],[RepeatNumber]";
            where += " and a.BookID = " + param.bookId;
            where += " and a.FirstTitleID = " + param.FirstTitleID;
            where += " and a.FirstModularID = " + param.FirstModularID;
            sql = string.Format(sql, where);
            DataTable dt = resources.SelectDataTable(sql);
            List<v_HearResources> s = DataTableHelper<v_HearResources>.ConvertToModel(dt).ToList();
            return APIResponse.GetResponse(s);
        }

        /*
        public APIResponse GetHearResource(int bookId, Dictionary<int,int> catalogList,EnglishResourceModel param)
        {
            List<v_HearResources> res = new List<v_HearResources>();
            APIResponse result = new APIResponse();
            var listSubject = StringEnumHelper.ToDictionaryKeyValue<SecondModularEnum>();
            List<FollowReadModule> list = new List<FollowReadModule>();
            foreach (var s in listSubject)
            {
                string url = GetModRequestUrl() + string.Format("api/book/getReadFollow?bookId={0}&moduleType={1}", bookId, s.Value);
                var str = HttpHelper.ModHttpGet(url);
                var response = str.FromJson<ComResponse<FollowReadResposne>>();

                response.data.catalogues=response.data.catalogues.Where(o => catalogList.Keys.Contains(o.catalogueId)).ToList();
                response.data.catalogues.ForEach(a => { a.catalogueId = catalogList[a.catalogueId]; });
                response.data.catalogues.ForEach(a =>
                {
                    a.modules.ForEach(x => x.catalogId = a.catalogueId);
                    list.AddRange(a.modules);
                });
                list.ForEach(x =>
                {
                    x.contents.ForEach(a =>
                    {
                        v_HearResources tb = new v_HearResources();
                        tb.BookID = param.bookId;
                        tb.FirstTitleID = param.FirstTitleID;
                        tb.SecondTitleID = x.catalogId;
                        tb.AudioFrequency = a.originSoundUrl;
                        tb.FirstModularID = param.FirstModularID;
                        tb.ModularEN = x.moduleName;
                        tb.Picture = a.coverUrl;
                        tb.RepeatNumber = 3;
                        tb.RoleName = a.role;
                        tb.SecondModularID = s.Value;
                        tb.SerialNumber = a.sort;
                        tb.TextDesc = a.content;
                        tb.TextSerialNumber = 0;
                        res.Add(tb);
                    });
                });
            }
            result.Data = res;
            return result;
        }
        */

        public APIResponse GetDubbingByCataId(int catalogId, EnglishResourceModel param)
        {
            string where = ""; 
            where += "1=1 and a.BookID = " + param.bookId;
            where += " and a.FirstTitleID = " + param.FirstTitleID;
            where += " and a.FirstModularID = " + param.FirstModularID;
            if (!string.IsNullOrEmpty(param.SecondTitleID))
            {
                where += " and a.SecondTitleID = " + param.SecondTitleID;
            }
            string sql = string.Format(@"SELECT DISTINCT a.[ID] ,
                                                    a.[BookID] ,
                                                    a.BookName ,
                                                    a.[FirstTitleID] ,
                                                    a.[FirstTitle] ,
                                                    a.[SecondTitleID] ,
                                                    a.[SecondTitle] ,
                                                    a.[FirstModularID] ,
                                                    a.[FirstModular] ,
                                                    a.[SecondModularID] ,
                                                    a.[SecondModular] ,
                                                    a.[VideoTitle] ,
                                                    a.[VideoNumber] ,
                                                    a.[MuteVideo] ,
                                                    a.[CompleteVideo] ,
                                                    a.[VideoTime] ,
                                                    a.[BackgroundAudio] ,
                                                    a.[VideoCover] ,
                                                    a.[VideoDesc] ,
                                                    a.[VideoDifficulty] ,
                                                    a.[CreateTime] ,
                                                    b.ID 'BID' ,
                                                    b.VideoID ,
                                                    b.DialogueText ,
                                                    b.DialogueNumber ,
                                                    b.StartTime ,
                                                    b.EndTime ,
                                                    b.CreateTime 'BCreateTime'
                                            FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails] a
                                                    LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] b ON a.BookID = b.BookID
                                                                                                          AND a.VideoNumber = b.VideoID
                                                WHERE {0}
            ", where);
            DataTable dt = resources.SelectDataTable(sql);
            List<V_DubbingInfo> s = DataTableHelper<V_DubbingInfo>.ConvertToModel(dt).ToList();
            return APIResponse.GetResponse(s);
        }
        /*
        public APIResponse GetDubbingByCataId(int catalogId, EnglishResourceModel param)
        {
              APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/teachingResource/getDubbingByCataId?cataId={0}", catalogId);
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<ComResponse<DubbingByCataID>>();

            V_DubbingInfo dubbingInfo = new V_DubbingInfo();
            List<V_DubbingInfo> dubbingList = new List<V_DubbingInfo>();
            DubbingFragments fragments = new DubbingFragments();
         
            foreach (var item in response.data.dubbings)
            {
                dubbingInfo.ID = item.id;
                dubbingInfo.SecondModularID = item.type;
                dubbingInfo.SecondModular = item.type == 1 ? "课内配音" : "电影配音";
                dubbingInfo.VideoNumber = item.seq;
                dubbingInfo.VideoTitle = item.title;
                dubbingInfo.MuteVideo = item.muteVideo;
                dubbingInfo.CompleteVideo = item.fullVideo;
                dubbingInfo.BackgroundAudio = item.bgAudio;
                dubbingInfo.VideoCover = item.coverImg;
                dubbingInfo.VideoDifficulty = item.difficulty;
                dubbingInfo.VideoDesc = item.desc;

                if (item.dubbingFragments != null)
                {
                    foreach (var df in item.dubbingFragments)
                    {
                        fragments.ID = df.fragmentId;
                        fragments.DialogueText = df.desc;
                        fragments.DialogueNumber = df.subSeq;
                        fragments.StartTime = df.fragmentStart;
                        fragments.EndTime = df.fragmentEnd;
                        dubbingInfo.dubbingFragments.Add(fragments);
                    }
                }
                dubbingList.Add(dubbingInfo);
            }
            result.Data = dubbingList;
            result.Success = true;
            return result;
        }*/

        public APIResponse GetDubbingByBookId(int bookId)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/teachingResource/getDubbingByBookId?bookId={0}", bookId);
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<ComResponse<DubbingByBookID>>();
            result.Data = response.data;
            result.Success = true;
            return result;
        }


        /// <summary>
        /// 电子书
        /// </summary>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        public APIResponse GetPrimaryEBook(int bookId, string secretKeyId)
        {
            APIResponse result = new APIResponse();
            //var cata = repository.SelectSearch<MODBookCatalog>(o => o.MODBookCatalogID == modCatalogueId).FirstOrDefault();
            string url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&secretKeyId={1}&start={2}&end={3}", bookId, secretKeyId, 1, int.MaxValue);
            if (string.IsNullOrWhiteSpace(secretKeyId))
            {
                url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&start={1}&end={2}", bookId, 1, int.MaxValue);
            }
            var str = HttpHelper.ModHttpGet(url);

            var response = str.FromJson<ComResponse<Pages<EBookPiece>>>();

            //result.Data = response.data.pages;
            result.Data = response.data.pages;
            return result;
        }

        public APIResponse GetPrimaryListen(int bookId, string secretKeyId)
        {
            APIResponse result = new APIResponse();
            //var cata = repository.SelectSearch<MODBookCatalog>(o => o.MODBookCatalogID == modCatalogueId).FirstOrDefault();
            string url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&secretKeyId={1}&start={2}&end={3}", bookId, secretKeyId, 1, int.MaxValue);

            if (string.IsNullOrWhiteSpace(secretKeyId))
            {
                url = GetModRequestUrl() + string.Format("api/book/getBookPages?bookId={0}&start={1}&end={2}", bookId, 1, int.MaxValue);
            }

            var str = HttpHelper.ModHttpGet(url);

            var response = str.FromJson<ComResponse<Pages<ListenPiece>>>();

            var pages = response.data.pages;
            var d = pages.Select(o => new { pageNumber = o.pageNumber, encryptSoundUrls = o.pieces.Select(s => s.encryptSoundUrl) });
            d = d.Where(o => o.encryptSoundUrls.Count() > 0);

            List<ListenModel> soundUrls = new List<ListenModel>();

            foreach (var page in d)
            {
                foreach (var s in page.encryptSoundUrls)
                {
                    soundUrls.Add(new ListenModel { pageNum = page.pageNumber, soundUrl = s });
                }
            }
            result.Data = soundUrls;
            return result;
        }
        /// <summary>
        /// 获取口训题目
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rowNum">页数</param>
        /// <param name="type">类型  1-真题 2-模拟题</param>
        /// <returns></returns>
        public APIResponse GetTopicSets(int page, int rowNum, int type)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/teachingResource/getTopicSets?page={0}&rowNum={1}&type={2}", page, rowNum, type);
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<TopicSetReponse>();
            result.Data = response.data;
            result.Success = true;
            return result;
        }

        public APIResponse GetTopicSetById(int id)
        {
            APIResponse result = new APIResponse();
            string url = GetModRequestUrl() + string.Format("api/teachingResource/getTopicSetById?id={0}", id);
            var str = HttpHelper.ModHttpGet(url);
            var response = str.FromJson<TopicSetReponseById>();
            result.Data = response.data;
            result.Success = true;
            return result;
        }

        #region mod 学科，版本，书籍下拉框数据
        /// <summary>
        /// 获取学科
        /// </summary>
        /// <returns></returns>
        public List<MarketClassify> GetSubjectList()
        {
            return GetBookSubjects().Data as List<MarketClassify>;
        }
        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public List<V_MarketClassify> GetVersionList(List<string> subjects, int stage)
        {
            var v = GetBooks(subjects, stage).Data as List<V_MarketClassify>;
            return v;
        }
        /// <summary>
        /// 获取书籍
        /// </summary>
        /// <returns></returns>
        public List<V_MarketBook> GetBookList(List<string> subjects, List<long?> vers, int stage)
        {
            List<V_MarketBook> books = new List<V_MarketBook>();
            var vs = GetBooks(subjects, stage).Data as List<V_MarketClassify>;
            vs = vs.Where(o => vers.Contains(o.MODID)).ToList();
            foreach (var b in vs.Select(o => o.MarketBooks))
            {
                books.AddRange(b);
            }

            return books;
        }

        #endregion
        private string GetModRequestUrl()
        {
            string reqeustUrl = "";
            string xmlPath = System.Web.HttpRuntime.AppDomainAppPath;
            HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                xmlPath = context.Server.MapPath("~/Config/SettingConfig.xml");
            }
            else
            {
                xmlPath = xmlPath + "\\Config\\SettingConfig.xml";
            }
            XDocument xdc = new XDocument(XDocument.Load(xmlPath));

            var pElement = xdc.Elements().First();
            //List<DbEntity> dbs = new List<DbEntity>();
            foreach (var n in pElement.Elements())
            {
                //  dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
                if (n.Name.LocalName == "ModRequestUrl")
                {
                    reqeustUrl = n.Value;
                }
            }
            return reqeustUrl;
        }
        private string GetSubjectsApiUrl()
        {
            string reqeustUrl = "";
            string xmlPath = System.Web.HttpRuntime.AppDomainAppPath;
            HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                xmlPath = context.Server.MapPath("~/Config/SettingConfig.xml");
            }
            else
            {
                xmlPath = xmlPath + "/Config/SettingConfig.xml";
            }
            XDocument xdc = new XDocument(XDocument.Load(xmlPath));

            var pElement = xdc.Elements().First();
            //List<DbEntity> dbs = new List<DbEntity>();
            foreach (var n in pElement.Elements())
            {
                //  dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
                if (n.Name.LocalName == "SubjectsApi")
                {
                    reqeustUrl = n.Value;
                }
            }
            return reqeustUrl;
        }

        private string GetVersionApiUrl()
        {
            string reqeustUrl = "";
            string xmlPath = System.Web.HttpRuntime.AppDomainAppPath;
            HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                xmlPath = context.Server.MapPath("~/Config/SettingConfig.xml");
            }
            else
            {
                xmlPath = xmlPath + "/Config/SettingConfig.xml";
            }
            XDocument xdc = new XDocument(XDocument.Load(xmlPath));

            var pElement = xdc.Elements().First();
            //List<DbEntity> dbs = new List<DbEntity>();
            foreach (var n in pElement.Elements())
            {
                //  dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
                if (n.Name.LocalName == "VersionsApi")
                {
                    reqeustUrl = n.Value;
                }
            }
            return reqeustUrl;
        }

        private string GetCatalogApiUrl()
        {
            string reqeustUrl = "";
            string xmlPath = System.Web.HttpRuntime.AppDomainAppPath;
            HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                xmlPath = context.Server.MapPath("~/Config/SettingConfig.xml");
            }
            else
            {
                xmlPath = xmlPath + "/Config/SettingConfig.xml";
            }
            XDocument xdc = new XDocument(XDocument.Load(xmlPath));

            var pElement = xdc.Elements().First();
            //List<DbEntity> dbs = new List<DbEntity>();
            foreach (var n in pElement.Elements())
            {
                //  dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
                if (n.Name.LocalName == "CatalogApi")
                {
                    reqeustUrl = n.Value;
                }
            }
            return reqeustUrl;
        }

        public class ListenModel
        {
            public int pageNum { get; set; }

            public string soundUrl { get; set; }
        }

    }
}
