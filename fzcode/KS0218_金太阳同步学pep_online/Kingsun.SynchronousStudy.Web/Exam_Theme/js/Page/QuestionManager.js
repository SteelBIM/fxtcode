/// <reference path="jquery-1.8.0.min.js" />
$(function () {
    $("#selEdition,#selGrade,#selBookReel").change(function () {
        $.post("?Action=getbookandcatalog", { EditionID: $("#selEdition").val(), GradeID: $("#selGrade").val(), BookReel: $("#selBookReel").val() }, function (data) {
            if (data && data != "null") {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    var html = '<option value="0" selected="selected">教材</option>';
                    $.each(result.Data.BookList, function (index, value) {
                        html += '<option value="' + value.BookID + '">' + (value.EditionName + "_" + value.BookName) + '</option>';
                    });
                    $("#selBook").html(html);
                    html = '<option value="0" selected="selected">目录</option>';
                    $.each(result.Data.CatalogList, function (index, value) {
                        html += '<option value="' + value.CatalogID + '">' + value.CatalogName + '</option>';
                    });
                    $("#selCatalog").html(html);
                } else {
                    alert(result.Message);
                }
                searchEvent();
            }
        });
    });
    $("#selBook").change(function () {
        $.post("?Action=getcatalog", { BookID: $("#selBook").val() }, function (data) {
            if (data && data != "null") {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    var html = '<option value="0" selected="selected">目录</option>';
                    $.each(result.Data.CatalogList, function (index, value) {
                        html += '<option value="' + value.CatalogID + '">' + value.CatalogName + '</option>';
                    });
                    $("#selCatalog").html(html);
                } else {
                    alert(result.Message);
                }
                searchEvent();
            }
        });
    });
    $("#selCatalog").change(function () {
        searchEvent();
    });
    $("#btnFresh").click(function () {
        searchEvent();
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
            {
                field: 'GradeReel', title: '教材', align: 'left', width: 25, formatter: function (value, row) {
                    return row.EditionName + "_" + (row.GradeID - 1) + (row.BookReel == 1 ? "A" : (row.BookReel == 2 ? "B" : (row.BookReel == 3 ? "C" : ""))) + "_" + row.BookName;
                }
            },
            { field: "CatalogName", title: "目录", width: 20, align: 'left' },
            {
                field: "Sort", title: "排序", width: 10, align: 'center', formatter: function (value, row) {
                    var html = '<select onchange="ChangeSort(this)" QuestionID="' + row.QuestionID + '">';
                    for (var i = 1; i < 100; i++) {
                        if (value == i) {
                            html += '<option value="' + i + '" selected="selected">' + i + '</option>';
                        }
                        else {
                            html += '<option value="' + i + '">' + i + '</option>';
                        }
                    }
                    return html;
                }
            },
            {
                field: 'QuestionID', title: '题目ID', align: 'left', width: 45, formatter: function (value, row) {
                    if (row.ParentID != '' && row.ParentID != null) {
                        return '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + value;
                    } else {
                        return value;
                    }
                }
            },
            { field: 'ParentID', title: '父级ID', align: 'left', width: 15 },
            { field: "QuestionModel", title: "模板", width: 5, align: 'left' },
            { field: "QuestionTitle", title: "标题", width: 40, align: 'left' },
            { field: "Mp3Url", title: "音频", width: 40, align: 'left' },
            { field: "ImgUrl", title: "图片", width: 40, align: 'left' },
            {
                field: "Answer", title: "填空答案", align: 'center', width: 40, formatter: function (value, row) {
                    if (value != null) {
                        return '<a href="javascript:void(0)" onclick="SaveAnswer(this,\'' + row.QuestionID + '\')">保存</a> '
                            + ' <input type="text" style="width:230px" value="' + value.replace("'", "\'") + '" />';
                    } else {
                        return "";
                    }
                }
            },
            {
                field: "SelectItem", title: "选项答案", align: 'center', width: 15, formatter: function (value, row) {
                    if (row.ParentID != '' && row.ParentID != null) {//为小题时，可编辑答案
                        if (row.QuestionModel == "M4" || row.QuestionModel == "M6" || row.QuestionModel == "M7" || row.QuestionModel == "M10") {
                            return '<a href="javascript:void(0)" onclick="EditSelectItem(\'' + row.QuestionID + '\')">编辑选项</a>'
                        }
                    }
                    return "";
                }
            },
            {
                field: "Mana", title: "操作", width: 15, align: 'center', formatter: function (value, row) {
                    return '<a href="javascript:void(0)" onclick="UpdateQuestion(\'' + row.QuestionID + '\')">编辑题目</a>';
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
    var bookid = $("#selBook").val();
    if (bookid != "0") {
        queryStr += " and BookID=" + bookid + "";
    }
    var catalogid = $("#selCatalog").val();
    if (catalogid != "0") {
        queryStr += " and CatalogID=" + catalogid + "";
    }
    $('#tbdatagrid').datagrid({
        url: '?action=Query&queryStr=' + encodeURI(queryStr)
    });
}

function PlayAudioOrImg(url) {
    window.open(url, "", "height=300,width=300,top=300,left=500,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no");
}

//改变排序
function ChangeSort(obj) {
    var questionid = $(obj).attr("QuestionID");
    var sort = $(obj).val();
    if (questionid) {
        $.post("?Action=changesort", { QuestionID: questionid, Sort: sort }, function (data) {
            $('#tbdatagrid').datagrid("reload");
        });
    }
    else {
        alert("题目ID不存在");
        $('#tbdatagrid').datagrid("reload");
    }
}

function UpdateQuestion(questionid) {
    window.open("QuestionUpdate.aspx?QuestionID=" + questionid, "_blank");
}

function SaveAnswer(obj, questionid) {
    var answer = $(obj).next().val().trim();
    if (answer == "") {
        alert("答案不能为空");
        return;
    } else {
        $.post("?Action=saveanswer", { QuestionID: questionid, Answer: answer }, function (data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                searchEvent();
            } else {
                alert(result.Message);
            }
        });
    }
}


function EditSelectItem(questionid) {
    window.open("SelectItemManager.aspx?QuestionID=" + questionid, "_blank");
}