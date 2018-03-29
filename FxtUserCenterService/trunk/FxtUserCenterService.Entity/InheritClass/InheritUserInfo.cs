using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity
{
    public class InheritUserInfo : UserInfo
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [SQLReadOnly]
        public string companyname { get; set; }
        /// <summary>
        /// appid
        /// </summary>
        [SQLReadOnly]
        public int appid { get; set; }
        /// <summary>
        /// appkey
        /// </summary>
        [SQLReadOnly]
        public string appkey { get; set; }
        /// <summary>
        /// apppwd
        /// </summary>
        [SQLReadOnly]
        public string apppwd { get; set; }
        /// <summary>
        /// signname
        /// </summary>
        [SQLReadOnly]
        public string signname { get; set; }
        /// <summary>
        /// apiurl
        /// </summary>
        [SQLReadOnly]
        public string apiurl { get; set; }
        /// <summary>
        /// apiurl
        /// </summary>
        [SQLReadOnly]
        public int producttypecode { get; set; }

        /// <summary>
        /// businessdb
        /// </summary>
        [SQLReadOnly]
        public string businessdb { get; set; }

        /// <summary>
        /// weburl
        /// </summary>
        [SQLReadOnly]
        public string weburl { get; set; }

        [SQLReadOnly]
        public DateTime overdate { get; set; }
        /// <summary>
        /// 产品账号最大数量
        /// </summary>
        public int maxaccountnumber { get; set; }
    }
}
