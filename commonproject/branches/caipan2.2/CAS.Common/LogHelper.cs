using System;

namespace CAS.Common
{
    public class LogHelper
    {
        public static log4net.ILog CASLog {
            get {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log.config"));
                return  log4net.LogManager.GetLogger("CASLog");
            }
        }

        /// <summary>
        /// 异常处理写日志
        /// 修改人rock 20151201,添加返回值返回错误信息编号，
        /// 修改人rock 20151203.添加BaseException信息输出，因为如果try里面有异常捕捉throw抛出异常的话异常的代码行会定位不准确
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message">自定义的消息</param>
        /// <returns>返回错误信息编号(供开发人员定位错误日志信息)</returns>
        public static string Error(Exception e,string message=null)
        {
            CASException ex = new CASException();
            ex.Level = 1;
            ex.Message = e.Message;
            ex.Source = e.Source;
            ex.StackTrace = e.StackTrace;
            ex.Type = e.GetType().ToString();
            ex.MethodName = e.TargetSite == null ? "" : e.TargetSite.Name;
            ex.ClassName = e.TargetSite == null ? "" : e.TargetSite.DeclaringType.FullName;
            //取session里的值 kevin
            ex.UserIP = WebCommon.GetIPAddress();
            ex.UserName = SessionHelper.Get(ConstCommon.UserName);
            ex.CompanyID = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.CompanyId));
            ex.CompanyName = SessionHelper.Get(ConstCommon.CompanyName);
            //错误信息的编码，供开发人员定位错误日志信息
            string s = Guid.NewGuid().ToString().Replace("-", "");
            s = s.Substring(0, 10 > s.Length ? s.Length : 10);
            string errorCode = DateTime.Now.ToString("yyyyMMddHHmmssff") + "X" + s;
            string messageStr = "";
            if (!string.IsNullOrEmpty(message))
            {
                 messageStr = "\r\nMessage2:" + message;
            }
            string errstr = "错误编号:" + errorCode + "\r\n"
                            + "Message:{0}\r\n"
                            + "Type:{1}\r\n"
                            + "Source:{2}\r\n"
                            + "StackTrace:{3}\r\n"
                            + "Method:{4}\r\n"
                            + "Class:{5}\r\n"
                            + "UserIP:{6}\r\n"
                            + "UserName:{7}\r\n"
                            + "CompanyID:{8}\r\n"
                            + "CompanyName:{9}\r\n";
            string error = string.Format(errstr
                                        , ex.Message + messageStr, ex.Type, ex.Source, ex.StackTrace, ex.MethodName, ex.ClassName
                                        , ex.UserIP, ex.UserName, ex.CompanyID, ex.CompanyName);
            //添加BaseException信息输出，因为如果try里面有异常捕捉throw抛出异常的话异常的代码行会定位不准确
            if (e != e.GetBaseException())
            {
                error = error + "BaseException:";
                CASLog.Error(error, e.GetBaseException());
            }
            else
            {
                CASLog.Error(error);
            }
            return errorCode;
        }

        /// <summary>
        /// 写简单日志
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            CASLog.Info(info);
        }
        public static void ErrorTest(Exception ex)
        {
            CASLog.Error(null, ex);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="error"></param>
        public static void Error(string error)
        {
            CASLog.Error(error);
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="o"></param>
        public static void Operation(CASOperation o)
        {
            //这里实际要存入数据库，不是写日志文件。
            CASOperation op = new CASOperation();
            op.OperationID = 0;//方法名
            op.Type = "";//具体的操作类型：新增，删除，修改等

            op.UserID = SessionHelper.Get(ConstCommon.UserId);
            op.UserIP = WebCommon.GetIPAddress();
            op.UserName = SessionHelper.Get(ConstCommon.UserName);
            op.CityID = Convert.ToInt32(SessionHelper.Get(ConstCommon.CityId));
            op.CompanyID = Convert.ToInt32(SessionHelper.Get(ConstCommon.CompanyId));
            op.FXTCompanyID = Convert.ToInt32(SessionHelper.Get(ConstCommon.FxtCompanyId));
            op.DepartmentID = Convert.ToInt32(SessionHelper.Get(ConstCommon.DepartmentId));
            op.SystemName = "CAS";
            op.SystemVersion = "1.0";
            string infostr = "Message:{0}\r\n"
                            + "Type:{1}\r\n"
                            + "UserIP:{2}\r\n"
                            + "UserID:{3}\r\n"
                            + "CityID:{4}\r\n"
                            + "CompanyID:{5}\r\n"
                            + "FXTCompanyID:{6}\r\n"
                            + "DepartmentID:{7}\r\n"
                            + "UserName:{8}\r\n"
                            + "SystemName:{9}\r\n"
                            + "SystemVersion:{10}\r\n";
            string info = string.Format(infostr
                                        , op.Message, op.Type, op.UserIP, op.UserID, op.CityID, op.CompanyID, op.FXTCompanyID
                                        , op.DepartmentID, op.UserName, op.SystemName, op.SystemVersion);
            CASLog.Info(info);
        }
      
    }

    public class CASException : LogEntityBase
    {
        private int level;

        /// <summary>
        /// 错误等级
        /// </summary>
       
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        private string className;
        /// <summary>
        /// 错误的类名
        /// </summary>
       
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string source;
        /// <summary>
        /// 错误源
        /// </summary>
       
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        private string stackTrace;
        /// <summary>
        /// 错误出处
        /// </summary>
       
        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value; }
        }

        private string methodName;
        /// <summary>
        /// 出错方法
        /// </summary>
       
        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

    }

   
    public class CASOperation : LogEntityBase
    {
        private int operationID;
        /// <summary>
        /// 操作ID
        /// </summary>
       
        public int OperationID
        {
            get { return operationID; }
            set { operationID = value; }
        }
    }

   
    public class LogEntityBase
    {
        private string message;
        /// <summary>
        /// 错误或操作的内容
        /// </summary>
       
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private string type;
        /// <summary>
        /// 错误或操作的类型
        /// </summary>
       
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string userIP;
        /// <summary>
        /// 用户IP
        /// </summary>
       
        public string UserIP
        {
            get { return userIP; }
            set { userIP = value; }
        }

        private string userID;
        /// <summary>
        /// 用户ID
        /// </summary>
       
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string userName;
        /// <summary>
        /// 用户中文名
        /// </summary>
       
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int cityID;
        /// <summary>
        /// 城市ID
        /// </summary>
       
        public int CityID
        {
            get { return cityID; }
            set { cityID = value; }
        }


        private int companyID;
        /// <summary>
        /// 公司ID
        /// </summary>
       
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private int fxtCompanyID;
        /// <summary>
        /// FXT公司ID
        /// </summary>
       
        public int FXTCompanyID
        {
            get { return fxtCompanyID; }
            set { fxtCompanyID = value; }
        }

        private int departmentID;
        /// <summary>
        /// 部门ID
        /// </summary>
       
        public int DepartmentID
        {
            get { return departmentID; }
            set { departmentID = value; }
        }

        private string systemName;
        /// <summary>
        /// 浏览器或系统名称
        /// </summary>
       
        public string SystemName
        {
            get { return systemName; }
            set { systemName = value; }
        }

        private string systemVersion;
        /// <summary>
        /// 浏览器或系统版本
        /// </summary>
       
        public string SystemVersion
        {
            get { return systemVersion; }
            set { systemVersion = value; }
        }
        //kevin
        public string CompanyName { get; set; }
    }

}
