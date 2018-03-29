using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;
using CAS.Entity.AttributeHelper;

namespace CAS.Entity.GJBEntity
{
    public class RegexInfo
    {
        public RegexInfo(string _regexStr, string _regexIndex)
        {
            this.regexStr = _regexStr;
            this.regexIndex = _regexIndex;
            this.RegexInfoList = new List<RegexInfo>();
        }
        public RegexInfo()
        {
            this.RegexInfoList = new List<RegexInfo>();
        }
        private string regexStr;
        public string RegexStr
        {
            get { return regexStr; }
            set { regexStr = value; }
        }
        private string regexIndex;
        public string RegexIndex
        {
            get { return regexIndex; }
            set { regexIndex = value; }
        }
        private List<RegexInfo> regexInfoList;
        /// <summary>
        /// 其他规则(防止一个页面多种规则)
        /// </summary>
        public List<RegexInfo> RegexInfoList
        {
            get { return regexInfoList; }
            set { regexInfoList = value; }
        }
    }
}
