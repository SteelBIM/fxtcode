/// <reference path="jquery-1.8.0.min.js" />

$(function () {
    var userid = GetQueryString("UserID");
    var TeaqueryStr = " CreateUser='" + userid + "'";
    $("#searchBtn").click(function () {
        var value = $("#searchKey").val();
        var title = $("#searchKey").attr("title");
        if ($.trim(TeaqueryStr) && value != title) {
            var type = $("#searchType").val();
            //如果查询条件为空默认查询全部
                TeaqueryStr += " and " + type + " like '%" + value + "%'";
        }
            $('#tbdatagrid').datagrid({
                url: '?action=TeaQuery&queryStr=' + encodeURI(TeaqueryStr)
            });      
    });

    $('#tbdatagrid').datagrid({
        url: "?action=TeaQuery&queryStr=" + TeaqueryStr,
        pagination: true,
        rownumbers: true,
        toolbar: "#divtoolbar",
        fitColumns: true,
        striped: true,
        singleSelect: true,
        pagesize: 20,
        pageList: [20, 30, 40, 50],
        width: $(window).width() - 20,
        height: $(window).height() - 55,
        columns: [[
            { field: 'ClassID', title: '班级', align: 'center', width: 20 },
            { field: 'TaskTitle', title: '作业标题', align: 'center', width: 15 },
            {
                field: "TaskStartDate", title: "布置时间", width: 30, align: 'center', formatter: function (value) {
                    return FormatTime(value);
                }
            },
            {
                field: "TaskEndDate", title: "截止时间", width: 30, align: 'center', formatter: function (value) {
                    return FormatTime(value);
                }
            },
        {
            field: "TaskList", title: "作业详情", align: 'center', width: 15, formatter: function (value, rows) {
                var html = '';
                html = '<a href="javascript:void(0)" onclick="ShowDetail(\'' + rows.ID + '\',\'' + rows.ClassID + '\',\'' + rows.CreateUser + '\')">查看</a>';
                return html;
            }
        }
                    //{ field: "Operate", title: "操作", align: 'center', width: 15 }TeacherID
        ]]
    });
    //taskID, classID

});

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function ShowDetail(TaskID, ClassID, TeacherID) {
    if (TaskID != "" && ClassID != "" && TeacherID != "") {
        location = "/admin/user/ViewTaskDetail.aspx?TaskID=" + TaskID + "&ClassID=" + ClassID + "&TeacherID=" + TeacherID;
    }
    else {
        return;
    }
}