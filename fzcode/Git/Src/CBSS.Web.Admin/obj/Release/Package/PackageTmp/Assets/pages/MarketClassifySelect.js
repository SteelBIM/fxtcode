var oTableInit = new Object();
//初始化Table
oTableInit.Init = function () {
    $('#tb_classifies').bootstrapTable({
        url: $("#serverUrl").val(),  //请求后台的URL（*）
        method: 'post',                          //请求方式（*）
        toolbar: '#toolbar',                     //工具按钮用哪个容器
        queryParams: { parentId: 0 },
        striped: true,                           //是否显示行间隔色
        cache: false,                            //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                        //是否显示分页（*）
        sortable: false,                         //是否启用排序
        sortOrder: "asc",                        //排序方式
        //queryParams: oTableInit.queryParams,     //传递参数（*）
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
        //height: 500,                           //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "MarketClassifyID",                   //每一行的唯一标识，一般为主键列
        showToggle: false,                       //是否显示详细视图和列表视图的切换按钮
        cardView: false,                         //是否显示详细视图
        detailView: true,                        //是否显示父子表
        columns: [{
            field: 'MarketClassifyID',
            visible: false
        }, {
            field: 'ParentId',
            visible: false
        }, {
            field: 'MarketClassifyName',
            title: '分类名称',
            width: 150
        },
        {
            title: '操作',
            formatter: function (value, row, index) {
                var action = "";
                action += " <a href=\"javascript:;\"  MarketClassifyID=" + row.MarketClassifyID + "   MarketClassifyName='" + row.MarketClassifyName + "'  onclick='Select(this)'>添加</a> ";

                return action;
            }
        }
        ],
        //注册加载子表的事件。注意下这里的三个参数！
        onExpandRow: function (index, row, $detail, jsonData) {
            oTableInit.InitSubTable(index, row, $detail, jsonData);
        },
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidth();
        },
        onRefresh: function (params) {
            //alert("onRefresh");
        }
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

//初始化子表格
oTableInit.InitSubTable = function (index, row, $detail) {
    var parentid = row.MarketClassifyID;
    var cur_table = $detail.html("<table id='childTable_" + parentid + "' ></table>").find("table");
    $(cur_table).bootstrapTable({
        url: $("#serverUrl").val(),
        method: 'post',
        queryParams: {
            parentId: parentid
        },
        sidePagination: "server",//注意！必要参数
        clickToSelect: true,
        showHeader: false,
        detailView: true,
        uniqueId: "MarketClassifyID",
        pageSize: 10000,
        pageList: [10000,10000],
        columns: [{
            field: 'MarketClassifyID',
            visible: false
        }, {
            field: 'ParentId',
            visible: false
        },
        {
            field: 'MarketClassifyName',
            title: '分类名称',
            width: 150
        },

        {
            title: '操作',
            formatter: function (value, row, index) {
                var action = "";
                action += " <a href=\"javascript:;\"  MarketClassifyID=" + row.MarketClassifyID + "   MarketClassifyName='" + row.MarketClassifyName + "'  onclick='Select(this)'>添加</a> ";
                return action;
            }
        }
        ],
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidth();
        },
        //无线循环取子表，直到子表里面没有记录
        onExpandRow: function (index, row, $detail) {
            oTableInit.InitSubTable(index, row, $detail);
        }
    });
};

function Edit(id) {
    $("#editModal").modal();
    $.post($("#getClassify").val(), { marketClassifyID: id }, function (data) {
        if (data) {
            $("#MarketClassifyID").val(data.MarketClassifyID);
            $("#ParentId").val(data.ParentId);
            $("#MarketClassifyName").val(data.MarketClassifyName);
            $("#MarketClassifyProperty").val(data.MarketClassifyProperty);
            $("#MarketID").val(data.MarketID);
            $("#MODType").val(data.MODType);
            $("#MODID").val(data.MODID);
        }
    });
}

function Select(obj) {
    $("#MarketClassifyId").val($(obj).attr("MarketClassifyID"));
    //$("#MarketBookName").val($(obj).attr("MarketClassifyName"));
    $("#MarketClassifyName").val($(obj).attr("MarketClassifyName"));
    $.post($("#getFullName").val(), { id: $(obj).attr("MarketClassifyID") }, function (data) {
        $("#MarketClassifyName").val(data);
    });
    $("#editModal").modal("hide");
   
    GetBookList();
}

//添加顶级分类
function AddParent() {
    $("#editModal").modal();
}

function Del(id) {
    $.post($("#delUrl").val(), { id: id }, function (data) {
        if (data.Success) {
            bootbox.alert("删除成功！");
            $("#tb_classifies").bootstrapTable("refresh");
        } else {
            bootbox.alert(data.ErrorMsg);
        }
    });
}

function SetIconWidth() { 
    $(".detail-icon").parent().css("width", "10px");
   $( $(".bootstrap-table")[0]).css("height", "550px");
   $( $(".bootstrap-table")[0]).css("overflow-y", "scroll");
}

function submitForm() {
    $("form").submit();
}