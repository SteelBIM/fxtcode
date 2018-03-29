var msgTime = null;
//新消息处理 kevin
function getNewMessages() {

    //新消息总数
    function getmsgcount(cnt) {
        if (cnt > 0) {
            $("#navtab li em.msgcnt").html("<i>" + cnt + "</i>").swapClass("icon_number");
            getmsg("one", onemsg);
        }
        else { $("#navtab li em.msgcnt").html("").removeClass("icon_number"); }
    }
    //取新消息api
    function getmsg(type, callback) {
        CAS.API({ type: "post", api: "messages.getnewmessage", data: { type: type, userid: CAS.Define.userid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, systypecode: CAS.Define.systypecode }, callback:
        function (d) {
            if (d.returntype == 1) {
                callback(d.data);
            } else {
                window.clearInterval(msgTime);
            }
        }
        });
    }
    //最新一个消息，可右下角弹出
    function onemsg(msg) {
        this.handlernewmsg = function () {
            var newmsgid = $.cookie("newmsgid");
            if (newmsgid == msg.id) return; //不重复弹出 
            
            if (CAS.isC()) { }
            else {
                var divnewmsg = $("#divnewmsg");
                var time;
                function hidemsg() {
                    divnewmsg.animate({ bottom: -5 - height }, 500, function () { divnewmsg.hide(); });
                }
                if (!divnewmsg.attr("id")) {
                    divnewmsg = $("<div id='divnewmsg' class='msg_box'><iframe frameborder=0 id='msg_frame' class='w100p h100p'></iframe></div>").appendTo(document.body);
                    divnewmsg.bind("mouseenter", function () { clearTimeout(time); })
                    .bind("mouseout", function () { time = setTimeout(hidemsg, 3000); });
                }
                divnewmsg.hide();
                var height = divnewmsg.height();
                $("#msg_frame").attr("src", CAS.RootUrl + "msg.aspx?msgid=" + msg.id).bind("load", function () {
                    divnewmsg.css({ bottom: -5 - height }).show().animate({ bottom: 5 }, 500);
                    time = setTimeout(hidemsg, 3000);
                });
                $.cookie("newmsgid", msg.id, { path: "/" });
            }
        }
        if($.cookie)
            this.handlernewmsg();
        else
            CAS.Use(["jquery.cookie.js"], function () {
                this.handlernewmsg();
            });
            
    }
    //10秒循环，本地调试不启用，避免代码重复进入断点，影响调试 kevin
    if(!CAS.Debug)
        msgTime = setInterval(function () { getmsg("count", getmsgcount); }, 10000);
    getmsg("count", getmsgcount);
}
//msg.aspx控制的消失消息框事件
function HideMsgBox() {
    var divnewmsg = $("#divnewmsg");
    if (divnewmsg.attr("id")) {
        divnewmsg.hide();
    }
}

$(function () {
    if(CAS.isC())
        $(".user_header").casmovec(); //如果在C++窗口中，操作头部移动窗体
    $("#navtab").cassimpletab(); //firstmaster中的tab切换
    $("#exit").casexit(); //退出
    $("span.icon_message").hide();
    $("div.user_header>p.title>em").html("["+CAS.Define.cityname+"][" + CAS.Define.userid + "]" + CAS.Define.username + "，您好！");    
    $("#usersetup").casusersetup(); //用户个人设置
    getNewMessages(); //取新消息
});