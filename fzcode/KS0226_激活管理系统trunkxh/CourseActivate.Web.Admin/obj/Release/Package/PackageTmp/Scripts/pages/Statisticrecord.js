var Statisticrecord = function () {
    //var batchid = getQueryString("batchid");
    var activatecode = getQueryString("activatecode");
    var activateuseid = getQueryString("activateuseid");
    var current = this;
    this.Init = function () {
        $('#tb_Statisticrecord').bootstrapTable({
            url: '/StatisticMgr/Statisticrecord_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: current.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "batchid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                title: '批次号',
                formatter: function (value, row) {
                    var action = activatecode;
                    return action.substring(0, 3);
                }
            }, {
                field: 'activatecode',
                title: '激活码',
                formatter: function (value) {
                    return activatecode;
                }
            }, {
                field: 'devicetype',
                title: '设备类型',
                formatter: function (value, row) {
                    var result = "";
                    if (value == 0) return result = "全部端";
                    if (value == 1) return result = "PC端";
                    if (value == 2) {
                        if (row.isios == null) return result = "移动端";
                        if (row.isios == 1) return result = "IOS端";
                        if (row.isios == 0) return result = "安卓端";
                    }
                }
            }, {
                field: 'devicecode',
                title: '设备ID'
            }, {
                field: 'createtime',
                title: '激活时间',
                formatter: function (value) { return Common.FormatTime(value); }
            }]
        });
    }
    //得到查询的参数
    this.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset,//页码
            activateuseid: activateuseid
        };
        return temp;
    };

    $("#btn_back").click(function () {
        window.history.back();
    })
}