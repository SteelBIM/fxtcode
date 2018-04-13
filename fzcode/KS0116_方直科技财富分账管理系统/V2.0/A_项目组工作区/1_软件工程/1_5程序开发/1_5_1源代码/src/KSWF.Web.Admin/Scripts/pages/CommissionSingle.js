// 应该付提成结算单
var Revoked = false;//是否有调账功能
$(function () {
    addcloud();
    $.post("/CommissionSingle/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var employeedataaction = "";
            if (jsonData.Export)//导出
            {
                $("#toolbaremployee").html("<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\" onclick=\"ExportBill()\"   type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>");
                $("#agentToolbar").html("<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\" onclick=\"AgentExportBill()\"   type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>");
                $("#sectionToolbar").html("<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\" onclick=\"DeptExportBill()\"   type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>");
            }
            if (jsonData.Revoked) {
                Revoked = true;
            }
            var dataaction = "";
            if (jsonData.Agent)
                dataaction += "<li go=\"0\" onclick=\"loaddata(0,this);\" >  <a href=\"#agent1\" data-toggle=\"tab\">代理商</a> </li>";
            if (jsonData.Employee)
                dataaction += "<li  go=\"1\"  onclick=\"loaddata(1,this);\">  <a href=\"#comp\" data-toggle=\"tab\">公司员工</a> </li>";
            if (jsonData.Dept)
                dataaction += "<li  go=\"2\"  onclick=\"loaddata(2,this);\">  <a href=\"#section\" data-toggle=\"tab\">公司部门</a></li>";
            $(".nav-pills").html(dataaction);
            var ptype = 0;;
            if (getQueryString("t") != null && getQueryString("t") > 0)
                ptype = getQueryString("t");
            if (ptype == 0) {
                $(".nav-pills li").eq(ptype).attr("class", "active");
                $(".nav-pills li").eq(ptype).click();
            }
            else {
                $($(".nav-pills li")).each(function (i, item) {
                    if ($(item).attr("go") == ptype) {
                        $(item).attr("class", "active");
                        $(item).click();
                    }
                });
            }
        }
    });
});

//加载代理商员工及部门按钮
function loaddata(type, obj) {
    $(".fade").hide();
    $($(obj).find("a").attr("href")).show();
    $("#deptid").val(0);
    $("#itype").val(type);
    LoadTreeDept();
    LoadUser();
}

 

//代理商结算展示
var TableInitAgent = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_agent').bootstrapTable({
            url: '/CommissionSingle/AgentBill_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#agentToolbar',                //工具按钮用哪个容器
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
            uniqueId: "osid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'osid',
                visible: false
            }, {
                field: 'os_no',
                title: '结算单编号'
            }, {
                field: 'agentname',
                title: '代理商'
            }, {
                title: '结算时间',
                formatter: function (value, row) {
                    return addDate(numbercontime(row.enddate, ""), -1);
                }
            }, {
                field: 'total_amount',
                title: '结算销售额'
            }, {
                field: 'total_count',
                title: '结算订单数'
            }, {
                field: 'total_bonus',
                title: '结算提成金额',
                formatter: function (value, row) {
                    return DataToFixed(row.adjust_amount + value);
                }
            }, {
                field: 'truename',
                title: '负责人姓名'
            }, {
                field: 'deptname',
                title: '部门'
            }, {
                field: 'channelmanager',
                title: '渠道经理'
            }, {
                field: 'createtime',
                title: '生成日期',
                formatter: function (value) {
                    return formatDate(value, "YYYY-MM-dd");
                }
            }, {
                field: 'addname',
                title: '操作人'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += " <a  href=\"/CommissionSingle/ViewBill?os_no=" + row.os_no + "&n=" + row.mastername_t + "\"> 查看 <a> "
                    if (Revoked)
                        action += " <a onclick=\"TransferAccount('" + row.osid + "'," + row.total_bonus + "," + row.adjust_amount + ",'" + row.adjust_reason + "')\"  href=\"javascript:;\"> 调账 <a> ";
                    return action;
                }
            }]
        });
    };
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            starttime: $("#startdate").val(),
            endtime: addDate($("#enddate").val(), 1),
            employeename: $("#agentname").val(),
            deptid: $("#deptid").val()
        };
        return temp;
    };
    return oTableInit;
};

//代理商结算提成总金额
function AgentBillTotal() {
    $.post("/CommissionSingle/AgentBillTotal", {
        starttime: $("#startdate").val(),
        endtime: addDate($("#enddate").val(), 1),
        employeename: $("#agentname").val(),
        deptid: $("#deptid").val()
    }, function (data) {
        if (data != null && data != "") {
            $("#agentbilltotal").html(DataToFixed(parseFloat(data)));
            removecloud();
        }
    });
}

//代理商 搜索
$("#searchbtn").click(function () {
    $('#tb_agent').bootstrapTable('selectPage', 1);
    AgentBillTotal();
});

//代理商导出
function AgentExportBill() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/CommissionSingle/AgentExport");
    var starttime = $("#startdate").val();
    var endtime = addDate($("#enddate").val(), 1);
    var employeename = $("#agentname").val();
    $form.append('<input type="hidden" name="truename" value="' + starttime + '" />');
    $form.append('<input type="hidden" name="endtime" value="' + endtime + '" />');
    $form.append('<input type="hidden" name="mastername_t" value="' + employeename + '" />');
    $form.append('<input type="hidden" name="deptid" value="' + deptid + '" />');
    $(document.body).append($form);
    removecloud();
    $form.submit();
    $form.remove();
}


//员工
var TableInitEmployee = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_employeements').bootstrapTable({
            url: '/CommissionSingle/EmployeeBill_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#toolbaremployee',                //工具按钮用哪个容器
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
            uniqueId: "osid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'osid',
                visible: false
            }, {
                field: 'os_no',
                title: '结算单编号'
            }, {
                field: 'truename',
                title: '员工姓名'
            }, {
                title: '结算时间',
                formatter: function (value, row) {
                    return addDate(numbercontime(row.enddate, ""), -1);
                }
            }, {
                field: 'total_amount',
                title: '结算销售额'
            }, {
                field: 'total_count',
                title: '结算订单数'
            }, {
                field: 'total_bonus',
                title: '结算提成金额',
                formatter: function (value, row) {
                    var bonus = (row.total_bonus + row.adjust_amount);
                    return DataToFixed(bonus);
                }
            }, {
                field: 'deptname',
                title: '部门'
            }, {
                field: 'createtime',
                title: '生成日期',
                formatter: function (value) {
                    return formatDate(value, "YYYY-MM-dd");
                }
            }, {
                field: 'addname',
                title: '操作人'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += " <a  href=\"/CommissionSingle/EmployeeBill?os_no=" + row.os_no + "&n=" + row.mastername_t + "\"> 查看 <a> ";
                    if (Revoked)
                        action += " <a onclick=\"TransferAccount('" + row.osid + "'," + row.total_bonus + "," + row.adjust_amount + ",'" + row.adjust_reason + "')\"  href=\"javascript:;\"> 调账 <a> ";
                    return action;
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            starttime: $("#employeestartdate").val(),
            endtime: addDate($("#employeeenddate").val(), 1),
            employeename: $("#employeename").val(),
            deptid: $("#deptid").val()
        };
        return temp;
    };
    return oTableInit;
};

//员工结算提成总金额
function EmployeeBillTotal() {
    $.post("/CommissionSingle/EmployeeBillTotal", {
        starttime: $("#employeestartdate").val(),
        endtime: addDate($("#employeeenddate").val(), 1),
        employeename: $("#employeename").val(),
        deptid: $("#deptid").val()
    }, function (data) {
        if (data != null && data != "") {
            $("#employeebilltotal").html(DataToFixed(parseFloat(data)));
            removecloud();
        }
    });
}

//员工 搜索
$("#employeesearch").click(function () {
    $('#tb_employeements').bootstrapTable('selectPage', 1);
    EmployeeBillTotal();
});

//员工导出
function ExportBill() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/CommissionSingle/Export");
    var starttime = $("#employeestartdate").val();
    var endtime = addDate($("#employeeenddate").val(), 1)
    var employeename = $("#employeename").val();
    var deptid = $("#deptid").val();
    $form.append('<input type="hidden" name="truename" value="' + starttime + '" />');
    $form.append('<input type="hidden" name="endtime" value="' + endtime + '" />');
    $form.append('<input type="hidden" name="mastername_t" value="' + employeename + '" />');
    $form.append('<input type="hidden" name="deptid" value="' + deptid + '" />');
    $(document.body).append($form);
    removecloud();
    $form.submit();
    $form.remove();
}


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
    addcloud();
    $("#deptid").val(id);
    var type = $("#itype").val();
    if (type == 0) {
        $('#tb_agent').bootstrapTable('selectPage', 1);
        AgentBillTotal();
    }
    if (type == 1) {
        $('#tb_employeements').bootstrapTable('selectPage', 1);
        EmployeeBillTotal();
    } else if (type == 2) {
        $('#tb_sectionDepartments').bootstrapTable('selectPage', 1);
        DeptBillTotal();
    }
    LoadUser();
}

//部门结算提成总金额
function DeptBillTotal() {
    $.post("/CommissionSingle/DeptBillTotal", {
        starttime: $("#deptstartdate").val(),
        endtime: addDate($("#deptenddate").val(), 1),
        principalname: $("#DeptPrincipalName").val(),
        deptid: $("#deptid").val()
    }, function (data) {
        if (data != null && data != "") {
            $("#deptbilltotal").html(DataToFixed(parseFloat(data)));
            removecloud();
        }
    });
}

//部门结算单展示
var TableInitdept = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_sectionDepartments').bootstrapTable({
            url: '/CommissionSingle/DeptBill_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#sectionToolbar',                //工具按钮用哪个容器
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
            uniqueId: "osid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'osid',
                visible: false
            }, {
                field: 'os_no',
                title: '结算单编号'
            }, {
                field: 'deptname',
                title: '部门'
            }, {
                title: '结算时间',
                formatter: function (value, row) {
                    return addDate(numbercontime(row.enddate, ""), -1);
                }
            }, {
                field: 'total_amount',
                title: '结算销售额',
                formatter: function (value, row) {
                    return DataToFixed(row.adjust_amount + value);
                }
            }, {
                field: 'total_count',
                title: '结算订单数'
            }, {
                field: 'total_bonus',
                title: '结算提成金额',
                formatter: function (value, row) {
                    return DataToFixed(row.adjust_amount + value);
                }
            }, {
                field: 'principalname',
                title: '负责人姓名'
            }, {
                field: 'createtime',
                title: '生成日期',
                formatter: function (value) {
                    return formatDate(value, "YYYY-MM-dd");
                }
            }, {
                field: 'addname',
                title: '操作人'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += " <a  href=\"/CommissionSingle/DeptBill?os_no=" + row.os_no + "&n=" + row.mastername_t + "\"> 查看 <a> ";
                    if (Revoked)
                        action += " <a onclick=\"TransferAccount('" + row.osid + "'," + row.total_bonus + "," + row.adjust_amount + ",'" + row.adjust_reason + "')\"  href=\"javascript:;\"> 调账 <a> ";
                    return action;
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            starttime: $("#deptstartdate").val(),
            endtime: addDate($("#deptenddate").val(), 1),
            principalname: $("#DeptPrincipalName").val(),
            deptid: $("#deptid").val()
        };
        return temp;
    };
    return oTableInit;
};

//部门 搜索
$("#deptsearch").click(function () {
    $('#tb_sectionDepartments').bootstrapTable('selectPage', 1);
    DeptBillTotal();
});

///部门导出
function DeptExportBill() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/CommissionSingle/DeptExportBill");
    var starttime = $("#employeestartdate").val();
    var endtime = addDate($("#employeeenddate").val(), 1);
    var principalname = $("#DeptPrincipalName").val();
    var deptid = $("#deptid").val();
    $form.append('<input type="hidden" name="truename" value="' + starttime + '" />');
    $form.append('<input type="hidden" name="endtime" value="' + endtime + '" />');
    $form.append('<input type="hidden" name="principalname" value="' + principalname + '" />');
    $form.append('<input type="hidden" name="deptid" value="' + deptid + '" />');
    $(document.body).append($form);
    removecloud();
    $form.submit();
    $form.remove();
}

// 订单guid 结算金额   调整金额  调整原因
function TransferAccount(o_on, Settlement, Adjustment, reason) {//调账
    if (reason != null && reason != "null") {
        $("#reason").val(reason);
    }
    else
        $("#reason").val("");
    $("#total_bonus").val(Settlement);
    $("#KontNumbering").val(o_on);
    $("#SettlementMoney").html(DataToFixed(Settlement + Adjustment));
    $("#Adjustment").val(Adjustment);
    $("#Settlement").modal("show");
}

function Adjustment() {
    var adjust_amount = $("#Adjustment").val();
    if (adjust_amount != "") {
        var reg = /^\-?[0-9]+(.[0-9]+)?$/;
        var r = adjust_amount.match(reg);
        if (r == null) {
            bootbox.alert('调整金额必须是有效的数字!');
            return false;
        }
    } else {
        adjust_amount = 0;
    }
    var total_bonus = $("#total_bonus").val();
    var bouns = DataToFixed(parseFloat(total_bonus) + parseFloat(adjust_amount));
    if (bouns > 0) {

    } else {
        bootbox.alert('调整金额已经超出了提成金额!请查证~');
        return false;
    }
    $("#AdjustmentTotal").html("调整后金额 ￥" + bouns);
}


//确定结算
$("#btn_settlement").click(function () {
    var adjust_amount = $("#Adjustment").val();
    if (adjust_amount != "") {
        var reg = /^\-?[0-9]+(.[0-9]+)?$/;
        var r = adjust_amount.match(reg);
        if (r == null) {
            bootbox.alert('调整金额必须是有效的数字!');
            return false;
        }
    } else {
        adjust_amount = 0;
    }
    var adjust_reason = $("#reason").val();
    if (adjust_reason != "" && adjust_reason.length > 200) {
        bootbox.alert('调整原因必须在200个字符内!');
        return false;
    }
    var total_bonus = $("#total_bonus").val();
    var bouns = DataToFixed(parseFloat(total_bonus) + parseFloat(adjust_amount));
    if (bouns > 0) {

    } else {
        bootbox.alert('调整金额已经超出了提成金额!请查证~');
        return false;
    }
    addcloud();
    var itype = $("#itype").val();
    $.post("/CommissionPay/Setbonus_update", { osid: $("#KontNumbering").val(), adjust_amount: adjust_amount, adjust_reason: adjust_reason, type: itype }, function (reust) {
        if (reust) {
            $("#Settlement").modal("hide");
            removecloud();
            bootbox.setDefaults("locale", "zh_CN");
            bootbox.alert("调账成功~", function (result) {
                if (itype == 0) {
                    $('#tb_agent').bootstrapTable("refresh");
                }
                else if (itype == 1) {
                    $('#tb_employeements').bootstrapTable("refresh");
                }
                else if (itype == 2) {
                    $('#tb_sectionDepartments').bootstrapTable("refresh");
                }
            });
        } else {
            bootbox.alert("调账失败，请重试~");
        }
    });
});



function LoadUser() {
    var type = $("#itype").val();
    if (type == 0) {//代理商
        $.post("/CommissionSingle/GetAgent", { deptid: $("#deptid").val() }, function (data) {
            $("#agentname").empty();
            $("#agentname").append($("<option>").text("全部").val(""));
            if (data != null && data != "") {
                $.each(data, function (index, item) {
                    var opt = $("<option>").text(item.agentname).val(item.mastername);
                    $("#agentname").append(opt);
                });
            }
            $('#agentname').selectpicker("refresh");
            AgentBillTotal();
            if ($('#tb_agent').html() == "") {
                var oTableAgent = new TableInitAgent();
                oTableAgent.Init();
            } else {
                $('#tb_agent').bootstrapTable('selectPage', 1);
            }
        });
    } else if (type == 1) {//员工
        $.post("/CommissionSingle/GetEmplloyee", { deptid: $("#deptid").val() }, function (data) {
            $("#employeename").empty();
            $("#employeename").append($("<option>").text("全部").val(""));
            if (data != null && data != "") {
                $.each(data, function (index, item) {
                    var opt = $("<option>").text(item.truename).val(item.mastername);
                    $("#employeename").append(opt);
                });
            }
            $('#employeename').selectpicker("refresh");
            EmployeeBillTotal();
            if ($('#tb_employeements').html() == "") {
                var oTableEmployee = new TableInitEmployee();
                oTableEmployee.Init();
            } else {
                $('#tb_employeements').bootstrapTable('selectPage', 1);
            }
        });
    } else if (type == 2) {//部门
        $.post("/CommissionSingle/DeptPrincipalName", { deptid: $("#deptid").val() }, function (data) {
            $("#DeptPrincipalName").empty();
            $("#DeptPrincipalName").append($("<option>").text("全部").val(""));
            if (data != null && data != "") {
                $.each(data, function (index, item) {
                    var opt = $("<option>").text(item.truename).val(item.truename);
                    $("#DeptPrincipalName").append(opt);
                });
            }
            $('#DeptPrincipalName').selectpicker("refresh");
            DeptBillTotal();
            if ($('#tb_sectionDepartments').html() == "") {
                var oTableInitdept = new TableInitdept();
                oTableInitdept.Init();
            } else {
                $('#tb_sectionDepartments').bootstrapTable('selectPage', 1);
            }
        });
    }
}