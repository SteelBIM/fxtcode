var GoodTableInit = function () {
    var oTableInit = new Object();
    oTableInit.Init = function (edit, del, Good_GoodPrice, Good_GoodModuleItem) {
        $('#tb_goodtable').bootstrapTable({
            url: '/Tbx/Good/GetGoodPage',         //请求后台的URL（*）
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
            uniqueId: "GoodID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [
                {
                    field: 'GoodName',
                    title: '策略名称'
                },
                {
                    field: 'ModuleNames',
                    title: '包含模块',
                    formatter: function (value, row, index) {
                        if (value != "" && value != null) {
                           var TedValue = TrimEndChar(value, ',');
                            if (value.length > 20) {
                                return "<span title='" + value + "'>" + TedValue.substr(0, 20) + "..." + "</span>";
                            } else {
                                return "<span title='" + value + "'>" + TedValue + "</span>";
                            }
                        }
                    } 
                }, {
                    field: 'GoodWay',
                    title: '商品出售方式',
                    formatter: function (value, row, index) {
                        if (row.GoodWay == 1) {
                            return "单册";
                        } else if (row.GoodWay == 2) {
                            return "套餐";
                        } else {
                            return "";
                        }
                    }
                }, {
                    field: 'Status',
                    title: '状态',
                    formatter: function (value, row, index) {
                        if (value == 0) {
                            return "未启用";
                        } else if (value == 1) {
                            return "启用";
                        } else {
                            return "禁用";
                        }
                    }
                }, {
                    field: 'Describe',
                    title: '描述'
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var cz = "";
                        if (Good_GoodModuleItem == "True") {
                            //cz += " <a class='btn mini red' title='配置模块' href='/Tbx/GoodModuleItem/Index/" + row.GoodID + "'><i class='icon-file' ></i > 配置模块</a >";
                            cz += " <a class='btn mini mini green' title='配置模块' onclick=\"tb_show('配置模块','/Tbx/GoodModuleItem/Index/" + row.GoodID + "?TB_iframe=true&amp;height=520&amp;width=800',false)\"  ><i class='icon-edit'> </i> 配置模块  </a >";
                        }
                        if (Good_GoodPrice == "True") {
                            cz += " <a class='btn mini green' title='价格管理' href='/Tbx/GoodPrice/Index/" + row.GoodID + "'><i class='icon-file' ></i > 价格管理</a >";
                        }
                        if (edit == "True") {
                            cz += " <a class='btn mini green thickbox' title='编辑' onclick=\"tb_show('编辑','/Tbx/Good/Edit/" + row.GoodID + "?TB_iframe=true&amp;height=520&amp;width=800',false)\"  ><i class='icon-edit'> </i> 编辑   </a >";
                        }
                        if (del == "True") {
                            cz += " <a class='btn mini red' id='mydelete' href='javascript: void (0)' onclick='DelGoodClick(\"" + row.GoodID + "\")'> <i class='icon-trash icon-white'></i> 删除</a>";
                        }
                        return cz;
                    }
                }]
        });
    };
    removecloud();
    oTableInit.queryParams = function (params) {
        var temp = {
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码  
            GoodWay: $("#GoodWay").val(),
            GoodName: $.trim($("#GoodName").val())
        };
        return temp;
    };
    return oTableInit;
};

$("#searchbtn").click(function () {
    $('#tb_goodtable').bootstrapTable('selectPage', 1);
})

function DelGoodClick(GoodID) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("您确定要删除吗~", function (result) {
        if (result) {
            $.post("/Tbx/Good/DelGoodByGoodID", { GoodID: GoodID }, function (data) {
                if (data == 1) {
                    $('#tb_goodtable').bootstrapTable('selectPage', 1);
                }
                else if (data == 0) {
                    bootbox.alert("策略被引用不能删除~");
                }
                else {
                    bootbox.alert("删除失败，请重试~");
                }
            });
        }
    });
}

function RefreshParentGood() {
    $("#TB_closeWindowButton").click();
    $('#tb_goodtable').bootstrapTable('selectPage', 1);
    bootbox.alert("配置成功~");
}