

$(function () {
    $.post("/AuthorityMgr/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            if (jsonData.Add) {
                var dataaction = "<button onclick=\"location.href = '/AuthorityMgr/AuthorityMgr_Add'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\" style=\"background-color:#E16965; color:#fff;border-color:#e16965; margin-right:15px; border-radius:5px;\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>新增角色 </button>"
                $("#toolbar").html(dataaction);
            }
            var oTable = new TableInit();
            oTable.Init(jsonData.Edit, jsonData.Del);
        }
    });
});
var height = $(window).height();

//初始化数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit, del) {
        $('#tb_departments').bootstrapTable({
            url: '/AuthorityMgr/AuthorityMgr_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10000,                       //每页的记录行数（*）
            pageList: [],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: height,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "groupid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'groupid',
                visible: false
            }, {
                field: 'groupname',
                title: '角色名称'
            }, {
                title: '数据查看权限',
                formatter: function (value, row, index) {
                    return DataautHority(row.dataauthority);
                }
            }, {
                field: 'creatername',
                title: '创建人'
            }, {
                field: 'createtime',
                title: '创建时间'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (row.creatername == "系统默认") {
                        action += "<a  href=\"/AuthorityMgr/AuthorityMgr_Add?Id=" + row.groupid + "\"> 查看 <a> "
                    } else {
                        if (edit)
                            action += "<a  href=\"/AuthorityMgr/AuthorityMgr_Add?Id=" + row.groupid + "\"> 编辑 <a> "
                        if (del)
                            action += " <a href=\"javascript:;\"  title=\"点击删除\" onclick='AuthorityMgrDel(" + row.groupid + ")'>删除</a> ";
                    }
                    return action;
                }
            }]
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
    return oTableInit;
};

function AuthorityMgrDel(groupid) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("确定删除~", function (result) {
        if (result) {
            addcloud();
            $.post("/AuthorityMgr/AuthorityMgr_Del", { "groupid": groupid }, function (data) {
                if (data.Success) {
                    removecloud();
                    bootbox.alert("删除成功~");
                    $('#tb_departments').bootstrapTable("refresh");
                } else {
                    removecloud();
                    bootbox.alert(data.ErrorMsg);
                }
            })
        }
    });
}

function DataautHority(number) {//数据查看权限
    if (number == 0) {
        return "全部";
    }
    else if (number == 1) {
        return "本人";
    }
    else if (number == 2) {
        return "本人+下级部门(含本部门)+下级代理商";
    }
    else if (number == 3) {
        return "本人+下级部门(不含本部门)+下级代理商";
    }
    else if (number == 4) {
        return "本人+下级代理商";
    }
}




