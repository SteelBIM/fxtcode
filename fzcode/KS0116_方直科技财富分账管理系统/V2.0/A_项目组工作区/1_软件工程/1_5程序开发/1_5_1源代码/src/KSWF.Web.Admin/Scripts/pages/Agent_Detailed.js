var masterId = getQueryString("Id");

$(function () {
    if (masterId > 0) {
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
                $("#fax").val(jsondata.agent_fax);
                $("#username").val(jsondata.mastername);
                $("#masterusername").val(jsondata.mastername);
                $("#username").attr("disabled", true);
                $("#reset-mastername").html(jsondata.mastername);
                $("#channeldeptid").val(jsondata.deptid);
                $.post("/GiveUpActionAuthority/GetDeptById", { "Id": jsondata.deptid }, function (deptdata) {
                    if (deptdata != null && deptdata != "") {
                        var jsondeptdata = eval(deptdata);
                        $("#deptname").html(jsondeptdata[0].deptname);
                        $("#DeptParentid").val(jsondeptdata[0].parentid);
                    }
                    $.post("/GiveUpActionAuthority/GetAreaByMasterName", { "mastername": jsondata.mastername }, function (areadata) {
                        var AreaIds = "";
                        var AreaNames = "";
                        if (areadata != null && areadata != "") {
                            $(eval(areadata)).each(function (i, item) {
                                AreaNames += "<br />" + item.text
                            });
                        }
                        $("#Area").html(AreaNames);
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
                });
            }
        });
    }
});