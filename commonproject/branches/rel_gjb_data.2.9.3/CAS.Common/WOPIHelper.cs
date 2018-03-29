namespace CAS.Common
{   

    /// <summary>
    /// office web app server 接口处理类 kevin 20140923
    /// 需要指定owas服务器地址及wopi地址
    /// owas : Office Web Apps Server
    /// wopi : Web Application Open Platform Interface 
    /// </summary>
    public class WOPIHelper
    { 
        public static string OWASAddress(string url)
        {
            WOPIAction action = new WOPIAction
            {
                Url = url,
                CanEdit = false,
                Source = "web"
            };
            return OWASAddress(action);
        } 

        public static string OWASAddress(WOPIAction action)
        {
            string owasurl = WebCommon.GetConfigSetting("OWASUrl");
            string url = action.Url;
            switch (url.Substring(url.LastIndexOf('.') + 1))
            {
                case "doc":
                case "docx":
                    owasurl = owasurl + (action.CanEdit ? "we/wordeditorframe.aspx" : "wv/wordviewerframe.aspx");
                    break;
                case "xls":
                case "xlsx":
                    owasurl = owasurl + "x/_layouts/xlviewerinternal.aspx" + (action.CanEdit ? "?edit=1" : "");
                    break;
                case "ppt":
                case "pptx":
                    owasurl = owasurl + "p/PowerPointFrame.aspx?PowerPointView=" + (action.CanEdit ? "EditView" : "ReadingView");
                    break;
                case "pdf":
                    owasurl = owasurl + "wv/wordviewerframe.aspx?PdfMode=1";
                    break;
            }
            string wopisrc = WebCommon.GetConfigSetting("WOPIUrl");
            wopisrc = wopisrc + EncryptHelper.TextToPassword(url);
            string src = "WOPISrc=" + wopisrc.Replace(":", "%3A").Replace("/", "%2F") + "?access_token=" + WebCommon.GetRndString(10) + "&ui=zh-CN";
            return (owasurl + ((owasurl.IndexOf("?") > 0) ? "&" : "?") + src);
        }
    }

    public class WOPIAction
    {
        // Properties
        public bool CanEdit { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
    }
}
