using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.Core.Utility
{
    /// <summary>
    /// 连表实体类
    /// </summary>
    public class RelationEntity<T1,T2> where T1 : class, new() where T2 : class, new()
    {
        public T1 ParentEntity { get; set; }

        public List<T2> ChildrenEntities { get; set; }

        public string[] ParentDisableColumns { get; set; }

        public string[] ChildrenDisableColumns { get; set; }

        public string ParentIdName { get; set; }
    }
}
