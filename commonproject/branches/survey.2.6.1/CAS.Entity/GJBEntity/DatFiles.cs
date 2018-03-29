using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Files : DatFiles
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 业务节点对象类型名称
        /// </summary>
       [SQLReadOnly]
        public string objecttypecodename { get; set;}

        /// <summary>
        /// 文件类型名称
        /// </summary>
        [SQLReadOnly]
        public string filetypecodename { get; set;}

        /// <summary>
        /// 文件类型子类型
        /// </summary>
        [SQLReadOnly]
        public string filetypesubcodename { get; set; }
        /// <summary>
        /// 附件大类名称
        /// </summary>
        [SQLReadOnly]
        public string annextypecodename { get;set; }

        [SQLReadOnly]
        public string createusername { get; set; }
        /// <summary>
        /// 附件小类名称
        /// </summary>
        [SQLReadOnly]
        public string annextypesubcodename { get; set; }

        /// <summary>
        /// 如果该业务对象有流程正在办理，则该属性为当前步骤的节点类型如下：
        /// <para>节点类型：0互审、1初审、2二审、3三审、4终审、5盖章、6复印</para>
        /// </summary>
        [SQLReadOnly]
        public int? nodetype { get; set; }

        
    }
    
}
