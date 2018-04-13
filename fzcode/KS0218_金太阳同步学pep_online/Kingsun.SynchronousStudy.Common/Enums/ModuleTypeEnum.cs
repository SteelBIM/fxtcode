using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common.Enums
{
    public enum ModuleTypeEnum
    {
        [StringValue("趣配音")]
        InterestDebbing = 1,
        [StringValue("单元测试")]
        ExamPaper = 2,
        [StringValue("说说看")]
        HearResources = 3,
        [StringValue("优学趣配音")]
        YXInterestDebbing = 4,
        [StringValue("优学单元测试")]
        YXExamPaper =5,
        [StringValue("优学说说看")]
        YXHearResources = 6
    }
}
