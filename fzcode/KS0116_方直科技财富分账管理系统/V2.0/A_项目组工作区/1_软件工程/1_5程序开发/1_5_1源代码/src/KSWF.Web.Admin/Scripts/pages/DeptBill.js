//员工结算单

var mastername = getQueryString("n");
var os_no = getQueryString("os_no");
var type = getQueryString("t");
$(function () {
    if (type != null) {
        $("#ReturnPreviousLayer").html("<input type=\"button\" onclick=\"location.href='/PaySingle/Index?t=" + type + "'\" class=\"btn btn-default\" value=\" 返回上一层\" />");
        $("#ReturnPreviousLayer").attr("onclick", "location.href='/PaySingle/Index?t=" + type + "'");
    } else {
        $("#ReturnPreviousLayer").html("<input type=\"button\" onclick=\"location.href='/CommissionSingle/Index?t=2'\" class=\"btn btn-default\" value=\" 返回上一层\" />");
        $("#ReturnPreviousLayer").attr("onclick", "location.href='/CommissionSingle/Index?t=2'");
    }

    $.post("/CommissionSingle/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            if (jsonData.Export)//导出
                $("#export").html("<button  onclick=\"Export()\" style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\"  type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>");
            var oTablekont = new TableKont();
            oTablekont.Init();
            var oTable = new TableInit();
            oTable.Init();
        }
    });

    $.post("/CommissionSingle/GetDeptBillDetailed", { os_no: $.trim(os_no) }, function (data) {
        if (data != null && data != "") {
            var jsondata = eval(data);
            var bonuslist = eval(jsondata.bonuslist);
            $("#KontNumbering").html(bonuslist[0].os_no);
            $("#ordertotal").html(DataToFixed(bonuslist[0].total_bonus + bonuslist[0].adjust_amount));//+ bonuslist[0].adjust_amount

            $("#team_bonus_r").html(bonuslist[0].team_bonus_r + "%");

            $("#adjust_amount").html(bonuslist[0].adjust_amount == null ? 0 : bonuslist[0].adjust_amount);
            $("#adjust_reason").html(bonuslist[0].adjust_reason);
            var startdate = bonuslist[0].startdate;
            var enddate = bonuslist[0].enddate;

            $("#konttime").html(numbercontime(startdate, "--") + addDate(numbercontime(enddate, ""),-1));

            $("#CurrentDate").html(formatDate(bonuslist[0].createtime, "YYYY-MM-dd"));

            var masterlist = eval(jsondata.masterlist);


            $("#deptname").html(bonuslist[0].deptname);

            $("#username").html(masterlist[0].truename);

            var operatingmasterlist = eval(jsondata.operatingmasterlist);
            $("#operator").html(operatingmasterlist[0].truename);
        }
    });

});

//导出表格
function Export() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/CommissionSingle/Dept_Export");
    $form.append('<input type="hidden" name="os_no" value="' + os_no + '" />');
    removecloud();
    $(document.body).append($form);
    $form.submit();
    $form.remove();
}

var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_ments').bootstrapTable({
            url: '/CommissionSingle/DepteOrdreInfo_View',         //请求后台的URL（*）
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
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: false,                  //是否显示所有的列
            showRefresh:false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "o_id",                 //每一行的唯一标识，一般为主键列
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
            }, {
                field: 'm_deptname',
                title: '部门'
            }, {
                field: 'm_a_name',
                title: '员工'
            }, {
                field: 'channel',
                title: '产品名称',
                formatter: function (value) {
                    return productname(value);
                }
            }, {
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
            }
            //, {
            //    field: 'o_bonus',
            //    title: '提成金额'
            //}
            ]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
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
            url: '/CommissionSingle/DeptKontInformationDisplay',         //请求后台的URL（*）
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
                formatter: function (value,row,index) {
                    return DataToFixed(NullEmtiy(value));
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            os_no: os_no
        };
        return temp;
    };
    return oTableInit;
};
function NullEmtiy(value) {
    if (value == null || value == 0)
        return " ";
    return value;
}

function NullConvertEmtiy(value) {
    if (value == null)
        return " ";
    return "、" + value;
}