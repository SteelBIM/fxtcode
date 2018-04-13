using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class AppSkinVersion
    {
        /// <summary>
        /// 皮肤版本ID
        /// </summary>
        public int SkinVersionID { get; set; }
        ///<summary>
        /// 皮肤版本号
        /// </summary>
        [Required(ErrorMessage = "皮肤版本号不能为空")]
        [RegularExpression(@"([1-9]{1,3})+(\.[0-9]{1,3})+(\.[0-9]{1,3})", ErrorMessage = "请输入正确的版本号")]
        public string SkinVersionNumber { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 皮肤版本描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 创建时间
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
        ///  皮肤版本号更新地址
        /// </summary>
        [Required(ErrorMessage = "皮肤版本号更新地址不能为空")]
        public string UpdateAddress { get; set; }
        /// <summary>
        ///  皮肤版本号MD5
        /// </summary>
        [Required(ErrorMessage = "皮肤版本号MD5值不能为空")]
        public string UpdateMD5 { get; set; }
        /// <summary>
        /// 皮肤版本状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUser { get; set; }
    }
}
