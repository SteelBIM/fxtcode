
var masterId = getQueryString("Id");
$(function () {
    $.post("/Agent/GetProduct", function (Product) {//加载产品
        if (Product != null) {
            var jsonproduct = eval(Product);
            var ProductHtml = "";
            $(jsonproduct).each(function (index, item) {
                ProductHtml += "<span style=\"float:left;margin-left:20px;\"> <input  type=\"checkbox\"   title=\"" + item.Value + "\"  value=\"" + item.Key + "\" id=\"" + item.Key + "\"  /><label style=\"margin-left:10px; font-size:15px;font-weight:500;color:#23527c;   \" for=\"" + item.Key + "\">" + item.Value + "</label> </span>";
            });
            $("#ProductHtml").html(ProductHtml);
        }
    });
    if (masterId > 0) {
        addcloud();
        $("#myModalLabel").text("编辑代理商");
        $("#ResetPassword").show();
        $("div [name='passwordupdate']").hide();
        $.post("/Agent/GetMasterById", { "Id": masterId }, function (jsondata) {
            if (jsondata != null && jsondata != "") {
                $("#agentname").val(jsondata.agentname);
                $("#agentstartdata").val(formatDate(jsondata.agent_startdate, 'YYYY-MM-dd'));
                $("#agentenddata").val(formatDate(jsondata.agent_enddate, 'YYYY-MM-dd'));
                $("#agenttel").val(jsondata.agent_tel);
                $("#postal").val(jsondata.agent_postal);
                $("#agentaddr").val(jsondata.agent_addr);
                $("#Remark").val(jsondata.agent_remark);
                $("#Name").val(jsondata.truename);
                $("#ParentMobile").val(jsondata.mobile);
                $("#QQNo").val(jsondata.qq);
                $("#Email").val(jsondata.email);
                $("#agentid").val(jsondata.agentid);
                $("#fax").val(jsondata.agent_fax);
                $("#username").val(jsondata.mastername);
                $("#masterusername").val(jsondata.mastername);
                $("#username").attr("disabled", true);
                $("#reset-mastername").html(jsondata.mastername);
                $("#agentdeptid").val(jsondata.deptid);
                $.post("/Agent/GetPrincipalDeptId", { "Principal": jsondata.parentid }, function (deptdataid) {
                    if (deptdataid != null) {
                        var jsondept = eval(deptdataid);
                        $("#channeldeptid").val(jsondept.deptid);
                        $("#p4deptid").val(jsondata.deptid);
                        $.post("/GiveUpActionAuthority/GetDeptById", { "Id": jsondept.deptid }, function (deptdata) {
                            if (deptdata != null && deptdata != "") {
                                var jsondeptdata = eval(deptdata);
                                $("#deptname").val(jsondeptdata[0].deptname);
                                $("#DeptParentid").val(deptdata[0].parentid);
                                $("#p4parentid").val(deptdata[0].parentid);
                                $("#p4deptid").val(deptdata[0].deptid);
                            }
                        });
                    }
                });
                $.post("/Agent/GetChannelPrincipal", { "deptid": 0, "Principal": jsondata.parentid }, function (response) {
                    if (response != "") {
                        $("#channel").empty();
                        $("#channel").append("<option mastername=\"\" accesskey=\"0\" value=\"\">选择渠道经理</option>");
                        $.each(eval(response), function (index, item) {
                            var opt = "";
                            if (item.mastername == jsondata.parentname) {
                                opt = $("<option selected mastername=\"" + item.mastername + "\" accesskey=\"" + item.deptid + "\">").text(item.truename).val(item.masterid);
                            } else {
                                opt = $("<option mastername=\"" + item.mastername + "\" accesskey=\"" + item.deptid + "\">").text(item.truename).val(item.masterid);
                            }
                            $("#channel").append(opt);
                        });
                    } else {
                        $("#channel").empty();
                    }
                   
                });
                $.post("/GiveUpActionAuthority/GetAreaByMasterName", { "mastername": jsondata.mastername }, function (areadata) {
                    var AreaIds = "";
                    var AreaNames = "";
                    if (areadata != null && areadata != "") {
                        $('#treeview-checkable-checked').treeview({
                            data: [{ text: "已选区域", nodes: areadata, state: { checked: false }, nodeId: 0 }],
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
                        $(eval(areadata)).each(function (i, item) {
                            AreaIds += item.tag + "@";
                            AreaNames += "<br / >" + item.text;
                            var itemproductkeu = "," + (item).productkeys + ",";
                            $("#ProductHtml input[type='checkbox']").each(function (index, itemproduct) {//勾选负责的产品
                                if (itemproductkeu.indexOf("," + $(this).val() + ",") != -1) {
                                    $(this).prop('checked', true);
                                }
                            });
                        });
                    }
                    $("#EmployeeDeptAreaIds").val(AreaIds);
                    $("#oldareas").val(AreaIds);
                    $("#Area").html(AreaNames);
                    removecloud();
                });
            }
        });
    } else {
        $("#agentselectarea").html("  <input type=\"button\" onclick=\"selectdeptarea();\" value=\"选择区域\" class=\"btn btn-primary\"  />");
        masterId = 0;
        ChannelPrincipal();
    }
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

    $("#AgentForm").bootstrapValidator({
        message: '值无效',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            agentname: {
                message: '请输入代理商名称',
                validators: {
                    notEmpty: {
                        message: '请输入代理商名称'
                    },
                    regexp: {
                        regexp: /^[\u4e00-\u9fa5_a-zA-Z0-9_\s]{1,30}$/,
                        message: '代理商名称长度不能超过30个汉字'
                    }
                }
            }
            //, agentstartdata: {
            //    message: '请选择日期',
            //    validators: {
            //        notEmpty: {
            //            message: '请选择日期'
            //        }
            //    }
            //},
            //agentenddata: {
            //    message: '请选择日期',
            //    validators: {
            //        notEmpty: {
            //            message: '请选择日期'
            //        }
            //    }
            //}
            , channel: {
                message: '请选择渠道经理',
                validators: {
                    notEmpty: {
                        message: '请选择渠道经理'
                    }
                }
            },
            agenttel: {
                message: '请输入有效固定电话',
                validators: {
                    regexp: {
                        regexp: /(\d{3}[-|－]\d{8}|\d{4}[-|－]\d{7})$/,
                        message: '请输入正确的有效固定电话'
                    }
                }
            }, postal: {
                message: '请输入有效邮政编码',
                validators: {
                    regexp: {
                        regexp: /^[0-9]{6}$/,
                        message: '请输入正确的邮政编码'
                    }
                }
            }, fax: {
                message: '请输入有效的传真',
                validators: {
                    stringLength: {
                        min: 0,
                        max: 50,
                        message: '传真在50个字符内'
                    }
                }
            }, agentaddr: {
                message: '请输入地址',
                validators: {
                    stringLength: {
                        min: 0,
                        max: 50,
                        message: '地址在50个汉字内'
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
            }, Name: {
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
            }, ParentMobile: {
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
            }, Email: {
                message: '请输入电子邮件',
                validators: {
                    regexp: {
                        regexp: /^[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?$/,
                        message: '请输入正确的电子邮件'
                    }
                }
            }, QQNo: {
                message: '请输入QQ号',
                validators: {
                    regexp: {
                        regexp: /^[1-9][0-9]{4,12}$/,
                        message: '请输入正确的QQ号'
                    }
                }
            }, username: {
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
            }, confirmPassword: {
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
            }
        }
    });
});

$("#submit").click(function () {
    if (masterId != null && masterId > 0) {

    } else {
        masterId = 0;
    }
    var agentstartdata = $.trim($("#agentstartdata").val());
    var agentenddata = $.trim($("#agentenddata").val());
    if (agentstartdata == "" || agentenddata == "") {
        bootbox.alert("请选择签约期~");
        return false;
    }
    var bootstrapValidator = $("#AgentForm").data('bootstrapValidator');
    bootstrapValidator.validate();
    if (!bootstrapValidator.isValid()) {
        return;
    }
    var agentname = $.trim($("#agentname").val());//代理商名

    var parentid = $("#channel").val();//渠道经理
    var parentname = $("#channel").find("option:selected").attr("mastername");

    var AreaIds = $("#EmployeeDeptAreaIds").val();//负责区域
    if (AreaIds == "") {
        bootbox.alert("请选择负责区域~");
        return false;
    }
    var productkeys = GetProductKeys();
    var oldproduct = $("#oldproduct").val();//原负责产品
    if (productkeys == "") {
        bootbox.alert("请选择负责产品~");
        return false;
    }
    var agenttel = $.trim($("#agenttel").val());
    var postal = $.trim($("#postal").val());
    var agentfax = $.trim($("#fax").val());
    var agentaddr = $.trim($("#agentaddr").val());
    var Remark = $("#Remark").val();//备注

    var Name = $.trim($("#Name").val());//真实姓名
    var ParentMobile = $.trim($("#ParentMobile").val());//手机号码
    var Email = $.trim($("#Email").val());//电子邮件
    var QQNo = $.trim($("#QQNo").val());//QQ号

    var username = $.trim($("#username").val());//用户名
    if (username == "") {
        bootbox.alert("请输入用户名~");
        return false;
    }
    var confirmPassword = $.trim($("#confirmPassword").val());//密码

    var DeptId = $("#channeldeptid").val();//渠道经理部门ID

    if (masterId > 0) {
        DeptId = $("#agentdeptid").val();//部门
    }

    if (DeptId == null || DeptId == "") {
        DeptId = 0;
    }
    addcloud();
    var parentmastername = $("#parentmastername").val();//渠道经理
    $.post("/Employee/UserNameIsExist", { "UserId": masterId, "UserName": username }, function (IsExist) {
        if (IsExist > 0) {
            removecloud();
            $("#username").val("");
            $("#username").focus();
            bootbox.alert("用户名已存在~");
            return false;
        } else {
            if (masterId > 0) {
                var dataupdate = {
                    masterId: masterId, mastername: username, truename: Name, mobile: ParentMobile, email: Email, qq: QQNo, deptid: DeptId, parentid: parentid, parentname: parentname, agentid: $("#agentid").val(),
                    mastertype: 1, agent_fax: agentfax, agent_remark: Remark, agentname: agentname, groupid: 6, agent_startdate: agentstartdata, agent_enddate: agentenddata, agent_tel: agenttel, agent_postal: postal, agent_addr: agentaddr
                };
                var oldareas = $("#oldareas").val();
                $.post("/Agent/Agent_Edit", { "jsondata": JSON.stringify(dataupdate), "Areas": AreaIds, "oldareas": oldareas, "parentmastername": parentname, "deptid": $("#p4deptid").val(), "productkeys": productkeys, "oldproduct": oldproduct }, function (data) {
                    if (data.Success) {
                        removecloud();
                        bootbox.setDefaults("locale", "zh_CN");
                        bootbox.alert("编辑成功~", function (result) {
                            location.href = "/Agent/Index";
                        });
                    } else {
                        removecloud();
                        bootbox.alert(data.ErrorMsg);
                    }
                });
            } else {
                var data = {
                    masterId: masterId, mastername: username, password: confirmPassword, truename: Name, mobile: ParentMobile, email: Email, qq: QQNo, deptid: DeptId, parentid: parentid, parentname: parentname,
                    channel: 1, state: 0, agent_fax: agentfax, agent_remark: Remark, agentname: agentname, agent_startdate: agentstartdata, agent_enddate: agentenddata, agent_tel: agenttel, agent_postal: postal, agent_addr: agentaddr, groupid: 6, dataauthority: 2
                };
               
                $.post("/Agent/Agent_Add", { "jsondata": JSON.stringify(data), "Areas": AreaIds, "parentmastername": parentname, "productkeys": productkeys }, function (data) {
                    if (data.Success) {
                        removecloud();
                        bootbox.setDefaults("locale", "zh_CN");
                        bootbox.alert("添加成功~", function (result) {
                            location.href = "/Agent/Index";
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

//选择渠道负责人呈现所在部门
$("#channel").change(function () {
    var deptid = $(this).find("option:selected").attr("accesskey");
    $("#channeldeptid").val(deptid);
    $("#p4deptid").val(deptid);
    if (deptid != "" && deptid > 0) {
        $.post("/GiveUpActionAuthority/GetDeptById", { "Id": deptid }, function (data) {
            if (data != null && data != "") {
                var jsondata = eval(data);
                $("#DeptParentid").val(jsondata[0].parentid);
                $("#p4parentid").val(jsondata[0].parentid);
                $("#deptname").val(jsondata[0].deptname);
                if (masterId == 0) {
                    $('#treeview-checkable-checked').treeview({
                        showIcon: false,
                        showCheckbox: true
                    });
                    $("#Area").html("");
                    $("#EmployeeDeptAreaIds").val("");
                }

            } else {
                $("#deptname").val("");
            }
        });
    } else {
        $("#deptname").val("");
    }
});

//绑定渠道负责人
function ChannelPrincipal() {
    $.post("/Agent/GetChannelPrincipal", { "deptid": 0, "Principal": 0 }, function (response) {
        if (response != "") {
            var jsondata = eval(response);
            $("#channel").empty();
            $("#channel").append("<option mastername=\"\" accesskey=\"0\" value=\"\">选择渠道经理</option>");
            $.each(eval(response), function (index, item) {
                var opt = $("<option mastername=\"" + item.mastername + "\" accesskey=\"" + item.deptid + "\">").text(item.truename).val(item.masterid);
                $("#channel").append(opt);
            });
        } else {
            $("#channel").empty();
        }
    });
}

//选择区域
function selectdeptarea() {
    var productKeys = TrimEndChar(GetProductKeys(), ",");
    if (productKeys == "" || productKeys == undefined || productKeys == "undefined") {
        bootbox.alert("请选择负责产品~")
        return false;
    }
    var qdmastername = $("#channel").find("option:selected").attr("mastername");
    var deptid = $("#p4deptid").val();
    var parentid = $("#p4parentid").val();
    if (deptid == "" || parentid == "" || deptid == "0" || qdmastername == "") {
        bootbox.alert("请选择渠道经理~")
        return false;
    }
    $("#searchkey").val("");
    $('#treeview-checkable-school').treeview({
        showIcon: false,
        showCheckbox: true
    });
    //$('#treeview-checkable-checked').treeview({
    //    showIcon: false,
    //    showCheckbox: true
    //});
    var mastername = $("#masterusername").val();
    addcloud();
    var type = 2;
    $.post("/Employee/CompDepart_GetAreas", { deptid: deptid, parentid: parentid, mastername: mastername, type: type, productid: productKeys }, function (data) {
        if (data.Success) {
            $('#treeview-checkable').treeview({
                data: data.Data,
                showIcon: false,
                showCheckbox: true,
                onNodeExpanded: function (event, node) {
                    var dataChild = { parentid: node.tag, mastername: mastername, deptid: deptid, type: type, productid: productKeys };
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
                            var isschoolselted = node.state.showcheckbox;
                            var ischeckbox = node.state.checked;
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
                            //$.post("/Employee/CompDepart_GetSchools", { tag: node.tag, mastername: mastername, deptid: deptid, type: type, productid: productKeys }, function (data) {
                            //    if (data.Success) {
                            //        $('#treeview-checkable-school').treeview({
                            //            data: [{ text: "全选", nodes: data.Data, ParentId: node.nodeId, AreaId: node.Id, state: { showcheckbox: isschoolselted } }],
                            //            showIcon: false,
                            //            showCheckbox: true,
                            //            onNodeChecked: function (event, node) {
                            //                var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            //                var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);//是否有学校已被选择 
                            //                if (node.nodes) {//全选
                            //                    var selectNodes = getNodeIdArr(node).toString();

                            //                    var array = selectNodes.split(',');
                            //                    for (var i = 0; i < array.length; i++) {
                            //                        var schoolnid = array[i];
                            //                        var schoolnodes = $('#treeview-checkable-school').treeview('getNode', schoolnid);
                            //                        if (!schoolnodes.state.showcheckbox) {
                            //                            $('#treeview-checkable-school').treeview('checkNode', [parseInt(schoolnid), { silent: true }]);
                            //                        }
                            //                    }
                            //                    if (!isschoolselted) {
                            //                        $('#treeview-checkable').treeview('checkNode', [node.ParentId, { silent: true }]);
                            //                        checkParentNode(areaParentNode, $('#treeview-checkable'));
                            //                    }

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
                            //                        if (!isschoolselted) {
                            //                            $('#treeview-checkable').treeview('checkNode', [areaParentNode.nodeId, { silent: true }]);
                            //                            checkParentNode(areaParentNode, $('#treeview-checkable'));
                            //                        }
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
                            //                for (x in parentNode.nodes) {
                            //                    $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                            //                }
                            //            }
                            //        }
                            //        removecloud();
                            //    }
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
};

//设置区域确定按钮事件（暂为新增设置区域确定按钮方法）
$("#btn_confirmAreadd").click(function () {
    //获取全部节点，如果全部选中，提示错误
    var nodes = $('#treeview-checkable').treeview("getAllNode");
    var allNodeCheck = true;
    for (n in nodes) {
        if (nodes[n].state.checked == false) {
            allNodeCheck = false;
            break;
        }
    }
    //if (allNodeCheck == true) {
    //    bootbox.alert("子区域不能全部选中！");
    //    return;
    //}

    var checkeds = $('#treeview-checkable').treeview("getChecked");
    //去重复
    var uniqueCheckeds = [];
    $.each(checkeds, function (i, el) {
        if ($.inArray(el, uniqueCheckeds) === -1) uniqueCheckeds.push(el);
    });

    var parentid = $("#p4deptid").val();//新增节点的父id,是父节点的deptid
    var i = 1;
    var areas = "";
    var districtid = "";

    var needCheckeds = [];
    //排除逻辑, 1 所有的兄弟节点都选中,所有相关节点(这个逻辑需再考察)
    for (x in uniqueCheckeds) {
        var parentNode = $('#treeview-checkable').treeview('getParent', uniqueCheckeds[x]);
        var allSiblingsChecked = true;
        if (parentNode.nodes) {
            for (y in parentNode.nodes) {
                if (parentNode.nodes[y].state.checked == false) {
                    allSiblingsChecked = false;
                    break;
                }
            }
            if (allSiblingsChecked == false) {
                needCheckeds.push(uniqueCheckeds[x]);
            } else {
                needCheckeds.push(parentNode);
            }
        } else {
            needCheckeds.push(uniqueCheckeds[x]);
        }
    }

    var uniqueNeedCheckeds = [];
    $.each(needCheckeds, function (i, el) {
        if ($.inArray(el, uniqueNeedCheckeds) === -1) uniqueNeedCheckeds.push(el);
    });
    var removeChildCheckeds = [];
    //去除父级的子级
    for (var k = 0; k < uniqueNeedCheckeds.length; k++) {
        var isNotChild = true;
        for (var l = 0; l < uniqueNeedCheckeds.length; l++) {
            if (uniqueNeedCheckeds[k].parentId == uniqueNeedCheckeds[l].nodeId) {
                isNotChild = false;
            }
        }
        if (isNotChild) {
            removeChildCheckeds.push(uniqueNeedCheckeds[k]);
        }
    }
    for (z in uniqueNeedCheckeds) {
        if (i == 1) {
            areas += uniqueNeedCheckeds[z].text;
            districtid += uniqueNeedCheckeds[z].tag;
            i += 1;
        } else {
            areas += "、" + uniqueNeedCheckeds[z].text;
            districtid += "," + uniqueNeedCheckeds[z].tag;
        }
    }
    var mastername = "";
    if (masterId > 0) {
        mastername = $.trim($("#masterusername").val());
    }
    if (districtid != "") {
        $.post("/Agent/JudgmentSelectedArea", { Area: districtid, mastername: mastername }, function (data) {
            if (data.Success) {
                bootbox.alert(data.ErrorMsg);
            }
        });
    }
    $("#Area").html(areas);
    $("#EmployeeDeptAreaIds").val(districtid);
    $('#setAreaModal').modal('hide');
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

//选择部门
function selectDepart() {
    if ($("#EmployeeDeptAreaIds").val() != "") {
        bootbox.setDefaults("locale", "zh_CN");
        bootbox.confirm("重置部门将会清空已选区域~ 请确认！ ", function (result) {
            if (result) {
                $('#treeview-checkable-checked').treeview({
                    showIcon: false,
                    showCheckbox: true
                });
                $("#Area").html("");
                $("#EmployeeDeptAreaIds").val("");
                $.ajax({
                    type: "Post",
                    url: "/Employee/SelectDept",
                    dataType: "json",
                    success: function (result) {
                        if (result != null && result != "") {
                            $('#tree').treeview({
                                data: result,
                                multiSelect: false,
                                highlightSelected: false,    //是否高亮选中
                                onhoverColor: "#E8E8E8",
                                selectable: true,
                                onNodeSelected: function (event, data) {
                                    var deptid = data.tag;
                                    if (data.createname == 0) {
                                        bootbox.alert("请不要选择基础部门~");
                                        return false;
                                    }
                                    $("#deptname").val(data.text);
                                    $.post("/Agent/GetChannelPrincipal", { deptid: deptid, "Principal": 0 }, function (response) {
                                        if (response != "") {
                                            var jsondata = eval(response);
                                            $("#channel").empty();
                                            $("#channel").append("<option mastername=\"\" accesskey=\"0\" value=\"\">选择渠道经理</option>");
                                            $.each(eval(response), function (index, item) {
                                                var opt = $("<option mastername=\"" + item.mastername + "\" accesskey=\"" + item.deptid + "\">").text(item.truename).val(item.masterid);
                                                $("#channel").append(opt);
                                            });
                                            $('#addModal').modal('hide');
                                        } else {
                                            $("#channel").empty();
                                            $('#addModal').modal('hide');
                                        }
                                    });
                                }
                            });
                            $('#addModal').modal('show');
                        } else {
                            bootbox.alert("未找到部门信息！请您先添加部门~")
                        }
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
                        highlightSelected: false,    //是否高亮选中
                        onhoverColor: "#E8E8E8",
                        selectable: true,
                        onNodeSelected: function (event, data) {
                            var deptid = data.tag;
                            if (data.createname == 0) {
                                bootbox.alert("请不要选择基础部门~");
                                return false;
                            }
                            $("#deptname").val(data.text);
                            $.post("/Agent/GetChannelPrincipal", { deptid: deptid, "Principal": 0 }, function (response) {
                                if (response != "") {
                                    var jsondata = eval(response);
                                    $("#channel").empty();
                                    $("#channel").append("<option mastername=\"\" accesskey=\"0\" value=\"\">选择渠道经理</option>");
                                    $.each(eval(response), function (index, item) {
                                        var opt = $("<option mastername=\"" + item.mastername + "\" accesskey=\"" + item.deptid + "\">").text(item.truename).val(item.masterid);
                                        $("#channel").append(opt);
                                    });
                                    $('#addModal').modal('hide');
                                } else {
                                    $("#channel").empty();
                                    $('#addModal').modal('hide');
                                }
                            });
                        }
                    });
                    $('#addModal').modal('show');
                } else {
                    bootbox.alert("未找到部门信息！请您先添加部门~")
                }
            }
        });
    }
}

function RemoveSelectArea(deptid) {//去除所有选择的区域
    if (masterId > 0) {
        $.post("/GiveUpActionAuthority/GetAreaNumber", { deptid: deptid }, function (number) {
            if (number > 0) {
                bootbox.alert("该部门有负责区域！清空该代理商原负责区域！");
                $('#treeview-checkable-checked').treeview({
                    showIcon: false,
                    showCheckbox: true
                });
                $("#Area").html("");
                $("#EmployeeDeptAreaIds").val("");
            } else {
                bootbox.alert("该部门没有负责区域！不影响该代理商原负责区域！");
            }
        });
    }
}


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