using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_BackupDataUserSetting : SYSBackupDataUserSetting
    {
        /// <summary>
        /// 归档人名称
        /// </summary>
        [SQLReadOnly]
        public string backupusername { get; set; }
        /// <summary>
        /// 归档资料负责人名称
        /// </summary>
        [SQLReadOnly]
        public  string backupdatausername { get; set; }
    }
}
