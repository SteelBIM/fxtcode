using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    public enum SecondModularEnum
    {
        [System.ComponentModel.Description("跟读单词")]
        FollowWord=1001,
        [System.ComponentModel.Description("跟读句子")]
        FollowSentense =1002,
        [System.ComponentModel.Description("跟读课文")]
        FollowArticle = 1003,
        [System.ComponentModel.Description("跟读语音")]
        FollowVoice=1004
    }
}
