var TempletManager = function () {

    return {

        init: function (pIndex) {

            TempletManager.initTempletMananger();

        },

        initTempletMananger: function () {
            
            getList(pageindex);

            /*******点击查询***********/
            $("#btnSearch").click(function (o) {

                getList(pageindex);
            });

            /*******点击新增楼盘模板***********/
            $("#btnAddProject").click(function (o) {

                window.location.href = "#/Templet/Create?type=1034001"
            });

            /*******点击新增楼栋模板***********/
            $("#btnAddBuilding").click(function (o) {

                window.location.href = "#/Templet/Create?type=1034002"
            });

            /*******点击新增房号模板***********/
            $("#btnAddHouse").click(function (o) {

                window.location.href = "#/Templet/Create?type=1034004"
            });

            /*****点击编辑****/
            $("#templetRowList").find(".btn_edit").live("click", function () {

                window.location.href = "#/Templet/Create?id=" + $(this).attr("data-templetId") + "&type=" + $(this).attr("data-type")
            });

            $("#templetRowList").find(".btn_delete").die();
            /****点击删除单个***********/
            $("#templetRowList").find(".btn_delete").live("click", function () {

                var ids = [$(this).attr("data-templetId")];

                Delete(ids);
            });

            /*******点击批量删除***********/
            $("#btnDelete").click(function (o) {

                var ids = [];

                var chks = $("#templetRowList").find("input:checked");

                if (chks.length < 1) {

                    alert("请选择要删除的数据！");

                    return false;
                }

                for (var i = 0; i < chks.length; i++) {

                    ids.push($(chks[i]).val());

                }

                Delete(ids);
            });

            $("#templetRowList").find(".btn_setcurrent").die()
            /****点击设置默认***********/
            $("#templetRowList").find(".btn_setcurrent").live("click", function () {

                if (!confirm("确定要设置为默认模板吗？")) {
                    return;
                }

                SetCurrent($(this).attr("data-templetId"), $(this).attr("data-type"));
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

                $("#templetRowList").find(".templetRow").find("input[type='checkbox']").attr("checked", checkedValue);

                $("#cbAll").attr("checked", checkedValue);
            })
        }

    };

    var pageindex = 1;

    /**********模板列表************/
    function getList(pageIndex) {

        var pageSize = 10;

        var templetName = $("#tempLetName").val();

        var dataType = $("#dataType").val();

        var paraJson = { templetName: templetName, dataType: dataType, pageIndex: pageIndex, pageSize: pageSize }

        $.extendAjax(

                {
                    url: "/Templet/LoadData",

                    data: paraJson,

                    type: "post",

                    dataType: "json"

                },

                function (data) {

                    $("#templetRowList").find(".templetRow").remove();

                    if (data.result) {

                        $("#templetCountShow").html(data.data.count);

                        if (data.data.list != null) {

                            $("#hdTempletCount").val(data.data.count);

                            var projectIds = "";

                            var list = data.data.list;

                            for (var i = 0; i < list.length; i++) {

                                var templetObj = list[i];

                                var dom = BindTempletDom(templetObj);

                                $("#templetRowList").append(dom);
                            }
                        }

                        var pageCount = (data.data.count - 1) / pageSize + 1;

                        BindPage(pageIndex, pageSize, data.data.count);

                    } else {

                        alert(data.message);
                    }
                },

               { dom: "#templetPanel" });
    };

    /**********绑定模板行html信息************/
    function BindTempletDom(templetObj) {

        var dom = $("#templetRowList").find("#templetRow").clone();

        dom.find(".txt_templetname").html(templetObj.TempletName);

        var datatype = "";

        if (templetObj.DatType == 1034001) {

            datatype = "楼盘";

        } else if (templetObj.DatType == 1034002) {

            datatype = "楼栋";

        } else if (templetObj.DatType == 1034004) {

            datatype = "房号";
        }

        dom.find(".txt_datatypename").html(datatype);

        if (templetObj.IsCurrent) {

            dom.find(".txt_iscurrent").html("是");

        } else {

            dom.find(".txt_iscurrent").html("");
        }

        dom.find(".btn_edit").attr("data-templetid", templetObj.TempletId);

        dom.find(".btn_edit").attr("data-type", templetObj.DatType);

        dom.find(".btn_delete").attr("data-templetid", templetObj.TempletId);

        dom.find(".btn_setcurrent").attr("data-templetid", templetObj.TempletId);

        dom.find(".btn_setcurrent").attr("data-type", templetObj.DatType);
        
        /****判断操作按钮权限****/
        //dom.find(".btn_edit").hide();
        //dom.find(".btn_delete").hide();
        ///*有删除全部权限||(删除自己权限&&当前小组为自己小组)*/
        //if (CheckFunctionCodes(functionCode_DeleteAll) || (CheckFunctionCodes(functionCode_DeleteMy) && templetObj.DepartmentId * 1 == nowDepartmentId)) {
        //    dom.find(".btn_delete").show();
        //}
        ///*有修改全部权限||(修改自己权限&&当前小组为自己小组)*/
        //if (CheckFunctionCodes(functionCode_UpdateAll) || (CheckFunctionCodes(functionCode_UpdateMy) && templetObj.DepartmentId * 1 == nowDepartmentId)) {
        //    dom.find(".btn_edit").show();
        //}

        dom.find(".cb_select").val(templetObj.TempletId);

        dom.attr("id", "templetRow_" + templetObj.TempletId).addClass("templetRow").show();

        return dom;
    };

    /**绑定分页**/
    function BindPage(nowIndex, pageSize, count) {

        BindPageCommon("#example", nowIndex, count, pageSize, 10,

            function (event, originalEvent, type, page) {

                pageindex = page;

                getList(page);

            }, null);
    }

    /**********删除************/
    function Delete(ids) {

        if (!confirm("确定要删除吗？")) {
            return;
        }

        //异步提交数据
        var paraJson = { ids: ids }

        $.extendAjax(

                {
                    url: "/Templet/Delete",

                    data: paraJson,

                    type: "post",

                    dataType: "json"

                },

                function (data) {

                    if (data.result) {

                        alert(data.message);
                        
                        getList(pageindex);
                        //RemoveByIds(ids);

                    } else {

                        alert(data.message);
                    }
                },

               { dom: "#templetPanel" });
    }

    /*********移除html**********/
    function RemoveByIds(ids) {

        for (var i = 0; i < ids.length; i++) {

            $("#templetRowList").find("#templetRow_" + ids[i]).remove();
        }
    }

    /*********设置默认模板**********/
    function SetCurrent(id,type) {

        //异步提交数据
        var paraJson = { id: id, DatType: type }

        $.extendAjax(

                {
                    url: "/Templet/SetCurrent",

                    data: paraJson,

                    type: "post",

                    dataType: "json"

                },

                function (data) {

                    if (data.result) {

                        alert(data.message);

                        getList(pageindex);

                    } else {

                        alert(data.message);
                    }
                },

               { dom: "#templetPanel" });
    }
}();