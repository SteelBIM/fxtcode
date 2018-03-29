using System;
using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using System.Data;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;


namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ImportTask : IImportTask
    {

        public int AddTask(DAT_ImportTask it)
        {
            var strSql = @"insert into FxtDataCenter.dbo.DAT_ImportTask(importtype,taskname,cityid,fxtcompanyid,createdate,creator,iscomplete,succeednumber,nameerrnumber,dataerrnumber,filepath) 
values(@importtype,@taskname,@cityid,@fxtcompanyid,@createdate,@creator,@iscomplete,@succeednumber,@nameerrnumber,@dataerrnumber,@filepath);
select SCOPE_IDENTITY() as Id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                //conn.Execute(strSql, it);
                //dynamic identity = conn.Query("SELECT @@IDENTITY AS Id").Single();
                //return Convert.ToInt32(identity.Id);
                dynamic identity = conn.Query(strSql, it).Single();
                return Convert.ToInt32(identity.Id);
            }
        }

        public IQueryable<DAT_ImportTask> GetTask(int importType, int cityid, int fxtcompanyid)
        {
            var strSql = @"select * from FxtDataCenter.dbo.DAT_ImportTask with(nolock) where cityid= @cityid and fxtcompanyid = @fxtcompanyid and importtype=@importtype order by createdate desc";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_ImportTask>(strSql, new { cityid, fxtcompanyid, importType }).AsQueryable();
            }
        }

        public int UpdateTask(long taskId, int succeedNumber, int dataErrNumber, int nameErrNumber, string filePath, int isComplete, int steps = 0)
        {
            var str = string.Empty;
            if (steps > 0) str += ",Steps=@Steps";

            var strSql = @"Update FxtDataCenter.dbo.DAT_ImportTask with(rowlock) set SucceedNumber=@SucceedNumber,DataErrNumber=@DataErrNumber,NameErrNumber=@NameErrNumber,FilePath=@FilePath,
IsComplete=@IsComplete " + str + "  where taskid = @TaskId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { TaskId = taskId, SucceedNumber = succeedNumber, DataErrNumber = dataErrNumber, NameErrNumber = nameErrNumber, FilePath = filePath, IsComplete = isComplete, Steps = steps });
            }
        }

        public int UpdateTask(long taskId, int nameErrNumber)
        {
            var strSql = @"
Update FxtDataCenter.dbo.DAT_ImportTask with(rowlock) 
set NameErrNumber = (case when NameErrNumber - @NameErrNumber < 0 then 0 else NameErrNumber - @NameErrNumber end)
    ,SucceedNumber = SucceedNumber + (case when @NameErrNumber < 0 then 0 else @NameErrNumber end)
where taskid = @TaskId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { TaskId = taskId, NameErrNumber = nameErrNumber });
            }
        }

        public SYS_ImportSet GetFieldProperty(string fieldName)
        {
            var strSql = "select * from SYS_ImportSet with(nolock) where columnName = @columnname";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_ImportSet>(strSql, fieldName).AsQueryable().FirstOrDefault();
            }
        }

        public int DeleteTask(long taskId)
        {
            var strSql = "delete from FxtDataCenter.dbo.DAT_ImportTask where taskId=@taskId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { taskId });
            }
        }

        //public int UpdateTaskSteps(long taskId, int steps)
        //{
        //    var strSql = "Update FxtDataCenter.dbo.DAT_ImportTask with(rowlock) set Steps =@Steps  where taskid = @TaskId";
        //    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
        //    {
        //        return conn.Execute(strSql, new { TaskId = taskId, Steps = steps });
        //    }


        //}

        public int TaskStepsIncreased(long taskId)
        {
            var strSql = "Update FxtDataCenter.dbo.DAT_ImportTask with(rowlock) set Steps = (case when Steps<99 then Steps + 1 else Steps end)  where taskid = @TaskId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { TaskId = taskId });
            }
        }

        public DAT_ImportTask GetTaskById(int taskId)
        {
            var strSql = @"select * from FxtDataCenter.dbo.DAT_ImportTask with(nolock) where taskId=@taskId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_ImportTask>(strSql, new { taskId }).AsQueryable().FirstOrDefault();
            }
        }

        public int UpdateCaseTaskNameErrNumber(int taskid)
        {
            var strSql = @"
update FxtDataCenter.dbo.DAT_ImportTask set NameErrNumber = (select COUNT(*) from fxtdatacenter.dbo.DAT_CaseTemp where TaskID = @TaskID)
where TaskID = @TaskID and ImportType = 1212003";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { TaskId = taskid });
            }
        }
    }
}
