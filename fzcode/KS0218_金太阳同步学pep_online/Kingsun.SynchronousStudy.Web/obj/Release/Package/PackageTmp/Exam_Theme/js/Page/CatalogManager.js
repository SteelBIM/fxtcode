/// <reference path="jquery-1.8.0.min.js" />
$(function () {
    $("#selEdition,#selGrade,#selBookReel").change(function () {
        $.post("?Action=getbook", { EditionID: $("#selEdition").val(), GradeID: $("#selGrade").val(), BookReel: $("#selBookReel").val() }, function (data) {
            if (data && data != "null") {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    var html = '<option value="0" selected="selected">教材</option>';
                    $.each(result.Data.BookList, function (index, value) {
                        html += '<option value="' + value.BookID + '">' + (value.EditionName + "_" + value.BookName) + '</option>';
                    });
                    $("#selBook").html(html);
                } else {
                    alert(result.Message);
                }
                searchEvent();
            }
        });
    });
    $("#selBook").change(function () {
        searchEvent()
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
            { field: "BookName", title: "教材名", width: 20, align: 'center' },
            { field: 'CatalogID', title: '目录ID', align: 'center', width: 10 },
            { field: 'CatalogName', title: '目录名', align: 'center', width: 10 },            
            {
                field: 'PageNo', title: '页码', width: 10, align: 'center', formatter: function (value, row) {
                    return value == 0 ? '' : value;
                }
            },
            { field: 'CatalogLevel', title: '目录层级', width: 10, align: 'center' },
            {
                field: "Sort", title: "排序", width: 10, align: 'center', formatter: function (value, row) {
                    var html = '<select onchange="ChangeSort(this)" CatalogID="' + row.CatalogID + '">';
                    for (var i = 1; i < 50; i++) {
                        if (value == i) {
                            html += '<option value="' + i + '" selected="selected">' + i + '</option>';
                        }
                        else {
                            html += '<option value="' + i + '">' + i + '</option>';
                        }
                    }
                    return html;
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
    $('#tbdatagrid').datagrid({
        url: '?action=Query&queryStr=' + encodeURI(queryStr)
    });
}

//改变排序
function ChangeSort(obj) {
    var catalogid = $(obj).attr("CatalogID");
    var sort = $(obj).val();
    if (catalogid) {
        var unitobj = { CatalogID: catalogid, Sort: sort };
        $.post("?Action=changesort", unitobj, function (data) {
            $('#tbdatagrid').datagrid("reload");
        });
    }
    else {
        alert("目录ID不存在");
        $('#tbdatagrid').datagrid("reload");
    }
}

function Remove(catalogid) {
    if (catalogid > 0) {
        if (confirm("移除资源操作操作会移除与该目录相关的题目信息和用户做题数据，确认要删除吗？")) {
            $.post("?Action=removeques", { CatalogID: catalogid }, function (data) {
                if (data && data != "null") {
                    var result = eval("(" + data + ")");
                    if (result.Success) {
                        searchEvent();
                    } else {
                        alert(result.Message);
                    }
                }
            });
        }
    } else {
        alert("目录ID不存在");
    }
}

function ImportExcel(catalogid) {
    if (catalogid > 0) {
        if (confirm("导入资源操作会覆盖当前题目信息，确认要删除吗？")) {
            $('#div_ImportResource').attr("style", "display:block");
            $('#div_ImportResource').dialog({
                title: '导入资源',
                width: 600,
                height: 400,
                closed: false,
                cache: false,
                modal: true
            });
            $("#iframe_ImportResource").attr("src", "../ExamPaperManagement/ImportExcel.aspx?catalogid=" + catalogid);
        }
    } else {
        alert("目录ID不存在");
    }
}

function UpdateCatalogName(obj, catalogoid) {
    var catalogname = $(obj).prev().val().trim();
    if (catalogname == "") {
        alert("目录名不能为空");
        return;
    } else {
        $.post("?Action=updatecatalogname", { CatalogID: catalogoid, CatalogName: catalogname }, function (data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                searchEvent();
            } else {
                alert(result.Message);
            }
        });
    }
}