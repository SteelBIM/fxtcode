using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA.PBFH;
using System.Data;
using CAS.Entity;
using CAS.Entity.DBEntity;
using CAS.Common;

namespace CAS.Logic.PBFH
{
    public class ProjectBL
    {
         /// <summary>
        /// 获取楼盘下拉列表  
        /// </summary>
        public static List<DATProject> GetProjectDropDownList(SearchBase search)
        {
            List<DATProject> list = ProjectDA.GetProjectDropDownList(search,true);
            //查出的条数不足时，取消左匹配再取一次数据补齐。
            if (list.Count < search.Top) {
                List<DATProject> list1 = ProjectDA.GetProjectDropDownList(search, false);
                list = list.Union(list1).Take(search.Top).ToList();        
            }
            return list;
        }
    }
}
