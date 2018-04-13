using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.BLL
{
    public class CourseImplement : BaseImplement
    {
        public override KingResponse ProcessRequest(KingRequest request)
        {
            if (string.IsNullOrEmpty(request.Function))
            {
                return KingResponse.GetErrorResponse("无法确定接口信息！", request);
            }
            if (string.IsNullOrEmpty(request.Data))
            {
                return KingResponse.GetErrorResponse("提交的数据不能为空！", request);
            }
            CourseManagement manage = new CourseManagement();
            KingResponse response = null;
            switch (request.Function.Trim())
            {
                case "AddCourse"://///新增课程
                    response = manage.AddCourse(request);
                    break;
                case "EditCourse"://///修改课程
                    response = manage.EditCourse(request);
                    break;
                case "ActiveCourse"://///激活应用
                    response = manage.ActiveCourse(request);
                    break;
                case "DisableCourse"://///禁用应用
                    response = manage.DisableCourse(request);
                    break;
                case "QueryCourse":////根据appid获取Course列表
                    response = manage.QueryCourse(request);
                    break;
                case "QueryCourseName":////查询课程名获取Course列表
                    response = manage.QueryCourseName(request);
                    break;
                case "SelectCourse"://///根据id查询course
                    response = manage.SelectCourse(request);
                    break;
                case "QueryCourseVersion":////查询获取CourseVersion列表
                    response = manage.QueryCourseVersion(request);
                    break;
                case "SelectCourseVersion"://///获取CourseVersion列表
                    response = manage.SelectCourseVersion(request);
                    break;
                case "ActiveCourseVersion"://///激活应用
                    response = manage.ActiveCourseVersion(request);
                    break;
                case "DisableCourseVersion"://///禁用应用
                    response = manage.DisableCourseVersion(request);
                    break;
                case "UpdateCourse"://///更新课程
                    response = manage.UpdateCourse(request);
                    break;
                case "EditCourseVersion":////编辑版本
                    response = manage.EditCourseVersion(request);
                    break;
                case "SelectTopVersion":////获取最大的版本
                    response = manage.SelectTopVersion(request);
                    break;
                case "SelectFirstVersion":////获取最大的版本
                    response = manage.SelectFirstVersion(request);
                    break;
                case "DeleteCourse":////删除课程，包括版本
                    response = manage.DeleteCourse(request);
                    break;
                case "SelectMaxDisableVersion":////查询启用版本中的最大版本
                    response = manage.SelectMaxDisableVersion(request);
                    break;
                default:
                    response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                    break;
            }
            return response;
        }
    }
}
