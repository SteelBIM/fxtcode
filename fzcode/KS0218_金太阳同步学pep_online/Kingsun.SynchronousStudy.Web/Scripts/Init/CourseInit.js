
/// <reference path="../../jquery-easyui/jquery.min.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../Management/CourseManagement.js" />


var CoursesInit = function () {
    var Current = this;
    Current.pageSize = 15;
    Current.PageIndex = 1;
    Current.appID = Constant.AppID;
    this.Init = function () {
        $("body").keydown(function (e) {
            if (e.keyCode == 13) {
                QueryloadData(Current.appID, getWhere(), 1, Constant.PageSize);
            }
        });

        $("#tbCourse").datagrid({
            title: '课程列表',
            nowrap: false,
            border: true,
            collapsible: false, //是否可折叠的  
            rownumbers: true,
            singleSelect: true,
            fitColumns: true,
            loadMsg: '正在加载数据...',
            pagination: true    //分页控件  
        });
        var p = $('#tbCourse').datagrid('getPager');
        p.pagination({
            pageSize: Constant.PageSize, //每页显示的记录条数，默认为10  
            pageList: Constant.PageSizeList, //可以设置每页记录条数的列表  
            beforePageText: '第', //页数文本框前显示的汉字  
            afterPageText: '页    共 {pages} 页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            onRefresh: function (pageNumber, pageSize) {
                loadData("", pageNumber, pageSize);
                Current.PageIndex = pageNumber;
            },
            onSelectPage: function (pageNumber, pageSize) {
                loadData("", pageNumber, pageSize);
                Current.PageIndex = pageNumber;
            }
        });
        //初始获取数据
        loadData("", 1, Constant.PageSize);
        //弹出添加版本更新的页面
        $("#divAddVersion").dialog({
            title: "版本更新",
            width: 700,
            height: 450,
            modal: true,
            closed: true
        });

        //添加按钮事件
        $("#btnNewCourse").click(function () {
            window.location.href = "../Course/CourseEdit.aspx?AppID=" + Current.appID;
        });

        $("#btn_back").click(function () {
            window.location.href = "../Application/ApplicationManager.aspx";
        });

        //新增课程版本保存按钮事件
        $("#btnSaveVersion").click(function () {

            var info = Current.getValueVersion();
            if (info != null || info != undefined) {
                courseManage.UpdateCourse(info.CourseID, info.CourseVersion, info.UpdateUrl, info.Description, info.UpdateMD5, info.FirstPageNum, info.TryUpdate, info.CompleteURL, info.CompleteMD5, function (data) {
                    if (data.Success) {
                        $("#divAddVersion").window("close");
                        window.location.href = "../Course/CourseVersion.aspx?CourseID=" + info.CourseID;
                    }
                    else {
                        alert("新增失败，提示" + data.ErrorMsg);
                    }
                });
            }
        });
        //取消按钮
        $("#btnCancelVersion").click(function () {
            $("#divAddVersion").window("close");
        });


        //验证版本是否是最大的
        $("#txtVersion").blur(function () {
            var CourseID = $("#hidCourseidSelect").val();
            if (CourseID == "" || CourseID == undefined) {
                return null;
            }
            var topVersion = courseManage.SelectTopVersion(CourseID);
            if (topVersion == undefined) {
                return null;
            }
            if (Common.toVersion($("#txtVersion").val()) == -1) {
                $("#txtVersion").focus();
                $("#spanVersion").text("版本输入有误").css("color", "red");
                return;
            }

            if (Common.toVersion(topVersion.Data.value) >= Common.toVersion($("#txtVersion").val())) {
                $("#txtVersion").focus();
                $("#spanVersion").text("版本比当前版本值小").css("color", "red");
            }
            else {
                $("#spanVersion").text("");
            }
        });



        $("#txtCompleteURL").change(function () {
            if ($(this).val() != "") {
                $("#tryupdate").attr("checked", true).attr("disabled", true);
                $("#tryupdate").hide();
            } else {
                $("#tryupdate").attr("checked", false).attr("disabled", false);
                $("#tryupdate").show();
            }
        });
        $("#txtCompleteMD5").change(function () {
            if ($(this).val() != "") {
                $("#tryupdate").attr("checked", true).attr("disabled", true);
                $("#tryupdate").hide();
            } else {
                $("#tryupdate").attr("checked", false).attr("disabled", false);
                $("#tryupdate").show();
            }
        });

        $("#btnSearch").click(function () {
            loadData("CourseName like '%" + getWhere() + "%'", Current.PageIndex, Constant.PageSize);
        });

        $("#txtSearchKey").focus(function () {
            var txtbox = $("#txtSearchKey");
            if (txtbox.val() == "请输入要查询课程名称") {
                $("#txtSearchKey").val("");
            }
        }).blur(function () {
            var txtbox = $("#txtSearchKey");
            if ($.trim(txtbox.val()) == "") {
                txtbox.val("请输入要查询课程名称");
            }
        });
    }
    this.cActiveVersion = function (ID) {
        courseManage.ActiveCourse(ID, function (data) {
            if (data.Success) {
                loadData("CourseName like '%" + getWhere() + "%'", Current.PageIndex, Constant.PageSize);
            }
            else {
                alert("启用失败。" + data.ErrorMsg);
            }
        });
    }
    this.cDisableVersion = function (ID) {
        courseManage.DisableCourse(ID, function (data) {
            if (data.Success) {
                loadData("CourseName like '%" + getWhere() + "%'", Current.PageIndex, Constant.PageSize);
            }
            else {
                alert("禁用失败。" + data.ErrorMsg);
            }
        });
    }
    this.cDelete = function (ID) {
        if (!confirm("该课程正在使用中，是否继续删除？")) {
            return;
        }
        courseManage.Delete(ID, function (data) {
            if (data.Success) {
                alert("删除成功！");
                QueryloadData(Current.appID, getWhere(), 1, Constant.PageSize);
            }
            else {
                alert("禁用失败。" + data.ErrorMsg);
            }
        });
    }


    //获取新增保存的值
    this.getValueVersion = function () {
        var obj = {};
        obj.UpdateUrl = $("#txtUpdateUrl").val();
        obj.CourseVersion = $("#txtVersion").val();
        obj.Description = $("#txtVersionDescription").val();
        obj.CourseID = $("#hidCourseidSelect").val();
        obj.UpdateMD5 = $("#txtUpdateMD5").val();
        obj.CompleteMD5 = $("#txtCompleteMD5").val();
        obj.CompleteURL = $("#txtCompleteURL").val();
        obj.TryUpdate = $("#tryupdate")[0].checked ? 1 : 0;
        if (obj.CompleteMD5 != "" || obj.CompleteURL != "") {
            obj.TryUpdate = 0;
        }
        //obj.FirstPageNum = $("#FirstPageNum").val();
        obj.FirstPageNum = 1;

        if (obj.CourseID == "" || obj.CourseID == undefined) {
            alert("CourseID为空，请重新操作");
            $("#divAddVersion").dialog("close");
        }


        obj.CourseVersion = "v" + obj.CourseVersion;


        if (obj.Description == "" || obj.Description == undefined || obj.Description.length > 300) {
            alert("课程简介输入有误,简介内容应该在0-150字之间");
            $("#txtCourseVersion").focus();
            return null;
        }
        //obj.Description = Common.HtmlEncode(obj.Description);

        errMsg = Common.ValidateTxt(obj.Description);
        if (errMsg != "" && errMsg != undefined) {
            alert(errMsg);
            $("#txtUpdateDescription").focus();
            return null;
        }



        if (obj.CompleteURL != "" || obj.CompleteMD5 != "") {
            if (obj.CompleteURL == "") {
                alert("请填写增量包地址");
                $("#txtCompleteUrl").focus();
                return null;
            }
            if (obj.CompleteMD5 == "") {
                alert("请填写增量包MD5");
                $("#txtCompleteMD5").focus();
                return null;
            }
            errMsg = Common.ValidateTxt(obj.CompleteURL);
            if (errMsg != "" && errMsg != undefined) {
                alert(errMsg);
                $("#txtCompleteUrl").focus();
                return null;
            }
            if (!Common.Validate.IsURL(obj.CompleteURL)) {
                alert("增量包文件url非法路径");
                $("#txtCompleteUrl").focus();
                return null;
            }


            if ((obj.CompleteURL).indexOf(".zip") <= -1) {
                alert("增量包文件必须是zip格式!");
                $("#txtCompleteUrl").focus();
                return null;
            }
            var errMsg1 = "";
            errMsg1 = Common.ValidateTxt(obj.CompleteMD5);
            if (errMsg1 != "" && errMsg1 != undefined) {
                alert("增量包文件MD5值输入有误，提示：" + errMsg1);
                $("#txtCompleteMD5").focus();
                return null;
            }
        }



        errMsg = Common.ValidateTxt(obj.UpdateUrl);
        if (errMsg != "" && errMsg != undefined) {
            alert(errMsg);
            $("#txtUpdateUrl").focus();
            return null;
        }
        if (obj.UpdateUrl == "" || obj.UpdateUrl == undefined) {
            alert("更新文件地址输入有误");
            $("#txtUpdateUrl").focus();
            return null;
        }
        if (!Common.Validate.IsURL(obj.UpdateUrl)) {
            alert("更新文件url非法路径");
            $("#txtUpdateUrl").focus();
            return null;
        }
        if ((obj.UpdateUrl).indexOf(".zip") <= -1) {
            alert("更新文件必须是zip格式!");
            $("#txtUpdateUrl").focus();
            return null;
        }
        if (obj.UpdateMD5 == "" || obj.UpdateMD5 == undefined) {
            alert("MD5值输入有误");
            $("#txtUpdateMD5").focus();
            return null;
        }
        errMsg = Common.ValidateTxt(obj.UpdateMD5);
        if (errMsg != "" && errMsg != undefined) {
            alert("MD5值输入有误，提示：" + errMsg);
            $("#txtUpdateMD5").focus();
            return null;
        }
        return obj;
    }

}

function getWhere() {
    var key = $.trim($("#txtSearchKey").val());
    if (key == "请输入要查询课程名称" || key == "" || key == undefined) {
        return "";
    }
    else {
        //        var errMsg = Common.ValidateTxt(key);
        //        if (errMsg != "" && errMsg != undefined) {
        //            alert("输入的搜索条件有误，提示：" + errMsg);
        //            return "1=0";
        //        }
        //        else {
        //            return key;
        //        }
        return key;
    }
}


function getValue() {
    var obj = {};
    obj.CourseName = $("#txtCourseName").val();
    obj.CourseVersion = $("#txtCourseVersion").val();
    obj.CourseDescription = $("#txtUpdateDescription").val();

    var errMsg = Common.ValidateTxt(obj.CourseName);
    if (obj.CourseName == "" || obj.CourseName == undefined || (errMsg != "" && errMsg != undefined)) {
        alert("课程名称输入有误");
        $("#txtCourseName").focus();
        return null;
    }
    errMsg = Common.ValidateTxt(obj.CourseVersion);
    if (obj.CourseVersion == "" || obj.CourseVersion == undefined || (errMsg != "" && errMsg != undefined)) {
        alert("课程版本输入有误");
        $("#txtCourseVersion").focus();
        return null;
    }


    errMsg = Common.ValidateTxt(obj.CourseDescription);
    if (errMsg != "" && errMsg != undefined) {
        alert(errMsg);
        $("#txtUpdateDescription").focus();
        return null;
    }
    return obj;
}

function loadData(WhereCondition, CurrentPageIndex, PageSize) {
    courseManage.QueryCourse(WhereCondition, CurrentPageIndex, PageSize, function (data) {
        if (data.Data == null || data.Data == undefined) {
            data.Data = { total: 0, rows: [] };
        }
        $("#tbCourse").datagrid("loadData", data.Data);
    });
}

function QueryloadData(appID, WhereCondition, CurrentPageIndex, PageSize) {
    courseManage.QueryCourseName(appID, WhereCondition, CurrentPageIndex, PageSize, function (data) {
        if (data.Data == null || data.Data == undefined) {
            data.Data = { total: 0, rows: [] };
        }
        $("#tbCourse").datagrid("loadData", data.Data);
    });
}

function CourseStatus(Status) {
    if (Status) {
        return "启用";
    }
    if (!Status) {
        return "禁用";
    }
    return "Error";
}

function CourseDate(date) {
    date = new Date(parseInt(date.substring(6, date.length - 2)))
    return date.format("yyyy年MM月dd日 hh时mm分");
}

function CourseOpetate(ID, row) {
    var paramCourse = "'" + ID + "'";
    var param = "'" + ID + "','" + row.AppID + "'";
    var param2 = "'" + ID + "','" + row.AppID + "','" + row.DownloadTimes + "'";
    var paramScan = "'" + ID + "','" + row.Version + "','" + row.Description.replace(/[\r\n]/g, "") + "','" + row.UpdateURL + "','" + row.UpdateMD5 + "'";
    //var paramScan = "'f632a994-f5d0-4515-8cf3-7b0cd76419e3','v1.0','PEP4B课程包','undefined','undefined'";

    //                "'bccfef86-a091-4ab6-94e0-6b9a8b3b6b20','v1.0','PEP小学英语','undefined','undefined'"

    var btnhtml = '&nbsp;<a href="javascript:void(0)" onclick="cScan(' + paramScan + ')">历史版本</a>&nbsp;';
    if (row.Disable) {
        btnhtml += '<a href="javascript:void(0)" onclick="CourseInit.cDisableVersion(' + param + ')">禁用</a>&nbsp;';
    }
    else {
        btnhtml += '<a href="javascript:void(0)" onclick="CourseInit.cActiveVersion(' + param + ')">启用</a>&nbsp;';
    }
    btnhtml += '<a href="javascript:void(0)" onclick="cVersionUpdate(' + paramCourse + ')">课程更新</a>&nbsp;';
    btnhtml += '<a href="javascript:void(0)" onclick="cUpdate(' + param + ')">修改</a>&nbsp;';
    btnhtml += '<a href="javascript:void(0)" onclick="CourseInit.cDelete(' + param + ')">删除</a>&nbsp;';
    return btnhtml;
}

function cUpdate(courseID, appID) {
    window.location = "CourseEdit.aspx?AppID=" + appID + "&CourseID=" + courseID;
}

function cScan(ID) {
    window.location.href = "../Course/CourseVersion.aspx?CourseID=" + ID;
}
function cVersionUpdate(ID) {
    $("#txtUpdateUrl").val("");
    $("#txtVersion").val("").focus();
    $("#txtVersionDescription").val("");
    $("#hidCourseidSelect").val(ID);
    $("#txtUpdateMD5").val("");
    //$("#FirstPageNum").val("1");      //课程首页页码
    //重新赋值课程首页页码 为当前最大版本页码
    var data = courseManage.SelectTopVersion(ID);
    if (data.Success) {
        if (data.Data != null) {
            //重新赋值课程首页页码 为当前最大版本页码
            //$("#FirstPageNum").val(data.Data.FirstPageNum);


            //赋值到版本文本框
            $("#txtVersion").val(Common.NewVersion(data.Data.value));
        }
        else {
            //$("#FirstPageNum").val("1");
        }
    }
    else {
        //$("#FirstPageNum").val("1");
    }

    $("#divAddVersion").dialog("open");
}



