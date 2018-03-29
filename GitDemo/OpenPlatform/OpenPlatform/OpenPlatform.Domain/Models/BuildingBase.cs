using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class BuildingBase
    {
       
        public long BuildingId { get; set; }
       
        public string BuildingGUID { get; set; }
     
        public int FXTCompanyId { get; set; }
      
        public string BuildingName { get; set; }
       
        public long? ProjectId { get; set; }
       
        public int? TotalFloor { get; set; }
       
        public int? BuildingStructure { get; set; }
       
        public DateTime CreateDate { get; set; }
       
        public bool Valid { get; set; }        
    }
}
