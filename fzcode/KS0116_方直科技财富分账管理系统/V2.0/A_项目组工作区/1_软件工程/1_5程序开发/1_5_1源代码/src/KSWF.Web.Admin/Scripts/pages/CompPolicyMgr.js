$(function () {
    //LoadUser();
    addcloud();
    LoadTreeDept();
    $.post("/CompPolicyMgr/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            if (jsonData.Edit)
                $("#actionset").html("<button style=\"background-color:#E16965;color:#fff; border-color: #E16965;\" onclick=\"batchsetting()\" type=\"button\" class=\"btn btn-default\" ><span class=\"glyphicon\" aria-hidden=\"true\"></span>批量设置策略</button>");
            var oTable = new TableInit();
            oTable.Init(jsonData.Edit);
        }
    });
    $("#searchbtn").click(function () {
        $('#tb_departments').bootstrapTable('selectPage', 1);
    })

});

function batchsetting() {
    var selects = $('#tb_departments').bootstrapTable('getSelections');
    var ids = '';
    for (var i = 0; i < selects.length; i++) {
        if (ids) {
            ids += "," + selects[i].mastername;
        } else {
            ids += selects[i].mastername;
        }
    }
    if (ids != "") {
        $.post("/CompPolicyMgr/Jump", { masternames: ids }, function (data) {
            location.href = "/CompPolicyMgr/CompPolicyMgrbatch?t=0";
        });
    } else {
        bootbox.alert("请至少选择一个员工~");
    }
}

function unicode(s) {
    var len = s.length;
    var rs = "";
    for (var i = 0; i < len; i++) {
        var k = s.substring(i, i + 1);
        rs += (i == 0 ? "" : ",") + s.charCodeAt(i);
    }
    return rs;
}
//初始化数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit) {
        $('#tb_departments').bootstrapTable({
            url: '/CompPolicyMgr/CompPolicyMgr_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100, 500],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: false,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "masterid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                //title: "<input type=\"checkbox\"   name=\"btSelectAll\" >",
                //field: 'notrffectivePolicy',
                //formatter: function (value, row, index) {
                //    if ((row.rffectivePolicy != null && row.rffectivePolicy != "") || (value != null && value != "")) {
                //        return " ";
                //    }
                //    return "<input type=\"checkbox\" data-index=\"" + index + "\" name=\"btSelectItem\" >";
                //}
                checkbox: true
            }, {
                field: 'mastername',
                title: '用户名'
            }, {
                field: 'truename',
                title: '姓名'
            }, {
                field: 'deptname',
                title: '所属部门'
            }, {
                field: 'responsiblearea',
                title: '负责区域',
                formatter: function (value, row, index) {
                    return "<span title=\"" + value + "\"> " + SubStr(value, 15, "...") + "<span>";
                }
            }, {
                field: 'rffectivePolicy',
                title: '生效商务策略名称',
                formatter: function (value) {
                    return "<span title=\"" + value + "\"> " + SubStr(value, 15, "...") + "<span>";
                }
            }, {
                field: 'notrffectivePolicy',
                title: '未生效商务策略名称',
                formatter: function (value) {
                    return "<span title=\"" + value + "\"> " + SubStr(value, 15, "...") + "<span>";
                }
            },
            {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (edit)
                        action += "<a  href=\"/CompPolicyMgr/CompPolicyMgr_Add?Id=" + row.masterid + "\"> 编辑 <a> "
                    return action;
                }
            }]
        });
    };
    removecloud();
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码
            deptid: $("#deptid").val(),
            mastername: $("#username").val(),
            type: 0
        };
        return temp;
    };
    return oTableInit;
};

function LoadTreeDept() {
    $.ajax({
        type: "Post",
        url: "/AccountInfo/GetDept",
        dataType: "json",
        success: function (result) {
            $('#tree').treeview({
                data: result,
                showIcon: false,
                onNodeExpanded: function (event, node) {
                    var tree = $('#tree');
                    if (node.state.expanded) {//展开  
                        tree.treeview('expandNode', node.nodeId);
                    } else { //折叠  
                        tree.treeview('collapseNode', node.nodeId);
                    }
                },
                onNodeSelected: function (event, node) {
                    itemOnclick(node.tag);
                }
            });
        }
    });
}

//点击事件
function itemOnclick(id) {
    //$("#username").val("");
    $("#deptid").val(id);
    //LoadUser();
    $('#tb_departments').bootstrapTable('selectPage', 1);
}

//function LoadUser() {
//    $.post("/CompPolicyMgr/GetMasterList", { deptid: $("#deptid").val() }, function (data) {
//        $("#username").empty();
//        $("#username").append($("<option>").text("全部").val(""));
//        if (data != null && data != "") {
//            $.each(data, function (index, item) {
//                var opt = $("<option>").text(item.truename).val(item.truename);
//                $("#username").append(opt);
//            });
//        }
//        $('#username').selectpicker("refresh");
//        removecloud();
//    });
//}


