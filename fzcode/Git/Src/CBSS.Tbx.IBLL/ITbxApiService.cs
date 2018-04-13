using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using CBSS.IBS.Contract;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
  public  interface ITbxApiService
    {

        /// <summary>
        /// 同步学获取资源同一接口
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TBXSourceTypeEnum">1,Ebook 2.同步听 3.说说看  4趣配音</param>
        /// <returns></returns>
        APIResponse GetModResource(EnglishResourceModel model);
        /// <summary>
        /// 资源密钥
        /// </summary>
        /// <param name="marketBookId"></param>
        /// <returns></returns>
        APIResponse GetBookSecretKey(int marketBookId);
        /// <summary>
        /// 获取课文
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetArticle(EnglishResourceModel param);

        /// <summary>
        /// 获取趣配音资源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetIntestingDubbing(EnglishResourceModel param);
        /// <summary>
        /// 逐句跟读
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetFollowRead(EnglishResourceModel param);
        /// <summary>
        /// 获取说说看资源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetSecondModuleHearResource(EnglishResourceModel param);

        APIResponse GetHearResource(EnglishResourceModel param);
        /// <summary>
        /// ebook
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetPrimaryEBook(EnglishResourceModel param);
        /// <summary>
        /// 同步听
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetPrimaryListen(EnglishResourceModel param);
        /// <summary>
        /// ebook
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetEBook(EnglishResourceModel param);
        /// <summary>
        /// 同步听
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetListen(EnglishResourceModel param);
        /// <summary>
        /// 获取单词
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetEWords(EnglishResourceModel param);
        /// <summary>
        /// 获取新目标单词
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetRJXMBWords(EnglishResourceModel param);
        /// <summary>
        /// 获取练习册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        APIResponse GetExercise(EnglishResourceModel param);

        /// <summary>
        /// 根据老师ID查询班级信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="classid"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        APIResponse GetClassInfoByTeacherId(string appId, string classid, List<ClassInfoList> classList);

        /// <summary>
        /// 根据班级Id查询年级学习人数
        /// </summary>
        /// <param name="appId">版本ID</param>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        APIResponse GetJuniorGradeNumByClassId(string appId, string classId);

        /// <summary>
        /// 根据书籍id、班级Id获取单元模块下学习人数
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="classId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        APIResponse GetUnitLearningByBookId(string bookId, string classId, int pageNumber);
        /// <summary>
        /// 根据书籍Id和班级Id查询单元学习人数
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="classId"></param>
        /// <param name="firstTitleId"></param>
        /// <param name="secondTitleId"></param>
        /// <returns></returns>
        APIResponse GetVideoDetailsByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId);
        /// <summary>
        /// 根据班级Id、册别、目录统计趣配音学习情况
        /// </summary>
        /// <param name="bookId">书籍id</param>
        /// <param name="classId">班级ID</param>
        /// <param name="firstTitleId">单元ID</param>
        /// <param name="secondTitleId">模块ID</param>
        /// <param name="appId"></param>
        /// <returns></returns>
        APIResponse GetClassStudyDetailsByClassId(string classId, string bookId, int videoNumber, int pageNumber,int modelType);

        /// <summary>
        /// 接口取模分发
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="ModuleID">模型ID</param>
        /// <returns></returns>
        APIResponse GetAPIDistributeUrl(long UserID, int ModelID);

        /// <summary>
        /// 说说看 根据书籍Id和班级Id查询单元学习人数
        /// </summary>
        /// <param name="bookId">书籍ID</param>
        /// <param name="classId">班级ID</param>
        /// <param name="pageNumber">分页页码</param>
        /// <param name="appId"></param>
        /// <returns></returns>
         APIResponse GetUnitLearningByBookId(string bookId, string classId, int pageNumber, string ModelType);

        APIResponse GetHearResourcesByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId, IBS_ClassUserRelation userClassList, string ModelType);


        APIResponse GetHearLeaningClassStudyDetailsByClassId(string bookId, int FirstTitleID, int SecondTitleID, int FirstModularID, int videoNumber, int pageNumber, IBS_ClassUserRelation userClass, string ModelType, string AppID);

        /// <summary>
        /// 根据书籍id、班级Id获取单元模块下学习人数
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="classId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        APIResponse GetYxUnitLearningByBookId(string bookId, string classId, int pageNumber);

        APIResponse GetYxVideoDetailsByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId);

        APIResponse GetYxClassStudyDetailsByClassId(string classId, string bookId, int videoNumber, int pageNumber,int modelType);
    }
}
