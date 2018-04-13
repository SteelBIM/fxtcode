using CBSS.Framework.Contract.API;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.IBLL
{
    public interface IIBSResource
    {
        bool NewMod2IBSResource();
        /// <summary>
        /// 获取学科
        /// </summary>
        /// <returns></returns>
        APIResponse GetBookSubjects();
        /// <summary>
        /// 获取版本和书
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        APIResponse GetBooks(List<string> subjects,int stage);

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="catalogueLevel">目录层级(这个参数暂时好像无效！但必须传一个值)</param>
        /// <returns></returns>
        APIResponse GetCatalogs(int bookId, int catalogueLevel = 1);
        #region 学科，版本，书籍 下拉框数据
        /// <summary>
        /// 获取学科
        /// </summary>
        /// <returns></returns>
        List<MarketClassify> GetSubjectList();
        /// <summary>
        /// 获取版本
        /// </summary>
        /// <param name="subjects"></param>
        /// <returns></returns>
        List<V_MarketClassify> GetVersionList(List<string> subjects,int stage);
        /// <summary>
        /// 获取书籍
        /// </summary>
        /// <param name="subjects"></param>
        /// <param name="vers"></param>
        /// <returns></returns>
        List<V_MarketBook> GetBookList(List<string> subjects, List<long?> vers,int stage);
        #endregion

        /// <summary>
        /// 从ibs获取目录
        /// </summary>
        /// <param name="modBookId"></param>
        /// <returns></returns>
        APIResponse GetCatalogsFromDb(int modBookId);
        /// <summary>
        /// 从ibs获取书籍
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        APIResponse GetBooksFromIbsDb(long versionId, int stage);
        /// <summary>
        /// 获取课文朗读资源
        /// </summary>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        APIResponse GetArticle(int bookId, int moduleId, int modCataId);

        /// <summary>
        /// 电子书资源
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        APIResponse GetEBook(int bookId, string secretKeyId);

        /// <summary>
        /// 单词资源
        /// </summary>
        /// <param name="secretKeyId"></param>
        /// <param name="catalogueId"></param>
        /// <returns></returns>
        APIResponse GetWordsByCatalogId(string secretKeyId, int catalogueId, int size = 30);


        APIResponse GetWordsByBookId(string secretKeyId, int bookId, int size = 30);
        /// <summary>
        /// 获取同步听资源
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        APIResponse GetListen(int bookId, string secretKeyId);

        /// <summary>
        /// 跟读资源
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        APIResponse GetFollowRead(int bookId, int moduleId, int modCataId);
        /// <summary>
        /// 获取说说看资源目录（跟读单词 跟读句子 跟读课文 跟读语音）
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="modCataId"></param>
        /// <returns></returns>
        APIResponse GetHearResource(int bookId, int modCataId, int moduleId, EnglishResourceModel param);
        /// <summary>
        /// 通过书本和二级目录获取说说看资源
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="modCataId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetHearResource(int bookId, int modCataId, EnglishResourceModel param);
        /// <summary>
        /// 获取一级目录下的资源
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="catalogList"></param>
        /// <returns></returns>
        APIResponse GetHearResource(int bookId, Dictionary<int, int> catalogList, EnglishResourceModel param);
        /// <summary>
        /// 根据目录ID获取趣配音资源
        /// </summary>
        /// <param name="catalogID"></param>
        /// <returns></returns>
        APIResponse GetDubbingByCataId(int catalogID, EnglishResourceModel param);
        /// <summary>
        /// 根据书本ID获取趣配音资源
        /// </summary>
        /// <param name="catalogID"></param>
        /// <returns></returns>
        APIResponse GetDubbingByBookId(int bookID);
        /// <summary>
        /// 电子书资源
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        APIResponse GetPrimaryEBook(int bookId, string secretKeyId);
        /// <summary>
        /// 获取同步听资源
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="secretKeyId"></param>
        /// <param name="modCatalogueId"></param>
        /// <returns></returns>
        APIResponse GetPrimaryListen(int bookId, string secretKeyId);
        /// <summary>
        /// 获取口训试题
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        APIResponse GetTopicSets(int page, int rowNum, int type);
        /// <summary>
        /// 通过ID获取口训试题卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        APIResponse GetTopicSetById(int id);
       // APIResponse GetFollowRead(int bookId, int moduleId, int modCataId);

        /// <summary>
        /// 同步听音频,关于返回的json数据:1.type是目录时， 会拿到titles， 通过titles取题目
        /// 2.type是试卷时， examPaper内容有效，反之无效,3.type是试题集的时候
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        APIResponse GetExerciseAudio(int bookId);

        /// <summary>
        /// 获取资源密钥
        /// </summary>
        /// <param name="modBookId"></param>
        /// <returns></returns>
        APIResponse GetBookSecretKey(int modBookId);
    }
}
