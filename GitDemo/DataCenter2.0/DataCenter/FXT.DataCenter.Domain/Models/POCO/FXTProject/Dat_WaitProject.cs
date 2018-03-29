using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_WaitProject
    {

        /// <summary>
        /// WaitProjectId
        /// </summary>
       [ExcelExportIgnore]
        public int WaitProjectId { get; set; }
        /// <summary>
        /// WaitProjectName
        /// </summary>
        [DisplayName("楼盘名称")]
        public string WaitProjectName { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
       [ExcelExportIgnore]
        public int CityId { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
       [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName("用户名")]
        public string UserId { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
         [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

    }
}
