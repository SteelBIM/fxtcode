using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    [Serializable]
    /// <summary>
    /// 权限视图
    /// </summary>
    public class vw_action
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int groupid { get; set; }
        /// <summary>
        /// 父菜单名称
        /// </summary>
        public string parentcolumname { get; set; }
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public int parentcolumid { get; set; }
        /// <summary>
        /// 父菜单排序
        /// </summary>
        public int parentsequence { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string columnname { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int columnid { get; set; }
        /// <summary>
        /// 菜单排序
        /// </summary>
        public int sequence { get; set; }
        /// <summary>
        /// 菜单链接地址
        /// </summary>
        public string actionurl { get; set; }
        /// <summary>
        /// 操作权限
        /// </summary>
        public string actionname { get; set; }
        /// <summary>
        /// 操作权限描述
        /// </summary>
        public string actionstr { get; set; }
        public string columnico { get; set; }
    }
}
