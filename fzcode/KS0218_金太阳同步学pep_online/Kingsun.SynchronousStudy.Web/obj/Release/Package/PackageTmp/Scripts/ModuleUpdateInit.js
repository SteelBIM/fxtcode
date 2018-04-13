var ModuleUpdateInit = function () {
    var Current = this;
    this.ModuleID = 0;//模块ID
    this.ModuleName = '';
    this.TeachingNaterialName = '';
    this.FirstTitle = '';
    this.FirstTitleID = 0;
    this.SecondTitle = '';
    this.SecondTitleID = 0;
    this.BookID = 0;
    this.MaxVersionNum = 0;
    this.MaxVersionNumStr = '';
    this.State = false;

    this.Init = function () {
        Current.BookID = Common.QueryString.GetValue("BookID");
        Current.ModuleID = parseInt(Common.QueryString.GetValue("ModularID"));
        Current.FirstTitleID = Common.QueryString.GetValue("FirstTitleID");
        Current.SecondTitleID = Common.QueryString.GetValue("SecondTitleID");
        Current.GetBookDetails();
    };

    //获取书本信息
    this.GetBookDetails = function () {
        Current.ClearInformation();
        var obj = { FirstTitleID: Current.FirstTitleID, SecondTitleID: Current.SecondTitleID, BookID: Current.BookID };
        $.post("?action=GetBookDetails", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")")
                Current.BookID = result.data.BookID;
                Current.FirstTitle = result.data.FirstTitle;
                Current.SecondTitle = result.data.SecondTitle;
                Current.TeachingNaterialName = result.data.TeachingNaterialName;
                $("#courseDetails").html(result.data.TeachingNaterialName);
                $("#unitDetails").html(result.data.FirstTitle);
                $("#moduleDetails").html(result.data.SecondTitle);
                Current.GetModuleDetails();
            } else {
                alert("书本信息获取失败");
            }
        });
    }

    $("#cancel").click(function () {
        window.parent.CloseDialog();
    })

    //获取模块所属信息
    this.GetModuleDetails = function () {
        var obj = { BookID: Current.BookID, FirstTitleID: Current.FirstTitleID, SecondTitleID: Current.SecondTitleID, ModuleID: Current.ModuleID };
        $.post("?action=GetModuleDetails", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")")
                Current.ModuleName = result.data.ModuleName;
            } else {
                alert("模块信息获取失败");
            }
        });
    }

    //获取最大版本号
    this.GetMaxVersionNum = function () {
        var obj = { BookID: Current.BookID, FirstTitleID: Current.FirstTitleID, SecondTitleID: Current.SecondTitleID, ModuleID: Current.ModuleID };
        //var obj = { TeachingNaterialName: Current.TeachingNaterialName, FirstTitle: Current.FirstTitle, SecondTitle: Current.SecondTitle, ModuleName: Current.ModuleName };
        $.ajax({
            url: "?action=getMaxVersionNum",
            type: "POST",
            data: obj,
            async: false,
            success: function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    var versionNumArr = [];
                    for (var i = 0; i < result.rows.length; i++) {
                        versionNumArr.push(Common.toVersion(result.rows[i].ModuleVersion));
                    }
                    for (var i = 0; i < versionNumArr.length; i++) {
                        if (i == 0) {
                            Current.MaxVersionNum = versionNumArr[i];
                            Current.MaxVersionNumStr = result.rows[i].ModuleVersion;
                        } else {
                            if (versionNumArr[i] >= Current.MaxVersionNum) {
                                Current.MaxVersionNum = versionNumArr[i];
                                Current.MaxVersionNumStr = result.rows[i].ModuleVersion;
                            }
                        }
                    }
                }
            }
        })
    }

    //清空文本框信息
    this.ClearInformation = function () {
        $("#courseDetails").html('');
        $("#unitDetails").html('');
        $("#moduleDetails").html('');
    }

    //模块更新
    $("#confirm").click(function () {
        //用来防止多次提交
        if (Current.State) {
            return false;
        }
        Current.State = true;
        Current.GetMaxVersionNum();
        var moduleAddress = $.trim($("#moduleAddress").val());
        if (moduleAddress != '') {
            moduleAddress = 'http://' + moduleAddress;
        }
        var moduleMD5 = $("#moduleMD5").val();
        var addModuleAddress = $.trim($("#addModuleAddress").val());
        if (addModuleAddress != '') {
            addModuleAddress = 'http://' + addModuleAddress;
        }
        var addModuleMD5 = $("#addModuleMD5").val();
        var moduleVersion = $("#moduleVersion").val();
        var description = $.trim($("#description").val());
        var flag = document.getElementById("mandatory").checked;
        var newVersion = "V" + moduleVersion;
        var result = Common.NewVersion(newVersion);
        var urlReg = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
        var mandatoryUpdate = false;
        if (flag) {
            mandatoryUpdate = true;
        }
        if (moduleAddress == "") {
            Current.State = false;
            alert('模块地址不能为空');
            return false;
        }
        if (moduleMD5 == "") {
            Current.State = false;
            alert('模块MD5值不能为空');
            return false;
        }
        if (moduleVersion == "") {
            Current.State = false;
            alert('模块版本号不能为空');
            return false;
        }
        if (result == "") {
            Current.State = false;
            alert("模块版本号格式错误");
            return;
        }
        if (result == "超过最大值") {
            Current.State = false;
            alert("版本号超过最大版本号(999.999.999)");
            return;
        }
        if (Common.toVersion(newVersion) <= Current.MaxVersionNum) {
            Current.State = false;
            alert('该版本号比最大版本号小，请重新输入版本号');
            return false;
        }
        moduleVersion = result;
        if (description == "") {
            Current.State = false;
            alert('更新描述不能为空');
            return false;
        }
        if (!urlReg.test(moduleAddress)) {
            Current.State = false;
            alert("请输入正确的模块地址");
            return;
        }
        var suffix = moduleAddress.substring((moduleAddress.length - 3), (moduleAddress.length)).toLowerCase();
        if (suffix != "zip" && suffix != "apk") {
            Current.State = false;
            alert("模块地址不存在，请检查");
            return;
        }
        if (Current.MaxVersionNum != 0) {
            var newVersionArr = VersionArr(newVersion);
            var maxVersionArr = new Array();
            maxVersionArr = VersionArr(Current.MaxVersionNumStr);
            if (newVersionArr[0] == maxVersionArr[0] && newVersionArr[1] == maxVersionArr[1]) {
                if (addModuleAddress == "") {
                    Current.State = false;
                    alert("增量包地址不能为空");
                    return;
                }
                if (addModuleMD5 == "") {
                    Current.State = false;
                    alert('增量包MD5值不能为空');
                    return false;
                }
            }
        }

        if (addModuleAddress != "" && !urlReg.test(addModuleAddress)) {
            Current.State = false;
            alert("请输入正确的增量包地址");
            return;
        }
        suffix = addModuleAddress.substring((addModuleAddress.length - 3), (addModuleAddress.length)).toLowerCase();
        if (addModuleAddress != "" && suffix != "zip" && suffix != "apk") {
            Current.State = false;
            alert("增量包地址不存在");
            return;
        }
        var obj = {
            BookID: Current.BookID,
            ModuleID: Current.ModuleID,
            ModuleName: Current.ModuleName,
            TeachingNaterialName: Current.TeachingNaterialName,
            FirstTitle: Current.FirstTitle,
            FirstTitleID: Current.FirstTitleID,
            SecondTitle: Current.SecondTitle,
            SecondTitleID: Current.SecondTitleID,
            ModuleAddress: moduleAddress,
            ModuleMD5: moduleMD5,
            AddModuleAddress: addModuleAddress,
            AddModuleMD5: addModuleMD5,
            ModuleVersion: moduleVersion,
            Description: description,
            MandatoryUpdate: mandatoryUpdate
        };
        $.post("?action=updateModule", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")")
                if (result.obj) {
                    alert("模块更新成功");
                    Current.State = false;
                    window.parent.CloseDialog();
                } else {
                    Current.State = false;
                    alert(result.result);
                }
            } else {
                Current.State = false;
                alert("请求发送失败，请重试");
            }
        });
    })
}

var moduleUpdateInit;
$(function () {
    moduleUpdateInit = new ModuleUpdateInit();
    moduleUpdateInit.Init();
});

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
        if (vera > 999) {
            return "超过最大值";
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
        if (version[0].length > 3 || version[1].length > 3) {
            return -1;
        }
        for (var i = 0; i < version.length; i++) {
            if (version[i].length > 3 || version[i].length < 1) return -1;
            if (isNaN(version[i])) return -1;
            version[i] = "000" + version[i];
            version[i] = version[i].substring(version[i].length - 3);
        }
        var num = parseInt(version[0] + version[1] + version[2]);
        return num;
    } else {
        return -1;
    }
}

////////////////////////////
////返回版本号数组(版本号规则验证之前完成)
////////////////////////////
function VersionArr(Version) {
    var version = Version.replace("V", "").replace("v", "");
    var arr = new Array();
    arr = version.split(".");
    return arr;
}