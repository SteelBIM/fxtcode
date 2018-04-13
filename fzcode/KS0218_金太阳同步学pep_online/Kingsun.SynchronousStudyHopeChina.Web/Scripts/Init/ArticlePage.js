/// <reference path="../../jquery-easyui/jquery.min.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="../Common/Constant.js" />
/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../Management/ArticleManagement.js" />
var ArticlePageInit = function () {
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
                    field: ' ID', title: 'id', width: 150, align: 'center', hidden: 'true'
                },
                {
                    field: "ATitle", title: "标题", width: 120, align: 'center'
                },
                {
                    field: "AContent", title: "文章内容", width: 120, align: 'center'
                },
                {
                    field: "APeriod", title: "所属学段", width: 120, align: 'center', formatter: function (value, row) {
                        var text = '';
                        switch ($.trim(row.APeriod)) {
                            case "F1":
                                text = "小学1~2年级(F)";
                                break;
                            case "F2":
                                text = "小学3~4年级(F)";
                                break;
                            case "F3":
                                text = "小学5~6年级(F)";
                                break;
                            case "F4":
                                text = "初中(F)";
                                break;
                            case "F5":
                                text = "高中(F)";
                                break;
                            case "F6":
                                text = "大学(F)";
                                break;
                            default:
                                text = "错误";
                                break;
                        }
                        return text;
                    }
                },
                {
                    field: "ARemark", title: "评测文本", width: 120, align: 'center'
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

        $("#search").click(function() {
            loadData(getWhere(), 1, Current.pageSize);
        });

        //通过关键字搜索
        $("#searchValue").focus(function () {
            var searchValue = $("#searchValue").val();
            if (searchValue == "查询文章标题") {
                $("#searchValue").val("");
            }
        })
        $("#searchValue").blur(function () {
            var searchValue = $("#searchValue").val();
            if (searchValue === "") {
                $("#searchValue").val("查询文章标题");
            }
        })

    }

}

//获取分页数据并且绑定到表格
function loadData(whereCondition, currentPageIndex, pageSize) {
    articleManage.QueryCourse(whereCondition, currentPageIndex, pageSize, function (data) {
        if (data.Data == null || data.Data == undefined) {
            data.Data = { total: 0, rows: [] };
        }
        $("#tbdatagrid").datagrid("loadData", data.Data);
    });

}

function getWhere() {
    var txt = $("#searchValue").val();
    var where = "";
    if (txt != null && txt != "" && txt != "查询文章标题") {
        var errormsg = Common.ValidateTxt(txt);
        if (errormsg == '') {
            where = "ATitle like '%" + txt + "%'";
        } else {
            alert(errormsg);
        }
    } else {
        where = "1=1";
    }
    return where;
}

function showContent(content) {
    easyuiMessage.Alert(content, "文章详情","info");

}
