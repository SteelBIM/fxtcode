/// <reference path="../../jquery-easyui/jquery.min.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../Management/ApplicationManagement.js" />
var CourseVersionPageInit = function () {
    var Current = this;
    Current.pageSize = 15;
    this.Init = function () {
        var CourseID = Common.QueryString.GetValue("CourseID");
        $("#hidCourseIDSelect").val(CourseID);
        //初始化表格
        $("#tbCourseVersion").datagrid({
            title: '课程系统版本',
            nowrap: false,
            border: true,
            collapsible: false, //是否可折叠的  
            rownumbers: true,
            singleSelect: true,
            fitColumns: true,
            loadMsg: '正在加载数据...',
            pagination: true    //分页控件  
        });
        var p = $('#tbCourseVersion').datagrid('getPager');
        p.pagination({
            pageSize: Constant.PageSize, //每页显示的记录条数，默认为10  
            pageList: Constant.PageSizeList, //可以设置每页记录条数的列表  
            beforePageText: '第', //页数文本框前显示的汉字  
            afterPageText: '页    共 {pages} 页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            onRefresh: function (pageNumber, pageSize) {
                loadData(CourseID, pageNumber, pageSize);
            },
            onSelectPage: function (pageNumber, pageSize) {
                loadData(CourseID, pageNumber, pageSize);
            }
        });
        //初始获取数据
        loadData(CourseID, 1, Current.pageSize);
        //弹出添加的页面
        $("#divAdd").window({
            title: "版本更新",
            width: 700,
            height: 450,
            closed: true,
            modal: true
        });
        //新增应用按钮事件
        $("#btnNewCourseVersion").click(function () {
            $("#hidAddOREdit").val("add");
            $("#txtCourseVersion").val("").focus().removeAttr("readonly");

            $("#txtUpdateDescription").val("");
            $("#txtUpdateUrl").val("");
            $("#txtUpdateMD5").val("");
            $("#txtCompleteURL").val("");
            $("#txtCompleteMD5").val("");
            $("#btnSave").removeAttr("disabled");
            //$("#FirstPageNum").val("1");        //课程首页页码

            var data = courseManage.SelectTopVersion(CourseID);
            if (data.Success) {
                if (data.Data != null) {
                    //重新赋值课程首页页码 为当前最大版本页码
                    //$("#FirstPageNum").val(data.Data.FirstPageNum);

                    //赋值到版本文本框
                    $("#txtCourseVersion").val(Common.NewVersion(data.Data.value));
                }
                else {
                    //$("#FirstPageNum").val("1");
                }
            }
            else {
                //$("#FirstPageNum").val("1");
            }

            $("#divAdd").window("open");

        });
        //新增应用版本保存按钮事件
        $("#btnSave").click(function () {
            var info = getValue();
            if (info != null || info != undefined) {
                if (info.AddOREdit == "add") {
                    //var value = AddValue();
                    courseManage.UpdateCourse(CourseID, info.CourseVersion, info.UpdateUrl, info.Description, info.UpdateMD5, info.FirstPageNum, info.TryUpdate, info.CompleteURL, info.CompleteMD5, info.ModuleId, function (data) {
                        if (data.Data != undefined) {
                            loadData(CourseID, 1, Current.pageSize);
                            if (data.Data.Success) {
                                alert("保存成功");
                                $("#divAdd").window("close");
                            }
                            else {
                                alert("保存失败");
                            }
                        }
                        else {
                            alert("保存失败");
                        }
                    });
                }
                if (info.AddOREdit == "edit") {
                    courseManage.EditCourseVersion(info.ID, info.UpdateUrl, info.Description, info.UpdateMD5, info.FirstPageNum, info.TryUpdate, info.CompleteURL, info.CompleteMD5, info.ModuleId, function (data) {
                        if (data.Data != undefined) {
                            loadData(CourseID, 1, Current.pageSize);
                            if (data.Data.Success) {
                                alert("修改成功");
                                $("#divAdd").window("close");
                            }
                            else {
                                alert("修改失败");
                            }
                        }
                        else {
                            alert("修改失败");
                        }
                    });
                }
            }
        });
        //取消按钮
        $("#btnCancel").click(function () {
            $("#divAdd").window("close");
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

        //验证版本是否是最大的
        $("#txtCourseVersion").blur(function () {
            if (CourseID == "" || CourseID == undefined) {
                return null;
            }
            if ($("#hidAddOREdit").val() == "edit") {
                return null;
            }
            var topVersion = courseManage.SelectTopVersion(CourseID);
            if (topVersion == undefined) {
                return null;
            }
            if (Common.toVersion($("#txtCourseVersion").val()) == -1) {
                $("#txtCourseVersion").focus();
                $("#spanVersion").text("版本输入有误").css("color", "red");
                return;
            }
            if (Common.toVersion(topVersion.Data.value) >= Common.toVersion($("#txtCourseVersion").val())) {
                $("#txtCourseVersion").focus();
                $("#spanVersion").text("当前版本值低于历史版本").css("color", "red");
            }
            else {
                $("#spanVersion").text("");
            }
        });
    }
}
//function AddValue() {
//    var CourseID = Common.QueryString.GetValue("CourseID");
//    var topVersion = courseManage.SelectTopVersion(CourseID);
//    if (topVersion.Data.value >= ("v" + $("#txtCourseVersion").val())) {
//        alert("课程版本输入有误，版本比当前版本值小");
//        $("#txtVersion").focus();
//        return null;
//    }
//}

//获取新增保存的值
function getValue() {
    //CompleteMD5
    var obj = {};
    obj.UpdateUrl = $("#txtUpdateUrl").val();
    obj.CourseVersion = $("#txtCourseVersion").val();
    obj.Description = $("#txtUpdateDescription").val();
    obj.UpdateMD5 = $("#txtUpdateMD5").val();
    obj.CompleteMD5 = $("#txtCompleteMD5").val();
    obj.CompleteURL = $("#txtCompleteURL").val();
    obj.CourseID = $("#hidCourseIDSelect").val();
    obj.AddOREdit = $("#hidAddOREdit").val();
    obj.ID = $("#hidID").val();
    obj.ModuleId = $("#slModuleID").val();
    //obj.FirstPageNum = $("#FirstPageNum").val();
    obj.FirstPageNum = 1;
    obj.TryUpdate = $("#tryupdate")[0].checked ? 1 : 0;
    if (obj.CompleteMD5 != "" || obj.CompleteURL != "") {
        obj.TryUpdate = 0;
    }
    //    if (obj.CourseVersion == "" || obj.CourseVersion == undefined || obj.CourseVersion >= 10 || obj.CourseVersion <= 0 || obj.CourseVersion.length > 4) {
    //        alert("课程版本输入有误,版本号应该0.01至9.99之间");
    //        $("#txtCourseVersion").focus();
    //        return null;
    //    }
    if ($("#slModuleID").val() == "0") {
        alert("请选择模块");
        return null;
    }
    if (Common.toVersion(obj.CourseVersion) == -1) {
        alert("课程版本输入有误");
        $("#txtCourseVersion").focus();
        return null;
    }
    if ($("#hidAddOREdit").val() == "add") {
        var topVersion = courseManage.SelectTopVersion(obj.CourseID);
        if (topVersion.Success) {
            if (!topVersion.Data) {
                alert("读取数据失败");
            }
            else {
                if (Common.toVersion(topVersion.Data.value) >= Common.toVersion($("#txtCourseVersion").val())) {
                    alert("课程版本输入有误，版本比当前版本值小");
                    $("#txtCourseVersion").focus();
                    return null;
                }
            }
        }
    }

    obj.CourseVersion = "v" + obj.CourseVersion;


    if (obj.Description == "" || obj.Description == undefined || obj.Description.length > 150) {
        alert("课程简介输入有误,简介内容应该在0-150字之间");
        $("#txtCourseVersion").focus();
        return null;
    }
    obj.Description = Common.HtmlEncode(obj.Description);

    errMsg = Common.ValidateTxt(obj.Description);
    if (errMsg != "" && errMsg != undefined) {
        alert(errMsg);
        $("#txtUpdateDescription").focus();
        return null;
    }

    if (obj.UpdateUrl == "" || obj.UpdateUrl == undefined) {
        alert("更新文件地址输入有误");
        $("#txtUpdateUrl").focus();
        return null;
    }


    errMsg = Common.ValidateTxt(obj.UpdateUrl);
    if (errMsg != "" && errMsg != undefined) {
        alert(errMsg);
        $("#txtUpdateUrl").focus();
        return null;
    }
    if (!Common.Validate.IsURL(obj.UpdateUrl)) {
        alert("更新文件url非法路径");
        $("#txtUpdateUrl").focus();
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
//获取分页数据并且绑定到表格
function loadData(WhereCondition, CurrentPageIndex, PageSize) {
    courseManage.QueryCourseVersion(WhereCondition, CurrentPageIndex, PageSize, function (data) {
        if (data.Data == null || data.Data == undefined) {
            data.Data = { total: 0, rows: [] };
        }
        $("#tbCourseVersion").datagrid("loadData", data.Data);
    });

}

function CourseVersionDate(date) {
    date = new Date(parseInt(date.substring(6, date.length - 2)))
    return date.format("yyyy年MM月dd日 hh时mm分");
}

function CourseVersionOpetate(ID, row) {
    var param = "'" + ID + "','" + row.CourseID + "'";
    var paramScan = "'" + ID + "','" + row.Version + "','" + row.Description.replace(/[\r\n]/g, "\\n").replace(/'/g, "/'") + "','" + row.UpdateURL + "','" + row.UpdateMD5 + "','" + row.Disable + "','" + row.FirstPageNum + "','" + row.TryUpdate + "'" + ",'" + row.CompleteURL + "','" + row.CompleteMD5 + "','" + row.ModuleID + "'";
    var btnhtml = "";
    if (row.Disable) {
        if (row.DownloadTimes > 0) {//如果此版本被下载过
            btnhtml += '&nbsp;<a href="javascript:void(0)" onclick="cDiableFalse()">禁用</a>&nbsp;&nbsp;';
        }
        else {//没有被下载过
            btnhtml += '&nbsp;<a href="javascript:void(0)" onclick="cDisableVersion(' + param + ')">禁用</a>&nbsp;&nbsp;';
        }
    }
    else {
        btnhtml += '&nbsp;<a href="javascript:void(0)" onclick="cActiveVersion(' + param + ')">启用</a>&nbsp;&nbsp;';
    }
    btnhtml += '<a href="javascript:void(0)" onclick="cScan(' + paramScan + ')">查看/修改</a>&nbsp;&nbsp;&nbsp;';
    btnhtml += '<a href="' + row.UpdateURL + '" target="_blank">下载</a>&nbsp;';
    return btnhtml;
}

function cActiveVersion(ID, CourseID) {
    courseManage.ActiveCourseVersion(ID, CourseID, function (data) {
        if (data.Data != undefined) {
            loadData(CourseID, 1, Constant.PageSize);
            alert("启用" + (data.Data.Success ? "成功" : "失败"));
        }
        else {
            alert("启用失败");
        }
    });
}
function cScan(ID, Version, Description, UpdateUrl, UpdateMD5, Disable, FirstPageNum, TryUpdate, CompleteURL, CompleteMD5, ModuleId) {
    $("#hidAddOREdit").val("edit");
    $("#hidID").val(ID);
    $("#txtCourseVersion").val(Version.substr(1, (Version.length - 1))).attr("readonly", "readonly");
    $("#txtUpdateDescription").val(Description).focus();
    $("#txtUpdateUrl").val(UpdateUrl);
    $("#txtUpdateMD5").val(UpdateMD5);
    $("#txtCompleteURL").val(CompleteURL == "null" ? "" : CompleteURL);
    $("#txtCompleteMD5").val(CompleteMD5 == "null" ? "" : CompleteMD5);
    $("#tryupdate")[0].checked = (TryUpdate == 1 ? true : false);
    $("#slModuleID").val(ModuleId);
    if (Disable == "true") {//版本被启用了不允许修改
        $("#btnSave").attr("disabled", "true");
    }
    else {//版本没有启用允许修改
        $("#btnSave").removeAttr("disabled");
    }
    //$("#FirstPageNum").val(FirstPageNum);
    $("#divAdd").window("open");
}

function cDisableVersion(ID, CourseID) {

    courseManage.DisableCourseVersion(ID, CourseID, function (data) {
        if (data.Data != undefined) {
            loadData(CourseID, 1, Constant.PageSize);
            alert("禁用" + (data.Data.Success ? "成功" : "失败"));
        }
        else {
            alert("禁用失败");
        }
    });
}

function cDiableFalse() {
    alert("此版本被下载过，不能被禁用");
}

function CourseVersionStatus(Status) {
    if (Status) {
        return "启用";
    }
    if (!Status) {
        return "禁用";
    }
    return "Error";
}

function GetModuleName(id) {
    if (id == "1") {
        return "点读";
    }
    if (id == "13") {
        return "练习册";
    }
    if (id == "18") {
        return "YX_EBook";
    }
    if (id == "19") {
        return "YX_课本剧";
    }
    if (id == "20") {
        return "YX_同步听";
    }
    if (id == "21") {
        return "YX_练习册";
    }
    if (id == "22") {
        return "YX2_跟我学";
    }
    return "Error";
}