$(function () {
});
//课本剧配音 type:0未提交，1：已提交
function HrefDubAction(type) {
    if (type == 0) {
        window.WebViewJavascriptBridge.callHandler(
            'gotoDubAction', function (responseData) {

            }
            );
    } else {    //查看
        window.WebViewJavascriptBridge.callHandler(
          'searchDubAction', function (responseData) {

          }
          );
    }
}
//故事演绎 type:0未提交，1：已提交
function HrefStarAction(type) { 
    if (type == 0) {
        window.WebViewJavascriptBridge.callHandler(
        'goToStarAction', function (responseData) {

        }
        );
    } else {    //查看
        window.WebViewJavascriptBridge.callHandler(
          'searchStarAction', function (responseData) {

          }
          );
    }
}
//根据按钮状态跳转页面
function ClickHref(UserID, StateID, AppID) {
    var LocalValue = GetQueryString("local");
    //如果是分享则跳转到下载地址页面
    if (LocalValue != null && LocalValue.toString().length > 0 && LocalValue.toString() == "weixin" || UserID == 0) {
        GetShareAddress(AppID);
        return false;
    }
    if (StateID == -1) {
        //跳转到报名页面Apply
        ConfirmPopupGoTo("您还未报名，快去报名吧！", "gotoDubAudio");
        //ConfirmPopup("您还未报名，快去报名吧！", "/Apply/Index?UserID=" + UserID + "&AppID=" + AppID + "");
    } else if (StateID == 0) {
        popup("比赛未开始，敬请期待吧！");
    } else if (StateID == 3) {  //显示查看成绩
        window.location.href = "../InterestDubbingGameMatch/UserGameMatchDetail_View?returnPage=MatchCatalog&UserID=" + UserID + "&AppID=" + AppID + "&type=2";
    } else {
        window.location.href = "../MatchCatalog/Index?returnPage=MatchCatalog&UserID=" + UserID + "&AppID=" + AppID + "";
    }
}
//学生荣誉榜
function StudentHonours(UserID) {
    //如果是分享则跳转到下载地址页面
    var LocalValue = GetQueryString("local");
    if (LocalValue != null && LocalValue.toString().length > 0 && LocalValue.toString() == "weixin" || UserID == 0) {
        GetShareAddress(AppID);
        return false;
    }
    //先判断身份，然后判断是否已加入班级
    $.post("../Apply/GetTB_UUMSUser", { UserID: UserID }, function (data) {
        //alert(data);
        data = eval(data);
        if (data.Success) {
            var UserType = data.Data.UserType;
            var rdm = Math.random(); 
            //判断身份是否是学生
            if (UserType == 26) {
                //判断是否已加入班级
                $.post("../Apply/CheckClassInfoByStuID", { UserID: UserID }, function (data) {
                    //alert(data);
                    data = eval(data);
                    if (data.Success) {
                        window.location.href = "../DubbingContest/honors-student.html?UserID=" + UserID + "&AppID=" + AppID + "&rdm="+rdm+"";
                    } else {
                        //popup("加入班级后才可报名");
                        ChangeClass();
                        //alert(data.Msg);
                    }
                });
            } else {    //老师
                window.location.href = "../DubbingContest/honors-student.html?UserID=" + UserID + "&AppID=" + AppID + "&rdm=" + rdm + "";
            }
        } else {
            popup(data.Msg);
        }
    });
}
//加入班级
function ChangeClass() {
    var black = document.createElement("div");
    var black1 = document.createElement("div");
    var black2 = document.createElement("div");
    var black3 = document.createElement("p");
    var black4 = document.createTextNode("加入班级后才可看排行榜");
    black.className = 'zong';
    black1.className = 'zhezhao';
    black2.className = 'hezi';
    black3.appendChild(black4);
    black2.appendChild(black3);
    black.appendChild(black1);
    black.appendChild(black2);
    document.body.appendChild(black);
    black1.onclick = function () {
        black.parentNode.removeChild(black);
        window.WebViewJavascriptBridge.callHandler(
         'ChangeClass', function (responseData) {} );
    }
}
//
//确认提示框
function popup2(aaa, href) {
    var black = document.createElement("div");
    var black1 = document.createElement("div");
    var black2 = document.createElement("div");
    var black3 = document.createElement("p");
    var black5 = document.createElement("a");
    var black6 = document.createElement("a");
    var a1 = document.createTextNode("确定");
    var a2 = document.createTextNode("取消");
    var black4 = document.createTextNode(aaa);
    black.className = 'zong1';
    black1.className = 'zhezhao1';
    black2.className = 'box2';
    black5.appendChild(a1);
    black6.appendChild(a2);
    black3.appendChild(black4);
    black2.appendChild(black3);
    black2.appendChild(black5);
    black2.appendChild(black6);
    black.appendChild(black1);
    black.appendChild(black2);
    document.body.appendChild(black);
    black6.onclick = function () {
        black.parentNode.removeChild(black);
    }
    black5.onclick = function () {
        black.parentNode.removeChild(black);
        window.location.href = href;
    }
};
//提示框
function popup(aaa) {
    var black = document.createElement("div");
    var black1 = document.createElement("div");
    var black2 = document.createElement("div");
    var black3 = document.createElement("p");
    var black4 = document.createTextNode(aaa);
    black.className = 'zong';
    black1.className = 'zhezhao';
    black2.className = 'hezi';
    black3.appendChild(black4);
    black2.appendChild(black3);
    black.appendChild(black1);
    black.appendChild(black2);
    document.body.appendChild(black);
    black1.onclick = function () {
        black.parentNode.removeChild(black);

    }
};
//提示框，确定后再跳转通过App端跳转到页面
function ConfirmPopupGoTo(aaa, gotoName) {
    var black = document.createElement("div");
    var black1 = document.createElement("div");
    var black2 = document.createElement("div");
    var black3 = document.createElement("p");
    var black5 = document.createElement("a");
    var black6 = document.createElement("a");
    var a1 = document.createTextNode("确定");
    var a2 = document.createTextNode("取消");
    var black4 = document.createTextNode(aaa);
    black.className = 'zong1';
    black1.className = 'zhezhao1';
    black2.className = 'box2';
    black5.appendChild(a1);
    black6.appendChild(a2);
    black3.appendChild(black4);
    black2.appendChild(black3);
    black2.appendChild(black5);
    black2.appendChild(black6);
    black.appendChild(black1);
    black.appendChild(black2);
    document.body.appendChild(black);
    black6.onclick = function () {
        black.parentNode.removeChild(black);
    }
    black5.onclick = function () {
        black.parentNode.removeChild(black);
        window.WebViewJavascriptBridge.callHandler(
         gotoName, function (responseData) {

         }
         );
    }
}
//提示框，确定后再跳转
function ConfirmPopup(aaa, href) {
    var black = document.createElement("div");
    var black1 = document.createElement("div");
    var black2 = document.createElement("div");
    var black3 = document.createElement("p");
    var black4 = document.createTextNode(aaa);
    black.className = 'zong';
    black1.className = 'zhezhao';
    black2.className = 'hezi';
    black3.appendChild(black4);
    black2.appendChild(black3);
    black.appendChild(black1);
    black.appendChild(black2);
    document.body.appendChild(black);
    black1.onclick = function () {
        black.parentNode.removeChild(black);
        window.location.href = href;
    }
}
//根据AppID获取分享地址
function GetShareAddress(AppID) {
    if (AppID == 0) {
        window.location.href = "http://tbx.kingsun.cn/downloadList.html";
    } else if (AppID == "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385") {//广州版
        window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gz.syslearning";
    } else if (AppID == "43716a9b-7ade-4137-bdc4-6362c9e1c999") {//上海本地
        window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shbd.syslearning";
    }
    else if (AppID == "241ea176-fce7-4bd7-a65f-a7978aac1cd2") {//深圳版
        window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.sz.syslearning";
    }
    else if (AppID == "9426808e-da8e-488c-9827-b082c19b62a7") {//上海全国
        window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shqg.syslearning";
    }
    else if (AppID == "0a94ceaf-8747-4266-bc05-ed8ae2e7e410") {//北京
        window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.bj.syslearning";
    }
    else if (AppID == "5373bbc9-49d4-47df-b5b5-ae196dc23d6d") {//人教pep
        window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.rj.syslearning";
    }
    else if (AppID == "37ca795d-42a6-4117-84f3-f4f856e03c62") {//广东
        window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gd.syslearning";
    } else {
        window.location.href = "http://tbx.kingsun.cn/downloadList.html";
    }
    return false;
}

//采用正则表达式获取地址栏参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
