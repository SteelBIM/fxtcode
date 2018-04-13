var Volume = function () {
    var Current = this;
    this.GradeID = Common.QueryString.GetValue("GradeID");
    this.ClassID = Common.QueryString.GetValue("ClassID");
    this.Init = function () {
        Current.GetUnitInfo();
    };

    this.GetUnitInfo = function () {
        var obj = { GradeID: Current.GradeID, ClassID: Current.ClassID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=getunitbygradeid", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);
                if (result.Success) {
                    var classhtml = "";
                    var items = result.Book;
                    for (var i = 0; i < items.length; i++) {
                        if (i == 0) {
                            bindDefaultInfo(Current.ClassID, items[i].BookID, items[i].BookName);
                        }
                        classhtml += "<li   id=\"" + items[i].BookID + "\" >" + items[i].BookName + "</li>";
                    } 
                    $("#div_Class").html(classhtml);
                } else {
                    var selHtml = '';
                    selHtml += '<span>--暂无册别--</span><i></i>';
                    $(".select-tit").html(selHtml);
                    $("#ul_Unit").html("");
                }
            }
        });
    };
};
//默认显示此班级年级对应的册别信息
function bindDefaultInfo(ClassID, BookID, BooKName) {
    //默认显示册别
    var selHtml = '';
    selHtml += '<span class="on">' + BooKName + '</span><i></i>';
    $(".select-tit").html(selHtml);
    //目录
    bindCatalogInfo(BookID, ClassID);
}
//绑定目录
function bindCatalogInfo(BookID, ClassID) {
    $.post("../Handler/WeChatHandler.ashx?queryKey=getcatalogbybookid", { BooKID: BookID }, function (data) {
        if (data) {
            var result = JSON.parse(data);
            if (result.Success) {
                var catalogHtml = '';
                var items = result.Data;
                for (var i = 0; i < items.length; i++) {
                    if (parseInt(items[i].Num) > 0) {
                        catalogHtml += ' <li><a href="SpeechEval.aspx?ClassID=' + ClassID + '&ParentID=' + items[i].CatalogID + '">' + items[i].ParentCatalogName + '/' + items[i].CatalogName + '</a></li>';
                    }
                    else {
                        catalogHtml += ' <li  class="noclick"><a href="#">' + items[i].ParentCatalogName + '/' + items[i].CatalogName + '</a></li>';
                    }
                }
                $("#ul_Unit").html(catalogHtml);
            } else {
                return false;
            }
        }
    });
}
/*切换册别*/
$(document).on('click', ".single-select1 .select-items ul li", function (e) {
    var nameS = "";
    $(".single-select1 .select-items li").removeClass("selected");
    $(this).addClass("selected");
    nameS = $(this).html();
    $(".select-tit span").addClass("on");
    $(".select-tit span").html(nameS);
    var BookID = $(this).attr("id");
    var ClassID = Common.QueryString.GetValue("ClassID");
    bindCatalogInfo(BookID, ClassID);
});

var volumeInit;
$(function () {
    volumeInit = new Volume();
    volumeInit.Init();
});