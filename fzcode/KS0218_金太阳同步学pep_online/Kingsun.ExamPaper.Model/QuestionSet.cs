using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 单题模板
    /// </summary>
    public class QuestionSet
    {
        public string QuestionID { get; set; }//题号
        public string QuestionTitle { get; set; }//题目标题
        public string QuestionModel { get; set; }//题目模板
        public string QuestionContent { get; set; }//题目内容
        public string SecondContent { get; set; }//题目内容
        public string Mp3Url { get; set; }
        public string ImgUrl { get; set; }
        public string QuestionAnswer { get; set; }//题目答案
        public string ParentID { get; set; }
        public int? UnitID { get; set; }//单元
        public string Section { get; set; }//part
        public int? Sort { get; set; }//排序
        public int? QuestionTime { get; set; }//预计用时
        public int? Difficulty { get; set; }//难度
        public int? IsSplit { get; set; }//是否可拆分
        public int? TaskQueSort { get; set; }//作业题目排序
        public int? Round { get; set; }//作业题目次数
        public int? WrongRate { get; set; }//题目错误率
        public int? MinQueCount { get; set; }//大题下的小题数目

        public IList<Tb_SelectItem> SelectList { get; set; }//选择题选项
        public IList<Tb_BlankAnswer> BlankAnswer { get; set; }//填空题答案
        //public Tb_StudentAnswer StuAnswer { get; set; }//学生答案
        //public IList<Tb_ReadRecord> ReadRecordList { get; set; }//学生跟读记录
    }
}
