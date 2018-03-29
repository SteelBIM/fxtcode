using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_SYSProjectMatch
    {     /// <summary>
        /// Id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectNameId
        /// </summary>
        public virtual int? ProjectNameId
        {
            get;
            set;
        }
        /// <summary>
        /// NetName
        /// </summary>
        public virtual string NetName
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectName
        /// </summary>
        public virtual string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int? CityId
        {
            get;
            set;
        }
        /// <summary>
        /// FXTCompanyId
        /// </summary>
        public virtual int? FXTCompanyId
        {
            get;
            set;
        }


        public static FxtApi_SYSProjectMatch ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_SYSProjectMatch>(json);
        }
        public static List<FxtApi_SYSProjectMatch> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_SYSProjectMatch>(json);
        }
    }
}
