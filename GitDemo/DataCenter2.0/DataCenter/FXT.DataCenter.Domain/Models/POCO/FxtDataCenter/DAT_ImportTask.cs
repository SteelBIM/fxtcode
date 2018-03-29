using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_ImportTask
    {

        /// <summary>
        /// 批量导入任务表
        /// </summary>
        public long TaskID { get; set; }
        /// <summary>
        /// 导入类型
        /// </summary>
        public int ImportType { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int FXTCompanyId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public int? IsComplete { get; set; }
        /// <summary>
        /// 成功导入数量
        /// </summary>
        public int? SucceedNumber { get; set; }
        /// <summary>
        /// 楼盘名称错误数量(导入楼盘案例时用到)
        /// </summary>
        public int? NameErrNumber { get; set; }
        /// <summary>
        /// 数据格式错误数量
        /// </summary>
        public int? DataErrNumber { get; set; }
        /// <summary>
        /// 文件存放路径
        /// </summary>
        public string FilePath { get; set; }

        public int Steps { get; set; }

        public string IsCompleteDisplay
        {
            get
            {

                switch (IsComplete)
                {
                    case 1:
                        return "完成";
                    case 0:
                        if (Steps == 0) return "处理中...";
                        return Steps+"%";
                    default:
                        return "失败";
                }

            }
        }

    }
}
