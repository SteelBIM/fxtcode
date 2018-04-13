
$(function () {
    //$.post("/Publish/GetData", function (data) {
    //    if (data != null && data != "") {
    //        var jsonData = eval(data);
    //        var oTable = new TableInit();
    //        oTable.Init(jsonData);
    //    }
    //});

    var oTable = new TableInit();
    oTable.Init();

    $("#btnAdd").click(function () {
        $("#ResetPasswordDiv").modal("show");
    });

    $("#btn_resetpassword").click(function () {
        var obj = { "publishname": $("#txt_publishname").val(), "Remarks": $("#Remark").val() }
        $.post("/Publish/AddPublishInfo", obj, function (data) {
            if (data > 0) {
                bootbox.alert("新增成功~");
                $("#ResetPasswordDiv").modal("hide");
                $('#tb_departments').bootstrapTable("refresh");
            }
        });
    });
});
var height = $(window).height();

//初始化数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_departments').bootstrapTable({
            //data: jsonData,
            url: '/Publish/GetData',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#btnAdd',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 20, 40, 50],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: height,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "publishid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'publishid',
                visible: false
            }, {
                field: 'publishname',
                title: '出版社'
            }, {
                title: '状态',
                formatter: function (value, row) {
                    var action = "";
                    if (row.status == 0) {
                        action += " 未启用";
                    } else if (row.status == 1) {
                        action += " 启用 ";
                    } else if (row.status == 2) {
                        action += " 禁用 ";
                    }
                    return action;
                }
            }, {
                field: 'Remarks',
                title: '备注'
            }, {
                title: '创建时间',
                formatter: function (value, row) {
                    return getLocalTime(row.createTime.replace("/Date(", "").replace(")/", ""));
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (row.status == 1) {
                        action += " <span>删除</span>  ";
                        action += " <a href=\"javascript:;\"  title=\"\" onclick='UpdateStatus(" + row.publishid + ",2)'>禁用</a>  ";

                    } else if (row.status == 2 || row.status == 0) {
                        action += " <a href=\"javascript:;\"  title=\"\" onclick='DeleteStatus(" + row.publishid + ",0)'>删除</a>  ";
                        action += " <a href=\"javascript:;\"  title=\"\" onclick='UpdateStatus(" + row.publishid + ",1)'>启用</a>  ";
                    }
                    return action;
                }
            }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset //页码
        };
        return temp;
    };
    return oTableInit;
};

function getLocalTime(nS) {
    return new Date(parseInt(nS)).toLocaleString().replace(/:\d{1,2}$/, '');
}

function UpdateStatus(publishid, status) {
    bootbox.setDefaults("locale", "zh_CN");
    addcloud();
    $.post("/Publish/UpdateStatus", { "publishid": publishid, "status": status }, function (data) {
        if (data) {
            removecloud();
            bootbox.alert("修改成功！");
            $('#tb_departments').bootstrapTable("refresh");

        } else {
            removecloud();
            bootbox.alert(data);
        }
    });
}

function DeleteStatus(publishid, status) {
    bootbox.setDefaults("locale", "zh_CN");
    addcloud();
    $.post("/Publish/DeleteStatus", { "id": publishid }, function (data) {
        if (data) {
            removecloud();
            bootbox.alert("删除成功！");
            $('#tb_departments').bootstrapTable("refresh");

        } else {
            removecloud();
            bootbox.alert(data);
        }
    });
}
