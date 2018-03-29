using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
  public  class AppraiseObjectPrice
    {
       
        public long AOPId { get; set; }
      
        public long? EAId { get; set; }
       
        public DateTime? ValueDate { get; set; }
       
        public decimal? MainHouseUnitPrice { get; set; }
       
        public decimal? MainHouseTotalPrice { get; set; }
       
        public decimal? OutbuildingTotalPrice { get; set; }
      
        public decimal? LandUnitPrice { get; set; }
       
        public decimal? LandTotalPrice { get; set; }
       
        public decimal? AppraiseTotalPrice { get; set; }        
    }
}
