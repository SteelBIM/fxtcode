using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    /// <summary>
    /// 班级与用户关系
    /// </summary>
    public class IBS_ClassUserRelation
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 班级编号
        /// </summary>
        public string ClassNum { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>
        public int GradeID { get; set; }

        /// <summary>
        /// 年级名称
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 学生列表
        /// </summary>
        public List<ClassStuS> ClassStuList { get; set; }

        /// <summary>
        /// 教师列表
        /// </summary>
        public List<ClassTchS> ClassTchList { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        public int SchID { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        public IBS_ClassUserRelation() 
        {
            ClassStuList = new List<ClassStuS>();
            ClassTchList = new List<ClassTchS>();
        }
    }

    /// <summary>
    /// 班级学生信息
    /// </summary>
    public class ClassStuS
    {
        /// <summary>
        /// 学生ID
        /// </summary>
        public long StuID { get; set; }
        /// <summary>
        /// 学生名称
        /// </summary>
        public string StuName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnableOss { get; set; }
    }

    /// <summary>
    /// 班级教师信息
    /// </summary>
    public class ClassTchS
    {
        /// <summary>
        /// 教师ID
        /// </summary>
        public long TchID { get; set; }
        /// <summary>
        /// 教师名称
        /// </summary>
        public string TchName { get; set; }

        /// <summary>
        /// 学科ID
        /// </summary>
        public int SubjectID { get; set; }

        /// <summary>
        /// 学科名称
        /// </summary>
        public string SubjectName { get; set; }

        public int IsEnableOss { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }
    }
}
