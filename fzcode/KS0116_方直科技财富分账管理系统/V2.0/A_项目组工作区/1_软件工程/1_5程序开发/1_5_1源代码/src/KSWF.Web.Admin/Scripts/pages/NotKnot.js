$(function () {

    $.post("/NotKnot/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Agent)
                dataaction += "<li go=\"0\" onclick=\"loaddata(0,this);\" >  <a href=\"#agent\" data-toggle=\"tab\">代理商</a> </li>";
            if (jsonData.Employee)
                dataaction += "<li go=\"1\" onclick=\"loaddata(1,this);\">  <a href=\"#employee\" data-toggle=\"tab\">公司员工</a> </li>";
            if (jsonData.Dept)
                dataaction += "<li go=\"2\" onclick=\"loaddata(2,this);\">  <a href=\"#department\" data-toggle=\"tab\">公司部门</a></li>";
            if (jsonData.Export)//导出
            {
                $("#employeeExport1").append("<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\" onclick=\"employeeexportbtn();\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出</button>");
                $("#deptExport1").append("<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\" onclick=\"departmentexportbtn();\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出</button>");
                $("#agentExport1").append("<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727;\" onclick=\"agentexportbtn();\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>导出</button>");
            }
            $(".nav-pills").html(dataaction);
            var ptype = 0;;
            if (getQueryString("t") != null && getQueryString("t") > 0)
                ptype = getQueryString("t");
            if (ptype == 0) {
                $(".nav-pills li").eq(ptype).attr("class", "active");
                $(".nav-pills li").eq(ptype).click();
            }
            $($(".nav-pills li")).each(function (i, item) {
                if ($(item).attr("go") == ptype) {
                    $(item).attr("class", "active");
                    $(item).click();
                }
            });
        }
    });
});

//加载代理商员工及部门按钮
function loaddata(type, obj) {
    addcloud();
    $(".fade").hide();
    $($(obj).find("a").attr("href")).show();
    if (type == 0) {
        var oTableAgent = new oTableInitAgent();
        oTableAgent.Init();
    } else if (type == 1) {
        var oTableEmployee = new oTableInitEmployee();
        oTableEmployee.Init();
    } else if (type == 2) {
        var oTableDept = new oTableInitDept();
        oTableDept.Init();
    }
    Statistics(type);
}

$(".nav-pills li").click(function () {
    var pannelType = $(this).attr("go");
    if (pannelType == 0) {
        var oTableAgent = new oTableInitAgent();
        oTableAgent.Init();
        //oTableAgent.DatePickerInit();

    } else if (pannelType == 1) {
        var oTableEmployee = new oTableInitEmployee();
        oTableEmployee.Init();
        //oTableEmployee.DatePickerInit();

    } else if (pannelType == 2) {
        var oTableDept = new oTableInitDept();
        oTableDept.Init();
        //oTableDept.DatePickerInit();
    }
    Statistics(pannelType);
});

function Statistics(pannelType) {
    var sTime; var eTime;
    if (pannelType == 0) {
        var sTime = $("#agenthidstartdate").val();
        var eTime = addDate($("#agenthidenddate").val(), 1);
    }
    else if (pannelType == 1) {
        var sTime = $("#employeehidstartdate").val();
        var eTime = addDate($("#employeehidenddate").val(), 1);
    }
    else if (pannelType == 2) {
        var sTime = $("#departmenthidstartdate").val();
        var eTime = addDate($("#departmenthidenddate").val(), 1);
    }
    $.post("/NotKnot/Statistics", { pannelType: pannelType,sTime:sTime,eTime:eTime }, function (data) {
        if (data) {
            if (pannelType == 0) {
                $("#agentTotalPayAmount").html(DataToFixed(data.totalPayAmount));
                $("#agentTotalOrder").html(data.totalOrder);
                $("#agentTotalActAmount").html(DataToFixed(data.totalActAmount));
            } else if (pannelType == 1) {
                $("#employeeTotalPayAmount").html(DataToFixed(data.totalPayAmount));
                $("#employeeTotalOrder").html(data.totalOrder);
                $("#employeeTotalActAmount").html(DataToFixed(data.totalActAmount));
            } else {
                $("#departmentTotalPayAmount").html(DataToFixed(data.totalPayAmount));
                $("#departmentTotalOrder").html(data.totalOrder);
            }
            removecloud();
        }
        else {
            bootbox.alert("加载失败！");
        }
    });
}

//员工
var oTableInitEmployee = function () {

    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_employee').bootstrapTable({
            url: '/NotKnot/GetPageList',      //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#employeeToolbar',        //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,     //传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            showColumns: false,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            uniqueId: "os_no",                   //每一行的唯一标识，一般为主键列
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [{
                field: 'o_id',
                title: '订单编号'
            }, {
                field: 'o_datetime', title: '订单日期',
                formatter: function (value, row, index) {
                    return FormatTime(value, "yyyy-MM-dd hh:mm:ss");
                }
            }, {
                field: 'path',
                title: '省',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        return v[0];
                    }
                }
            }, {
                field: 'path',
                title: '市',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        if (v[0].indexOf('市') != -1) {
                            return v[0];
                        } else {
                            if (v.length > 1)
                                return v[1];
                        }
                    }
                }
            }, {
                field: 'path',
                title: '区/县',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        if (v[0].indexOf('市') != -1) {
                            if (v.length == 2) {
                                return v[1];
                            }
                        }
                        if (v.length > 2) {
                            return v[2];
                        }
                    }
                }
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
                field: 'm_mastertype',
                title: '渠道'
            }, {
                field: 'm_deptname',
                title: '部门'
            }, {
                field: 'm_a_name',
                title: '员工姓名'
            }, {
                field: 'channel',
                title: '产品名称',
                formatter: function (value, row, index) {
                    var v = GetChannelByKey(value);
                    return v ? v.Value : value;
                }
            }, {
                field: 'o_payamount',
                title: '支付金额',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_feeamount',
                title: '手续费',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_actamount',
                title: '实际到账',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_bonus',
                title: '提成金额',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }],
            onLoadSuccess: function (data) {
                //alert("onLoadSuccess");
            },
            onRefresh: function (params) {
                //alert("onRefresh");
            }
        });

    }

    oTableInit.DatePickerInit = function () {
        var picker1 = $('#employeedatetimepicker1').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1,
            linkField: "employeehidstartdate"
        });
        var picker2 = $('#employeedatetimepicker2').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1,
            linkField: "employeehidenddate"
        });
        //动态设置最小值
        picker1.on('changeDate', function (e) {
            var d = new Date();
            picker2.datetimepicker('setStartDate', e.date);
        });
        //动态设置最大值
        picker2.on('changeDate', function (e) {
            picker1.datetimepicker('setEndDate', e.date);
        });
    }

    oTableInit.queryParams = function (params) {
        var temp = {
            pageindex: params.offset,
            pagesize: params.limit,
            pannelType: 1,
            startDate: $('#employeehidstartdate').val(),
            endDate: addDate($("#employeehidenddate").val(), 1)
        };
        return temp;
    }

    return oTableInit;
}

//员工搜索
$("#employeesearchbtn").click(function () {
    $('#tb_employee').bootstrapTable('selectPage', 1);
    Statistics(1);
   
})

//员工导出表格
function employeeexportbtn() {
    var params = { limit: 0, offset: 0 };
    var oTableEmployee = new oTableInitEmployee();
    var obj = oTableEmployee.queryParams(params);
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/NotKnot/ExportOrderXls");
    for (var key in obj) {
        $form.append('<input type="hidden" name="' + key + '" value="' + obj[key] + '" />');
    }
    $(document.body).append($form);
    $form.submit();
    $form.remove();
}

//部门
var oTableInitDept = function () {

    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_department').bootstrapTable({
            url: '/NotKnot/GetPageList',      //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#departmentToolbar',        //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,     //传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            showColumns: false,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            uniqueId: "os_no",                   //每一行的唯一标识，一般为主键列
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [{
                field: 'o_id',
                title: '订单编号'
            }, {
                field: 'o_datetime', title: '订单日期',
                formatter: function (value, row, index) {
                    return FormatTime(value, "yyyy-MM-dd hh:mm:ss");
                }
            }, {
                field: 'path',
                title: '省',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        return v[0];
                    }
                }
            }, {
                field: 'path',
                title: '市',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        if (v[0].indexOf('市') != -1) {
                            return v[0];
                        } else {
                            if (v.length > 1)
                                return v[1];
                        }
                    }
                }
            }, {
                field: 'path',
                title: '区/县',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        if (v[0].indexOf('市') != -1) {
                            if (v.length == 2) {
                                return v[1];
                            }
                        }
                        if (v.length > 2) {
                            return v[2];
                        }
                    }
                }
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
                field: 'm_mastertype',
                title: '渠道'
            }, {
                field: 'm_deptname',
                title: '部门'
            }, {
                field: 'm_a_name',
                title: '员工姓名'
            }, {
                field: 'channel',
                title: '产品名称',
                formatter: function (value, row, index) {
                    var v = GetChannelByKey(value);
                    return v ? v.Value : value;
                }
            }, {
                field: 'o_payamount',
                title: '支付金额',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_feeamount',
                title: '手续费',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_actamount',
                title: '实际到账',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }],
            onLoadSuccess: function (data) {
                //alert("onLoadSuccess");
            },
            onRefresh: function (params) {
                //alert("onRefresh");
            }
        });

    }

    oTableInit.DatePickerInit = function () {
        var picker1 = $('#departmentdatetimepicker1').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1,
            linkField: "departmenthidstartdate"
        });
        var picker2 = $('#departmentdatetimepicker2').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1,
            linkField: "departmenthidenddate"
        });
        //动态设置最小值
        picker1.on('changeDate', function (e) {
            var d = new Date();
            picker2.datetimepicker('setStartDate', e.date);
        });
        //动态设置最大值
        picker2.on('changeDate', function (e) {
            picker1.datetimepicker('setEndDate', e.date);
        });
    }

    oTableInit.queryParams = function (params) {
        var temp = {
            pageindex: params.offset,
            pagesize: params.limit,
            pannelType: 2,
            startDate: $('#departmenthidstartdate').val(),
            endDate: addDate($("#departmenthidenddate").val(), 1)
        };
        return temp;
    }

    return oTableInit;
}

//部门搜索
$("#departmentsearchbtn").click(function () {
    $('#tb_department').bootstrapTable('selectPage', 1);
    Statistics(2);
})

//部门导出表格
function departmentexportbtn() {
    var params = { limit: 0, offset: 0 };
    var oTableDept = new oTableInitDept();
    var obj = oTableDept.queryParams(params);
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/NotKnot/ExportOrderXls");
    for (var key in obj) {
        $form.append('<input type="hidden" name="' + key + '" value="' + obj[key] + '" />');
    }
    $(document.body).append($form);
    $form.submit();
    $form.remove();
}

//代理商
var oTableInitAgent = function () {

    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_agent').bootstrapTable({
            url: '/NotKnot/GetPageList',        //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#agentToolbar',           //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,     //传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            showColumns: false,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            uniqueId: "os_no",                  //每一行的唯一标识，一般为主键列
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [{
                field: 'o_id',
                title: '订单编号'
            }, {
                field: 'o_datetime', title: '订单日期',
                formatter: function (value, row, index) {
                    return FormatTime(value, "yyyy-MM-dd hh:mm:ss");
                }
            }, {
                field: 'path',
                title: '省',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        return v[0];
                    }
                }
            }, {
                field: 'path',
                title: '市',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        if (v[0].indexOf('市') != -1) {
                            return v[0];
                        } else {
                            if (v.length > 1)
                                return v[1];
                        }
                    }
                }
            }, {
                field: 'path',
                title: '区/县',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        var v = $.trim(value).split(' ');
                        if (v[0].indexOf('市') != -1) {
                            if (v.length == 2) {
                                return v[1];
                            }
                        }
                        if (v.length > 2) {
                            return v[2];
                        }
                    }
                }
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
                field: 'm_mastertype',
                title: '渠道'
            }, {
                field: 'm_deptname',
                title: '部门'
            }, {
                field: 'm_a_name',
                title: '代理商名称'
            }, {
                field: 'channel',
                title: '产品名称',
                formatter: function (value, row, index) {
                    var v = GetChannelByKey(value);
                    return v ? v.Value : value;
                }
            }, {
                field: 'o_payamount',
                title: '支付金额',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_feeamount',
                title: '手续费',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_actamount',
                title: '实际到账',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_bonus',
                title: '提成金额',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }],
            onLoadSuccess: function (data) {
                //alert("onLoadSuccess");
            },
            onRefresh: function (params) {
                //alert("onRefresh");
            }
        });

    }

    oTableInit.DatePickerInit = function () {
        var picker1 = $('#agentdatetimepicker1').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1,
            linkField: "agenthidstartdate"
        });
        var picker2 = $('#agentdatetimepicker2').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1,
            linkField: "agenthidenddate"
        });
        //动态设置最小值
        picker1.on('changeDate', function (e) {
            var d = new Date();
            picker2.datetimepicker('setStartDate', e.date);
        });
        //动态设置最大值
        picker2.on('changeDate', function (e) {
            picker1.datetimepicker('setEndDate', e.date);
        });
    }

    oTableInit.queryParams = function (params) {
        var temp = {
            pageindex: params.offset,
            pagesize: params.limit,
            pannelType: 0,
            startDate: $('#agenthidstartdate').val(),
            endDate: addDate($("#agenthidenddate").val(), 1)
        };
        return temp;
    }

    return oTableInit;
}

//代理商搜索
$("#agentsearchbtn").click(function () {
    $('#tb_agent').bootstrapTable('selectPage', 1);
    Statistics(0);
})

//代理商导出表格
function agentexportbtn() {
    var params = { limit: 0, offset: 0 };
    var oTableAgent = new oTableInitAgent();
    var obj = oTableAgent.queryParams(params);
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/NotKnot/ExportOrderXls");
    for (var key in obj) {
        $form.append('<input type="hidden" name="' + key + '" value="' + obj[key] + '" />');
    }
    $(document.body).append($form);
    $form.submit();
    $form.remove();
}

function FormatTime(time, format) {
    if (!time) {
        return "";
    }
    if (format == undefined || format == "") {
        format = "yyyy年MM月dd日 hh:mm:ss";
    }
    var date = new Date(parseInt(time.substring(6, time.length - 2)))
    return date.format(format);
}

Date.prototype.format = function (format) {
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

function GetChannelByKey(key) {
    if (DefaultChannel) {
        for (var i = 0; i < DefaultChannel.length; i++) {
            if (DefaultChannel[i].Key == key) {
                return DefaultChannel[i];
            }
        }
    }
}

