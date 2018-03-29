$(function(){
    $("#btnSubmit").bind("click",function(){
        SubmitData();
    });
});
/*********提交数据*****/
function SubmitData()
{
    $("#errorAlert").hide();
    var departmentName=$("#txtDepartmentName").val();
    if(departmentName==null||departmentName=="")
    {
        alert("请输入小组名称");
        return;
    }
    var _Pat = /^[\u4e00-\u9fa5,\a-zA-Z0-9]*$/;
    var bol = _Pat.test(departmentName);
    if(!bol)
    {
        alert("不能输入特殊字符,请重新输入");
        return;
    }
    
    var departmentId=$("#hdDepartmentId").val();
    var paraJson = { id: departmentId, DepartmentName: departmentName };
    $("#btnSubmit").val("提交中...");
    $.extendAjax(
                {   
                    url: "/DepartmentInfo/SetDepartment_SubmitData_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#btnSubmit").val("提交");
                        if(!data.result)
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
                            $("#alert_success").show();
                            setTimeout("parent.$.fancybox.close();", 600);
                            parent.EditDepartment_Response(data.data,departmentId,departmentName);
                            
                        }
                },
               {dom:"#SubmitPanel"});
}