using CourseActivate.Activate.Constract.Model;
using CourseActivate.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.DAL
{
    public class ActivateManagementDAL : Repository
    {
        public object InsertBatch(tb_batch batch)
        {
            return Insert<tb_batch>(batch);
        }

        public List<object> InsertBatchactivate(List<tb_batchactivate> activateCodeList)
        {
            return InsertRange<tb_batchactivate>(activateCodeList);
        }
    }
}
