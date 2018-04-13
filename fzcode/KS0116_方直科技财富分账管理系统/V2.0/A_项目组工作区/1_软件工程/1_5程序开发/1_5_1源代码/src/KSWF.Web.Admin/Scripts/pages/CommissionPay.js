

//代理商总金额
function AgentTotal() {
    $.post("/CommissionPay/GetAgentTotal", {
        agentname: $("#agentname").val(),
        deptid: $("#deptid").val()
    }, function (data) {
        if (data != null && data != "") {
            var jsondata = eval(data);
            $("#sales").html(DataToFixed(jsondata[0].o_payamount));
            $("#ordernumber").html(jsondata[0].o_number);
            $("#amountmoney").html(DataToFixed(jsondata[0].o_bonus));
            removecloud();
        }
    });
}

//代理商数据展示
var TableInitAgent = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_agent').bootstrapTable({
            url: '/CommissionPay/AgentCommissionSttlement_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#agentToolbar',                //工具按钮用哪个容器
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
                field: 'masterid',
                visible: false
            }, {
                field: 'agentname',
                title: '代理商'
            }, {
                field: 'payamount',
                title: '未结算销售额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'ordernumber',
                title: '未结算订单数'
            }, {
                field: 'bouns',
                title: '未结算提成金额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'truename',
                title: '负责人姓名'
            }, {
                field: 'deptname',
                title: '所属部门'
            }, {
                field: 'channel',
                title: '渠道经理'
            }, {
                field: 'enddate',
                title: '上次结算日期',
                formatter: function (value) {
                    if (value == 0)
                        return "--";
                    else
                        return addDate(numbercontime(value, ""), -1);
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += " <a  href=\"/CommissionPay/AccountingAgent?n=" + row.mastername + "&tr=" + coding(row.agentname) + "&d=" + coding(row.deptname) + "&q=" + coding(row.channel) + "\"> 核算 <a> "
                    return action;
                }
            }]
        });
    };
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            agentname: $.trim($("#agentname").val()),
            deptid: $("#deptid").val()
        };
        return temp;
    };
    return oTableInit;
};

//代理商 搜索
$("#searchbtn").click(function () {
    $('#tb_agent').bootstrapTable('selectPage', 1);
    AgentTotal();
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
        },
        error: function () {
            alert("树形结构加载失败！")
        }
    });
}

//员工未提成总金额
function EmployeeTotal() {
    $.post("/CommissionPay/EmployeeTotal", {
        truename: $.trim($("#employeename").val()),
        deptid: $("#deptid").val()
    }, function (data) {
        if (data != null && data != "") {
            var jsondata = eval(data);
            $("#o_sales").html(DataToFixed(data.o_payamount));
            $("#o_number").html(data.o_number);
            $("#o_bonus").html(DataToFixed(data.o_bonus));
            removecloud();
        }
    });
}

//员工列表展示
var TableInitEmployee = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_employeements').bootstrapTable({
            url: '/CommissionPay/EmployeeCommissionSttlement_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbaremployee',                //工具按钮用哪个容器
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
                field: 'masterid',
                visible: false
            }, {
                field: 'truename',
                title: '员工姓名'
            }, {
                field: 'o_payamount',
                title: '未结算销售额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_number',
                title: '未结算订单数'
            }, {
                field: 'o_bonus',
                title: '未结算提成金额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'deptname',
                title: '所属部门'
            }, {
                field: 'endtime',
                title: '上次结算日期',
                formatter: function (value) {
                    if (value == 0)
                        return "--";
                    else {
                        return addDate(numbercontime(value, ""), -1);
                    }
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += " <a  href=\"/CommissionPay/Accounting?n=" + row.mastername + "&tr=" + coding(row.truename) + "&d=" + coding(row.deptname) + "\"> 核算 <a> "
                    return action;
                }
            }]
        });
    };
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            truename: $.trim($("#employeename").val()),
            deptid: $("#deptid").val()
        };
        return temp;
    };
    return oTableInit;
};

//员工 搜索
$("#employeesearch").click(function () {
    $('#tb_employeements').bootstrapTable('selectPage', 1);
    EmployeeTotal();
});

//点击事件
function itemOnclick(id) {
    addcloud();
    $("#deptid").val(id);
    var type = $("#itype").val();
    if (type == 0) {
        $('#tb_agent').bootstrapTable('selectPage', 1);
        AgentTotal();
    }
    if (type == 1) {
        $('#tb_employeements').bootstrapTable('selectPage', 1);
        EmployeeTotal();
    } else if (type == 2) {
        $('#tb_sectionDepartments').bootstrapTable('selectPage', 1);
        DeptTotal();
    }
    LoadUser();
}


function DeptTotal() {
    $.post("/CommissionPay/GetDeptTotal", {
        principalname: $.trim($("#DeptPrincipalName").val()),
        deptid: $("#deptid").val()
    }, function (data) {
        if (data != null && data != "") {
            var jsondata = eval(data);
            $("#dept_sales").html(DataToFixed(data.dept_sales));
            $("#dept_number").html(data.dept_number);
            removecloud();
        }
    });
}

//部门数据展示
var TableInitDept = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_sectionDepartments').bootstrapTable({
            url: '/CommissionPay/DeptommissionSttlement_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#sectionToolbar',                //工具按钮用哪个容器
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
            //uniqueId: "masterid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'm_deptid',
                visible: false
            }, {
                field: 'm_deptname',
                title: '部门'
            }, {
                field: 'o_payamount',
                title: '未结算销售额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_number',
                title: '未结算订单数'
            }, {
                field: 'principalname',
                title: '负责人姓名'
            }, {
                field: 'enddata',
                title: '上次结算日期',
                formatter: function (value) {
                    if (value == 0)
                        return "--";
                    else {
                        return addDate(numbercontime(value, ""), -1);
                    }
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += " <a  href=\"/CommissionPay/AccountingDept?id=" + row.m_deptid + "&tr=" + coding(row.principalname) + "&d=" + coding(row.m_deptname) + "\"> 核算 <a> "
                    return action;
                }
            }]
        });
    };
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            principalname: $.trim($("#DeptPrincipalName").val()),
            deptid: $("#deptid").val()
        };
        return temp;
    };
    return oTableInit;
};

//部门搜索
$("#detpsearch").click(function () {
    $('#tb_sectionDepartments').bootstrapTable('selectPage', 1);
    DeptTotal();
});

$(function () {
    $.post("/CommissionPay/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
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
            } else {
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

function loaddata(type, obj) {
    addcloud();
    $(".fade").hide();
    $($(obj).find("a").attr("href")).show();
    $("#itype").val(type);
    $("#deptid").val("0");
    LoadTreeDept();
    LoadUser();
}

function LoadUser() {
    var type = $("#itype").val();
    if (type == 0) {//代理商
        $.post("/CommissionPay/GetAgent", { deptid: $("#deptid").val() }, function (data) {
            $("#agentname").empty();
            $("#agentname").append($("<option>").text("全部").val(""));
            if (data != null && data != "") {
                $.each(data, function (index, item) {
                    var opt = $("<option>").text(item.agentname).val(item.agentname);
                    $("#agentname").append(opt);
                });
            }
            $('#agentname').selectpicker("refresh");
            AgentTotal();
            if ($('#tb_agent').html() == "") {
                var oTableAgent = new TableInitAgent();
                oTableAgent.Init();
            } else {
                $('#tb_agent').bootstrapTable('selectPage', 1);
            }
        });
    } else if (type == 1) {//员工
        $.post("/CommissionPay/GetEmplloyee", { deptid: $("#deptid").val() }, function (data) {
            $("#employeename").empty();
            $("#employeename").append($("<option>").text("全部").val(""));
            if (data != null && data != "") {
                $.each(data, function (index, item) {
                    var opt = $("<option>").text(item.truename).val(item.truename);
                    $("#employeename").append(opt);
                });
            }
            $('#employeename').selectpicker("refresh");
             EmployeeTotal();
            if ($('#tb_employeements').html() == "") {
                var oTableEmployee = new TableInitEmployee();
                oTableEmployee.Init();
            } else {
                $('#tb_employeements').bootstrapTable('selectPage', 1);
            }
        });
    } else if (type == 2) {//部门
        $.post("/CommissionPay/DeptPrincipalName", { deptid: $("#deptid").val() }, function (data) {
            $("#DeptPrincipalName").empty();
            $("#DeptPrincipalName").append($("<option>").text("全部").val(""));
            if (data != null && data != "") {
                $.each(data, function (index, item) {
                    var opt = $("<option>").text(item.truename).val(item.truename);
                    $("#DeptPrincipalName").append(opt);
                });
            }
            $('#DeptPrincipalName').selectpicker("refresh");
            DeptTotal();
            if ($('#tb_sectionDepartments').html() == "") {
                var oTableInitDept = new TableInitDept();
                oTableInitDept.Init();
            } else {
                $('#tb_sectionDepartments').bootstrapTable('selectPage', 1);
            }
        });
    }
}



