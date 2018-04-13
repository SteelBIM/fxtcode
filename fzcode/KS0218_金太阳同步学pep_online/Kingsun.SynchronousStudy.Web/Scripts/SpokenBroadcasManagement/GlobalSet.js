///////////全局设置//////////// 
var CourseListInit = function () {
    var Current = this;
    this.Init = function () {
        Current.InitCourseList();
    };
    //初始化列表
    this.InitCourseList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=Query",
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            columns: [[
                    {
                        field: 'ID', title: 'ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.ID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'AheadMinutes', title: '提前进入时间', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.AheadMinutes + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'LimitNum', title: '人数限制', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.LimitNum + '</a>';
                            return html;
                        }
                    },
                    {
                        field: "Operate", title: "操作", width: 120, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html = '<a href="javascript:void(0)" onclick="UpdateGlobalSet(' + row.ID + ')" >修改</a>   ';
                            return html;
                        }
                    }
            ]]
        });
    }
}


var courseListInit;
$(function () {
    courseListInit = new CourseListInit();
    courseListInit.Init();
});

//修改
function UpdateGlobalSet(id) {
    $.post("?action=GetGlobalSetModel", { ID: id }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            $("#txtID").text(result[0].ID);
            $("#txtAheadMinutes").val(result[0].AheadMinutes);
            $("#txtLimitNum").val(result[0].LimitNum);
            
            $("#divUpdateGlobalSet").css("display", "block");
            $("#divUpdateGlobalSet").dialog({
                title: '修改全局设置',
                width: 750,
                height: 500,
                closed: false,
                cache: false,
                modal: true,
                buttons: [
                    {
                        text: '保存',
                        handler: function () {
                            UpdateGlobalSetSave(id);
                        }
                    }, {
                        text: '关闭',
                        handler: function () {
                            $("#divUpdateGlobalSet").dialog('close');
                        }
                    }
                ]
            });
        }
    });
}
//修改保存
function UpdateGlobalSetSave(id) {
    if ($("#txtAheadMinutes").val()=="") {
        alert('提前进入时间不能为空');
        return false;
    }
    if (isNaN($("#txtAheadMinutes").val())) {
        alert('提前进入时间只能为数字');
        return false;
    }
    if ($("#txtLimitNum").val() == "") {
        alert('人数限制不能为空');
        return false;
    }
    if (isNaN($("#txtLimitNum").val())) {
        alert('人数限制只能为数字');
        return false;
    }
    $.post("?action=UpdateGlobalSet", { ID: id, txtAheadMinutes: $("#txtAheadMinutes").val(), txtLimitNum: $("#txtLimitNum").val() }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            if (result.state == "1") {
                alert("修改成功!");
                $('#tbdatagrid').datagrid("reload");
                $("#divUpdateGlobalSet").dialog('close');
            } else {
                alert(result.msg);
            }
        }
    });
}