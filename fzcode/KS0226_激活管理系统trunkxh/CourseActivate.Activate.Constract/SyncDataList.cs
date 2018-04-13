using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract
{
    public static class SyncDataList
    {
        private static List<Model.SyncRangmodel> _DataList;

        public static List<Model.SyncRangmodel> DataList
        {
            get { return SyncDataList._DataList; }
            set { SyncDataList._DataList = value; }
        }

        public static void Add(this Model.SyncRangmodel model)
        {
            if (_DataList == null)
            {
                _DataList = new List<Model.SyncRangmodel>();
            }
            _DataList.Add(model);
        }

    }
}
