using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class TeacherInfo
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 地区ID
        /// </summary>
        public int? AreaID { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        public int? SchoolID { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 学科ID
        /// </summary>
        public int? SubjectID { get; set; }

        /// <summary>
        /// 学科
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 省份ID
        /// </summary>
        public int? ProvinceID { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
    }
}
