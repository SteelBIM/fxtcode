using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class StringValueAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        private readonly int _SortIndex;

        public int SortIndex
        {
            get { return _SortIndex; }
        }
        #endregion

        #region Constructor
        public StringValueAttribute(int sortIndex, string value)
        {
            this._SortIndex = sortIndex;
            this.StringValue = value;
        }

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
        #endregion
    }
}
