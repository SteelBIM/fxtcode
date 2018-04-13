using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CBSS.Core.Cache;
using CBSS.Core.Utility;

namespace CBSS.Web.Admin.Common
{
    public class AdminCacheContext 
    {
        public static AdminCacheContext Current
        {
            get
            {
                return CacheHelper.GetItem<AdminCacheContext>();
            }
        }

    }
}
