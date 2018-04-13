/// <reference path="jquery-ui.js" />
$(function () {
    $.post("?action=getuserpho&Rand=" + Math.random(), "", function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            $("#UserPhone").val(result.Data);
        }
    })

    $("#aSubmit").click(function () {
        var _suggest = $.trim($("#Suggest").val());
        var _userqq = $.trim($("#UserQQ").val());
        var _userphone = $.trim($("#UserPhone").val());
        var _phonepattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[0678]|18[0-9]|14[57])[0-9]{8}$/;
        var _qqpattern = /^[1-9]*[1-9][0-9]*$/;
        if (!_suggest) {
            alert("请输入您要反馈的信息!");
            return;
        }
        if (_userqq == "" && _userphone == "") {
            alert("QQ和电话不能同时为空！");
            return;
        };
        if (_userqq != "") {
            if (!_qqpattern.test(_userqq)) {
                alert("请输入正确的QQ号码！");
                return;
            }
        }
        if (_userphone != "") {
            if (!_phonepattern.test(_userphone)) {
                alert("请输入正确的电话号码！");
                return;
            }
        }
        var obj = { Content: _suggest, UserQQ: _userqq, UserPhone: _userphone };
        $.post("?action=ProblemsFeedBack", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Data == "Success") {
                    ClearAllTxt();
                    art.dialog({
                        content: document.getElementById('dialog-form'),
                        width: 490,
                        height: 230,
                        lock: true,
                        fixed: true
                    });
                }
                else {
                    alert("您反馈的问题未提交成功，请重新提交！");
                    return;
                }
            }
        });
        $(".aL").click(function () {
            var list = art.dialog.list;
            for (var i in list) {
                list[i].close();
            };
        })
        $(".aR").click(function () {
            var list = art.dialog.list;
            for (var i in list) {
                list[i].close();
            };
            window.opener = null;
            window.close();
        })
    });

    ////提交成功 清空页面内容
    function ClearAllTxt() {
        $("#Suggest").val("");
        $("#UserQQ").val("");
        //$("#UserPhone").val("");
    }
});