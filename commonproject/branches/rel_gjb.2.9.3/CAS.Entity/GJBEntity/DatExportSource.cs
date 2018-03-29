using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ExportSource : DatExportSource
    {
        [SQLReadOnly]
        public string showname { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        [SQLReadOnly]
        public string f_py { get; set; }
        /// <summary>
        /// 字段对应的表单数据类型
        /// </summary>
        [SQLReadOnly]
        public string tablefieldtypecodename { get; set; }
        
    }
}
