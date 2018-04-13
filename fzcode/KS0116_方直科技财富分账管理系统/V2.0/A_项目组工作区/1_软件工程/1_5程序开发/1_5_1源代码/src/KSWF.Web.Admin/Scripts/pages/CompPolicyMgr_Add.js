var masterId = getQueryString("Id");
var type = getQueryString("t");
$(function () {
    if (type == 1) {
        $(".page-header .span1").html("编辑代理商商务策略");
        $(".page-header .span2").attr("onclick", "location.href = '/AgentPolicyMgr/Index'");
        $("#mastertype").html("代理商");
    } else {
        type = 0
    }
    $.post("/CompPolicyMgr/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            $.post("/Employee/GetMasterById", { "Id": masterId }, function (jsondata) {
                if (jsondata != null && jsondata != "") {
                    $("#MasterTrueName").html(jsondata.truename);
                    $("#MasterName").val(jsondata.mastername);
                    //if (type == 1) {
                    //    $("#MasterName").val(jsondata.agentrname);
                    //}
                    var actionData = eval(data);
                    var oTable = new TableInit();
                    oTable.Init(actionData.Edit, actionData.Del);
                }
            });
        }
    });
    $("#PolicyForm").bootstrapValidator({
        message: '值无效',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            productname: {
                message: '请选择产品',
                validators: {
                    notEmpty: {
                        message: '请选择产品'
                    }
                }
            }, bpolicyprname: {
                message: '请选择策略',
                validators: {
                    notEmpty: {
                        message: '请选择策略'
                    }
                }
            }
        }
    });

});
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit, del) {
        $('#tb_departments').bootstrapTable({
            url: '/CompPolicyMgr/CompPolicyMgrAdd_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
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
            height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "masterid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'id',
                visible: false
            }, {
                field: 'productname',
                title: '产品名称'
            }, {
                field: 'pllicyname',
                title: '商务策略名称'
            }, {
                field: 'startdate',
                title: '生效时间',
                formatter: function (value) {
                    return formatDate(value, "YYYY-MM-dd");
                }
            }, {
                field: 'effectivestatus',
                title: '状态',
                formatter: function (value) {
                    if (value == "1") {
                        return "<span style='color:green;'>已生效<span>";
                    }
                    return "<span style='color:red;'>未生效</span>";
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (edit && row.effectivestatus == "0")
                        action += "<a  href=\"javascript:;\" onclick=\"policy(" + row.id + ")\"> 编辑 <a> "
                    if (del)
                        action += " <a href=\"javascript:;\" onclick=\"delpolicy(" + row.id + ",'" + row.pllicyname + "')\"> 清空策略 <a> "
                    return action;
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            mastername: $("#MasterName").val(),
            selproductname: "",
            status:""

        };
        return temp;
    };
    return oTableInit;
};
$("#searchbtn").click(function () {
    $('#tb_departments').bootstrapTable("refresh");
});

//加载商务策略
function LoadbPolicypr(productid) {
    var ptype = 0;
    if (type == 1) {
        ptype = 1;
    }
    $.post("/CompPolicyMgr/getbpolicyp", { ptype: ptype, productid: productid }, function (data) {
        if (data != null && data != "") {
            $("#bpolicyprname").empty();
            $.each(data, function (index, item) {
                var opt = $("<option>").text(item.pllicyname).val(item.bid);
                $("#bpolicyprname").append(opt);
            });
            var mr = $("<option  selected=selected>").text("选择商务策略").val("");
            $("#bpolicyprname").append(mr);
        }
    });
}
//选择产品改变策略
$("#productname").change(function () {
    var productid = $(this).find("option:selected").val();
    if (productid != "") {
        var ptype = 0;
        if (type == 1) {
            ptype = 1;
        }
        $.post("/CompPolicyMgr/getbpolicyp", { ptype: ptype, productid: productid }, function (data) {
            if (data != null && data != "") {
                $("#bpolicyprname").empty();
                $.each(data, function (index, item) {
                    var opt = $("<option>").text(item.pllicyname).val(item.bid);
                    $("#bpolicyprname").append(opt);
                });
                var mr = $("<option  selected=selected>").text("选择商务策略").val("");
                $("#bpolicyprname").append(mr);
            } else {
                $("#bpolicyprname").empty();
            }
        });
    } else {
        $("#bpolicyprname").empty();
    }
});
//修改新增策略
function policy(policyid) {
    $("#productname").val("");
    $("#effectivedate").val("");
    $("#bpolicyprname").empty();
    if (policyid > 0) {
        $.post("/CompPolicyMgr/getmasterbpolicypbyid", { Id: policyid }, function (data) {
            if (data != null && data != "") {
                var jsonData = eval(data);
                $("#productname").val(jsonData[0].pid);
                $("#bpolicyprname").val(jsonData[0].pllicyname);
                $("#effectivedate").val(formatDate(jsonData[0].startdate, "YYYY-MM-dd"));
                $.post("/CompPolicyMgr/getbpolicyp", { ptype: type, productid: jsonData[0].pid }, function (data) {
                    if (data != null && data != "") {
                        $("#bpolicyprname").empty();
                        $.each(data, function (index, item) {
                            var opt = $("<option>").text(item.pllicyname).val(item.bid);
                            if (jsonData[0].bid == item.bid) {
                                opt = $("<option  selected=selected>").text(item.pllicyname).val(item.bid);
                            }
                            $("#bpolicyprname").append(opt);
                        });
                        $("#setbpolicypr").modal('show');
                    }
                });
            }
        });
    } else {
        $("#setbpolicypr").modal('show');
    }
}
function delpolicy(policyid, pllicyname) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("确定清空吗~", function (result) {
        if (result) {
            $.post("/CompPolicyMgr/CompPolicyMgr_Del", { Id: policyid, pllicyname: pllicyname }, function (data) {
                if (data) {
                    $('#tb_departments').bootstrapTable("refresh");
                }
                else {
                    bootbox.alert("清空失败！请重试~");
                }
            });
        }
    });
}
//确认修改
$("#btn_setbpolicypr").click(function () {
    var bootstrapValidator = $("#PolicyForm").data('bootstrapValidator');
    bootstrapValidator.validate();
    if (!bootstrapValidator.isValid()) {
        return;
    }
    var effectivedate = $("#effectivedate").val();
    if (effectivedate == "") {
        bootbox.alert("请选择生效日期~");
        return false;
    }
    var masterbpolicyprid = $("#policyid").val();
    var bpolicyprid = $("#bpolicyprname").val();
    if (bpolicyprid == "") {
        bootbox.alert("请选择商务策略！");
        return false;
    }
    var product = $("#productname").val();
    if (product == "") {
        bootbox.alert("请选择商务策略！");
        return false;
    }
    $.post("/CompPolicyMgr/CompPolicy_Add", { id: masterbpolicyprid, mastername: $("#MasterName").val(), bid: bpolicyprid, startdate: effectivedate, product: product }, function (data) {
        if (data.Success) {
            $("#setbpolicypr").modal('hide');
            $('#tb_departments').bootstrapTable("refresh");
        } else {
            bootbox.alert(data.ErrorMsg);
        }
    });
});


