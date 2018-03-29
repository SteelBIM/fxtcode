$(function(){
    $("#publicAvgPriceRowTitle").find("a").bind("click",function(){
        var columnName=$(this).attr("data").split(',')[0];
        var dec=$(this).attr("data").split(',')[1];
        OrderFloatValue(columnName,dec);
        $("#publicAvgPriceRowTitle").find("a").removeClass("order_select");
        $(this).addClass("order_select");

    });
});
function Fancybox_PublicCaseCrossPrice()
{
    $.fancybox({          
            'speedIn':100,
            'speedOut':0,
            'padding': 0 ,
		    'overlayOpacity' : 0,
            'hideOnOverlayClick': false, 
            'href':'#alert_pubAvgPrice',
            'onClosed':function(){$("#alert_pubAvgPrice").attr("data-fancyboxOpen","0");}
    });  
}
function OpenPublicCaseCrossPrice(_projectId,_fxtCityId,_date)
{
    var rowLength=4;
    $("#publicCrossRowList").find(".publicCrossRow").remove();
    for(var i=0;i<rowLength;i++)
    {
        var rowInfo=$("#publicCrossRow").clone();
        rowInfo.attr("id","").addClass("publicCrossRow").show();
        rowInfo.find(".buildingTypeName").html("");
        rowInfo.find(".buildingArea30").html("");
        rowInfo.find(".buildingArea3060").html("");
        rowInfo.find(".buildingArea6090").html("");
        rowInfo.find(".buildingArea90120").html("");
        rowInfo.find(".buildingArea120").html("");
        $("#publicCrossRowList").append(rowInfo);
    }
    $("#alert_pubAvgPrice").attr("data-fancyboxOpen","1");
    Fancybox_PublicCaseCrossPrice();
    var blockPara={message:"<img src=\"/Images/loading3.gif\" width='50px' heigth='50px'/>",
                                                                    overlayCSS:{backgroundColor:"White",cursor:"",opacity:0.3},
                                                                    centerY:false,
                                                                    css:{border:"none",backgroundColor:"none"}
                                                                    }; 
    
    var companyIds=GetFxtShowDataCaseCompanyIds(_fxtCityId);
    var paraJson={projectId:_projectId,fxtCityId:_fxtCityId,fxtCompanyIds:companyIds,date:_date};
    $.extendAjax(
    {   
        url: "/OperationMaintenance/CasePriceAnalyse_GetPublicCrossAvgPriceByProjectIdAndDate_Api",
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
                return;
            }  
            var list=data.detail.List;
            if(list!=null&&list.length>0)
            {
                if(list[0].ProjectId!=_projectId||list[0].CityId!=_fxtCityId||list[0].Date!=_date)
                {
                    return;
                }
            }
            BindPublicAvgPriceRow(list);
            $("#hdPublicJsonData").val(JSON.stringify(list));
            if($("#alert_pubAvgPrice").attr("data-fancyboxOpen")=="1")
            {
                Fancybox_PublicCaseCrossPrice();
            }
            SetFxtShowDataCaseCompanyIds(data.detail.FxtCityId,data.detail.FxtCompanyIds);
        } 
    },{dom:"#alert_pubAvgPrice",blockObj:blockPara}
    );
    
}
function BindPublicAvgPriceRow(_ObjList)
{
    for(var i=0;i<_ObjList.length;i++)
    {
        var obj=_ObjList[i];
        var rowDom=$($("#publicCrossRowList").find(".publicCrossRow").get(i));
        if(rowDom!=null)
        {
            var fcityid=obj.CityId; var pjId=obj.ProjectId; var btCode=obj.BuildingTypeCode;
            var pCode=obj.PurposeCode; var _nowdate=obj.Date
            rowDom.find(".buildingTypeName").html(obj.BuildingTypeCodeName);
            var area1Value=obj.Area1AvgPrice+" ("+obj.Area1Count+") "+ FloatValueColor(parseInt(obj.Area1FloatValue * 100) + "%");
            rowDom.find(".buildingArea30").html(GetPublicA(area1Value,fcityid,pjId,btCode,pCode,obj.Area1Code,_nowdate));
            var area2Value=obj.Area2AvgPrice+" ("+obj.Area2Count+") "+ FloatValueColor(parseInt(obj.Area2FloatValue * 100) + "%");
            rowDom.find(".buildingArea3060").html(GetPublicA(area2Value,fcityid,pjId,btCode,pCode,obj.Area2Code,_nowdate));
            var area3Value=obj.Area3AvgPrice+" ("+obj.Area3Count+") "+ FloatValueColor(parseInt(obj.Area3FloatValue * 100) + "%");
            rowDom.find(".buildingArea6090").html(GetPublicA(area3Value,fcityid,pjId,btCode,pCode,obj.Area3Code,_nowdate));
            var area4Value=obj.Area4AvgPrice+" ("+obj.Area4Count+") "+ FloatValueColor(parseInt(obj.Area4FloatValue * 100) + "%");
            rowDom.find(".buildingArea90120").html(GetPublicA(area4Value,fcityid,pjId,btCode,pCode,obj.Area4Code,_nowdate));
            var area5Value=obj.Area5AvgPrice+" ("+obj.Area5Count+") "+ FloatValueColor(parseInt(obj.Area5FloatValue * 100) + "%");
            rowDom.find(".buildingArea120").html(GetPublicA(area5Value,fcityid,pjId,btCode,pCode,obj.Area5Code,_nowdate));
        }
    }
}
function GetPublicA(text,_fxtCityId,_projectId,_buildingTypeCode,_purposeCode,_areaTypeCode,_date)
{
    var url="/OperationMaintenance/CaseList?fxtCityId={0}&projectId={1}&buildingTypeCode={2}&purposeCode={3}&areaTypeCode={4}&date={5}";
    url=url._StringFormat(_fxtCityId,_projectId,_buildingTypeCode,_purposeCode,_areaTypeCode,_date);
    var html="<a href=\"{0}\"  target=\"_blank\">{1}</a>";
    return html._StringFormat(url,text);
}
/**js排序(普通住宅)**/
function OrderFloatValue(_orderBy,_direction)
{
    var _nowJsonData=$("#hdPublicJsonData").val();
    if(_nowJsonData!=null&&_nowJsonData!=""&&_orderBy!=null&&_orderBy!=""&&_direction!=null&&_direction!="")
    {
        var jsonObj=JSON.parse(_nowJsonData);
        jsonObj.sort(function (a, b) {
            if(_direction=="asc")
            {
                return getAbsValue(a[_orderBy]) > getAbsValue(b[_orderBy]) ? 1 : getAbsValue(a[_orderBy]) == getAbsValue(b[_orderBy]) ? 0 : -1;
            }
            return getAbsValue(a[_orderBy]) < getAbsValue(b[_orderBy]) ? 1 : getAbsValue(a[_orderBy]) == getAbsValue(b[_orderBy]) ? 0 : -1;
         }); 
        BindPublicAvgPriceRow(jsonObj);
    }
}
function Fancybox_VillaCaseCrossPrice()
{
    $.fancybox({   
		    'height': 180,        
            'speedIn':100,
            'speedOut':0,
            'padding': 0 ,
		    'overlayOpacity' : 0,
            'hideOnOverlayClick': false, 
            'href':'#alert_villaAvgPrice',
            'onClosed':function(){$("#alert_villaAvgPrice").attr("data-fancyboxOpen","0");}
    });  
}
function OpenVillaCaseCrossPrice(_projectId,_fxtCityId,_date)
{

    var rowLength=1;
    $("#villaCrossRowList").find(".villaCrossRow").remove();
    for(var i=0;i<rowLength;i++)
    {
        var rowInfo=$("#villaCrossRow").clone();
        rowInfo.attr("id","").addClass("villaCrossRow").show();
        rowInfo.find(".purposeType1").html("");
        rowInfo.find(".purposeType2").html("");
        rowInfo.find(".purposeType3").html("");
        rowInfo.find(".purposeType4").html("");
        rowInfo.find(".purposeType5").html("");
        $("#villaCrossRowList").append(rowInfo);
    }
    $("#alert_villaAvgPrice").attr("data-fancyboxOpen","1");
    Fancybox_VillaCaseCrossPrice();
    
    var blockPara={message:"<img src=\"/Images/loading3.gif\" width='25px' heigth='25px'/>",
                                                                    overlayCSS:{backgroundColor:"White",cursor:"",opacity:0.3},
                                                                    centerY:false,
                                                                    css:{border:"none",backgroundColor:"none"}
                                                                     }; 
           
    var companyIds=GetFxtShowDataCaseCompanyIds(_fxtCityId);                                                        
    var paraJson={projectId:_projectId,fxtCityId:_fxtCityId,fxtCompanyIds:companyIds,date:_date};
    $.extendAjax(
    {   
        url: "/OperationMaintenance/CasePriceAnalyse_GetVillaCrossAvgPriceByProjectIdAndDate_Api",
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
                return;
            }  
            var list=data.detail.List;
            if(list!=null&&list.length>0)
            {
                if(list[0].ProjectId!=_projectId||list[0].CityId!=_fxtCityId||list[0].Date!=_date)
                {
                    return;
                }
            }
            for(var i=0;i<list.length;i++)
            {
                var obj=list[i];
                var rowDom=$($("#villaCrossRowList").find(".villaCrossRow").get(i));
                if(rowDom!=null)
                {
                    var purpose1Value=obj.Purpose1AvgPrice+" ("+obj.Purpose1Count+") "+ FloatValueColor(parseInt(obj.Purpose1FloatValue * 100) + "%");
                    rowDom.find(".purposeType1").html(GetVillaA(purpose1Value,obj.CityId,obj.ProjectId,obj.Purpose1Code,obj,Date));
                    var purpose2Value=obj.Purpose2AvgPrice+" ("+obj.Purpose2Count+") "+ FloatValueColor(parseInt(obj.Purpose2FloatValue * 100) + "%");
                    rowDom.find(".purposeType2").html(GetVillaA(purpose2Value,obj.CityId,obj.ProjectId,obj.Purpose2Code,obj,Date));
                    var purpose3Value=obj.Purpose3AvgPrice+" ("+obj.Purpose3Count+") "+ FloatValueColor(parseInt(obj.Purpose3FloatValue * 100) + "%");
                    rowDom.find(".purposeType3").html(GetVillaA(purpose3Value,obj.CityId,obj.ProjectId,obj.Purpose3Code,obj,Date));
                    var purpose4Value=obj.Purpose4AvgPrice+" ("+obj.Purpose4Count+") "+ FloatValueColor(parseInt(obj.Purpose4FloatValue * 100) + "%");
                    rowDom.find(".purposeType4").html(GetVillaA(purpose4Value,obj.CityId,obj.ProjectId,obj.Purpose4Code,obj,Date));
                    var purpose5Value=obj.Purpose5AvgPrice+" ("+obj.Purpose5Count+") "+ FloatValueColor(parseInt(obj.Purpose5FloatValue * 100) + "%");
                    rowDom.find(".purposeType5").html(GetVillaA(purpose5Value,obj.CityId,obj.ProjectId,obj.Purpose5Code,obj,Date));
                }
            }
            if($("#alert_villaAvgPrice").attr("data-fancyboxOpen")=="1")
            {
                Fancybox_VillaCaseCrossPrice();
            }
            SetFxtShowDataCaseCompanyIds(data.detail.FxtCityId,data.detail.FxtCompanyIds);
        } 
    },{dom:"#alert_villaAvgPrice",blockObj:blockPara}
    );
}
function GetVillaA(text,_fxtCityId,_projectId,_purposeCode,_date)
{
    var url="/OperationMaintenance/CaseList?fxtCityId={0}&projectId={1}&purposeCode={2}&date={3}";
    url=url._StringFormat(_fxtCityId,_projectId,_purposeCode,_date);
    var html="<a href=\"{0}\"  target=\"_blank\">{1}</a>";
    return html._StringFormat(url,text);
}