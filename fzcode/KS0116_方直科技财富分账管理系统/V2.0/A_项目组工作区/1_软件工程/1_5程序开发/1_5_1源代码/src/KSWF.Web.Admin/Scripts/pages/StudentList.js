var StudentListPage = function () {
    var Current = this;
    this.Init = function () {
        Current.TabInit();
        $("#searchbtn").click(function () {
            $('#tb_student').bootstrapTable('selectPage', 1);
        });

        //导出表格
        $("#exportbtn").click(function () {
            var params = { limit: 0, offset: 0 };
            var $form = $('<form target="down-file-iframe" method="post" />');
            $form.attr('action', "/ClassStatis/ExportStudentXls");
            $form.append('<input type="hidden" name="ClassID" value="' + $("#hidcid").val() + '" />');
            $(document.body).append($form);
            $form.submit();
            //$.post("/Order/ExportOrderXls", obj);
            $form.remove();
        })
        var t = Common.QueryString.GetValue("t");
        $("#aback").click(function () {
            window.location.href = '/ClassStatis/Index?t=' + t;
        })
    }
    this.InitTotal = function () {
        $.post("GetClassStudentCount", function (data) {
            $("#stucount").text(data.StudentCount);
        })
    }

    this.TabInit = function () {
        $('#tb_student').bootstrapTable({
            url: '/ClassStatis/GetStudentPageList',     //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: Current.GetParams,     //传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            //search: true,                     //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            //strictSearch: true,
            showColumns: false,                  //是否显示所有的列
            showRefresh:false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            // height: 500,                     //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度 设置之后表头内容无法对齐。
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            //showToggle: true,                 //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: Current.GetColumn()
        }).on('load-success.bs.table', function (data) {
            Current.InitTotal();
        });
    }

    this.GetColumn = function () {
        var column1 = [{ field: 'ClassName', title: '班级名称' },
            { field: 'ClassNum', title: '班级ID' },
                 {
                     field: 'StuMobile',
                     title: '学生手机号'
                 }, {
                     field: 'StuTrueName',
                     title: '学生姓名'
                 }, {
                     field: 'StuUserName',
                     title: '学生用户名'
                 }
                , { field: 'CreateDate', title: '加入时间', formatter: function (value, row, index) { return Current.FormatTime(value, "yyyy-MM-dd hh:mm:ss"); } }
        ];
        return column1;
    }

    this.GetParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset  //页码
            , SearchKey: $("#searchkey").val()
            , ClassID: $("#hidcid").val()
        };
        return temp;
    }

    this.FormatTime = function (time, format) {
        if (!time) {
            return "";
        }
        if (format == undefined || format == "") {
            format = "yyyy年MM月dd日 hh:mm:ss";
        }
        var date = new Date(parseInt(time.substring(6, time.length - 2)))
        return date.format(format);
    }
}
Date.prototype.format = function (format) {
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}