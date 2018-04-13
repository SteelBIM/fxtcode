using CBSS.Tbx.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Tbx.Contract.DataModel;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using CBSS.Framework.DAL;
using CBSS.Tbx.IBLL;
using CBSS.IBS.BLL;
using CBSS.IBS.IBLL;
using CBSS.Framework.Redis;

namespace CBSS.Tbx.BLL
{
    /// <summary>
    /// Tbx系统接口实现
    /// </summary>
    public partial class TbxService : ITbxService
    {
        Repository tbxRecordRepository = new Repository("TbxRecord");
        Repository repository = new Repository("Tbx");
        Repository HearResourceRepository = new Repository("HearResource");
        Repository InterestDubbingRepository = new Repository("InterestDubbing");

        RedisHashHelper redis = new RedisHashHelper("Tbx");
        RedisListHelper redisList = new RedisListHelper("Tbx");
        static string filepath = "Config/SettingConfig.xml";
        static string _getOssFilesUrl = XMLHelper.GetAppSetting(filepath, "FileConfig", "getOssFiles");
        static string _getFilesUrl = XMLHelper.GetAppSetting(filepath, "FileConfig", "getFiles");
        static string _getVideoFiles = XMLHelper.GetAppSetting(filepath, "FileConfig", "getVideoFiles");
        static string LinkUrl = XMLHelper.GetAppSetting(filepath, "FileConfig", "HearResourcesURL");
        static IIBSService ibsService = new IBSService();
    }
}
