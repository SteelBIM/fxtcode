var RevisionHistoryInit = function () {
    var Current = this;
    this.ModuleID = 0;//模块ID
    this.ModuleName = '';
    this.TeachingNaterialName = '';
    this.FirstTitle = '';
    this.FirstTitleID = 0;
    this.SecondTitle = '';
    this.SecondTitleID = 0;
    this.BookID = 0;

    this.Init = function () {
        Current.BookID = parseInt(Common.QueryString.GetValue("BookID"));
        Current.ModuleID = parseInt(Common.QueryString.GetValue("ModularID"));
        Current.FirstTitleID = parseInt(Common.QueryString.GetValue("FirstTitleID"));
        Current.SecondTitleID = parseInt(Common.QueryString.GetValue("SecondTitleID"));
        Current.InitModuleInfo();
    };

    //加载App信息列表
    this.InitModuleInfo = function () {
        var queryStr = " 1=1 ";
        queryStr += " and  BooKID =" + Current.BookID;
        queryStr += " and  ModuleID =" + Current.ModuleID;
        queryStr += " and FirstTitleID =" + Current.FirstTitleID;
        if (Current.SecondTitleID != 0) {
            queryStr += " and SecondTitleID =" + Current.SecondTitleID;
        }
        queryStr += " ORDER BY CreateDate DESC ";
        $('#tbdatagrid').datagrid({
            url: "?action=querymodulelist&queryStr=" + encodeURI(queryStr),
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 55,
            columns: [[
                { field: 'ModuleName', title: '模块名称', align: 'center', width: 15 },
                { field: 'ModuleAddress', title: '资源包地址', align: 'center', width: 30 },
                { field: 'MD5', title: 'MD5值', align: 'center', width: 30 },
                { field: 'ModuleVersion', title: '模块版本', align: 'center', width: 15 },
                 {
                     field: "State", title: "状态", width: 15, align: 'center', formatter: function (value, rows) {
                         var html = '';
                         html += ' <span>' + (rows.State == true ? '启用' : '禁用') + '</a>';
                         return html;
                     }
                 },
               {
                   field: "Operate", title: "操作", width: 15, align: 'center', formatter: function (value, rows) {
                       var html = '';
                       html += ' <a href="javascript:void(0)" onclick="revisionHistoryInit.ChangeState(\'' + rows.ID + '\',\'' + rows.State + '\')">' + (rows.State == true ? '禁用' : '启用') + '</a>';
                       return html;
                   }
               }
            ]]
        });
    }

    //修改App版本状态
    this.ChangeState = function (moduleID, State) {
        var msg = (State == "true" ? "禁用" : "启用");
        if (confirm("确定" + msg + "吗？")) {
            var obj = { ModuleID: moduleID, State: State };
            $.post("?action=changestate", obj, function (data) {
                if (data) {
                    $('#tbdatagrid ').datagrid("reload");
                } else {
                    alert("修改失败!");
                }
            });
        }
    }

}

var revisionHistoryInit;
$(function () {
    revisionHistoryInit = new RevisionHistoryInit();
    revisionHistoryInit.Init();
});