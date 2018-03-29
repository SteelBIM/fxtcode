using CAS.DataAccess.BaseDAModels;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class BaseBL
    {
        public static void BeginBaseDATransaction()
        {
            Base.BeginBaseDATransaction(SqlServerSet.ConnectionName);
        }

        public static void CommitBaseDATransaction()
        {
            Base.CommitBaseDATransaction(SqlServerSet.ConnectionName);
        }

        public static void RollbackBaseDATransaction()
        {
            Base.RollbackBaseDATransaction(SqlServerSet.ConnectionName);
        }

    }
}
