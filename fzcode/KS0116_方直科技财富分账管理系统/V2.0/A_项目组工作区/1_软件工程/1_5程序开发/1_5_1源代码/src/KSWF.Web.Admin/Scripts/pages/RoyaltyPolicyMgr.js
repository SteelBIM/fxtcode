
$(function () {
    addcloud();
    $.post("/RoyaltyPolicyMgr/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Add)
                dataaction += "<button style=\"background-color:#E16965; color:#fff;border-color:#E16965;\" onclick=\"Add()\"  type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>新增策略 </button>"
            $("#toolbar").html(dataaction);
            var ptype = getQueryString("t");
            if (ptype == null)
                ptype = 0;
            $(".nav-pills li").eq(ptype).attr("class", "active");
            $("#ptype").val(ptype);
            var oTable = new TableInit();
            oTable.Init(jsonData.Add, jsonData.Edit, jsonData.Del);
            if (ptype == 1) {
                $($thead).each(function (i, item) {
                    if ($(item).attr("data-field") == "divided") {
                        $(item).html("<div class=\"th-inner \"> 销售折扣</div><div class=\"fht-cell\"></div>");
                    }
                });
            }
        }
    });

    //员工代理商策略切换
    $(".nav-pills li").click(function () {
        var policytype = $(this).attr("go");
        $("#ptype").val(policytype);
        var $thead = $("#tb_departments thead tr th");
        if (policytype == 1) {
            $($thead).each(function (i, item) {
                if ($(item).attr("data-field") == "divided") {
                    $(item).html("<div class=\"th-inner \"> 销售折扣</div><div class=\"fht-cell\"></div>");
                }
            });
        } else {
            $($thead).each(function (i, item) {
                if ($(item).attr("data-field") == "divided") {
                    $(item).html("<div class=\"th-inner \"> 基础提成比例</div><div class=\"fht-cell\"></div>");
                }
            });
        }
        $('#tb_departments').bootstrapTable("refresh");
    });
});

//加载数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (add, edit, del) {
        $('#tb_departments').bootstrapTable({
            url: '/RoyaltyPolicyMgr/RoyaltyPolicyMgrt_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
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
            uniqueId: "bid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'bid',
                visible: false
            }, {
                field: 'pllicyname',
                title: '商务策略名称'
            }, {
                field: 'pid',
                title: '产品',
                formatter: function (value) {
                    return productname(value);
                }
            }, {
                field: 'category',
                title: '分类',
                formatter: function (value, row, index) {
                    return ConAttributes(row.category);
                }
            }, {
                field: 'version',
                title: '版本',
                formatter: function (value, row, index) {
                    return ConAttributes(row.version);
                }
            }, {
                field: 'divided',
                title: " 基础提成比例",
                formatter: function (value, row, index) {
                    return ConAttributes(row.divided);
                }
            }, {
                field: 'class_divided',
                title: '班级奖励',
                formatter: function (value, row, index) {
                    return ConAttributes(row.class_divided);
                }
            }
            , {
                field: 'remark',
                title: '备注'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (add)
                        action += " <a  href=\"javascript:;\" onclick=\"CopyPolicy(" + row.bid + ")\"> 复制 <a> "
                    if (edit)
                        action += "<a  href=\"/RoyaltyPolicyMgr/RoyaltyPolicyMgr_Eidt?t=" + $("#ptype").val() + "&Id=" + row.bid + "\"> 编辑 <a> "
                    if (del)
                        action += "<a href=\"javascript:;\" onclick='Delete(" + row.bid + ")'>删除</a>"
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
            pageindex: params.offset,  //页码
            ptype: $("#ptype").val()
            //systemcode: $("#systemcode").val()
        };
        return temp;
    };

    return oTableInit;
};
//分类版本提成比例转换
function ConAttributes(Attributes) {
    if (Attributes == 0 || Attributes == "0" || Attributes == "0,0")
        return "全部";
    var reust = "";
    var array = Attributes.split(',');
    for (var i = 0; i < array.length; i++) {
        if (i == array.length - 1) {
            reust += "<p style='height:20px; line-height:20px; width:100%;'>" + array[i] + "<p>";
        } else {
            reust += "<p style='height:30px; line-height:30px; border-bottom:1px solid #ccc; width:100%;'>" + array[i] + "<p>";
        }
    }
    return reust;
}
//复制
function CopyPolicy(bid) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("确定复制?", function (result) {
        if (result) {
            addcloud();
            $.post("/RoyaltyPolicyMgr/RoyaltyPolicyMgr_Copy", { bid: bid }, function (data) {
                if (data.Success) {
                    removecloud();
                    bootbox.alert("复制成功~");
                    $('#tb_departments').bootstrapTable("refresh");
                } else {
                    removecloud();
                    bootbox.alert(data.ErrorMsg);
                }
            });
        }
    });
}
//删除
function Delete(Id) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("确定删除~", function (result) {
        if (result) {
            $.post("/RoyaltyPolicyMgr/Bpolicypr_Del", { Id: Id }, function (data) {
                if (data.Success) {
                    $('#tb_departments').bootstrapTable("refresh");
                }
                else {
                    bootbox.alert(data.ErrorMsg);
                }
            });
        }
    });
}
//添加策略
function Add() {
    location.href = '/RoyaltyPolicyMgr/RoyaltyPolicyMgr_Add?t=' + $("#ptype").val();
}