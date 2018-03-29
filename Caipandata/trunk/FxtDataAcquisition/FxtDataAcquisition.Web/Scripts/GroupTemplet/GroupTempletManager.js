var GroupTempletManager = function () {

    return {

        init: function () {

            GroupTempletManager.initGroupTempletMananger();

        },

        initGroupTempletMananger: function () {

            getList(pageindex);

            /*******点击查询***********/
            $("#btnSearch").click(function (o) {

                getList(pageindex);
            });

            /*******点击新增***********/
            $("#btnAdd").click(function (o) {

                Create();
            });

            $("#templetRowList").find(".btn_delete").die();
            /*****点击编辑****/
            $("#rowListGroup").find(".btn_edit").live("click", function () {

                Create($(this).attr("data-Id"));
            });

            /****点击删除单个***********/
            $("#rowListGroup").find(".btn_delete").die();
            $("#rowListGroup").find(".btn_delete").live("click", function () {

                var ids = [$(this).attr("data-Id")];

                Delete(ids);
            });

            /*******点击批量删除***********/
            $("#btnDelete").click(function (o) {

                var ids = [];

                var chks = $("#rowListGroup").find("input:checked");

                if (chks.length < 1) {

                    alert("请选择要删除的数据！");

                    return false;
                }

                for (var i = 0; i < chks.length; i++) {

                    ids.push($(chks[i]).val());

                }

                Delete(ids);
            });

            /******点击全选********/
            $("#cbAll").click(function () {

                var checkedValue = $(this).attr("checked");

                if (checkedValue == "checked") {

                    checkedValue = true;
                }

                else {

                    checkedValue = false;
                }

                $("#rowListGroup").find(".rowobj").find("input[type='checkbox']").attr("checked", checkedValue);

                $("#cbAll").attr("checked", checkedValue);
            })
        }

        ,
        /*******编辑小组信息成功后响应回来的方法*********/
        EditResponse: function (data, id, name, type) {

            if (data != null && data != "") {

                var dom = BindDom(data);

                $("#rowListGroup").prepend(dom);
            }

            else {

                var row = $("#rowListGroup").find("#row_" + id);

                row.find(".txt_name").html(name);

                var datatype = "";

                if (type == 1034001) {

                    datatype = "楼盘";

                } else if (type == 1034002) {

                    datatype = "楼栋";

                } else if (type == 1034004) {

                    datatype = "房号";
                }

                row.find(".txt_datatypename").html(datatype);
            }
        }
    };

    var pageindex = 1;

    /**********模板列表************/
    function getList(pageIndex) {

        var pageSize = 10;

        var name = $("#name").val();

        var dataType = $("#dataType").val();

        var paraJson = { name: name, dataType: dataType, pageIndex: pageIndex, pageSize: pageSize }

        $.extendAjax(

                {
                    url: "/GroupTemplet/LoadData",

                    data: paraJson,

                    type: "post",

                    dataType: "json"

                },

                function (data) {
                    
                    $("#rowListGroup").find(".rowobj").remove();

                    if (data.result) {

                        $("#countShow").html(data.data.count);

                        if (data.data.list != null) {

                            $("#hdCount").val(data.data.count);

                            var projectIds = "";

                            var list = data.data.list;

                            for (var i = 0; i < list.length; i++) {

                                var Obj = list[i];

                                var dom = BindDom(Obj);

                                $("#rowListGroup").append(dom);
                            }
                        }

                        var pageCount = (data.data.count - 1) / pageSize + 1;

                        BindPage(pageIndex, pageSize, data.data.count);

                    } else {

                        alert(data.message);
                    }
                },

               { dom: "#panel" });
    };

    /**********绑定模板行html信息************/
    function BindDom(Obj) {

        var dom = $("#rowListGroup").find("#rowobj").clone();

        dom.find(".txt_name").html(Obj.FieldGroupTempletName);

        var datatype = "";

        if (Obj.DatType == 1034001) {

            datatype = "楼盘";

        } else if (Obj.DatType == 1034002) {

            datatype = "楼栋";

        } else if (Obj.DatType == 1034004) {

            datatype = "房号";
        }

        dom.find(".txt_datatypename").html(datatype);

        dom.find(".btn_edit").attr("data-id", Obj.FieldGroupTempletId);

        dom.find(".btn_delete").attr("data-id", Obj.FieldGroupTempletId);

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

        dom.find(".cb_select").val(Obj.FieldGroupTempletId);

        dom.attr("id", "row_" + Obj.FieldGroupTempletId).addClass("rowobj").show();

        return dom;
    };

    /**绑定分页**/
    function BindPage(nowIndex, pageSize, count) {

        BindPageCommon("#example", nowIndex, count, pageSize, 10,

            function (event, originalEvent, type, page) {

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
                    url: "/GroupTemplet/Delete",

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

               { dom: "#panel" });
    }

    /*********移除html**********/
    function RemoveByIds(ids) {

        for (var i = 0; i < ids.length; i++) {

            $("#rowListGroup").find("#row_" + ids[i]).remove();
        }
    }

    /*******打开编辑信息******/
    function Create(id) {
        var url = "/GroupTemplet/Create?id=" + id
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
}();