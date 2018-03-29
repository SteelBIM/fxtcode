using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.Serialization;

namespace CaseExport
{
    internal class CurrentData
    {
        #region Fields
        ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
        

        #endregion

        private CurrentData()
        {
            QueryResult = new Dictionary<int, TableQueryResponseModel>();
        }

        class UniqueInstance
        {
            internal static CurrentData instance;
            static UniqueInstance()
            {
                instance = new CurrentData();
            }
        }
        public static CurrentData Instance
        {
            get
            {
                return UniqueInstance.instance;
            }
        }

        public ILog Logger
        {
            get
            {
                return logger;
            }
        }

        public Dictionary<int, TableQueryResponseModel> QueryResult { get; set; }
      
    }

    

}
