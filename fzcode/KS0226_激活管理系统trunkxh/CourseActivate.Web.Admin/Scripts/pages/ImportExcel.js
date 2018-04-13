
$(function () {
    var act = new GetActivateType();

    var oFileInput = new FileInput();

    oFileInput.Init("txt_file", "/ImportExcel/SaveAsExcel");

    $("#submit").click(function () {
        var bookid = "";
        if ($("#SelectActivateType").val() == "0") {
            bootbox.alert("请选择激活码类型！");
            return;
        }

        if (!$("#chaxunid").prop("disabled") && $("#bookname").val() == "") {
            bootbox.alert("请选择课程！");
            return;
        }

        var radionum = document.getElementsByName("optionsRadios");
        if (radionum[0].checked) {
            bookid = 0;
        } else {
            bookid = $("#xx").val();
        }

        var obj = {
            activatetypeid: $("#SelectActivateType").val(),
            bookid: bookid,
            remark: $("#Remark").val(),
            excel: $('#photoCover').val(),
        }

        $.post("/ImportExcel/CheckExcel", obj, function (data) {
            if (data.Success) {
                $.post("/ImportExcel/LoadExcel", obj, function (data) {
                    if (data.Success) {
                        bootbox.alert("导入成功！");
                        history.go(-1);
                    }
                });
            }
            else {
                //bootbox.confirm(data.Data, function (result) {
                //      if (result) {                     

                //      }
                //  })
                bootbox.alert(data.ErrorMsg);
                return;
            }
        });

    });

    //查询课程id
    $("#chaxunid").click(function () {
        $("#xx").val();
        $("#ResetPasswordDiv").modal("show");
        GetManagement();
        GetGrade();
        GetSubject();
        $("#SelectManagement").val('0');
        $("#SelectGrade").val('0');
        $("#SelectSubject").val('0');
        $('#tb_bookInfo').bootstrapTable("refresh");
    });

    var queryObj = function (params) {
        var obj = { EditionID: $("#SelectManagement").val(), Grade: $("#SelectGrade").val(), Subject: $("#SelectSubject").val() };
        return obj;
    };
    $(function () {
        $('#tb_bookInfo').bootstrapTable({
            url: '/ImportExcel/GetBookInfo',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: false,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: queryObj,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 100,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: false,
            showColumns: false,                  //是否显示所有的列
            // showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "BookID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                //是否显示父子表
            columns: [{
                field: 'BookName',
                title: '课程名称'
            }, {
                field: 'BookID',
                title: '课程ID'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    action += "<a id=A" + row.BookID + " onclick=\"addBookID(" + row.BookID + ",'" + row.BookName + "')\"; > 添加 <a> ";
                    action += "<a id=B" + row.BookID + " onclick=\"delBookID(" + row.BookID + ")\" style='display:none'> 删除 <a> ";
                    return action;
                }
            }]
        });
    })

    $("#btnSerach").click(function () {
        if ($("#SelectManagement").val() == "0") {
            bootbox.alert("请选择版本！");
            return;
        }
        if ($("#SelectGrade").val() == "0") {
            bootbox.alert("请选择年级！");
            return;
        }
        if ($("#SelectSubject").val() == "0") {
            bootbox.alert("请选择学科！");
            return;
        }
        //var obj = { "EditionID": $("#SelectManagement").val(), "Grade": $("#SelectGrade").val(), "Subject": $("#SelectSubject").val() }
        $('#tb_bookInfo').bootstrapTable("refresh");
    });


    $("#btn_resetpassword").click(function () {
        $("#ResetPasswordDiv").modal("hide");
        $("#xx").val(bidarr.substring(0, bidarr.length - 1));
    });

});
var bidarr = "";

function addBookID(id,name) {
    //$("#B" + id).css("display", "block");
    //$("#A" + id).css("display", "none");
    $("#ResetPasswordDiv").modal("hide");
    $("#xx").val(id);
    $("#bookname").val(name);
    //bidarr = bidarr += id + ',';
}

function delBookID(id) {
    $("#B" + id).css("display", "none");
    $("#A" + id).css("display", "block");
    var s = id + ',';
    bidarr = bidarr.replace("" + s + "", "");
}

var GetActivateType = function () {
    $.post("/ImportExcel/GetActivateType", function (data) {
        if (data != null) {
            for (var i = 0; i < data.length; i++) {
                var result = data[i];
                var html = '';
                html += ' <option value="' + result.activatetypeid + '">' + result.activatetypename + '</option>';
                $(html).appendTo("#SelectActivateType");
            }
        }
    });
}

var GetManagement = function () {
    $("#SelectManagement").html("");
    $.post("/ImportExcel/GetManagement", function (data) {
        if (data != null) {
            for (var i = 0; i < data.length; i++) {
                var result = data[i];
                var html = '';
                html += ' <option value="' + result.EditionID + '">' + result.EditionName + '</option>';
                $(html).appendTo("#SelectManagement");
            }
        }
    });
}

var GetGrade = function () {
    $("#SelectGrade").html("");
    $.post("/ImportExcel/GetGrade", function (data) {
        if (data != null) {
            for (var i = 0; i < data.length; i++) {
                var result = data[i];
                var html = '';
                html += ' <option value="' + result.GradeID + '">' + result.GradeName + '</option>';
                $(html).appendTo("#SelectGrade");
            }
        }
    });
}

var GetSubject = function () {
    $("#SelectSubject").html("");
    $.post("/ImportExcel/GetSubject", function (data) {
        if (data != null) {
            for (var i = 0; i < data.length; i++) {
                var result = data[i];
                var html = '';
                html += ' <option value="' + result.SubjectID + '">' + result.SubjectName + '</option>';
                $(html).appendTo("#SelectSubject");
            }
        }
    });
}

//初始化fileinput
var FileInput = function () {
    var oFile = new Object();
    //初始化fileinput控件（第一次初始化）
    oFile.Init = function (ctrlName, uploadUrl) {
        var control = $('#' + ctrlName);

        //初始化上传控件的样式
        control.fileinput({
            language: 'zh', //设置语言
            uploadUrl: uploadUrl, //上传的地址
            //allowedFileExtensions: ['jpg', 'gif', 'png'],//接收的文件后缀
            showUpload: false, //是否显示上传按钮
            showCaption: false,//是否显示标题
            browseClass: "btn btn-primary", //按钮样式     
            dropZoneEnabled: false,//是否显示拖拽区域
            //queryParams: obj,//传递参数（*）
            //minImageWidth: 50, //图片的最小宽度
            //minImageHeight: 50,//图片的最小高度
            //maxImageWidth: 1000,//图片的最大宽度
            //maxImageHeight: 1000,//图片的最大高度
            //maxFileSize: 0,//单位为kb，如果为0表示不限制文件大小
            //minFileCount: 0,
            maxFileCount: 1,
            showPreview: false,//是否显示预览图
            showRemove: false,

            enctype: 'multipart/form-data',
            validateInitialCount: false,
            previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",

            msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！",
        });

        $("#txt_file").on('filebatchselected', function (event, data, id, index) {
            $(this).fileinput("upload");
        });

        //导入文件上传完成之后的事件
        $("#txt_file").on("fileuploaded", function (event, Date, previewId, index) {
            $(".kv-upload-progress").addClass("hide");
            var data = Date.response;
            $('#photoCover').val(data.Data);
            $('#filename').val(data.Data.substring(data.Data.lastIndexOf("\\")+1));
        });
    }
    return oFile;
};

//关联课程ID
function change(i) {
    if (i == 1) {
        $("#xx").val('');
        $("#xx").attr("disabled", "true");
        $("#chaxunid").attr("disabled", "true");
    }
    if (i == 2) {
        $("#xx").attr("disabled", false);
        $("#chaxunid").attr("disabled", false);
    }
};