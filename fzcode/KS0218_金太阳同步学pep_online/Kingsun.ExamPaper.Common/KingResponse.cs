using System.Web;

namespace Kingsun.ExamPaper.Common
{
    public class KingResponse
    {
        public static object GetErrObj(string message)
        {
            object obj = new { Success = false, Data = "", Message = message };
            return obj;
        }

        public static object GetObj(object Data, string message = "")
        {
            object obj = new { Success = true, Data = Data, Message = message };
            return obj;
        }

        public static void ResponseWriteError(string message)
        {
            HttpContext.Current.Response.Write(JsonHelper.EncodeJson(GetErrObj(message)));
            HttpContext.Current.Response.End();
        }
        public static void ResponseWrite(object Data, string message = "")
        {
            HttpContext.Current.Response.Write(JsonHelper.EncodeJson(GetObj(Data, message)));
            HttpContext.Current.Response.End();
        }
    }
}
