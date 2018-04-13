
$(document).ready(function () {
    addcloud();
    var oTable = new UserPayOrderTableInit();
    oTable.Init();
});


var $payway = $("#PayWay option");
var $paystatus = $("#PayStatus option");

//初始化数据
var UserPayOrderTableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit) {
        $('#tb_userpyaorder').bootstrapTable({
            url: '/UserOrder/UserPayOrder/GetUserPayOrderList',         //请求后台的URL（*）
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
            uniqueId: "ID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'UserPayOrderID',
                visible: false
            }, {
                field: 'OrderID',
                title: '订单号'
            }, {
                field: 'AppName',
                title: '应用名称'
            }, {
                field: 'UserName',
                title: '用户名称'
            }, {
                field: 'UserPhone',
                title: '用户手机号'
            }, {
                field: 'PayWay',
                title: '支付方式',
                formatter: function (value) {
                    var text = "";
                    $payway.each(function () {
                        if (value == $(this).val()) {
                            text = $(this).text();
                            return false;
                        }
                    });
                    return text;
                }
            }, {
                field: 'TotalPrice',
                title: '订单总价'
            }, {
                field: 'PreferentialPrice',
                title: '优惠价'
            }, {
                field: 'PayMoney',
                title: '支付金额'
            }, {
                field: 'Status',
                title: '状态',
                formatter: function (value) {
                    var text = "";
                    $paystatus.each(function () {
                        if (value == $(this).val()) {
                            text = $(this).text();
                            return false;
                        }
                    });
                    return text;
                }
            }, {
                field: 'CreateDate',
                title: '创建时间',
                formatter: function (value) {
                    return formatDate(value, "YYYY-MM-dd HH:mm:ss");
                }
            }, {
                field: 'PayDate',
                title: '支付时间',
                formatter: function (value) {
                    return formatDate(value, "YYYY-MM-dd HH:mm:ss");
                }
            }, {
                field: 'GoodNames',
                title: '商品'
            } ]
        });
    };
    removecloud();
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码
            PayStatus: $.trim($("#PayStatus").val()) == "" ? 0 : $.trim($("#PayStatus").val()),
            PayWay: $.trim($("#PayWay").val()) == "" ? 0 : $.trim($("#PayWay").val()),
            AppName: $.trim($("#AppName").val()),
            UserName: $.trim($("#UserName").val()),
            UserPhone: $.trim($("#UserPhone").val()),
            OrderID: $.trim($("#OrderID").val())
        };
        return temp;
    };
    return oTableInit;
};



$("#searchbtn").click(function () {
    $('#tb_userpyaorder').bootstrapTable('selectPage', 1);
});
$("#export").click(function () {
    addcloud();
    var $form = $('<form target="down-file-iframe" method="post" />');
    $form.attr('action', "/UserOrder/UserPayOrder/Export");
    Status = $.trim($("#PayStatus").val()) == "" ? 0 : $.trim($("#PayStatus").val());
    PayWay = $.trim($("#PayWay").val()) == "" ? 0 : $.trim($("#PayWay").val());
    AppName = $.trim($("#AppName").val());
    UserName = $.trim($("#UserName").val());
    UserPhone = $.trim($("#UserPhone").val());
    OrderID = $.trim($("#OrderID").val());

    $form.append('<input type="hidden" name="Status" value="' + Status + '" />');
    $form.append('<input type="hidden" name="PayWay" value="' + PayWay + '" />');
    $form.append('<input type="hidden" name="AppName" value="' + AppName + '" />');
    $form.append('<input type="hidden" name="UserName" value="' + UserName + '" />');
    $form.append('<input type="hidden" name="UserPhone" value="' + UserPhone + '" />');
    $form.append('<input type="hidden" name="OrderID" value="' + OrderID + '" />');
    removecloud();
    $(document.body).append($form);
    $form.submit();
    $form.remove();
});


