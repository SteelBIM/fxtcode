using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.AttributeHelper
{
    //byte 2014-12-5
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExportDisplayNameAttribute : Attribute
    {
        public ExportDisplayNameAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
