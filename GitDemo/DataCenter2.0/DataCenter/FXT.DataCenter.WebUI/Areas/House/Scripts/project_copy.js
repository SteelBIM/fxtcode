$(function () {
    $("#btnProjectCopy").click(function () {
        var project_data = projectNameList.split("&");
        $("#copy_project_name").autocomplete(project_data, {
            max: 10, //列表里的条目数 
            minChars: 0, //自动完成激活之前填入的最小字符 
            width: 214,
            matchContains: true//包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示 
        }).result(function (e, selval, attr) {
            //debugger;
            //var sel_newval = (selval.toString().split("|"))[0];
            //$("#copy_project_name").val("").val(sel_newval);
        });
        $(this).tb_windowAddFooter({
            sender1: "sender1", //第一个按钮的ID
            name1: "确 定", //第一个按钮的名称
            sender2: "sender2", //第二个按钮的ID
            name2: "取 消", //第二个按钮的名称
            sen1func: function () { //第一个按钮的功能函数
                var projectName = $("#Original_ProjectName").val(); //原始楼盘名称
                var projectName_copy = $("#copy_project_name").val();
                if (projectName == projectName_copy) {
                    alert("原楼盘名称与目标楼盘名称相同");
                    return false;
                }
                //disabled
                $.ajax({
                    type: "POST",
                    dataType: "Json",
                    url: "/house/project/ProjectCopy",
                    data: { ProjectName: projectName_copy, AreaID: $("#areaid").val(), projectId: $("#projectid").val(), othername: $.trim($("#copy_project_othername").val()), address: $.trim($("#copy_project_address").val()), fxtcompanyid: $.trim($("#fxtcompanyid").val()) },
                    cache: false,
                    beforeSend: function () {
                        //layer.load('楼盘复制,请等待....',3);
                        $("#sender1").text("").text("楼盘复制中....");
                        $("#sender1").attr("disabled", "disabled");
                    },
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
});