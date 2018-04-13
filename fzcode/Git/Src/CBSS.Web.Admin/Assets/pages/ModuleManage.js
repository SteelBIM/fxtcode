//初始化数据
var ModuleTableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit, del) {
        $('#tb_booktable').bootstrapTable({
            url: '/Tbx/Module/GetModuleManageJsonPage',         //请求后台的URL（*）
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
            uniqueId: "ModuleID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                checkbox: true,
                visible: false
            }, {
                field: 'ModuleID',
                visible: false
            }, {
                field: 'ModuleName',
                title: '模块名称'
            }, {
                field: 'ParentModuleName',
                title: '上级模块'
            }, {
                field: 'ModelName',
                title: '模型名称'
            }, {
                    field: 'SourceAccessMode',
                title: '资源访问方式',
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "MOD";
                    } else if (value == 2) {
                        return "上传";
                    } else if (value == 3) {
                        return "第三方";
                    }
                    return value;
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
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (row.ParentModuleName == "") {
                        action += "<a class='btn mini green thickbox' title='上传资源' onclick=\"tb_show('上传资源','/Tbx/Module/Resources/" + row.ModuleID + "?TB_iframe=true&amp;height=520&amp;width=600',false)\"  ><i class='icon-edit'> </i> 上传资源  </a > ";
                    }
                    if (edit == "True") {
                        action += "<a class='btn mini green thickbox' title='编辑' onclick=\"tb_show('编辑','/Tbx/Module/Edit/" + row.ModuleID + "?MarketBookID=" + row.MarketBookID + "&TB_iframe=true&amp;height=520&amp;width=600',false)\"  ><i class='icon-edit'> </i> 编辑  </a > ";
                    } if (del == "True") {
                        action += "<a class='btn mini red' id='mydelete' href='javascript: void (0)' onclick='DelModuleByModuleID(" + row.ModuleID + "," + row.MarketBookID + ")'><i class='icon- trash icon- white'></i> 删除 </a>";
                    }
                    return action;
                }
            }]
        });
    };
    removecloud();
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset //页码 
        };
        return temp;
    };
    return oTableInit;
};