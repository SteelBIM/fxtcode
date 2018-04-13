var isEdit = "";
var editionID = "";
var AppVersions = function () {
    var current = this;
    this.Init = function () {
        $('#AppVersions').bootstrapTable({
            url: '/AppEditions/GetAppVersions',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: false,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: current.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10000,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 10000],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "APPVersionID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'Version',
                title: '版本号'
            }, {
                field: 'Remark',
                title: '描述'
            },
             {
                 field: 'Status',
                 title: '状态',
                 formatter: function (value) {
                     if (value == "0") return "未启用";
                   else  if (value == "1") return "启用";
                   else if (value == "2") return "禁用";
                   else
                       return "未启用"
                 }
             },
            {
                field: 'CreateDate',
                title: '发布时间',
                formatter: function (value) { return Common.FormatTime(value); }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += "<a href=\"javascript:void(0)\" onclick=\"EditVersion(" + row.APPVersionID + ")\"> 修改 <a> "
                    action += "<a href=\"javascript:void(0)\" onclick=\"DeleteVersion(" + row.APPVersionID + ")\"> 删除 <a> "
                    return action;
                }
            }]
        });
    }

   
    //得到查询的参数
    this.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码
            appId:$("#appId").val()
        };
        return temp;
    };
    $("#btn_Add").click(function () {
        isEdit = 0;
        $("#ResourceType").val("");
        $("#Remark").val("");
        $("#setEdition").modal('show');
    });
}

