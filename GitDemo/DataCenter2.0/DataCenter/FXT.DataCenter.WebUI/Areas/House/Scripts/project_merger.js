$(function () {
    var projectidTo = 0;
    //$("#Merger_projectname").change(function () {
    //    projectidTo = $(this).find("option:selected").val();
    //});
    $("#btnSubmit").click(function () {
        var projectId = $("#projectidTo").val(); //原始楼盘名称
        var projectidTo = $("#projectid").val(); 
        var areaid = $("#areaid").val(); 
        var fxtcompanyid = $("#fxtcompanyid").val(); 
        if (projectId == projectidTo) {
            alert("请选择您要被合并的楼盘");
            return false;
        }
        $.ajax({
            type: "POST",
            dataType: "Json",
            url: "/house/project/ProjectMerger",
            data: { projectId: projectId, AreaID: areaid, projectidTo: projectidTo, fxtcompanyid: fxtcompanyid },
            success: function (json_data) {
                if (json_data.reslut > 0) {
                    alert("楼盘合并成功");
                    if (window.confirm("您确定要删除被合并的楼盘吗?")) {
                        $.ajax({
                            type: "POST",
                            dataType: "Json",
                            url: "/house/project/ProjectMergerDel",
                            data: { projectidTo: projectidTo, fxtcompanyid: $.trim($("#fxtcompanyid").val()) },
                            async: false,
                            success: function (del_json) {
                                if (del_json.reslut > 0) {
                                    alert("被合并楼盘楼盘删除成功");
                                    parent.location.reload();
                                } else {
                                    alert("被合并楼盘楼盘删除失败");
                                }
                            }
                        });
                    }
                    else {
                        parent.location.reload();
                    }
                }
                else {
                    if (json_data.reslut == 0) {
                        alert("楼盘合并失败");
                    }
                    else if (json_data.reslut == -1) {
                        alert("楼盘合并失败(被合并楼盘和合并楼盘存在相同的楼栋名称)");
                    }
                    else {
                        parent.location.reload();
                    }
                }
            },
            error: function () {
                alert("error");
            }
        })
        return false;
    })
});