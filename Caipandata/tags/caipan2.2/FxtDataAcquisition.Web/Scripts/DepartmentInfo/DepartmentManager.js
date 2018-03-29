var functionCode_DeleteMy = 0;
var functionCode_DeleteAll = 0;
var functionCode_UpdateMy = 0;
var functionCode_UpdateAll = 0;
var nowDepartmentId = 0;
$(function () {
    functionCode_DeleteMy = $("#hdFunctionCode_DeleteMy").val() * 1;
    functionCode_DeleteAll = $("#hdFunctionCode_DeleteAll").val() * 1;
    functionCode_UpdateMy = $("#hdFunctionCode_UpdateMy").val() * 1;
    functionCode_UpdateAll = $("#hdFunctionCode_UpdateAll").val() * 1;
    nowDepartmentId = $("#hdNowDepartmentId").val() * 1;
    GetDepartmentList(1);
    /***点击查询***/
    $("#btnSearch").bind("click", function () {
        $("#hdDepartmentCount").val("");
        GetDepartmentList(1);
    });
    /*****点击编辑****/
    $("#departmentRowList").find(".btn_edit").live("click", function () {
        EditDepartment($(this).attr("data-departmentId"), $("#hdCompanyName").val());
    });
    /****点击删除单个***********/
    $("#departmentRowList").find(".btn_delete").live("click", function () {
        DeleteDepartment($(this).attr("data-departmentId"));
    });
    /******点击新增小组**********/
    $("#btnAdd").bind("click", function () {
        EditDepartment("", $("#hdCompanyName").val());
    });
    /*******点击批量删除***********/
    $("#btnDelete").bind("click", function () {
        var dom = $("#departmentRowList").find(".departmentRow");
        var departmentIds = GetCheckValueByFind(dom, ',');
        DeleteDepartment(departmentIds);
    });
    /******点击全选********/
    $("#cbAll").bind("click", function () {
        var checkedValue = $(this).attr("checked");
        if (checkedValue == "checked") {
            checkedValue = true;
        }
        else {
            checkedValue = false;
        }
        $("#departmentRowList").find(".departmentRow").find("input[type='checkbox']").attr("checked", checkedValue);
        $("#cbAll").attr("checked", checkedValue);
    })
});
/******删除小组******/
function DeleteDepartment(departmentIds) {
    if (!confirm("是否确定删除选中小组")) {
        return;
    }
    if (departmentIds == null || departmentIds == "") {
        alert("请选择要删除的小组");
        return;
    }
    var paraJson = { departmentIds: departmentIds };
    $.extendAjax(
                {
                    url: "/DepartmentInfo/DepartmentManager_Delete_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (data.result) {

                        AlertFancybox("删除成功");
                        RemoveDepartmentByIds(departmentIds);
                    } else {
                        alert(data.message);
                    }
                },
               { dom: "#departmentPanel" });
}
/*********移除小组html**********/
function RemoveDepartmentByIds(departmentIds) {
    var ids = departmentIds.split(',');
    for (var i = 0; i < ids.length; i++) {
        if (ids[i] != null && ids[i] != "") {
            $("#departmentRowList").find("#departmentRow_" + ids[i]).remove();
        }
    }
}
/********查询小组列表*************/
function GetDepartmentList(pageIndex) {
    var keyWord = $("#txtKeyword").val();
    //if(keyWord!=null&&keyWord!="")
    //{
    //     var _Pat = /^[\u4e00-\u9fa5,\a-zA-Z0-9]*$/;
    //     var bol = _Pat.test(keyWord);
    //     if(!bol)
    //     {
    //         alert("不能输入特殊字符,请重新输入");
    //         return;
    //     }
    //}
    var pageSize = 10;
    //var isGetCount=1;
    //if($("#hdDepartmentCount").val()!="")
    //{
    //    isGetCount=0;
    //}
    //var paraJson = { keyWord: keyWord, pageIndex: pageIndex, pageSize: pageSize, isGetCount: isGetCount };
    var paraJson = { keyWord: keyWord, pageIndex: pageIndex, pageSize: pageSize };
    $.extendAjax(
                {
                    url: "/DepartmentInfo/DepartmentManager_GetList_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    $("#departmentRowList").find(".departmentRow").remove();
                    if (data.result) {
                        $("#hdDepartmentCount").val(data.data.count);

                        if (data.data.list != null) {
                            var projectIds = "";
                            var list = data.data.list;
                            for (var i = 0; i < list.length; i++) {
                                var departmentObj = list[i];
                                var dom = BindDepartmentDom(departmentObj);
                                $("#departmentRowList").append(dom);
                            }

                        }

                        var count_data = $("#hdDepartmentCount").val();
                        var pageCount = parseInt(((count_data * 1) - 1) / pageSize) + 1;
                        //alert(count_data);
                        BindPage(pageIndex, pageSize, count_data);
                    } else {
                        alert(data.message);
                    }

                    //if(data!=null)
                    //{
                    //    if(data.result!=1)
                    //    {
                    //        if(data.errorType==0)
                    //        {
                    //           alert(decodeURIComponent(data.message));
                    //        }
                    //        return;
                    //    }
                    //    if($("#hdDepartmentCount").val()=="")
                    //    {
                    //        $("#hdDepartmentCount").val(data.detail.Count);
                    //    }
                    //    if (data.detail.List != null) 
                    //    {
                    //        var projectIds="";
                    //        var list = data.detail.List;
                    //        for (var i = 0; i < list.length; i++) 
                    //        {
                    //            var departmentObj=list[i];
                    //            var dom =BindDepartmentDom(departmentObj);
                    //            $("#departmentRowList").append(dom);
                    //        }

                    //    }

                    //    var count_data =$("#hdDepartmentCount").val();
                    //    var pageCount = parseInt(((count_data*1) - 1) / pageSize) + 1;
                    //    //alert(count_data);
                    //    BindPage(pageIndex,pageSize,count_data);                        
                    //}
                },
               { dom: "#departmentPanel" });
}
/**********绑定小组行html信息************/
function BindDepartmentDom(departmentObj) {
    var dom = $("#departmentRowList").find("#departmentRow").clone();
    dom.find(".txt_departmentname").html(decodeURIComponent(departmentObj.DepartmentName));
    dom.find(".btn_edit").attr("data-departmentId", departmentObj.DepartmentId);
    dom.find(".btn_delete").attr("data-departmentId", departmentObj.DepartmentId);
    /****判断操作按钮权限****/
    dom.find(".btn_edit").hide();
    dom.find(".btn_delete").hide();
    /*有删除全部权限||(删除自己权限&&当前小组为自己小组)*/
    if (CheckFunctionCodes(functionCode_DeleteAll) || (CheckFunctionCodes(functionCode_DeleteMy) && departmentObj.DepartmentId * 1 == nowDepartmentId)) {
        dom.find(".btn_delete").show();
    }
    /*有修改全部权限||(修改自己权限&&当前小组为自己小组)*/
    if (CheckFunctionCodes(functionCode_UpdateAll) || (CheckFunctionCodes(functionCode_UpdateMy) && departmentObj.DepartmentId * 1 == nowDepartmentId)) {
        dom.find(".btn_edit").show();
    }
    dom.find(".cb_select").val(departmentObj.DepartmentId);
    dom.attr("id", "departmentRow_" + departmentObj.DepartmentId).addClass("departmentRow").show();
    return dom;
}
/**绑定分页**/
function BindPage(nowIndex, pageSize, count) {
    BindPageCommon("#example", nowIndex, count, pageSize, 10,
                                      function (event, originalEvent, type, page) {
                                          GetDepartmentList(page);
                                      }, null);
}
/*******打开编辑小组信息******/
function EditDepartment(departmentId, companyName) {
    var url = "/DepartmentInfo/SetDepartment?departmentId={0}&companyName={1}"
    url = url._StringFormat(departmentId, encodeURIComponent(companyName));
    $.fancybox({
        'href': url,
        'width': 600,
        'height': 250,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
        }
    });
}
/*******编辑小组信息成功后响应回来的方法*********/
function EditDepartment_Response(data, departmentId, departmentName) {
    if (data != null && data != "") {
        var dom = BindDepartmentDom(data);
        $("#departmentRowList").prepend(dom);
    }
    else {
        $("#departmentRowList").find("#departmentRow_" + departmentId).find(".txt_departmentname").html(departmentName);
    }
}
/***************************************************************Common************************************/
/**********获取当前用户对此页面所用户的权限****************/
function GetNowFunctionCodes() {
    var doms = $("#divFunctionCodes").find(".functioncode");
    var codes = "";
    for (var i = 0; i < doms.length; i++) {
        var val = $(doms.get(i)).text();
        codes = codes + val + ",";

    }
    codes = codes.TrimEnd(',');
    return codes;
}
/**********验证当前用户是否有指定操作权限****************/
function CheckFunctionCodes(functionCode) {
    var doms = $("#divFunctionCodes").find(".functioncode_" + functionCode);
    if (doms.length > 0) {
        return true;
    }
    return false;
}