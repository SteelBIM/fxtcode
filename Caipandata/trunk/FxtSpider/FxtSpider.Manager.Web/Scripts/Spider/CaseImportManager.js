/**1440*900**/
var citySelectDom=null;
$(function () {
    $("#dataPanel").show();
    $(".selectWhere").find('input[type=checkbox],input[type=radio],input[type=file]').uniform();
    BindLikeProjectName();
    /**点击查询**/
    $("#btnSearch").live("click",function(){
        $("#caseList").attr("data", "");
        GetCaseSearchInfo(1);
        return false;
    });
    /**点击入库or未入库**/
    $(".importCase").bind("click",function(){
        var nowVal=$("#hdIsImport").val();
        $("#hdIsImport").val($(this).attr("data"));
        $("#caseList").attr("data", "");
        var result= GetCaseSearchInfo(1);
        if(!result)
        {
           $("#hdIsImport").val(nowVal);
        }
        return result;
    });
   
   
    /**选择城市时绑定下拉匹配**/
    $("#selectCity").bind("change",function(){
        BindLikeProjectName();
    });
/**    $("#tableCase").freezeHeader();//固定表头
//    var nowLeft=$("#topTitle").offset().left*1;
//    $("#tablepanel").scroll(function(){
//        var nowScrollLeft=$(this).scrollLeft()*1        
//        $("#hdtableCase").css("left",(nowLeft-nowScrollLeft)+"px");
//        
//    });**/
    var nowHeight=$("#row_footer").height()*1;
    var nowBottom=$(document).height()-(nowHeight+$("#row_footer").offset().top);
    if(nowBottom>0)
    {
        $("#tableWindow").css("height",($("#tableWindow").height()+nowBottom)+"px"); 
    }
});

/**绑定楼盘名模糊下拉效果**/
function BindLikeProjectName()
{
    $("#txtProjectName").unautocomplete();/**卸载当前事件**/
    var _cityId=$("#selectCity").val().split(',')[0];
    if(_cityId==0||_cityId=="0")
    {
        _cityId="";
    }
    $("#txtProjectName").autocomplete("/Common/GetProjectNameByLike_Api", {
        width:230,
        max: 20,
        multipleSeparator: "",
        scrollHeight: 300,
        autoFill: false,
        searchEval:"click",
        extraParams: { cityId: _cityId },
        formatItem: function (row,i,max) { 
             var obj =eval("(" + row + ")"); 
             return decodeURIComponent(obj.ProjectName)+"&nbsp;&nbsp;&nbsp;&nbsp;<font color='blue'>["+decodeURIComponent(obj.AreaName)+"]</font>";
        },
        formatResult: function(row) {
             var obj =eval("(" + row + ")"); 
             return decodeURIComponent(obj.ProjectName);
        }
    }); 
}

/**绑定分页**/
function BindPage(nowIndex,pageSize,count) 
{
  BindPageCommon("#example",nowIndex,count,pageSize,15,
                                    function (event, originalEvent, type, page) {    
                                        GetCaseSearchInfo(page);
                                    },null);
}
/**查询**/
function GetCaseSearchInfo(_start)
{
    /**参数赋值**/
    var _isGetCount=0;
    var _pageIndex=_start;
    var _pageSize=20;
    var _cityId=$("#selectCity").val().split(',')[0];
    var _webIdsStr=GetCheckVal("#selectWeb");
    var _purposeIdsStr=GetCheckVal("#selectPurpose");
    var _area=$("#selectArea").val().split(',')[1];
    var _projectName=$("#txtProjectName").val();
    var _startCreateDate=$("#startCreateDate").val();
    var _endCreateDate=$("#endCreateDate").val();
    var _startCaseDate=$("#startCaseDate").val();
    var _endCaseDate=$("#endCaseDate").val();
    var _startImportDate=$("#startImportDate").val();
    var _endImportDate=$("#endImportDate").val();
    var _isImport=$("#hdIsImport").val();
    var count_data = $("#caseList").attr("data");
    if (count_data == null || count_data == "") 
    {
       _isGetCount=1;
    }
    /**参数验证**/
    if(_cityId==0||_cityId=="0")
    {
        alert("请选择城市");
        return false;
    }
    /**数据传输**/
    if(_isGetCount==1)
    {
       $("#caseCount").hide();
    }
    if(_isImport=="0"||_isImport==0)
    {   
       _startImportDate="";
       _endImportDate="";
       $("#importDateWhere").hide();
    }
    else
    {
       $("#importDateWhere").show();
    }
    var paraJson = 
    {  
        cityId:_cityId,
        webIdsStr:_webIdsStr,
        purposeIdsStr:_purposeIdsStr,
        area:_area,
        projectName:_projectName,
        startCreateDate:_startCreateDate,
        endCreateDate:_endCreateDate,
        startCaseDate:_startCaseDate,
        endCaseDate:_endCaseDate,
        startImportDate:_startImportDate,
        endImportDate:_endImportDate,
        isImport:_isImport,
        isGetCount:_isGetCount,
        pageIndex:_pageIndex,
        pageSize:_pageSize,
        isGetCount:_isGetCount
    };
    $.extendAjax(
                 { url: "/Spider/CaseImportManager_Search_Api",
                     data: paraJson,
                     type: "post",
                     dataType: "json"
                 },
                 function (data) {
                     $("#caseList").find(".caseInfo").remove();/**清空数据**/
                     var count_data = $("#caseList").attr("data");/**获取总个数(不为第一页时)**/
                     if(count_data==""){count_data=0;}
                     var listCount=0;
                     var isImport=0;
                     var pageCount =0;
                     if (data != null) 
                     {
                         if(data.result!=1&&data.result!="1")
                         {
                             alert(decodeURIComponent(data.message));
                             return;
                         }
                         if (data.detail.IsGetCount == 1)/**获取到了总个数**/ 
                         {
                             var count = data.detail.Count * 1;
                             $("#caseList").attr("data", count);/**设置总个数**/
                             count_data=count;
                             $("#caseCount").show();
                             $("#caseCount").html("总记录数"+count+"个");
                         }
                         if (data.detail.List != null) 
                         {
                             var list = data.detail.List;
                             listCount=list.length;
                             for (var i = 0; i < list.length; i++) 
                             {
                                 var dom = BindCaseInfoHtml(list[i]);
                                 $("#caseList").append(dom);
                             }
                         }
                         if(data.detail.IsImport==1||data.detail.IsImport=="1")/**查询已入库案例时**/
                         {
                             isImport=1;
                             GetCaseFxtProjectByFxtCaseId();
                             $("#tableCase").find(".th_optionType").hide();/**隐藏操作列**/
                             $("#tableCase").find(".optionType").hide();
                             $("#tableCase").find(".th_check").hide();/**隐藏多选框列**/
                             $("#tableCase").find(".check").hide();
                             $("#optionPanel").hide();/**隐藏底部操作按钮**/
                         }
                         else
                         {
                             GetCaseFxtProjectByProjectName();
                             $("#tableCase").find(".th_optionType").show();
                             $("#tableCase").find(".optionType").show();
                             $("#tableCase").find(".th_check").show();
                             $("#tableCase").find(".check").show();
                             $("#optionPanel").show();
                         }
                     }
                     if(listCount>0&&isImport==0)/**有数据并且为查询未入库案例时**/
                     {
                         $("#optionPanel").find(".optionBtn").show();/**显示底部操作按钮**/
                     }
                     else
                     {
                         $("#optionPanel").find(".optionBtn").hide();
                     }
                     pageCount = parseInt(((count_data*1) - 1) / _pageSize) + 1;
                     BindPage(_start, _pageSize,count_data*1);
                 }
                 ,{dom:"#dataPanel"});
   return true;
}
/**获取爬取案例的对应正式库楼盘名**/
function GetCaseFxtProjectByProjectName()
{
    var dom = $("#caseList").find(".caseInfo");
    var _cityName=$("#selectCity").val().split(',')[1];
    var _projectNames="";
    for(var i=0;i<dom.length;i++)
    {
        var projectName=$(dom.get(i)).find(".loupan").find(".text").html();
        projectName=encodeURIComponent(projectName);
        _projectNames=_projectNames+projectName;
        if(i<dom.length-1)
        {
            _projectNames=_projectNames+",";
        }
    }
    $.extendAjax({
                  url:"/Spider/CaseImportManager_GetFxtCaseProjectNameByNowProjectName_Api",
                  data:{cityName:encodeURIComponent(_cityName),projectNamesStr:_projectNames},
                  type:"post",
                  dataType:"json"
                 },
                 function(data){
                     $("#caseList").find(".caseInfo").find(".fxtloupan").find(".test").html("");
                     if(data!=null)
                     {
                         var list=data.detail;
                         if(list!=null)
                         {
                             for(var i=0;i<list.length;i++)
                             {
                                 var obj=list[i];
                                 var name=decodeURIComponent(obj.projectName);
                                 var fxtName=decodeURIComponent(obj.fxtProjectName);
                                 var dom2 = $("#caseList").find(".caseInfo");
                                 for(var j=0;j<dom2.length;j++)
                                 { 
                                     var nowName=$(dom2.get(j)).find(".loupan").find(".text").html();
                                     if(nowName==name)
                                     {
                                         $(dom2.get(j)).find(".fxtloupan").find(".text").html(fxtName);
                                         $(dom2.get(j)).find(".fxtloupan").find(".txt_span").find(".textValue").val(fxtName);
                                     }
                                 }
                             }
                         }
                     }   
    
                 });


}
/**获取爬取案例的对应正式库楼盘名**/
function GetCaseFxtProjectByFxtCaseId()
{
    var dom = $("#caseList").find(".caseInfo");
    var _cityId=$("#selectCity").val().split(',')[0];
    var fxtCaseIds="";
    for(var i=0;i<dom.length;i++)
    {
        var fxtCaseId=$(dom.get(i)).attr("data-fxtId");
        fxtCaseIds=fxtCaseIds+fxtCaseId;
        if(i<dom.length-1)
        {
           fxtCaseIds=fxtCaseIds+",";
        }
    }
    $.extendAjax(
                 { url: "/Spider/CaseImportManager_GetFxtCaseProjectName_Api",
                     data: {cityId:_cityId,fxtIds:fxtCaseIds},
                     type: "post",
                     dataType: "json"
                 },
                 function (data) {
                     if (data != null) 
                         {
                             if(data.result==1||data.result=="1")
                             {
                                 var list=data.detail;
                                 if(list!=null)
                                 {
                                     for(var i=0;i<list.length;i++)
                                     {
                                         var projectName=decodeURIComponent(list[i].ProjectName);
                                         $("#caseList").find(".caseFxtId_"+list[i].FxtId).find(".fxtloupan").find(".text").html(projectName)
                                     }
                                 }
                             }
                         }
                 });

}
/**获取指定标签下checkbox的选中值**/
function GetCheckVal(_dom)
{
  var ids="";
  var checkList=$(_dom).find("input[type='checkbox']");
  for(var i=0;i<checkList.length;i++)
  {
      var isSelect=$(checkList.get(i)).attr("checked");
      if(isSelect=="checked")
      {
          var val=$(checkList.get(i)).val();
          ids=ids+val+",";
      }
  }
  ids=ids.TrimEnd(',');
  return ids;

}

function BindCaseInfoHtml(caseObj) 
{
    var dom = $("#caseList").find("#caseRom").clone();
    var rowNumber="caseList_" + caseObj.ID;
    dom.attr("id", rowNumber).addClass("caseInfo").addClass("caseFxtId_"+caseObj.fxtId).show();
    dom.attr("data-fxtId",caseObj.fxtId);
    dom.attr("data-Id",caseObj.ID);
    dom.find(".Id").find(".text").html(caseObj.ID);
    dom.find(".check").find(".text").val(caseObj.ID);
    dom.find(".optionType").find(".delCase").attr("data-Id",caseObj.ID);
    dom.find(".laiyuan").find(".text").html(decodeURIComponent(caseObj.web));
    dom.find(".chengshi").attr("data-cityId",caseObj.cityId);
    dom.find(".chengshi").find(".text").html(decodeURIComponent(caseObj.city));
    dom.find(".xingzhengqu").find(".text").html(decodeURIComponent(caseObj.areaName));
    dom.find(".pianqu").find(".text").html(decodeURIComponent(caseObj.areaName2));
    dom.find(".loupan").find(".text").html(decodeURIComponent(caseObj.projectName));
    dom.find(".anlishijian").find(".text").html(decodeURIComponent(caseObj.caseDate));
    dom.find(".loudong").find(".text").html(decodeURIComponent(caseObj.building));
    dom.find(".fanghao").find(".text").html(decodeURIComponent(caseObj.houseNumber));
    dom.find(".yongtu").find(".text").html(decodeURIComponent(caseObj.purpose));
    dom.find(".mianji").find(".text").html(decodeURIComponent(caseObj.are));
    dom.find(".danjia").find(".text").html(decodeURIComponent(caseObj.unitPrice));
    dom.find(".zongjia").find(".text").html(decodeURIComponent(caseObj.totalPrice));
    dom.find(".anlileixing").find(".text").html(decodeURIComponent(caseObj.caseType));
    dom.find(".jiegou").find(".text").html(decodeURIComponent(caseObj.structure));
    dom.find(".jianzhuleixing").find(".text").html(decodeURIComponent(caseObj.buildingType));
    dom.find(".zonglouceng").find(".text").html(decodeURIComponent(caseObj.totalFloor));
    dom.find(".suozailouceng").find(".text").html(decodeURIComponent(caseObj.floorNumber));
    dom.find(".huxing").find(".text").html(decodeURIComponent(caseObj.houseType));
    dom.find(".chaoxiang").find(".text").html(decodeURIComponent(caseObj.front));
    dom.find(".zhuangxiu").find(".text").html(decodeURIComponent(caseObj.fitment));
    dom.find(".niandai").find(".text").html(decodeURIComponent(caseObj.buildingDate));
    dom.find(".dianhua").find(".text").html(decodeURIComponent(caseObj.phone));
    dom.find(".titleUrl").find(".text").html(decodeURIComponent(caseObj.url));
    dom.find(".dizhi").find(".text").html(decodeURIComponent(caseObj.address));
    dom.find(".shijian").find(".text").html(decodeURIComponent(caseObj.createDate));
    dom.find(".xingshi").find(".text").html(decodeURIComponent(caseObj.xingshi));
    dom.find(".huayuan").find(".text").html(decodeURIComponent(caseObj.huayuan));
    dom.find(".tingjiegou").find(".text").html(decodeURIComponent(caseObj.tingjiegou));
    dom.find(".chewei").find(".text").html(decodeURIComponent(caseObj.chewei));
    dom.find(".peitao").find(".text").html(decodeURIComponent(caseObj.peitao));
    dom.find(".dixiashi").find(".text").html(decodeURIComponent(caseObj.dixiashi));
    dom.find(".bizhong").find(".text").html(decodeURIComponent(caseObj.moneyUnit));
    dom.find(".infoTitle").find(".text").html(decodeURIComponent(caseObj.title));
    dom.find(".count").find(".text").html(decodeURIComponent(caseObj.count));
    dom.find(".fxtloupan").find(".text").html("数据加载中...");

    var _isImport = $("#hdIsImport").val();
    if (_isImport != 1 && _isImport != "1") 
    {
        var tdDom = dom.find(".btnShow");
    }
    dom.find('input[type=checkbox],input[type=radio],input[type=file]').uniform();
    return dom;
}


/**设置状态为数据传输中**/
function setPageOperationLoading()
{
   $("#caseList").attr("data-operation","1");
}
/**设置状态为数据传输完成**/
function setPageOperationOver()
{
   $("#caseList").attr("data-operation","0");
}
/**判断数据状态**/
function isPageOperationLoading()
{
   var result=$("#caseList").attr("data-operation");
   if(result==1||result=="1")
   {
       return true;
   }
   return false;
}