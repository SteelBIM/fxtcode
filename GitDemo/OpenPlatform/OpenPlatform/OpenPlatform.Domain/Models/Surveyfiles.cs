using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class Surveyfiles
    {
       public long Id { get; set; }
       public long GJBObjId { get; set; }
       public string Name { get; set; }
       public string Path { get; set; }
       public DateTime? UpTime { get; set; }
       public string Smallimgpath { get; set; }
       public int? Annextypecode { get; set; }
       public int? Annextypesubcode { get; set; }
       public string Imagetype { get; set; }
       public int? Filesize { get; set; }
       public int? Flietypecode { get; set; }
       public int? Filesubtypecode { get; set; }
       public DateTime? CreateDate { get; set; }
       public string Remark { get; set; }
       public DateTime? Filecreatedate { get; set; }
       public int FxtCompanyId { get; set; }
    }
}
