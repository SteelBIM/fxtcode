using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FxtCenterServiceOpen.Actualize.Common
{
    [Serializable]
    public class ZipCodeMapModel
    {
         /// <summary>
        /// 房讯通国标码
        /// </summary>
        public string fxtzipcode { get; set; }

        /// <summary>
        /// 客户国标码
        /// </summary>
        public string zipcode { get; set; }    
    }
}
