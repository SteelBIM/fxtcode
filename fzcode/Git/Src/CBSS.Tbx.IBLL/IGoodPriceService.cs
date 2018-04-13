using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface IGoodPriceService
    {
        IEnumerable<GoodPrice> GetGoodPriceList(out int totalcount, GoodPriceRequest request);
        void SaveGoodPrice(GoodPrice model);
        GoodPrice GetGoodPrice(int id);
        int DelGoodPriceByGoodPriceID(int GoodPriceID);
    }
}
