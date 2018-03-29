using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.DTO
{
   public class ApiInvokeLogDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime InvokeTime { get; set; }
        public int ApiType { get; set; }
        public int DataItem { get; set; }
        public string Ip { get; set; }
        public string FunctionName { get; set; }
        public int ProductTypeCode { get; set; }

        public string RequestParameter { get; set; }
    }
}
