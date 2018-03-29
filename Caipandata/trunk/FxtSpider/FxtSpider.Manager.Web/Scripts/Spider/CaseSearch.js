$(function () {
    //GetCaseSearchInfo(1);
    $("#btnSearch").live("click",function(){
        $("#caseList").attr("data", "");
        GetCaseSearchInfo(1);
        return false;
    });
    $("#btnExcel").live("click",function(){
        DownloadCaseSearchInfo();
        return false;
    });
});
function BindPage(nowIndex, pageCount) 
{
    var options = {
        currentPage: nowIndex, /**当前页**/
        totalPages: pageCount, /**总页数**/
        numberOfPages: 5, /**显示页码数**/
        pageUrl: function (type, page, current) {
            return "javascript:;";
        },
        onPageClicked: function (event, originalEvent, type, page) {    
            GetCaseSearchInfo(page);
        }
    }
    $("#example").bootstrapPaginator(options);
}

function GetCaseSearchInfo(_start)
{
    var _cityId=0;
    var _webId=0;
    var _startDate="";
    var _endDate="";
    var _pageLength=20;
    var isGetCount=0;
    
    _cityId=$("#selectCity").val();
    _webId=$("#selectWeb").val();
    _startDate=$("#selectStartDate").val();
    _endDate=$("#selectEndDate").val();
    if(_startDate==null||_startDate=="")
    {
       alert("请选择开始时间");
       return false;
    }
    if(_endDate==null||_endDate=="")
    {
       alert("请选择结束时间");
       return false;
    }
    var count_data2 = $("#caseList").attr("data");
    if (count_data2 == null || count_data2 == "") 
    {
       isGetCount=1;
    }
    var paraJson = 
    {   cityId: _cityId,
        webId: _webId,
        startDate: _startDate,
        endDate: _endDate,
        start: _start,
        pageLength: _pageLength,
        _isGetCount: isGetCount
    };
    $.extendAjax(
                 { url: "/Spider/CaseSearch_Api",
                     data: paraJson,
                     type: "post",
                     dataType: "json"
                 },
                 function (data) {
                    $("#caseList").find(".caseInfo").remove();
                     var count_data = $("#caseList").attr("data");
                     if (data != null) 
                     {
                         if(data.result!=1&&data.result!="1")
                         {
                            alert(decodeURIComponent(data.message));
                            return;
                         }
                         if (count_data == null || count_data == "") 
                         {
                             $("#caseList").attr("data", data.detail.Count);
                             count_data = data.detail.Count * 1;
                             var pageCount = parseInt((count_data - 1) / _pageLength) + 1;
                             $("#caseCount").html("总记录数"+count_data+"个");
                             BindPage(_start, pageCount);
                         }
                         if (data.detail.List != null) 
                         {
                             var list = data.detail.List;
                             for (var i = 0; i < list.length; i++) {
                                 var dom = BindCaseInfoHtml(list[i]);
                                 $("#caseList").append(dom);
                             }
                         }
                     }
                 },{dom:"#dataPanel"});

}

function DownloadCaseSearchInfo()
{
    var domUrl="/Spider/DownloadCaseSearch_Api/?cityId={0}&webId={1}&startDate={2}&endDate={3}";
    var _cityId=0;
    var _webId=0;
    var _startDate="";
    var _endDate="";
    _cityId=$("#selectCity").val();
    _webId=$("#selectWeb").val();
    _startDate=$("#selectStartDate").val();
    _endDate=$("#selectEndDate").val();
    if(_startDate==null||_startDate=="")
    {
       alert("请选择开始时间");
       return false;
    }
    if(_endDate==null||_endDate=="")
    {
       alert("请选择结束时间");
       return false;
    }
    domUrl=domUrl._StringFormat(_cityId,_webId,_startDate,_endDate);

/**    $.extendAjax(
//                 { url: domUrl,
//                     type: "get",
//                     dataType: "text"
//                 },
//                 function (data) {
//                 alert(data);
//                 });**/
    window.open(domUrl,"dom");

}
function BindCaseInfoHtml(caseObj) 
{
    var dom = $("#caseList").find("#caseRom").clone();
    dom.attr("id", "caseList_" + caseObj.ID).addClass("caseInfo").show();
    dom.find(".laiyuan").html(decodeURIComponent(caseObj.web));
    dom.find(".chengshi").html(decodeURIComponent(caseObj.city));
    dom.find(".xingzhengqu").html(decodeURIComponent(caseObj.areaName));
    dom.find(".pianqu").html(decodeURIComponent(caseObj.areaName2));
    dom.find(".loupan").html(decodeURIComponent(caseObj.projectName));
    dom.find(".anlishijian").html(decodeURIComponent(caseObj.caseDate));
    dom.find(".loudong").html(decodeURIComponent(caseObj.building));
    dom.find(".fanghao").html(decodeURIComponent(caseObj.houseNumber));
    dom.find(".yongtu").html(decodeURIComponent(caseObj.purpose));
    dom.find(".mianji").html(decodeURIComponent(caseObj.are));
    dom.find(".danjia").html(decodeURIComponent(caseObj.unitPrice));
    dom.find(".zongjia").html(decodeURIComponent(caseObj.totalPrice));
    dom.find(".anlileixing").html(decodeURIComponent(caseObj.caseType));
    dom.find(".jiegou").html(decodeURIComponent(caseObj.structure));
    dom.find(".jianzhuleixing").html(decodeURIComponent(caseObj.buildingType));
    dom.find(".zonglouceng").html(decodeURIComponent(caseObj.totalFloor));
    dom.find(".suozailouceng").html(decodeURIComponent(caseObj.floorNumber));
    dom.find(".huxing").html(decodeURIComponent(caseObj.houseType));
    dom.find(".chaoxiang").html(decodeURIComponent(caseObj.front));
    dom.find(".zhuangxiu").html(decodeURIComponent(caseObj.fitment));
    dom.find(".niandai").html(decodeURIComponent(caseObj.buildingDate));
    dom.find(".dianhua").html(decodeURIComponent(caseObj.phone));
    dom.find(".titleUrl").html(decodeURIComponent(caseObj.url));
    dom.find(".dizhi").html(decodeURIComponent(caseObj.address));
    dom.find(".shijian").html(decodeURIComponent(caseObj.createDate));
    dom.find(".xingshi").html(decodeURIComponent(caseObj.xingshi));
    dom.find(".huayuan").html(decodeURIComponent(caseObj.huayuan));
    dom.find(".tingjiegou").html(decodeURIComponent(caseObj.tingjiegou));
    dom.find(".chewei").html(decodeURIComponent(caseObj.chewei));
    dom.find(".peitao").html(decodeURIComponent(caseObj.peitao));
    dom.find(".dixiashi").html(decodeURIComponent(caseObj.dixiashi));
    dom.find(".bizhong").html(decodeURIComponent(caseObj.moneyUnit));
    dom.find(".infoTitle").html(decodeURIComponent(caseObj.title));
    return dom;
}
