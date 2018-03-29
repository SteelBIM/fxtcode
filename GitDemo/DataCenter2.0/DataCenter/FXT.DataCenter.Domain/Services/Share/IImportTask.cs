using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IImportTask
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="it"></param>
        /// <returns></returns>
        int AddTask(DAT_ImportTask it);

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        IQueryable<DAT_ImportTask> GetTask(int importType, int cityid, int fxtcompanyid);

        /// <summary>
        /// 获取单条任务信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        DAT_ImportTask GetTaskById(int taskId);

        /// <summary>
        /// 更新任务表
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="succeedNumber">成功数量</param>
        /// <param name="dataErrNumber">错误数量</param>
        /// <param name="nameErrNumber">名称错误数量</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="isComplete"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        int UpdateTask(long taskId, int succeedNumber, int dataErrNumber, int nameErrNumber, string filePath, int isComplete, int steps = 0);

        /// <summary>
        /// 更新任务表
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="nameErrNumber">楼盘名称不匹配数量</param>
        /// <returns></returns>
        int UpdateTask(long taskId, int nameErrNumber);

        /// <summary>
        /// 进度自增
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        int TaskStepsIncreased(long taskId);

        /// <summary>
        /// 删除任务列表
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        int DeleteTask(long taskId);
        
        /// <summary>
        /// 根据字段名称获取字段属性
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        SYS_ImportSet GetFieldProperty(string fieldName);
        
        /// <summary>
        /// 更新楼盘名称不匹配的案例总条数
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        int UpdateCaseTaskNameErrNumber(int taskid);
    }
}
