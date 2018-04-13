

var CourseManagerInit = function () {
    var Current = this;
    this.BookArr = [];//教材数据
    this.StageID = 0;//学段ID
    this.SubjectID = 0;//科目ID
    this.EditionID = 0;//版本ID
    this.GradeID = 0;//年级ID
    this.BreelID = 0;//册别ID
    this.Stage = '';//学段
    this.Subject = '';//科目
    this.Edition = '';//版本
    this.Grade = '';//年级
    this.Breel = '';//册别
    this.BookID = 0;//书本ID
    this.CourseArr = [];

    this.Init = function () {
        Current.InitCourseList();
        var username = $("#userName").html();
        $("#addPerson").html(username);
    };

    //初始化课程列表信息
    this.InitCourseList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=Query",
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            columns: [[
                {
                    field: ' bookName', title: '书本名称', width: 150, align: 'center', formatter: function (value, row) { 
                        Current.CourseArr.push(row);
                        var html = '';
                        html += ' <span>' + row.EducationLevel + row.CourseCategory + row.TextbookVersion + row.JuniorGrade + row.TeachingBooks + '</a>';
                        return html;
                    }
                },
                {
                    field: "Operate", title: "操作", width: 120, align: 'center', formatter: function (value, row) {
                        var html = '';
                        html = '<a href="javascript:void(0)" onclick="courseManagerInit.Deletes(' + row.ID + ',' + row.BookID + ')">删除</a>   ';
                        html += '<a href="javascript:void(0)" onclick="courseManagerInit.CourseDetails(' + row.ID + ',\'' + row.BookID + '\')">详情</a>   ';
                        html += '<a href="../ModuleManagement/ModuleConfig.aspx?bookid=' + row.BookID + '&BookName=' + encodeURI(row.EducationLevel + row.CourseCategory + row.TextbookVersion + row.JuniorGrade + row.TeachingBooks) + '" >配置模块</a>   ';
                        if (row.State) {
                            html += '<a href="javascript:void(0)" onclick="courseManagerInit.ExportExcel(' + row.BookID + ')">下载模板</a>   ';
                            html += '<a href="javascript:void(0)" onclick="courseManagerInit.ImportExcel(' + row.BookID + ')">导入资源</a>   ';
                        } else {
                            html += '<span>下载模板</span>   ';
                            html += '<span>导入资源</span>   ';
                        }
                        html += '<a href="javascript:void(0)" onclick="courseManagerInit.UpdataCatalog(' + row.BookID + ',\'' + row.EducationLevel + row.CourseCategory + row.TextbookVersion + row.JuniorGrade + row.TeachingBooks + '\')">更新目录</a>   ';
                        html += '<a href="javascript:void(0)" onclick="courseManagerInit.ExportCatalogExcel(' + row.BookID + ')">下载目录页码模板</a>   ';
                        html += '<a href="javascript:void(0)" onclick="courseManagerInit.ImportCatalogExcel(' + row.BookID + ')">导入目录页码资源</a>   ';
                        html += '<a href="/Course/CourseVersion.aspx?CourseID=' + row.BookID + '">课程版本管理</a>   ';
                        return html;
                    }
                },
            ]]
        });
    }

    this.UpdataCatalog = function (bookId, bookName) {
        window.open("../ModuleManagement/ModuleFirstConfig.aspx?bookid=" + bookId + "&bookname=" + bookName, '_self');
        //var obj = { BookID: bookId, BookName: bookName };
        //$.post("?action=UpdataCatalog", obj, function (data) {
        //    if (data) {
        //        var result = eval("(" + data + ")");
        //        if (result) {
        //            alert("更新成功!");
        //        } else {
        //            alert("更新失败!");
        //        }
        //    }
        //});
    }

    //导入资源
    this.ImportExcel = function (BookID) {
        $('#div_ImportResource').attr("style", "display:block");
        $('#div_ImportResource').dialog({
            title: '导入资源',
            width: 600,
            height: 400,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_ImportResource").attr("src", "../CourseManagement/ImportExcel.aspx?BookID=" + BookID);
    }

    //下载模板
    this.ExportExcel = function (BookID) {
        $('#div_ImportResource').attr("style", "display:block");
        $('#div_ImportResource').dialog({
            title: '下载模板',
            width: 600,
            height: 400,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_ImportResource").attr("src", "../CourseManagement/ExportExcel.aspx?BookID=" + BookID);
    }

    //导入目录页码资源
    this.ImportCatalogExcel = function (BookID) {
        $('#div_ImportResource').attr("style", "display:block");
        $('#div_ImportResource').dialog({
            title: '导入资源',
            width: 600,
            height: 400,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_ImportResource").attr("src", "../CourseManagement/ImportExcel.aspx?BookID=" + BookID + "&Type=catalog");
    }

    //下载目录页码模板
    this.ExportCatalogExcel = function (BookID) {
        $('#div_ImportResource').attr("style", "display:block");
        $('#div_ImportResource').dialog({
            title: '下载模板',
            width: 600,
            height: 400,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_ImportResource").attr("src", "../CourseManagement/ExportExcel.aspx?BookID=" + BookID + "&Type=catalog");
    }

    //初始化教材列表
    this.InitStandBook = function () {
        //////////////////////获取MOD教材数据/////////////
        Common.GetSBListByStages(0, 0, 0, 0, 0, function (data) {
            $.each(data.Data, function (b, book) {
                Current.BookArr.push(book);
            });
            /////////////////加载科目//////////////////
            Current.InitStage(function (back) {
                if (back)
                    return callback(true);
            });
        });
    }

    ///////////////////////////////////////////
    /////////////////加载学段//////////////////
    ///////////////////////////////////////////
    this.InitStage = function (callback) {
        var temp = [];
        $.each(Current.BookArr, function (b, book) {
            temp.push(book);
        });
        Common.CodeAjax("do.jsonp", "SSTAGE", function (data) {
            var stage = data["SSTAGE"];
            var stageArr = [];
            $.each(stage, function (e, obj) {
                $.each(temp, function (b, book) {
                    if (book.Stage == obj.ID) {
                        stageArr.push(obj);
                        return false;
                    }
                })
            });
            var titleobj = {};
            titleobj.ID = 0;
            titleobj.CodeName = "请选择学段";
            stageArr.unshift(titleobj);
            $("#selectStage").KingsunSelect({
                data: stageArr,
                onchange: function (index, data) {
                    Current.StageID = data.ID;
                    Current.Stage = data.CodeName;
                    Current.PageIndex = 0;
                    Current.InitSubject(function () {
                        return callback(true);
                    });
                }
            });
            $("#selectStage").data("select").selectValue(Current.StageID);
        });
    }


    ///////////////////////////////////////////
    /////////////////加载科目//////////////////
    ///////////////////////////////////////////
    this.InitSubject = function (callback) {
        var temp = [];
        $.each(Current.BookArr, function (b, book) {
            if (book.Stage == Current.StageID) {
                temp.push(book);
            }
        });
        Common.CodeAjax("do.jsonp", "SUB", function (data) {
            var subject = data["SUB"];
            var sub = [];
            $.each(subject, function (e, obj) {
                $.each(temp, function (b, book) {
                    if (book.Subject == obj.ID) {
                        sub.push(obj);
                        return false;
                    }
                })
            });
            var titleobj = {};
            titleobj.ID = 0;
            titleobj.CodeName = "请选择科目";
            sub.unshift(titleobj);
            $("#selectSubject").KingsunSelect({
                data: sub,
                onchange: function (index, data) {
                    Current.SubjectID = data.ID;
                    Current.Subject = data.CodeName;
                    Current.PageIndex = 0;
                    Current.InitEidtin(function () {
                        return callback(true);
                    });
                }
            });
            $("#selectSubject").data("select").selectValue(Current.SubjectID);
        });
    }

    ///////////////////////////////////////////
    /////////////////加载版本//////////////////
    ///////////////////////////////////////////
    this.InitEidtin = function (callback) {
        var temp = [];
        $.each(Current.BookArr, function (b, book) {
            if (book.Stage == Current.StageID && book.Subject == Current.SubjectID) {
                temp.push(book);
            }
        });
        Common.CodeAjax("do.jsonp", "ED", function (data) {
            var edition = data["ED"];
            var edi = [];
            $.each(edition, function (e, obj) {
                $.each(temp, function (b, book) {
                    if (book.Edition == obj.ID) {
                        edi.push(obj);
                        return false;
                    }
                })
            });
            var titleobj = {};
            titleobj.ID = 0;
            titleobj.CodeName = "请选择版本";
            edi.unshift(titleobj);
            $("#selectEdition").KingsunSelect({
                data: edi,
                onchange: function (index, data) {
                    Current.EditionID = data.ID;
                    Current.Edition = data.CodeName;
                    Current.InitGrade(function () {
                        return callback(true);
                    });
                    Current.InitGrade(function () {
                        return callback(true);
                    });
                }
            });
            $("#selectEdition").data("select").selectValue(Current.EditionID);
        });
    }

    ///////////////////////////////////////////
    /////////////////加载年级//////////////////
    ///////////////////////////////////////////
    this.InitGrade = function (callback) {
        var temp = [];
        $.each(Current.BookArr, function (b, book) {
            if (book.Stage == Current.StageID && book.Subject == Current.SubjectID && book.Edition == Current.EditionID) {
                temp.push(book);
            }
        });
        Common.CodeAjax("do.jsonp", "GRADE", function (data) {
            var grade = data["GRADE"];
            var gra = [];
            $.each(grade, function (g, obj) {
                $.each(temp, function (b, book) {
                    if (book.Grade == obj.ID) {
                        gra.push(obj);
                        return false;
                    }
                })
            });
            var titleobj = {};
            titleobj.ID = 0;
            titleobj.CodeName = "请选择年级";
            gra.unshift(titleobj);
            $("#selectGrade").KingsunSelect({
                data: gra,
                onchange: function (index, data) {
                    Current.GradeID = data.ID;
                    Current.Grade = data.CodeName;
                    Current.PageIndex = 0;
                    Current.InitBreel(function () {
                        return callback(true);
                    });
                }
            });
            $("#selectGrade").data("select").selectValue(Current.GradeID);
        });
    }

    ///////////////////////////////////////////
    /////////////////加载册别//////////////////
    ///////////////////////////////////////////
    this.InitBreel = function (callback) {
        var temp = [];
        $.each(Current.BookArr, function (b, book) {
            if (book.Stage == Current.StageID && book.Grade == Current.GradeID && book.Edition == Current.EditionID && book.Subject == Current.SubjectID) {
                temp.push(book);
            }
        });
        Common.CodeAjax("do.jsonp", "BREEL", function (data) {
            var breel = data["BREEL"];
            var bre = [];
            $.each(breel, function (b, obj) {
                $.each(temp, function (b, book) {
                    if (book.Booklet == obj.ID) {
                        bre.push(obj);
                        return false;
                    }
                })
            });
            var titleobj = {};
            titleobj.ID = 0;
            titleobj.CodeName = "请选择册别";
            bre.unshift(titleobj);
            $("#selectBreel").KingsunSelect({
                data: bre,
                onchange: function (index, data) {
                    Current.BreelID = data.ID;
                    Current.Breel = data.CodeName;
                    Current.PageIndex = 0;
                    if (Current.BreelID != 0) {
                        $.each(temp, function (b, book) {
                            if (book.Booklet == Current.BreelID) {
                                Current.BookID = book.ID;
                                return false;
                            }
                        })
                    } else {
                        Current.BookID = 0;
                    }
                }
            });
            $("#selectBreel").data("select").selectValue(Current.BreelID);
        });
    };

    //添加课程
    $("#addCourse").click(function () {
        $("#divAddCourse").attr("style", "display:block");
        var myDate = new Date();
        var year = myDate.getFullYear();
        var month = myDate.getMonth() + 1;
        var date = myDate.getDate();
        var hour = myDate.getHours();
        var minutes = myDate.getMinutes();
        var seconds = myDate.getSeconds();
        var time = year + '-' + month + '-' + date + ' ' + hour + ':' + minutes + ':' + seconds;
        $("#AddTime").val(time);
        Current.InitStandBook();
        $("#divAddCourse").dialog({
            title: '添加课程',
            width: 750,
            height: 250,
            closed: false,
            cache: false,
            modal: true,
            buttons: [
                {
                    text: '保存',
                    handler: Current.SaveCourse
                }, {
                    text: '关闭',
                    handler: function () {
                        $("#divAddCourse").dialog('close');
                    }
                }
            ]
        });
    });

    //保存新添加课程
    this.SaveCourse = function () {

        if (Current.StageID == 0 || Current.SubjectID == 0 || Current.EditionID == 0 || Current.GradeID == 0 || Current.BreelID == 0) {
            alert("课程信息不完全，请重新选择")
            return false;
        }
        var myDate = new Date();
        var year = myDate.getFullYear();
        var month = myDate.getMonth() + 1;
        var date = myDate.getDate();
        var hour = myDate.getHours();
        var minutes = myDate.getMinutes();
        var seconds = myDate.getSeconds();
        var time = year + '-' + month + '-' + date + ' ' + hour + ':' + minutes + ':' + seconds;
        var bookID = Current.BookID;
        var userName = $("#addPerson").html();
        var coverID = '';
        var teachingNaterialName = Current.Stage + Current.Subject + Current.Edition + Current.Grade + Current.Breel;
        for (var i = 0; i < Current.CourseArr.length; i++) {
            if (Current.CourseArr[i].TeachingNaterialName == teachingNaterialName) {
                alert("已存在相同的课程");
                return false;
            }
        }
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
                        var obj = { EducationLevel: Current.Stage, StageID: Current.StageID, CourseCategory: Current.Subject, SubjectID: Current.SubjectID, TextbookVersion: Current.Edition, EditionID: Current.EditionID, JuniorGrade: Current.Grade, GradeID: Current.GradeID, TeachingBooks: Current.Breel, BreelID: Current.BreelID, CourseCover: coverID, UserName: userName, CreateDate: time, BookID: bookID };
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
            var obj = { EducationLevel: Current.Stage, StageID: Current.StageID, CourseCategory: Current.Subject, SubjectID: Current.SubjectID, TextbookVersion: Current.Edition, EditionID: Current.EditionID, JuniorGrade: Current.Grade, GradeID: Current.GradeID, TeachingBooks: Current.Breel, BreelID: Current.BreelID, CourseCover: coverID, UserName: userName, CreateDate: time, BookID: bookID };
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
    }

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
                }
            }
        },
        onUploadError: function (file, errorCode, erorMsg, errorString) {
            $("#fileinfo").val("");
            Common.tips(erorMsg);
        }
    });

    //删除课程
    this.Deletes = function (courseid, bookid) {
        var bookID = bookid;
        var courseArr = [];
        if (confirm("确定要删除吗？")) {
            var obj = { CourseID: courseid };
            $.post("?action=delete", obj, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    if (result) {
                        alert("删除成功!");
                        for (var i = 0; i < courseArr.length; i++) {
                            if (Current.CourseArr[i].BookID != bookID) {
                                courseArr.push(Current.CourseArr[i]);
                            }
                        }
                        Current.CourseArr = courseArr;
                        $('#tbdatagrid').datagrid("reload");
                    } else {
                        alert("删除失败!");
                    }
                }
            });
        }
    }

    //课程详情页面
    this.CourseDetails = function (courseid, bookID) {
        window.open("CourseDetails.aspx?courseID=" + courseid + "&bookID=" + bookID, '_self');
    }


    //通过关键字搜索
    $("#searchValue").focus(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "查询关键字") {
            $("#searchValue").val("");
        }
    })
    $("#searchValue").blur(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "") {
            $("#searchValue").val("查询关键字");
        }
    })
    $("#search").click(function () {
        var searchValue = $("#searchValue").val();
        var queryStr = "1=1 and State = 1";
        if (searchValue == "查询关键字") {
            $('#tbdatagrid').datagrid({
                url: '?action=Query'
            });
            return false;
        }
        queryStr += " and TeachingNaterialName like '%" + searchValue + "%'";
        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    })

}

var courseManagerInit;
$(function () {
    courseManagerInit = new CourseManagerInit();
    courseManagerInit.Init();
});



////配置模块
//function AmanyConfigure(courseid, bookID) {
//    window.open("CourseModuleConfig.aspx?bookID=" + bookID);
//}

////下载模块
//function DownModel(courseid) {
//    alert("下载模块");
//}