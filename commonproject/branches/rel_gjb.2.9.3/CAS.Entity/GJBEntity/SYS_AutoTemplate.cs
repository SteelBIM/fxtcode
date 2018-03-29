using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_AutoTemplate : DBEntity.SYSAutoTemplate
    {
        [SQLReadOnly]
        public string createusername { get; set; }
        [SQLReadOnly]
        public string filename { get; set; }
        [SQLReadOnly]
        public string fileguid { get; set; }
        [SQLReadOnly]
        public string excelfilename { get; set; }
        [SQLReadOnly]
        public string excelfileguid { get; set; }
        [SQLReadOnly]
        public string RTPinYin { get; set; }
        [SQLReadOnly]
        public string templatecodename { get; set; }
    }
}
