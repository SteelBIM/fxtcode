namespace CAS.Entity.DBEntity
{
    public class SYS_CountryChargeStandard : SYSCountryChargeStandard
    {
        private string _chargetypename;
        /// <summary>
        ///收费类型名称
        /// </summary>
        public string chargetypename
        {
            get { return _chargetypename; }
            set { _chargetypename = value; }
        }
    }
}
