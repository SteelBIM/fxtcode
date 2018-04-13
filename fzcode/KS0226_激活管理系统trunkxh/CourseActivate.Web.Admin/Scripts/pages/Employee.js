$(function () {
    $.post("/Employee/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Add)
                dataaction += "<button onclick=\"location.href = '/Employee/Employee_Add'\" id=\"btn_add\" type=\"button\" class=\"btn btn-default\"  style=\"background-color:#E16965; color:#fff;border-color:#e16965; margin-right:15px; border-radius:5px;\" ><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>新建</button>"
            $("#toolbar").html(dataaction);
            var oTable = new TableInit();
            oTable.Init(jsonData.Edit);
        }
    });
    $("#searchbtn").click(function () {
        // $('#tb_departments').bootstrapTable("refresh");
        $('#tb_departments').bootstrapTable('selectPage', 1);
    })
});

//初始化数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (edit) {
        $('#tb_departments').bootstrapTable({
            url: '/Employee/Employee_View',         //请求后台的URL（*）
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
            uniqueId: "masterid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'masterid',
                visible: false
            }, {
                field: 'mastername',
                title: '用户名'
            }, {
                field: 'email',
                title: '邮箱地址'
            }, {
                field: 'mobile',
                title: '手机号'
            }, {
                field: 'issend',
                title: '消息通知',
                formatter: function (value, index) {
                    var result = "";
                    if (value == 0)
                        result = "未启用";
                    if (value == 1)
                        result = "短信";
                    if (value == 2)
                        result = "邮件";
                    if (value == 3)
                        result = "短信、邮件";
                    return result;
                }
            }
            , {
                field: 'remark',
                title: '备注',
                formatter: function (value) {
                    var shortV = value;
                    if (value != null) {
                        if (value.length > 20) {
                            shortV = value.substring(0, 20) + "……";
                        }
                        var html = '<label style="font-weight:normal;" title="' + value + '">' + shortV + '</label>'
                        return html;
                    }
                }
            }
            , {
                field: 'updatetime',
                title: '上一次操作时间'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    var param = "'" + row.masterid + "','" + row.mastername + "'";
                    if (edit)
                        action += "<a  href=\"/Employee/Employee_Add?Id=" + row.masterid + "\"> 编辑 <a> "
                    action += "<a href=\"javascript:void(0)\" onclick=\"deletedata(" + param + ")\"> 删除 <a> "
                    return action;
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码
            deptid: $("#deptid").val(),
            type: 1
        };
        return temp;
    };
    return oTableInit;
};

function deletedata(masterid, mastername) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("是否删除该账号？", function (result) {
        if (result) {
            if (mastername == "admin" || mastername == "Admin") {
                bootbox.alert("admin帐户不能删除！");
                return false;
            } else {
                $.post("/Employee/Employee_Delete", { "masterid": masterid }, function (data) {
                    if (data.Success) {
                        $('#tb_departments').bootstrapTable("refresh");
                    }
                    else {
                        bootbox.alert(data.ErrorMsg);
                    }
                });
            }
        }
    }
  )
}


