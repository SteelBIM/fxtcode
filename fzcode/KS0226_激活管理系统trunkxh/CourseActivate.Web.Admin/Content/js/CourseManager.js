/// <reference path="jquery-1.8.0.min.js" />
$(function () {
    $("#diaCourse").removeAttr("style").dialog({ title: '修改课程', width: 650, height: 400, closed: true, modal: true });
    $("#searchBtn").click(function () {
        var queryStr = " 1=1 ";
        var editionid = $("#selectEdition").val();
        if (editionid != "0") {
            queryStr += " and EditionID=" + editionid + "";
        }
        var gradeid = $("#selectGrade").val();
        if (gradeid != "0") {
            queryStr += " and GradeID=" + gradeid + "";
        }
        var value = $("#searchKey").val();
        var title = $("#searchKey").attr("title");
        //如果查询条件为空默认查询全部
        if ($.trim(queryStr) && value != title) {
            var type = $("#searchType").val();
            if (type == "CourseName") {
                queryStr += " and CourseName like '%" + value + "%'";
            }
            else {
                queryStr += " and ID=" + value;
            }
        }
        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    });

    $("#selectGrade").change(function () {
        $("#searchBtn").click();
    });
    $("#selectEdition").change(function () {
        $("#searchBtn").click();
    });

    $('#tbdatagrid').datagrid({
        url: "?action=Query",
        pagination: true,
        rownumbers: true,
        toolbar: "#divtoolbar",
        fitColumns: true,
        striped: true,
        singleSelect: true,
        pagesize: 20,
        pageList: [20, 30, 40, 50],
        width: $(window).width() - 20,
        height: $(window).height() - 55,
        columns: [[
            { field: 'ID', title: '编号', align: 'center', width: 120 },
            { field: 'CourseName', title: '课程名称', align: 'center', width: 100 },
            { field: 'CourseCover', title: '课程封面', align: 'center', width: 100 },
            { field: "GradeName", title: "年级", width: 60, align: 'center' },
            { field: "EditionName", title: "版本", width: 100, align: 'center' },
            { field: "BookReel", title: "册别", width: 60, align: 'center' },
             {
                 field: "IDs", title: "编辑课程信息", width: 60, align: 'center', formatter: function (value, row) {
                     var html = '';
                     html = '<a href="javascript:void(0)" onclick="ShowDia(' + row.ID + ',\'' + row.CourseName + '\',\''+row.CourseCover+'\')">编辑</a>';
                     return html;
                 }
             },
        ]]
    });

    
});

function ShowDia(courseid,coursename,coursecover) {
    $("#diaCourse").dialog("open");
    var html = '<div class="updatediv"><table align="center" style="border-collapse: collapse; border-color:#000000; height:200px; width:300px; padding: 1px; align-content:center; border:1px;">';
    html += '<tr><td>课程名称：</td>';
    html += '<td><input id="coursename" type="text" value="' + coursename + '"/></td></tr>';
    html += '<tr><td>课程封面：</td>';
    html += '<td><input type="file" id="file_upload" name="file_upload"/></td></tr>';
    html += '<tr><td>预览：</td><td><img id="coursecover" src="' + coursecover + '"/></td></tr>';
    html += '<tr><td colspan="2" align="center">';
    html += '<a href="javascript:void(0)" courseid="' + courseid + '" onclick="SaveTitle(this)">保存</a>';
    html += '</td></tr>';
    html += '</table></div>';
    $("#divCourse").html(html);

    $("#file_upload").uploadify({
        //指定swf文件
        'swf': '/Admin/AppTheme/js/uploadify/uploadify.swf',
        //后台处理的页面
        'uploader': '/Admin/Questions/UploadHandler.ashx',
        //按钮显示的文字
        'buttonText': '上传图片',
        //显示的高度和宽度，默认 height 30；width 120
        //'height': 15,
        //'width': 80,
        //上传文件的类型  默认为所有文件    'All Files'  ;  '*.*'
        //在浏览窗口底部的文件类型下拉菜单中显示的文本
        'fileTypeDesc': 'Image Files',
        //允许上传的文件后缀
        'fileTypeExts': '*.gif; *.jpg; *.png',
        //发送给后台的其他参数通过formData指定
        //'formData': { 'someKey': 'someValue', 'someOtherKey': 1 },
        //上传文件页面中，你想要用来作为文件队列的元素的id, 默认为false  自动生成,  不带#
        //'queueID': 'fileQueue',
        //选择文件后自动上传
        'auto': true,
        //设置为true将允许多文件上传
        'multi': false,
        onUploadSuccess: function (file, data, response) {
            if (data) {
                $("#coursecover").attr("src", data);
            }
            else {
                alert("上传失败");
            }
        }
    });
}

function SaveTitle(obj) {
    var courseid = $(obj).attr("courseid");
    var coursename = $("#coursename").val();
    var coursecover = $("#coursecover").attr("src");
    if (courseid && coursename && coursecover) {
        var obj = { CourseID: courseid, CourseName: coursename, CourseCover: coursecover };
        $.post("?action=save", obj, function (data) {
            if (data) {
                $('#tbdatagrid').datagrid("reload");
                $("#diaCourse").dialog("close");
            } else {
                alert("保存失败!");
            }
        })
    }
}