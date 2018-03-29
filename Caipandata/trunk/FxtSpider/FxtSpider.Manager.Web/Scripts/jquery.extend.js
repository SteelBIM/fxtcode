function getPageScroll(dom)
{ 
  var domHeigth=0;
  if(dom!=window)
  {
      domHeigth=$(dom).offset().top; 
  }
  var yScroll; 
  if (self.pageYOffset) 
  { 
    yScroll = self.pageYOffset;         
    //xScroll = self.pageXOffset; 
  } 
  else if (document.documentElement && $(document).scrollTop())
  {
    yScroll = $(document).scrollTop();  
  } 
  else if (document.body) 
  { 
    yScroll = $("body").scrollTop();  
  } 
  var thisHeight=$(window).height()/2; 
  var _top=(yScroll+thisHeight)-domHeigth;
  if(_top<=0)
  {
  _top=1;
  }
  return _top;
} 
var block_loading_img_obj={message:"<img src=\"/Images/loading3.gif\" width='90px' heigth='90px'/>",
                                                                    overlayCSS:{backgroundColor:"White",cursor:"",opacity:0.3},
                                                                    centerY:false,
                                                                    css:{border:"none",backgroundColor:"none"}
                                                                    };
(function ($) {
    $.extend({
                extendAjax: function (data, fun,loadShow,templatecache) 
                            {
                                if(loadShow!=null)
                                {
                                    loadShow=jQuery.extend({
                                                               dom: "",
                                                               blockObj:null
                                                            },
                                                            loadShow);
                                    loadShow.blockObj=jQuery.extend(
                                                                   {message:"<img src=\"/Images/loading3.gif\" width='90px' heigth='90px'/>",
                                                                    overlayCSS:{backgroundColor:"White",cursor:"",opacity:0.3},
                                                                    centerY:false,
                                                                    css:{top:getPageScroll(loadShow.dom==""?window:loadShow.dom)+'px',border:"none",backgroundColor:"none"}
                                                                    },
                                                      loadShow.blockObj);
                                    if(loadShow.dom=="")
                                    {
                                        $.blockUI(loadShow.blockObj);
                                    }
                                    else
                                    {
                                        $(loadShow.dom).block(loadShow.blockObj);  
                                    }
                                }
                                data = jQuery.extend({data: "",url: "",type: "get",dataType: "json",dataToJSON: false,remote: true,cache: true}, data );
                                $.ajax({
                                        type: data.type,
                                        data: data.data,
                                        url: data.url,
                                        dataType: data.dataType,
                                        cache: false,
                                        context: { data: data },
                                        beforeSend: function (XHR) {},
                                        success:function (tmpldata) 
                                                {
                                                    if(loadShow!=null)
                                                    {
                                                        if(loadShow.dom=="")
                                                        {
                                                            $.unblockUI();
                                                        }
                                                        else
                                                        {
                                                            $(loadShow.dom).unblock();  
                                                        }
                                                    }
                                                    if (data.dataToJSON)
                                                    {
                                                        tmpldata = $.parseJSON(tmpldata);
                                                    }
                                                    if(data.dataType=="json")
                                                    {
                                                        if(tmpldata.result!=1&&tmpldata.result!="1")
                                                        {
                                                            //不为登录超时
                                                            if(tmpldata.errorType!="1"&&tmpldata.errorType!=1)
                                                            {
                                                                //alert(decodeURIComponent(tmpldata.message));
                                                            }
                                                        }
                                                    }
                                                    fun(tmpldata);
                                                },
                                        error: function (XmlHttpRequest, textStatus, errorThrown)
                                        {
                                            if(loadShow!=null)
                                            {
                                                if(loadShow.dom=="")
                                                {
                                                    $.unblockUI();
                                                }
                                                else
                                                {
                                                    $(loadShow.dom).unblock();  
                                                }
                                            }
                                            alert($.parseJSON(XmlHttpRequest.responseText).Message);
                                            //var dataError = $.parseJSON(XmlHttpRequest.responseText);
                                        }
                                  });

                              },
                extendAlter:function(message,_function)
                {
                    $.growlUI('',message,3000,_function); 
                }


    });
})(jQuery);
Date.prototype.format = function (format) {
    /* 
    * eg:format="yyyy-MM-dd hh:mm:ss"; 
    */
    var o = {
        "M+": this.getMonth() + 1, // month  
        "d+": this.getDate(), // day  
        "h+": this.getHours(), // hour  
        "m+": this.getMinutes(), // minute  
        "s+": this.getSeconds(), // second  
        "q+": Math.floor((this.getMonth() + 3) / 3), // quarter  
        "S": this.getMilliseconds()
        // millisecond  
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
                        - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                            ? o[k]
                            : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}