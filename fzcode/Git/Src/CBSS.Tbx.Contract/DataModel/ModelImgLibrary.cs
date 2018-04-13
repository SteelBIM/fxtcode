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
    [Table("ModelImgLibrary")]
    public class ModelImgLibrary
    {
        public int ModelImgLibraryID { get; set; }
        /// <summary>
        /// 模型ID
        /// </summary>
        public int ModelID { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [Required(ErrorMessage = "图片名称不能为空")]
        [StringLength(18, ErrorMessage = "图片名称长度不能超过18位")]
        public string ImgName { get; set; }
        /// <summary>
        /// 图片格式，1：静态图 2：动态图 3：序列图
        /// </summary>
        public int ImgType { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [Required(ErrorMessage = "图片路径不能为空")]
        public string ImgPath { get; set; }
        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }

        public int CreateUser { get; set; }
    }
}
