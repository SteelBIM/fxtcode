
$(function(){

    $("#txtFloat").html(FloatValueColor($("#txtFloat").html()));
    GetCaseList(1);
    $("#caseList").find(".option").find(".up").live("click",function(){

        var _fxtCityId=$("#hdFxtCityId").val();
        var _projectId=$("#hdProjectId").val();
        var _buildingTypeCode=$("#hdBuildingTypeCode").val()==""?null:$("#hdBuildingTypeCode").val();
        var _purposeCode=$("#hdPurposeCode").val();
        var _areaTypeCode=$("#hdAreaTypeCode").val()==""?null:$("#hdAreaTypeCode").val();
        var _date=$("#hdDate").val()==""?null:$("#hdDate").val();
        var caseId=$(this).attr("data-caseId");

        var url="/OperationMaintenance/SetCase_Fancybox?fxtCityId={0}&projectId={1}&caseId={2}&buildingTypeCode={3}&purposeCode={4}&areaTypeCode={5}&date={6}"
        url=url._StringFormat(_fxtCityId,_projectId,caseId,_buildingTypeCode==null?"":_buildingTypeCode
        ,_purposeCode,_areaTypeCode==null?"":_areaTypeCode,_date==null?"":_date);
        $.fancybox({
            'href': url,/**"/OperationMaintenance/SetCase_Fancybox?fxtCityId=6&projectId=45772&caseId="+caseId+"&buildingTypeCode=2003004&purposeCode=1002001&areaTypeCode=8006004&date=2012-6",**/
            'width':600,
            'height': 500,
            'padding' :0 ,
            'overlayShow': true,
            'autoScale': false,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'type': 'iframe',
            'onClosed' : function(){
            }
        });
        return false;
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
    })
    /**新增案例**/
    $("#addCase").bind("click",function(){
        OpenAddProjectBy();    
    });
    /**删除单条案例**/
    $("#caseList").find(".option").find(".del").live("click",function(){
        var caseId=$(this).attr("data-caseId");
        DeleteCase(caseId);
        return false;
    });
    /**删除选中案例**/
    $("#delCase").live("click",function(){
        var dom=$("#caseList").find(".caseInfo").find(".check").find("input[checked='checked']");
        var ids=GetCheckVal($("#caseList").find(".caseInfo").find(".check"));
        DeleteCase(ids);
    });
    /**重新计算均价**/
    $("#btnResetCross").bind("click",function(){
        ResetCross();
    });
    /**确定修改价格**/
    $("#caseList").find(".danjia").find(".subdanjia").live("click",function(){
        var _caseId=$(this).attr("data-caseId");
        ComputeTotalPrice(_caseId);
        SubmitCase(_caseId);
        BindTableHeader("tableCase","tableWindow");
    });
    /**点击修改价格**/
    $("#caseList").find(".danjia").find(".updanjia").live("click",function(){
        var _caseId=$(this).attr("data-caseId");
        var dom=$("#caseList").find("#caseList_"+_caseId);
        dom.find(".danjia").find("._text2").val(dom.find(".danjia").find("._text").html()).show();
        dom.find(".danjia").find(".subdanjia").show();
        dom.find(".danjia").find(".canceldanjia").show();
        dom.find(".danjia").find("._text").hide();
        dom.find(".danjia").find(".updanjia").hide();
        BindTableHeader("tableCase","tableWindow");
    });
    /**点击取消修改价格**/
    $("#caseList").find(".danjia").find(".canceldanjia").live("click",function(){
        var _caseId=$(this).attr("data-caseId");
        var dom=$("#caseList").find("#caseList_"+_caseId);
        dom.find(".danjia").find(".subdanjia").hide();
        dom.find(".danjia").find(".canceldanjia").hide();
        dom.find(".danjia").find("._text2").val(dom.find(".danjia").find("._text").html());
        dom.find(".danjia").find("._text2").hide();
        dom.find(".danjia").find("._text").show();
        dom.find(".danjia").find(".updanjia").show();
        BindTableHeader("tableCase","tableWindow");
        ComputeTotalPrice(_caseId);
    });
    /**单价改变事件(计算总价)**/
    $("#caseList").find(".danjia").find("._text2").live("input",function(){
        var _caseId=$(this).attr("data-caseId");
        ComputeTotalPrice(_caseId);
        BindTableHeader("tableCase","tableWindow");
    });
    /**固定表头**/
    /**$("#tableCase").freezeHeader();**/
    
});

function ComputeTotalPrice(_caseId)
{  
    var dom=$("#caseList").find("#caseList_"+_caseId);      
    var _Pat = /^\d+\.*\d*$/;
    var utPrice=dom.find(".danjia").find("._text2").val();
    var area=dom.find(".mianji").html();
    if(_Pat.test(utPrice)&&_Pat.test(area))
    {
        dom.find(".zongjia").html(utPrice*area);
    }
}
/**打开新增楼盘**/
function OpenAddProjectBy()
{
    var _fxtCityId=$("#hdFxtCityId").val();
    var _projectId=$("#hdProjectId").val();
    var _buildingTypeCode=$("#hdBuildingTypeCode").val()==""?null:$("#hdBuildingTypeCode").val();
    var _purposeCode=$("#hdPurposeCode").val();
    var _areaTypeCode=$("#hdAreaTypeCode").val()==""?null:$("#hdAreaTypeCode").val();
    var _date=$("#hdDate").val()==""?null:$("#hdDate").val();

    var url="/OperationMaintenance/SetCase_Fancybox?fxtCityId={0}&projectId={1}&buildingTypeCode={2}&purposeCode={3}&areaTypeCode={4}&date={5}"
    url=url._StringFormat(_fxtCityId,_projectId,_buildingTypeCode==null?"":_buildingTypeCode
    ,_purposeCode,_areaTypeCode==null?"":_areaTypeCode,_date==null?"":_date);
    $.fancybox({
        'href':url,/** "/OperationMaintenance/SetCase_Fancybox?fxtCityId=6&projectId=45772&buildingTypeCode=2003004&purposeCode=1002001&areaTypeCode=8006004&date=2012-6",**/
        'width':600,
        'height': 500,
        'padding' :0 ,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed' : function(){
        }
    });
}
function SetCase_Fancybox_Response(_caseObj,_areaList,_actionType)
{
   BindArea(_areaList);   
   var nowCount=0;
   if($("#txtCaseCount").html()!="")
   {
       nowCount=$("#txtCaseCount").html()*1;
   }
   if(_actionType=="add")
   {
       var dom=BindCaseDom(_caseObj,null);
       $("#caseList").prepend(dom);
   }
   else
   {
       BindCaseDom(_caseObj,$("#caseList").find("#caseList_" + _caseObj.CaseID));
   }
   nowCount=nowCount+1;
   $("#txtCaseCount").html(nowCount);
}
/**获取楼盘列表**/
function GetCaseList(_pageIndex)
{
    var _pageSize=20;
    var _fxtCityId=$("#hdFxtCityId").val();
    var _projectId=$("#hdProjectId").val();
    var _buildingTypeCode=$("#hdBuildingTypeCode").val()==""?null:$("#hdBuildingTypeCode").val();
    var _purposeCode=$("#hdPurposeCode").val();
    var _areaTypeCode=$("#hdAreaTypeCode").val()==""?null:$("#hdAreaTypeCode").val();
    var _date=$("#hdDate").val()==""?null:$("#hdDate").val();
    var _isGetCount=0;
    /**参数验证**/
    if(_fxtCityId==0||_fxtCityId=="0")
    {
        alert("请选择城市");
        return false;
    }
    if($("#txtCaseCount").html()=="")
    {
        _isGetCount=1;
    }
    var companyIds=$("#hdFxtCompanyIds").val();
    var paraJson={fxtCityId:_fxtCityId,
                  projectId:_projectId,
                  fxtCompanyIds:companyIds,
                  buildingTypeCode:_buildingTypeCode,
                  purposeCode:_purposeCode,
                  areaTypeCode:_areaTypeCode,
                  date:_date,
                  pageIndex:_pageIndex,
                  pageSize:_pageSize,
                  isGetCount:_isGetCount};
    $.extendAjax(
                {   
                    url: "/OperationMaintenance/CaseList_GetCaseListByProjectIdAndFxtCityIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDate",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#caseList").find(".caseInfo").remove();
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        if($("#txtCaseCount").html()=="")
                        {
                            $("#txtCaseCount").html(data.detail.Count);
                        }
                        BindArea(data.detail.AreaList);
                        /**alert(JSON.stringify(data.detail.List));**/
                        if (data.detail.List != null) 
                        {
                            var projectIds="";
                            var list = data.detail.List;
                            for (var i = 0; i < list.length; i++) 
                            {
                                var caseObj=list[i];
                                var dom =BindCaseDom(caseObj,null);
                                $("#caseList").append(dom);
                            }

                        }
                        var count_data =$("#txtCaseCount").html();
                        var pageCount = parseInt(((count_data*1) - 1) / _pageSize) + 1;
                        BindPage(_pageIndex,_pageSize,count_data);                        
                    }
                    BindTableHeader("tableCase","tableWindow");
                },
               {dom:"#dataPanel"});
}
function BindCaseDom(caseObj,_dom)
{
    var dom = $("#caseList").find("#caseRom").clone();
    if(_dom!=null)
    {
        dom=_dom;
    }
    dom.attr("id", "caseList_" + caseObj.CaseID).addClass("caseInfo").show();
    dom.find(".loupan").attr("data-projectId",caseObj.ProjectId).html(GetProjectName(caseObj.ProjectId));
    dom.find(".loudong").html(caseObj.BuildingId);/**/
    dom.find(".fanghao").html(caseObj.HouseNo==null?"":decodeURIComponent(caseObj.HouseNo));
    dom.find(".yongtu").attr("data-code",caseObj.PurposeCode).html(GetCodeName("PurposeCode",caseObj.PurposeCode));/**/
    dom.find(".mianji").html(caseObj.BuildingArea);/**/
    dom.find(".danjia").find("._text").html(caseObj.UnitPrice);/**/
    dom.find(".danjia").find("._text2").attr("data-caseId",caseObj.CaseID).val(caseObj.UnitPrice).hide();
    dom.find(".danjia").find(".subdanjia").attr("data-caseId",caseObj.CaseID).hide();
    dom.find(".danjia").find(".canceldanjia").attr("data-caseId",caseObj.CaseID).hide();
    dom.find(".danjia").find(".updanjia").attr("data-caseId",caseObj.CaseID);
    dom.find(".zongjia").html(caseObj.TotalPrice);/**/
    dom.find(".anlileixing").attr("data-code",caseObj.CaseTypeCode).html(GetCodeName("CaseTypeCode",caseObj.CaseTypeCode));/**/
    dom.find(".jiegou").attr("data-code",caseObj.StructureCode).html(GetCodeName("StructureCode",caseObj.StructureCode));/**/
    dom.find(".jianzhuleixing").attr("data-code",caseObj.BuildingTypeCode).html(GetCodeName("BuildingTypeCode",caseObj.BuildingTypeCode));/**/
    dom.find(".suozailouceng").html(caseObj.FloorNumber);/**/
    dom.find(".zonglouceng").html(caseObj.TotalFloor);/**/
    dom.find(".huxing").attr("data-code",caseObj.HouseTypeCode).html(GetCodeName("HouseTypeCode",caseObj.HouseTypeCode));/**/
    dom.find(".chaoxiang").attr("data-code",caseObj.FrontCode).html(GetCodeName("FrontCode",caseObj.FrontCode));/**/
    dom.find(".bizhong").attr("data-code",caseObj.MoneyUnitCode).html(GetCodeName("MoneyUnitCode",caseObj.MoneyUnitCode));/**/
    dom.find(".anlishijian").html(caseObj.CaseDate==null?"":decodeURIComponent(caseObj.CaseDate));
    dom.find(".infoTitle").html(caseObj.Remark==null?"":decodeURIComponent(caseObj.Remark));
    dom.find(".laiyuan").html(caseObj.SourceName==null?"":decodeURIComponent(caseObj.SourceName));
    dom.find(".titleUrl").html(caseObj.SourceLink==null?"":decodeURIComponent(caseObj.SourceLink));
    dom.find(".dianhua").html(caseObj.SourcePhone==null?"":decodeURIComponent(caseObj.SourcePhone));
    dom.find(".xingzhengqu").attr("data-areaId",caseObj.AreaId).html(GetAreaName(caseObj.AreaId));/**/
    dom.find(".niandai").html(caseObj.BuildingDate);
    dom.find(".zhuangxiu").attr("data-code",caseObj.FitmentCode).html(GetCodeName("FitmentCode",caseObj.FitmentCode));/**/
    dom.find(".fushu").html(caseObj.SubHouse==null?"":decodeURIComponent(caseObj.SubHouse));
    dom.find(".peitao").html(caseObj.PeiTao==null?"":decodeURIComponent(caseObj.PeiTao));
    dom.find(".ruluren").html(caseObj.Creator==null?"":decodeURIComponent(caseObj.Creator));
    dom.find(".option").find(".up").attr("data-caseId",caseObj.CaseID);
    dom.find(".option").find(".del").attr("data-caseId",caseObj.CaseID);
    dom.find(".check").find(".sel_check").val(caseObj.CaseID);
    
    return dom;
}

/**绑定分页**/
function BindPage(nowIndex,pageSize,count) 
{
  BindPageCommon("#example",nowIndex,count,pageSize,10,
                                    function (event, originalEvent, type, page) {    
                                        GetCaseList(page);
                                    },null);
}
function BindArea(_areaList)
{
    for(var i=0;i<_areaList.length;i++)
    {
        if($("#selectArea").find(".area_"+_areaList[i].AreaId).length<1)
        {
            var option="<option value=\""+_areaList[i].AreaId+"\" class=\"area_"+_areaList[i].AreaId+"\" >"+decodeURIComponent(_areaList[i].AreaName)+"</option>"   
            $("#selectArea").append(option);
        }
    }

}
function GetCodeName(_codeType,_code)
{
    return $("#select"+_codeType).find(".code_"+_code).html();
}
function GetProjectName(_projectId)
{
    return $("#selectProject").find(".project_"+_projectId).html();
}
function GetAreaName(_areaId)
{
    return $("#selectArea").find(".area_"+_areaId).html();
}
function DeleteCase(_caseIds)
{
    if(!confirm("是否确实删除此案例"))
    {
        return;
    }
    if(_caseIds==null||_caseIds=="")
    {
        alert("请选择要删除的案例");
        return;
    }
    var _fxtCityId=$("#hdFxtCityId").val();
    var paraJson={fxtCityId:_fxtCityId,caseIds:_caseIds};
    $.extendAjax(
                {   
                    url: "/OperationMaintenance/CaseList_DeleteCaseByFxtCityIdAndCaseIds",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    if(data)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            ShowError(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            alert("删除成功!");
                            var nowCaseIds=data.detail;
                            if(nowCaseIds!=null&&nowCaseIds!="")
                            {
                                nowCaseIds=nowCaseIds.split(',');
                                for(var i=0;i<nowCaseIds.length;i++)
                                {
                                    $("#caseList").find("#caseList_"+nowCaseIds[i]).remove();
                                }
                            }   
                            var nowCount=0;
                            if($("#txtCaseCount").html()!="")
                            {
                                nowCount=$("#txtCaseCount").html()*1;
                            }
                            nowCount=nowCount-1;
                            $("#txtCaseCount").html(nowCount);
                        }
                    }
                },
                {dom:"#dataPanel"});
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
function ResetCross()
{
    if(!confirm("是否重新计算此交叉值均价"))
    {
        return;
    }
    var _fxtCityId=$("#hdFxtCityId").val();
    var _projectId=$("#hdProjectId").val();
    var _buildingTypeCode=$("#hdBuildingTypeCode").val()==""?null:$("#hdBuildingTypeCode").val();
    var _purposeCode=$("#hdPurposeCode").val();
    var _areaTypeCode=$("#hdAreaTypeCode").val()==""?null:$("#hdAreaTypeCode").val();
    var _date=$("#hdDate").val()==""?null:$("#hdDate").val();
    var paraJson={fxtCityId:_fxtCityId,
                  projectId:_projectId,
                  purposeCode:_purposeCode,
                  buildingTypeCode:_buildingTypeCode,
                  areaTypeCode:_areaTypeCode,
                  date:_date};
    $.extendAjax(
                {   
                    url: "/OperationMaintenance/CaseList_ResetCross",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    if(data)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            ShowError(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            alert("计算完成!");
                            var priceObj=data.detail;
                            if(priceObj!=null)
                            {
                                $("#txtAvgPrice").html(priceObj.AvgPrice);
                                $("#txtFloat").html(FloatValueColor(priceObj.FloatValue));
                            }
                        }
                    }
                },
                {dom:"#dataPanel"});  
}


/*更新案例(价格)*/
function SubmitCase(_caseId)
{
    var dom = $("#caseList").find("#caseList_" + _caseId);
    dom.find(".danjia").find("._text2")
    var _fxtCityId=$("#hdFxtCityId").val();
    var _projectId=$("#hdProjectId").val();  
    var _unitPrice=dom.find(".danjia").find("._text2").val();
    var _totalPrice=dom.find(".zongjia").html();
    var _Pat = /^\d+\.*\d*$/;
    if(_unitPrice=="")
    {
        alert("单价不能为空!");
        return;
    }
    if(_totalPrice=="")
    {
        alert("总价不能为空!",".txtTotalPrice");
        return;
    }  
    bol = _Pat.test(_unitPrice);
    if(!bol)
    {
        alert("请正确填写单价!",".txtUnitPrice");
        return;
    }  
    bol = _Pat.test(_unitPrice);
    if(!bol)
    {
        alert("请正确填写总价!",".txtTotalPrice");
        return;
    }
    var paraJson={
            caseId:_caseId,
            fxtCityId:_fxtCityId,
            unitPrice:_unitPrice,
            totalPrice:_totalPrice
    }
    $.extendAjax(
                {   
                    url: "/OperationMaintenance/CaseList_SubmitPrice_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            dom.find(".danjia").find("._text").html(_unitPrice).show();
                            dom.find(".danjia").find(".subdanjia").hide();
                            dom.find(".danjia").find(".canceldanjia").hide();
                            dom.find(".danjia").find("._text2").hide();
                            dom.find(".danjia").find(".updanjia").show();
                            alert("保存成功!");
                        }
                    }
                },
               {dom:"#dataPanel"});
}

/**固定表头公共方法**/
function BindTableHeader(_tableDomId,_tableWindowDomId)
{
    var nowId="tableHeader_"+_tableDomId;
    if($("body").find("#"+nowId).length<1)
    {
        var tableHeader=$("#"+_tableDomId).clone();
        tableHeader.attr("id",nowId);
        tableHeader.find("tbody").remove();
        tableHeader.css("position","absolute");
        tableHeader.css("margin-bottom","0px");        
        tableHeader.hide();
        tableHeader.find("tr[id!='']").attr("id","")
        tableHeader.find("th[id!='']").attr("id","")
        tableHeader.find("div[id!='']").attr("id","")
        tableHeader.find("span[id!='']").attr("id","")
        tableHeader.find("a[id!='']").attr("id","")
        tableHeader.find("font[id!='']").attr("id","")
        tableHeader.find("thead[id!='']").attr("id","")
        $("#"+_tableDomId).before(tableHeader);
    }

    var tableHeaderDom=$("#tableHeader_"+_tableDomId);
    tableHeaderDom.css("width",($("#"+_tableDomId).width()+1)+"px");
    var trHeaderDmos=tableHeaderDom.find("thead").find("tr").find("th");
    var trDoms=$("#"+_tableDomId).find("thead").find("tr").find("th");
    for(var i=0;i<trDoms.length;i++)
    {
        var trW=$(trDoms.get(i)).width();
        $(trHeaderDmos.get(i)).css("width",trW+"px");
    }
    $("#"+_tableWindowDomId).unbind("scroll");
    $("#"+_tableWindowDomId).scroll(function(){
        var nowScrollTop=$(this).scrollTop()*1        
        $("#"+nowId).css("top",nowScrollTop+"px");
        if(nowScrollTop>0)
        {
            $("#"+nowId).show();
        }
        else
        {
            $("#"+nowId).hide();
        }
        
    });

}



