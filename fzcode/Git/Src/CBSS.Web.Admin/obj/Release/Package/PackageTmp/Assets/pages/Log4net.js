//初始化数据
var log4netTableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit) {
        $('#tb_roletable').bootstrapTable({
            url: '/Cfgmanager/Log4net/GetLog4netPage',         //请求后台的URL（*）
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
            uniqueId: "Id",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [
                {
                    checkbox: true
                }, {
                    field: 'Id',
                    visible: false
                }, {
                    field: 'Level',
                    title: '消息类型'
                }, {
                    field: 'Logger',
                    title: '日志类型'
                }, {
                    field: 'Date',
                    title: '时间',
                    formatter: function (value, row, index) {
                        return formatDate(row.Date, "YYYY-MM-dd HH:mm:ss");
                    }
                }, {
                    field: 'Message',
                    title: 'Message',
                    formatter: function (value, row, index) {
                        if (row.Message != "" && row.Message != null) {
                            if (row.Message.length > 20) {
                                return "<span title='" + row.Message.replace(/'/g, '"') + "'>" + row.Message.substr(0, 20).replace(/'/g, '"') + "..." + "</span>";
                            } else {
                                return "<span title='" + row.Message.replace(/'/g, '"') + "'>" + row.Message.replace(/'/g, '"') + "</span>";
                            }
                        }
                    }
                }, {
                    field: 'Exception',
                    title: 'Exception',
                    formatter: function (value, row, index) {
                        if (row.Exception != "" && row.Exception != null) {
                            if (row.Exception.length > 20) {
                                return "<span title='" + row.Exception.replace(/'/g, '"') + "'>" + row.Exception.substr(0, 20).replace(/'/g, '"') + "..." + "</span>";
                            } else {
                                return "<span title='" + row.Exception.replace(/'/g, '"') + "'>" + row.Exception.replace(/'/g, '"') + "</span>";
                            }
                        }
                    }
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var cz = "";
                        if (edit == "True") {
                            cz += "<a class='btn mini green thickbox' title='查看' onclick=\"tb_show('查看','/Cfgmanager/Log4net/Edit/" + row.Id + "?TB_iframe=true&amp;height=800&amp;width=1000',false)\"  ><i class='icon-edit'> </i> 查看  </a > ";
                        }
                        return cz;
                    }
                }]
        });
    };
    removecloud();
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码  
            Level: $("#Level").val(),
            Logger: $("#Logger").find("option:selected").text()
        };
        return temp;
    };
    return oTableInit;
};
$("#searchbtn").click(function () {
    $('#tb_roletable').bootstrapTable('selectPage', 1);
})
//多选删除
$("#deletes").click(function () {
    var selects = $('#tb_roletable').bootstrapTable('getSelections');
    var ids = '';
    for (var i = 0; i < selects.length; i++) {
        if (ids) {
            ids += "," + selects[i].Id;
        } else {
            ids += selects[i].Id;
        }
    }
    if (ids != "") {
        bootbox.setDefaults("locale", "zh_CN");
        bootbox.confirm("确定删除~", function (result) {
            if (result) {
                $.post("/Cfgmanager/Log4net/Delete", { ids: ids }, function (data) {
                    if (data) {
                        //$('#tb_roletable').bootstrapTable('selectPage', 1);
                        window.location.reload();
                    }
                    else {
                        bootbox.alert("删除失败，请重试~");
                    }
                });
            }
        });
    } else {
        bootbox.alert("请至少选择一项~");
    }
});