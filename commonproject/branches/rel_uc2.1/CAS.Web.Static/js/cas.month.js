//只显示月份的日期选择框  kevin
; (function ($) {
    $.fn.casDate = $.casDate = function (args) {
        var $this = $(this);
        var defaults = { format:"month" };
        args = $.extend({}, defaults, args);
        var width=160,height=130;
        switch(args.format){
            case "month":
                var div = document.createElement("div"); 
                var top = $this[0].offsetTop + $this.outerHeight();
                var left = $this.offset().left;                
                $(div).hide().css({position:"absolute",height:height,background:"#fff",width:width
                    ,border:"1px solid #ccc"}).insertAfter($this);
                var html=["<div class='casdate_title'><span class='casdate_clear'><a class='casdate_a_clear' href='javascript:void(0)'>CLS</a></span><span class='casdate_select'><select class='casdate_year' style='width:80px;'>"];
                var date = new Date();
                var maxyear=date.getFullYear();
                for (var i = maxyear; i >=1990; i--) {
                    html.push("<option value='"+i+"'>"+i+"年</option>");
                }
                html.push("</select></span><span class='casdate_close'><a class='casdate_a_close' href='javascript:void(0)'>×</a></span></div>");
                html.push("<div class='casdate_month' style='height:" + (height -26) + "px;'><ul>");
                for (var i = 1; i <= 12; i++) {
                    html.push("<li><a href='javascript:void(0)' month="+(i>9?i:"0"+i)+">"+i+"月</a></li>");
                }
                html.push("</ul></div>");
                $(div).html(html.join(""));
                
                var months=$(".casdate_month a",div);
                var curyear=maxyear,curmonth;
                $(".casdate_year",div).change(function(){
                    curyear = $(this).val();
                    setVal();
                });
                months.mouseover(function(){$(this).css({background:"#ccc"})})
                .mouseout(function(){$(this).css({background:"#fff"})}).mousedown(function(e){
                    months.removeClass("on");
                    $(this).addClass("on");
                    curmonth = $(this).attr("month");
                    setVal();
                    $(div).hide();
                    e.stopPropagation();
                });
                function setVal(v){
                    if(v){
                        curyear = v.substring(0,4);
                        curmonth = v.substring(5,7);
                        $(".casdate_year",div).val(curyear);
                        months.filter("[month="+curmonth + "]").addClass("on");                        
                        return;
                    }  
                    if(curyear && curmonth)
                        $this.val(curyear + "-" + curmonth);
                    else if(!curmonth){
                        months.removeClass("on");
                        $this.val("");
                    }
                    if(curyear){$(".casdate_year",div).val(curyear);}
                }
                $(div).mousedown(function(e){e.stopPropagation();});
                $(document).mousedown(function(){$(div).hide();});
                $(".casdate_a_clear",div).mousedown(function(e){curyear=maxyear;curmonth="";setVal();$(div).hide();e.stopPropagation();});
                $(".casdate_a_close",div).mousedown(function(e){$(div).hide();e.stopPropagation();});                
                $this.click(function(){$(div).css({top:top,left:left}).show();
                    //setVal($this.val());
                    });
                if($this.val()!=""){
                     setVal($this.val());
                    $this.val(curyear + "-" + curmonth);
                }
                break;
        }
    };
})(jQuery)