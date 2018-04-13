using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class KeyAndType
    {
        public string key { get; set; }

        public TypeOfReport Type { get; set; }
    }

    public enum TypeOfReport
    {
        WordRead = 0,//单词跟读
        WordDictation,//单词听写
        ArticleRead//课文朗读
    }
}
