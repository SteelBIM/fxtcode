using CAS.Entity.SurveyDBEntity;
using System.Collections.Generic;

namespace CAS.Entity.SurveyEntityNew
{
    public class SYSCodeExt : SYSCode
    {
        public string showname { get; set; }
    }
    /// <summary>
    /// 比较属性
    /// </summary>
    public class SYSCodeCompare : IEqualityComparer<SYSCodeExt>
    {

        public bool Equals(SYSCodeExt x, SYSCodeExt y)
        {
            if (x.codename == y.codename)       //分别对属性进行比较
                return true;
            else
                return false;   
        }

        public int GetHashCode(SYSCodeExt obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
