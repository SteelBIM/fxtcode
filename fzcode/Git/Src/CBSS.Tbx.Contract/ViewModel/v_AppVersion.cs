using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
   public class v_AppVersion
    {
        /// <summary>
        /// ID
        /// </summary>
        public int AppVersionID { get; set; }

        /// <summary>
        /// 应用版本号
        /// </summary> 
        public string AppVersionNumber { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>

        public string AppID { get; set; }
        /// <summary>
        /// 应用皮肤ID
        /// </summary>

        public int AppSkinID { get; set; }
        /// <summary>
        /// 应用版本号更新类型1整包更新2增量更新
        /// </summary>
        public int AppVersionUpdateType { get; set; }
        /// <summary>
        /// 应用版本号更新地址
        /// </summary>
        public string AppVersionUpdateAddress { get; set; }
        /// <summary>
        /// 应用版本号MD5
        /// </summary>
        public string AppVersionUpdateMD5 { get; set; }
        /// <summary>
        /// 应用版本号描述
        /// </summary>
        public string AppVersionDescribe { get; set; }
        /// <summary>
        /// 状态 0未启用1启用2禁用3已删除
        /// </summary>

        public int Status { get; set; }
        /// <summary>
        /// 是否强制更新 1：是 2：否
        /// </summary>
        public int IsForcedUpdate { get; set; }
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }

        public int CreateUser { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>

        public string AppName { get; set; }
        /// <summary>
        /// 应用类别
        /// </summary>

        public int AppType { get; set; }
    }
}
