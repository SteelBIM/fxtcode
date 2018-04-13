
$("#group").change(function () {
    var groupId = $("#group").find("option:selected").val();
    if (groupId == 4 || groupId == 5 || groupId == 6) {
        $("#areadiv").show();
    } else {
        $("#areadiv").hide();
    }
});


$(function () {
    //编辑 
    var masterId = getQueryString("Id");
    if (masterId > 0) {
        $("#selectdeptbtn").hide();
        $("#ResetPassword").show();
        $("div [name='passwordupdate']").hide();
        $.post("/Employee/GetMasterById", { "Id": masterId }, function (jsondata) {
            if (jsondata != null && jsondata != "") {
                $("#username").val(jsondata.mastername);
                $("#masterusername").val(jsondata.mastername);
                $("#username").attr("disabled", true);
                $("#reset-mastername").html(jsondata.mastername);
                $("#ParentMobile").val(jsondata.mobile);
                $("#Email").val(jsondata.email);
                $("#Remark").val(jsondata.remark);
                $("#role").val(jsondata.groupid);
                if (jsondata.issend == 1) {
                    $("#sendmsg").prop("checked", "checked");
                }
                if (jsondata.issend == 2) {
                    $("#sendmail").prop("checked", "checked");
                }
                if (jsondata.issend == 3) {
                    $("#sendmail").prop("checked", "checked");
                    $("#sendmsg").prop("checked", "checked");
                }
            }
        });
    } else {
        masterId = 0;
    }

    $("#submit").click(function () {//提交 
        var bootstrapValidator = $("#userForm").data('bootstrapValidator');
        bootstrapValidator.validate();
        if (!bootstrapValidator.isValid()) {
            return;
        }
        var username = $.trim($("#username").val());//用户名
        var confirmPassword = $.trim($("#confirmPassword").val());//密码
        var ParentMobile = $.trim($("#ParentMobile").val());//手机号码
        var Email = $.trim($("#Email").val());//电子邮件
        var Remark = $("#Remark").val();//备注
        var issend = "";
        var role = $("#role").val();
        if (role == "" || role == null) {
            bootbox.alert("角色不能为空！"); return false;
        }
        if (document.getElementById("sendmail").checked || document.getElementById("sendmsg").checked) {
            if (document.getElementById("sendmail").checked && document.getElementById("sendmsg").checked) {
                if (Email == "" || ParentMobile == "") {
                    bootbox.alert("邮箱和手机号不能为空！"); return false;
                }
                else issend = 3;
            }
            else {
                if (document.getElementById("sendmail").checked)
                    if (Email == "") {
                        bootbox.alert("邮箱不能为空！"); return false;
                    }
                    else issend = 2;
                if (document.getElementById("sendmsg").checked)
                    if (ParentMobile == "") {
                        bootbox.alert("手机号不能为空！");
                        return false;
                    }
                    else issend = 1;
            }
        }
        else { issend = 0 }
        addcloud();
        $.post("/Employee/UserNameIsExist", { "UserId": masterId, "UserName": username }, function (IsExist) {
            if (IsExist > 0) {
                removecloud();
                bootbox.alert("用户名已存在~");
            } else {
                if (masterId > 0) {
                    if ($("#isresetpassword").val() == 0) {
                        confirmPassword = "";
                    }
                    $.post("/Employee/Employee_Edit", { "mastername": username, "mobile": ParentMobile, "email": Email, "agent_remark": Remark, "issend": issend, "groupid": role }, function (data) {
                        if (data.Success) {
                            removecloud();
                            bootbox.setDefaults("locale", "zh_CN");
                            bootbox.alert("编辑成功~", function (result) {
                                location.href = "/Employee/Index";
                            });
                        } else {
                            removecloud();
                            bootbox.alert(data.ErrorMsg);
                        }
                    });
                } else {
                    var data = {
                        masterId: masterId, mastername: username, password: confirmPassword, mobile: ParentMobile, email: Email,
                        channel: 0, state: 0, agent_remark: Remark, issend: issend, groupid: role
                    };

                    $.post("/Employee/Employee_Add", { "jsondata": JSON.stringify(data) }, function (data) {
                        if (data.Success) {
                            removecloud();
                            bootbox.setDefaults("locale", "zh_CN");
                            bootbox.alert("添加成功~", function (result) {
                                location.href = "/Employee/Index";
                            });
                        } else {
                            removecloud();
                            bootbox.alert(data.ErrorMsg);
                        }
                    });
                }
            }
        });
    });

    if (masterId > 0) {
        $("#updateForm").bootstrapValidator({
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                updatepassword: {
                    message: '请输入6-20个字符的密码',
                    validators: {
                        notEmpty: { message: '请输入6-20个字符的密码' },
                        stringLength: {
                            min: 6,
                            max: 20,
                            message: '密码长度必须大于6且小于20个字符'
                        },
                        regexp: {
                            regexp: /^[a-zA-Z0-9_\.]+$/,
                            message: '密码只能由字母，数字，点和下划线组成'
                        },
                        different: {
                            field: 'masterusername',
                            message: '密码不能与用户名相同'
                        }
                    }
                },
                updateconfirmPassword: {
                    validators: {
                        message: '请确认密码',
                        notEmpty: { message: '请确认密码' },
                        identical: {
                            field: 'updatepassword',
                            message: '确认密码与密码不一至'
                        },
                        different: {
                            field: 'masterusername',
                            message: '密码不能与用户名相同'
                        }
                    }
                }
            }
        });
    }

    $("#userForm").bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            username: {
                message: '请输入5-20个字符的用户名',
                validators: {
                    notEmpty: { message: '请输入5-20个字符的用户名' },
                    stringLength: {
                        min: 5,
                        max: 20,
                        message: '用户名长度必须大于5且小于20个字符'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_\.]+$/,
                        message: '用户名只能由字母，数字，点和下划线组成'
                    },
                    different: {
                        field: 'password',
                        message: '用户名和密码不能相同'
                    }
                }
            },
            password: {
                message: '请输入6-20个字符的密码',
                validators: {
                    notEmpty: { message: '请输入6-20个字符的密码' },
                    stringLength: {
                        min: 6,
                        max: 20,
                        message: '密码长度必须大于6且小于20个字符'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_\.]+$/,
                        message: '密码只能由字母，数字，点和下划线组成'
                    },
                    different: {
                        field: 'username',
                        message: '密码不能与用户名相同'
                    }
                }
            },
            confirmPassword: {
                validators: {
                    message: '请确认密码',
                    notEmpty: { message: '请确认密码' },
                    identical: {
                        field: 'password',
                        message: '确认密码与密码不一至'
                    },
                    different: {
                        field: 'username',
                        message: '密码不能与用户名相同'
                    }
                }
            },
            ParentMobile: {
                message: '请输入手机号',
                validators: {
                    regexp: {
                        regexp: /^1[3|4|5|7|8][0-9]\d{8}$/,
                        message: '请输入正确的手机号'
                    }
                }
            },
            Email: {
                message: '请输入电子邮件',
                validators: {
                    regexp: {
                        regexp: /^[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?$/,
                        message: '请输入正确的电子邮件'
                    }
                }
            },
            Remark: {
                message: '请输入备注,最多600个字符',
                validators: {
                    regexp: {
                        regexp: '^[\u4E00-\u9FA5A-Za-z0-9_]+$',
                        message: '备注不能有特殊符号'
                    },
                    stringLength: {
                        min: 0,
                        max: 600,
                        message: '备注长度在600个汉字内'
                    }
                }
            }
        }
    });
});


//重置密码
$("#ResetPassword").click(function () {
    $("#ResetPasswordDiv").modal("show");
});

//确认重置密码
$("#btn_resetpassword").click(function () {
    var bootstrapValidator = $("#updateForm").data('bootstrapValidator');
    bootstrapValidator.validate();
    if (!bootstrapValidator.isValid()) {
        return;
    }
    $.post("/Employee/UpdatePossword", { "MasterName": $("#masterusername").val(), "Possword": $("#updateconfirmPassword").val() }, function (data) {
        if (data.Success) {
            bootbox.alert("密码修改成功~");
            $("#ResetPasswordDiv").modal("hide");
        } else {
            bootbox.alert(data.ErrorMsg);
        }
    });
});







