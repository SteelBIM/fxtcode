using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.Model.TBX;

namespace Kingsun.IBS.IDAL
{
    public interface IProcDal
    {
        List<StudyCurriculum> proc_GetUserStudyCurriculum();

        string proc_GetUserStudyDetailsLite3(List<UserClass> userClass);
      /*  List<StudyDirectory> proc_GetUserStudyDirectory();
        List<StudyReport> proc_GetUserStudyReport();*/

        string CreateTB_UserStudyCurriculum(List<TB_UserStudyCurriculum_Day> stuCurriculum);

   
        string CreateTB_UserStudyDirectory(List<TB_UserStudyDirectory> stuCurriculum);
        string CreateTB_UserStudyReport(List<TB_UserStudyReport> stuCurriculum);
    }
}
