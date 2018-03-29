using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class SYSTaxTemplate_Ext : SYSTaxTemplate
    {
        public string propertytypecodename { get; set; }
        public string ownertypecodename { get; set; }
        public string isover5yearcodename { get; set; }
        public string isfirstbuycodename { get; set; }
        public string isuniquehousecodename { get; set; }
        public string areatypecodecodename { get; set; }
        public string bankname { get; set; }
    }
}
