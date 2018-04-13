$(function () {
    $('#tbdatagrid').datagrid({
        url: "?action=StudentList",
        title: '反馈问题表&nbsp;&nbsp;&nbsp;&nbsp;',
        pagination: true,
        rownumbers: true,
        toolbar: "#tb",
        fitColumns: true,
        striped: true,
        singleSelect: true,
        pagesize: 20,
        width: $(window).width() - 20,
        height: $(window).height() - 55,
        loadMsg: '正在加载数据...',
        columns: [[
            { field: 'Resource', title: '来源', align: 'center', width: 100 },
            { field: 'UserType', title: '身份', align: 'center', width: 100 },
            { field: 'UserName', title: '账号', align: 'center', width: 100 },

            { field: 'QQ', title: 'QQ', align: 'center', width: 100 },
            { field: 'Phone', title: '电话', align: 'center', width: 100 },
            {
                field: 'CreateDate', title: '提交时间', align: 'center', width: 100, formatter: function (date) {
                    return FormatTime(date);
                }
            },
            { field: 'Content', title: '内容', align: 'center', width: 100 },
            { field: 'AppVersion', title: '版本', align: 'center', width: 100 },
            {
                field: 'IsDo', title: '处理状态', width: 100, align: 'center',
                formatter: function (value, rec, index) {
                    if (value == "0") {
                        var d = '<a href="#" mce_href="#" onclick="del(\'' + rec.ID + '\')">未处理</a> ';
                        return d;
                    }
                    else {
                        var d = '已处理 ';
                        return d;
                    }
                }
            }
        ]]
    });






    //查询按钮
    $("#searchBtn").click(function () {
        var queryStr = $("#searchKey").val();
        var value = $("#searchKey").val();
        var title = $("#searchKey").attr("title");
        //如果查询条件为空默认查询全部
        if ($.trim(queryStr) && value != title) {
            var type = $("#selectType").val();
            if (type == "UserName") {
                queryStr = type + "= '" + queryStr + "'";
            }
            else {
                queryStr = type + " like '%" + queryStr + "%'";
            }
        }
        else {
            queryStr = "1=1";
        }
        $('#tbdatagrid').datagrid({
            url: '?action=StudentList&queryStr=' + encodeURI(queryStr)
        });

    });

    $("#searchKey").focus(function () {
        var value = $(this).val();
        var title = $(this).attr("title");
        if ($.trim(value) == title) {
            $(this).val("").css("color", "black");
        }
    }).blur(function () {
        var value = $(this).val();
        var title = $(this).attr("title");
        if (!$.trim(value) || $.trim(value) == title) {
            $(this).val(title).css("color", "gray");
        }
    });

})





function del() {  //删除操作  
    $.messager.confirm('确认', '确认删除?', function (row) {
        if (row) {
            var selectedRow = $('#tbdatagrid').datagrid('getSelected');  //获取选中行  
            $.ajax({
                url: '?action=delete&id=' + selectedRow.ID,
                success: function (data) {
                    if (data == "true") {
                        alert("删除成功");
                        $('#tbdatagrid').datagrid('reload');
                    }
                    else if (data == "false") {
                        alert("删除失败");
                    }
                    else {
                        alert(data);
                    }

                }
            });

        }
    })
}