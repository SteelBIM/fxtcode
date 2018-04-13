var AccountInfoPage = function () {
    var Current = this;
    this.Init = function () {
        //确认重置密码
        $("#btn_resetpassword").click(function () {
            var bootstrapValidator = $("#updateForm").data('bootstrapValidator');
            bootstrapValidator.validate();
            if (!bootstrapValidator.isValid()) {
                return;
            }
           
            $.post("/AccountInfo/UpdateUserPassword", $("#updateForm").serialize(), function (data) {
                if (data.Success) {
                    bootbox.alert("密码修改成功~");
                    $("#ResetPasswordDiv").modal("hide");
                } else {
                    bootbox.alert(data.ErrorMsg);
                }
            });
        });

        $("#btnresetPwd").click(function () {
            $("#oldpassword").val('');
            $("#updatepassword").val('');
            $("#updateconfirmPassword").val('');
            $("#ResetPasswordDiv").modal("show");
        })

        Current.ValidatorUpdateForm();
    }

    this.ValidatorUpdateForm = function () {
        $("#updateForm").bootstrapValidator({
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                OldPassword:{
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
                    }
                },
                NewPassword: {
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
                            field: 'OldPassword',
                            message: '密码不能与旧密码相同'
                        }
                    }
                },
                updateconfirmPassword: {
                    validators: {
                        message: '请确认密码',
                        notEmpty: { message: '请确认密码' },
                        identical: {
                            field: 'NewPassword',
                            message: '确认密码与密码不一至'
                        }
                    }
                }
            }
        });
    }



}



