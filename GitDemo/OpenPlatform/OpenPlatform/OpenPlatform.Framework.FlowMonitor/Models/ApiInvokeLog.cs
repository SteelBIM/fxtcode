using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPlatform.Framework.FlowMonitor.Models
{
  public  class ApiInvokeLog
    {
      public int Id { get; set; }
      public int CompanyId { get; set; }
      public DateTime InvokeTime { get; set; }
      public int ApiType { get; set; }
      public int DataItem { get; set; }
      public string Ip { get; set; }
      public string FunctionName { get; set; }
    }
}
