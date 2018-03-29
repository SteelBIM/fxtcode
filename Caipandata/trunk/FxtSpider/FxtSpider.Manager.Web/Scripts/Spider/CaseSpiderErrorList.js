$(function(){
    GetErrorList(1);
    BindTableHeader("tableError","tableWindow");
});

/**获取错误列表**/
function GetErrorList(_pageIndex)
{
    var _pageSize=20;
    var _cityId=$("#hdCityId").val();
    var _webId=$("#hdWebId").val();
    var _startDate=$("#hdStartDate").val();
    var _endDate=$("#hdEndDate").val();
    var _isGetCount=0;
    /**参数验证**/
    if(_cityId==0||_cityId=="0"||_webId==0||_webId=="0")
    {
        alert("请选择城市和网站");
        return false;
    }
    if($("#txtErrorCount").html()=="")
    {
        _isGetCount=1;
    }
    var paraJson={cityId:_cityId,
                  webId:_webId,
                  startDate:encodeURIComponent(_startDate),
                  endDate:encodeURIComponent(_endDate),
                  pageIndex:_pageIndex,
                  pageSize:_pageSize,
                  isGetCount:_isGetCount};
    $.extendAjax(
                {   
                    url: "/Spider/CaseSpiderErrorListByAndCityAndWebAndDate_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#errorInfoList").find(".errorInfo").remove();
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        if($("#txtErrorCount").html()=="")
                        {
                            $("#txtErrorCount").html(data.detail.Count);
                        }
                        if (data.detail.List != null) 
                        {
                            var list = data.detail.List;
                            for (var i = 0; i < list.length; i++) 
                            {
                                var obj=list[i];
                                var dom =BindErrorDom(obj,null);
                                $("#errorInfoList").append(dom);
                            }

                        }
                        var count_data =$("#txtErrorCount").html();
                        var pageCount = parseInt(((count_data*1) - 1) / _pageSize) + 1;
                        BindPage(_pageIndex,_pageSize,count_data);                        
                    }
                    BindTableHeader("tableError","tableWindow");
                },
               {dom:"#dataPanel"});
}
function BindErrorDom(obj)
{
    var dom = $("#errorInfoList").find("#errorInfo").clone();
    dom.attr("id", "info" + obj.ID).addClass("errorInfo").show();
    dom.find(".city").html($("#hdCityName").val());
    dom.find(".web").html($("#hdWebName").val());
    dom.find(".errorCode").html(GetErrorName(obj.ErrorTypeCode));
    dom.find(".remark").text(decodeURIComponent(obj.Remark));
    dom.find(".url").html(decodeURIComponent(obj.Url));
    dom.find(".date").html(decodeURIComponent(obj.CreateTime));
    return dom;
}
function GetErrorName(code)
{
   return $("#selectErrorCode").find(".code_"+code).html();
}
/**绑定分页**/
function BindPage(nowIndex,pageSize,count) 
{
  BindPageCommon("#example",nowIndex,count,pageSize,10,
                                    function (event, originalEvent, type, page) {    
                                        GetErrorList(page);
                                    },null);
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
        tableHeader.css("position","fixed");
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