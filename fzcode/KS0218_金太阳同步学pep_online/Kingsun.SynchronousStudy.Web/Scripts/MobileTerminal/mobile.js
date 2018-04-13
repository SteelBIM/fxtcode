// JavaScript Document
var ratio=0;//缩放比例

//主体高度自动填应充
$(window).resize(function(){
    resizeImg();
});


//自动调节绝对定位对象的缩放尺寸
function autoSizeObj(obj){	
	var W=$(obj).width();
	var H=$(obj).height();
	var L=$(obj).css("left");
		L=L.replace("px","");
	var T=$(obj).css("top");
		T=T.replace("px","");
	var realW=W*ratio;
	var realH=H*ratio;
	var realL=L*ratio;
	var realT=T*ratio;	
	$(obj).width(realW);
	$(obj).height(realH);
	$(obj).css({"left":realL});
	$(obj).css({"top":realT});	
}

/*通用工具类方法
------------------------------------------------*/
//检测是否移动设备来访
function browserRedirect() {
    var sUserAgent = navigator.userAgent.toLowerCase();
    var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
    var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
    var bIsMidp = sUserAgent.match(/midp/i) == "midp";
    var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
    var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
    var bIsAndroid = sUserAgent.match(/android/i) == "android";
    var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
    var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
    if (bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) {
        return true;
    } else {
        return false;
    }
}

//写Cookie
function addCookie(objName, objValue, objHours) {
    var str = objName + "=" + escape(objValue);
    if (objHours > 0) {//为0时不设定过期时间，浏览器关闭时cookie自动消失
        var date = new Date();
        var ms = objHours * 3600 * 1000;
        date.setTime(date.getTime() + ms);
        str += "; expires=" + date.toGMTString();
    }
    document.cookie = str;
}

//读Cookie
function getCookie(objName) {//获取指定名称的cookie的值
    var arrStr = document.cookie.split("; ");
    for (var i = 0; i < arrStr.length; i++) {
        var temp = arrStr[i].split("=");
        if (temp[0] == objName) return unescape(temp[1]);
    }
    return "";
}

//去掉数组中的重复项
Array.prototype.unique = function() {
    var res = [], hash = {};
    for(var i=0, elem; (elem = this[i]) != null; i++)  {
        if (!hash[elem])
        {
            res.push(elem);
            hash[elem] = true;
        }
    };
    return res;
}

//JavaScript实现按照指定长度为数字前面补零输出
function PrefixInteger(num, length) {
	return (Array(length).join('0') + num).slice(-length);
}

/*--获取网页传递的参数：
    如获取Default.aspx?ID=123这个URL中ID的值时，调用方法：request("ID")
--*/
function request(paras)
{
    var url = location.href;
    var paraString = url.substring(url.indexOf("?")+1,url.length).split("&");
    var paraObj = {}
    for (i=0; j=paraString[i]; i++){
    paraObj[j.substring(0,j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=")+1,j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if(typeof(returnValue)=="undefined"){
    return "";
    }else{
    return returnValue;
    }
}

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

