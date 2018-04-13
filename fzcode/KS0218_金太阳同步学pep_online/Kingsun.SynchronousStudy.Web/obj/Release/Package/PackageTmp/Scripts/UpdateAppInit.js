var UpdateAppInit = function () {
    var Current = this;
    this.VersionID = 0;
    this.IOSMaxNum = 0; // IOS 最大版本号
    this.AndroidMaxNum = 0;// Android 最大版本号

    this.Init = function () {
        Current.VersionID = parseInt(Common.QueryString.GetValue("VersionID"));
        Current.GetMaxVersionNum();
        var username = $("#userName").html();
        $("#uploadPerson").html(username);
        $("#makeSure").click(function () {
            var checked = document.getElementById("IOS").checked;
            var versionType = "";
            if (checked) {
                versionType = "IOS";
            } else {
                versionType = "Android";
            }
            var versionNum = $("#appVersion").val();
            var content = $.trim($("#introduce").val());
            var name = $("#uploadPerson").html();
            var address = $.trim($("#address").val());
            if (address != '') {
                address = 'http://' + address;
            }
            var verifyValue = $("#verifyValue").val();
            var flag = document.getElementById("mandatory").checked;
            var newVersion = "V" + versionNum;
            var result = Common.NewVersion(newVersion);
            var urlReg = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
            var mandatoryUpdate = false;
            if (flag) {
                mandatoryUpdate = true;
            }
            if (versionNum == "") {
                alert('版本号不能为空！');
                return false;
            }
            if (result == "") {
                alert("版本号格式错误");
                return;
            }
            if (result == "超过最大值") {
                alert("版本号超过最大版本号(999.999.999)");
                return;
            }
            if (versionType == "IOS") {
                if (Common.toVersion(newVersion) <= Current.IOSMaxNum) {
                    alert('该版本号小于最大版本号，请重新输入版本号');
                    return false;
                }
            }
            if (versionType == "Android") {
                if (Common.toVersion(newVersion) <= Current.AndroidMaxNum) {
                    alert('该版本号小于最大版本号，请重新输入版本号');
                    return false;
                }
            }
            versionNum = result;
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
            var obj = { VersionType: versionType, VersionNum: versionNum, VersionID: Current.VersionID, Content: content, Name: name, Address: address, VerifyValue: verifyValue, MandatoryUpdate: mandatoryUpdate };
            $.post("?action=updateApp", obj, function (data) {
                if (data) {
                    var result = eval("(" + data + ")")
                    if (result.obj) {
                        alert("版本更新成功");
                        //调用父页面函数，关闭对话框
                        window.parent.CloseDialog();
                    } else {
                        alert(result.result);
                    }
                } else {
                    alert("请求发送失败，请重试");
                }
            });
        })


        //点击取消按钮返回应用列表页面
        $("#cancel").click(function () {
            //调用父页面函数，关闭对话框
            window.parent.CloseDialog();
        })
    };

    //获取最大版本号
    this.GetMaxVersionNum = function () {
        var queryStr = " 1=1 ";
        queryStr += " and  VersionID =" + Current.VersionID;
        $.post("?action=getMaxVersionNum&queryStr=" + encodeURI(queryStr), function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                var IOSNumArr = [];
                var AndroidNumArr = [];
                if (result.IOSList != null && result.IOSList.length > 0) {
                    for (var i = 0; i < result.IOSList.length; i++) {
                        IOSNumArr.push(Common.toVersion(result.IOSList[i].VersionNumber));
                    }
                }
                if (result.AndroidList != null && result.AndroidList.length > 0) {
                    for (var i = 0; i < result.AndroidList.length; i++) {
                        AndroidNumArr.push(Common.toVersion(result.AndroidList[i].VersionNumber));
                    }
                }
                for (var i = 0; i < IOSNumArr.length; i++) {
                    if (i == 0) {
                        Current.IOSMaxNum = IOSNumArr[i];
                    } else {
                        if (IOSNumArr[i] > Current.IOSMaxNum) {
                            Current.IOSMaxNum = IOSNumArr[i];
                        }
                    }
                }
                for (var i = 0; i < AndroidNumArr.length; i++) {
                    if (i == 0) {
                        Current.AndroidMaxNum = AndroidNumArr[i];
                    } else {
                        if (AndroidNumArr[i] > Current.AndroidMaxNum) {
                            Current.AndroidMaxNum = AndroidNumArr[i];
                        }
                    }
                }
            }
        });
    }
}

var updateAppInit;
$(function () {
    updateAppInit = new UpdateAppInit();
    updateAppInit.Init();
});


////////////////////////////
////检验版本号格式是否正确
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