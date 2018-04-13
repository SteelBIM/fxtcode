///////////上课用户统计//////////// 
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
                        field: 'UserID', title: '用户ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.UserID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'UserName', title: '用户名', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.UserName + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'TrueName', title: '真实姓名', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.TrueName + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'TelePhone', title: '联系方式', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.TelePhone + '</a>';
                             return html;
                         }
                     },
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
                             field: 'NewPrice', title: '课时价格', width: 80, align: 'center', formatter: function (value, row) {
                                 var html = '';
                                 html += ' <span>' + row.NewPrice + '</a>';
                                 return html;
                             }
                         },
                       {
                           field: 'CourseStartTime', title: '上课时间', width: 300, align: 'center', formatter: function (value, row) {
                               var html = '';
                               //var CourseStartTime = new Date(eval('new ' + (row.CourseStartTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                               html += ' <span>' + row.CourseStartTime + '</a>';
                               return html;
                           }
                       },
                        {
                            field: 'StartTime', title: '最早进入教室时间', width: 150, align: 'center', formatter: function (value, row) {
                                var html = '';
                                var StartTime = new Date(eval('new ' + (row.StartTime.replace(/\//g, '')))).format('yyyyMMdd hh:mm:ss');
                                html += ' <span>' + StartTime + '</a>';
                                return html;
                            }
                        },
                         {
                             field: 'EndTime', title: '最晚退出教室时间', width: 150, align: 'center', formatter: function (value, row) {
                                 var html = '';
                                 var EndTime = new Date(eval('new ' + (row.EndTime.replace(/\//g, '')))).format('yyyyMMdd hh:mm:ss');
                                 html += ' <span>' + EndTime + '</a>';
                                 return html;
                             }
                         }, {
                             field: 'OutTimes', title: '退出次数', width: 150, align: 'center', formatter: function (value, row) {
                                 var html = '';
                                 html += ' <span>' + row.OutTimes + '</a>';
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
        if (selType == 0) {//联系方式
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " TelePhone like '%" + searchValue + "%'";
            }
        } else if (selType == 1) {//课程名称
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " CourseName like '%" + searchValue + "%'";
            }
        } else {    //课时名称
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " CoursePeriodName like '%" + searchValue + "%'";
            }
        }
        if (txtSearchStartTime != "" && txtSearchEndTime != "") {
            if (txtSearchStartTime > txtSearchEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            }
            queryStr += "and CourseStartTime between '" + txtSearchStartTime + "' and '" + txtSearchEndTime + "' ";
        } else {
            if (txtSearchStartTime != "") {
                queryStr += "and CourseStartTime>='" + txtSearchStartTime + "'";
            }
            if (txtSearchEndTime != "") {
                queryStr += "and CourseStartTime<='" + txtSearchEndTime + "'";
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
        if (selType == 0) {//联系方式
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " TelePhone like '%" + searchValue + "%'";
            }
        } else if (selType == 1) {//课程名称
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " CourseName like '%" + searchValue + "%'";
            }
        } else {    //课时名称
            if (searchValue == "") {
                queryStr += "1=1";
            } else {
                queryStr += " CoursePeriodName like '%" + searchValue + "%'";
            }
        }
        if (txtSearchStartTime != "" && txtSearchEndTime != "") {
            if (txtSearchStartTime > txtSearchEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            }
            queryStr += "and CourseStartTime between '" + txtSearchStartTime + "' and '" + txtSearchEndTime + "' ";
        } else {
            if (txtSearchStartTime != "") {
                queryStr += "and CourseStartTime>='" + txtSearchStartTime + "'";
            }
            if (txtSearchEndTime != "") {
                queryStr += "and CourseStartTime<='" + txtSearchEndTime + "'";
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