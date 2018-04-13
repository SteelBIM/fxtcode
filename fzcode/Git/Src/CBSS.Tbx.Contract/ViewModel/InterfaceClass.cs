using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    /// <summary>
    /// 接口公共类
    /// </summary>

    /// <summary>
    /// 根据老师ID查询所属班级(参数)
    /// </summary>
    public class ClassInfoByTeaId
    {
        public string AppID { get; set; }
        public string UserId { get; set; }
        public string Version { get; set; }
    }

    /// <summary>
    /// 根据老师ID查询所属班级(结果集)
    /// </summary>
    public class ClassInfoList
    {
        public string Id { get; set; }
        public string ClassName { get; set; }
        public bool IsStudy { get; set; }
    }

    /// <summary>
    /// 根据班级Id查询年级学习人数(参数集)
    /// </summary>
    public class JuniorGradeInfoByClassId
    {
        public string AppId { get; set; }
        public string ClassId { get; set; }
        public string Version { get; set; }
        public int Channel { get; set; }
    }

    /// <summary>
    /// 根据班级Id查询年级学习人数(结果集)
    /// </summary>
    public class JuniorGradeInfoList
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public bool IsStudy { get; set; }
    }

    /// <summary>
    /// 班级学习人数
    /// </summary>
    public class ClassStudyNum
    {
        public int num { get; set; }
        public string ClassId { get; set; }
        public string ClassName { get; set; }
    }

    /// <summary>
    /// 根据书籍Id和班级Id查询单元学习人数(参数集)
    /// </summary>
    public class UnitLearningListByBookId
    {
        public string BookId { get; set; }
        public string ClassId { get; set; }
        public string Version { get; set; }
        public int PageNumber { get; set; }

        public int FirstTitleID { get; set; }

        public int SecondTitleID { get; set; }

        public int FirstModularID { get; set; }
        public int VideoNumber { get; set; }
        public int Channel { get; set; }
        public string AppID { get; set; }
    }

    /// <summary>
    /// 根据书籍Id和班级Id查询单元学习人数(数据库查询结果集)
    /// </summary>
    public class UnitLearningList
    {
        public int UserID { get; set; }
        public int StudentStudyCount { get; set; }
        public int BookID { get; set; }
        public int FirstTitleID { get; set; }
        public int SecondTitleID { get; set; }

        public int VideoNumber { get; set; }
    }

    /// <summary>
    /// 根据书籍Id和班级Id查询单元学习人数(返回结果集)
    /// </summary>
    public class UnitLearningInfoList
    {
        public int FirstTitleID { get; set; }
        public int SecondTitleID { get; set; }
        public bool IsStudy { get; set; }
        public string Catalague { get; set; }
    }

    /// <summary>
    /// 根据班级Id、册别、目录统计趣配音学习情况(结果集)
    /// </summary>
    public class VideoDetailsList
    {
        public int ClassStudentCount { get; set; }
        public int StudentStudyCount { get; set; }
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public string VideoTitle { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }

    /// <summary>
    /// 根据班级Id、册别、目录统计趣配音学习情况(参数集)
    /// </summary>
    public class VideoDetailsByModuleId
    {
        public string BookId { get; set; }
        public string FirstTitleID { get; set; }
        public string SecondTitleID { get; set; }
        public string ClassId { get; set; }
        public int PageNumber { get; set; }
        public string Version { get; set; }
        public int Channel { get; set; }

    }

    public class IUserVideoDetails
    {
        public int ID { get; set; }
        public double TotalScore { get; set; }
        public string VideoFileID { get; set; }
        public int State { get; set; }
        public int UserID { get; set; }
        public int IsEnableOss { get; set; }
        public DateTime CreateTime { get; set; }
        public string VideoType { get; set; }
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
    }

    public class VideoDetails
    {
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public string BookName { get; set; }
        public int FirstTitleID { get; set; }
        public string FirstTitle { get; set; }
        public int SecondTitleID { get; set; }
        public string SecondTitle { get; set; }
        public string VideoTitle { get; set; }
        public int FirstModularID { get; set; }
        public int SecondModularID { get; set; }
        public string ModularName { get; set; }
    }

    public class UInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public string TrueName { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class UserVideosInfo
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string CreateTime { get; set; }
        public string UserImg { get; set; }
        public int VedioId { get; set; }
        public string VedioName { get; set; }
        public string Score { get; set; }
        public int DubTimes { get; set; }
        public bool IsStudy { get; set; }
        public string LinkURL { get; set; }
    }

    /// <summary>
    /// 测评卷用户信息
    /// </summary>
    public class Tb_StuCatalog
    {
        public string StuCatID { get; set; }
        public string StuID { get; set; }
        public int CatalogID { get; set; }
        public decimal BestTotalScore { get; set; }
        public DateTime DoDate { get; set; }
        public int AnswerNum { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public int IsEnableOss { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
    }

    /// <summary>
    /// 测评卷用户信息
    /// </summary>
    public class UserExamInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CreateTime { get; set; }
        public string UserImg { get; set; }
        public string StuCatID { get; set; }
        public double Score { get; set; }
        public int AnswerNum { get; set; }
        public bool IsStudy { get; set; }
        public string ShowName { get; set; }
    }

    /// <summary>
    /// 获取期末模拟测试卷 返回参数
    /// </summary>
    public class ExamPaperListModel
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
        /// <summary>
        /// 班级总人数
        /// </summary>
        public int ClassNum { get; set; }
        /// <summary>
        /// 做题人数
        /// </summary>
        public int QuestionNum { get; set; }
    }

    public class UserInfoListNum
    {
        /// <summary>
        /// 用户登录码
        /// </summary>
        public string UserNum { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassNum { get; set; }

        public ArrayList ComboInfo { get; set; }

        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string TelePhone { get; set; }
        public string NickName { get; set; }
        public string UserImage { get; set; }
        public string TrueName { get; set; }
        public int UserRoles { get; set; }
        /// <summary>
        /// 用户身份
        /// </summary>
        public int? UserType { get; set; }

        public string Token { get; set; }

        public int? SchoolID { get; set; }

        public string SchoolName { get; set; }

        /// <summary>
        /// 是否需要完善信息
        /// </summary>
        public string needImproveSource { get; set; }

    }


    public class tbmodulesort
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public int? ModuleID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 上级模块ID
        /// </summary>
        public int? SuperiorID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 所属教材ID
        /// </summary>
        public int? BookID { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        public int? FirstTitleID { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        public int? SecondTitleID { get; set; }


        public int ActiveState { get; set; }

    }

    public class versionchange
    {
        /// <summary>
        /// 模块版本号
        /// </summary>
        public string ModuleVersion { get; set; }

        /// <summary>
        /// 更新描述
        /// </summary>
        public string UpdateDescription { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? State { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool? IsUpdate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 增量包MD5
        /// </summary>
        public string IncrementalPacketMD5 { get; set; }

        /// <summary>
        /// 一级标题
        /// </summary>
        public string FirstTitle { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        public int? SecondTitleID { get; set; }

        /// <summary>
        /// 二级标题
        /// </summary>
        public string SecondTitle { get; set; }

        /// <summary>
        /// 模块地址
        /// </summary>
        public string ModuleAddress { get; set; }

        /// <summary>
        /// MD5值
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 增量包地址
        /// </summary>
        public string IncrementalPacketAddress { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 主模块ID
        /// </summary>
        public int? ModuleID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public int? BooKID { get; set; }

        /// <summary>
        /// 教材名称
        /// </summary>
        public string TeachingNaterialName { get; set; }

        /// <summary>
        /// 模块名
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        public int? FirstTitleID { get; set; }
    }
    public class RedisVideoInfo
    {
        public string VideoID { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public string VideoNumber { get; set; }
        public string TotalScore { get; set; }
        public List<string> NumberOfOraise { get; set; }
        public string CreateTime { get; set; }
        public string ClassID { get; set; }
        public string SchoolID { get; set; }
        public string TrueName { get; set; }
        public string UserImage { get; set; }
        /// <summary>
        /// 1:趣配音,2:单元测试，3：说说看
        /// </summary>
        public string ModuleType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FirstTitleID { get; set; }

        public string FirstTitle { get; set; }
        public string SecondTitleID { get; set; }
        public string SecondTitle { get; set; }
        public string VideoTitle { get; set; }
        public string VideoImageAddress { get; set; }
        public string FirstModularID { get; set; }
        public string FirstModular { get; set; }

        /// <summary>
        /// 说说看视频主序号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 说说看视频子序号
        /// </summary>
        public string TextSerialNumber { get; set; }
        public string VideoFileID { get; set; }

        public int DubbingNum { get; set; }

        public int IsEnableOss { get; set; }
        public string SecondModularID { get; set; }
    }

    public class VideoDetail
    {
        public int FirstTitleID { get; set; }
        public string FirstTitle { get; set; }
        public int SecondTitleID { get; set; }
        public string SecondTitle { get; set; }
        public int FirstModularID { get; set; }
        public string FirstModular { get; set; }
        public string VideoTitle { get; set; }
        public int VideoNumber { get; set; }
        public int BookID { get; set; }
        public int SecondModularID { get; set; }
    }
}
