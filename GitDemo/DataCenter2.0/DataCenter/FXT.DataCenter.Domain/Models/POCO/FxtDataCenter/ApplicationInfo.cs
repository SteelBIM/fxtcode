using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace FXT.DataCenter.Domain.Models
{
    public class ApplicationInfo
    {
        private string _systypecode = "";//默认值请勿修改
        public ApplicationInfo()
        {
        }
        public ApplicationInfo(string sysTypeCode)
        {
            _systypecode = sysTypeCode;
        }
        public string splatype
        {
            get
            {
                return ConfigurationManager.AppSettings["SplaType"];
            }
        }
        public string platver
        {
            get
            {
                return ConfigurationManager.AppSettings["PlatVer"];
            }
        }
        public string stype
        {
            get
            {
                return ConfigurationManager.AppSettings["SType"];
            }
        }
        public string version
        {
            get
            {
                return ConfigurationManager.AppSettings["Version"];
            }
        }
        public string vcode
        {
            get
            {
                return ConfigurationManager.AppSettings["VCode"];
            }
        }
        public string systypecode
        {
            get
            {
                return _systypecode;
            }
        }
        public string channel
        {
            get
            {
                return ConfigurationManager.AppSettings["Channel"];
            }
        }
    }
    [Serializable]
    public class Apps
    {
        public string appid { get; set; }
        public string apppwd { get; set; }
        public string appurl { get; set; }
        public string appkey { get; set; }
    }
}
