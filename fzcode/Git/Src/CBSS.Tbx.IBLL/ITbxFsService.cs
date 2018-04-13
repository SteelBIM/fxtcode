using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface ITbxFsService
    {
        void JuniorEnglishReport2DB();
        void JuniorEnglishSpokenRecord2DB();
        void OrderInfo2YXDB();

        void ExecuteLearningReport();
        void ExecuteStudentClassChangeLearningReport();
        void ExcutedInterestingRank();
    }
}
