
function GetSelectArea() {//获取已选区域
    var areaids = "";
    var productchecked = $("#ProductHtml input[type='checkbox']:checked");
    if (productchecked.length > 0) {
        if ($("#Same").is(':checked')) {
            areaids = $("#EmployeeDeptAreaIds").val();
        } else {
            for (var i = 0; i < productchecked.length; i++) {
                areaids += $("#EmployeeDeptAreaIds" + $(productchecked[i]).val()).val();
            }
        }
    }
    return areaids;
}

$("#btn_confirmemplloyArea").click(function () {
    var ts = $('#treeview-checkable-checked').treeview("getAllNode");
    if (ts.length <= 1) {
        bootbox.alert("请选择区域。");
    } else {
        var SelectProductId = $("#SelectProductId").val();
        var parentid = $("#p4deptid").val();//新增节点的父id,是父节点的deptid
        var areas = "";
        var districtid = "";
        var node = $("#treeview-checkable-checked").treeview('getNode', 0);
        if (node.nodes) {
            for (z in node.nodes) {
                areas += "<p>" + node.nodes[z].text + "</p>";
                districtid += node.nodes[z].tag + "|" + SelectProductId + "@";
            }
        }
        var p4deptid = $.trim($("#p4deptid").val());
        //if (districtid != "") {
        //    $.post("/Employee/JudgmentSelectedArea", { Area: districtid, mastername: mastername, deptid: p4deptid }, function (data) {
        //        if (data.Success) {
        //            bootbox.alert(data.ErrorMsg);
        //        }
        //    });
        //}

        $("#ProductHtml input[type='checkbox']:checked").each(function (index, item) {
            if (districtid != null && districtid != "") {
                if (SelectProductId.indexOf($(item).val()) != -1) {
                    $(this).attr("disabled", true);
                }
            } else {
                if (SelectProductId.indexOf($(item).val()) != -1) {
                    $(this).attr("disabled", false);
                }
            }
        });
        if ($("#Same").is(':checked')) {
            $("#Area").html(areas);
            $("#EmployeeDeptAreaIds").val(districtid);
        } else {
            $("#Area" + SelectProductId).html(areas);
            $("#EmployeeDeptAreaIds" + SelectProductId).val(districtid);
        }
        $('#setAreaModal').modal('hide');
    }
});

var masterId = getQueryString("Id");

function GetProductKeys() {//获取已选产品
    var ProductKeys = "";
    var productchecked = $("#ProductHtml input[type='checkbox']:checked");
    if (productchecked.length > 0) {
        for (var i = 0; i < productchecked.length; i++) {
            ProductKeys += $(productchecked[i]).val() + ",";
        }
    }
    return ProductKeys;
}
$(function () {
    $.post("/Employee/GetProduct", function (Product) {//加载产品
        if (Product != null) {
            var jsonproduct = eval(Product);
            var ProductHtml = "";
            var ProductAreaHtml = "";//产品选择区域
            $(jsonproduct).each(function (index, item) {
                ProductHtml += "<span style=\"float:left;margin-left:20px;\"> <input  type=\"checkbox\"   title=\"" + item.Value + "\"  value=\"" + item.Key + "\" id=\"" + item.Key + "\"  /><label style=\"margin-left:10px; font-size:15px;font-weight:500;color:#23527c;   \" for=\"" + item.Key + "\">" + item.Value + "</label> </span>";
                ProductAreaHtml += "<div   id=\"ProductArea" + item.Value + "\" style=\'clear:both; height:10px;display:none;\'></div>";
                ProductAreaHtml += " <input id=\'EmployeeDeptAreaIds" + item.Key + "\' type=\'hidden\'   />";
                ProductAreaHtml += "<div class=\"AreaNone\" id=\"AreaNone" + item.Key + "\" style=\"display:none;\"> <div  id=\'Area" + item.Key + "\' class=\'col-lg-5\' style=\'width:500px;   height:120px;  margin-top:-30px;   padding:10px;  margin-left:140px; border:1px solid #ccc; overflow:auto;\'>";
                ProductAreaHtml += "</div>";
                ProductAreaHtml += "<span style=\' margin-top:-70px; margin-left:656px; width:120px; float:left;\'>" + item.Value + "</span>";
                ProductAreaHtml += "<input style=\'width:80px;  margin-top:-50px;  margin-left:656px;\' type=\'button\' onclick=\'selectdeptarea(" + item.Key + ",1);\' value=\'选择区域\' class=\'btn btn-primary\' /></div>";
                ProductAreaHtml += "<div style=\'clear:both; height:10px;\'></div>";
            });
            $("#ProductHtml").html(ProductHtml);
            $("#sponsibleareahtml").html(ProductAreaHtml);
        }
    });
    $("#ProductHtml").change(function () {
        var productchecked = $("#ProductHtml input[type='checkbox']:checked");
        $(".AreaNone").hide();
        if (productchecked.length == 1) {
            $(productchecked).each(function () {
                $("#EmployeeDeptAreaIds").val($("#EmployeeDeptAreaIds" + $(this).val()).val());
                $("#Area").html($("#Area" + $(this).val()).html());
            });
            $("#productresponsiblearea").hide();
            $("#Same").prop("checked", true);
        }
        else if (productchecked.length > 1) {
            $("#productresponsiblearea").show();
        }
        Same();
    });
    $("#Same").change(function () {
        Same();
    });
    $("#NotSame").change(function () {
        var productchecked = $("#ProductHtml input[type='checkbox']:checked");
        if (productchecked.length == 1) {
            $(productchecked).each(function () {
                $("#EmployeeDeptAreaIds").val($("#EmployeeDeptAreaIds" + $(this).val()).val());
                $("#Area").html($("#Area" + $(this).val()).html());
            });
        }
        Same();
    });
    //编辑 

    if (masterId > 0) {
        $("#myModalLabel").text("编辑公司用户");
        $("#ResetPassword").show();
        $("div [name='passwordupdate']").hide();
        addcloud();
        $.post("/Employee/GetMasterById", { "Id": masterId }, function (jsondata) {
            if (jsondata != null && jsondata != "") {
                $.post("/Employee/GetMasterAgentNumber", { "Id": masterId }, function (AgentNumber) {
                    if (AgentNumber != null && AgentNumber > 0) {
                        $("#selectdeptbtn").hide();
                        $("#HideComment").html(" <input type=\"button\" onclick=\"NotDepart();\" value=\"选择部门\" id=\"selectdeptbtn\" class=\"btn btn-primary\" style=\"width:80px; margin-top:-55px;\" />");
                    } else {
                        $("#selectdeptbtn").show();
                        $("#HideComment").html("");
                    }
                });
                $("#username").val(jsondata.mastername);
                $("#masterusername").val(jsondata.mastername);
                $("#username").attr("disabled", true);
                $("#reset-mastername").html(jsondata.mastername);
                $("#Name").val(jsondata.truename);
                $("#ParentMobile").val(jsondata.mobile);
                $("#QQNo").val(jsondata.qq);
                $("#Email").val(jsondata.email);
                $("#Remark").val(jsondata.agent_remark);
                $("#DeptId").val(jsondata.deptid);
                $.post("/GiveUpActionAuthority/GetDeptById", { "Id": jsondata.deptid }, function (deptdata) {
                    if (deptdata != null && deptdata != "") {
                        $("#DeptName").val(deptdata[0].deptname);
                        $("#DeptParentid").val(deptdata[0].parentid);
                        $("#p4deptid").val(jsondata.deptid);
                        $("#p4parentid").val(deptdata[0].parentid);
                    }
                });
                if (jsondata.groupid == 4 || jsondata.groupid == 5 || jsondata.groupid == 6) {
                    $.post("/GiveUpActionAuthority/GetAreaByMasterName", { "mastername": jsondata.mastername }, function (areadata) {
                        if (areadata != null && areadata != "") {
                            var jsonareadata = eval(areadata);
                            var issame = true;
                            for (var i = 0; i < jsonareadata.length; i++) {
                                if (jsonareadata[0].productkeys != jsonareadata[i].productkeys) {
                                    issame = false;
                                    break;
                                }
                            }
                            if (issame) {//所有产品区域都相同的时候
                                var productkeys = TrimStateChar(jsonareadata[0].productkeys, ",");
                                productkeys = TrimEndChar(jsonareadata[0].productkeys, ",");
                                if (productkeys.indexOf(",") != -1) {// 是否包含多个产品
                                    $("#productresponsiblearea").show();
                                }

                                var AreaIds = "";
                                var AreaNames = "";
                                $(eval(areadata)).each(function (i, item) {
                                    AreaIds += item.tag + "@";
                                    AreaNames += "<br / >" + item.text;
                                });
                                $("#EmployeeDeptAreaIds").val(AreaIds);
                                $("#Area").html(AreaNames);
                                var products = "," + productkeys + ",";
                                $("#ProductHtml input[type='checkbox']").each(function (index, item) {//勾选负责的产品
                                    if (products.indexOf("," + $(this).val() + ",") != -1) {
                                        $(this).prop('checked', true);
                                        $(this).attr("disabled", true);
                                        $("#EmployeeDeptAreaIds" + $(this).val()).val(AreaIds);
                                        $("#Area" + $(this).val()).html(AreaNames);
                                    }
                                });
                                $("#Same").prop('checked', true);//选中产品负责区域相同（默认）
                                $("#sponsiblearea").show();//显示区域

                                UpdateAreaView(0, areadata, jsondata.mastername);//呈现选中区域
                            } else {//所负责产品区域不相同的时候
                                $("#NotSame").prop('checked', true);//所选产品负责区域不相同（默认)
                                var sponsibleareahtml = "";//区域选框
                                var allproduct = "";
                                for (var i = 0; i < jsonareadata.length; i++) {
                                    allproduct += jsonareadata[i].productkeys + ",";
                                }
                                var productIds = ",";
                                var array = allproduct.split(',');
                                if (array != null && array.length > 0)
                                {
                                    for (var a = 0; a < array.length; a++)
                                    {
                                        if (array[a] != null && array[a]!="")
                                        {
                                            if (productIds.indexOf("," + array[a] + ",") == -1)
                                                productIds += array[a] + ",";
                                        }
                                    }
                                }
                                allproduct = productIds;
                                $("#ProductHtml input[type='checkbox']").each(function (index, item) {//勾选负责的产品
                                   
                                    var productkeys = $(this).val();
                                  
                                    if (allproduct.indexOf("," + productkeys + ",") != -1) {
                                        $(this).attr("disabled", true);
                                        $(this).prop('checked', true);
                                        $.post("/GiveUpActionAuthority/GetMasterNameArea", { "mastername": jsondata.mastername, "productkeys": productkeys }, function (productarea) {
                                            var AreaIds = "";
                                            var AreaNames = "";
                                            if (productarea != null && productarea != "") {
                                                $(eval(productarea)).each(function (i, item) {
                                                    AreaIds += item.tag + "@";
                                                    AreaNames += "<br / >" + item.text;
                                                });
                                            }
                                            $("#EmployeeDeptAreaIds" + productkeys).val(AreaIds);
                                            $("#Area" + productkeys).html(AreaNames);
                                            $("#AreaNone" + productkeys).show();
                                            $("#ProductArea" + productkeys).show();
                                        });
                                    }
                                });
                                $("#productresponsiblearea").show();//呈现所选产品负责区域
                                $("#sponsibleareas").show();//显示区域
                            }
                        }
                    });
                } else {
                    $("#areadiv").hide();
                }
                $.post("/Employee/GetAllGroup", function (data) {
                    if (data != null && data != "") {
                        $("#group").empty();
                        $.each(data, function (index, item) {
                            if (item.groupid != 6) {
                                var opt = $("<option accesskey=\"" + item.dataauthority + "\">").text(item.groupname).val(item.groupid);
                                $("#group").append(opt);
                            }
                        });
                        var mr = $("<option accesskey=\"0\" selected=selected>").text("选择角色").val("");
                        $("#group").append(mr);
                        $("#group").val(jsondata.groupid);
                        removecloud();
                    }
                });
            }
        });
    } else {
        masterId = 0;
        GroupDropdownList();//绑定角色
    }

    $("#submit").click(function () {//提交 
        var bootstrapValidator = $("#userForm").data('bootstrapValidator');
        bootstrapValidator.validate();
        if (!bootstrapValidator.isValid()) {
            return;
        }
        var username = $.trim($("#username").val());//用户名
        var confirmPassword = $.trim($("#confirmPassword").val());//密码
        var Name = $.trim($("#Name").val());//真实姓名
        var ParentMobile = $.trim($("#ParentMobile").val());//手机号码
        var Email = $.trim($("#Email").val());//电子邮件
        var QQNo = $.trim($("#QQNo").val());//QQ号
        var groupid = $("#group").val();//角色
        var dataauthority = $("#group").find("option:selected").attr("accesskey");//数据操作权限
        var DeptId = $("#DeptId").val();//部门
        if (DeptId == undefined || DeptId == "" || DeptId == 0) {
            bootbox.alert("请选择部门~");
            return false;
        }
        var AreaIds = GetSelectArea();// $("#EmployeeDeptAreaIds").val();//负责区域
        var groupId = $("#group").find("option:selected").val();
        if (groupId != 4 && groupId != 5 && groupId != 6) {
            AreaIds = ""
        }

        var productkeys = "";
        if ($("#Same").is(':checked')) {
            productkeys = GetProductKeys();
        }
        var Remark = $("#Remark").val();//备注
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
                    $.post("/Employee/Employee_Edit", { "masterId": masterId, "mastername": username, "truename": Name, "mobile": ParentMobile, "email": Email, "qq": QQNo, "deptid": DeptId, "groupid": groupid, "agent_remark": Remark, "dataauthority": dataauthority, "Areas": AreaIds, "productkeys": productkeys }, function (data) {
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
                        masterId: masterId, mastername: username, password: confirmPassword, truename: Name, mobile: ParentMobile, email: Email, qq: QQNo, deptid: DeptId, groupid: groupid,
                        channel: 0, state: 0, agent_remark: Remark,
                        dataauthority: dataauthority, mastertype: 0
                    };

                    $.post("/Employee/Employee_Add", { "jsondata": JSON.stringify(data), "Areas": AreaIds, "productkeys": productkeys }, function (data) {
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
                message: '请输入6-20个字符的用户名',
                validators: {
                    notEmpty: { message: '请输入6-20个字符的用户名' },
                    stringLength: {
                        min: 6,
                        max: 20,
                        message: '用户名长度必须大于6且小于20个字符'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_\.]+$/,
                        message: '用户名只能由字母，数字，点和下划线组成'
                    },
                    //threshold: 6, //有6字符以上才发送ajax请求，（input中输入一个字符，插件会向服务器发送一次，设置限制，6字符以上才开始）
                    //remote: {//ajax验证。server result:{"valid",true or false} 向服务发送当前input name值，获得一个json数据。例表示正确：{"valid",true}  
                    //    url: '/Employee/UserNameIsExist',//验证地址
                    //    message: '用户已存在',//提示消息
                    //    delay: 1000,//每输入一个字符，就发ajax请求，服务器压力还是太大，设置2秒发送一次ajax（默认输入一个字符，提交一次，服务器压力太大）
                    //    type: 'POST',//请求方式
                    //    /**自定义提交数据，默认值提交当前input value**/
                    //    data:{                 //传参数
                    //        username:function(){return $("#username").val()},    //username参数名，不用引号，如果想传入特定控件的值一定加入function,这样才能把值传入，不知道为什么……
                    //    },
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
            Name: {
                message: '请输入姓名',
                validators: {
                    notEmpty: {
                        message: '请输入姓名'
                    },
                    regexp: {
                        regexp: /^[\u4e00-\u9fa5_a-zA-Z0-9_\s]{1,10}$/,
                        message: '姓名长度不能超过10个汉字'
                    }
                }
            },
            ParentMobile: {
                message: '请输入手机号',
                validators: {
                    notEmpty: {
                        message: '请输入手机号'
                    },
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
            QQNo: {
                message: '请输入QQ号',
                validators: {
                    regexp: {
                        regexp: /^[1-9][0-9]{4,12}$/,
                        message: '请输入正确的QQ号'
                    }
                }
            }, group: {
                message: '请选择角色',
                validators: {
                    notEmpty: {
                        message: '请选择角色'
                    }
                }
            }, Remark: {
                message: '请输入备注,最多600个字符',
                validators: {
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

//角色绑定
function GroupDropdownList() {
    $.post("/Employee/GetAllGroup", function (data) {
        if (data != null && data != "") {
            $("#group").empty();
            $.each(data, function (index, item) {
                if (item.groupid != 6) {
                    var opt = $("<option accesskey=\"" + item.dataauthority + "\">").text(item.groupname).val(item.groupid);
                    $("#group").append(opt);
                }
            });
            var mr = $("<option accesskey=\"0\" selected=selected>").text("选择角色").val("");
            $("#group").append(mr);
        }
    });
}

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

//选择部门
function selectDepart() {
    if ($("#EmployeeDeptAreaIds").val() != "") {
        bootbox.setDefaults("locale", "zh_CN");
        bootbox.confirm("重置部门将会清空已选区域~ 请确认！ ", function (result) {
            if (result) {
                $("#EmployeeDeptAreaIds").val("");//地区ID为为空
                $("#Area").html("");
                $('#treeview-checkable-checked').treeview({
                    showIcon: false,
                    showCheckbox: true
                });
                $.ajax({
                    type: "Post",
                    url: "/Employee/SelectDept",
                    dataType: "json",
                    success: function (result) {
                        if (result != null && result != "") {
                            $('#tree').treeview({
                                data: result,
                                multiSelect: false,
                                highlightSelected: true,    //是否高亮选中
                                onhoverColor: "#E8E8E8",
                                selectable: true,
                                onNodeSelected: function (event, data) {
                                    var deptid = data.tag;
                                    if (data.createname == 0) {
                                        bootbox.alert("请不要选择基础部门~");
                                        return false;
                                    }
                                    $("#DeptId").val(deptid);
                                    $("#DeptName").val(data.text);
                                    $('#addModal').modal('hide');
                                    $("#DeptParentid").val(data.ParentId);
                                    $("#p4deptid").val(deptid);
                                    $("#p4parentid").val(data.ParentId);
                                }
                            });
                            $('#addModal').modal('show');
                        } else {
                            bootbox.alert("未找到部门信息！请您先添加部门~")
                        }
                    },
                    error: function () {
                        bootbox.alert("加载部门失败！")
                    }
                });
            }
        });
    } else {
        $.ajax({
            type: "Post",
            url: "/Employee/SelectDept",
            dataType: "json",
            success: function (result) {
                if (result != null && result != "") {
                    $('#tree').treeview({
                        data: result,
                        multiSelect: false,
                        highlightSelected: true,    //是否高亮选中
                        onhoverColor: "#E8E8E8",
                        selectable: true,
                        onNodeSelected: function (event, data) {
                            var deptid = data.tag;
                            if (data.createname == 0) {
                                bootbox.alert("请不要选择基础部门~");
                                return false;
                            }
                            $("#DeptId").val(deptid);
                            $("#DeptName").val(data.text);
                            $('#addModal').modal('hide');
                            $("#DeptParentid").val(data.ParentId);
                            $("#p4deptid").val(deptid);
                            $("#p4parentid").val(data.ParentId);
                        }
                    });
                    $('#addModal').modal('show');
                } else {
                    bootbox.alert("未找到部门信息！请您先添加部门~")
                }
            },
            error: function () {
                bootbox.alert("加载部门失败！")
            }
        });
    }
}

function Same() {
    if ($("#Same").is(':checked')) {
        $("#sponsiblearea").show();
        $("#sponsibleareas").hide();
    } else if ($("#NotSame").is(':checked')) {
        $("#sponsibleareas").show();
        var productchecked = $("#ProductHtml input[type='checkbox']:checked");
        if (productchecked.length > 1) {
            for (var i = 0; i < productchecked.length; i++) {
                $("#ProductArea" + $(productchecked[i]).val()).show();
                $("#AreaNone" + $(productchecked[i]).val()).show();
            }
        }
        $("#sponsiblearea").hide();
    }
}

function selectdeptarea(productid, $tag) {
    var productids = "";
    if (productid == 0) {
        var productchecked = $("#ProductHtml input[type='checkbox']:checked");
        for (var i = 0; i < productchecked.length; i++) {
            productids += $(productchecked[i]).val() + ",";
        }
        productids = TrimEndChar(productids, ",");
    } else {
        productids = productid;
    }
    if (productids == "" || productids == null || productids == "undefined" || productids == undefined) {
        bootbox.alert("请您选择产品~")
        return false;
    }
    $("#SelectProductId").val(productids);
    var deptid = $("#p4deptid").val();
    var parentid = $("#p4parentid").val();
    if (deptid == "" || parentid == "" || deptid == "0") {
        bootbox.alert("请您先选择部门~")
        return false;
    }
    $("#searchkey").val("");
    $('#treeview-checkable-school').treeview({
        showIcon: false,
        showCheckbox: true
    });
    if (productid > 0) {
        UpdateAreaView(productid, $tag, mastername);
    }
    var type = 1;
    addcloud();
    $.post("/Employee/CompDepart_GetAreas", { deptid: deptid, parentid: parentid, mastername: mastername, type: type, productid: productids }, function (data) {
        if (data.Success) {
            $('#treeview-checkable').treeview({
                data: data.Data,
                showIcon: false,
                showCheckbox: true,
                onNodeExpanded: function (event, node) {
                    var dataChild = { parentid: node.tag, mastername: mastername, deptid: deptid, type: type, productid: productids };
                    $.post("/Employee/CompDepart_GetChildrenAreas", dataChild, function (data) {
                        if (data.Success) {
                            $('#treeview-checkable').treeview("deleteChildrenNode", node.nodeId);
                            $('#treeview-checkable').treeview("addNode", [node.nodeId, { node: data.Data }]);

                            //如果父节点选中,默认子节点全部选中
                            if (node.state.checked) {
                                var newNode = $('#treeview-checkable').treeview('getNode', node.nodeId);
                                var selectNodes = getNodeIdArr(newNode);
                                if (selectNodes) {
                                    $('#treeview-checkable').treeview('checkNode', [selectNodes, { silent: true }]);
                                }
                            }
                        } else {
                            bootbox.alert(data.ErrorMsg);
                        }
                    });
                },
                onNodeChecked: function (event, node) {
                    //判断是否是加载学校的父级区域，做为全选按钮
                    var schoolParentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                    var onloadAreaId = schoolParentNode.ParentId;
                    if (node.nodeId == onloadAreaId) {
                        $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                        checkedNodes(schoolParentNode, $('#treeview-checkable-school'));
                    }
                    //父节点选中，选中所有子节点,递归
                    checkedNodes(node, $('#treeview-checkable'));

                    //子节点选中, 如果其他所有子节点已经选中,选中父节点,递归
                    checkParentNode(node, $('#treeview-checkable'));
                },
                onNodeUnchecked: function (event, node) {
                    var schoolParentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                    var onloadAreaId = schoolParentNode.ParentId;
                    if (node.nodeId == onloadAreaId) {
                        $('#treeview-checkable-school').treeview('uncheckNode', [0, { silent: true }]);
                        uncheckedNodes(schoolParentNode, $('#treeview-checkable-school'));
                    }

                    //父节点取消，取消所有子节点都,递归
                    uncheckedNodes(node, $('#treeview-checkable'));

                    //子节点取消，父节点也取消,因为一定没选全部子节点都选中
                    uncheckParentNode(node, $('#treeview-checkable'));
                },
                onNodeSelected: function (event, node) {
                    //没有子节点的节点选中，加载学校树
                    if (!node.nodes) {
                        var arrayschool = node.tag.split("|");
                        if (arrayschool[2] > 0) {
                            bootbox.alert("请直接选择学校！");
                        }
                        else {
                            var parentnode = node.state.showcheckbox;//当前点击的节点
                            var ischeckbox = node.state.checked;
                            var showcheckbox = true;
                            if (node.state.showcheckbox == false) {
                                showcheckbox = false;
                            }
                            addcloud();

                            $.post("/Employee/CompDepart_GetSchools", { tag: node.tag, mastername: mastername, deptid: deptid, type: type, productid: productids }, function (data) {
                                if (data.Success) {
                                    $('#treeview-checkable-school').treeview({
                                        data: [{ text: "全选", nodes: data.Data, ParentId: node.nodeId, AreaId: node.Id, state: { showcheckbox: showcheckbox } }],
                                        showIcon: false,
                                        showCheckbox: true,
                                        onNodeChecked: function (event, node) {
                                            var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                            var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                                            if (node.nodes) {//全选
                                                //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                                //$('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                                if (parentNode.nodes) {//全选

                                                    for (x in parentNode.nodes) {
                                                        if (!parentNode.nodes[x].state.showcheckbox) {
                                                            $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                                                        }
                                                    }
                                                    //removecloud();
                                                }
                                                //if (!parentNode) {//当前点击的节点可被选中 则选中该节点
                                                $('#treeview-checkable').treeview('checkNode', [node.ParentId, { silent: true }]);
                                                checkParentNode(areaParentNode, $('#treeview-checkable'));
                                                //}

                                            } else {
                                                //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                                var otherChecked = true;
                                                if (parentNode.nodes) {
                                                    for (x in parentNode.nodes) {
                                                        if (parentNode.nodes[x].state.checked == false) {
                                                            otherChecked = false;
                                                        }
                                                    }
                                                }
                                                //var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                                                if (otherChecked) {
                                                    $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                                    //if (!isschoolselted) {
                                                    $('#treeview-checkable').treeview('checkNode', [areaParentNode.nodeId, { silent: true }]);
                                                    checkParentNode(areaParentNode, $('#treeview-checkable'));
                                                    //}
                                                }
                                            }
                                        },
                                        onNodeUnchecked: function (event, node) {
                                            if (node.nodes) {
                                                var selectNodes = getNodeIdArr(node);
                                                $('#treeview-checkable-school').treeview('uncheckNode', [selectNodes, { silent: true }]);
                                                $('#treeview-checkable').treeview('uncheckNode', [node.ParentId, { silent: true }]);
                                            } else {
                                                $('#treeview-checkable-school').treeview('uncheckNode', [0, { silent: true }]);
                                            }
                                            var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                            var parentAreaNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId)
                                            $('#treeview-checkable').treeview('uncheckNode', [parentAreaNode.nodeId, { silent: true }]);
                                            uncheckParentNode(parentAreaNode, $('#treeview-checkable'));
                                        }
                                    });

                                    if (ischeckbox) {
                                        var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                        $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                        if (parentNode.nodes) {//全选
                                            for (x in parentNode.nodes) {
                                                $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                                            }
                                        }
                                    }
                                    //removecloud();
                                } else {
                                    //bootbox.alert(data.ErrorMsg);
                                }
                                removecloud();
                            });

                            //$.post("/Employee/CompDepart_GetSchools", { tag: node.tag, mastername: mastername, deptid: deptid, type: type, productid: productids }, function (data) {
                            //    if (data.Success) {
                            //        $('#treeview-checkable-school').treeview({
                            //            data: [{ text: "全选", nodes: data.Data, ParentId: node.nodeId, AreaId: node.Id, state: { showcheckbox: showcheckbox } }],
                            //            showIcon: false,
                            //            showCheckbox: true,
                            //            onNodeChecked: function (event, node) {
                            //                var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            //                var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                            //                if (node.nodes) {//全选
                            //                    $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                            //                    if (parentNode.nodes) {//全选
                            //                        for (x in parentNode.nodes) {
                            //                            if (!parentNode.nodes[x].state.showcheckbox) {
                            //                                $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                            //                            }
                            //                        }
                            //                    }
                            //                    //if (!parentNode) {//当前点击的节点可被选中 则选中该节点
                            //                    $('#treeview-checkable').treeview('checkNode', [node.ParentId, { silent: true }]);
                            //                    checkParentNode(areaParentNode, $('#treeview-checkable'));
                            //                    //}
                            //                    //removecloud();
                            //                } else {
                            //                    //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            //                    var otherChecked = true;
                            //                    if (parentNode.nodes) {
                            //                        for (x in parentNode.nodes) {
                            //                            if (parentNode.nodes[x].state.checked == false) {
                            //                                otherChecked = false;
                            //                            }
                            //                        }
                            //                    }
                            //                    //var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                            //                    if (otherChecked) {
                            //                        $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                            //                        //if (!isschoolselted) {
                            //                        $('#treeview-checkable').treeview('checkNode', [areaParentNode.nodeId, { silent: true }]);
                            //                        checkParentNode(areaParentNode, $('#treeview-checkable'));
                            //                        //}
                            //                    }
                            //                }

                            //            },
                            //            onNodeUnchecked: function (event, node) {
                            //                if (node.nodes) {
                            //                    var selectNodes = getNodeIdArr(node);
                            //                    $('#treeview-checkable-school').treeview('uncheckNode', [selectNodes, { silent: true }]);
                            //                    $('#treeview-checkable').treeview('uncheckNode', [node.ParentId, { silent: true }]);
                            //                } else {
                            //                    $('#treeview-checkable-school').treeview('uncheckNode', [0, { silent: true }]);
                            //                }
                            //                var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            //                var parentAreaNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId)
                            //                $('#treeview-checkable').treeview('uncheckNode', [parentAreaNode.nodeId, { silent: true }]);
                            //                uncheckParentNode(parentAreaNode, $('#treeview-checkable'));
                            //            }
                            //        });
                            //        if (ischeckbox) {
                            //            var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            //            $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                            //            if (parentNode.nodes) {//全选
                            //                addcloud();
                            //                for (x in parentNode.nodes) {
                            //                    $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                            //                    removecloud();
                            //                }
                            //            }
                            //        }
                            //    }
                            //    removecloud();
                            //});
                        }
                    }
                }
            });
            removecloud();
            $('#setAreaModal').modal('show');
        }
        else {
            removecloud();
            alert(data.ErrorMsg);
        }
    })
}

function UpdateAreaView(productid, type, mastername) {//修改时选中区域呈现
    if (masterId > 0) {
        if (type == 1) {
            $.post("/GiveUpActionAuthority/GetMasterNameArea", { "mastername": mastername, "productkeys": productid }, function (productarea) {
                $('#treeview-checkable-checked').treeview({
                    data: [{ text: "已选区域", nodes: productarea, state: { checked: false }, nodeId: 0 }],
                    showIcon: false,
                    showCheckbox: true,
                    onNodeChecked: function (event, node) {
                        if (node.nodes) {
                            var selectNodes = getNodeIdArr(node);
                            $('#treeview-checkable-checked').treeview('checkNode', [selectNodes, {
                                silent: true
                            }]);
                        } else {
                            var parentNode = $('#treeview-checkable-checked').treeview('getNode', 0);
                            var otherChecked = true;
                            if (parentNode.nodes) {
                                for (x in parentNode.nodes) {
                                    if (parentNode.nodes[x].state.checked == false) {
                                        otherChecked = false;
                                    }
                                }
                            }
                            if (otherChecked) {
                                $('#treeview-checkable-checked').treeview('checkNode', [0, {
                                    silent: true
                                }]);
                            }
                        }
                    },
                    onNodeUnchecked: function (event, node) {
                        if (node.nodes) {
                            var selectNodes = getNodeIdArr(node);
                            $('#treeview-checkable-checked').treeview('uncheckNode', [selectNodes, {
                                silent: true
                            }]);
                        } else {
                            $('#treeview-checkable-checked').treeview('uncheckNode', [0, {
                                silent: true
                            }]);
                        }
                    }
                });
            });
        } else {
            $('#treeview-checkable-checked').treeview({
                data: [{ text: "已选区域", nodes: type, state: { checked: false }, nodeId: 0 }],
                showIcon: false,
                showCheckbox: true,
                onNodeChecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('checkNode', [selectNodes, {
                            silent: true
                        }]);
                    } else {
                        var parentNode = $('#treeview-checkable-checked').treeview('getNode', 0);
                        var otherChecked = true;
                        if (parentNode.nodes) {
                            for (x in parentNode.nodes) {
                                if (parentNode.nodes[x].state.checked == false) {
                                    otherChecked = false;
                                }
                            }
                        }
                        if (otherChecked) {
                            $('#treeview-checkable-checked').treeview('checkNode', [0, {
                                silent: true
                            }]);
                        }
                    }
                },
                onNodeUnchecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('uncheckNode', [selectNodes, {
                            silent: true
                        }]);
                    } else {
                        $('#treeview-checkable-checked').treeview('uncheckNode', [0, {
                            silent: true
                        }]);
                    }
                }
            });
        }
    } else {
        $('#treeview-checkable-checked').treeview({
            showIcon: false,
            showCheckbox: true
        });
    }
}

$("#group").change(function () {
    var groupId = $("#group").find("option:selected").val();
    if (groupId == 4 || groupId == 5 || groupId == 6) {
        $("#areadiv").show();
    } else {
        $("#areadiv").hide();
    }
});

function NotDepart() {
    bootbox.alert("请先更换此员工负责代理商的渠道经理");
}