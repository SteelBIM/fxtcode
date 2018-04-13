var oTableInit2 = new Object();
//初始化Table
oTableInit2.Init = function () {
    $('#catalogs').bootstrapTable({
        url: $("#serverUrl2").val(),  //请求后台的URL（*）
        method: 'post',                          //请求方式（*）
        toolbar: '#toolbar',                     //工具按钮用哪个容器
        queryParams: { parentId: 0 },
        striped: true,                           //是否显示行间隔色
        cache: false,                            //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                        //是否显示分页（*）
        sortable: false,                         //是否启用排序
        sortOrder: "asc",                        //排序方式
        //queryParams: oTableInit2.queryParams,     //传递参数（*）
        sidePagination: "server",                //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                           //初始化加载第一页，默认第一页
        pageSize: 10000,                            //每页的记录行数（*）
        pageList: [10000],             //可供选择的每页的行数（*）
        search: false,                           //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
        strictSearch: true,
        showColumns: false,                       //是否显示所有的列
        showRefresh: false,                       //是否显示刷新按钮
        minimumCountColumns: 2,                  //最少允许的列数
        clickToSelect: false,                    //是否启用点击选中行
        height: 600,                           //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "MarketBookCatalogID",                   //每一行的唯一标识，一般为主键列
        showToggle: false,                       //是否显示详细视图和列表视图的切换按钮
        cardView: false,                         //是否显示详细视图
        detailView: true,                        //是否显示父子表
        columns: [{
            field: 'MarketBookCatalogID',
            visible: false
        }, {
            field: 'ParentCatalogID',
            visible: false
        },
        {
            title: '操作',
            field: 'IsShow',
            width: 10,
            formatter: function (value, row, index) {
                var action = "";
                if (value == 1)
                    action += " <a href=\"javascript:;\" cid=" + row.MarketBookCatalogID + "  onclick='ChangeCheckbox(this)'> <input type='checkbox' checked='checked' /></a> ";
                else
                    action += " <a href=\"javascript:;\" cid=" + row.MarketBookCatalogID + "  onclick='ChangeCheckbox(this)'> <input type='checkbox'  /></a> ";
                return action;
            }
        },
        {
            field: 'MarketBookCatalogName',
            title: '目录名称',
            width: 150
        },
        //{
        //    field: 'MODBookCatalogID',
        //    title: 'MOD目录ID',
        //    width: 150
        //},
        {
            field: 'MarketBookCatalogCover',
            title: '目录封面',
            width: 150,
            formatter: function (value, row, index) {
                var action = "";
                if (value != null && $.trim(value) != "")
                    action += "<a style='width:70px' target='_blank' href='" + value + "'>查看</a>";
                action += "&nbsp;&nbsp;<a style='width:70px'  onclick='Edit(" + row.MarketBookCatalogID + ")'>编辑</a>";
                //添加子目录
                action += "&nbsp;&nbsp;<a style='width:70px'  onclick='Edit(" + row.MarketBookCatalogID + ")'>子目录+</a>";
                return action;
            }
        },
            //{
            //    field: 'CreateDate',
            //    title: '创建日期',
            //    width: 150,
            //    formatter: function (value, index) {
            //        if (value)
            //            return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""))).toLocaleDateString();
            //        else
            //            return "-";
            //    }
            //}
        ],
        //注册加载子表的事件。注意下这里的三个参数！
        onExpandRow: function (index, row, $detail, jsonData) {
            oTableInit2.InitSubTable(index, row, $detail, jsonData);
        },
        onLoadSuccess: function (data) {
            SetIconWidthForCatalog();
        },
        onRefresh: function (params) {
            //alert("onRefresh");
        }
    });
};

//得到查询的参数
oTableInit2.queryParams = function (params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
        pagesize: params.limit,   //页面大小
        pageindex: params.offset //页码
    };
    return temp;
};

//初始化子表格
oTableInit2.InitSubTable = function (index, row, $detail) {
    var parentid = row.MarketBookCatalogID;
    var cur_table = $detail.html("<table id='childTable_" + parentid + "' ></table>").find("table");
    $(cur_table).bootstrapTable({
        url: $("#serverUrl3").val(),
        method: 'post',
        queryParams: {
            parentId: parentid
        },
        sidePagination: "server",//注意！必要参数
        clickToSelect: true,
        showHeader: false,
        detailView: true,
        uniqueId: "MarketBookCatalogID",
        pageSize: 10000,
        pageList: [10000],
        columns: [{
            field: 'MarketBookCatalogID',
            visible: false
        }, {
            field: 'ParentCatalogID',
            visible: false
        },
        {
            title: '操作',
            field: 'IsShow',
            width: 10,
            formatter: function (value, row, index) {
                var action = "";
                if (value == 1)
                    action += " <a href=\"javascript:;\" cid=" + row.MarketBookCatalogID + "  onclick='ChangeCheckbox(this)'> <input type='checkbox' checked='checked' /></a> ";
                else
                    action += " <a href=\"javascript:;\" cid=" + row.MarketBookCatalogID + "  onclick='ChangeCheckbox(this)'> <input type='checkbox'  /></a> ";
                return action;
            }
        },
        {
            field: 'MarketBookCatalogName',
            title: '目录名称',
            width: 150
        },
        //{
        //    field: 'MODBookCatalogID',
        //    title: 'MOD目录ID',
        //    width: 150
        //},
        {
            field: 'MarketBookCatalogCover',
            title: '目录封面',
            width: 150,
            formatter: function (value, row, index) {
                var action = "";
                if (value != null && $.trim(value) != "")
                    action += "<a style='width:70px' target='_blank' href='" + value + "'>查看</a>";
                action += "&nbsp;&nbsp;<a style='width:70px'  onclick='Edit(" + row.MarketBookCatalogID + ")'>编辑</a>";
                return action;
            }
        },
            //{
            //    field: 'CreateDate',
            //    title: '创建日期',
            //    width: 150,
            //    formatter: function (value, index) {
            //        if (value)
            //            return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""))).toLocaleDateString();
            //        else
            //            return "-";
            //    }
            //}
        ],
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidthForCatalog();
        },
        //无线循环取子表，直到子表里面没有记录
        onExpandRow: function (index, row, $detail) {
            oTableInit2.InitSubTable(index, row, $detail);
        }
    });
};

function Edit(id) {
    $("#editCatalog").modal();
    var ossfile = $("#ossfile").html();
    if (ossfile.indexOf('成功') != -1)
        $("#ossfile").html("");

    $.post($("#getClassify2").val(), { MarketBookCatalogID: id }, function (data) {
        if (data) {
            $("#MarketBookCatalogID").val(data.MarketBookCatalogID);
            $("#MarketBookCatalogCover").val(data.MarketBookCatalogCover);
        }
    });
}


function SetIconWidthForCatalog() {
    $(".detail-icon").parent().css("width", "10px");
    //$(".bootstrap-table").css("height", "650px");
    //$(".bootstrap-table").css("overflow-y", "scroll");
}

function ChangeCheckbox(obj) {
    var id = $(obj).attr("cid");
    var cb = $(obj).find("input:checkbox");
    if ($(cb).prop("checked")) {
        $.post($("#updateCata").val(), { marketBookCatalogID: id, status: 1 }, function (data) {
            if (data.Success) {
                alert("上架成功");
            }
        });
        $("#childTable_" + id).find("input:checkbox").prop("checked", true);
    } else {
        $.post($("#updateCata").val(), { marketBookCatalogID: id, status: 0 }, function (data) {
            if (data.Success) {
                alert("下架成功");
            }
        });
        $("#childTable_" + id).find("input:checkbox").prop("checked", false);
    }
}





