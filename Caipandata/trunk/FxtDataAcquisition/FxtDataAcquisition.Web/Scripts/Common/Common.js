/******************************************************************************************************************/
/**************************************字符串处理******************************************************************/
/******************************************************************************************************************/
/**模拟C#中string.Format**/
String.prototype._StringFormat=function()
{
  if(arguments.length==0)
  {
    return this;
  }
  for(var _StringFormat_s=this, _StringFormat_i=0; _StringFormat_i<arguments.length; _StringFormat_i++)
  {
    _StringFormat_s=_StringFormat_s.replace(new RegExp("\\{"+_StringFormat_i+"\\}","g"), arguments[_StringFormat_i]);
  }
  return _StringFormat_s;
};
String.prototype.contains = function(substring)  
{  
    return this.indexOf(substring) != -1 ? true:false;  
};  
/***自定义trim()方法去除字串左右杂质 **/
String.prototype.Trim = function (Useless)
{         
    /**eval函数转换字符串形式的表达式 **/
    var regex = eval("/^" + Useless + "*|" + Useless + "*$/g");   
    return this.replace(regex, "");      
};  
/***自定义lTrim()方法去除字串左侧杂质 **/
String.prototype.TrimStart = function (Useless) 
{    
    var regex = eval("/^" + Useless + "*/g");                   
    return this.replace(regex, "");      
};  
/***自定义rTrim()方法去除字串右侧杂质 **/
String.prototype.TrimEnd = function (Useless) 
{    
    var regex = eval("/"+Useless + "*$/g");                      
    return this.replace(regex, "");          
};  
/***自定义时间日期字符串格式转换***/
Date.prototype.Format = function(formatStr)   
{   
    var str = formatStr;   
    var Week = ['日','一','二','三','四','五','六'];  
  
    str=str.replace(/yyyy|YYYY/,this.getFullYear());   
    str=str.replace(/yy|YY/,(this.getYear() % 100)>9?(this.getYear() % 100).toString():'0' + (this.getYear() % 100));   
  
    str=str.replace(/MM/,(this.getMonth()+1)>9?(this.getMonth()+1).toString():'0' + (this.getMonth()+1));   
    str=str.replace(/M/g,(this.getMonth()+1));   
  
    str=str.replace(/w|W/g,Week[this.getDay()]);   
  
    str=str.replace(/dd|DD/,this.getDate()>9?this.getDate().toString():'0' + this.getDate());   
    str=str.replace(/d|D/g,this.getDate());   
  
    str=str.replace(/hh|HH/,this.getHours()>9?this.getHours().toString():'0' + this.getHours());   
    str=str.replace(/h|H/g,this.getHours());   
    str=str.replace(/mm/,this.getMinutes()>9?this.getMinutes().toString():'0' + this.getMinutes());   
    str=str.replace(/m/g,this.getMinutes());   
  
    str=str.replace(/ss|SS/,this.getSeconds()>9?this.getSeconds().toString():'0' + this.getSeconds());   
    str=str.replace(/s|S/g,this.getSeconds());   
  
    return str;   
}   
/****获取绝对值********/
function getAbsValue(_val)
{
    if(_val<0)
    {
        return (-_val)*1;
    }
    return _val*1;
}

/******************************************************************************************************************/
/**************************************js插件二次封装**************************************************************/
/******************************************************************************************************************/
/**绑定ajax分页样式,
_dom:显示分页的html标签
_nowPageIndex:当前页码
_count:总个数
_pageSize:每页多少条
_numberOfPages:显示页码数
_onPageClicked:function(event, originalEvent, type, page){};点击页码时事件
_pageOptions:分页控件其他参数
**/
function BindPageCommon(_dom,_nowPageIndex,_count,_pageSize,_numberOfPages,_onPageClicked,_pageOptions)
{
    var pageCount = parseInt(((_count*1) - 1) / _pageSize) + 1;
    _pageOptions =jQuery.extend( {
        bootstrapMajorVersion:3,
        currentPage: _nowPageIndex, /**当前页**/
        totalPages: pageCount, /**总页数**/
        numberOfPages: _numberOfPages, /**显示页码数**/
        pageUrl: function (type, page, current) { return "javascript:;";},
        onPageClicked: function (event, originalEvent, type, page) { _onPageClicked(event, originalEvent, type, page)},
        itemTexts: function (type, page, current) {
             switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "末页";
                case "page":
                    return page;
            }
        }
    },
    _pageOptions);
    $(_dom).bootstrapPaginator(_pageOptions);
    if(pageCount<=1)
    {
        $(_dom).hide();
    }
    else
    {
        $(_dom).show();
    }
}
/********************************
******不包含任何按钮的对话框*****
**text:需显示的文本
**evalStr:关闭后需执行的操作,可为''
**time:关闭时间 单位 毫秒
**overlayShow:是否遮住外层,true:遮住,false:不遮住. 可为null
********************************/
function AlertFancybox(text)
{
    AlertFancybox2(text,function(){});                              
}
function AlertFancybox2(text,_function)
{
   $("#div_fancybox_alert_util").remove();
  
   /*定义样式*/
   var alertStyle=GetAlertStyle();
   /*定义弹出div*/
   var alertStrDiv="<span id='div_fancybox_util' style='display:none'> "+ GetAlertDiv(text)+"</span>";  
   
   /*加载定义项*/
   $("body").append(alertStyle+alertStrDiv);        
      
   /*定义脚本*/
   $.fancybox({
                   'padding':0,
                   'speedIn':100,
                   'speedOut':0,
                   'scrolling' : 'no',
                   'titleShow' : false,
                   'centerOnScroll' : true,
                   'showCloseButton' :false,
                   'overlayShow': true,
                   'overlayColor': 'White',
                   'hideOnOverlayClick': false,  
                   'href':'#div_fancybox_alert_util',    
                   'overlayOpacity':0,
                   'onClosed':function()
                   {
                       _function();
                   }
              });
   
  
   $("#div_fancybox_util").remove(); 
   /*延迟关闭 */
   setTimeout("$.fancybox.close()",1600);                                
}
/*****弹出框style****/
function GetAlertStyle()
{
  var alertCssStr=""+
  "<style>"+
    ".kuan_xtmr{width:250px; border:solid 3px #668CAE; background:#f9f9f9;} "+
    ".kuan_xtmr .kuan_title{width:240px; height:29px; background:url(/images/kuan_bj.gif); line-height:29px; text-align:left; padding-left:10px;} "+
    ".kuan_xtmr .nr_title{width:250px; overflow:auto;} "+
    ".kuan_xtmr .nr_title .nr_xz{padding:15px 0 15px 10px; margin-left:10px; _margin-left:5px; width:210px; float:left; line-height:24px; text-align:center;} "+
    ".kuan_xtmr .nr_title .nr_xz .jr{background:url('/images/Services_n81.png') no-repeat; color:#FFFFFF; font-weight:bold; height:22px; margin-right:5px; line-height:22px; text-align:center; width:75px; border:0px;} "+
    ".kuan_xtmr .nr_title .nr_xz .qx{background:url('/images/Services_n81-22.png') no-repeat; color:#FFFFFF; font-weight:bold; height:22px; margin-right:5px; line-height:22px; text-align:center; width:75px; border:0px;} "+
  "</style>";
  
  return alertCssStr;
}
function GetAlertDiv(text)
{
  var alertDiv=""+
      "<div class='kuan_xtmr' id='div_fancybox_alert_util' style='z-index:1000000;'>"+
	     "<div class='nr_title' style='z-index:1000000;'>"+
	    	  "<div class='nr_xz f16' style='z-index:1000000;font-size:20px;'>"+
		      text+
		      "</div>"+		  
	     "</div>"+
      "</div>";
   return alertDiv
}
/*****弹出确认框****
**title:标题*******
**content:消息****
**callback:function(bool)回调函数****/
function DA_Confirm(title,content,callback)
{
    callback(confirm(content));
}
/*****弹出提示框***
**content:消息****/
function DA_Alert(content,callback)
{
    alert(content);
    callback();
}
/******************************************************************************************************************/
/**************************************处理html的公共方法**********************************************************/
/******************************************************************************************************************/
/**获取指定标签下checkbox的选中值
_dom:checkbox的jquery对象,例如:$(".check")
_char:分隔值的字符
**/
function GetCheckValue(_dom,_char)
{
  var ids="";
  var checkList=$(_dom);
  for(var i=0;i<checkList.length;i++)
  {
      var isSelect=$(checkList.get(i)).attr("checked");
      if(isSelect=="checked")
      {
          var val=$(checkList.get(i)).val();
          ids=ids+val+_char;
      }
  }
  ids=ids.TrimEnd(_char);
  return ids;

}
/**获取指定标签下checkbox的选中值
_dom:存放checkbox的html区域的jquery对象,例如:$(".check")
_char:分隔值的字符
**/
function GetCheckValueByFind(_dom,_char)
{
  var ids="";
  var checkList=_dom.find("input[type='checkbox']");
  for(var i=0;i<checkList.length;i++)
  {
      var isSelect=$(checkList.get(i)).attr("checked");
      if(isSelect=="checked")
      {
          var val=$(checkList.get(i)).val();
          ids=ids+val+_char;
      }
  }
  ids=ids.TrimEnd(_char);
  return ids;
}

//取数组
function GetCheckValueArrayByFind(_dom) {
    var ids = [];
    var checkList = _dom.find("input[type='checkbox']");
    for (var i = 0; i < checkList.length; i++) {
        var isSelect = $(checkList.get(i)).attr("checked");
        if (isSelect == "checked") {
            var val = $(checkList.get(i)).val();
            ids.push(val);
        }
    }
    return ids;
}

/******************************************************************************************************************/
/**************************************页面功能的公共方法**********************************************************/
/******************************************************************************************************************/

/**弹出登陆框
callback:function(bool){}登陆完成后的回调函数,参数为是否登陆成功
**/
function OpenLogin(callback)
{
        $.fancybox({
            'href': Url_Login_LoginBox,/**"/OperationMaintenance/SetCase_Fancybox?fxtCityId=6&projectId=45772&caseId="+caseId+"&buildingTypeCode=2003004&purposeCode=1002001&areaTypeCode=8006004&date=2012-6",**/
            'width':400,
            'height': 250,
            'padding' :0 ,
            'hideOnOverlayClick':false,
            'showCloseButton':false,
            'overlayShow': true,
            'autoScale': false,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'type': 'iframe',
            'onClosed' : function(){
                callback(true);
            }
        });
}

