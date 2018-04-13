function SingleActionHtml(parentcolumn, ID, Actionstr) {
    if (ID.indexOf('_View') != -1) {
        return "<span style=\"float:left;\"> <input go=\"" + parentcolumn + "\" onchange=\"Parent('" + parentcolumn + "',this)\"  type=\"checkbox\"   value=\"" + ID + "\" id=\"" + ID + "\"  /><label style=\"margin-left:10px; font-size:15px;font-weight:500;color:#23527c;  width:120px;\" for=\"" + ID + "\">" + Actionstr + "</label> </span>";
    }
    return " <span style=\"display:none;\" class=\"" + parentcolumn + "\">  <input style=\"margin-left:20px;\" value=\"" + ID + "\"   type=\"checkbox\" class=\"" + parentcolumn + "\"   id=\"" + ID + "\"  /><label style=\"margin-left:10px;font-weight:500; \" for=\"" + ID + "\">" + Actionstr + "</label></span>";
}
function Parent(ID, obj) {
    if ($(obj).is(':checked')) {
        $("." + ID).show();
        $("#OperationAuthority input:checkbox[class='" + ID + "']").prop('checked', true);
    } else {
        $("." + ID).hide();
        $("#OperationAuthority input:checkbox[class='" + ID + "']").prop('checked', false);
    }
}
function StrJson(str) {
    if (str != "") {
        return eval($.parseJSON("[" + str + "]"));
    }
}
function ExistsParentColumid(list, parentcolumid) {
    for (var i = 0; i < list.length; i++) {
        if (list[i].parentcolumid == parentcolumid) {
            return true;
        }
    }
    return false;
}
function ExistsColumid(list, columnid) {
    for (var i = 0; i < list.length; i++) {
        if (list[i].parentcolumid == 0 && list[i].columnid == columnid) {
            return true;
        }
    }
    return false;
}



$(function () {
    var groupid = getQueryString("Id");
    if (groupid > 0) {
        $("#myModalLabel").text("编辑角色");
    } else {
        groupid = 0;
    }
    if ($("#OperationAuthority").length > 0) {
        addcloud();
        $.ajax({
            type: "POST",
            url: "/AuthorityMgr/GetAllAction",
            async: true,
            dataType: "json",
            success: function (response) {
                if (response != null && response != "") {
                    var actionhtml = "";
                    var jsondata = eval(response);
                    var parentlist = "";
                    $(eval(response)).each(function (i, item) {
                        if (parentlist != "") {
                            var parentjsonlist = StrJson(parentlist);
                            if (item.parentcolumid == 0) {
                                if (!ExistsColumid(parentjsonlist, item.columnid)) {
                                    parentlist += "," + JSON.stringify(item);
                                }
                            } else {
                                if (!ExistsParentColumid(parentjsonlist, item.parentcolumid)) {
                                    parentlist += "," + JSON.stringify(item);
                                }
                            }
                        } else {
                            parentlist += JSON.stringify(item);
                        }
                    });
                    $(StrJson(parentlist)).each(function (i, item) {
                        if (item.parentcolumid == 0) {
                            actionhtml += "<div style=\"clear:both; height:25px;\" class=\"columaction\">";
                            $(eval(response)).each(function (i, item2) {
                                if (item.actionurl == item2.actionurl) {
                                    actionhtml += SingleActionHtml(item.actionurl, item2.actionname, item2.actionstr);
                                }
                            });
                            actionhtml += "</div>";
                        } else {
                            actionhtml += "<div style=\"clear:both; margin-top:10px; margin-bottom:10px; float:left; font-size:15px;font-weight:500;color:#23527c;  width:120px;\">" + item.parentcolumname + "</div>";// + ActionHtml("parentcolumname" + i, item.parentcolumname);
                            actionhtml += "<div style=\"clear:both; \" >";
                            var actionurl = "";
                            $(eval(response)).each(function (a, item2) {
                                if (item.parentcolumid == item2.parentcolumid) {
                                    if (actionurl.indexOf(item2.actionurl) == -1) {
                                        actionhtml += "<div style=\"clear:both; height:25px;\" class=\"columaction\">";
                                        $(eval(response)).each(function (i, item3) {
                                            if (item2.actionurl == item3.actionurl) {
                                                actionhtml += SingleActionHtml(item2.actionurl, item3.actionname, item3.actionstr);
                                            }
                                        });
                                        actionhtml += "</div>";
                                        actionurl += item2.actionurl + ",";
                                    }
                                }
                            });
                            actionhtml += "</div>";
                        }
                    });

                    $("#OperationAuthority").html(actionhtml);

                    if (groupid > 0) {//修改情况下默认选中权限
                        $.post("/AuthorityMgr/GetGroupById", { "Id": groupid }, function (data) {
                            if (data != null && data != "") {
                                var jsondata = eval(data);
                                $("#groupname").val(jsondata.groupname);
                                $("#DataSelectAction").find("option").each(function (i) {
                                    if ($(this).val() == jsondata.dataauthority) {
                                        $(this).attr("selected", true);
                                    }
                                });

                                $.post("/AuthorityMgr/GetGroupAction", { "groupid": groupid }, function (data2) {//加载已选中权限
                                    if (data2 != null && data2 != "") {
                                        $("#OperationAuthority input:checkbox").each(function (i, item) {
                                            $(eval(data2)).each(function (i, item2) {
                                                var itemvalue = $(item).val();
                                                if (itemvalue != "" && itemvalue != "on" && itemvalue == item2.actionname && itemvalue.indexOf("_View") != -1) {
                                                    $("." + $(item).attr("go")).show();
                                                }
                                                if (itemvalue != "" && itemvalue != "on" && itemvalue == item2.actionname) {
                                                    $("." + $(item).attr("go")).show();
                                                    $(item).prop('checked', true);
                                                }
                                            });
                                        });
                                        if (jsondata.creatername == "系统默认") {
                                            $("#mymodallabel").text("查看角色");
                                            $("#OperationAuthority input:checkbox").each(function (i, item) {
                                                $(item).attr("disabled", true);
                                            });
                                            $("#groupname").attr("disabled", true);
                                            $("#dataselectaction").attr("disabled", true);
                                            $("#submit").hide();
                                        }
                                    }
                                });

                            };
                            removecloud();
                        });
                    } else {
                        removecloud();
                    }
                } else {
                    removecloud();
                    bootbox.alert("请求失败！请重试~");
                    return;
                }
            },
            error: function (request, status, error) {
                bootbox.alert("请求失败！提示：" + status + error);
            }
        });
    }

    $("#submit").click(function () {//提交
        var groupname = $.trim($("#groupname").val());
        var DataSelectAction = $("#DataSelectAction").val();
        var action = "";
        $("#OperationAuthority input:checkbox").each(function (i, item) {
            if ($(item).is(':checked')) {
                var itemvalue = $(item).val();
                if (itemvalue != "" && itemvalue != "on") {
                    action += itemvalue + ",";
                }
            }
        });
        if (groupname == "" || groupname.length > 30) {
            bootbox.alert("请输入角色名称！30个汉字内。");
            return false;
        }
        if (action == "") {
            bootbox.alert("请选择操作权限");
            return false;
        }
        addcloud();
        $.post("/AuthorityMgr/GroupNameIsExist", { "groupid": groupid, "groupname": groupname }, function (IsExist) {
            if (IsExist > 0) {
                removecloud();
                bootbox.alert("角色名称已存在~");
            } else {
                if (groupid > 0) {
                    $.post("/AuthorityMgr/AuthorityMgr_Edit", { "groupid": groupid, "groupname": groupname, "dataauthority": DataSelectAction, "actions": action }, function (data) {
                        if (data > 0) {
                            if (data == 2) {
                                bootbox.setDefaults("locale", "zh_CN");
                                bootbox.alert("编辑成功~", function (result) {
                                    location.href = "/AuthorityMgr/Index";
                                });
                            }
                        } else {//失败
                            alert("编辑失败，请稍后重试~");
                            removecloud();
                        }
                    });
                } else {
                    $.post("/AuthorityMgr/AuthorityMgr_Add", { "groupname": groupname, "dataauthority": DataSelectAction, "actions": action }, function (data) {
                        if (data.Success) {
                            bootbox.alert("添加成功~");
                            location.href = "/AuthorityMgr/Index";
                        } else {
                            removecloud();
                            bootbox.alert(data.ErrorMsg);
                        }
                    });
                }
            }
        });
    });
});









