using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity
{
    [TableAttribute("dbo.CompanyProduct_App")]
    public class CompanyProductApp : BaseTO
    {
        private int _cpaid;
        [SQLField("cpaid", EnumDBFieldUsage.PrimaryKey, true)]
        public int cpaid
        {
            get { return _cpaid; }
            set { _cpaid = value; }
        }
        private int _cpid;
        public int cpid
        {
            get { return _cpid; }
            set { _cpid = value; }
        }
        private int _appid;
        public int appid
        {
            get { return _appid; }
            set { _appid = value; }
        }
        private string _apppwd;
        public string apppwd
        {
            get { return _apppwd; }
            set { _apppwd = value; }
        }
        private string _appkey;
        public string appkey
        {
            get { return _appkey; }
            set { _appkey = value; }
        }
        private string _apiurl;
        public string apiurl
        {
            get { return _apiurl; }
            set { _apiurl = value; }
        }
    }

}
