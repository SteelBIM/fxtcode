using CBSS.Framework.Contract;
using CBSS.Framework.Contract.API;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface IAppService
    {
        bool DelAppGoodItem(int AppGoodItemID);
        void SaveAppGoodItem(AppGoodItem model);
        IEnumerable<AppGoodItem> GetAppGoodItemList(Expression<Func<AppGoodItem, bool>> expression);
        int SaveApp(Contract.DataModel.App model);

        //IEnumerable<Contract.DataModel.App> GetAppList();
        IEnumerable<Contract.DataModel.App> GetAppList(out int totalaount, AppRequest request = null);
        Contract.DataModel.App GetApp(string id);
        int DelAppByAppID(string AppID);
        IEnumerable<Contract.DataModel.App> GetAppListByStatus();
        /// <summary>
        /// 根据AppID删除应用(0：有对应版本不能删除，1：删除成功，2：删除失败)
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        bool DeleteApp(List<int> ids);

        APIResponse VerifyParam<T>(string inputStr, out T input, List<string> ignoreParams = null);

        APIResponse SendTelephoneCode(string telephone);

        APIResponse DecidePhoneCode(string telephone, string code);

        bool SendMessage(string messageContent, string telephone);

        bool ContainsBadChar(string p_StringName);

        List<AppVersion> GetVersion(string appID, string versionNumber, int appType, out string errMsg);

        bool SetValidUserRecord(string userId);

        string AddToken(string userId, string equipmentID);

        string PswToSecurity(string strPsw);

        void RemoveOnlineUser(string userID);

        void SetUserLoginRecord(Rds_UserLoginRecord loginRecord);

        bool SetUseAppRecord(Rds_UseAppRecord useAppRecord);

        APIResponse SubmitTopicSetAnswer(Rds_UserTopicSetAnswerModel answer);

        APIResponse GetStuTopicSetAnswer(Rds_UserTopicSetAnswerModel model);

        APIResponse AddReport<T>(T report, string hashID, string key, string proName, TypeOfReport type);
        APIResponse AddArticleReport(Rds_UserArticleReadRecord report, string hashID, string key, string proName, TypeOfReport type);
        APIResponse ShareReport<T>(T report, string hashID, string key);

        T GetShareReport<T>(string key);

        APIResponse GetReport<RT, RK, T, K>(int userID, int catalogID, int moduleID, string hashID, string recordName, string reportName)
            where T : class, new()
            where K : class, new()
            where RT : class, new()
            where RK : class, new();


    }
}
