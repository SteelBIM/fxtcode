using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IDAL
{
    public interface IIBSInitUserVideoDAL
    {

       
        /// <summary>
        /// 初始化用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
   /*     bool InitializeUserVideoInfo(string connectionstring);

        /// <summary>
        /// 初始化Rds_StudyReport_Module 用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        bool InitializeRdsStudyReportModule(string connectionstring);*/

        /// <summary>
        /// 初始化Rds_StudyReport_ModuleTitle 用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        bool InitializeRdsStudyReportModuleTitle(string connectionstring);
        bool initStudyReportBookCatalogues(string connectionstring);

        bool TodayInitializeRdsStudyReportModuleTitle(string connectionstring);
        bool TodayinitStudyReportBookCatalogues(string connectionstring);
    }
}
