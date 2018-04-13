var oTableInit = new Object();
//初始化Table
oTableInit.Init = function (add, edit, del) {
    $('#tb_classifies').bootstrapTable({
        url: $("#serverUrl").val(),  //请求后台的URL（*）
        method: 'post',                          //请求方式（*）
        toolbar: '#toolbar',                     //工具按钮用哪个容器
        queryParams: { parentId: 0 },
        striped: true,                           //是否显示行间隔色
        cache: false,                            //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                        //是否显示分页（*）
        sortable: false,                         //是否启用排序
        sortOrder: "asc",                        //排序方式
        //queryParams: oTableInit.queryParams,     //传递参数（*）
        sidePagination: "server",                //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                           //初始化加载第一页，默认第一页
        pageSize: 10000,                            //每页的记录行数（*）
        pageList: [10000],             //可供选择的每页的行数（*）
        search: false,                           //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
        strictSearch: true,
        showColumns: false,                       //是否显示所有的列
        showRefresh: false,                       //是否显示刷新按钮
        minimumCountColumns: 2,                  //最少允许的列数
        clickToSelect: false,                    //是否启用点击选中行
        //height: 500,                           //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "MarketClassifyID",                   //每一行的唯一标识，一般为主键列
        showToggle: false,                       //是否显示详细视图和列表视图的切换按钮
        cardView: false,                         //是否显示详细视图
        detailView: true,                        //是否显示父子表
        columns: [{
            field: 'MarketClassifyID',
            visible: false
        }, {
            field: 'ParentId',
            visible: false
        }, {
            field: 'MarketClassifyName',
            title: '分类名',
        },
        //{
        //    field: 'MarketClassifyProperty',
        //    title: '分类属性',
        //    width: 150
        //},
        {
            field: 'MarketID',
            title: '市场类型',
            formatter: function (value, row, index) {
                if (value == 1) {
                    return "同步教材"
                } else {
                    return "老MOD同步教材";
                }
            }
        }, {
            field: 'MODType',
            title: 'MOD类型',
            formatter: function (value, row, index) {
                switch (value) {
                    case 1:
                        return "学科";
                    case 2:
                        return "版本";
                    case 3:
                        return "年级";
                    case 4:
                        return "册别";
                    case 5:
                        return "学段";
                    default:
                        return "无";
                }
            }
        },
        {
            field: 'ModClassifyName',
            title: 'MOD市场分类',
            visible: false,
        },
        {
            field: 'CreateDate',
            title: '创建日期',
            formatter: function (value, index) {
                if (value)
                    return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""))).toLocaleDateString();
                else
                    return "-";
            }
        }, {
            title: '操作',
            formatter: function (value, row, index) {
                var action = "";
                if (add == "True") {
                    action += " <a href=\"javascript:;\" class=\"btn mini green\"   onclick='Add(" + row.MarketClassifyID + ",\"" + row.ModClassifyName + "\"," + row.MarketID + ",\"" + row.MarketClassifyName + "\")'>子分类 <i class='icon-plus icon-white'></i></a> ";
                }
                if (edit=="True") {
                    action += " <a href=\"javascript:;\" class=\"btn mini green\"  onclick=\"Edit(" + row.MarketClassifyID + "," + row.ParentId+",'')\">编辑 <i class='icon-edit'></i></a> ";
                }
                if (del=="True") {
                    action += " <a href=\"javascript:;\" class=\"btn mini red\"  onclick='Del(" + row.MarketClassifyID + ")'>删除 <i class='icon-trash icon-white'></i></a> ";
                } 
                return action;
            }
        }
        ],
        //注册加载子表的事件。注意下这里的三个参数！
        onExpandRow: function (index, row, $detail, jsonData) {
            oTableInit.InitSubTable(index, row, $detail, add, edit, del, jsonData);
        },
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidth();
        },
        onRefresh: function (params) {
            //alert("onRefresh");
        }
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

//初始化子表格
oTableInit.InitSubTable = function (index, row, $detail, add, edit, del) {
    var parentid = row.MarketClassifyID;
    var ParentModClassifyName = row.ModClassifyName;
    var cur_table = $detail.html("<table id='childTable_" + parentid + "' ></table>").find("table");
    $(cur_table).bootstrapTable({
        url: $("#serverUrl").val(),
        method: 'post',
        queryParams: {
            parentId: parentid
        },
        sidePagination: "server",//注意！必要参数
        clickToSelect: true,
        showHeader: false,
        detailView: true,
        uniqueId: "MarketClassifyID",
        pageSize: 10000,
        pageList: [10000],
        columns: [{
            field: 'MarketClassifyID',
            visible: false
        }, {
            field: 'ParentId',
            visible: false
        },
        {
            field: 'MarketClassifyName',
            title: '分类名',
        },
        //{
        //    field: 'MarketClassifyProperty',
        //    title: '分类属性',
        //    width: 150,
        //},
        {
            field: 'MarketID',
            title: '市场类型',
            formatter: function (value, row, index) {
                if (value == 1) {
                    return "同步教材"
                } else {
                    return "老MOD同步教材";
                }
            }
        }, {
            field: 'MODType',
            title: 'MOD类型',
            formatter: function (value, row, index) {
                switch (value) {
                    case 1:
                        return "学科";
                    case 2:
                        return "版本";
                    case 3:
                        return "年级";
                    case 4:
                        return "册别";
                    case 5:
                        return "学段";
                    default:
                        return "无";
                }
            }
        },
        {
            field: 'ModClassifyName',
            title: 'MOD市场分类',
            visible: false,
        },
        {
            field: 'CreateDate',
            title: '创建日期',
            formatter: function (value, index) {
                if (value)
                    return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""))).toLocaleDateString();
                else
                    return "-";
            }
        },
        {
            title: '操作',
            formatter: function (value, row, index) {
                var action = "";
                if (add == "True") {
                    action += " <a href=\"javascript:;\" class=\"btn mini green\"   onclick='Add(" + row.MarketClassifyID + ",\"" + row.ModClassifyName + "\"," + row.MarketID + ",\"" + row.MarketClassifyName +
                        "\")'>子分类 <i class='icon-plus icon-white'></i></a> ";
                }
                if (edit == "True") {
                    action += " <a href=\"javascript:;\" class=\"btn mini green\"  onclick=\"Edit(" + row.MarketClassifyID + "," + row.ParentId + ",'" + ParentModClassifyName+"')\">编辑 <i class='icon-edit'></i></a> ";
                }
                if (del == "True") {
                    action += " <a href=\"javascript:;\" class=\"btn mini red\"  onclick='Del(" + row.MarketClassifyID + ")'>删除 <i class='icon-trash icon-white'></i></a> ";
                } 
                return action; 
            }
        }
        ],
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidth();
        },
        //无线循环取子表，直到子表里面没有记录
        onExpandRow: function (index, row, $detail) {
            oTableInit.InitSubTable(index, row, $detail, add, edit, del);
        }
    });
};

function Edit(id, ParentId, ParentModClassifyName) {
    $("#editModal").modal();
    $("#ParentName").val(ParentModClassifyName);
    $("#myModalLabel").html("编辑分类");
    if (ParentId > 0) {
        $("#markettypehtml").hide();
        $("#MODType option[value='1']").hide();
        $("#MODType option[value='2']").show();
    } else {
        $("#markettypehtml").show();
        $("#MODType option[value='1']").show();
        $("#MODType option[value='2']").hide();
    }

    $("#parenttypehtml").hide();
    $.post($("#getClassify").val(), { marketClassifyID: id }, function (data) {
        if (data) {
            $("#MarketClassifyID").val(data.MarketClassifyID);
            $("#ParentId").val(data.ParentId);
            $("#MarketClassifyName").val(data.MarketClassifyName);
            $("#MarketClassifyProperty").val(data.MarketClassifyProperty);
            $("#MarketID").val(data.MarketID);
            $("#MODType").val(data.MODType);
            if (data.MODType > 2) {
                $("#modIdLabel").hide();
                $("#modIdDiv").hide();
                $("#UseModNameBtn").hide();
            } else {
                $("#modIdLabel").show();
                $("#modIdDiv").show();
                $("#UseModNameBtn").show();
            }
            InitModClassify(data.MODID);
        }
    });
}
//添加子级分类
function Add(id, modName, MarketID,name) {
    $("#MarketClassifyID").val(0);
    $("#editModal").modal();
    $("#ParentId").val(id);
    $("#markettypehtml").hide();
    $("#parenttypename").html(name);
    $("#parenttypehtml").show();
    $("#modIdDiv").show();
    $("#modIdLabel").show();
    
    $("#MarketID").val(MarketID);
    $("#ParentName").val(modName);
    $("#myModalLabel").html("添加子分类");
    InitModClassify(0);//该方法在视图Index
    $("#MODType").val("");
    $("#MODID").val("");
    $("#MarketClassifyName").val("");
    // $("#MODType").append("<option value='2' >版本</option>");
    $("#MODType option[value='1']").hide();
    $("#MODType option[value='2']").show();
}

//添加顶级分类
function AddParent() {
    $("#MarketClassifyID").val(0);
    $("#ParentId").val("");
    $("#markettypehtml").show();
    $("#parenttypehtml").hide();
    $("#editModal").modal();
    $("#myModalLabel").html("新增顶级分类");
    InitModClassify(0);//该方法在视图Index
    $("#MarketID").val("");
    $("#MODType").val("");
    $("#MODID").val("");
    //$("#MODType").append("<option value='1' >学科</option>");
    $("#MODType option[value='2']").hide();
    $("#MODType option[value='1']").show();
    $("#MarketClassifyName").val("");
}

function Del(id) {
    bootbox.confirm("确定要删除?", function (del) {
        if (del) {
            $.post($("#delUrl").val(), { id: id }, function (data) {
                if (data.Success) {
                    bootbox.alert("删除成功！");
                    $("#tb_classifies").bootstrapTable("refresh");
                } else {
                    bootbox.alert(data.ErrorMsg);
                }
            });
        }
    })

}

function SetIconWidth() {
    $(".detail-icon").parent().css("width", "10px");
}

function submitForm() {

    var MODType = $("#MODType").val();
    if (MODType == null && MODType == "")
        MODType = 0;
    

    if (!$("#MarketID").val()) {
        bootbox.alert("请选择市场类型");
        return;
    }
    if (MODType == 0) {
        bootbox.alert("请选择类型！");
        return;
    }

    if (MODType>2) {
        $("#MODID").val(0);
        if (!$("#MarketClassifyName").val()) {
            bootbox.alert("自定义版本必须填写自定义名称！");
            return;
        }
    } else {
        if ($("#MODID").val() == 0) {
            bootbox.alert("请选择MOD市场分类！");
            return;
        }
    }
    $("form").submit();
}