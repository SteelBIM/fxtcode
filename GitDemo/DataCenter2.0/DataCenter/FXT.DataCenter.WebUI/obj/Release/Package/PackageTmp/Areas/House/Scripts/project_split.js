$(function () {
    //option_lenTo 目标楼盘下的楼栋数
    //option_len 原始楼盘下的楼栋数
    $.ajax({
        url: "/house/project/BindBuildList",
        dataType: "Json",
        data: { projectId: projectId },
        cache: false,
        type: "Post",
        success: function (json) {
            if (json.data) {
                var html = "";
                //<option value="@item.buildingid" id="b_@(item.buildingid)">@item.buildingname</option>
                for (var i = 0; i < json.data.length; i++) {
                    html += "<option value=\"" + json.data[i].buildingid + "\" id=\"" + json.data[i].buildingid + "\">";
                    html += json.data[i].buildingname;
                    html += "</option>";
                }
                $("#ClassLevel").empty().append(html);
            }
        }
    });
    var option_lenTo = 0, option_len = 0;
    $("#btnProjectSplit").click(function () {
        option_lenTo = $("#ClassLevel_sub option").length, option_len = $("#ClassLevel option").length, build_str = "", build_Name = "";
        var project_data = projectNameList.split("&");
        $("#p_nameTo").autocomplete(project_data, {
            max: 10, //列表里的条目数 
            minChars: 0, //自动完成激活之前填入的最小字符 
            width: 214,
            matchContains: true//包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示 
        });
        $(this).tb_windowAddFooter({
            sender1: "sender1", //第一个按钮的ID
            name1: "确 定", //第一个按钮的名称
            sender2: "sender2", //第二个按钮的ID
            name2: "取 消", //第二个按钮的名称
            sen1func: function () { //第一个按钮的功能函数
                var pName = $("#p_name").val(), pNameTo = $("#p_nameTo").val();
                option_lenTo = $("#ClassLevel_sub option").length, option_len = $("#ClassLevel option").length;
                if (pNameTo == null || $.trim(pNameTo) == "") {
                    alert("目标楼盘不能为空");
                    return false;
                }
                if (pName == pNameTo) {
                    alert("原始楼盘和目标楼盘不能相同");
                    return false;
                }
                if (parseInt(option_lenTo)<=0) {
                    alert("请选择楼栋");
                    return false;
                }
                build_str = "", build_Name="";
                $("select[id='ClassLevel_sub'] option").each(function () {
                    build_str += $(this).val() + ",";
                    build_Name += $(this).text() + ",";
                });
                if (build_str != null && build_str != "") {
                    build_str = build_str.substring(0, build_str.length - 1);
                    build_Name = build_Name.substring(0, build_Name.length - 1);
                } else {
                    alert("请选择楼栋");
                    return false;
                }
                $.ajax({
                    type: "POST",
                    dataType: "Json",
                    url: "/house/project/ProjectSplit",
                    data: { ProjectName: pNameTo, AreaID: $("#areaid").val(), projectId: $("#projectid").val(), buidIdList: build_str, build_Name: build_Name, fxtcompanyid: $.trim($("#fxtcompanyid").val()) },
                    cache: false,
                    success: function (json_data) {
                        alert(json_data.reslut);
                        top.tb_remove();
                    },
                    error: function () {
                        alert("error");
                    }
                });
            }
        });

    });
    $("#ClassLevel option").live("click", function () {
        $("#ClassLevel_sub").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
        $(this).remove();
    });
    $("#ClassLevel_sub option").live("click", function () {
        $("#ClassLevel").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
        $(this).remove();
    });
});