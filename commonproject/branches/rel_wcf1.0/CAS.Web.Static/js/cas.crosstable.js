//交叉表，用于房号列表区展示 kevin
; (function ($) {    
    $.fn.casCrossTable = $.casCrossTable =
    function (args) {
        var that = this;
        var $this = $(this);        
        //重设单元格信息区域位置
        if(args.setdata){
            setdata(args.refresh,args.key,args.data,args.crossdata,args.iddata);
            return;
        }
        function setdata(refresh,key,data,cdata,iddata){
            if (data.length>0) {
                var html = [];
                $.each(data, function (i, item) {
                    var tr=$("tr[row=" + item[key] + "]",$this);
                    $.each(item,function(j,cell){
                         $("td[index='" +j + "']",tr).children().eq(0).text(cell + " ");
                    }); 
                });
                if(refresh){
                    crossdata = cdata;
                    window.tddatas.length=0; 
                    $.each(data, function (i, item) {
                        $.each(item, function (j, o) {
                            if(key==j) return;
                            var tddata={};
                            $.each(crossdata,function(c,cr){
                                tddata = $.extend({},tddata,JSON2.parse('{"' + cr.key + '":"' + cr.data[i][j] + '"}'));
                            });
                            window.tddatas.push({id:iddata[i][j],data:tddata});                            
                        });
                    });
                }
            }
        }
        $this.attr("cellpadding", 0).attr("cellspacing", 0);
        var crossdata =null;
        window.tddatas=[];
        var mask = $("#crossmask");
        if(!mask[0])
        {
            //穿透用的层，这里不用改变TD背景实现是因为在IE中过慢 kevin
            var mask=$("<div id='crossmask' style='pointer-events:none;filter:alpha(opacity=30);opacity:0.3;background:#09c ;border:0px solid #000;position:absolute'/>");
            function noPointerEvents (element) {
                $(element).bind('click mousedown mouseup mousemove', function (evt) {
                   this.style.display = 'none';
                   var x = evt.pageX, y = evt.pageY, 	    
                   under = document.elementFromPoint(x, y);
                   this.style.display = '';
                   evt.stopPropagation();
                   evt.preventDefault();               
                   $(under).trigger(evt.type);
                }); 
            }
            mask.appendTo(document.body);    
            //穿透层事件
            noPointerEvents(mask[0]);
        }
        //当前选中项
        function setCurrent(){
            var oldrow = $(document).data("crossoldrow");
            var oldcol = $(document).data("crossoldcol");    
            var newrow = $(document).data("crossnewrow");
            var newcol = $(document).data("crossnewcol");       
            if(oldrow>0 && oldcol>0){
                var oldtd=$($this[0].rows[oldrow].cells[oldcol]);
                var newtd=$($this[0].rows[newrow].cells[newcol]);
                var top = oldtd.position().top;
                var left= oldtd.position().left;
                var width =newtd.position().left-left+newtd.outerWidth(); 
                var height = newtd.position().top-top+newtd.outerHeight(); 
                mask.appendTo(oldtd);
                mask.show();
                mask.css({top:top,left:left,width:width,height:height});
            }
        }
        $(window).bind("resize",function(){setTimeout(setCurrent,100)});
        
        function callback(key,data,iddata,crossdata) {
            if (data.length>0) {
                var key= key;
                var html = [];
                
                $this.empty();
                $.each(data, function (i, item) {
                    if (i == 0)//第一行
                    {
                        html.push("<colgroup><col/>");
                        $.each(item, function (j, o) {
                            if(args.key==j) return;
                            html.push("<col index='"+j+"'/>")
                        });
                        html.push("</colgroup>");
                        html.push("<tr row='0'><td row='0' index='0' class='rowheader colheader'>&nbsp;</td>");
                        $.each(item, function (j, o) {
                            if(args.key==j) return;
                            html.push("<td row='0' class='rowheader' index='" + j + "'>" + j + "</td>");
                        });
                        html.push("</tr>");
                    }                    
                    html.push("<tr row='"+item[args.key]+"'><td index='0' class='colheader'><div>" + item[args.key] + "</div></td>");
                    $.each(item, function (j, o) {
                        if(args.key==j) return;
                        var tddata={};
                        $.each(crossdata,function(c,cr){
                            tddata = $.extend({},tddata,JSON2.parse('{"' + cr.key + '":"' + cr.data[i][j] + '"}'));
                        });
                        window.tddatas.push({id:iddata[i][j],data:tddata});
                        html.push("<td index='" + j + "' id='"+iddata[i][j]+"'><div class='crosscontent'>" + o + "&nbsp;</div></td>");
                    });
                    html.push("</tr>");
                    
                });
                $this.html(html.join("")).show();
                //$this.width(($this[0].rows[0].cells.length-1)*50 + 20);
                var isdown=false;//普通单元格
                var iscoldown=false;//列头
                var isrowdown=false;//行头
                var isalldown=false;//左上角
                var oldrow=-1;
                var oldcol=-1;
                var newrow=-1;
                var newcol=-1;
                $("td",$this).bind("mousedown", function () {
                    var td = $(this);
                    var row = td.parent().attr("row");
                    var index = td.attr("index");
                    oldrow=td.parent()[0].rowIndex;
                    oldcol=td[0].cellIndex;
                    newrow=oldrow;
                    newcol=oldcol;
                    
                    if(oldrow>0 && oldcol>0){                               
                        isdown=true;
                    }else if(oldrow>0){
                        newrow = oldrow;
                        oldcol=1;
                        newcol=$this[0].rows[0].cells.length-1;
                        isrowdown=true;
                    }else if(oldcol>0){                        
                        newcol = oldcol;
                        oldrow=1;
                        newrow=$this[0].rows.length-1;                        
                        iscoldown=true;
                    }else{
                        oldrow=1;
                        newrow=$this[0].rows.length-1;
                        oldcol=1;
                        newcol=$this[0].rows[0].cells.length-1;
                        isalldown=true;                        
                    }
                    $(document).data("crossoldrow",oldrow);
                    $(document).data("crossoldcol",oldcol);    
                    $(document).data("crossnewrow",newrow);
                    $(document).data("crossnewcol",newcol);                                        
                    setCurrent();
                }).bind("mousemove",function(){
                    var td = $(this);
                    newrow=td.parent()[0].rowIndex;
                    newcol=td[0].cellIndex;
                    var rowstart = -1;
                    var rowend = -1;
                    var colstart = -1;
                    var colend =-1; 
                    if(isdown){
                        //log(oldrow+","+newrow+","+oldcol+","+newcol);
                        if(newrow>0 && newcol>0){
                            rowstart = Math.min(oldrow,newrow);
                            rowend = Math.max(oldrow,newrow);
                            colstart = Math.min(oldcol,newcol);
                            colend = Math.max(oldcol,newcol);                               
                        }
                    }else if(iscoldown){
                        if(newrow==0){
                            colstart = Math.min(oldcol,newcol);
                            colend = Math.max(oldcol,newcol);  
                            rowstart=1;
                            rowend= $this[0].rows.length-1;
                        }    
                    }else if(isrowdown){
                        if(newcol==0){
                            rowstart = Math.min(oldrow,newrow);
                            rowend = Math.max(oldrow,newrow);
                            colstart = 1;
                            colend=$this[0].rows[0].cells.length-1;                            
                        }    
                    }
                    if(isdown || iscoldown || isrowdown){
                        $(document).data("crossoldrow",rowstart);
                        $(document).data("crossoldcol",colstart);                           
                        $(document).data("crossnewrow",rowend);
                        $(document).data("crossnewcol",colend);
                        setCurrent();
                    }
                })
                .bind("mouseup",function(){
                    if(isdown || iscoldown || isrowdown || isalldown){
                        isdown=false;iscoldown=false;isrowdown=false;isalldown=false;
                        if(args.callback){ 
                            var oldrow = $(document).data("crossoldrow");
                            var oldcol = $(document).data("crossoldcol");    
                            var newrow = $(document).data("crossnewrow");
                            var newcol = $(document).data("crossnewcol");  

                            //返回选中的项
                            var roldrow =$($this[0].rows[oldrow]).attr("row");
                            var rnewrow =$($this[0].rows[newrow]).attr("row");
                            var roldcol =$($this[0].rows[0].cells[oldcol]).attr("index");
                            var rnewcol =$($this[0].rows[0].cells[newcol]).attr("index");

                            if(oldrow>0 && oldcol>0 && newrow>0 && newcol>0){
                                var ids=[];
                                var iddata=null;
                                for (var i = oldrow; i <= newrow; i++) {
                                    for (var j = oldcol; j <= newcol; j++) {
                                        var id=$($this[0].rows[i].cells[j]).attr("id");
                                        if(id) ids.push(id);
                                    }
                                }
                                //一个单元格时返回其关联数据
                                if(ids.length==1){
                                    $.each(window.tddatas,function(x,tddata){
                                        if(tddata.id==ids[0]){
                                            iddata = tddata.data;
                                            return false;  
                                        }
                                    });
                                }
                                args.callback(ids,$.extend({}, iddata, {oldrow:roldrow,newrow:rnewrow,oldcol:roldcol,newcol:rnewcol}));
                                 
                            }
                        }
                    }
                })
                .bind("selectstart",function(){return false;});
                $this.bind("selectstart",function(){return false;});
                
            }
        }
        callback(args.key,args.data,args.iddata,args.crossdata);
    }
})(jQuery)