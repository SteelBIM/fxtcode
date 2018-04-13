using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL
{
    public interface IIBSInitUserVideoBLL
    {

        bool InitializeUserVideoInfo(string conenction);
        bool InitializeBook(string connectionstring);
        bool InitializeRdsStudyReportModule(string conenction);

        bool InitializeRdsStudyReportModuleTitle(string conenction);

        void initStudyReportBookCatalogues(string connectionstring);

        bool RemoveRedis();

        bool TodayInitializeUserVideoInfo(string conenction);

        bool TodayInitializeBook(string connectionstring);
        bool TodayInitializeRdsStudyReportModule(string conenction);

        bool TodayInitializeRdsStudyReportModuleTitle(string conenction);

        void TodayinitStudyReportBookCatalogues(string connectionstring);
    }
}
