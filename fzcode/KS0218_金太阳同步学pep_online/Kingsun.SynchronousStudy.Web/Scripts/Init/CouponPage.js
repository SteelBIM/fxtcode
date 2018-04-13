/// <reference path="../../jquery-easyui/jquery.min.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../Management/ApplicationManagement.js" />
/// <reference path="../Management/ServiceManagement.js" />
/// <reference path="../Constant.js" />

var MonInit = function () {
    var Current = this;
    var CurrentPageIndex = 1;
    var CurrentPageSize = 20;
    this.Init = function () {
        $("#mon").datagrid({
            title: '统计',
            nowrap: false,
            border: true,
            collapsible: false, //是否可折叠的  
            rownumbers: true,
            singleSelect: true,
            fitColumns: true,
            loadMsg: '正在加载数据...',
            columns: [[
                { title: '优惠卷ID', field: 'ID', align: 'center', width: 20 },
                {
                    title: '优惠卷状态', field: 'Status', align: 'center', width: 20, formatter: function (value, row, index) {
                        return row.Status == 2 ? "已经使用" : "未使用";
                    }
                },
                //{ title: '类型', field: 'MonitorType', align: 'center', width: 20 },
                { title: '用户手机', field: 'TelePhone', width: 20, align: 'center' },
                { title: '使用版本', field: 'TeachingNaterialName', align: 'center', width: 20 },
                 //{ title: 'Url', field: 'Url', align: 'center', width: 20 },
            ]],
            pagination: true    //分页控件  
        });
        var p = $('#mon').datagrid('getPager');
        p.pagination({
            pageSize: CurrentPageSize, //每页显示的记录条数，默认为10  
            pageList: [10, 15, 20, 25, 30], //可以设置每页记录条数的列表  
            beforePageText: '第', //页数文本框前显示的汉字  
            afterPageText: '页    共 {pages} 页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            onRefresh: function (pageNumber, pageSize) {
                Current.loadData(Current.getWhere(), pageNumber, pageSize);
            },
            onSelectPage: function (pageNumber, pageSize) {
                Current.loadData(Current.getWhere(), pageNumber, pageSize);
            }
        });
        Current.loadData(Current.getWhere(), CurrentPageIndex, CurrentPageSize);

    }

    this.getWhere = function () {
        return "";
    }

    this.loadData = function (WhereCondition, CurrentPageIndex, PageSize) {
        orderManage.QueryCoupon(WhereCondition, CurrentPageIndex, PageSize, function (data) {
            if (data.Data == null || data.Data == undefined) {
                data.Data = { total: 0, rows: [] };
            }
            $("#mon").datagrid("loadData", data.Data);
        });
    }
}