using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract.IBSResource
{
    public class ModResponse
    {
        public int retcode { get; set; }

        public object data { get; set; }

    }

    public class SubjectResponse
    {
        public int retcode { get; set; }

        public SubjectModel data { get; set; }
    }

    public class SubjectModel
    {
        public Dictionary<string, string> courses { get; set; }
    }

    public class VersionResponse
    {
        public int retcode { get; set; }

        /// <summary>
        /// 版本数据
        /// </summary>
        public Version data { get; set; }
    }

    public class Version
    {
        public Dictionary<string, List<VersionDetail>> verList { get; set; }
    }

    public class VersionDetail
    {
        /// <summary>
        /// 版本ID
        /// </summary>
        public int versionId { get; set; }

        /// <summary>
        /// 版本名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 书籍List
        /// </summary>
        public List<Book> books { get; set; }
    }

    public class CurrentBook
    {
        public Book book { get; set; }
    }

    public class Book
    {
        /// <summary>
        /// 书籍ID
        /// </summary>
        public int bookId { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 书名（全名）
        /// </summary>
        public string fullName { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string coverUrl { get; set; }

        public int start { get; set; }

        public int end { get; set; }

        public int secretKeyId { get; set; }

        public string secretKey { get; set; }

        public int versionId { get; set; }

        public int stages { get; set; }

        public int gradeLevel { get; set; }

        public int volume { get; set; }

        public long courseId { get; set; }

        public List<Catalog> catalogues { get; set; }

    }

    public class BookInfoResponse
    {
        public int retcode { get; set; }

        /// <summary>
        /// 版本数据
        /// </summary>
        public BookInfo data { get; set; }
    }

    public class BookInfo
    {
        public Book book { get; set; }
    }

    public class Catalog
    {
        public int bookId { get; set; }

        public int id { get; set; }

        public int level { get; set; }

        public string title { get; set; }

        public int start { get; set; }

        public int end { get; set; }

        public string coverUrl { get; set; }

        public int parentId { get; set; }
        public bool hasRolePlay { get; set; }

        public List<Catalog> catalogues { get; set; }
    }

    public class ComResponse<T>
    {
        public int retcode { get; set; }

        public T data { get; set; }

    }


    public class ExerciseBooks
    {
        public List<ExerciseBook> exerciseBooks { get; set; }
    }
    public class ExerciseBook
    {
        public int id { get; set; }
        public int bookId { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public string remarks { get; set; }
        public int gradeLevel { get; set; }
        public long courseId { get; set; }
        public int versionId { get; set; }
        public int volume { get; set; }

        public List<category> categories { get; set; }
    }

    public class category
    {
        public int id { get; set; }

        public int exerBookId { get; set; }

        public string name { get; set; }

        //   public int sequence { get; set; }

        public int type { get; set; }

        public string audioUrl { get; set; }

        public int audioPlayTime { get; set; }

        //  public bool isImportNow { get; set; }

        public List<category> children { get; set; }

        public int level { get; set; }

        public List<object> titles { get; set; }

        public int parentId { get; set; }

        public examPaper examPaper { get; set; }


    }
    public class examPaper
    {
        public int id { get; set; }
        public string name { get; set; }
        public int stage { get; set; }
        public int gradeLevel { get; set; }
        public long courseId { get; set; }
        public int versionId { get; set; }
        //type:  {
        //    id: 0 {get;set;}
        //    name:  {get;set;}
        //    remarks:  {get;set;}
        //    createTime: 0
        //} {get;set;}
        public int year { get; set; }
        public int provinceCode { get; set; }
        public int status { get; set; }

        public int creatorId { get; set; }
        public int exerbookId { get; set; }
        public int isSynch { get; set; }
        public string audioUrl { get; set; }
        public int audioPlayTime { get; set; }
        public int volume { get; set; }

        public List<object> titles { get; set; }
    }
    public class FollowReadResposne
    {
        public List<FollowRead> catalogues { get; set; }
    }


    #region  趣配音
    public class DubbingByBookID
    {
        public Dictionary<int, List<Dubbing>> dubbings { get; set; }
    }

    public class DubbingByCataID
    {
        public List<Dubbing> dubbings { get; set; }
    }


    public class Dubbing
    {
        public int id { get; set; }

        public long bookId { get; set; }

        public long cataId { get; set; }
        /// <summary>
        /// 类型，1-课内配音 2-电影配音
        /// </summary>
        public int type { get; set; }

        public int seq { get; set; }

        public string title { get; set; }

        public string muteVideo { get; set; }
        public string fullVideo { get; set; }
        public string bgAudio { get; set; }
        public string coverImg { get; set; }

        public int difficulty { get; set; }

        public string desc { get; set; }

        public List<DubbingFragment> dubbingFragments { get; set; }

        public string audioEncryptUrl { get; set; }

        public int audioPlayTime { get; set; }
    }

    public class DubbingFragment
    {
        public int fragmentId { get; set; }

        public int subSeq { get; set; }

        public int fragmentStart { get; set; }

        public int fragmentEnd { get; set; }

        public string desc { get; set; }
    }
    #endregion

    #region 口训题目
    public class TopicSetReponse
    {
        public string retcode { get; set; }

        public TopicSetsReponse data { get; set; }

    }

    public class TopicSetReponseById
    {
        public string retcode { get; set; }

        public TopicSetsReponseById data { get; set; }

    }

    public class TopicSetsReponseById
    {
        public TopicSetModule topicSet { get; set; }

    }


    public class TopicSetsReponse
    {
        public List<TopicSetModule> topicSets { get; set; }

        public bool hasMore { get; set; }
    }


    public class TopicSetModule
    {
        public int id { get; set; }

        public string name { get; set; }

        public string year { get; set; }

        public int type { get; set; }

        public string startAudio { get; set; }

        public int startDuration { get; set; }

        public string endAudio { get; set; }
        public int endDuration { get; set; }

        public string testAudio { get; set; }

        public int testDuration { get; set; }

        public long createTime { get; set; }

        public ImitationReading imitationReading { get; set; }

        public InfoAcq infoAcq { get; set; }

        public InfoRepostAndQuery infoRepostAndQuery { get; set; }

    }

    public class InfoRepostAndQuery
    {
        public int id { get; set; }

        public string title { get; set; }

        public InfoRepost infoRepost { get; set; }

        public InfoQuery infoQuery { get; set; }
    }

    public class InfoRepost
    {
        public int repostId { get; set; }

        public string name { get; set; }
        public string content { get; set; }

        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public string image { get; set; }

        public int imageReadTime { get; set; }
        public string subContent { get; set; }

        public string subContentAudio { get; set; }

        public int subContentDuration { get; set; }

        public int readyTime { get; set; }
        public int recordingTime { get; set; }
        public string answer { get; set; }

        public string evaluateStd { get; set; }

        public int score { get; set; }
    }
    public class InfoQuery
    {
        public string id { get; set; }
        public string name { get; set; }
        public string content { get; set; }

        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public List<InfoQueryItem> infoQueryItems { get; set; }
    }

    public class InfoQueryItem
    {
        public int id { get; set; }
        public string item { get; set; }
        public int readyTime { get; set; }
        public int recordingTime { get; set; }
        public string answer { get; set; }

        public string evaluateStd { get; set; }

        public int score { get; set; }

    }
    public class ImitationReading
    {
        public int id { get; set; }
        public string title { get; set; }

        public string content { get; set; }

        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public string subContent { get; set; }

        public string subContentAudio { get; set; }

        public int subContentDuration { get; set; }

        public int readyTime { get; set; }

        public int recordingTime { get; set; }

        public string answer { get; set; }

        public string evaluateStd { get; set; }

        public int score { get; set; }
    }

    public class InfoAcq
    {
        public int id { get; set; }

        public string title { get; set; }

        public List<InfoAcqSection> infoAcqSections { get; set; }
    }

    public class InfoAcqSection
    {
        public int sectionId { get; set; }
        public string sectionName { get; set; }

        public List<InfoAcqSectionMains> infoAcqSectionMains { get; set; }
    }

    public class InfoAcqSectionMains
    {
        public int sectionMainId { get; set; }
        public string content { get; set; }
        public string contentAudio { get; set; }

        public int contentDuration { get; set; }

        public string subContent { get; set; }

        public string subContentAudio { get; set; }

        public int subContentDuration { get; set; }

        public int readyTime { get; set; }

        public List<InfoAcqItem> infoAcqItems { get; set; }

    }

    public class InfoAcqItem
    {
        public int sectionItemId { get; set; }

        public string item { get; set; }

        public string itemAudio { get; set; }

        public int itemDuration { get; set; }

        public int recordingTime { get; set; }

        public string answer { get; set; }

        public string evaluateStd { get; set; }
        public int score { get; set; }
    }

    #endregion


    public class FollowRead
    {
        //"catalogueId": 2594,
        //       "title": "Unit 1 Hello!",
        //       "modules": [
        //           {
        //               "moduleId": 245,
        //               "moduleName": "Vocabulary",
        //               "contents": [
        //                   {
        //                       "content": "at",
        //                       "coverUrl": "https://rescdn.kingsunedu.com/source/textbooks/GDBXXYYSNJSC9571/follow/U1/img/at.jpg",
        //                       "originSoundUrl": "https://rescdn.kingsunedu.com/source/textbooks/GDBXXYYSNJSC9571/follow/U1/sound/at.mp3",
        public int catalogueId { get; set; }
        public string title { get; set; }

        public List<FollowReadModule> modules { get; set; }
    }

    public class FollowReadModule
    {
        public int moduleId { get; set; }

        public string moduleName { get; set; }

        public int moduleType { get; set; }

        public List<ModuleContent> contents { get; set; }

        public int catalogId { get; set; }
    }

    public class ModuleContent
    {
        public string content { get; set; }

        public string coverUrl { get; set; }

        public string originSoundUrl { get; set; }

        public string encryptSoundUrl { get; set; }

        public int sort { get; set; }
        public string role { get; set; }

        public int duration { get; set; }
    }

    public class Pages<T>
    {
        public List<Page<T>> pages { get; set; }
    }

    public class Page<T>
    {
        public int pageId { get; set; }

        public int pageNumber { get; set; }

        public string originImgUrl { get; set; }

        public string encryptImgUrl { get; set; }

        public List<T> pieces { get; set; }
    }

    /// <summary>
    /// 课文朗读MODEL
    /// </summary>
    public class ArticlePiece
    {
        public int pieceId { get; set; }
        public string original { get; set; }

        public string translation { get; set; }

        public string originSoundUrl { get; set; }

        public string encryptSoundUrl { get; set; }

        public int duration { get; set; }

        // public coordinate coordinate { get; set; }
    }

    /// <summary>
    /// 电子书MODEL
    /// </summary>
    public class EBookPiece
    {
        public int pieceId { get; set; }
        public string original { get; set; }

        public string translation { get; set; }

        public string originSoundUrl { get; set; }

        public string encryptSoundUrl { get; set; }

        public int duration { get; set; }

        public coordinate coordinate { get; set; }
    }

    public class ListenPiece
    {
        public int pieceId { get; set; }
        //public string original { get; set; }

        //public string translation { get; set; }

        public string originSoundUrl { get; set; }

        public string encryptSoundUrl { get; set; }

        public int duration { get; set; }

        //public coordinate coordinate { get; set; }
    }

    public class coordinate
    {
        private decimal? x1 { get; set; }

        private decimal? y1 { get; set; }

        private decimal? width1 { get; set; }

        private decimal? height1 { get; set; }
        public decimal? x
        {
            get
            {
                return x1;
            }
            set
            {
                if (value == null)
                {
                    x1 = 0;
                }
                else
                {
                    x1 = value;
                }
            }
        }

        public decimal? y
        {
            get
            {
                return y1;
            }
            set
            {
                if (value == null)
                {
                    y1 = 0;
                }
                else
                {
                    y1 = value;
                }
            }
        }

        public decimal? width
        {
            get
            {
                return width1;
            }
            set
            {
                if (value == null)
                {
                    width1 = 0;
                }
                else
                {
                    width1 = value;
                }
            }
        }



        public decimal? height
        {
            get
            {
                return height1;
            }
            set
            {
                if (value == null)
                {
                    height1 = 0;
                }
                else
                {
                    height1 = value;
                }
            }
        }


    }
    public class Words
    {
        public List<Word> words { get; set; }
    }

    public class WordsByBookId
    {
        public Dictionary<int, List<Word>> words { get; set; }
    }


    /// <summary>
    /// 单词model
    /// </summary>
    public class Word
    {

        public int wordId { get; set; }

        /// <summary>
        /// 单词原文
        /// </summary>
        public string original { get; set; }

        /// <summary>
        /// 播放时长
        /// </summary>
        public int duration { get; set; }

        /// <summary>
        /// 加密链接
        /// </summary>
        public string encryptSoundUrl { get; set; }

        //public string originSoundUrl { get; set; }

        /// <summary>
        /// 译文
        /// </summary>
        public string translation { get; set; }

        public string originSoundUrl { get; set; }

        public string originImgUrl { get; set; }

        public string encryptImgUrl { get; set; }

        public string encryptSentenceImgUrl { get; set; }
        public string enMeaning { get; set; }

        public string structure { get; set; }

        public string phoneticSymbol { get; set; }

        public string similarWords { get; set; }
        public List<string> similarWordList { get; set; }
        public List<WordPhrase> phrases { get; set; }

        public List<WordSentence> sentences { get; set; }
    }

    public class WordPhrase
    {
        public int phraseId { get; set; }

        public int wordId { get; set; }

        public string phrase { get; set; }

        public string translation { get; set; }
    }

    public class WordSentence
    {
        public string sentenceId { get; set; }
        public int wordId { get; set; }
        public string sentence { get; set; }
        public string translation { get; set; }

        public string originSoundUrl { get; set; }

        public string encryptSoundUrl { get; set; }
        public int duration { get; set; }

        public string originImgUrl { get; set; }

        public string encryptImgUrl { get; set; }
    }

    public class EnglishArticleModel
    {

    }

}
