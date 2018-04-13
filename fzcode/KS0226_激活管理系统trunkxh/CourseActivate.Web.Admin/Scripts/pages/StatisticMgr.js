var StatisticMgr = function () {
    var current = this;
    this.Init = function () {
        $('#tb_Statisticbook').bootstrapTable({
            url: '/StatisticMgr/StatisticMgr_View',         //请求后台的URL（*）
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
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "bookid",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                title: '序号',
                formatter: function (value, row, index) {
                    return index + 1;
                }
            }, {
                field: 'BookName',
                title: '书本名称'
            }, {
                field: 'bookid',
                title: '书本id'
            }, {
                field: 'num',
                title: '激活码总数'
            }, {
                field: 'usenum',
                title: '已激活'
            }, {
                field: 'Status',
                title: '状态',
                formatter: function (value) {
                    var result = "";
                    if (value == 0) result = "未启用";
                    if (value == 1) result = "启用";
                    if (value == 2) result = "禁用";
                    return result;
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += "<a  href=\"/StatisticMgr/Statisticactivate?bookid=" + row.bookid + "\"> 查看 <a> "
                    return action;
                }
            }]
        });
    }
    //得到查询的参数
    this.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset,//页码
            statge: $("#Period").val(),
            grade: $("#Grade").val(),
            edition: $("#Edition").val(),
            subject: $("#Subject").val(),
            bookreel: $("#Reel").val(),
            status: $("#Status").val()
        };
        return temp;
    };

    $("#btn_search").click(function () {
        $('#tb_Statisticbook').bootstrapTable('selectPage', 1);
        // Current.Init();
    })

    $("#btn_Import").click(function () {
        bootbox.confirm("一次只能导出10万条数据,超出部分将被忽略,是否继续导出？", function (result) {

            if (result) {
                //var st = { "statge": 0, "grade": 0, "edition": 0, "subject": 0, "bookreel": 0, "status": 1 };
                var $form = $('<form target="down-file-iframe" method="post" />');
                $form.attr('action', "/StatisticMgr/book_Export?statge=" + $("#Period").val() + "&grade=" + $("#Grade").val() + "&edition= " + $("#Edition").val() + "&subject= " + $("#Subject").val() + "&bookreel= " + $("#Reel").val() + "&status= " + $("#Status").val());
                $(document.body).append($form);
                $form.submit();
                $form.remove();
            }

        })


    })

    $.post("/ResourceMgr/GetSelectValue", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            for (var i = 0; i < jsonData.length; i++) {
                if (jsonData[i].Keys == "学段") {
                    var htmlPeriod = '';
                    htmlPeriod += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlPeriod).appendTo("#Period");
                }
                if (jsonData[i].Keys == "年级") {
                    var htmlGrade = '';
                    htmlGrade += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlGrade).appendTo("#Grade");
                }
                if (jsonData[i].Keys == "版本") {
                    var htmlEdition = '';
                    htmlEdition += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlEdition).appendTo("#Edition");
                    $(htmlEdition).appendTo("#ModalEdition");
                }
                if (jsonData[i].Keys == "学科") {
                    var htmlSubject = '';
                    htmlSubject += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlSubject).appendTo("#Subject");
                }
                if (jsonData[i].Keys == "册别") {
                    var htmlReel = '';
                    htmlReel += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlReel).appendTo("#Reel");
                }
            }
            var htmlStatus = '';
            htmlStatus += ' <option value="' + 0 + '">' + '未启用' + '</option>';
            htmlStatus += ' <option value="' + 1 + '">' + '启用' + '</option>';
            htmlStatus += ' <option value="' + 2 + '">' + '禁用' + '</option>';
            $(htmlStatus).appendTo("#Status");
        }
    })
}