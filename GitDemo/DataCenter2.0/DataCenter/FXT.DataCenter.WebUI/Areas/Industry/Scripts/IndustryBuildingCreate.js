//面包屑导航
$("#navigation .breadcrumb li:eq(1)").remove();
$("#navigation .breadcrumb li:eq(2)").remove();
var html = "<li><span><a href=\"/Industry/IndustryProject/Index\">工业基础数据</a></span> <i class=\"icon-angle-right\"></i></li>";
html += "<li><span><a href=\"javascript:history.go(-1);\">工业楼栋</a></span> <i class=\"icon-angle-right\"></i></li>";
if ('@Model' == "") {
    html += "<li><span>新增</span></li>";
} else {
    html += "<li><span>编辑</span></li>";
}
$("ul.breadcrumb").append(html);

$(document).ready(function () {
    $("#SaleDate,#EndDate").datepicker({ format: "yyyy-mm-dd" });

    //确定
    $("#btnSave").on("click", function () {
        var projectId = $("#ProjectId").val();
        var buildingId = $("#BuildingId").val() ? $("#BuildingId").val() : -1;
        var buildingName = $.trim($("#BuildingName").val());

        if (buildingName == "") {
            $("#msgForBuildingName").text("*必填");
            return;
        }
        else {
            $("#msgForBuildingName").text("*");
        }
        alert(projectId);
        alert(buildingId);
        alert(buildingName);

        $.post('@Url.Action("IsExistBuildingIndustry")', { projectId: projectId, buildingId: buildingId, buildingName: buildingName }, function (data) {
            if (data) {
                alert("该工业楼栋已存在");
            } else {
                $("#mainForm").submit();
            }
        });
    });

    //加载百度地图
    initBaiduMap();
});

function initBaiduMap() {

    var lay, //弹出层
        map, //地图对象
        initLngAndLat = [{ Lng: '114.025974', Lat: '22.546054' }], //初始化经纬度、默认深圳
        clickLngAndLat = []; //保存坐标的数组

    //坐标拾取
    $("#GetCoordinate").click(function () {

        lay = $.layer({
            type: 1,
            title: false,
            closeBtn: false,//控制层右上角关闭按钮。closeBtn的值分别为: [关闭按钮的风格（支持0和1）, true]若不想显示关闭按钮，配置 closeBtn: false即可
            border: [5, 0.5, '#666', true],
            offset: ['0px', ''],
            move: ['.xubox_title', false],
            area: ['860px', '500px'],
            page: {
                dom: "#baidumap"
            },
            success: function () {
                layer.shift('top', 500);
            }
        });


        var x = $.trim($("#X").val());
        var y = $.trim($("#Y").val());
        if (x != null && x != "" && y != null && y != "") {
            clickLngAndLat.push({ Lng: x, Lat: y });
        }

        map = new BMap.Map("container"); //加载地图
        map.clearOverlays(); //清除标注
        var navigationControl = new BMap.NavigationControl({
            // 靠左上角位置
            anchor: BMAP_ANCHOR_TOP_LEFT,
            // LARGE类型
            type: BMAP_NAVIGATION_CONTROL_LARGE
        });
        map.addControl(navigationControl);
        map.addControl(new BMap.MapTypeControl()); //添加地图类型控件

        var point;
        if (clickLngAndLat.length > 0) { //如果是编辑状态，则加载已有坐标。否，加载默认坐标
            point = new BMap.Point(clickLngAndLat[0].Lng, clickLngAndLat[0].Lat);
        } else {
            point = new BMap.Point(initLngAndLat[0].Lng, initLngAndLat[0].Lat);
        }

        var marker = new BMap.Marker(point); // 创建标注
        map.addOverlay(marker); // 将标注添加到地图中
        map.centerAndZoom(point, 15);
        map.enableScrollWheelZoom();

        map.addEventListener("click", function (e) {
            map.setDefaultCursor("pointer"); //设置鼠标手势
            clickLngAndLat = [];
            map.clearOverlays(); //清除除标注
            var jsonStr = { Lng: e.point.lng, Lat: e.point.lat };
            clickLngAndLat.push(jsonStr);
            point = new BMap.Point(clickLngAndLat[0].Lng, clickLngAndLat[0].Lat);
            //map.centerAndZoom(point, 18);
            //map.enableScrollWheelZoom();
            $("#txtlatitude").val("").val(e.point.lat);
            $("#txtlongitude").val("").val(e.point.lng);
            var marker = new BMap.Marker(point); // 创建标注
            map.addOverlay(marker); // 将标注添加到地图中
            //marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画

        });

        if (clickLngAndLat.length > 0) {
            $("#txtlatitude").val("").val(clickLngAndLat[0].Lat);
            $("#txtlongitude").val("").val(clickLngAndLat[0].Lng);
        }

    });

    //坐标拾取--确定
    $("#btn_xy").click(function () {
        if (clickLngAndLat != null && clickLngAndLat.length > 0) {
            $("#X").val("").val(clickLngAndLat[0].Lng);
            $("#Y").val("").val(clickLngAndLat[0].Lat);
            map.clearOverlays();
        }
        layer.close(lay);
    });

    //坐标拾取--取消
    $("#btnClrearCallout").click(function () {
        clickLngAndLat = [];
        map.clearOverlays();
        layer.close(lay);
    });

    //搜索
    $("#areaSearch").click(function () {
        // 创建地址解析器实例 
        var myGeo = new BMap.Geocoder();
        var searchTxt = $("#txtarea").val();
        if ($.trim(searchTxt) == null || $.trim(searchTxt) == "") {
            layer.alert("请输入搜索条件");
            return false;
        }
        // 将地址解析结果显示在地图上，并调整地图视野 
        myGeo.getPoint(searchTxt, function (point) {
            if (point) {
                map.centerAndZoom(point, 15);
                $("#txtlatitude").val('').val(point.lat);
                $("#txtlongitude").val('').val(point.lng);
                //var pointMarker = new BMap.Point(point.lng, point.lat);
                //map.addOverlay(new BMap.Marker(point));
            } else
                layer.alert("搜索不到结果");
        }, "全国");
    });
}
