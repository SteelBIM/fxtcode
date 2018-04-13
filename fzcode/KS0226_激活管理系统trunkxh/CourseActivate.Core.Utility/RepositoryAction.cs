using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Core.Utility
{
    public class RepositoryAction
    {
        public Acitons Actions { get; set; }

        public object Entity { get; set; }

        public List<object> Entities { get; set; }

        public string[] DisableColumns { get; set; }

        public string sql { get; set; }

        public object Param { get; set; }
    }

    public enum Acitons
    {
        Insert,
        InsertRange,
        Delete,
        Update,
        ExecuteSql
    }
}
