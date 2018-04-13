//初始化数据
var GoodPriceTableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_goodpricetable').bootstrapTable({
            url: '/Tbx/GoodPrice/GetGoodPricePage',         //请求后台的URL（*）
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
            uniqueId: "GoodPriceID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [
                {
                    field: 'GoodsBpolicyMonths',
                    title: '使用时长(月)'
                }, {
                    field: 'GoodsOriginalPrice',
                    title: '原格'
                }, {
                    field: 'AndroidPrice',
                    title: '安卓价格'
                }, {
                    field: 'IOSCommodityID',
                    title: 'IOS商品ID'
                }, {
                    field: 'IOSPrice',
                    title: 'IOS价格'
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var cz = "";
                        cz += "<a class='btn mini green thickbox' title='编辑' onclick=\"tb_show('编辑','/Tbx/GoodPrice/Edit/" + row.GoodPriceID + "?TB_iframe=true&amp;height=520&amp;width=800',false)\"  ><i class='icon-edit'> </i> 编辑  </a >";

                        cz += "   <a class='btn mini drak' id='mydelete' href='javascript: void (0)' onclick='DelGoodPriceClick(\"" + row.GoodPriceID + "\")'> <i class='icon- trash icon- white'></i> 删除</a>";
                        return cz;
                    }
                }]
        });
    };
    removecloud();
    oTableInit.queryParams = function (params) {
        var temp = {
            pagesize: params.limit,   //页面大小
            pageindex: params.offset //页码   
        };
        return temp;
    };
    return oTableInit;
};



function DelGoodPriceClick(GoodPriceID) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("您确定要删除吗~", function (result) {
        if (result) {
            $.post("/Tbx/GoodPrice/DelGoodPriceByGoodPriceID", { GoodPriceID: GoodPriceID }, function (data) {
                if (data) {
                    $('#tb_goodpricetable').bootstrapTable('selectPage', 1);
                }
                else {
                    bootbox.alert("删除失败，请重试~");
                }
            });
        }
    });
}