var isEdit = "";
var editionID = "";
var editionconfig = function () {
    var current = this;
    this.Init = function () {
        $('#tb_Edition').bootstrapTable({
            url: '/EditionConfig/EditionConfig_View',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: current.queryParams,//传递参数（*）
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
            uniqueId: "EditionID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                field: 'EditionID',
                title: '版本ID'
            }, {
                field: 'EditionName',
                title: '版本名称'
            }, {
                field: 'SubjectID',
                title: '学科',
                formatter: function (value, index) {
                    var result = "";
                    if (value == 1) return result = "语文";
                    if (value == 2) return result = "数学";
                    if (value == 3) return result = "英语";
                }
            }, {
                field: 'Remark',
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
                field: 'CreateTime',
                title: '创建时间',
                formatter: function (value) { return Common.FormatTime(value); }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += "<a href=\"javascript:void(0)\" onclick=\"EditeditionConfig(" + row.EditionID + ")\"> 编辑 <a> "
                    action += "<a href=\"javascript:void(0)\" onclick=\"deleteeditionConfig(" + row.EditionID + ")\"> 删除 <a> "
                    return action;
                }
            }]
        });
    }
    //得到查询的参数
    this.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset //页码
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
function EditeditionConfig(EditionID) {
    isEdit = 1;
    editionID = EditionID;
    $.post("/EditionConfig/GetEditionById", { "Id": EditionID }, function (jsondata) {
        if (jsondata != null && jsondata != "") {
            $("#Edition").val(jsondata[0].EditionName);
            if (jsondata[0].SubjectID == 1) $("input[name='subject']").get(0).checked = true;
            if (jsondata[0].SubjectID == 2) $("input[name='subject']").get(1).checked = true;
            if (jsondata[0].SubjectID == 3) $("input[name='subject']").get(2).checked = true;
            $("#Remark").val(jsondata[0].Remark);
            $("#setEdition").modal('show');
        }
    });
}
function deleteeditionConfig(EditionID) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("是否删除该版本？", function (result) {
        if (result) {
            $.post("/EditionConfig/EditionConfig_Delete", { "EditionID": EditionID }, function (data) {
                if (data.Success) {
                    $('#tb_Edition').bootstrapTable("refresh");
                }
                else {
                    bootbox.alert(data.ErrorMsg);
                }
            });
        }
    }
  )
}
$("#btn_setdata").click(function () {
    var sunject = "";
    if (document.getElementsByName("subject")[0].checked) sunject = 1;
    if (document.getElementsByName("subject")[1].checked) sunject = 2;
    if (document.getElementsByName("subject")[2].checked) sunject = 3;
    var edition = $("#Edition").val();
    var remark = $("#Remark").val();
    if (edition == null || edition == "")
    { bootbox.alert("版本名称不能为空"); return false; }
    if (sunject == null || sunject == "")
    { bootbox.alert("学科不能为空"); return false; }
    var patrn = /[\/\\:\*<>\|]/;
    if (patrn.test(remark)) {
        bootbox.alert("备注不能有特殊符号"); return false;
    }
    addcloud();
    if (isEdit == 0) {
        $.post("/EditionConfig/EditionConfig_Add", { "EditionName": edition, "subjectID": sunject, "Remark": remark }, function (data) {
            if (data.Success) {
                removecloud();
                $("#setEdition").modal('hide');
                $('#tb_Edition').bootstrapTable("refresh");
            }
            else {
                removecloud();
                $("#setEdition").modal('hide');
                bootbox.alert(data.ErrorMsg);
            }
        });
    } else {
        $.post("/EditionConfig/EditionConfig_Update", { "EditionID": editionID, "EditionName": edition, "subjectID": sunject, "Remark": remark }, function (data) {
            if (data.Success) {
                removecloud();
                $("#setEdition").modal('hide');
                $('#tb_Edition').bootstrapTable("refresh");
            }
            else {
                removecloud();
                $("#setEdition").modal('hide');
                bootbox.alert(data.ErrorMsg);
            }
        });
    }
})
