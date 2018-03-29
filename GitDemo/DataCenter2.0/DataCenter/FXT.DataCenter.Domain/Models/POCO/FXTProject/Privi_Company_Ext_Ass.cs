using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Company_Ext_Ass
    {
        private int _fk_companyid;
        public int fk_companyid
        {
            get { return _fk_companyid; }
            set { _fk_companyid = value; }
        }
        private bool _ass_house = true;
        public bool ass_house
        {
            get { return _ass_house; }
            set { _ass_house = value; }
        }
        private bool _ass_land = false;
        public bool ass_land
        {
            get { return _ass_land; }
            set { _ass_land = value; }
        }
        private bool _ass_assets = false;
        public bool ass_assets
        {
            get { return _ass_assets; }
            set { _ass_assets = value; }
        }

    }
}
