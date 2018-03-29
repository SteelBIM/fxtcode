using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class Customer
    {
       public long CustId { get; set; }

       public string CompanyName { get; set; }

       public string UserId { get; set; }

       public string Password { get; set; }
    }
}
