using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_LanEmail : DatLanEmail
    {
        [SQLReadOnly]
        public string tousertruename { get; set; }
        //用户头像 kevin
        [SQLReadOnly]
        public string fromuserheadphoto { get; set; }
        [SQLReadOnly]
        public List<Dat_Files> DatFileslist { get; set; }
    }

    /// <summary>
    /// 聊天分组实体 kevin 20140620
    /// </summary>
    public class Dat_LanEmail_Group : BaseTO
    {
        public int fromuserid { get; set; }
        public string fromusername { get; set; }
        public string fromuserheadphoto { get; set; }
        public int newcnt { get; set; }
        public DateTime createdon { get; set; }
        public string content { get; set; }
        public string title { get; set; }
        public int businesstype { get; set; }
        public long businessid { get; set; }
    }
}
