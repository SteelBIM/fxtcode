using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Account.Contract.ViewModel
{
    [Serializable]
    /// <summary>
    /// 权限视图
    /// </summary>
    public class v_action
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int ID { get; set; }
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
        /// <summary>
        /// Control路径
        /// </summary>
        public string columnico { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string columnimg { get; set; }
        /// <summary>
        /// 0展示模块 1 不展示模块
        /// </summary>
        public int ishide { get; set; }
    }
}
