using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Web.Admin.Models
{
    public class v_batchinfos
    {
        public int batchid { get; set; }
        public string batchcode { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public int? editionid { get; set; }
        public string editionname { get; set; }
        public int? bookid { get; set; }
        public string bookname { get; set; }
        public int? indate { get; set; }
        public int? purpose { get; set; }
        public int? activatenum { get; set; }
        public int activatetypeid { get; set; }
        public int? status { get; set; }
        public int? masterid { get; set; }
        public string mastername { get; set; }
        public string remark { get; set; }
        public DateTime? createtime { get; set; }

        /// <summary>
        /// 出版社ID
        /// </summary>
        public int publishid { get; set; }
        /// <summary>
        /// 出版社名称
        /// </summary>
        public string publishname { get; set; }
        /// <summary>
        /// 出版社状态（0:未启用，1:启用，2:禁用）
        /// </summary>
       
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? createTime { get; set; }

  //      public int activatetypeid { get;set;}
        public string activatetypename { get; set; }    
        public int? type { get; set; }
        public int? way { get; set; }
        public int devicenum { get; set; }

        public int? activateusenum { get; set; }

        public string createtype { get; set; }
    }
}
