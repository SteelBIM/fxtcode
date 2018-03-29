using System;

namespace FXT.VQ.UserService.Model
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
                return ConfigSettings.mUserCenterSystypecode;
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
