
$(function () {
   $("#dataPanel").show();
   /**ajax获取当前爬取详情数据(按周)**/
   BindSelectDate();
   /**查询吸取详情(按周)**/
   $("#btnSearch").live("click",function(){
      $("#floatValue").val($("#selectFloatValue").val());
      $("#orderByValue_week").val("");
      $("#nowDataJsonValue_week").val("");
      GetCaseSearchInfoByWeek("","");
      return false
   });
   /**点击浮动值排序(按周)**/
   $(".floatValueOrder").live("click",function(){
      var nowOrderByValue=$("#orderByValue_week").val();
      var order=$(this).attr("data");
      var _orderBy=order.split(',')[0];
      var _direction=order.split(',')[1];
      var orderByValue=_orderBy+","+_direction;
      if(orderByValue!=nowOrderByValue)
      {
          $(".floatValueOrder").find("font").html("");
          $(this).find("font").html("*")
          $("#orderByValue_week").val(_orderBy+","+_direction);
          OrderCaseSearchInfoByWeek(_orderBy,_direction);
      }
      return false;
   });
   /**切换显示周期**/
   $("#selectDate").live("change",function(){
        BindSelectDate();
   });
   $(".openError").live("click",function(){
       var url="/Spider/CaseSpiderErrorList?cityId={0}&webId={1}&startDate={2}&endDate={3}";
       
       var cityId=$(this).attr("data-cityId");
       var webId=$(this).attr("data-webId");
       var startDate=$(this).attr("data-startDate");/**"2014-03-12"**/
       var endDate=$(this).attr("data-endDate");/**encodeURIComponent("2014-03-12 23:59:59");**/
       url=url._StringFormat(cityId,webId,startDate,endDate);
       //alert(url);
       $.fancybox({
            'href': url,
            'width':1000,
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
   });
});
/**选择周期选项卡时**/
function BindSelectDate()
{
    var  selectVal=$("#selectDate").val();
    $(".byDate").hide();
    $("."+selectVal).show();
    if(selectVal=="byWeek")
    {      
       $("#floatValue").val($("#selectFloatValue").val());
       $("#orderByValue_week").val("");
       $("#nowDataJsonValue_week").val("");
       GetCaseSearchInfoByWeek("","");
    }
    else
    {
       $("#nowDataJsonValue_days").val("");
       GetCaseSearchInfoByDays("","");
    }
}
/****************************************按天********************************/

/**ajax获取当前爬取详情数据(按天)**/
function GetCaseSearchInfoByDays(_orderBy,_direction)
{
    /**获取日期数组**/
    var datesDom=$("#byDaysPanel").find(".nowDates");
    var strings="";
    for(var i=0;i<datesDom.length;i++)
    {
        var val=$(datesDom.get(i)).attr("data");
        strings = strings + val;
        if(i<(datesDom.length-1))
        {
           strings=strings+",";
        }
    }
    /**获取当前已经查询出的数据(用于排序)**/
    var _nowJsonData="";
    if(_nowJsonData!=null&&_nowJsonData!=""&&_orderBy!=null&&_orderBy!=""&&_direction!=null&&_direction!="")
    {
        _nowJsonData=$("#nowDataJsonValue_days").val();    
    } 
    /**参数赋值**/
    var parJsonData={datesStr:strings,orderBy:_orderBy,direction:_direction,nowJsonData:_nowJsonData};
    /**ajax获取数据**/
    $.extendAjax(
                 { url: "/Spider/CaseSpiderCountInfoListByDays_Api",
                     type: "post",
                     data:parJsonData,
                     dataType: "json"
                 },
                 function (data) {
                    $("#caseList_days").find(".SpiderCountList_days").remove();
                    if (data != null) 
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        BindCaseSearchInfoDomByDays(data.detail.List,data.detail.Date);
                        var jsonStr = JSON.stringify(data.detail.List);
                        $("#nowDataJsonValue_days").val(jsonStr);
                    }
                 },{dom:"#dataPanel"});

}
/**绑定案例吸取详情(按天)**/
function BindCaseSearchInfoDomByDays(jsonObj,dateJsonObj)
{    if(jsonObj==null)
    {
        return;
    }       
    //alert(JSON.stringify(dateJsonObj));
    for(var i=0;i<jsonObj.length;i++)
    {
        
        var spiderInfoRomDom=$("#caseRom_days").clone();
        spiderInfoRomDom.attr("id", "SpiderCountList_" +i+"_days").addClass("SpiderCountList_days").show();
        spiderInfoRomDom.find(".spiderCity").html(decodeURIComponent(jsonObj[i].CityName))
        spiderInfoRomDom.find(".spiderWeb").html(decodeURIComponent(jsonObj[i].WebName));
        var datesDom=$("#byDaysPanel").find(".nowDates");
        var strings="";
        for(var j=0;j<datesDom.length; j++)
        {
            spiderInfoRomDom.find(".dateColumn_"+j).find("font").html(jsonObj[i]["DateColumn"+j]).hide();
            spiderInfoRomDom.find(".dateColumn_"+j).find("a").html(jsonObj[i]["DateColumn"+j]);
            spiderInfoRomDom.find(".dateColumn_"+j).find("a").attr("data-cityId",jsonObj[i].CityId).addClass("openError");
            spiderInfoRomDom.find(".dateColumn_"+j).find("a").attr("data-webId",jsonObj[i].WebId);
            spiderInfoRomDom.find(".dateColumn_"+j).find("a").attr("data-startDate",dateJsonObj["DateColumn"+j+"_start"]);
            spiderInfoRomDom.find(".dateColumn_"+j).find("a").attr("data-endDate",dateJsonObj["DateColumn"+j+"_end"]);
        }
        $("#caseList_days").append(spiderInfoRomDom);
    }
}
/********************************按周****************************************/
/**js排序当前爬取详情数据(按周)**/
function OrderCaseSearchInfoByWeek(_orderBy,_direction)
{
    var _nowJsonData=$("#nowDataJsonValue_week").val();
    $("#caseList_week").find(".SpiderCountList_week").remove();
    /**用于js排序**/
    if(_nowJsonData!=null&&_nowJsonData!=""&&_orderBy!=null&&_orderBy!=""&&_direction!=null&&_direction!="")
    {
        var jsonObj=JSON.parse(_nowJsonData);
        var list=jsonObj.List;
        var dateObj=jsonObj.Date;
        list.sort(function (a, b) {
            if(_direction=="asc")
            {
                return getAbsValue(a[_orderBy]) > getAbsValue(b[_orderBy]) ? 1 : getAbsValue(a[_orderBy]) == getAbsValue(b[_orderBy]) ? 0 : -1;
            }
            return getAbsValue(a[_orderBy]) < getAbsValue(b[_orderBy]) ? 1 : getAbsValue(a[_orderBy]) == getAbsValue(b[_orderBy]) ? 0 : -1;
         }); 
        BindCaseSearchInfoDomByWeek(list,dateObj);
    }
}
/**ajax获取当前爬取详情数据(按周)**/
function GetCaseSearchInfoByWeek(_orderBy,_direction)
{
    var _floatValue=$("#floatValue").val();
    /**获取当前已经查询出的数据(用于排序)  **/  
    var _nowJsonData="";
    if(_nowJsonData!=null&&_nowJsonData!=""&&_orderBy!=null&&_orderBy!=""&&_direction!=null&&_direction!="")
    {
        _nowJsonData=$("#nowDataJsonValue_week").val();    
    }
    var parJsonData={floatValue:_floatValue,orderBy:_orderBy,direction:_direction,nowJsonData:_nowJsonData};
    /**ajax获取数据**/
    $.extendAjax(
                 { url: "/Spider/CaseSpiderCountInfoListByWeek_Api",
                     type: "post",
                     data:parJsonData,
                     dataType: "json"
                 },
                 function (data) {
                    $("#caseList_week").find(".SpiderCountList_week").remove();
                    if (data != null) 
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        BindCaseSearchInfoDomByWeek(data.detail.List,data.detail.Date);
                        var jsonStr = JSON.stringify(data.detail);
                        $("#nowDataJsonValue_week").val(jsonStr);
                    }
                 },{dom:"#dataPanel"});

}
/**绑定案例吸取详情(按周)**/
function BindCaseSearchInfoDomByWeek(jsonObj,dateJsonObj)
{           
    if(jsonObj==null)
    {
        return;
    }             
    for(var i=0;i<jsonObj.length;i++)
    {
        var spiderInfoRomDom=$("#caseRom_week").clone();
        spiderInfoRomDom.attr("id", "SpiderCountList_" +i+"_week").addClass("SpiderCountList_week").show();
        spiderInfoRomDom.find(".spiderCity").html(decodeURIComponent(jsonObj[i].CityName))
        spiderInfoRomDom.find(".spiderWeb").html(decodeURIComponent(jsonObj[i].WebName));
        spiderInfoRomDom.find(".lastWeekCount").find("font").html(jsonObj[i].LastWeekCount).hide();
        spiderInfoRomDom.find(".lastWeekCount").find("a").html(jsonObj[i].LastWeekCount);
        spiderInfoRomDom.find(".lastWeekCount").find("a").attr("data-cityId",jsonObj[i].CityId).addClass("openError");
        spiderInfoRomDom.find(".lastWeekCount").find("a").attr("data-webId",jsonObj[i].WebId);
        spiderInfoRomDom.find(".lastWeekCount").find("a").attr("data-startDate",dateJsonObj.lastweek_start);
        spiderInfoRomDom.find(".lastWeekCount").find("a").attr("data-endDate",dateJsonObj.lastweek_end);
        spiderInfoRomDom.find(".weekCount").find("font").html(jsonObj[i].WeekCount).hide();
        spiderInfoRomDom.find(".weekCount").find("a").html(jsonObj[i].WeekCount);
        spiderInfoRomDom.find(".weekCount").find("a").attr("data-cityId",jsonObj[i].CityId).addClass("openError");
        spiderInfoRomDom.find(".weekCount").find("a").attr("data-webId",jsonObj[i].WebId);
        spiderInfoRomDom.find(".weekCount").find("a").attr("data-startDate",dateJsonObj.week_start);
        spiderInfoRomDom.find(".weekCount").find("a").attr("data-endDate",dateJsonObj.week_end);
        spiderInfoRomDom.find(".floatValue").html(parseInt((jsonObj[i].FloatValue * 100)) + "%");
        spiderInfoRomDom.find(".importCount").html(jsonObj[i].ImportCount);
        spiderInfoRomDom.find(".notImportCount").html(jsonObj[i].NotImportCount);
        $("#caseList_week").append(spiderInfoRomDom);
    }
}



