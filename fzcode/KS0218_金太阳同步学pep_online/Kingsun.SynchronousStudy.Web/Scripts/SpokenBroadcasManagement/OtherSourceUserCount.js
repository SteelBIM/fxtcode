///////////其它来源用户统计//////////// 
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
                        field: 'ID', title: 'ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.ID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'UserName', title: '姓名', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.UserName + '</a>';
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
                             field: 'CreateTime', title: '创建时间', width: 150, align: 'center', formatter: function (value, row) {
                                 var html = '';
                                 var CreateTime = new Date(eval('new ' + (row.CreateTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                                 html += ' <span>' + CreateTime + '</a>';
                                 return html;
                             }
                         }
            ]]
        });
    }
    //通过关键字搜索
    $("#searchValue").focus(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "请输入姓名") {
            $("#searchValue").val("");
        }
    })
    $("#searchValue").blur(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "") {
            $("#searchValue").val("请输入姓名");
        }
    })
    $("#search").click(function () {
        var searchValue = $("#searchValue").val();
        var txtStartTime = $("#txtStartTime").val();
        var txtEndTime = $("#txtEndTime").val();
        //alert(txtStartTime);
        //alert(txtEndTime);
        var queryStr = "";
        if (searchValue == "请输入姓名" & txtStartTime == "" & txtEndTime == "") {
            $('#tbdatagrid').datagrid({
                url: '?action=Query'
            });
            return false;
        }
        if (searchValue == "请输入姓名") {
            queryStr += " 1=1 ";
        } else {
            queryStr += "   UserName like '%" + searchValue + "%'";
        }
        
        if (txtStartTime != "" & txtEndTime != "") {
            if (txtStartTime > txtEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            } else {
                queryStr += " and CreateTime between '" + txtStartTime + "' and '" + txtEndTime + "'";
            }
        } else {
            if (txtStartTime != "") {
                queryStr += " and CreateTime>='" + txtStartTime + "'";
            }
            if (txtEndTime != "") {
                queryStr += " and CreateTime<='" + txtEndTime + "'";
            }
        }

        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    })
    $("#exportExcel").click(function () {
        var searchValue = $("#searchValue").val();
        var queryStr = "";
        queryStr += "   UserName like '%" + searchValue + "%'";
        window.open("?action=excel&queryStr=" + encodeURI(queryStr));
    });
}

var courseListInit;
$(function () {
    courseListInit = new CourseListInit();
    courseListInit.Init();
});