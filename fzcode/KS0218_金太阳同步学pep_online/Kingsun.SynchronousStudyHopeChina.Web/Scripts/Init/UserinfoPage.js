/// <reference path="../../jquery-easyui/jquery.min.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="../Common/Constant.js" />
/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../Management/UserinfoManagement.js" />
var UserinfoPageInit = function () {
    var Current = this;
    Current.pageSize = 10;
    this.Init = function () {
        //初始化表格
        $("#tbdatagrid").datagrid({
            title: '课程系统版本',
            nowrap: false,
            border: true,
            collapsible: false, //是否可折叠的  
            rownumbers: true,
            singleSelect: true,
            fitColumns: true,
            loadMsg: '正在加载数据...',
            pagination: true,    //分页控件  
            columns: [[
                {
                    field: 'Userid', title: 'id', width: 150, align: 'center', hidden: 'true'
                },
                {
                    field: "Username", title: "姓名", width: 120, align: 'center'
                },
                {
                    field: "AccountName", title: "用户名", width: 120, align: 'center'
                },
                {
                    field: "Parentname", title: "父母姓名", width: 120, align: 'center'
                },
                {
                    field: "Phone", title: "电话", width: 120, align: 'center'
                },
                {
                    field: "Parentname", title: "父母姓名", width: 120, align: 'center'
                },
                {
                    field: "Teachername", title: "老师姓名", width: 120, align: 'center'
                },
                {
                    field: "Teacherphone", title: "老师手机", width: 120, align: 'center'
                },
                {
                    field: "Schoolname", title: "学校名称", width: 120, align: 'center'
                },
                {
                    field: "Period", title: "阶段", width: 120, align: 'center'
                },
                {
                    field: "Isjoin", title: "是否参加", width: 120, align: 'center',formatter: function(value, row) {
                        if (row.Isjoin == "True") {
                            return "是";
                        } else {
                            return "否";
                        }
                    }
                },
                {
                    field: "IsFinish", title: "是否已交卷", width: 120, align: 'center', formatter: function(value, row) {
                        if (row.IsFinish == "True") {
                            return "是";
                        } else {
                            return "否";
                        }
                    }
                },
                {
                    field: "Createtime", title: "报名时间", width: 120, align: 'center'
                },
                {
                    field: "Finishtime", title: "交卷时间", width: 120, align: 'center'
                },
                {
                    field: "ArticleId", title: "文章id", width: 120, align: 'center'
                },
                {
                    field: "Tasktime1", title: "第一次成绩", width: 150, align: 'center', formatter: function (value, row) {
                        var html = '';
                        var score = 0;
                        if (row.Taskvalue1 != null && row.Taskvalue1 != 'null' && row.Taskvalue1 != undefined && row.Taskvalue1 != "") {
                            score = row.Taskvalue1;
                            html += '<span style="color:blue">' + score + '分</span>';
                            html += '&nbsp;<a href="' + row.Filepath1 + '">录音下载</a>';
                        } else {
                            html += '<span style="color:blue">0.00分</span>';
                            html += '&nbsp;无录音';
                        }

                        return html;

                    }
                },
                {
                    field: "Tasktime2", title: "第二次成绩", width: 150, align: 'center', formatter: function (value, row) {
                        var html = '';
                        var score = 0;
                        if (row.Taskvalue2 != null && row.Taskvalue2 != 'null' && row.Taskvalue2 != undefined && row.Taskvalue2 != "") {
                            score = row.Taskvalue2;
                            html += '<span style="color:blue">' + score + '分</span>';
                            html += '&nbsp;<a href="' + row.Filepath2 + '">录音下载</a>';
                        } else {
                            html += '<span style="color:blue">0.00分</span>';
                            html += '&nbsp;无录音';
                        }
                       
                        return html;

                    }
                },
                {
                    field: "Tasktime3", title: "第三次成绩", width: 150, align: 'center', formatter: function (value, row) {
                        var html = '';
                        var score = 0;
                        if (row.Taskvalue3 != null && row.Taskvalue3 != 'null' && row.Taskvalue3 != undefined && row.Taskvalue3 != "") {
                            score = row.Taskvalue3;
                            html += '<span style="color:blue">' + score + '分</span>';
                            html += '&nbsp;<a href="' + row.Filepath3 + '">录音下载</a>';
                        } else {
                            html += '<span style="color:blue">0.00分</span>';
                            html += '&nbsp;无录音';
                        }
                        return html;

                    }
                }
            ]]
        });
        var p = $('#tbdatagrid').datagrid('getPager');
        p.pagination({
            pageSize: Constant.PageSize, //每页显示的记录条数，默认为10  
            pageList: Constant.PageSizeList, //可以设置每页记录条数的列表  
            beforePageText: '第', //页数文本框前显示的汉字  
            afterPageText: '页    共 {pages} 页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            onRefresh: function (pageNumber, pageSize) {
                loadData(getWhere(), pageNumber, pageSize);
            },
            onSelectPage: function (pageNumber, pageSize) {
                loadData(getWhere(), pageNumber, pageSize);
            }
        });
        //初始获取数据
        loadData("1=1", 1, Current.pageSize);

        $("#search").click(function () {
            loadData(getWhere(), 1, Current.pageSize);
        });

        //通过关键字搜索
        $("#searchValue").focus(function () {
            var searchValue = $("#searchValue").val();
            if (searchValue == "查询用户姓名") {
                $("#searchValue").val("");
            }
        })
        $("#searchValue").blur(function () {
            var searchValue = $("#searchValue").val();
            if (searchValue === "") {
                $("#searchValue").val("查询用户姓名");
            }
        })

    }

}

//获取分页数据并且绑定到表格
function loadData(whereCondition, currentPageIndex, pageSize) {
    UserinfoManage.QueryUserinfo(whereCondition, currentPageIndex, pageSize, function (data) {
        if (data.Data == null || data.Data == undefined) {
            data.Data = { total: 0, rows: [] };
        }
        $("#tbdatagrid").datagrid("loadData", data.Data);
    });

}

function getWhere() {
    var txt = $("#searchValue").val();
    var where = "";
    if (txt != null && txt != "" && txt != "查询用户姓名") {
        var errormsg = Common.ValidateTxt(txt);
        if (errormsg == '') {
            where = "Username like '%" + txt + "%'";
        } else {
            alert(errormsg);
        }
        
    } else {
        where = "1=1";
    }
    return where;
}

