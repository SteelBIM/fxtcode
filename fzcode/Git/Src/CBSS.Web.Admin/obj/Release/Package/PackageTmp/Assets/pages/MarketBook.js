var marketTableInit = new Object();
//初始化Table
marketTableInit.Init = function () {
    $('#tb_marketbook_select').bootstrapTable({
        url: $("#serverUrl").val(),  //请求后台的URL（*）
        method: 'post',                          //请求方式（*）
        //toolbar: '#toolbar',                     //工具按钮用哪个容器
        queryParams: { parentId: 0 },
        striped: true,                           //是否显示行间隔色
        cache: false,                            //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: false,                        //是否显示分页（*）
        sortable: false,                         //是否启用排序
        sortOrder: "asc",                        //排序方式
        //queryParams: marketTableInit.queryParams,     //传递参数（*）
        sidePagination: "server",                //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                           //初始化加载第一页，默认第一页
        pageSize: 10000,                            //每页的记录行数（*）
        pageList: [10000],             //可供选择的每页的行数（*）
        search: false,                           //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
        strictSearch: true,
        showColumns: false,                       //是否显示所有的列
        showHeader: false,                        //是否显示列标题
        showRefresh: false,                       //是否显示刷新按钮
        minimumCountColumns: 2,                  //最少允许的列数
        clickToSelect: false,                    //是否启用点击选中行
        //height:1000,                           //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
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
            title: '分类名称'
        }],
        //注册加载子表的事件。注意下这里的三个参数！
        onExpandRow: function (index, row, $detail, jsonData) {
            marketTableInit.InitSubTable(index, row, $detail, jsonData);
        },
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidth();
        },
        onRefresh: function (params) {
            //alert("onRefresh");
        },
        ////点击操作
        onClickRow: function (row, tr) {
            var MarketBookName = "";
            var MarketClassifyID = row.MarketClassifyID;
            var MarketClassifyName = row.MarketClassifyName;
            SubmitSelect(MarketBookName, MarketClassifyID, MarketClassifyName);
        }
    });
};

//得到查询的参数
marketTableInit.queryParams = function (params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
        pagesize: params.limit,   //页面大小
        pageindex: params.offset //页码
    };
    return temp;
};

//初始化子表格
marketTableInit.InitSubTable = function (index, row, $detail) {
    var parentid = row.MarketClassifyID;
    var cur_table = $detail.html("<table id='child_" + parentid + "' ></table>").find("table");
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
        pageList: [10000, 10000],
        columns: [{
            field: 'MarketClassifyID',
            visible: false
        }, {
            field: 'ParentId',
            visible: false
        },
        {
            field: 'MarketClassifyName',
            title: '分类名称'
        }
        ],
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidth();
        },
        //无线循环取子表，直到子表里面没有记录
        onExpandRow: function (index, row, $detail) {
            marketTableInit.InitSubTable(index, row, $detail);
        },
        ////点击操作
        onClickRow: function (row, tr) {
            var MarketBookName = "";
            var MarketClassifyID = row.MarketClassifyID;
            var MarketClassifyName = row.MarketClassifyName;
            SubmitSelect(MarketBookName, MarketClassifyID, MarketClassifyName);
        }
    });
};
function SetIconWidth() {
    $(".detail-icon").parent().css("width", "10px");
    $($(".bootstrap-table")[0]).css("height", "550px");
    $($(".bootstrap-table")[0]).css("overflow-y", "hidden");
}
function submitForm() {
    $("form").submit();
}





//初始化数据
var MarketBookTableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit, del, MarketBook_Catalogs) {
        $('#tb_marketbook').bootstrapTable({
            url: '/Tbx/MarketBook/GetMarketBookPage',         //请求后台的URL（*）
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
            uniqueId: "MarketBookID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'MarketBookID',
                visible: false
            }, {
                field: 'MarketBookName',
                title: '书籍名称',
                formatter: function (value, row, index) {
                    if (value) {
                        return row.MarketClassifyName + value;
                    } else {
                        return row.MODBookName;
                    }
                }
            }, {
               field: 'ISBN',
                title: '书籍编号'
            },
            {
                field: 'MODBookName',
                title: 'MOD书籍'
            },
            {
                field: 'MarketClassifyName',
                title: '书籍类别'
            },
            {
                field: 'MarketBookCover',
                title: '书籍封面',
                formatter: function (value) {
                    if (value != null && value != "")
                        return "<img src='" + value + "' style=\"height:50px;\" >";
                    return "";
                }
            },
            {
                field: 'MODBookCover',
                title: 'MOD书籍封面',
                formatter: function (value) {
                    if (value != null && value != "")
                        return "<img src='" + value + "' style=\"height:50px;\" >";
                    return "";
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var html = "";
                    if (MarketBook_Catalogs=="True") {
                        html += " <a class=\"btn mini green\" href=\"javascript:Catalogs(" + row.MarketBookID + ")\"><i class=\"icon-search\"></i> 目录</a> "
                    } 
                    if (edit=="True")
                        html += "<a class='btn mini green thickbox' title='编辑' onclick=\"tb_show('编辑','/Tbx/MarketBook/Edit/" + row.MarketBookID + "?TB_iframe=true&amp;height=720&amp;width=800',false)\"  ><i class='icon-edit'> </i> 编辑  </a > ";

                    if (del == "True")
                        html += "<a class=\"btn mini red\" href=\"javascript:;\" onclick=\"DelBook(" + row.MarketBookID + ")\"><i class=\"icon-trash icon-white\"></i> 删除</a> ";
                 
                    return html;
                }
            }]
        });
    };

    ///Tbx/MarketBook/Edit/64?TB_iframe=true&height=720&width=800
    removecloud();
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码
            MarketClassifyId: $("#MarketClassifyId").val(),
            MarketBookName: ""
        };
        return temp;
    };
    return oTableInit;
};


$("#searchbtn").click(function () {
    $('#tb_marketbook').bootstrapTable('selectPage', 1);
})


//加载树形菜单
function LoadTreeMarketClassify() {
    $.ajax({
        type: "Post",
        url: "/Tbx/Module/GetMarketClassify",
        dataType: "json",
        success: function (result) {
            $('#tree').treeview({
                data: result,
                showIcon: false,
                onNodeExpanded: function (event, node) {
                },
                onNodeSelected: function (event, node) {
                    //itemOnclick(node.tag);
                }
            });
        }
    });
}

//点击事件
function itemOnclick(id) {
    $("#MarketClassifyId").val(id);
    $('#tb_marketbook').bootstrapTable('selectPage', 1);
}
