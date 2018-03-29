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
/****根据涨跌幅设置颜色(大于15%)*****/
function FloatValueColor(_floatValue)
{
   var val= _floatValue.replace("%","")*1;
   if(val<0&&getAbsValue(val)>15)
   {
       _floatValue="<font style='color:red'>"+_floatValue+"<font/>";
   }
   else if(val>0&&getAbsValue(val)>15)
   {
       _floatValue="<font style='color:blue'>"+_floatValue+"<font/>";
   }
   return _floatValue;

}
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
        currentPage: _nowPageIndex, /**当前页**/
        totalPages: pageCount, /**总页数**/
        numberOfPages: _numberOfPages, /**显示页码数**/
        pageUrl: function (type, page, current) { return "javascript:;";},
        onPageClicked: function (event, originalEvent, type, page) { _onPageClicked(event, originalEvent, type, page)},
        itemTexts:function(type, page, current){
             switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "最后一页";
                case "page":
                    return page;
            }
        }
    },
    _pageOptions);
    $(_dom).bootstrapPaginator(_pageOptions);
}
//获取绝对值
function getAbsValue(_val)
{
    if(_val<0)
    {
        return (-_val)*1;
    }
    return _val*1;
}

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