/// <reference path="jquery-1.8.0.min.js" />
var isCompressing = 0;//标记是否正在打包
$(function () {
    $("#selEdition").change(function () {
        searchEvent()
    });
    $("#selGrade").change(function () {
        searchEvent()
    });
    $("#selBookReel").change(function () {
        searchEvent()
    });
    $("#btnAdd").click(function () {
        if ($("#selEdition").val() != "0" && $("#selGrade").val() != "0" && $("#selBookReel").val() != "0") {
            $.post("?Action=AddRes", { EditionID: $("#selEdition").val(), GradeID: $("#selGrade").val(), BookReel: $("#selBookReel").val() }, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    if (result.Success) {
                        searchEvent();
                    } else {
                        alert(result.Message);
                    }
                }
            });
        } else {
            alert("请先选择版本、年级和册别！");
        }
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
            { field: 'EditionName', title: '版本', align: 'center', width: 20 },
            {
                field: 'GradeReel', title: '年册', align: 'center', width: 10, formatter: function (value, row) {
                    return (row.GradeID - 1) + (row.BookReel == 1 ? "A" : (row.BookReel == 2 ? "B" : (row.BookReel == 3 ? "C" : "")));
                }
            },
            { field: 'ResID', title: '资源ID', align: 'center', width: 10 },
            { field: "ResUrl", title: "资源包路径", width: 50, align: 'center' },
            { field: "ResMD5", title: "资源包MD5值", width: 30, align: 'center' },
            { field: "ResUpTimes", title: "资源包打包次数", width: 10, align: 'center' },
            { field: "ResVersion", title: "资源包版本", width: 30, align: 'center' },
            {
                field: "ResMana", title: "操作", width: 15, align: 'center', formatter: function (value, row) {
                    return '<a href="javascript:void(0)" onclick="Compress(this,' + row.ResID
                        + ',\'' + (row.EditionName + '_' + (row.GradeID - 1) + (row.BookReel == 1 ? 'A' : (row.BookReel == 2 ? 'B' : (row.BookReel == 3 ? 'C' : "")))) + '\')">打包</a>';
                }
            }
        ]]
    });
});

function searchEvent() {
    var queryStr = " 1=1 ";
    var editionid = $("#selEdition").val();
    if (editionid != "0") {
        queryStr += " and EditionID=" + editionid + "";
    }
    var gradeid = $("#selGrade").val();
    if (gradeid != "0") {
        queryStr += " and GradeID=" + gradeid + "";
    }
    var bookreel = $("#selBookReel").val();
    if (bookreel != "0") {
        queryStr += " and BookReel=" + bookreel + "";
    }
    $('#tbdatagrid').datagrid({
        url: '?action=Query&queryStr=' + encodeURI(queryStr)
    });
}

function Compress(obj, ResID, fileUrl) {
    if (isCompressing == 1) {
        alert("正在打包，请不要重复操作");
        return;
    }
    $(obj).text("打包中，请稍候...");
    isCompressing = 1;
    if (ResID) {
        $.post("?action=Compress", { ResID: ResID, FileUrl: fileUrl }, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    isCompressing = 0;
                    alert("打包成功！");
                    searchEvent();
                } else {
                    $(obj).text("打包");
                    alert(result.Message);
                }
            }
        });
    } else {
        $(obj).text("打包");
        alert("未获取到资源ID");
    }
}
//关闭资源管理窗口
function CloseRes() {
    $("#diaResource").dialog("close");
}