using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.Contract.DataModel
{
    [Auditable]
    [Table("Log4net")]
    public class Log4net
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Host { get; set; }
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
