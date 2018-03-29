using CAS.DataAccess.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    public class TempFinanceTransferDA : BaseDA
    {
        public static int Add(TempFinanceTransfer model)
        {
            return InsertFromEntity<TempFinanceTransfer>(model);
        }
    }
}
