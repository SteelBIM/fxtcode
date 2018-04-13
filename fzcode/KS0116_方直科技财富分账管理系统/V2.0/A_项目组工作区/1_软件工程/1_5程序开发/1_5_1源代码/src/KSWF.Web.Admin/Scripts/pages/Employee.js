$(function () {
    addcloud();
    LoadTreeDept();
    $.post("/Employee/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Add)
                dataaction += "<button style=\"background-color:#E16965; color:#fff;border-color:#E16965; margin-right:15px; border-radius:5px;\"  onclick=\"location.href = '/Employee/Employee_Add'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>新增 </button>"
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
            if (jsonData.Export)//导出
                dataaction += "<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727; border-radius:5px;\" onclick=\"ExportUser()\"   type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>"

            $("#toolbar").html(dataaction);
            var oTable = new TableInit();
            oTable.Init(jsonData.Detailed, jsonData.Edit, jsonData.Pullblack);

        }
    });
    $("#searchbtn").click(function () {
        // $('#tb_departments').bootstrapTable("refresh");
        $('#tb_departments').bootstrapTable('selectPage', 1);
    })
});

//初始化数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (detailed, edit, black) {
        $('#tb_departments').bootstrapTable({
            url: '/Employee/Employee_View',         //请求后台的URL（*）
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
                field: 'mastername',
                title: '用户名'
            }, {
                field: 'truename',
                title: '真实姓名'
            }, {
                field: 'groupname',
                title: '角色'
            }, {
                field: 'deptname',
                title: '所属部门'
            }
            , {
                field: 'responsiblearea',
                title: '负责区域',
                formatter: function (value, row, index) {
                    return "<span title=\"" + value + "\"> " + SubStr(value, 15, "...") + "<span>";
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (detailed)
                        action += " <a  href=\"/Employee/Employee_Detailed?Id=" + row.masterid + "\"> 预览 <a> "
                    if (edit)
                        action += "<a  href=\"/Employee/Employee_Add?Id=" + row.masterid + "\"> 编辑 <a> "
                    if (black)
                        action += "<a href=\"javascript:;\" onclick=\" PullBlackEmplloy(" + row.masterid + ",'" + row.mastername + "','" + row.responsiblearea + "'," + row.deptid + ",'" + row.deptname + "');\">拉黑</a>";
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
            deptid: $("#deptid").val(),
            mastername: $.trim($("#searchkey").val()),
            type: 1
        };
        return temp;
    };
    return oTableInit;
};


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
        },
        error: function () {
            $.ajax({
                type: "Post",
                url: "/Employee/GetDept",
                dataType: "json",
                success: function (result) {
                    $('#tree').treeview({
                        data: result,
                        showIcon: false,
                        onNodeExpanded: function (event, node) {
                            //var tree = $('#tree');
                            //if (node.state.expanded) {//展开  
                            //    tree.treeview('expandNode', node.nodeId);
                            //} else { //折叠  
                            //    tree.treeview('collapseNode', node.nodeId);
                            //}
                        }
                    });
                }
            });
        }
    });
}

//点击事件
function itemOnclick(id) {
    $("#deptid").val(id);
    //$('#tb_departments').bootstrapTable("refresh");
    $('#tb_departments').bootstrapTable('selectPage', 1);
}

//导出表格
function ExportUser() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/Employee/Employee_Export");
    var mastername = $.trim($("#searchkey").val());
    var deptid = $("#deptid").val();
    $form.append('<input type="hidden" name="mastername" value="' + mastername + '" />');
    $form.append('<input type="hidden" name="deptid" value="' + deptid + '" />');
    $(document.body).append($form);
    removecloud();
    $form.submit();
    $form.remove();
}

//拉黑
function PullBlackEmplloy(masterid, mastername, responsiblearea, deptid, deptname) {
    $("#masterId").val(masterid);
    $("#mastername").val(mastername);
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("拉黑后将不可恢复！请确认此操作~", function (result) {
        if (result) {
            $("#deptname").html(deptname);
            //加载区域
            $.post("/Employee/GetDeptEmployee", { masterid: masterid, deptid: deptid }, function (jsondata) {
                if (responsiblearea != "" && jsondata != null && jsondata != "") {
                    isdeptemplayee = true;
                    $("#employeename").empty();
                    $("#handoveragent").empty();
                    $("#employeename").append(" <option value=''>选择交接人</option>");
                    $("#handoveragent").append(" <option value=''>选择交接人</option>");
                    $.each(jsondata, function (index, item) {
                        $("#employeename").append($("<option>").text(item.truename).val(item.mastername));
                        $("#handoveragent").append($("<option>").text(item.truename).val(item.masterid + "|" + item.mastername));
                    });
                    $("#PullblackUser").modal("show");
                } else {
                    //加载代理商
                    $.post("/Employee/GetEmployeeAllAgent", { masterid: masterid }, function (jsondata) {
                        if (jsondata != null && jsondata != "") {
                            var agentlist = "<ol>";
                            $.each(jsondata, function (index, item) {
                                agentlist += "<li>" + item.agentname + "</li>";
                            });
                            $("#agentlist").html(agentlist + "</ol>");
                            $("#HandoverAgent").modal("show");
                        } else {
                            PullBlack();
                        }
                    });
                }
            });
        }
    });
}

function handoverarea(type) { //1交接区域 2清空区域
    $("#channelhandover").val("");
    if (type == 1) {
        var employeename = $("#employeename").find("option:selected").val();
        if (employeename == undefined || employeename == "undefined" || employeename == "") {
            bootbox.alert("请选择区域交接人~");
            return false;
        } else {
            $("#areahandover").val(employeename);
        }
    } else {
        $("#areahandover").val();
    }
    $("#PullblackUser").modal("hide");
    var masterid = $("#masterId").val();
    $.post("/Employee/GetEmployeeAllAgent", { masterid: masterid }, function (jsondata) {
        if (jsondata != null && jsondata != "") {
            var agentlist = "";
            $.each(jsondata, function (index, item) {
                agentlist += "<br />" + item.agentname;
            });
            $("#agentlist").html(agentlist);
            $("#HandoverAgent").modal("show");
            return false;
        } else {
            PullBlack();
        }
    });
}


$("#btn_handoveragent").click(function () {//确定交接代理商
    var handoveragent = $("#handoveragent").find("option:selected").val();
    if (handoveragent == undefined || handoveragent == "undefined" || handoveragent == "") {
        bootbox.alert("请选择代理商交接人~");
    } else {
        $("#channelhandover").val(handoveragent);
        PullBlack();
    }
});

function PullBlack() {
    $.post("/Employee/EmplloyPullBlack", { masterid: $("#masterId").val(), mastername: $("#mastername").val(), areahandover: $("#areahandover").val(), channelhandover: $("#channelhandover").val() }, function (pullblackreust) {
        if (pullblackreust) {
            bootbox.alert("拉黑成功~");
            $("#HandoverAgent").modal("hide");
            //$('#tb_departments').bootstrapTable("refresh");
            $('#tb_departments').bootstrapTable('selectPage', 1);
        } else {
            bootbox.alert("拉黑失败！请重试~");
        }
    });
}

