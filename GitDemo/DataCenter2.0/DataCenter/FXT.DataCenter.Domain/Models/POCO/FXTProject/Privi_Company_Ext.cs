using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Company_Ext
    {
        private int _fk_companyid;
        public int fk_companyid
        {
            get { return _fk_companyid; }
            set { _fk_companyid = value; }
        }
        private string _companydesc;
        /// <summary>
        /// 公司简介
        /// </summary>
        public string companydesc
        {
            get { return _companydesc; }
            set { _companydesc = value; }
        }

    }
}
