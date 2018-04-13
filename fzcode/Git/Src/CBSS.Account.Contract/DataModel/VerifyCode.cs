using System;
using System.Linq;
using CBSS.Framework.Contract;
using System.Collections.Generic;
using CBSS.Core.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBSS.Account.Contract
{
    [Serializable]
    [Table("VerifyCode")]
    public class VerifyCode : ModelBase
    {
        public Guid Guid { get; set; }
        public string VerifyText { get; set; }
    }

}



