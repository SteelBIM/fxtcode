
var sort = function () {

    var Initials = $('.initials');
    var LetterBox = $('#letter');
    Initials.find('ul').append('<li>A</li><li>B</li><li>C</li><li>D</li><li>E</li><li>F</li><li>G</li><li>H</li><li>I</li><li>J</li><li>K</li><li>L</li><li>M</li><li>N</li><li>O</li><li>P</li><li>Q</li><li>R</li><li>S</li><li>T</li><li>U</li><li>V</li><li>W</li><li>X</li><li>Y</li><li>Z</li><li>#</li>');
    initials();
    $(".num_name").click(function () {
        //alert($(this).html() + "||" + $(this).attr("id"));
        var userId = getQueryString("UserID");
        var provinceID = getQueryString("provinceID");
        var cityID = getQueryString("cityID");
        var districtID = getQueryString("districtID");
        var schoolID = getQueryString("schoolID");
        var editionID = getQueryString("editionID");
        var truename = getQueryString("TrueName");

        //如果先选择版本，那每次传输都戴上版本参数
        var strwhere = "";
        if (editionID != "" && editionID != "undefined" && editionID != null) {
            strwhere += "&editionID=" + editionID;
        }

        if (truename != "" && truename != "undefined" && truename != null) {
            strwhere += "&TrueName=" + truename;
        }

        //如果参数中有eType=23则表示选择版本
        var eType = getQueryString("eType");
        if (eType == "23") {
            var where = "";
            if (provinceID != "" && provinceID != "undefined" && provinceID != null) {
                where += "&provinceID=" + provinceID;
            }
            if (cityID != "" && cityID != "undefined" && cityID != null) {
                where += "&cityID=" + cityID;
            }
            if (districtID != "" && districtID != "undefined" && districtID != null) {
                where += "&districtID=" + districtID;
            }
            if (schoolID != "" && schoolID != "undefined" && schoolID != null) {
                where += "&schoolID=" + schoolID;
            }
            window.location.href = "../StudyReportManagement/AddInformation.aspx?editionID=" + $(this).html() + "_" + $(this).attr("id") + where + "&UserID=" + userId + "&TrueName=" + truename;

        } else {
            if (provinceID == "" || provinceID == "undefined" || provinceID == null) {
                window.location.href = "../StudyReportManagement/AddInformation.aspx?provinceID=" + $(this).html() + "_" + $(this).attr("id") + "&UserID=" + userId + strwhere;
            } else if (cityID == "" || cityID == "undefined" || cityID == null) {
                window.location.href = "../StudyReportManagement/AddInformation.aspx?provinceID=" + provinceID + "&cityID=" + $(this).html() + "_" + $(this).attr("id") + strwhere + "&UserID=" + userId;
            } else if (districtID == "" || districtID == "undefined" || districtID == null) {
                window.location.href = "../StudyReportManagement/AddInformation.aspx?provinceID=" + provinceID + "&cityID=" + cityID + "&UserID=" + userId + "&districtID=" + $(this).html() + "_" + $(this).attr("id") + strwhere;
            } else if (schoolID == "" || schoolID == "undefined" || schoolID == null) {
                window.location.href = "../StudyReportManagement/AddInformation.aspx?provinceID=" + provinceID + "&cityID=" + cityID + "&UserID=" + userId + "&districtID=" + districtID + "&schoolID=" + $(this).html() + "_" + $(this).attr("id") + strwhere;
            }
        }
    });



    $(".initials ul li").click(function () {
        var _this = $(this);
        var LetterHtml = _this.html();
        LetterBox.html(LetterHtml).fadeIn();

        Initials.css('background', 'rgba(145,145,145,0.6)');

        setTimeout(function () {
            Initials.css('background', 'rgba(145,145,145,0)');
            LetterBox.fadeOut();
        }, 1000);

        var _index = _this.index()
        if (_index == 0) {
            $('html,body').animate({ scrollTop: '0px' }, 300);//点击第一个滚到顶部
        } else if (_index == 27) {
            var DefaultTop = $('#default').position().top;
            $('html,body').animate({ scrollTop: DefaultTop + 'px' }, 300);//点击最后一个滚到#号
        } else {
            var letter = _this.text();
            if ($('#' + letter).length > 0) {
                var LetterTop = $('#' + letter).position().top;
                $('html,body').animate({ scrollTop: LetterTop - 45 + 'px' }, 300);
            }
        }
    })

    var windowHeight = $(window).height();
    var InitHeight = windowHeight - 45;
    Initials.height(InitHeight);
    var LiHeight = InitHeight / 28;
    Initials.find('li').height(LiHeight);


}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return decodeURI(r[2]);
    } else {
        return null;
    }
}

function initials() {//公众号排序
    var SortList = $(".sort_list");
    var SortBox = $(".sort_box");
    SortList.sort(asc_sort).appendTo('.sort_box');//按首字母排序
    function asc_sort(a, b) {
        return makePy($(b).find('.num_name').text().charAt(0))[0].toUpperCase() < makePy($(a).find('.num_name').text().charAt(0))[0].toUpperCase() ? 1 : -1;
    }

    var initials = [];
    var num = 0;
    SortList.each(function (i) {
        var initial = makePy($(this).find('.num_name').text().charAt(0))[0].toUpperCase();
        if (initial >= 'A' && initial <= 'Z') {
            if (initials.indexOf(initial) === -1)
                initials.push(initial);
        } else {
            num++;
        }

    });

    $.each(initials, function (index, value) {//添加首字母标签
        SortBox.append('<div class="sort_letter" id="' + value + '">' + value + '</div>');
    });
    if (num != 0) { SortBox.append('<div class="sort_letter" id="default">#</div>'); }

    for (var i = 0; i < SortList.length; i++) {//插入到对应的首字母后面
        var letter = makePy(SortList.eq(i).find('.num_name').text().charAt(0))[0].toUpperCase();
        switch (letter) {
            case "A":
                $('#A').after(SortList.eq(i));
                break;
            case "B":
                $('#B').after(SortList.eq(i));
                break;
            case "C":
                $('#C').after(SortList.eq(i));
                break;
            case "D":
                $('#D').after(SortList.eq(i));
                break;
            case "E":
                $('#E').after(SortList.eq(i));
                break;
            case "F":
                $('#F').after(SortList.eq(i));
                break;
            case "G":
                $('#G').after(SortList.eq(i));
                break;
            case "H":
                $('#H').after(SortList.eq(i));
                break;
            case "I":
                $('#I').after(SortList.eq(i));
                break;
            case "J":
                $('#J').after(SortList.eq(i));
                break;
            case "K":
                $('#K').after(SortList.eq(i));
                break;
            case "L":
                $('#L').after(SortList.eq(i));
                break;
            case "M":
                $('#M').after(SortList.eq(i));
                break;
            case "N":
                $('#N').after(SortList.eq(i));
                break;
            case "O":
                $('#O').after(SortList.eq(i));
                break;
            case "P":
                $('#P').after(SortList.eq(i));
                break;
            case "Q":
                $('#Q').after(SortList.eq(i));
                break;
            case "R":
                $('#R').after(SortList.eq(i));
                break;
            case "S":
                $('#S').after(SortList.eq(i));
                break;
            case "T":
                $('#T').after(SortList.eq(i));
                break;
            case "U":
                $('#U').after(SortList.eq(i));
                break;
            case "V":
                $('#V').after(SortList.eq(i));
                break;
            case "W":
                $('#W').after(SortList.eq(i));
                break;
            case "X":
                $('#X').after(SortList.eq(i));
                break;
            case "Y":
                $('#Y').after(SortList.eq(i));
                break;
            case "Z":
                $('#Z').after(SortList.eq(i));
                break;
            default:
                $('#default').after(SortList.eq(i));
                break;
        }
    };
}
