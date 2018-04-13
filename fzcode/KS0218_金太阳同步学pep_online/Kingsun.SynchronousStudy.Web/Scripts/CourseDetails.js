var CourseDetailsInit = function () {
    var Current = this;
    this.BookID = ""; //书本ID  BookID
    this.CourseCover = "";
    this.getCoverUrl = "";

    this.Init = function () {
        Current.BookID = parseInt(Common.QueryString.GetValue("bookID"));
        $("#bookID").html(Current.BookID);
        $("#tdCover").attr("style", "display:none");
        Current.GetBookName();
    }; 
    //获取书本名称
    this.GetBookName = function () {
      
        var obj = { BookID: Current.BookID };
        $.post("?action=getBookName", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                $("#book").html(result.obj.TeachingNaterialName);
                $("#bookName").html(result.obj.TeachingNaterialName);
                $("#fileinfo").val(result.obj.CourseCover);
                $("#addPerson").val(result.obj.UserName);
                Current.CourseCover = result.obj.CourseCover;
            }
        });
    }


    //添加课程
    $("#modify").click(function () { 
        //Current.getCoverUrl = $("#getFiles").html() + "?FileID=";
        if (Current.CourseCover != "") {
            $("#tdCover").attr("style", "display:block");
            $("#coursesCover").attr("src",  Current.CourseCover);
        }
        $("#divAddCourse").attr("style", "display:block");
        $("#showFileName").html("");
        var myDate = new Date();
        var year = myDate.getFullYear();
        var month = myDate.getMonth() + 1;
        var date = myDate.getDate();
        var hour = myDate.getHours();
        var minutes = myDate.getMinutes();
        var seconds = myDate.getSeconds();
        var time = year + '-' + month + '-' + date + ' ' + hour + ':' + minutes + ':' + seconds;
        $("#AddTime").val(time);
        $("#divAddCourse").dialog({
            title: '修改课程',
            width: 550,
            height: 350,
            closed: false,
            cache: false,
            modal: true,
            buttons: [
                {
                    text: '确定',
                    handler: Current.SaveCourse
                }, {
                    text: '关闭',
                    handler: function() {
                        $("#divAddCourse").dialog('close');
                    }
                }
            ]
        });
    });

     //下载资源包模板
    $("#btnExport").click(function () {
        $('#div_UpdateRevision').attr("style", "display:block");
        $('#div_UpdateRevision').dialog({
            title: '下载模板',
            width: 600,
            height: 400,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_UpdateRevision").attr("src", "../GeneralTools/ExportExcelTools.aspx?BookID=" + Current.BookID);
    });

    //下载资源包模板
    $("#btnImport").click(function () {
        $('#div_UpdateRevision').attr("style", "display:block");
        $('#div_UpdateRevision').dialog({
            title: '下载模板',
            width: 600,
            height: 400,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_UpdateRevision").attr("src", "../GeneralTools/ImportExcelTools.aspx?BookID=" + Current.BookID);
    });


    //上传图片到文件服务器
    $('#file_upload').uploadify({
        width: 80,
        height: 20,
        fileSizeLimit: '4MB',
        buttonText: "选择图片",
        fileTypeExts: '*.jpg;*.bmp;*.gif;*.png',
        auto: true,
        multi: false,
        swf: '../AppTheme/js/uploadify/uploadify.swf',
        uploader: Constant.file_Url + 'UploadHandler.ashx',
        onSelect: function (file) {
            $("#showFileName").html(file.name);
        },
        onUploadSuccess: function (file, data, respone) {
            if (respone) {
                data = eval("(" + data + ")");
                if (data.Success) {
                    var IDArr = [];
                    IDArr.push(data.Data.ID)
                    var IDJson = $.toJSON(IDArr);
                    $.ajax({
                        url: Constant.file_Url + "ConfirmHandler.ashx",
                        type: "post",
                        data: { t: IDJson },
                        dataType: "jsonp",
                        success: function (data) {
                        }
                    });
                    $("#fileinfo").val(data.Data.ID);
                    $("#tdCover").attr("style", "display:block");
                    Current.CourseCover = data.Data.ID;
                    $("#coursesCover").attr("src", Current.getCoverUrl + data.Data.ID);
                }
            }
        },
        onUploadError: function (file, errorCode, erorMsg, errorString) {
            $("#fileinfo").val("");
            Common.tips(erorMsg);
        }
    });

    //保存修改后的课程
    this.SaveCourse = function () { 
        var myDate = new Date();
        var year = myDate.getFullYear();
        var month = myDate.getMonth() + 1;
        var date = myDate.getDate();
        var hour = myDate.getHours();
        var minutes = myDate.getMinutes();
        var seconds = myDate.getSeconds();
        var time = year + '-' + month + '-' + date + ' ' + hour + ':' + minutes + ':' + seconds;
        var coverID = '';// $("#fileinfo").val();
        //上传图片
        var UploadImg = $("#UploadImg").val();
        if (UploadImg != "") {
            $.ajaxFileUpload({
                url: '/Handler/UploadFile.ashx?action=OssUploadImg', //用于文件上传的服务器端请求地址
                secureuri: false, //是否需要安全协议，一般设置为false
                fileElementId: 'UploadImg', //文件上传域的ID
                dataType: 'json', //返回值类型 一般设置为json
                success: function (data, status)  //服务器成功响应处理函数
                {
                    if (data.msg == "1") {
                        //alert(data.url);
                        coverID = data.url;
                        var obj = { CourseCover: coverID, CreateDate: time, BookID: Current.BookID };
                        $.post("?action=save", obj, function (data) {
                            if (data) {
                                var result = eval("(" + data + ")");
                                if (result) {
                                    alert("保存成功!");
                                    $('#tbdatagrid').datagrid("reload");
                                    $("#divAddCourse").dialog('close');
                                } else {
                                    alert("保存失败!");
                                }
                            }
                        });
                    } else {
                        alert('上传图片失败');
                    }
                },
                error: function (data, status, e)//服务器响应失败处理函数
                {
                    alert('异常');
                }
            });
        } else {
            var obj = { CourseCover: coverID, CreateDate: time, BookID: Current.BookID };
            $.post("?action=save", obj, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    if (result) {
                        alert("保存成功!");
                        $('#tbdatagrid').datagrid("reload");
                        $("#divAddCourse").dialog('close');
                    } else {
                        alert("保存失败!");
                    }
                }
            });
        }
        //var obj = { CourseCover: coverID, CreateDate: time, BookID: Current.BookID };
        //$.post("?action=save", obj, function (data) {
        //    if (data) {
        //        var result = eval("(" + data + ")");
        //        if (result) {
        //            alert("修改成功!");
        //            $('#tbdatagrid').datagrid("reload");
        //            $("#divAddCourse").dialog('close');
        //        } else {
        //            alert("保存失败!");
        //        }
        //    }
        //});
    }

}

var courseDetailsInit;
$(function () {
    courseDetailsInit = new CourseDetailsInit();
    courseDetailsInit.Init();
});
