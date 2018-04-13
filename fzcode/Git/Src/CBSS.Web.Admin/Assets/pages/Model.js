var oTableInit = new Object();
//初始化Table
oTableInit.Init = function (add, edit, del, down, Model_ImgLibrary) {
    $("#tb_model").bootstrapTable({
        url: $("#GetModelJsonPage").val(),  //请求后台的URL（*）
        method: 'post',                          //请求方式（*）
        toolbar: '#toolbar',                     //工具按钮用哪个容器
        //queryParams: { parentId: 0 },
        striped: true,                           //是否显示行间隔色
        cache: false,                            //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                        //是否显示分页（*）
        sortable: false,                         //是否启用排序
        sortOrder: "asc",                        //排序方式
        queryParams: function (params) {
            return {
                parentId: 0,
                pageNumber: (params.offset / params.limit) + 1,  //页码 
                pageSize: params.limit,                         //页面大小
            };
        },     //传递参数（*）
        sidePagination: "server",                //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                           //初始化加载第一页，默认第一页
        pageSize: 10,                            //每页的记录行数（*）
        pageList: [10],             //可供选择的每页的行数（*）
        search: false,                           //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
        strictSearch: true,
        showColumns: false,                       //是否显示所有的列
        showRefresh: false,                       //是否显示刷新按钮
        minimumCountColumns: 2,                  //最少允许的列数
        clickToSelect: true,                    //是否启用点击选中行
        //height: 500,                           //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "ModelID",                   //每一行的唯一标识，一般为主键列
        showToggle: false,                       //是否显示详细视图和列表视图的切换按钮
        cardView: false,                         //是否显示详细视图
        detailView: true,                        //是否显示父子表
        columns: [
            {
                field: 'ModelID',
                visible: false
            },
            {
                field: 'ParentID',
                visible: false
            }, {
                field: 'ModelName',
                title: '模型名称',
                width: 150
            }, {
                field: 'OldModelID',
                title: '老同步学模型ID',
                width: 150
            },
            {
                field: 'FunctionID',
                title: '功能',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "评测"
                    } else if (value == 2) {
                        return "跟读";
                    } else if (value == "1,2") {
                        return "评测,跟读";
                    } else {
                        return "";
                    }
                }
            },
            {
                field: 'ModelType',
                title: '模型分类',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "课内模型"
                    } else {
                        return "课外模型";
                    }
                }
            },
            {
                field: 'ModelSourceType',
                title: '模型资源分类',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "原生"
                    } else {
                        return "H5";
                    }
                }
            },
            {
                field: 'Status',
                title: '是否启用',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "启用"
                    } else {
                        return "禁用";
                    }
                }
            },
            {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (add == "True") {
                        action += " <a class=\"btn mini green\"  href=\"javascript:;\"  onclick='Add(" + row.ModelID + ",this)'>子模型 <i class='icon-plus icon-white'></i></a> ";
                    }
                    if (Model_ImgLibrary == "True") {
                        action += " <a class=\"btn mini green\" href=\"javascript:;\"  onclick='ModelImgLibraryClick(" + row.ModelID + ")'>图片库 <i class='icon-picture'></i></a> ";
                    }
                    if (row.ResourceTemplate != "") {
                        if (down == "True") {
                            action += " <a class=\"btn mini green\" href=\"" + row.ResourceTemplate + "\"  target='_blank'>下载模板 <i class='icon-picture'></i></a> ";
                        }
                    }
                    if (edit == "True") {
                        action += " <a class=\"btn mini green\" href=\"javascript:;\"  onclick='Edit(" + row.ModelID + ")'>编辑 <i class='icon-edit'></i></a> ";
                    }
                    if (del == "True") {
                        if (row.ModelID > 7) {
                            action += " <a class=\"btn mini red\" href=\"javascript:;\"  onclick='Del(" + row.ModelID + ")'>删除 <i class='icon-trash icon-white'></i></a> ";
                        }
                    }
                    return action;
                }
            }
        ],
        //注册加载子表的事件。注意下这里的三个参数！
        onExpandRow: function (index, row, $detail, jsonData) {
            oTableInit.InitSubTable(index, row, $detail, add, edit, del, down, Model_ImgLibrary);
        },
        onLoadSuccess: function (data) {
            //alert("onLoadSuccess");
            SetIconWidth();
        },
        onRefresh: function (params) {
            //alert("onRefresh");
        }
    });

    //模型分类
    $("#ModelSourceType").change(function () {
        if ($("#ModelSourceType").val() == 2) {
            $("#H5UrlDiv").show();
        } else {
            $("#H5UrlDiv").hide();
        }
    });
}
//得到查询的参数
oTableInit.queryParams = function (params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
        pagesize: params.limit,   //页面大小
        pageindex: params.offset //页码
    };
    return temp;
};
//初始化子表格
oTableInit.InitSubTable = function (index, row, $detail, add, edit, del, down, Model_ImgLibrary) {
    var parentid = row.ModelID;
    var cur_table = $detail.html("<table id='childTable_" + parentid + "' ></table>").find("table");
    $(cur_table).bootstrapTable({
        url: $("#GetModelJson").val(),  //请求后台的URL（*）
        method: 'post',                          //请求方式（*） 
        queryParams: { parentId: parentid },
        striped: true,                           //是否显示行间隔色
        cache: false,                            //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*） 
        sortable: false,                         //是否启用排序
        sortOrder: "asc",                        //排序方式
        //queryParams: oTableInit.queryParams,     //传递参数（*）
        sidePagination: "server",                //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                           //初始化加载第一页，默认第一页
        pageSize: 10,                            //每页的记录行数（*）
        pageList: [10],             //可供选择的每页的行数（*）
        search: false,                           //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
        strictSearch: true,
        showColumns: false,                       //是否显示所有的列
        showRefresh: false,                       //是否显示刷新按钮
        minimumCountColumns: 2,                  //最少允许的列数
        clickToSelect: false,                    //是否启用点击选中行
        //height: 500,                           //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "ModelID",                   //每一行的唯一标识，一般为主键列
        showToggle: false,                       //是否显示详细视图和列表视图的切换按钮
        cardView: false,                         //是否显示详细视图
        detailView: true,                        //是否显示父子表
        showHeader: false,
        columns: [
            {
                field: 'ModelID',
                visible: false
            },
            {
                field: 'ParentID',
                visible: false
            },
            {
                field: 'ModelName',
                title: '模型名称2',
                width: 150
            },
            {
                field: 'FunctionID',
                title: '功能',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "评测"
                    } else {
                        return "跟读";
                    }
                }
            },
            {
                field: 'ModelType',
                title: '模型分类',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "课内模型"
                    } else {
                        return "课外模型";
                    }
                }
            },
            {
                field: 'ModelSourceType',
                title: '模型资源分类',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "原生"
                    } else {
                        return "H5";
                    }
                }
            },
            {
                field: 'Status',
                title: '是否激活',
                width: 150,
                formatter: function (value, row, index) {
                    if (value == 1) {
                        return "启用"
                    } else {
                        return "禁用";
                    }
                }
            },
            {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (add == "True") {
                        action += " <a class=\"btn mini green\"  href=\"javascript:;\"  onclick='Add(" + row.ModelID + ",this)'>子模型 <i class='icon-plus icon-white'></i></a> ";
                    }
                    if (Model_ImgLibrary == "True") {
                        action += " <a class=\"btn mini green\" href=\"javascript:;\"  onclick='ModelImgLibraryClick(" + row.ModelID + ")'>图片库 <i class='icon-picture'></i></a> ";
                    }
                    if (row.ResourceTemplate != "") {
                        if (down == "True") {
                            action += " <a class=\"btn mini green\" href=\"" + row.ResourceTemplate + "\"  target='_blank'>下载模板 <i class='icon-picture'></i></a> ";
                        }
                    }
                    if (edit == "True") {
                        action += " <a class=\"btn mini green\" href=\"javascript:;\"  onclick='Edit(" + row.ModelID + ")'>编辑 <i class='icon-edit'></i></a> ";
                    }
                    if (del == "True") {
                        if (row.ModelID > 7) {
                            action += " <a class=\"btn mini red\" href=\"javascript:;\"  onclick='Del(" + row.ModelID + ")'>删除 <i class='icon-trash icon-white'></i></a> ";
                        }
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
            oTableInit.InitSubTable(index, row, $detail, add, edit, del, down, Model_ImgLibrary);
        }
    });
};

function SetIconWidth() {
    $(".detail-icon").parent().css("width", "10px");
}
//图片库
function ModelImgLibraryClick(ModelID) {
    window.location.href = "/Tbx/ModelImgLibrary/Index/" + ModelID;
}
//添加顶级分类
function AddParent() {
    $("#parentModelDiv").hide();
    //重置表单
    $("#ModelID").val(0);
    $("#ModelName").val("");
    $("#Sort").val(0);
    $("#FunctionID").selectpicker('val', '请选择');
    $("#ModelType").val(1);
    $("#ModelSourceType").val(1);
    $("#ResourceTemplate").val("");
    $("#H5UrlDiv").hide();
    $("#H5Url").val("");
    $("input[name='Status']").eq(0).parent().attr("class", "checked");
    $("#editModal").modal();
}
//添加子级分类
function Add(id, obj) {
    var parentModel = $(obj).parent().parent().find("td:eq(1)").text();
    //alert(parentModel);
    $("#parentModelDiv").show();
    $("#parentModelDiv").find("span").text(parentModel);
    $("#myModalLabel").text("新增模型");
    //重置表单
    $("#ModelID").val(0);
    $("#ModelName").val("");
    $("#Sort").val(0);
    $("#FunctionID").selectpicker('val', '请选择');
    $("#ModelType").val(1);
    $("#ModelSourceType").val(1);
    $("#ResourceTemplate").val("");
    $("#H5UrlDiv").hide();
    $("#H5Url").val("");
    $("input[name='Status']").eq(0).parent().attr("class", "checked");

    $("#editModal").modal();
    $("#ParentID").val(id);
}
//编辑
function Edit(id) {
    $("#parentModelDiv").hide();
    $("#editModal").modal();
    $("#myModalLabel").text("编辑模型");
    $.post($("#getModel").val(), { ModelID: id }, function (data) {
        if (data) {
            $("#ModelID").val(data.ModelID);
            $("#ModelName").val(data.ModelName);
            $("#ParentID").val(data.ParentID);
            $("#Sort").val(data.Sort);
            $("#FunctionID").selectpicker('val', data.FunctionID.split(','));
            $("#ResourceTemplate").val(data.ResourceTemplate);
            $("#ModelType").val(data.ModelType);
            $("#ModelSourceType").val(data.ModelSourceType);
            $("input[name='Status']").parent().attr("class", "");
            $("#ModelStatus").val(data.Status);
            $("input[name='Status']").eq(data.Status - 1).parent().attr("class", "checked");
            if (data.ModelSourceType == 2) {
                $("#H5UrlDiv").show();
                $("#H5Url").val(data.H5Url);
            } else {
                $("#H5UrlDiv").hide();
                $("#H5Url").val("");
            }
        }
    });
}
//删除
function Del(id) {
    bootbox.confirm("您确定要删除吗？", function (isDel) {
        if (isDel) {
            $.post($("#delUrl").val(), { ModelID: id }, function (data) {
                if (data) {
                    bootbox.alert(data);
                    $('#tb_model').bootstrapTable('selectPage', 1);
                } else {
                    bootbox.alert(data);
                }
            });
        }
    });
}
//提交表单
function submitForm() {


    addcloudNew("加载中，请稍后...");
    if($("#ModelStatus").val() == 2){
        $("#forbidden").click();
    }

    var FunctionID = $('#FunctionID').val();
    var ResourceTemplate = $('#ResourceTemplate').val();
    //alert(FunctionID == null);
    var ModelName = $("#ModelName").val().trim();
    var H5Url = $("#H5Url").val().trim();
    if (ModelName == "") {
        removecloudNew();
        bootbox.alert('模型名称不能为空！');
        return false;
    } else if (ModelName.length > 19) {
        removecloudNew();
        alert('模型名称长度不能超过18个字符');
    } else {
        if (FunctionID == null) {
            removecloudNew();
            bootbox.alert('请选择模型功能');
            return false;
        }
        if ($("#ModelSourceType").val() == 2) {
            if (H5Url == "") {
                removecloudNew();
                bootbox.alert('H5链接地址不能为空！');
                return false;
            }
        }
        //模板地址
        if (ResourceTemplate != "") {
            var index = ResourceTemplate.lastIndexOf(".");
            if (index < 0) {
                removecloudNew();
                bootbox.alert('Excel文件上传格式不正确！');
                return false;
            } else {
                var ext = ResourceTemplate.substring(index + 1, ResourceTemplate.length);
                if (ext == "xls" || ext == "xlsx") {

                } else {
                    removecloudNew();
                    bootbox.alert('文件上传格式不正确！');
                    return false;
                }
            }
        }
        //验证重名
        var ModelID = $("#ModelID").val();
        $.post($("#checkModelNameUrl").val(), { ModelID: ModelID, ModelName: ModelName }, function (data) {
            //alert(data);
            if (data) {
                removecloudNew();
                bootbox.alert('模型名称不能重名！');
            } else {
                $("form").submit();
            }
        });
    }
}