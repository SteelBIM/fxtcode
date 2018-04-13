using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    [Auditable]
    [Table("AppVersion")]
    public class AppVersion
    {
        /// <summary>
        /// ID
        /// </summary>
        public int AppVersionID { get; set; }

        /// <summary>
        /// 应用版本号
        /// </summary>
        [Required(ErrorMessage = "应用版本号不能为空")]
        [RegularExpression(@"([1-9])([0-9]{0,2})(\.[0-9]{1,3})(\.[0-9]{1,3})", ErrorMessage ="请输入正确的版本号")]
        public string AppVersionNumber { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>

        public string AppID { get; set; }
        /// <summary>
        /// 应用类型，1安卓 2苹果
        /// </summary>
        //[RegularExpression(@"/^\+?[1-9]\d*$/", ErrorMessage = "请选择应用类型")]
        public int AppType { get; set; }
        /// <summary>
        /// 应用皮肤ID
        /// </summary>

        public int AppSkinID { get; set; }
        /// <summary>
        /// 应用版本号更新类型1整包更新2增量更新
        /// </summary>
        public string AppVersionUpdateType { get; set; }
        /// <summary>
        /// 应用文件地址
        /// </summary>
        [Required(ErrorMessage = "应用文件地址不能为空")]
        public string AppVersionUpdateAddress { get; set; }
        /// <summary>
        /// 应用文件MD5值
        /// </summary>
        [Required(ErrorMessage = "应用文件MD5值不能为空")]
        public string AppVersionUpdateMD5 { get; set; }
        /// <summary>
        /// 应用版本号描述
        /// </summary>
        [StringLength(50, ErrorMessage = "更新简介长度不能超过50位")]
        public string AppVersionDescribe { get; set; } 
        /// <summary>
        /// 状态1启用2禁用3已删除
        /// </summary>

        public int Status { get; set; }
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }

        public int CreateUser { get; set; }
        /// <summary>
        /// 是否强制更新 1：是 2：否
        /// </summary>
        public int IsForcedUpdate { get; set; }
    }
}
