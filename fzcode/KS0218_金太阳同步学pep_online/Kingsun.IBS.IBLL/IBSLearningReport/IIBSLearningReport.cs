using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL.IBSLearningReport
{
    public interface IIBSLearningReport
    {
        void InitLearningReport(string kingsun);
        void InitLearningReportModuleTitle(string kingsun);

        void InitLearningReportBookCatalogues(string kingsun);

        void InitLearningReportUserInfo(string kingsun);


        void TodayInitLearningReport(string kingsun);
        void TodayInitLearningReportModuleTitle(string kingsun);

        void TodayInitLearningReportBookCatalogues(string kingsun);

        void TodayInitLearningReportUserInfo(string kingsun);

        void ExecuteLearningReport();
        void ExecuteStudentClassChangeLearningReport();

        void RemoveRedis();
    }
}
