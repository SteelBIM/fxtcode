var mastername = getQueryString("n");
var os_no = getQueryString("os_no");
var type = getQueryString("t");
$(function () {
    if (type != null) {
        //$("#ReturnPreviousLayer").html("<input type=\"button\" onclick=\"location.href='/PaySingle/Index?t=" + type + "'\" class=\"btn btn-default\" value=\" 返回上一层\" />");
        $("#ReturnPreviousLayer").attr("onclick", "location.href='/PaySingle/Index?t=" + type + "'");
    } else {
        //$("#ReturnPreviousLayer").html("<input type=\"button\" onclick=\"location.href='/CommissionSingle/Index?t=0'\" class=\"btn btn-default\" value=\" 返回上一层\" />");
        $("#ReturnPreviousLayer").attr("onclick", "location.href='/CommissionSingle/Index?t=0'");
    }
    $.post("/CommissionSingle/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            if (jsonData.Export)//导出
                $("#export").html("<button  onclick=\"Export()\" style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\"  type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>");
            var oTable = new TableInit();
            oTable.Init();
            var oTablekont = new TableKont();
            oTablekont.Init();
        }
    });
    $.post("/CommissionSingle/GetBillDetailed", { os_no: $.trim(os_no) }, function (data) {
        if (data != null && data != "") {
            var jsondata = eval(data);
            var bonuslist = eval(jsondata.bonuslist);
            $("#KontNumbering").html(bonuslist[0].os_no);
            $("#ordertotal").html(DataToFixed(bonuslist[0].total_bonus + bonuslist[0].adjust_amount));//+ bonuslist[0].adjust_amount
            $("#adjust_amount").html(bonuslist[0].adjust_amount);
            $("#adjust_reason").html(bonuslist[0].adjust_reason);
            var startdate = bonuslist[0].startdate;
            var enddate = bonuslist[0].enddate;
            $("#konttime").html(numbercontime(startdate, "--") + addDate(numbercontime(enddate, ""), -1));
            $("#CurrentDate").html(formatDate(bonuslist[0].createtime, "YYYY-MM-dd"));
            var agentmaster = eval(jsondata.agentmaster);
            if (agentmaster[0].agent_tel != null)
                $("#agent_tel").html(agentmaster[0].agent_tel);
            if (agentmaster[0].agent_fax != null)
                $("#agent_fax").html(agentmaster[0].agent_fax);
            if (agentmaster[0].agent_addr != null)
                $("#address").html(agentmaster[0].agent_addr);
            if (agentmaster[0].mobile != null)
                $("#iphone").html(agentmaster[0].mobile);

            var masterlist = eval(jsondata.masterlist);
            $("#deptnameagent").html(masterlist[0].deptname);

            $("#papername").html(masterlist[0].parentname);
            $("#agenttruename").html(masterlist[0].truename);

            $("#agentname").html(masterlist[0].agentname);
            var operatingmasterlist = eval(jsondata.operatingmasterlist);
            $("#operator").html(operatingmasterlist[0].truename);
        }
    });
});

//导出表格
function Export() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/CommissionSingle/Agent_Export");
    $form.append('<input type="hidden" name="mastername" value="' + mastername + '" />');
    $form.append('<input type="hidden" name="os_no" value="' + os_no + '" />');
    removecloud();
    $(document.body).append($form);
    $form.submit();
    $form.remove();
}

//订单详情
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_ments').bootstrapTable({
            url: '/CommissionSingle/AgentidOrdreInfo_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#Toolbar',                //工具按钮用哪个容器
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
            showRefresh:false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            //uniqueId: "o_id",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'o_id',
                title: '订单编号'
            }, {
                field: 'o_datetime',
                title: '订单日期',
                formatter: function (value) {
                    return formatDate(value, "YYYY-MM-dd");
                }
            }, {
                field: 'path',
                title: '所属地区'
            }, {
                field: 'schoolname',
                title: '学校'
            }, {
                field: 'gradename',
                title: '年级'
            }, {
                field: 'classname',
                title: '班级'
            }, {
                field: 'u_teachername',
                title: '老师姓名'
            } , {
                field: 'channel',
                title: '产品名称',
                formatter: function (value, row, index) {
                    return productname(value);
                }
            }
            , {
                field: 'o_payamount',
                title: '支付金额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_feeamount',
                title: '手续费',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_actamount',
                title: '实际到账',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_bonus',
                title: '提成金额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            mastername: $.trim(mastername),
            os_no: os_no
        };

        return temp;
    };
    return oTableInit;
};
//结算
var TableKont = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_kont').bootstrapTable({
            url: '/CommissionSingle/AgentKontInformationDisplay',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: false,                   //是否显示分页（*）
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
            //uniqueId: "o_id",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'productname',
                title: '产品名称'
            }, {
                field: 'p_category',
                title: '类别'
            }, {
                field: 'p_version',
                title: '版本'
            }, {
                field: 'divided',
                title: '销售折扣',
                formatter: function (value) {
                    if (value != null && value != 0)
                        return value * 100 + "%";
                    return "";
                }
            }, {
                field: 'class_divided',
                title: '班级奖励',
                formatter: function (value) {
                    if (value != null && value != 0)
                        return value * 100 + "%";
                    return "";
                }
            }, {
                field: 'ordernumber',
                title: '总订单数',
                formatter: function (value) {
                    return NullEmtiy(value);
                }
            }, {
                field: 'o_payamount',
                title: '总销售金额',
                formatter: function (value) {
                    return DataToFixed(NullEmtiy(value));
                }
            }, {
                field: 'o_actamount',
                title: '总销售毛利',
                formatter: function (value) {
                    return DataToFixed(NullEmtiy(value));
                }
            }, {
                field: 'classnumber',
                title: '绑定班级订单数',
                formatter: function (value) {
                    return NullEmtiy(value);
                }
            }, {
                field: 'classpayamount',
                title: '绑定班级销售额',
                formatter: function (value) {
                    return DataToFixed(NullEmtiy(value));
                }
            }, {
                field: 'classactamount',
                title: '绑定班级销售毛利',
                formatter: function (value) {
                    return DataToFixed(NullEmtiy(value));
                }
            }, {
                field: 'basis_bonus',
                title: '销售折扣金额',
                formatter: function (value, row, index) {
                    if (row.productname == "调整金额")
                        return "";
                    return DataToFixed(NullEmtiy(value));
                }
            }, {
                field: 'p_class_bonus',
                title: '班级奖励金额',
                formatter: function (value) {
                    return DataToFixed(NullEmtiy(value));
                }
            }, {
                title: '合计',
                formatter: function (value, row, index) {
                    if (row.productname == "总计：" || row.productname == "调整金额")
                        return DataToFixed(row.total);
                    return DataToFixed(row.basis_bonus + row.p_class_bonus);
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            mastername: $.trim(mastername),
            os_no: os_no
        };
        return temp;
    };
    return oTableInit;
};

function NullConvertEmtiy(value) {
    if (value == null)
        return " ";
    return "、" + value;
}
function NullEmtiy(value) {
    if (value == null || value == 0)
        return " ";
    return value;
}