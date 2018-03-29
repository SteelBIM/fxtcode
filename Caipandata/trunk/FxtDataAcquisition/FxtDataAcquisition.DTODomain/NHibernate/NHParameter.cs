using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.NHibernate
{
    public class NHParameter
    {
        public string ParameterName
        {
            get;
            set;
        }
        public object ParameterValue
        {
            get;
            set;
        }
        public IType DbType
        {
            get;
            set;
        }
        public NHParameter(string parameterName, object parameterValue, IType dbType)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
            this.DbType = dbType;
        }
    }
}
