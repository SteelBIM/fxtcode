using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class V_AppSkinVersion
    {
        /// <summary>
        /// AppID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AppType { get; set; }

        /// <summary>
        /// 皮肤版本ID
        /// </summary>
        public int SkinVersionID { get; set; }
        /// <summary>
        /// 皮肤版本号
        /// </summary>
        public string SkinVersionNumber { get; set; }
        /// <summary>
        /// 皮肤版本描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 皮肤版本创建时间
        /// </summary>
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }
        /// <summary>
        /// 皮肤版本号更新类型1整包更新2增量更新
        /// </summary>
        public int UpdateType { get; set; }
        /// <summary>
        /// 皮肤版本号更新地址
        /// </summary>
        public string UpdateAddress { get; set; }
        /// <summary>
        /// 皮肤版本号MD5
        /// </summary>
        public string UpdateMD5 { get; set; }
        /// <summary>
        /// 皮肤版本状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 皮肤版本创建者
        /// </summary>
        public int CreateUser { get; set; }
    }
}
