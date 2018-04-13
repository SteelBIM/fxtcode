
$(function () {
    var oTable = new TableInit();
    oTable.Init();


    $("#exportexcel").click(function () {
        bootbox.confirm("一次只能导出10万条数据,超出部分将被忽略,是否继续导出？", function (result) {
            if (result) {
                var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
                    pagesize: 100000,   //页面大小
                    pageindex: 0, //页码
                    startdate: $("#startdate").val(),
                    enddate: $("#enddate").val(),
                    batch: $("#txt_batch").val(),
                    activate: $("#txt_activate").val(),
                    buyway: $("#buyway").val()
                };
                window.location.href = "/ActivateLog/Employee_Export?pagesize=" + temp.pagesize + "&pageindex=" + temp.pageindex + "&startdate=" + temp.startdate + "&enddate=" + temp.enddate + "&batch=" + temp.batch + "&activate=" + temp.activate + "&buyway=" + temp.buyway;

                //导出表格
                //this.exportbtn = function () {
                //var params = { limit: 0, offset: 0 };
                //var obj = Current.GetParams(params);
                //var $form = $('<form target="down-file-iframe" method="post" />');
                //$form.attr('action', "/ActivateLog/Employee_Export");
                ////for (var key in obj) {
                ////    $form.append('<input type="hidden" name="' + key + '" value="' + obj[key] + '" />');
                ////}
                //$(document.body).append($form);
                //$form.submit();
                ////$.post("/Order/ExportOrderXls", obj);
                //$form.remove();
                //}
            }
        });


    });
});

//初始化数据
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_departments').bootstrapTable({
            //data: jsonData,
            url: '/ActivateLog/GetBatchActivateInfo',         //请求后台的URL（*）
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
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 20, 40, 50],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            //showColumns: true,                  //是否显示所有的列
            //showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: height,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            onLoadSuccess: function (res) {
                $("#count").html(res.total);
            },
            // exportDataType: "basic",              //'basic':导出当前页, 'all':导出所有数据, 'selected'：导出选中数据.
            //showExport: true,  //是否显示导出按钮  
            //buttonsAlign: "right",  //按钮位置  
            //exportTypes: ['excel'],  //导出文件类型  
            //Icons: 'glyphicon-export',
            //exportOptions: {
            //    ignoreColumn: [0, 1],  //忽略某一列的索引  
            //    fileName: '总台帐报表',  //文件名称设置  
            //    worksheetName: 'sheet1',  //表格工作区名称  
            //    tableName: '总台帐报表',
            //    excelstyles: ['background-color', 'color', 'font-size', 'font-weight']
            //},
            columns: [{
                field: 'ID',
                visible: false
            }, {
                title: '首次激活时间',
                formatter: function (value, row) {
                    //return getLocalTime(row.createtime.replace("/Date(", "").replace(")/", ""));
                    var date = new Date(parseInt(row.createtime.replace("/Date(", "").replace(")/", ""), 10));
                    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                    var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                    var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
                    var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
                    var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
                    return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes ;
                }
            }, {
                title: '批次号',
                formatter: function (value, row) {
                    var action = row.activatecode;
                    return action.substring(0, 3);
                }
            }, {
                field: 'BookName',
                title: '激活课程'
            }, {
                field: 'activatetypename',
                title: '激活码类型'
            }, {
                field: 'publishname',
                title: '出版社'
            }, {
                field: 'activatecode',
                title: '激活码'
            }, {
                field: 'username',
                title: '用户'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += " <a href=\"/ActivateLog/ActivateLogDetails?activatecode=" + row.activatecode + "\"  title=\"\" >详情</a>  ";
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
            startdate: $("#startdate").val(),
            enddate: $("#enddate").val(),
            batch: $("#txt_batch").val(),
            activate: $("#txt_activate").val(),
            buyway: $("#buyway").val()
        };
        return temp;
    };
    return oTableInit;


};

function getLocalTime(nS) {
    return new Date(parseInt(nS)).toLocaleString().replace(/:\d{1,2}$/, '');
}