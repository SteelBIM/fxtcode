//加载树形菜单
function LoadTreeMarketClassify() {
    $.ajax({
        type: "Post",
        url: "/Tbx/Module/GetMarketClassify?IsMarkBook=true",
        dataType: "json",
        success: function (result) {
            $('#tree').treeview({
                data: result,
                showIcon: false,
                onNodeExpanded: function (event, node) {
                },
                onNodeSelected: function (event, node) {
                    //itemOnclick(node.tag);
                }
            });
        }
    });
}
//点击事件
function itemOnclick(MarketBookID) {
    addcloudNew("加载中...");
    $.post("/Tbx/GoodModuleItem/GetGoodModulePage", { MarketBookID: MarketBookID }, function (data) {
        if (data) {
            var html = "";
            for (var i = 0; i < data.rows.length; i++) {
                html += ModuleHtml(data.rows[i].ModuleID, data.rows[i].ModuleName, data.rows[i].IsChecked, MarketBookID);
            }
            $("#tabModule").html(html);
            $("#btnDiv").show();
        } else {
            $("#btnDiv").hide();
            $("#tabModule").html("<div style='color:red;'><h5>暂无数据！</h5></div>");
        }
        removecloudNew();
    });
}


function ModuleHtml(ModuleID, ModuleName, IsChecked, MarketBookID) {
    var moduleids = $("#moduleids").val();
    var checked = "";
    if (moduleids != "") {
        if (moduleids.indexOf("," + ModuleID + ",") != -1 || moduleids.indexOf("|" + ModuleID + ",") != -1) {
            checked = "checked";
        }
    }
    return " <span  class=\"goodmodule\"><input  onchange=\"ChangeModule(" + MarketBookID + "," + ModuleID + ",this)\"   type='checkbox' id=\"module_" + ModuleID + "\" value='" + ModuleID + "' " + checked + " /><label  style=' cursor: pointer; ' for=\"module_" + ModuleID + "\">" + ModuleName + "</label></span> ";
}

function ChangeModule(MarketBookID, ModuleID, obj) {
    var moduleids = $("#moduleids").val();
    var ids = MarketBookID.toString() + "|" + ModuleID.toString();
    if ($(obj).is(':checked')) {
        if (moduleids != "") {
            $("#moduleids").val(moduleids + ids + ",");
        } else {
            $("#moduleids").val("," + ids + ",");
        }
    } else {
        $("#moduleids").val(moduleids.replace("," + ModuleID + ",", ","));
        if (moduleids.indexOf("," + ids + ",") != -1)
            $("#moduleids").val(moduleids.replace("," + ids + ",", ","));
    }
}

//提交策略配置模块信息
function btnGoodModuleClick() {
    //var MarketClassifyId = '';
    //$("input:checkbox[name='ckGoodModule']:checked").each(function () { 
    //    MarketClassifyId += $(this).val() + ','; 
    //});
    var moduleids = $("#moduleids").val();
    $.post("/Tbx/GoodModuleItem/CommitGoodModule", { MarketClassifyId: moduleids }, function (data) {
        if (data == true) {
            window.parent.RefreshParentGood();
        } else {
            alert("配置失败~");
        }
    });
}