$(function(){
    /**新建楼盘**/
    $(".addProject").live("click",function(){
       OpenAddProject();
       return false;
    });

    /**导入案例**/
    $("#btnImport").live("click",function(){
        var dom=$("#caseList").find(".caseInfo").find(".check").find("input[checked='checked']");
        var ids=GetCheckVal($("#caseList").find(".caseInfo").find(".check"));
        ImportNowPageCase(ids);
    });
    /**全选**/
    $(".cbSelectAll").live("click",function(){
        var checkedValue=$(this).attr("checked");
        if(checkedValue=="checked")
        {
            checkedValue=true;
        }
        else
        {
            checkedValue=false;
        }
        $("#caseList").find("input[type='checkbox']").attr("checked",checkedValue);
        $(".cbSelectAll").attr("checked",checkedValue);
        $("#caseList").find("input[type='checkbox']").uniform();
    });
    /**删除选中该项**/
    $("#btnDelSelect").live("click",function(){
        var dom=$("#caseList").find(".caseInfo").find(".check").find("input[checked='checked']");
        var ids=GetCheckVal($("#caseList").find(".caseInfo").find(".check"));
        DeleteCaseById(ids);
    });
    /**删除当前**/
    $("#caseList").find(".optionType").find(".delCase").live("click",function(){
        var id=$(this).attr("data-Id");
        DeleteCaseById(id);
    });
});
/**打开新增楼盘**/
function OpenAddProject()
{
   $.fancybox({
	    'href': "/Project/AddProject_Fancybox?cityName="+encodeURIComponent("深圳"),
	    'width':600,
		'height': 380,
		'padding' :0 ,
		'overlayShow': true,
		'autoScale': false,
		'transitionIn': 'none',
		'transitionOut': 'none',
		'type': 'iframe'
   });
}

function DeleteCaseById(_caseIds)
{
    if(!confirm("确定删除此信息"))
    { 
        return;
    }
    if(_caseIds==null||_caseIds=="")
    {
        alert("请选择要删除的案例");
        return;
    }
    $.extendAjax(
                 { 
                    url:"/Spider/CaseImportManager_DeleteCaseByIds_Api",
                    data:{caseIds:_caseIds},
                    type:"post",
                    dataType:"json"
                 },
                 function(data){
                     if(data.result==1||data.reslut=="1")
                     {
                         $.extendAlter("删除成功",function(){ });
                         var ids=_caseIds.split(',');
                         for(var i=0;i<ids.length;i++)
                         {
                             if(ids[i]!=null&&ids[i]!="")
                             {
                                 $("#caseList").find("#caseList_" +ids[i]).remove();
                             }
                         }
                     }
                     else
                     {
                         alert(decodeURIComponent(data.message));
                     }
                 },{dom:"#dataPanel"});
}
function ImportNowPageCase(_caseIds) 
{
    if(_caseIds==null||_caseIds=="")
    {
        alert("请选择要导入的案例");
        return;
    }
    $.extendAjax(
                 { 
                    url:"/Spider/CaseImportManager_ImportCaseByIds_Api",
                    data:{caseIds:_caseIds},
                    type:"post",
                    dataType:"json"
                 },
                 function(data){
                     if(data.result==1||data.reslut=="1")
                     {
                         
                         $.extendAlter("导入完成",function(){
                             if(data.detail.succeedCount>0)
                             {
                                 GetCaseSearchInfo(1);                       
                             }
                             if(data.detail.upCount!=data.detail.succeedCount)
                             {
                                 var str="上传{0}个,成功{1}个,失败ID:{2}"._StringFormat(data.detail.upCount,data.detail.succeedCount,data.detail.failId);
                                 alert(str);
                             }
                         });
                     }
                     else
                     {
                         alert(decodeURIComponent(data.message));
                     }
                 },{dom:"#dataPanel"});
}