namespace Kingsun.ExamPaper.WebAPI.Models
{
    public class UserModel
    {
        /// <summary>
        /// 同步学本地缓存的BookID
        /// </summary>
        public int? OtherBookID { get; set; }
        public string UserID { get; set; }
        /// <summary>
        /// 目录ID
        /// </summary>
        public int? CatalogID { get; set; }
        /// <summary>
        /// （用于GetQuestionList接口）0：查看报告，1：进入做题
        /// </summary>
        public int? IsDo { get; set; }
    }
}