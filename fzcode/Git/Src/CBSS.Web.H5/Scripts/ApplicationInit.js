var ApplicationInit = function () {
    var Current = this;
    this.AppID = "";
    this.VersionID = 0;
    //this.MaxVersionNum = 0;
    this.IOSMaxNum = 0; // IOS 最大版本号
    this.AndroidMaxNum = 0;// Android 最大版本号

    this.Init = function () {
        Current.VersionID = parseInt(Common.QueryString.GetValue("VersionID"));
        Current.InitApplicationList();
    };

    //加载App信息列表
    this.InitApplicationList = function () {
        var queryStr = "1=1";
        queryStr += " and VersionID = " + Current.VersionID;
        queryStr += " and VersionType != 1";
        $('#tbdatagrid').datagrid({
            url: "?action=queryAppList&queryStr=" + encodeURI(queryStr),
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
                { field: 'VersionNumber', title: '应用版本号', align: 'center', width: 15 },
                {
                    field: 'VersionType', title: '版本类型', align: 'center', width: 15, formatter: function (value, rows) {
                        var html = '';
                        html += ' <span>' + (rows.VersionType == 1 ? 'IOS' : 'Android') + '</span>';
                        return html;
                    }
                },
                { field: 'VersionDescription', title: '版本描述', align: 'center', width: 45 },
                {
                    field: "State", title: "状态", width: 15, align: 'center', formatter: function (value, rows) {
                        var html = '';
                        html += ' <span>' + (rows.State == true ? '启用' : '禁用') + '</span>';
                        return html;
                    }
                },
                {
                    field: 'CreateDate', title: '发布时间', align: 'center', width: 20, formatter: function (value, rows) {
                        var html = '';
                        html += '<span>' + Common.FormatTime(rows.CreateDate, "yyyy-MM-dd hh:mm:ss") + '</span>';
                        return html;
                    }
                },
                { field: 'UserName', title: '上传者', align: 'center', width: 15 },
               {
                   field: "Operate", title: "操作", width: 15, align: 'center', formatter: function (value, rows) {
                       var html = '';
                       html += ' <a href="javascript:void(0)" onclick="applicationInit.ChangeState(\'' + rows.ID + '\',\'' + rows.State + '\')">' + (rows.State == true ? '禁用' : '启用') + '</a>';
                       html += ' <a href="javascript:void(0)" onclick="applicationInit.ViewEdit(\'' + rows.ID + '\',\'' + rows.State + '\')">' + '查看' + '</a>';
                       html += '<span>/</span>';
                       html += ' <a href="javascript:void(0)" onclick="applicationInit.AppEdit(\'' + rows.ID + '\',\'' + rows.State + '\')">' + '修改' + '</a>';
                       html += ' <a href="' + rows.FileAddress + '">' + '下载' + '</a>';
                       html += ' <a href="javascript:void(0)" onclick="applicationInit.ChangeisEnabled(\'' + rows.ID + '\',\'' + rows.isEnabled + '\')">' + (rows.isEnabled == 0 ? '显示' : '隐藏') + '</a>';
                       return html;
                   }
               }
            ]]
        });
    }

    $("#IOSCheck").click(function () {
        Current.ScreeningVersion();
    })
    $("#AndroidCheck").click(function () {
        Current.ScreeningVersion();
    })

    this.ScreeningVersion = function () {
        var IOSCheckState = document.getElementById("IOSCheck").checked;
        var AndroidCheckState = document.getElementById("AndroidCheck").checked;
        var queryStr = "1=1";
        queryStr += " and VersionID = " + Current.VersionID;
        if (IOSCheckState && !AndroidCheckState) {
            queryStr += " and VersionType = 1"
        }
        if (!IOSCheckState && AndroidCheckState) {
            queryStr += " and VersionType != 1"
        }
        $('#tbdatagrid').datagrid({
            url: "?action=queryAppList&queryStr=" + encodeURI(queryStr),
        });
    }

    //添加新版本对话框
    $("#addNew").click(function () {
        $('#div_UpdateRevision').attr("style", "display:block");
        $('#div_UpdateRevision').dialog({
            title: '版本更新',
            width: 900,
            height: 500,
            closed: false,
            cache: false,
            modal: true
        });
        $("#iframe_UpdateRevision").attr("src", "UpdateApplication.aspx?VersionID=" + Current.VersionID)
    })

    //查看app信息
    this.ViewEdit = function (appid, state) {
        var appID = appid;
        var appObj = { AppID: appID };
        Current.ClearInformation();
        $("#makeSure").attr("style", "visibility:hidden");
        $("#cancel").attr("style", "visibility:hidden");
        $("#appInfo").attr("style", "display:block");
        $('#appInfo').dialog({
            title: '版本信息',
            width: 450,
            height: 300,
            closed: false,
            cache: false,
            modal: true,
            buttons: [{
                text: '关闭',
                handler: function () {
                    $('#appInfo').dialog('close');
                }
            }]
        });
        $.post("?action=getAppInfo", appObj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                var versionType = result.obj.VersionType;
                var versionNum = result.obj.VersionNumber.substring(1, result.obj.VersionNumber.legth);
                var address = result.obj.FileAddress.substring(7, result.obj.FileAddress.legth);
                if (versionType == 1) {
                    document.getElementById("IOS").checked = true;
                } else {
                    document.getElementById("Android").checked = true;
                }
                $("#appVersion").html(versionNum);
                $("#introduce").val(result.obj.VersionDescription);
                $("#address").val(address);
                $("#verifyValue").val(result.obj.FileMD5);
                var flag = result.obj.MandatoryUpdate;
                document.getElementById("mandatory").checked = flag;
            }
        });
    }

    //修改app信息
    this.AppEdit = function (appid, state) {
        var State = state;
        if (State == "true") {
            alert("启用状态下无法修改")
            return false;
        }
        $("#makeSure").attr("style", "visibility:visible");
        $("#cancel").attr("style", "visibility:visible");
        Current.AppID = appid;
        var appObj = { AppID: Current.AppID };
        Current.ClearInformation();
        $("#appInfo").attr("style", "display:block");
        $('#appInfo').dialog({
            title: '修改',
            width: 450,
            height: 300,
            closed: false,
            cache: false,
            modal: true,
            buttons: []
        });
        $.post("?action=getAppInfo", appObj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                var versionType = result.obj.VersionType;
                var versionNum = result.obj.VersionNumber.substring(1, result.obj.VersionNumber.legth);
                var address = result.obj.FileAddress.substring(7, result.obj.FileAddress.legth);
                if (versionType == 1) {
                    document.getElementById("IOS").checked = true;
                } else {
                    document.getElementById("Android").checked = true;
                }
                $("#appVersion").html(versionNum);
                $("#introduce").val(result.obj.VersionDescription);
                $("#address").val(address);
                $("#verifyValue").val(result.obj.FileMD5);
                var flag = result.obj.MandatoryUpdate;
                document.getElementById("mandatory").checked = flag;
            }
        });
    }

    //保存App更新后的信息
    $("#makeSure").click(function () {
        var version = document.getElementById("IOS").checked;
        var versionType = "";
        if (version) {
            versionType = "IOS";
        } else {
            versionType = "Android";
        }
        var versionNum = $("#appVersion").html();
        var content = $.trim($("#introduce").val());
        var name = $("#uploadPerson").val();
        var address = $.trim($("#address").val());
        var result = false;
        if (address != '') {
            address = 'http://' + address;
        }
        var verifyValue = $("#verifyValue").val();
        var flag = document.getElementById("mandatory").checked;
        var urlReg = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
        var mandatoryUpdate = false;
        if (flag) {
            mandatoryUpdate = true;
        }
        if (content == '') {
            alert('更新简介不能为空！');
            return false;
        }
        if (!checkTextValid(content)) {
            alert("文本框中不能含有\n\n 1 单引号: ' \n 2 双引号: \" \n 3 竖 杠: | \n 4 尖角号: < > \n\n请检查输入！");
            return false;
        }
        if (versionType == "Android") {
            if (address == '') {
                alert('更新文件地址不能为空！');
                return false;
            }
            if (!urlReg.test(address)) {
                alert("请输入正确的更新文件地址");
                return;
            }
            var suffix = address.substring((address.length - 3), (address.length)).toLowerCase();
            if (suffix != "zip" && suffix != "apk") {
                alert("包地址不存在，请检查");
                return;
            }
            if (verifyValue == '') {
                alert('更新文件MD5值不能为空！');
                return false;
            }
        }
        var obj = { AppID: Current.AppID, VersionType: versionType, VersionNum: versionNum, Content: content, Name: name, Address: address, VerifyValue: verifyValue, MandatoryUpdate: mandatoryUpdate };
        $.post("?action=updateApp", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.obj) {
                    $('#appInfo').dialog('close');
                    $("#appInfo").attr("style", "display:none");
                    $('#tbdatagrid ').datagrid("reload");
                    alert("版本信息保存成功")
                } else {
                    alert(result.result);
                }
            }
        });
    })

    //关闭对话框
    $("#cancel").click(function () {
        $('#appInfo').dialog('close');
        $("#appInfo").attr("style", "display:none");
    })

    //修改App版本状态
    this.ChangeState = function (appID, State) {
        var msg = (State == "true" ? "禁用" : "启用");
        if (confirm("确定" + msg + "吗？")) {
            var appObj = { AppID: appID, State: State };
            $.post("?action=changestate", appObj, function (data) {
                if (data) {
                    $('#tbdatagrid ').datagrid("reload");
                } else {
                    alert("修改失败!");
                }
            });
        }
    }

    //修改激活码显示隐藏状态
    this.ChangeisEnabled = function (appID, isEnabled) {
        var msg = (isEnabled == 0 ? "显示" : "隐藏");
        if (confirm("确定" + msg + "吗？")) {
            var appObj = { AppID: appID, isEnabled: isEnabled };
            $.post("?action=changeisEnabled", appObj, function (data) {
                if (data) {
                    $('#tbdatagrid ').datagrid("reload");
                } else {
                    alert("修改失败!");
                }
            });
        }
    }

    //清空app信息列表
    this.ClearInformation = function () {
        $("#appVersion").html("");
        $("#introduce").val("");
        $("#address").val("");
        $("#verifyValue").val("");
        document.getElementById("mandatory").checked = false;
    }

}

var applicationInit;
$(function () {
    applicationInit = new ApplicationInit();
    applicationInit.Init();
});


//关闭版本更新对话框
function CloseDialog() {
    $('#div_UpdateRevision').dialog('close');
    $('#tbdatagrid ').datagrid("reload");
}

////////////////////////////
////版本自增 参数：oldVersion=当前最大版本号
////////////////////////////
Common.NewVersion = function (oldVersion) {
    //当前最大版本号尾数加1
    var maxVersion = oldVersion.replace("V", "").replace("v", "");
    var arr = new Array();
    arr = maxVersion.split(".");
    if (arr.length == 3) {
        var vera = parseInt(arr[0]), verb = parseInt(arr[1]), verc = parseInt(arr[2]);
        if (isNaN(vera) || isNaN(verb) || isNaN(verc)) {
            return "";
        }
        if (verc > 999) {
            verc = 0;
            verb += 1;
        }
        if (verb > 999) {
            verb = 0;
            vera += 1;
        }

        return vera + "." + verb + "." + verc;

    } else {
        return "";
    }

}

//将版本号转化为整数
Common.toVersion = function (version) {
    version = $.trim(version);
    if (version) {
        var version = version.replace("V", "").replace("v", "").split('.');
        if (version.length != 3) {
            return -1;
        }
        if (version[0].length > 3 || version[1].length > 3) return -1;
        for (var i = 0; i < version.length; i++) {
            if (version[i].length > 3 || version[i].length < 1) return -1;
            if (isNaN(version[i])) return -1;
            version[i] = "000" + version[i];
            version[i] = version[i].substring(version[i].length - 3);
        }

        return parseInt(version[0] + version[1] + version[2]);
    } else {
        return -1;
    }
}


function checkTextValid(str) {
    var resultTag = 0;
    var flag = 0;
    var chr = '';
    var reg = /^[^\|"'<>]*$/;
    for (var i = 0; i < str.length; i++) {
        chr = str[i];
        if (!reg.test(chr)) {
            resultTag += 1;
        }
    }
    if (resultTag == flag)
        return true;
    else {
        return false;
    }
}