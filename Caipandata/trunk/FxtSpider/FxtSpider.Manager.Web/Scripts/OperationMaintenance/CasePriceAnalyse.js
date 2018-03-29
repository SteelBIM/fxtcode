var PriceNowDateTime=new Date();
$(function () {
    PriceNowDateTime=new Date($("#nowDateTime").val());
    $(".where").find("select").select2();
    /*查询*/
    $("#btnSearch").bind("click",function(){
        var mothsCount=$("#selectDate").val()*1;
        SetDateColumn(mothsCount)
        $("#example").attr("data-count","");
        GetProjects(1);
    });
    /*查勘交叉均价*/
    $("#contentColumn").find(".aClick").live("click",function(){
        var projectId=$(this).attr("data-projectid");
        var fxtCityId=$(this).attr("data-cityid");
        var date=$(this).attr("data-date");
        if($(this).hasClass("aClick_Public"))
        {
            OpenPublicCaseCrossPrice(projectId,fxtCityId,date);
        }
        else{
            OpenVillaCaseCrossPrice(projectId,fxtCityId,date);
        }
        return false;
    });
    /*排序*/
    $("#topTitle_tr2").find(".order").live("click",function(){
        var columnName=$(this).attr("data").split(',')[0];
        var dec=$(this).attr("data").split(',')[1];
        OrderProjectFloatValue(columnName,dec);
        $("#topTitle_tr2").find(".order").removeClass("order_select");
        $(this).addClass("order_select");
    });
    var nowHeight=$("#row_footer").height()*1;
    var nowBottom=$(document).height()-(nowHeight+$("#row_footer").offset().top);
    if(nowBottom>0)
    {
        $("#divTable").css("height",($("#divTable").height()+nowBottom)+"px"); 
    }
});
var SelectDates="";
var StartDate="";
/**设置表头和基础行**/
function SetDateColumn(mothsCount)
{
    SelectDates="";
    var nowYear=PriceNowDateTime.getFullYear();
    var nowMonth=PriceNowDateTime.getMonth()+1;
    if(PriceNowDateTime.getDate()>=21)
    {
      nowMonth=nowMonth+1;
    }
    var startYear=nowYear;
    var startMonth=nowMonth;
    if(nowMonth<=mothsCount)
    {
        startMonth=12-(mothsCount-nowMonth);
        startYear=startYear-1;
    }
    else
    {
        startMonth=startMonth-mothsCount;
    }
    StartDate=startYear+"-"+(startMonth<=9?"0"+startMonth:startMonth);
    
    $("#topTitle").find("#topTitle_tr").find(".dateColumn").remove();
    $("#topTitle").find("#topTitle_tr2").find(".dateColumn2").remove();
    $("#topTitle").find("#topTitle_tr2").find(".dateColumn3").remove();
    $("#contentColumn").find(".publicColumn").remove();
    $("#contentColumn").find(".villaColumn").remove();
    $("#contentColumn").find(".publicColumnValue").remove();
    $("#contentColumn").find(".villaColumnValue").remove();
    for(var i=0;i<mothsCount;i++)
    {
        var nowDate="";
        if(startMonth>=12)
        {
            startMonth=1;  
            startYear=startYear+1;
        }
        else 
        {
            startMonth=startMonth+1;
        }
        nowDate=startYear+"-"+(startMonth<=9?"0"+startMonth:startMonth);
        SelectDates=SelectDates+nowDate;
        if(i<mothsCount-1)
        {
           SelectDates=SelectDates+",";
        }
        /********设置表头********/
        /*日期头部*/
        var titleColumn=$("#dateColumn").clone();
        titleColumn.attr("id","").addClass("dateColumn").show();
        titleColumn.find(".font_moths").html(nowDate);
        $("#topTitle").find("#topTitle_tr").append(titleColumn);
        /*普通住宅,别墅头部*/
        var titleColumn2=$("#dateColumn2").clone();
        var titleColumn3=$("#dateColumn3").clone();
        titleColumn2.attr("id","").addClass("dateColumn2").show();
        titleColumn3.attr("id","").addClass("dateColumn3").show();
        titleColumn2.find(".aOrderDesc").attr("data","PublicFloat_"+nowDate+",desc");
        titleColumn2.find(".aOrderAse").attr("data","PublicFloat_"+nowDate+",asc");
        titleColumn3.find(".aOrderDesc").attr("data","VillaFloat_"+nowDate+",desc");
        titleColumn3.find(".aOrderAse").attr("data","VillaFloat_"+nowDate+",asc");
        $("#topTitle").find("#topTitle_tr2").append(titleColumn2);
        $("#topTitle").find("#topTitle_tr2").append(titleColumn3);

        /********设置行内容列个数********/
        var publicColumnValue=$("#publicColumnValue").clone();
        publicColumnValue.attr("id","").addClass("publicColumnValue").addClass("publicColumnValue_"+nowDate).show()
        var villaColumnValue=$("#villaColumnValue").clone();
        villaColumnValue.attr("id","").addClass("villaColumnValue").addClass("villaColumnValue_"+nowDate).show()
        $("#purposeColumn").append(publicColumnValue);
        $("#purposeColumn").append(villaColumnValue)
    }
    var rowCount=$("#selectRowCount").val()*1;
}
/**获取楼盘列表**/
function GetProjects(_pageIndex)
{
    $("#topTitle_tr2").find(".order").hide();  
    var _pageSize=20;
    var _cityId=$("#selectCity").val().split(',')[0];
    var _cityName=$("#selectCity").val().split(',')[1];
    var _isGetCount="0";
    /**参数验证**/
    if(_cityId==0||_cityId=="0")
    {
        alert("请选择城市");
        return false;
    }
    if($("#example").attr("data-count")=="")
    {
        _isGetCount="1";
    } 
    $("#hdProjectPriceJsonData").val("");
    var paraJson={cityName:encodeURIComponent(_cityName),pageIndex:_pageIndex,pageSize:_pageSize,isGetCount:_isGetCount};
    $.extendAjax(
                {   
                    url: "/OperationMaintenance/CasePriceAnalyse_GetProjectListByCityName_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#contentColumn").find(".purposeColumn").remove();
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        if($("#example").attr("data-count")=="")
                        {
                            $("#example").attr("data-count",data.detail.Count);
                        }
                        if (data.detail.List != null) 
                        {
                            var projectIds="";
                            var list = data.detail.List;
                            projectIds=BindProjectDom(list,false);
                            GetColumnValueByProjectId(projectIds,list[0].CityID);

                        }
                        var count_data = $("#example").attr("data-count");
                        var pageCount = parseInt(((count_data*1) - 1) / _pageSize) + 1;
                        BindPage(_pageIndex,_pageSize, count_data*1);                        
                    }
                    SetTablePanelWidth();
                },
               {dom:"#dataPanel"});
}
/***绑定楼盘html信息*****/
function BindProjectDom(list,isOrderBy)
{  
    var dates=SelectDates.split(',');
    var addJson="";
    var projectIds="";
    for (var i = 0; i < list.length; i++) 
    {
        var purposeColumn=$("#purposeColumn").clone();
        purposeColumn.attr("id","purposeColumn_"+list[i].ProjectId).addClass("purposeColumn").show();
        purposeColumn.attr("data-index",i);
        purposeColumn.find(".projectNameValue").html(decodeURIComponent(list[i].ProjectName));
        purposeColumn.attr("data-projectid",list[i].ProjectId);
        purposeColumn.attr("data-cityid",list[i].CityID);
        purposeColumn.find(".publicColumnValue").find(".aClick").html("数据加载中...");
        purposeColumn.find(".villaColumnValue").find(".aClick").html("数据加载中...");
        if(!isOrderBy)/*不为排序时的绑定*/
        {
           var nowJson=JSON.stringify(list[i]);
           purposeColumn.find(".nowJsonData").find(".hdJsonData").val(nowJson);
           purposeColumn.addClass("isloadprice");/*当前为加载均价中*/
        }
        $("#contentColumn").append(purposeColumn);
        projectIds=projectIds+list[i].ProjectId;
        if(i<list.length-1)
        {
            projectIds=projectIds+",";
        }
    } 
    return projectIds;
}
/**重置表格div宽度**/
function SetTablePanelWidth()
{
    $("#divTable").css("width","5000px");
    var nowW=$("#topTitle").width()*1+16;
    $("#divTable").css("width",nowW+"px");

}
/**绑定分页**/
function BindPage(nowIndex,pageSize,count) 
{
  BindPageCommon("#example",nowIndex,count,pageSize,15,
                                    function (event, originalEvent, type, page) {    
                                        GetProjects(page);
                                    },null);
}
/**根据一个或多个楼盘id获取均价**/
function GetColumnValueByProjectId(_projectId,_fxtCityId)
{ 
    var companyIds=GetFxtShowDataCaseCompanyIds(_fxtCityId);
    var paraJson={dates:StartDate+","+SelectDates,projectIds:_projectId,fxtCityId:_fxtCityId,fxtCompanyIds:companyIds};
    $.extendAjax(
    {   
        url: "/OperationMaintenance/CasePriceAnalyse_GetPriceByMonthsAndProjectIds_Api",
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
            BindProjectPriceDom(list,false);
            SetFxtShowDataCaseCompanyIds(data.detail.FxtCityId,data.detail.FxtCompanyIds);
        } 
        SetTablePanelWidth();              
    });
}
function GetFxtShowDataCaseCompanyIds(fxtCityId)
{
    if($("#fxtShowDataCaseCompanyIds_"+fxtCityId).length<1)
    {
        var dom=$("#fxtShowDataCaseCompanyIds").clone();
        dom.attr("id","fxtShowDataCaseCompanyIds_"+fxtCityId);
        $("#divFxtCompanyIds").append(dom);
    }
    return $("#fxtShowDataCaseCompanyIds_"+fxtCityId).val();
}
function SetFxtShowDataCaseCompanyIds(fxtCityId,companyIds)
{
    if($("#fxtShowDataCaseCompanyIds_"+fxtCityId).length<1)
    {
        var dom=$("#fxtShowDataCaseCompanyIds").clone();
        dom.attr("id","fxtShowDataCaseCompanyIds_"+fxtCityId);
        $("#divFxtCompanyIds").append(dom);
    }
    if($("#fxtShowDataCaseCompanyIds_"+fxtCityId).val()=="")
    {
        $("#fxtShowDataCaseCompanyIds_"+fxtCityId).val(companyIds);
    }
}
/**绑定均价html**/
function BindProjectPriceDom(list,orderBy)
{ 
    for(var i=0;i<list.length;i++)
    {
        var valueObj=list[i];
        if(valueObj!=null)
        {
            var purposeColumn=$("#purposeColumn_"+valueObj.ProjectId);
            purposeColumn.removeClass("isloadprice");/*标记为均价已加载*/ 
            var nowJson="";
            var nowProjectJson=purposeColumn.find(".nowJsonData").find(".hdJsonData").val().Trim('}').Trim('{');                    
            var _dates=SelectDates.split(',');
            for(var j=0;j<_dates.length;j++)
            { 
                if(!orderBy)/*如果当前数据不是在排序过程中绑定*/
                {
                    nowJson=nowJson+",\"PublicFloat_"+_dates[j]+"\":"+valueObj["PublicFloat_"+_dates[j]];
                    nowJson=nowJson+",\"VillaFloat_"+_dates[j]+"\":"+valueObj["VillaFloat_"+_dates[j]];
                    nowJson=nowJson+",\"PublicAvgPrice_"+_dates[j]+"\":"+valueObj["PublicAvgPrice_"+_dates[j]];
                    nowJson=nowJson+",\"VillaAvgPrice_"+_dates[j]+"\":"+valueObj["VillaAvgPrice_"+_dates[j]];
                    nowJson=nowJson+",\"PublicCount_"+_dates[j]+"\":"+valueObj["PublicCount_"+_dates[j]];
                    nowJson=nowJson+",\"VillaCount_"+_dates[j]+"\":"+valueObj["VillaCount_"+_dates[j]];
                }

                var publicAvgPrice=valueObj["PublicAvgPrice_"+_dates[j]];
                var publicCount=valueObj["PublicCount_"+_dates[j]];
                var publicFloat=parseInt((valueObj["PublicFloat_"+_dates[j]] * 100)) + "%";
                var villaAvgPrice=valueObj["VillaAvgPrice_"+_dates[j]];
                var villaCount=valueObj["VillaCount_"+_dates[j]];
                var villaFloat=parseInt((valueObj["VillaFloat_"+_dates[j]] * 100)) + "%";
                var aClickDom=purposeColumn.find(".publicColumnValue_"+_dates[j]).find(".aClick");
                var aClickDom2=purposeColumn.find(".villaColumnValue_"+_dates[j]).find(".aClick");
                aClickDom.html(publicAvgPrice+" ("+publicCount+") "+FloatValueColor(publicFloat)).addClass("aClick_Public");
                aClickDom2.html(villaAvgPrice+" ("+villaCount+") "+FloatValueColor(villaFloat)).addClass("aClick_Villa");; 
                aClickDom.attr("data-projectid",valueObj.ProjectId); 
                aClickDom.attr("data-cityid",valueObj.CityID);
                aClickDom.attr("data-date",_dates[j]);
                aClickDom2.attr("data-projectid",valueObj.ProjectId); 
                aClickDom2.attr("data-cityid",valueObj.CityID);
                aClickDom2.attr("data-date",_dates[j]);

            }
            if(!orderBy)/*如果当前数据不是在排序过程中绑定*/
            {
                nowProjectJson="{"+nowProjectJson+nowJson+"}";
                purposeColumn.find(".nowJsonData").find(".hdJsonData").val(nowProjectJson);
            }
        }
    }
    if($("#contentColumn").find(".isloadprice").length<1)
    {
        $(".order").show();      
    }
}
/**js排序(普通住宅)**/
function OrderProjectFloatValue(_orderBy,_direction)
{
    var _nowJsonData=$("#hdProjectPriceJsonData").val();
    if(_nowJsonData==""||_nowJsonData==null)
    {
        _nowJsonData="[";
        var dom=$("#contentColumn").find(".purposeColumn");
        for(var i=0;i<dom.length;i++)
        {
            var _json=$(dom.get(i)).find(".nowJsonData").find(".hdJsonData").val();
            _nowJsonData=_nowJsonData+_json;
            if(i<dom.length-1)
            {
                _nowJsonData=_nowJsonData+",";
            }
        }
        _nowJsonData=_nowJsonData+"]";
        $("#hdProjectPriceJsonData").val(_nowJsonData);
    }
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
        $("#contentColumn").find(".purposeColumn").remove();
        BindProjectDom(jsonObj,true);
        BindProjectPriceDom(jsonObj,true);
    }
}
