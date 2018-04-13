var first = "";
var ActivateTypeid = "";
var oldDevicenum = "";
var ActivateTypeMgr = function () {
    var Current = this;
    //初始化数据
    this.Init = function () {
        $('#tb_activatetype').bootstrapTable({
            url: '/ActivateTypeMgr/ActivateTypeMgr_View',         //请求后台的URL（*）
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
            //showColumns: true,                  //是否显示所有的列
            //showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "activatetypeid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'activatetypename',
                title: '激活码类型'
            }, {
                field: 'publishname',
                title: '出版社'
            }, {
                field: 'type',
                title: '应用类型',
                formatter: function (value, index) {
                    var result = "";
                    if (value == 0) {
                        result = "全部端";
                    }
                    if (value == 1) {
                        result = "PC客户端";
                    }
                    if (value == 2) {
                        result = "APP端";
                    }
                    return result;
                }
            }, {
                field: 'way',
                title: '购买方式',
                formatter: function (value, index) {
                    var result = "";
                    if (value == 1) {
                        result = "线上";
                    }
                    if (value == 2) {
                        result = "线下";
                    }
                    return result;
                }
            }, {
                field: 'devicenum',
                title: '可激活设备数'
            }, {
                field: 'status',
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
            }, {
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
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    var param = row.activatetypeid + "," + row.status;
                    action += "<a href=\"javascript:void(0)\" onclick=\"EditActivate(" + row.activatetypeid + ")\"> 编辑 <a> "
                    if (row.status == 0)
                        action += "<a href=\"javascript:void(0)\" onclick=\"deleteActivate(" + row.activatetypeid + ")\"> 删除 <a> "
                    if (row.status == 0 || row.status == 2)
                        action += "<a href=\"javascript:void(0)\" onclick=\"updateStatus(" + param + ")\"> 启用 <a> "
                    if (row.status == 1)
                        action += "<a href=\"javascript:void(0)\" onclick=\"updateStatus(" + param + ")\"> 禁用 <a> "
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
            publishname: $("#publish").val(),
            type: $("#type").val(),
            way: $("#buyway").val()
        };
        return temp;
    };

    $("#searchbtn").click(function () {
        $('#tb_activatetype').bootstrapTable('selectPage', 1);
        // Current.Init();
    })

    $("#btn_setdata").click(function () {
        var CodeType = $("#codeType").val();
        var type = $("#addtype").val();
        var publish = $("#publishname").val();
        var way = $("#way").val();
        var device = $("#device").val();
        var remark = $("#Remark").val();
        if (!first) {
            if (CodeType == null || CodeType == "")
            { bootbox.alert("激活码类型不能为空"); return false; }
            if (type == null || type == "")
            { bootbox.alert("应用类型不能为空"); return false; }
            if (publish == null || publish == "") {
                bootbox.alert("应用类型不能为空"); return false;
            }
            if (way == null || way == "") {
                bootbox.alert("购买方式不能为空"); return false;
            }
            if (device == null || device == "") {
                bootbox.alert("可激活设备数不能为空"); return false;
            } else {
                var patrn = /^[1-9]+[0-9]*]*$/;
                if (!patrn.test(device)) {
                    bootbox.alert("可激活设备数只能为正整数"); return false;
                } else {
                    if (device >= 1000000) {
                        bootbox.alert("可激活设备数只能为0~999999"); return false;
                    }
                }
            }
            var patrn = /[\/\\:\*<>\|]/;
            if (patrn.test(remark)) {
                bootbox.alert("备注不能有特殊符号"); return false;
            }
            addcloud();
            $.post("/ActivateTypeMgr/ActivateTypeMgr_Add", { "activatetypename": CodeType, "publishid": publish, "type": type, "way": way, "devicenum": device, "remark": remark }, function (data) {
                if (data.Success) {
                    removecloud();
                    $("#setbpolicypr").modal('hide');
                    $('#tb_activatetype').bootstrapTable("refresh");
                }
                else {
                    removecloud();
                    $("#setbpolicypr").modal('hide');
                    bootbox.alert(data.ErrorMsg);
                }
            });
        } else {
            if (device == null || device == "") {
                bootbox.alert("可激活设备数不能为空"); return false;
            } else {
                var patrn = /^[1-9]+[0-9]*]*$/;
                if (!patrn.test(device)) {
                    bootbox.alert("可激活设备数只能为正整数"); return false;
                } else {
                    if (device >= 1000000) {
                        bootbox.alert("可激活设备数只能为0~999999"); return false;
                    }
                }
            }
            if (device < oldDevicenum) {
                bootbox.alert("可激活设备数不可低于之前的数量！"); return false;
            } else {
                var patrn = /[\/\\:\*<>\|]/;
                if (patrn.test(remark)) {
                    bootbox.alert("备注不能有特殊符号"); return false;
                }
                addcloud();
                $.post("/ActivateTypeMgr/ActivateTypeMgr_Update", { "activatetypeid": ActivateTypeid, "devicenum": device, "remark": remark }, function (data) {
                    if (data.Success) {
                        removecloud();
                        $("#setbpolicypr").modal('hide');
                        bootbox.setDefaults("locale", "zh_CN");
                        bootbox.alert("添加成功~", function (result) {
                            $('#tb_activatetype').bootstrapTable("refresh");
                        });
                    }
                    else {
                        removecloud();
                        $("#setbpolicypr").modal('hide');
                        bootbox.alert(data.ErrorMsg);
                    }
                });
            }
        }
    })
}
//编辑
function EditActivate(activatetypeid) {
    first = 1;
    ActivateTypeid = activatetypeid;
    $.post("/ActivateTypeMgr/GetActivateById", { "Id": activatetypeid }, function (jsondata) {
        if (jsondata != null && jsondata != "") {
            $("#codeType").val(jsondata[0].activatetypename);
            $("#codeType").attr("disabled", true);
            $("#addtype").val(jsondata[0].type);
            $("#addtype").attr("disabled", true);
            $("#publishname").val(jsondata[0].publishname);
            $("#publishname").attr("disabled", true);
            $("#way").val(jsondata[0].way);
            $("#way").attr("disabled", true);
            $("#device").val(jsondata[0].devicenum);
            $("#Remark").val(jsondata[0].remark);
            $("#setbpolicypr").modal('show');
            oldDevicenum = jsondata[0].devicenum;
        }
    });

}

function updateStatus(activatetypeid, status) {
    $.post("/ActivateTypeMgr/ActivateTypeMgr_Enable", { "activatetypeid": activatetypeid, "status": status }, function (data) {
        if (data.Success) {
            $('#tb_activatetype').bootstrapTable("refresh");
            //window.location.reload();
        }
    })
}

function deleteActivate(activatetypeid) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("是否删除该激活码类型？", function (result) {
        if (result) {
            $.post("/ActivateTypeMgr/ActivateTypeMgr_Delete", { "activatetypeid": activatetypeid }, function (data) {
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
    first = 0;
    $("#codeType").val("");
    $("#addtype").val("");
    $("#publishname").val("");
    $("#way").val("");
    $("#device").val("");
    $("#Remark").val("");
    $("#codeType").attr("disabled", false);
    $("#publishname").attr("disabled", false);
    $("#addtype").attr("disabled", false);
    $("#way").attr("disabled", false);
    $("#setbpolicypr").modal('show');
}











