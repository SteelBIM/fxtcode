using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.IBSLearnReport
{
    public interface IIBSHearResourceBLL
    {
        bool InitializeClass(string connectionstring);
        bool InitializeBook(string connectionstring);
        bool InitializeRdsStudyReportModule(string connectionstring);
        bool InitializeRdsStudyReportModuleTitle(string connectionstring);
        void initStudyReportBookCatalogues(string connectionstring);
    }
}
