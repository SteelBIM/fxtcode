$(function () {
    $("#signOutLogin").bind("click", function () {
        DA_Confirm("退出", "是否确认退出", function (ok) {
            if (ok) {
                window.location.href = Url_Login_Login_Out;
            }
        });
    });

});

/**设置头部导航显示**/
function SetLeftMenu(menuIndex) {
    var menus = menuIndex.split("_");
    var subMenuDom = $(".left_submenu" + menus[0]);
    subMenuDom.addClass("active");
    var menuDom = subMenuDom.find(".left_menu" + menus[1]);
    if (menuDom.length > 0) {
        subMenuDom.addClass("open");
        menuDom.addClass("active");
    }

    var subMenuText = subMenuDom.find(".submenuText").html();
    var menuText = menuDom.find(".menuText").text();
    var navigationDom1 = $("#navigationItem").clone();
    navigationDom1.attr("id", "").attr("style", "").html(subMenuText);
    $("#breadcrumb").append(navigationDom1);
    if (menuDom.length > 0) {
        var navigationDom2 = $("#navigationItem").clone();
        navigationDom2.attr("id", "").show().html(menuText);
        $("#breadcrumb").append(navigationDom2);
    }
}