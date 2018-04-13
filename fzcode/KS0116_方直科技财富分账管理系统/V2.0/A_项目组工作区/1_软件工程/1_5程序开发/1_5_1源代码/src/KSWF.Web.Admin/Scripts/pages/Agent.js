$(function () {
    addcloud();
    LoadTreeDept();
    $.post("/Agent/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Add)
                dataaction += "<button style=\"background-color:#E16965; color:#fff;border-color: #E16965; margin-right:15px; border-radius:5px;\" onclick=\"location.href = '/Agent/Agent_Add'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>新增 </button>"
            if (jsonData.Export)//导出
                dataaction += "<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727; border-radius:5px;\"  onclick=\"ExportUser()\"   type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>"
            $("#toolbar").html(dataaction);
            var oTable = new TableInit();
            oTable.Init(jsonData.Detailed, jsonData.Edit, jsonData.Pullblack);
            //if (jsonData.Move)//移动
            //    dataaction += "<button onclick=\"location.href = '/Employee/Employee_Move'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>移动 </button>"
            //if (jsonData.Locking)//锁定
            //    dataaction += "<button onclick=\"location.href = '/Employee/Employee_Locking'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>锁定 </button>"
            //if (jsonData.Locking)//解锁
            //    dataaction += "<button onclick=\"location.href = '/Employee/Employee_Locking'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>解锁 </button>"
            //if (jsonData.Pullblack)//拉黑
            //    dataaction += "<button onclick=\"location.href = '/Employee/Employee_Pullblack'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>拉黑 </button>"
            //if (jsonData.Blacklist)//黑名单
            //    dataaction += "<button onclick=\"location.href = '/Employee/Employee_Blacklist'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>黑名单 </button>"
        }
    });
    $("#searchbtn").click(function () {
        // $('#tb_departments').bootstrapTable("refresh");
        $('#tb_departments').bootstrapTable('selectPage', 1);
    })
});
//加载树形菜单
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
    $("#deptid").val(id);
    $('#tb_departments').bootstrapTable('selectPage', 1);
}

//初始化数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (detailed, edit, pullblack) {
        $('#tb_departments').bootstrapTable({
            url: '/Agent/Agent_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
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
                checkbox: true
            }, {
                field: 'masterid',
                visible: false
            }, {
                field: 'agentname',
                title: '代理商名称'
            }, {
                field: 'mastername',
                title: '用户名'
            }, {
                field: 'truename',
                title: '负责人姓名'
            }, {
                field: 'parentname',
                title: '渠道经理'
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
                field: 'agent_enddate',
                title: '签约截止日期',
                formatter: function (value, row, index) {
                    return formatDate(row.agent_enddate, 'YYYY-MM-dd');
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (detailed)
                        action += " <a  href=\"/Agent/Agent_Detailed?Id=" + row.masterid + "\"> 预览 <a> "
                    if (edit)
                        action += "<a  href=\"/Agent/Agent_Add?Id=" + row.masterid + "\"> 编辑 <a> "
                    if (pullblack)
                        action += " <a href=\"javascript:;\" onclick=\"AgentPullBlack('" + row.agentid + "')\"> 拉黑</a>";
                    return action;
                }
            }]
        });
    };
    removecloud();
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码
            agentname: $.trim($("#agentname").val()),
            deptid: $("#deptid").val()
        };
        return temp;
    };
    return oTableInit;
};

//导出表格
function ExportUser() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/Agent/Agent_Export");
    var agentname = $.trim($("#agentname").val());
    var parentname = "";
    $form.append('<input type="hidden" name="agentname" value="' + agentname + '" />');
    $form.append('<input type="hidden" name="parentname" value="' + parentname + '" />');
    removecloud();
    $(document.body).append($form);
    $form.submit();
    $form.remove();
}

function format(date) {
    var datestr = new Date(date).toLocaleDateString();
    var reg = new RegExp("/", "g");
    var datefomart = datestr.replace(reg, "-");
    return datefomart;
}

//拉黑
function AgentPullBlack(agentid) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("拉黑后将不可恢复！并且将会清理所有部门、员工及下级代理商的区域信息。请确认此操作~", function (result) {
        if (result) {
            $.post("/Agent/AgentPullBlack", { agentid: agentid }, function (reust) {
                if (reust) {
                    bootbox.alert("拉黑成功~");
                    $('#tb_departments').bootstrapTable('selectPage', 1);
                } else {
                    bootbox.alert("拉黑失败！请重试~");
                }
            });
        }
    });
}