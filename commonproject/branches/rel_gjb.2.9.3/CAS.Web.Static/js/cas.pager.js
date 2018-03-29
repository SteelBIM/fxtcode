//分页 kevin
;(function ($) {
    $.fn.casPager=$.casPager=function(args){
        if($(".pager").size()<=0)
        {
            var html='<table class="pager">                                                                 ';     
            html+='	<tfoot>                                                                ';     
            html+='		<tr>                                                                 ';     
            html+='			<td colspan="11">                                                  ';     
            html+='				<span class="water-table-listbtn">                               ';     
            html+='											</span>                                            ';     
            html+='				<span class="water-table-page">                                  ';     
            html+='					<span id="pagemsg" class="water-table-pagemsg">当前0/0页</span>';     
            html+='					<a id="fpbtn" class="btn mr5">首页</a>                 ';     
            html+='					<a id="rpbtn" class="btn mr5">上页</a>                 ';     
            html+='					<a id="npbtn" class="btn mr5">下页</a>                 ';     
            html+='					<a id="lpbtn" class="btn mr5">尾页</a>                 ';     
            html+='					<span id="pagemsg" class="water-table-pagemsg">跳转            ';     
            html+='					<input type="text" id="gpinput" size="3" value="0"/>页         ';     
            html+='					</span>                                                        ';     
            html+='					<a id="gpbtn" class="btn">跳转</a>                 ';     
            html+='				</span>                                                          ';     
            html+='			</td>                                                              ';     
            html+='		</tr>                                                                ';     
            html+='	</tfoot>                                                               ';     
            html+='</table>    ';
            var o = $(html).insertAfter($(this));
            o.casinput();
            $("#gpinput").casnumber();
            $("a",o).casbutton();
            initpage(args.page,args.pagecount,args.callback);
        }
        setpage(args.op,args.page,args.total,args.pagecount);
        function setpage(pagerow,pagenum,rowcount,pagecount){
            $("#pagemsg").html("当前" + pagenum + "/" +pagecount + "页&nbsp;共" +rowcount + "条");
            if (pagenum == 1) {
                $("#fpbtn").casdisable();
                $("#rpbtn").casdisable();
            }else {
                $("#fpbtn").casenable();
                $("#rpbtn").casenable();
            }
            if (pagenum == pagecount) {
                $("#npbtn").casdisable();
                $("#lpbtn").casdisable();
            }else {
                $("#npbtn").casenable();
                $("#lpbtn").casenable();
            }
            if(pagecount<=1) $("#gpbtn").casdisable();
            $("#gpinput").val(pagenum);
        }
        
        function initpage(pagenum,pagecount,callback){
            $("#fpbtn").click(function(){
                callback(1);
            });
            $("#rpbtn").click(function(){
                var pagenum = parseInt($("#gpinput").val());
    	        callback(pagenum - 1);
            });
            $("#npbtn").click(function(){
                var pagenum = parseInt($("#gpinput").val());
                if ((pagenum + 1) >= pagecount) 
        	        callback(pagecount);
                else 
        	        callback(pagenum + 1);
            });
            $("#lpbtn").click(function(){
    	        callback(pagecount);
            });
	        $("#gpbtn").click(function(){
                var tzys = $("#gpinput").val();
                var re = /^[1-9]+[0-9]*$/;
                if (tzys == null || tzys == undefined || tzys == '') {
                    alert("请输入跳转页数!");
                    $("#gpinput").focus();
                    return;
                }
                if (!re.test(tzys)) {
                    alert("请输入正确跳转页数!");
                    $("#gpinput").focus();
                    return;
                }
                if (tzys > pagecount) 
                    tzys = pagecount;
                if (tzys <= 0) 
                    tzys = 1;
                callback(tzys);
            });
        }
    }
    
        
})(jQuery)