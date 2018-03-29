using System;

namespace FXT.VQ.DataService.Model
{
    [Serializable]
    public class ApplicationInfo
    {
        public ApplicationInfo()
        {
        }
        public string splatype
        {
            get
            {
                return "null";
            }
        }
        public string platver
        {
            get
            {
                return "null";
            }
        }
        public string stype
        {
            get
            {
                return "null";
            }
        }
        public string version
        {
            get
            {
                return "null";
            }
        }
        public string vcode
        {
            get
            {
                return "null";
            }
        }
        public string systypecode
        {
            get
            {
                return ConfigSettings.mDataCenterSystypecode;
            }
        }
        public string channel
        {
            get
            {
                return "null";
            }
        }
    }
}
