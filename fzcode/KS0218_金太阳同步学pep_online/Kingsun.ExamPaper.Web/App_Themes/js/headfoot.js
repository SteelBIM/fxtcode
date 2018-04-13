// JavaScript Document

$(function(){
	UpDownList();
	ShowWeixin();
	ShowRecLink();
	getSubject();
	indexCenter();
	
	/*鼠标点击body除下拉菜单以外的区域时隐藏页头下拉菜单*/		
	$("body").click(function(event){   
        var e = event || window.event || e; // 兼容IE7
        var obj = $(e.srcElement || e.target);
        if ($(obj).attr('class') == "centerLink" || $(obj).attr('id') == "userName" || $(obj).attr('class') == "selectList" || $(obj).attr('id') == "subject") {

        } else {
            $(".personalLink").slideUp();
			$(".centerLink h4 a").removeClass("down");
			$(".subjectList").slideUp();
			$(".selectList h4 a").removeClass("down");

        }
    });
	
	});
	
	
	
	/*页头个人中心下拉菜单显示*/
	function UpDownList(){
		$(".centerLink h4 a").click(function(){		
		var dis = $(this).parent().next("ul").css("display");
        if (dis == "block") {
            $(".personalLink").slideUp();
            $(this).removeClass("down");
        } else {
            $(".personalLink").slideDown();
            $(this).addClass("down");
           }
		   });
		   
	}
	
	/*页头学科选择下拉菜单显示*/
	function getSubject(){
		$(".selectList h4 a").click(function(){		
		var sh = $(this).parent().next("ul").css("display");
        if (sh == "block") {
            $(".subjectList").slideUp();
            $(this).removeClass("down");
        } else {
            $(".subjectList").slideDown();
            $(this).addClass("down");
           }
		   });
		   

	}
	
	/*页脚二维码显示*/
	function ShowWeixin(){
		$(".footCenter a.weixin").mouseover(function(){
			$(this).find("span").css("display","block");			
			}).mouseout(function () {
            $(this).find("span").css("display","none");
			});
		}
		
	/*页头应用（课例集、云课堂、电影课）菜单显示*/	
	function ShowRecLink(){
		    $(".sycRec a.a6").mouseenter(function(){
			    $(this).next("ul").css("display","block");			
			});
			$(".sycRec ul").mouseleave(function () {
                $(this).css("display","none");
			});			
		}
		
		
	//个人中心下拉列表实现
	function indexCenter(){
			$(".header .topTool li.liName a.userName").click(function(){	
				$(this).next(".downList").slideToggle();
				$(this).find("em").toggleClass("down");
		});
		$(".header .topTool li.liName").mouseleave(function(){
			$(this).find(".downList").slideUp();
			$(this).find("a.userName em").removeClass("down");
		})
	}

	

