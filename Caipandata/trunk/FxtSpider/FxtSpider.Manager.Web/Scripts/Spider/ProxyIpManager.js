$(function(){
    GetIpList(1);
    $("#btnSubmit").bind("click",function(){
        AddIpList();
    });
});

/**绑定分页**/
function BindPage(nowIndex,pageSize,count) 
{
  BindPageCommon("#example",nowIndex,count,pageSize,10,
                                    function (event, originalEvent, type, page) {    
                                        GetIpList(page);
                                    },null);
}
/**获取错误列表**/
function GetIpList(_pageIndex)
{
    var _pageSize=5;
    var _isGetCount=0;
    if($("#txtIpCount").html()=="")
    {
        _isGetCount=1;
    }
    var paraJson={
                  pageIndex:_pageIndex,
                  pageSize:_pageSize,
                  isGetCount:_isGetCount};
    $.extendAjax(
                {   
                    url: "/Spider/ProxyIpManagerGetList_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#ipList").find(".ipInfo").remove();
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        if($("#txtIpCount").html()=="")
                        {
                            $("#txtIpCount").html(data.detail.Count);
                        }
                        if (data.detail.List != null) 
                        {
                            var list = data.detail.List;
                            for (var i = 0; i < list.length; i++) 
                            {
                                var obj=list[i];
                                var dom =BindIpDom(obj);
                                $("#ipList").append(dom);
                            }

                        }
                        var count_data =$("#txtIpCount").html();
                        var pageCount = parseInt(((count_data*1) - 1) / _pageSize) + 1;
                        BindPage(_pageIndex,_pageSize,count_data);                        
                    }
                    BindTableHeader("tableIp","tableWindow");
                },
               {dom:"#dataPanel"});
}
function BindIpDom(obj)
{
    var dom = $("#ipList").find("#ipInfo").clone();
    dom.attr("id", "ipInfo" + obj.ID).addClass("ipInfo").show();
    dom.find(".web").html(decodeURIComponent(obj.WebName));
    dom.find(".ip").html(decodeURIComponent(obj.Ip));
    dom.find(".datetime").html(decodeURIComponent(obj.CreateTime));
    dom.find(".ipStatu").html(obj.Status);
    return dom;
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
        tableHeader.css("position","fixed");//absolute
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
        //$("#"+nowId).css("top",nowScrollTop+"px");
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


/**新增代理IP**/
function AddIpList()
{
    var _ip=$("#txtProxyIp").val();
    var _ipArea=$("#txtProxyIpArea").val();
    var _webIds=GetCheckValue($("#selectWeb").find(".cbWeb"),',');
    if(_ip==null||_ip==""||_ipArea==null||_ipArea=="")
    {
        alert("请填写ip和ip所在地区");
        return false;
    }
    if(_webIds==null||_webIds=="")
    {
        alert("请选择要分配IP的网站");
        return false;
    }
    var paraJson={
                  ip:_ip,
                  ipArea:_ipArea,
                  webIds:_webIds};
    $("#btnSubmit").val("提交中...");
    $.extendAjax(
                {   
                    url: "/Spider/ProxyIpManagerAddWebProxyIp_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#btnSubmit").val("确定");
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        if (data.detail != null) 
                        {
                            var list = data.detail;
                            for (var i = 0; i < list.length; i++) 
                            {
                                var obj=list[i];
                                var dom =BindIpDom(obj);
                                $("#ipList").append(dom);
                            }
                            if($("#txtIpCount").html()!=""&&list.length>0)
                            {
                                var count_data =$("#txtIpCount").html()*1;
                                count_data=count_data+list.length;
                                $("#txtIpCount").html(count_data);
                            }  

                        }      
                        alert("插入成功");               
                    }
                    BindTableHeader("tableIp","tableWindow");
                },
               {dom:"#dataPanel"});
}