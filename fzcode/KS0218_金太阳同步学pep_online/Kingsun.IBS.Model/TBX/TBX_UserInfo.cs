using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    public class TBX_UserInfo 
    {
        public IBS_UserInfo iBS_UserInfo { get; set; }

        public int ProvinceID { get; set; }
        public string Province { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }

        public DateTime? verifyEmailDate { get; set; }

        public int isVerifyEmailTrue { get; set; }

        public int BookID { get; set; }
        public string AppID { get; set; }

        /// <summary>
        /// 班级学校详情列表，学生一条数据，老师多条数据
        /// </summary>
        public List<ClassSchDetail> ClassSchDetailList { get; set; }

        public TBX_UserInfo() 
        {
            ClassSchDetailList = new List<ClassSchDetail>();
            iBS_UserInfo = new IBS_UserInfo();
        }
    }


    /// <summary>
    /// 班级学校，学生一条数据，老师多条数据
    /// </summary>
    public class ClassSchDetail : ClassSch
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 学科ID
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 年级ID
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        public string SchName { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public string AreaName { get; set; }
    }

}
