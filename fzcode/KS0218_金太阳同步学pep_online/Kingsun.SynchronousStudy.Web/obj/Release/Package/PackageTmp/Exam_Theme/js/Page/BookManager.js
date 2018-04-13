/// <reference path="jquery-1.8.0.min.js" />
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
            { field: 'EditionName', title: '版本', align: 'center', width: 40 },
            {
                field: 'GradeReel', title: '年册', align: 'center', width: 15, formatter: function (value,row) {
                    return (row.GradeID - 1) + (row.BookReel == 1 ? "A" : (row.BookReel == 2 ? "B" : (row.BookReel == 3 ? "C" : "")));
                }
            },
            { field: 'BookID', title: '教材ID', align: 'center', width: 15 },
            { field: "BookName", title: "教材名", width: 40, align: 'center' },
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