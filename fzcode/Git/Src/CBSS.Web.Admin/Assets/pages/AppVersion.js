//初始化数据
var AppVersionTableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_roletable').bootstrapTable({
            url: '/Tbx/AppVersion/GetAppVersionPage',         //请求后台的URL（*）
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
            uniqueId: "AppVersionID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [
                {
                    field: 'AppVersionNumber',
                    title: '版本号',
                    formatter: function (value, row, index) {
                        return "V" + value;
                    }
                }, {
                    field: 'AppType',
                    title: '类别',
                    formatter: function (value, row, index) {
                        if (value == 1) {
                            return "安卓";
                        } else {
                            return "苹果";
                        }
                    }
                }, {
                    field: 'AppName',
                    title: '应用名称'
                }, {
                    field: 'AppVersionDescribe',
                    title: '版本描述',
                    formatter: function (value, row, index) {
                        if (value != "" && value != null) {
                            if (value.length > 20) {
                                return "<span title=" + value + ">" + value.substr(0, 20) + "..." + "</span>";
                            } else {
                                return "<span title=" + value + ">" + value + "</span>";
                            }
                        }
                    }
                }, {
                    field: 'AppVersionUpdateType',
                    title: '更新类型',
                    formatter: function (value, row, index) {
                        if (value == 1) {
                            return "整包更新";
                        } else {
                            return "增量更新";
                        }
                    }
                }, {
                    field: 'IsForcedUpdate',
                    title: '是否强制更新',
                    formatter: function (value, row, index) {
                        if (value == 1) {
                            return "是";
                        } else {
                            return "否";
                        }
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
                    field: 'CreateDate',
                    title: '发布时间',
                    formatter: function (value, row, index) {
                        return formatDate(row.CreateDate, "YYYY-MM-dd HH:mm:ss");
                    }
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var cz = "";
                        if (row.Status == 1) {
                            cz += " <a class='btn mini green thickbox' onclick='UpdateStatus(this," + row.AppVersionID + ",2)'> 禁用  </a > ";
                        } else {
                            cz += " <a class='btn mini green thickbox' onclick='UpdateStatus(this," + row.AppVersionID + ",1)'> 启用  </a > ";
                        }
                        cz += "<a class='btn mini green thickbox' title='编辑版本' onclick=\"tb_show('编辑版本','/Tbx/AppVersion/Edit/" + row.AppVersionID + "?TB_iframe=true&amp;height=520&amp;width=800',false)\"  ><i class='icon-edit'> </i> 编辑  </a > ";

                        cz += " <a class='btn mini green thickbox' href='" + row.AppVersionUpdateAddress + "'  target='_blank'> <i class='icon-edit'> </i> 下载  </a >";
                        cz += " <a class='btn mini red' id='mydelete' href='javascript: void (0)' onclick='DelAppVersionClick(\"" + row.AppVersionID + "\")'> <i class='icon-trash icon-white'></i> 删除</a>";
                        return cz;
                    }
                }]
        });
    };
    removecloud();
    //得到查询的参数
    oTableInit.queryParams = function (params) { 
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的 
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码   
            AppType: $("#AppType").val()
        };
        return temp;
    };
    return oTableInit;
};
$("#searchbtn").click(function () {
    $('#tb_roletable').bootstrapTable('selectPage', 1);
}) 
//修改版本状态
function UpdateStatus(obj, AppVersionID, status) {
    $.post('/Tbx/AppVersion/UpdateStatus', { AppVersionID: AppVersionID, Status: status }, function (data) {
        if (data) {
            //if (status==1) {
            //    $(obj).parent().parent().find("td:eq(6)").text("启用"); 
            //    $(obj).text("禁用");
            //} else {
            //    $(obj).parent().parent().find("td:eq(6)").text("禁用"); 
            //}

            alert('操作成功');
            window.location.reload();
        } else {
            alert('操作失败');
        }
    });
}
//删除
function DelAppVersionClick(AppVersionID) {
    if (confirm("您确定要删除吗？")) {
        $.post("/Tbx/AppVersion/DelAppVersion", { AppVersionID: AppVersionID }, function (data) {
            //alert(data);
            if (data) {
                alert(data);
                window.location.reload();
            }
        });
    }
}
