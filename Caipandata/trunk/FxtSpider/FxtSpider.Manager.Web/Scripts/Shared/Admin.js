/**设置头部导航显示**/
function SetLeftMenu(menuIndex) 
{
    var menus = menuIndex.split("_");
    var subMenuDom=$(".left_submenu" + menus[0]);
    var menuDom=subMenuDom.find(".left_menu" + menus[1]);
    var subMenuText=subMenuDom.find(".submenuText").html();
    var menuText=menuDom.find(".menuText").html();
    subMenuDom.addClass("open").addClass("active");
    menuDom.addClass("active");
    var navigationDom1=$("#navigationItem").clone();
    navigationDom1.attr("id","").show().html(subMenuText);
    $("#breadcrumb").append(navigationDom1);
    if(menuDom.length>0)
    {
        var navigationDom2=$("#navigationItem").clone();
        navigationDom2.attr("id","").show().html(menuText);
        $("#breadcrumb").append(navigationDom2);
    }
}