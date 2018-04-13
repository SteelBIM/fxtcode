var ResourceMgrFiles = function () {
    var Current = this;
    //初始化数据
    this.Init = function () {
        $('#tb_activatetype').bootstrapTable({
            url: '/ResourceMgrFiles/GetList',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: Current.queryParams,//传递参数（*）
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
            uniqueId: "ResID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [
                {
                    field: 'UpdateTime',
                    title: '新建时间',
                    formatter: function (value, index) {
                        if (value)
                            return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""))).toLocaleDateString();
                        else
                            return "";
                    }
                },
                {
                    field: 'ResName',
                    title: '资源名称'
                }, {
                    field: 'ResID',
                    title: '资源ID'
                }, {
                    field: 'UpdateTime',
                    title: '更新时间',
                    formatter: function (value, index) {
                        if (value)
                            return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""))).toLocaleDateString();
                        else
                            return "";
                    }
                },
                 {
                     field: 'ResVersion',
                     title: '资源版本号',
                 },
                 {
                     field: 'ModularName',
                     title: '资源类型',
                     formatter: function (value, row, index) {
                         return row.ModularID + "." + row.ModularName
                     }
                 },
                 {
                     field: 'Status',
                     title: '状态',
                     formatter: function (value, index) {
                         var result = "";
                         if (value == 0) {
                             result = "未启用";
                         }
                         if (value == 1) {
                             result = "已启用";
                         }
                         if (value == 2) {
                             result = "禁用";
                         }
                         return result;
                     }
                 },
                {
                    field: 'remark',
                    title: '备注',
                },
            {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "<a href='javascript:' onclick=\"ModifyRes(" + row.ResID + ")\"> 修改 <a> ";
                    var param = row.ResID + "," + row.Status;
                    action += "<a href='javascript:' onclick=\"deleteActivate(" + row.ResID + ")\"> 删除 <a> "
                    if (row.Status == 0 || row.Status == 2)
                        action += "<a href='javascript:' onclick=\"updateStatus(" + param + ")\"> 启用 <a> "
                    if (row.Status == 1)
                        action += "<a href='javascript:' onclick=\"updateStatus(" + param + ")\"> 禁用 <a> "
                    return action;
                }
            }]
        });
        getPublish();
    }
    //得到查询的参数
    this.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            //pagesize: params.limit,   //页面大小            
            //pageindex: params.offset, //页码
            //batchcode: $("#batchcode").val() || "",
            bookid: $("#bookid").val() || -1,
            //publishid: $("#publishid").val() || -1,
            //type: $("#type").val() || -1,
            //state: $("#state").val() || -1,
        };
        return temp;
    };
}
function getPublish() {
    $.post("/ActivateTypeMgr/GetPublish", function (data) {
        if (data.Success) {
            if (data.Data) {
                for (var i = 0; i < data.Data.length; i++) {
                    var result = data.Data[i];
                    var html = '';
                    html += ' <option value="' + result.publishid + '">' + result.publishname + '</option>';
                    $(html).appendTo("#publishid");
                }
            }
        }
    })
}


function updateStatus(resid, status) {
    $.post("/ResourceMgrFiles/UpdateState", { "resid": resid, "status": status }, function (data) {
        if (data.Success) {
            $('#tb_activatetype').bootstrapTable("refresh");
            //window.location.reload();
        }
    })
}

function deleteActivate(resid) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("确定删除？", function (result) {
        if (result) {
            $.post("/ResourceMgrFiles/ResDelete", { "resid": resid }, function (data) {
                if (data.Success) {
                    $('#tb_activatetype').bootstrapTable("refresh");
                }
                else {
                    bootbox.alert(data.ErrorMsg);
                }
            });
        }
    }
  )
}
function add() {
    $("#setbpolicypr").modal('show');
    getPublish();
}

function saveData() {
    var CodeType = $("#codeType").val();
    var type = $("#type").val();
    var device = $("#device").val();
    var remark = $("#Remark").val();
}

function formatDate(now) {
    var year = now.getYear();
    var month = now.getMonth() + 1;
    var date = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    return year + "-" + month + "-" + date + "   " + hour + ":" + minute + ":" + second;
}



