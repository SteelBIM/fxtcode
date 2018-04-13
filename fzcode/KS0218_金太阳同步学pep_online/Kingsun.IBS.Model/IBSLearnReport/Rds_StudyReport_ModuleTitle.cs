using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.IBSLearnReport
{
    public class Rds_StudyReport_ModuleTitle
    {
        public int UserID { get; set; }

        public List<Rds_StudyReport_BookDetail> detail { get; set; }

        public Rds_StudyReport_ModuleTitle()
        {
            detail=new List<Rds_StudyReport_BookDetail>();            
        }


    }


    public class Rds_StudyReport_BookDetail
    {
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public string VideoID { get; set; }
        public double BestScore { get; set; }
        public string CreateTime { get; set; }
        public int? FirstTitleID { get; set; }
        public int? SecondTitleID { get; set; }
        public int? FirstModularID { get; set; }
        public int? SecondModularID { get; set; }
        public int DubbingNum { get; set; }

    }

    public class Rds_StudyReport_BookCatalogues_BookID
    {

        public int FirstTitleID { get; set; }

        public string FirstTitle { get; set; }

        public int SecondTitleID { get; set; }

        public string SecondTitle { get; set; }

        public int FirstModularID { get; set; }

        public string FirstModular { get; set; }

        public List<Rds_StudyReport_BookCatalogues_Video> Videos { get; set; }

        public Rds_StudyReport_BookCatalogues_BookID()
        {
            Videos=new List<Rds_StudyReport_BookCatalogues_Video>();
        }
    }

    public class Rds_StudyReport_BookCatalogues_Video
    {
        public string VideoImageAddress { get; set; }

        public int IsEnableOss { get; set; }

        public string VideoTitle { get; set; }

        public int VideoNumber { get; set; }
        public int? SecondModularID { get; set; }
    }

    public class Rds_StudyReport_Class
    {
        public string ClassID { get; set; }

        public int SubjectID { get; set; }
        /// <summary>
        /// 类型 1趣配音 2单元测试 3说说看
        /// </summary>
        public int ModuleType { get; set; }
        /// <summary>
        /// 0-未读 1-已读
        /// </summary>
        public int Flag { get; set; }
    }
    public class Rds_StudyReport_Book : Rds_StudyReport_Class
    {
        public int BookID { get; set; }
        /// <summary>
        /// 类型 1趣配音 2单元测试 3说说看
        /// </summary>
        public int ModuleType { get; set; }
    }
    public class Rds_StudyReport_Module : Rds_StudyReport_Book
    {
        public int FirstTitleID { get; set; }
        public string FirstTitle { get; set; }
        public int? SecondTitleID { get; set; }
        public string SecondTitle { get; set; }
     
    }
}
