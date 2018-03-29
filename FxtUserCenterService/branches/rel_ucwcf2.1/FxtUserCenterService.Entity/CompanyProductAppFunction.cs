using System;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity
{
    [TableAttribute("dbo.App_Function")]
    public class CompanyProductAppFunction : BaseTO
    {
        private int _appid;
        public int appid
        {
            get { return _appid; }
            set { _appid = value; }
        }
        private string _functionname;
        public string functionname
        {
            get { return _functionname; }
            set { _functionname = value; }
        }
        private string _callbackurl;
        public string callbackurl
        {
            get { return _callbackurl; }
            set { _callbackurl = value; }
        }
        private string _callbackformat;
        public string callbackformat
        {
            get { return _callbackformat; }
            set { _callbackformat = value; }
        }
        private string _parame1;
        public string parame1
        {
            get { return _parame1; }
            set { _parame1 = value; }
        }
        private string _parame2;
        public string parame2
        {
            get { return _parame2; }
            set { _parame2 = value; }
        }
        private string _parame3;
        public string parame3
        {
            get { return _parame3; }
            set { _parame3 = value; }
        }
        private string _parame4;
        public string parame4
        {
            get { return _parame4; }
            set { _parame4 = value; }
        }
    }
}