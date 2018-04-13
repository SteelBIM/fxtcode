using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    public enum ModuleTypeEnum
    {
        [System.ComponentModel.Description("趣配音")]
        InterestDebbing = 1,
        [System.ComponentModel.Description("单元测试")]
        ExamPaper = 2,
        [System.ComponentModel.Description("说说看")]
        HearResources = 3,
        [System.ComponentModel.Description("优学趣配音")]
        YXInterestDebbing = 4,
        [System.ComponentModel.Description("优学单元测试")]
        YXExamPaper = 5,
        [System.ComponentModel.Description("优学说说看")]
        YXHearResources = 6
    }
}
