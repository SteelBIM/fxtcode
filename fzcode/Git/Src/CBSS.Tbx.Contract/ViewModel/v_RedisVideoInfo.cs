using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_RedisVideoInfo
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

}
