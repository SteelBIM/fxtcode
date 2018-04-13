// JavaScript Document
var identifying;//验证码 
var winWidth = 0;//屏幕宽度
var winHeight = 0;//屏幕高度
var h_h = 0;//页头高度
var f_h = 0;//页脚高度
var maxH = 0;//内容区最大高度
var number = 0;//添加年级的变量
var niannum = "";//选择年级变量 
var classnum = new Array();

var demo = function () {
    this.getArry = function () {
        var classnum2 = classnum;
        return classnum2;
    }
}

var popup = function popup(aaa) {
    var black = document.createElement("div");
    var black1 = document.createElement("div");
    var black2 = document.createElement("h2");
    var black3 = document.createElement("div");
    var black4 = document.createElement("a");
    var black5 = document.createTextNode(aaa);
    var black6 = document.createTextNode("确定");
    black.className = 'zong';
    black1.className = 'zhezhao';
    black3.className = 'hezi';  
    black4.appendChild(black6);
    black2.appendChild(black5);
    black.appendChild(black1);
    black.appendChild(black3);
    black3.appendChild(black2);
    black3.appendChild(black4);
    document.body.appendChild(black);
    black1.onclick = function () {
        black.parentNode.removeChild(black);

    }
    black4.onclick = function () {
        black.parentNode.removeChild(black);

    }
};

$(function () {
    $("#inp1").blur(function () {
        yanzheng();
    });
    for (var i = 6; i > 0; i--) {
        qiehuan(i);
        jishu();
    }
    classnum.sort(paixu);
    shengcheng();
    select1();
    findDimensions();
    resizeImg();
});

$(function () {
    /*班级列表邀请学生弹框*/
    $(".footer .invite1").click(function () {
        $(".box").css("display", "block");
        $(".shadow1").css("display", "block");
    });
    $(".box .close").click(function () {
        $(".box").css("display", "none");
        $(".shadow1").css("display", "none");
    });
    //跳转到学校选择界面
    $(".school #inp2").click(function () {
        window.location.href = "selschoolone.html";
    });
});

function resizeImg() {
    //计算缩放比例  
    //ratio = w/720;
    $(".main").height(maxH);
}

//函数：获取并计算屏幕尺寸
function findDimensions() {
    //获取窗口宽度 
    if (window.innerWidth)
        winWidth = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        winWidth = document.body.clientWidth;
    //获取窗口高度 
    if (window.innerHeight)
        winHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))
        winHeight = document.body.clientHeight;
    //通过深入Document内部对body进行检测，获取窗口大小 
    if (document.documentElement && document.documentElement.clientHeight && document.documentElement.clientWidth) {
        winHeight = document.documentElement.clientHeight;
        winWidth = document.documentElement.clientWidth;
    }
    h_h = $(".header").height();//页头高度
    f_h = $(".footer").height();//页脚高度
    //自动调节主体内容高度
    maxH = winHeight - h_h - f_h;
    $(".main").height(maxH);
}

/*删除效果*/
function select1() {
    //删除数组中的元素的构建函数
    Array.prototype.indexOf = function (val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == val) return i;
        }
        return -1;
    };
    Array.prototype.remove = function (val) {
        var index = this.indexOf(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    };

    //选择班级修改	
    $(".Html4 .bao dl dt").click(function (e) {

        if ($(this).hasClass("on1")) {
            $(this).removeClass("on1");
            var b = $(this).html();
            classnum.remove((niannum + b));
            classnum.sort(paixu);
            shengcheng();
            jishu();
        }
        else {
            $(this).addClass("on1");
            var a = $(this).html();
            classnum.push((niannum + a));
            classnum.sort(paixu);
            shengcheng();
            jishu();
        }

    });
    /*点击已选班级取消选择的班级*/
    $(document).on("click", ".Html4 .ul2 li", function () {
        var c = $(this).html();
        classnum.remove(c);

        niannum = $(".Html4 .bao ul .on span").html();
        $(".Html4 .bao dl dt").removeClass("on1");
        for (var i = 0; i < classnum.length; i++) {
            for (var k = 1; k < 13; k++) {
                var d = classnum[i];
                if (d == (niannum + k + "班")) {
                    $(".Html4 .bao dl .dt" + k).addClass("on1");
                }
            }
        }
        classnum.sort(paixu);
        shengcheng();
        jishu();
    });
}

//切换班级和年级的效果
function qiehuan(j) {
    $(".Html4 .bao ul li").removeClass("on");
    $(".Html4 .bao ul .li" + j).addClass("on");

    niannum = $(".Html4 .bao ul .li" + j + " span").html();
    $(".Html4 .bao dl dt").removeClass("on1");
    for (var i = 0; i < classnum.length; i++) {
        for (var k = 1; k < 13; k++) {
            var c = classnum[i];
            if (c == (niannum + k + "班")) {
                $(".Html4 .bao dl .dt" + k).addClass("on1");
            }
        }
    }
}

//计算选择的个数
function jishu() {
    $(".Html4 .p3 span").html(classnum.length);
}

//生成选择的年级班级列表
function shengcheng() {
    $(".ul2").html("");
    var html1 = "";
    for (var i = 0; i < classnum.length; i++) {
        var c = classnum[i];
        var li = "<li>" + c + "</li>";
        html1 += li;
    }
    $(".ul2").html(html1);
}

//对选择的班级年级数组进行排序
function paixu(value1, value2) {
    var a, b;
    var nianji1 = value1.substr(0, 3);
    var nianji2 = value2.substr(0, 3);
    var banji1 = parseInt(value1.substr(3));
    var banji2 = parseInt(value2.substr(3));
    /*var banji3=banji1.substr(0,banji1.length-1);
	var banji4=banji2.substr(0,banji2.length-1);*/
    switch (nianji1) {
        case "一年级":
            a = 1;
            break;
        case "二年级":
            a = 2;
            break;
        case "三年级":
            a = 3;
            break;
        case "四年级":
            a = 4;
            break;
        case "五年级":
            a = 5;
            break;
        case "六年级":
            a = 6;
            break;
    }
    switch (nianji2) {
        case "一年级":
            b = 1;
            break;
        case "二年级":
            b = 2;
            break;
        case "三年级":
            b = 3;
            break;
        case "四年级":
            b = 4;
            break;
        case "五年级":
            b = 5;
            break;
        case "六年级":
            b = 6;
            break;
    }
    if (a < b) {

        return -1;

    } else if (a > b) {

        return 1;

    } else {
        if (banji1 < banji2) {

            return -1;

        } else if (banji1 > banji2) {

            return 1;

        } else {

            return 0;

        }
        return 0;

    }
}

/*判断手机号是否正确*/
function checkMobileStrict(v) {
    var yd = ['134', '135', '136', '137', '138', '139', '150', '151', '152', '157', '158', '159', '187', '188'];
    var lt = ['130', '131', '132', '155', '156', '185', '186'];
    var dx = ['133', '153', '180', '189'];
    var whole = []; whole = whole.concat(yd, lt, dx);
    if (v == '') return 1;
    if (v.length != 11) { return 2; }
    if (isNaN(v)) { return 2; }
    var phone_sect = v.substr(0, 3);
    var find = false;
    var i = 0;
    for (i = 0; (i < whole.length) ; i++)
    { if (phone_sect == whole[i]) { find = true; break; } }
    if (find) {
        return 0;
    }
    else {
        return 2;
    }
}

function yanzheng() {
    if (checkMobileStrict($("#inp1").val())) {
        $(".Html1 .get").css("background-color", "#FFB9C3");
        return false;
    }
    else {
        $(".Html1 .get").css("background-color", "#FF7387");
    }
}
