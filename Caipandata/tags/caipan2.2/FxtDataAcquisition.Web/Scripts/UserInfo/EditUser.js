$(function(){
    $("#btnSubmit").bind("click",function(){
        SubmitData();
    });
});
function SubmitData()
{
    $("#errorAlert").hide();
    var userName = $("#txtUserName").val();
    var trueName = $("#txtTrueName").val();
    var departmentId=$("#selectDepartment").val();
    if(departmentId==0||departmentId=="0")
    {
        alert("请选择小组或创建小组");
        return false;
    }
    var roleIds=GetCheckValueByFind($("#cbRoleIds"),',');
    var paraJson = { userName: userName, trueName: trueName, departmentId: departmentId, roleIds: roleIds };
    $("#btnSubmit").val("提交中...");
    $.extendAjax(
                {   
                    url: "/UserInfo/EditUser_SubmitData_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#btnSubmit").val("提交");
                    if(data!=null)
                    {
                        if(data.result!=1)
                        {
                            
                            if(data.errorType==0)
                            {
                                $("#errorAlert").show();
                                $("#errorAlert").html(decodeURIComponent(data.message));
                            }
                            return;
                        }    
                        else
                        {
                            alert("保存成功!");
                            parent.EditUser_Response(data.detail);
                            parent.$.fancybox.close();
                        }
                    }
                },
               {dom:"#SubmitPanel"});
}

function LocationDer(url)
{
    parent.window.location.href=""+url+"";
}