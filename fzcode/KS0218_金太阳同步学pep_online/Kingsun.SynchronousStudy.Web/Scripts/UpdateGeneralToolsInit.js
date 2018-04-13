var UpdateVideoInit = function () {
    var Current = this;
    var BookID = "";

    this.Init = function () {
        Current.GetVideoList();
    };

    //加载视频信息列表
    this.GetVideoList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=queryvideolist",
            pagination: true,
            rownumbers: true,
            toolbar: "#divtoolbar",
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 55,
            columns: [[
                { field: 'BookName', title: '书籍名称', align: 'center', width: 30 },
                { field: 'FirstTitle', title: '一级标题', align: 'center', width: 30 },
                { field: 'SecondTitle', title: '二级标题', align: 'center', width: 30 },
                { field: 'FirstModular', title: '一级模块', align: 'center', width: 30 },
                { field: 'SecondModular', title: '二级模块', align: 'center', width: 30 },
                {
                    field: 'operate', title: '操作', align: 'center', width: 30, formatter: function (value, rows) {
                        var html = '';
                        html += '<a href="javascript:void(0)" onclick="updateVideoInit.Update(\'' + rows.ID + '\')">修改</a>';
                        return html;
                    }
                }
            ]]
        });
    }

    //条件查询
    this.Search = function () {
        var queryStr = "1=1";
        var bookName = $.trim($("#bookName").val());
        var firstTitle = $.trim($("#firstTitle").val());
        var secondTitle = $.trim($("#secondTitle").val());
        var firstModule = $.trim($("#firstModule").val());
        var secondModule = $.trim($("#secondModule").val());
        if (bookName != "") {
            queryStr += " and BookName like '%" + bookName + "%'";
        }
        if (firstTitle != "") {
            queryStr += " and FirstTitle like '%" + firstTitle + "%'";
        }
        if (secondTitle != "") {
            queryStr += " and SecondTitle like '%" + secondTitle + "%'";
        }
        if (firstModule != "") {
            queryStr += " and FirstModular like '%" + firstModule + "%'";
        }
        if (secondModule != "") {
            queryStr += " and SecondModular like '%" + secondModule + "%'";
        }
        $('#tbdatagrid').datagrid({
            url: '?action=queryvideolist&queryStr=' + encodeURI(queryStr)
        });
    }

    //修改视频信息
    this.Update = function (id) {
        var videoID = id;
        $('#div_UpdateVideo').attr("style", "display:block");
        $('#div_UpdateVideo').dialog({
            title: '更新视频信息',
            width: 900,
            height: 500,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_UpdateVideo").attr("src", "UpdateVideoInfo.aspx?VideoID=" + videoID);
    }

}


var updateVideoInit;
$(function () {
    updateVideoInit = new UpdateVideoInit();
    updateVideoInit.Init();
});

//关闭版本更新对话框
function CloseDialog() {
    $('#div_UpdateVideo').dialog('close');
}