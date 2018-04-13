using System.Collections.Generic;

namespace Kingsun.ExamPaper.ImportTool
{
    public class CacheHelper
    {
        private SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
        private static volatile CacheHelper instance = null;
        private static object lockHelper = new object();

        private CacheHelper()
        {

        }
        public void Add(string key, string value)
        {
            dic.Add(key, value);
        }

        public void Remove(string key)
        {
            dic.Remove(key);
        }

        public string this[string index]
        {
            get
            {
                if (dic.ContainsKey(index))
                    return dic[index];
                else
                    return null;
            }
            set { dic[index] = value; }
        }

        public static CacheHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new CacheHelper();
                        }
                    }
                }
                return instance;
            }
        }
    }
}