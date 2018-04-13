//部门核算

//核算

var deptid = getQueryString("id");
var truename = decodeURI(getQueryString("tr"));
var deptname = decodeURI(getQueryString("d"));

$(function () {
    $("#principalname").html(truename);
    $("#deptname").html(deptname);
    $("#username").html(truename);
    $("#userdeptname").html(deptname);

    $.post("/CommissionPay/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Export)//导出
                dataaction += "<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727; margin-right:15px; border-radius:5px;\"  onclick=\"Export()\"   type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出 </button>"
            //if (jsonData.Kont)
            dataaction += "<button onclick= \"Kont()\" style=\"background-color:#F3815D;color:#fff; border-color: #F3815D; border-radius:5px;\"    type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>结算 </button>"
            $("#Toolbar").append(dataaction);
            $.post("/CommissionPay/GetDeptRecentlySettledTime", { deptid: $.trim(deptid) }, function (data) {
                if (data != null && data != "" && data > 0) {
                    $("#startdate").val(addDate(numbercontime(data, ""), -1));
                }
                $("#startdate").attr("disabled", true);
            });
            var oTable = new TableInit();
            oTable.Init();
            deptTotal();
        }
    });
});


var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_ments').bootstrapTable({
            url: '/CommissionPay/DeptOrdreInfo_View',         //请求后台的URL（*）
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
            showRefresh: false,                  //是否显示刷新按钮
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
                field: 'channel',
                title: '产品名称',
                formatter: function (value) {
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
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            index: params.offset,
            size: params.limit,
            deptid: deptid,
            endtime: addDate($("#enddate").val(), 1)
        };
        return temp;
    };
    return oTableInit;
};

function NullConvertEmtiy(value) {
    if (value == null)
        return "";
    return "、" + value;
}
function NullEmtiy(value) {
    if (value != null && value == 0)
        return value;
    return "";
}


function deptTotal() {
    $.post("/CommissionPay/GetDeptOrderTotal", {
        deptid: deptid,
        endtime: addDate($("#enddate").val(), 1)
    }, function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            $("#o_number").html(jsonData.o_number);
            $("#o_payamount").html(jsonData.o_payamount);
        } else {
            $("#ordernumber").html("0");
            $("#amountmoney").html("0");
        }
    });
}


$("#searchbtn").click(function () {
    $('#tb_ments').bootstrapTable('selectPage', 1);
    deptTotal();
});
//导出
function Export() {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/CommissionPay/DeptExport");
    var enddate = addDate($("#enddate").val(), 1);
    $form.append('<input type="hidden" name="m_deptid" value="' + deptid + '" />');
    $form.append('<input type="hidden" name="o_datetime" value="' + enddate + '" />');
    $(document.body).append($form);
    removecloud();
    $form.submit();
    $form.remove();
}

//结算
function Kont() {
    if (truename != null && truename != "") {
        var enddate = $("#enddate").val();
        if (enddate == "") {
            bootbox.setDefaults("locale", "zh_CN");
            bootbox.alert("请选择结算结束日期~", function (result) {
                $("#enddate").focus();
            });
            return false;
        }
        $.post("/CommissionPay/GetDeptOrderTotal", {
            deptid: deptid,
            endtime: addDate($("#enddate").val(), 1)
        }, function (data) {
            if (data != null && data != "") {
                var jsonData = eval(data);
                if (jsonData.o_payamount > 0) {
                    var oTable = new TableKont();
                    oTable.Init();
                    $("#ordertotal").html(jsonData.o_payamount);
                    $("#settlementtotal").val(jsonData.o_payamount);
                    $("#konttime").html($("#startdate").val() + "&nbsp;&nbsp;" + $("#enddate").val());
                    $("#Settlement").modal("show");
                } else {
                    bootbox.alert("该时间段无部门订单，请重试~");
                }
            }
        });
    } else {
        bootbox.alert("找不到部门负责人，请先添加部门负责人~");
    }
}

//结算
var TableKont = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_kont').bootstrapTable({
            url: '/CommissionPay/DeptKontInformationDisplay',         //请求后台的URL（*）
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
                title: '总订单数'
            }, {
                field: 'o_payamount',
                title: '总销售金额',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_actamount',
                title: '总销售毛利',
                formatter: function (value) {
                    return DataToFixed(value);
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            deptid: deptid,
            endtime: addDate($("#enddate").val(), 1)
        };
        return temp;
    };
    return oTableInit;
};
$("#btn_adjustamount").click(function () {
    $("#adjustamountdiv").toggle();
});
//确定结算
$("#btn_settlement").click(function () {
    var ProportionCommission = $("#ProportionCommission").val();//团队提成比例
    if (ProportionCommission != "") {
        if (!checkproportion(ProportionCommission)) {
            bootbox.alert("团队提成比例取整数！");
            return false;
        }
    } else {
        bootbox.alert("团队提成比例不能为空！");
        return false;
    }
    addcloud();
    $.post("/CommissionPay/DeptSetbonus_Add", { os_no: $("#KontNumbering").html(), endtime: addDate($("#enddate").val(), 1), deptid: deptid, deptname: deptname, commission: ProportionCommission }, function (reust) {
        if (reust.Success) {
            removecloud();
            bootbox.setDefaults("locale", "zh_CN");
            bootbox.alert("结算成功~", function (result) {
                location.href = "/CommissionSingle/Index?t=2";
            });
        } else {
            bootbox.alert(reust.ErrorMsg);
        }
    });
});


//验证提成比例(两位正整数) 4.1
function checkproportion(proportion) {
    var regu = /^[1-9][0-9]?$/;
    var re = new RegExp(regu);
    if (re.test(proportion)) {
        return true;
    } else {
        return false;
    }
}

function CalculationResults() {
    var ProportionCommission = $("#ProportionCommission").val();//团队提成比例
    if (ProportionCommission != "") {
        if (!checkproportion(ProportionCommission)) {
            bootbox.alert("团队提成比例取整数！");
            return false;
        }
    } else {
        bootbox.alert("团队提成比例不能为空！");
        return false;
    }
    var bouns = parseInt($("#settlementtotal").val());
    $("#calculationcomment").html("结算金额 ￥"+(ProportionCommission * bouns)/100);
}