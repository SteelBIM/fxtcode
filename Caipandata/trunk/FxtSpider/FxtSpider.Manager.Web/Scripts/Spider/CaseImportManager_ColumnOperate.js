$(function(){
    /**鼠标移上对应库楼盘单元格显示列操作按钮**/
    $(".caseInfo").find(".fxtloupan").live("mouseover",function(){

        var dom=$("#caseList").find(".InfoOperationLoading");  
        if($(this).attr("data")=="fxtloupan")
        {
            if($(this).find(".text").html()=="数据加载中...")
            {
                return false;
            }           
        }
        if(dom.length<1)
        {
            $(this).find(".up").show().val("设置");
            $(this).find(".add").show();
        }
    });
    $("#caseList").find(".caseInfo").live("mouseover",function(){
        var _isImport = $("#hdIsImport").val();
        if (_isImport != 1 && _isImport != "1") 
        {
            var dom=$(this);
            var length1=dom.find(".txt_span").length;
            var length2=dom.find(".btn_span").length;
            if(length1>0||length2>0)
            {
                return;
            }
            var rowNumber=dom.attr("id");
            var tdDom = dom.find(".btnShow");
            for (var i = 0; i < tdDom.length; i++) 
            {

                var className = $(tdDom.get(i)).attr("data");
                /**修改框**/
                var txtSpan = $("#txtSpan").clone();
                txtSpan.attr("id", "");
                var inputType = $(tdDom.get(i)).attr("data-inputType");
                var nowText = $(tdDom.get(i)).find(".text").html();
                if (inputType == "text") 
                {
                    txtSpan.find(".selectInput").remove();
                    txtSpan.find(".textValue").attr("data-column", className);
                    txtSpan.find(".textValue").attr("data-row", rowNumber);
                    txtSpan.find(".textValue").val(nowText);
                    var sourceId = $(tdDom.get(i)).attr("data-selectSource");
                    if(className=="fxtloupan"||className=="loupan")
                    {
                        var _cityName=dom.find(".chengshi").find(".text").html();
                        SetColumnValueAutocomplete2(txtSpan.find(".textValue"),_cityName);
                    }
                    else if(className=="pianqu")
                    {
                        var _cityId=dom.find(".chengshi").attr("data-cityId");
                        SetColumnValueAutocomplete3(txtSpan.find(".textValue"),_cityId);
                    }
                    else
                    {
                        SetColumnValueAutocomplete(txtSpan.find(".textValue"), sourceId);
                    }
                }
                else 
                {
                    txtSpan.find(".textInput").remove();
                    var sourceId = $(tdDom.get(i)).attr("data-selectSource");
                    var source = $("#" + sourceId).find("option").clone();
                    txtSpan.find(".selectInput").append(source);
                    if (nowText != null && nowText != "") 
                    {
                        txtSpan.find(".selectInput").val(nowText);
                    }
                }
                $(tdDom.get(i)).append(txtSpan);
                /**操作按钮**/
                var btnSpan = $("#btnSpan").clone();
                btnSpan.attr("id", "").addClass("btnOptPanel").show();
                btnSpan.find(".up").attr("data-column", className);
                btnSpan.find(".up").attr("data-row", rowNumber);
                btnSpan.find(".add").attr("data-column", className);
                btnSpan.find(".add").attr("data-row", rowNumber);
                btnSpan.find(".subm").attr("data-column", className);
                btnSpan.find(".subm").attr("data-row", rowNumber);
                btnSpan.find(".cancel").attr("data-column", className);
                btnSpan.find(".cancel").attr("data-row", rowNumber);
                if (className != "fxtloupan") 
                {
                    btnSpan.removeClass("btn_span").addClass("btn_span2")
                    btnSpan.find(".add").remove();
                }
                $(tdDom.get(i)).append(btnSpan);
            }
        }
    });
    $(".caseInfo").find(".fxtloupan").live("mouseout",function(){
        var dom=$("#caseList").find(".InfoOperationLoading");  
        if(dom.length<1)
        {
            $(this).find(".up").hide();
            $(this).find(".add").hide();
        }
    });
    /**鼠标点击其它单元格(保存or修改)操作**/
    $(".caseInfo").find("td").live("click",function(){
        /**当前单元格没有在修改**/
        if(!$(this).hasClass("InfoOperationLoading"))
        { 
            var dom=$("#caseList").find(".InfoOperationLoading");            
            /**没有其他单元在修改**/
            if(dom.length<1)
            {    
                if($(this).attr("data")=="fxtloupan")
                {
                    ClickCancelNowBtn();
                    return;
                }           
                if($(this).attr("data")=="fxtloupan")/**如果当前单元格不是列"fxtloupan"**/ 
                {
                   if($(this).find(".text").html()=="数据加载中...")/**并且数据还在加载中**/
                   {
                       return;
                   }           
                }
                ClickOperationBtnUp($(this).find(".up"));                
            }
            else
            {
                /**没有其他单元格在提交数据中**/
                if(!isPageOperationLoading())
                {   
                    var nowDom=$(this);
                    if(dom.attr("data")=="fxtloupan")/**上一个单元格为fxtloupan**/
                    {
                        ClickCancelNowBtn();
                        if(nowDom.attr("data")!="fxtloupan")/**当前单元格为不为fxtloupan**/
                        {
                            ClickOperationBtnUp(nowDom.find(".up"));/**当前为修改状态**/
                        }
                        return;
                    }  
                    ClickOperationBtnSubm(dom.find(".subm"),function(){
                        if($(this).attr("data")=="fxtloupan")
                        {
                           if($(this).find(".text").html()=="数据加载中...")
                           {
                               return;
                           }           
                        }
                        if(nowDom.attr("data")!="fxtloupan")/**上一个单元和格为当前单元格!=fxtloupan**/
                        {
                            ClickOperationBtnUp(nowDom.find(".up"));
                        }
                    });
                }
            }
        }
        return;
    });
    /**点击单元格上的操作"修改"按钮**/
    $("#caseList").find(".caseInfo").find(".up").live("click",function(){
        ClickOperationBtnUp(this);
        return false;
    });
    /**点击单元格上的操作"确定"按钮**/
    $("#caseList").find(".caseInfo").find(".subm").live("click",function(){
        ClickOperationBtnSubm(this,function(){});
        return false;
    });
    /**点击单元格上的操作"取消"按钮**/
    $("#caseList").find(".caseInfo").find(".cancel").live("click",function(){
        ClickCancelNowBtn();
        return false;
    });
    /**点击单元格中的新建楼盘**/
    $("#caseList").find(".caseInfo").find(".add").live("click",function(){
        ClickOperationBtnAdd(this);
        return false;
    });

});
/**************************************单元格中的操作**********************************/
/**点击"修改"时**/
function ClickOperationBtnUp(nowDom)
{        
    var column=$(nowDom).attr("data-column");
    var row=$(nowDom).attr("data-row");
    var parentDom=$("#caseList").find("#"+row).find("."+column);
    var parentRowDom=$("#caseList").find("#"+row);
    var width=parentDom.find(".text").width();
    //txt_span
    if(column=="fxtloupan"&&parentDom.find(".txt_span").find(".textValue").val()=="")
    {
        parentDom.find(".txt_span").find(".textValue").val(parentRowDom.find(".loupan").find(".text").html());
        width=parentRowDom.find(".loupan").find(".text").width();
    }
    width=width*1+50;
   // alert(parentDom.length);
    parentDom.find(".txt_span").show().css("width",width+"px");
    parentDom.find(".txt_span").find(".textValue").css("width",(width-2)+"px");
    parentDom.find(".txt_span").find(".textValue").focus();
    parentDom.find(".text").hide();
    if(column=="fxtloupan")
    {
        parentDom.find(".subm").show();
    }
    else
    {
        parentDom.find(".cancel").show();
    }
    $(nowDom).hide();


    $("#caseList").find(".InfoOperationLoading").removeClass("InfoOperationLoading");
    parentDom.addClass("InfoOperationLoading");
}
/**点击"确定"时**/
function ClickOperationBtnSubm(nowDom,_function)
{   
    var column=$(nowDom).attr("data-column");
    var row=$(nowDom).attr("data-row");
    var parentDom=$("#caseList").find("#"+row).find("."+column);
    var parentRowDom=$("#caseList").find("#"+row);
    var textVal=parentDom.find(".txt_span").find(".textValue").val();
    var caseIds=$("#caseList").find("#"+row).attr("data-Id");
    var nowCity=parentRowDom.find(".chengshi").find(".text").html();
    var nowArea=parentRowDom.find(".xingzhengqu").find(".text").html();
    var nowProjName=parentRowDom.find(".loupan").find(".text").html();
    var nowText=parentDom.find(".text").html();
    /**当前值是否被修改过**/
    if(parentDom.find(".text").html()!=textVal)
    {  
        var caseDomList=$("#caseList").find(".caseInfo"); 
        var isAll=false;/**confirm("是否将本页中相同列的信息一起修改");**/
        var _submCount="now";
        if(column=="loupan")
        {
            isAll=true;
            _submCount="all";
        }
        if(isAll)
        {
            /**将当前列表中同列信息一起修改     **/
            for(var i=0;i<caseDomList.length;i++)
            {
                var _nowCity=$(caseDomList.get(i)).find(".chengshi").find(".text").html();
                var _nowArea=$(caseDomList.get(i)).find(".xingzhengqu").find(".text").html();
                var _nowProjName=$(caseDomList.get(i)).find(".loupan").find(".text").html();
                var _nowText=$(caseDomList.get(i)).find("."+column).find(".text").html();
                if(_nowCity==nowCity&&(_nowArea.contains(nowArea)||nowArea.contains(_nowArea))&&_nowProjName==nowProjName&&_nowText==nowText)
                {
                    caseIds=caseIds+","+$(caseDomList.get(i)).attr("data-Id");
                }
            }
        }
        parentDom.find(".loading").show();
        if(column=="fxtloupan")
        {
            $(nowDom).hide();
        }
        else
        {
            parentDom.find(".cancel").hide();
        }
        setPageOperationLoading();/**标记有数据正在传输**/
        UpdateColumnValue(caseIds,column,textVal,nowText,nowCity,nowArea,_submCount,function(data){
            parentDom.find(".loading").hide();
            if(column=="fxtloupan")
            {
                $(nowDom).show();
            }
            setPageOperationOver();
            if(data!=null)
            {
                if(data.result==1||data.result=="1")
                {
                    parentDom.find(".text").show().html(textVal);
                    parentDom.find(".txt_span").hide();
                    $(nowDom).hide();
                    if(isAll||column=="fxtloupan")
                    {            
                        /**将当前列表中同列信息一起修改     **/
                        for(var i=0;i<caseDomList.length;i++)
                        {
                            var _nowCity=$(caseDomList.get(i)).find(".chengshi").find(".text").html();
                            var _nowArea=$(caseDomList.get(i)).find(".xingzhengqu").find(".text").html();
                            var _nowProjName=$(caseDomList.get(i)).find(".loupan").find(".text").html();
                            var _nowText=$(caseDomList.get(i)).find("."+column).find(".text").html();
                            if(_nowCity==nowCity&&(_nowArea.contains(nowArea)||nowArea.contains(_nowArea))&&_nowProjName==nowProjName&&_nowText==nowText)
                            {
                                $(caseDomList.get(i)).find("."+column).find(".text").html(textVal);
                                $(caseDomList.get(i)).find("."+column).find(".txt_span").find(".textValue").val(textVal);
                                if(column=="fxtloupan")
                                {
                                    $(caseDomList.get(i)).find(".fxtloupan").find(".text").html(textVal);
                                    $(caseDomList.get(i)).find(".fxtloupan").find(".txt_span").find(".textValue").val(textVal);
                                }
                            }
                        } 
                    }
                    $("#caseList").find(".InfoOperationLoading").removeClass("InfoOperationLoading");
                    _function();
                }
                else
                {
                    if(column!="fxtloupan")
                    {
                        parentDom.find(".cancel").show();
                    }
                    alert(decodeURIComponent(data.message));
                }

            }
        });
    }
    else
    {                   
        parentDom.find(".text").show().html(textVal);
        if(column=="fxtloupan")
        {
            parentDom.find(".up").show();
        }
        else
        {
            parentDom.find(".cancel").hide();
        }
        parentDom.find(".txt_span").hide();
        $(nowDom).hide();
        setPageOperationOver();
        $("#caseList").find(".InfoOperationLoading").removeClass("InfoOperationLoading");
        _function();
    }

}
/**点击单元格"建盘"时**/
function ClickOperationBtnAdd(nowDom)
{        
    var column=$(nowDom).attr("data-column");
    var row=$(nowDom).attr("data-row");
    var parentRowDom=$("#caseList").find("#"+row);
    var projectName=parentRowDom.find(".loupan").find(".text").html();
    var cityName=parentRowDom.find(".chengshi").find(".text").html();
    var areaName=parentRowDom.find(".xingzhengqu").find(".text").html();
    var parentDomColumn=$("#caseList").find("#"+row).find("."+column);
    ClickOperationBtnUp(parentDomColumn.find(".up"));
    var caseId=parentRowDom.attr("data-Id");
    OpenAddProjectBy(projectName,cityName,areaName,caseId);
}
/**取消当前按钮的修改操作**/
function ClickCancelNowBtn()
{
    var parentDom=$("#caseList").find(".InfoOperationLoading");
    var textVal=parentDom.find(".text").html();
    parentDom.find(".txt_span").find(".textValue").val(textVal);
    parentDom.find(".text").show();
    parentDom.find(".txt_span").hide();
    parentDom.find(".up").hide();
    parentDom.find(".subm").hide();
    parentDom.find(".add").hide();
    parentDom.find(".cancel").hide();
    $("#caseList").find(".InfoOperationLoading").removeClass("InfoOperationLoading");
    setPageOperationOver();
}
/**上传单元格中修改的信息**/
function UpdateColumnValue(_id,_columnName,_columnValue,_nowValue,_cityName,_nowArea,_submCount,_function)
{
   $.extendAjax({ 
                  url:"/Spider/CaseImportManager_UpdateColumnInfo_Api",
                  data:{ caseIds:_id,columnValue:encodeURIComponent(_columnValue),columnName:encodeURIComponent(_columnName),nowValue:encodeURIComponent(_nowValue),cityName:encodeURIComponent(_cityName),areaName:_nowArea,submCount:_submCount},
                  type:"post",
                  dataType:"json"
               },
               function(data){
                   _function(data);
               },{dom:"#tablepanel"});
    
}
function SetColumnValueAutocomplete3(dom,_cityId)
{
    $(dom).unautocomplete();
    $(dom).autocomplete("/Common/GetSubAreaByLikeAndCityId_Api", {
        width:230,
        max: 100,
        highlight: false,
        multiple: false,
        multipleSeparator: "",
        scroll: true,
        selectFirst:false,
        scrollHeight: 300,
        searchEval:"click",
        minChars:0,
        extraParams :{cityId:_cityId},
        formatItem: function (row,i,max) { 
             var obj =eval("(" + row + ")"); 
             return row;
        },
        formatResult: function(row) {
             var obj =eval("(" + row + ")"); 
             return row;
        }		
    }); 
}
function SetColumnValueAutocomplete2(dom,_cityName)
{
    $(dom).unautocomplete();
    $(dom).autocomplete("/Common/GetFxtProjectByLikeAndCityName_Api", {
        width:230,
        max: 100,
        highlight: false,
        multiple: false,
        multipleSeparator: "",
        scroll: true,
        selectFirst:false,
        scrollHeight: 300,
        searchEval:"click",
        minChars:0,
        extraParams :{cityName:encodeURIComponent(_cityName)},
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

/**给修改单元格时的文本框下拉提示(单元格为fxtloupan除外)**/
function SetColumnValueAutocomplete(dom,sourceId)
{
    $(dom).unautocomplete();
    if(sourceId==null||sourceId=="undefined")
    {
        return;
    }
    var domData=$("#"+sourceId).find("option");
    var data = new Array();
    if(domData.length>0)
    {
        for(var i=0;i<domData.length;i++)
        {
            var val=$(domData.get(i)).attr("value");
            data.push(val);
        }
        $(dom).autocomplete(data, {
		    max: 10000,
		    highlight: false,
		    multiple: false,
		    multipleSeparator: "",
		    scroll: true,
		    selectFirst:false,
		    scrollHeight: 100,
            minChars:0,
            isMate:false,
            searchEval:"click",
            parse: function(data) {}		
        }); 
    }
   
}
/**打开新增楼盘**/
function OpenAddProjectBy(_projectName,_cityName,_areaName,_caseId)
{
    _projectName=encodeURIComponent(_projectName);
    _cityName=encodeURIComponent(_cityName);
    _areaName=encodeURIComponent(_areaName);
    $.fancybox({
        'href': "/Project/AddProject_Fancybox?projectName="+_projectName+"&cityName="+_cityName+"&areaName="+_areaName+"&caseId="+_caseId,
        'width':600,
        'height': 380,
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