using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class bpolicy
    {
        public cfg_bpolicy cfg_bpolicy { get; set; }
        //List类型是否可用
        public List<cfg_bpolicyproduct> cfg_bpolicyproducts { get; set; }
    }
}