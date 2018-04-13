///////////课程统计//////////// 
var CourseListInit = function () {
    var Current = this;
    this.Init = function () {
        Current.InitCourseList();
    };
    //初始化列表
    this.InitCourseList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=Query",
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            columns: [[
                    {
                        field: 'CourseID', title: '课程ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.CourseID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'CourseName', title: '课程名称', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.CourseName + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'CourseAppointNum', title: '课程预约人数', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.CourseAppointNum + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'CourseCount', title: '课程上课人数', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.CourseCount + '</a>';
                             return html;
                         }
                     },
                     {
                         field: 'CoursePeriodID', title: '课时ID', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.CoursePeriodID + '</a>';
                             return html;
                         }
                     },
                      {
                          field: 'CoursePeriodName', title: '课时名称', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.CoursePeriodName + '</a>';
                              return html;
                          }
                      },
                       {
                           field: 'StartTime', title: '课时上课时间', width: 150, align: 'center', formatter: function (value, row) {
                               var html = '';
                               var StartTime = new Date(eval('new ' + (row.StartTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                               html += ' <span>' + StartTime + '</a>';
                               return html;
                           }
                       },
                      {
                          field: 'CoursePeriodAppointNum', title: '课时预约人数', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.CoursePeriodAppointNum + '</a>';
                              return html;
                          }
                      },
                      {
                          field: 'CoursePeriodCount', title: '课时上课人数', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.CoursePeriodCount + '</a>';
                              return html;
                          }
                      }
            ]]
        });
    }
    //通过关键字搜索
    //$("#searchValue").focus(function () {
    //    var searchValue = $("#searchValue").val();
    //    if (searchValue == "请输入课程名称") {
    //        $("#searchValue").val("");
    //    }
    //})
    //$("#searchValue").blur(function () {
    //    var searchValue = $("#searchValue").val();
    //    if (searchValue == "") {
    //        $("#searchValue").val("请输入课程名称");
    //    }
    //})
    $("#search").click(function () {
        var selType = $("#selType").val();
        var searchValue = $("#searchValue").val();
        var txtSearchStartTime = $("#txtSearchStartTime").val();
        var txtSearchEndTime = $("#txtSearchEndTime").val();
        var queryStr = ""; 
        if (selType == 0) {//课程
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " a.Name like '%" + searchValue + "%'";
            }
        } else {    //课时
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " b.Name like '%" + searchValue + "%'";
            }
        }
        if (txtSearchStartTime != "" && txtSearchEndTime != "") {
            if (txtSearchStartTime > txtSearchEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            }
            queryStr += "and StartTime between '" + txtSearchStartTime + "' and '" + txtSearchEndTime + "' ";
        } else {
            if (txtSearchStartTime != "") {
                queryStr += "and StartTime>='" + txtSearchStartTime + "'";
            }
            if (txtSearchEndTime != "") {
                queryStr += "and StartTime<='" + txtSearchEndTime + "'";
            }
        }
        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    })
    $("#exportExcel").click(function () {
        var selType = $("#selType").val();
        var searchValue = $("#searchValue").val();
        var txtSearchStartTime = $("#txtSearchStartTime").val();
        var txtSearchEndTime = $("#txtSearchEndTime").val();
        var queryStr = "";
        if (selType == 0) {//课程
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " a.Name like '%" + searchValue + "%'";
            }
        } else {    //课时
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " b.Name like '%" + searchValue + "%'";
            }
        }
        if (txtSearchStartTime != "" && txtSearchEndTime != "") {
            if (txtSearchStartTime > txtSearchEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            }
            queryStr += "and StartTime between '" + txtSearchStartTime + "' and '" + txtSearchEndTime + "' ";
        } else {
            if (txtSearchStartTime != "") {
                queryStr += "and StartTime>='" + txtSearchStartTime + "'";
            }
            if (txtSearchEndTime != "") {
                queryStr += "and StartTime<='" + txtSearchEndTime + "'";
            }
        }
        window.open("?action=excel&queryStr=" + encodeURI(queryStr));
    });
}

var courseListInit;
$(function () {
    courseListInit = new CourseListInit();
    courseListInit.Init();
});