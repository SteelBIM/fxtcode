$(function () {
    var x = $("#X").val(), y = $("#Y").val(), LngOrLat = $("#LngOrLat").val();
    //初始化经纬度、默认深圳
    var InitialLng_Lat = [{ Lng: cityX, Lat: cityY }];
    //初始化三维城市、默认深圳
    var currentCity = "深圳";
    //保存坐标的数组
    var ClikcLng_Lat = [];
    //保存地图标注的数组
    var MapLineLng_Lat = [];
    //地图对象
    //var map;
    //坐标点对象
    //var point;

    //坐标拾取
    $("#GetCoordinate").click(function () {

        var clonum = $.layer({
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

        var currPage = "GetCoordinate";
        $(".GetCoordinate").show();
        $(".MapCallout").hide();
        x = $.trim($("#X").val());
        y = $.trim($("#Y").val());
        if (x != null && x != "" && y != null && y != "") {
            ClikcLng_Lat.push({ Lng: x, Lat: y });
        }

        var map = new BMap.Map("container");
        map.clearOverlays();//清除除标注
        var navigationControl = new BMap.NavigationControl({
            // 靠左上角位置
            anchor: BMAP_ANCHOR_TOP_LEFT,
            // LARGE类型
            type: BMAP_NAVIGATION_CONTROL_LARGE
        });
        map.addControl(navigationControl);
        map.addControl(new BMap.MapTypeControl());

        var point;
        if (ClikcLng_Lat != null && ClikcLng_Lat.length > 0) {
            point = new BMap.Point(ClikcLng_Lat[0].Lng, ClikcLng_Lat[0].Lat);
        } else {
            point = new BMap.Point(InitialLng_Lat[0].Lng, InitialLng_Lat[0].Lat);
        }


        map.centerAndZoom(point, 15);
        map.enableScrollWheelZoom();
        var marker = new BMap.Marker(point);  // 创建标注
        map.addOverlay(marker);              // 将标注添加到地图中
        map.setCurrentCity(currentCity);

        map.addEventListener("click", function (e) {
            map.setDefaultCursor("pointer");//设置鼠标手势
            ClikcLng_Lat = [];
            map.clearOverlays();//清除除标注
            var json_str = { Lng: e.point.lng, Lat: e.point.lat };
            ClikcLng_Lat.push(json_str);
            point = new BMap.Point(ClikcLng_Lat[0].Lng, ClikcLng_Lat[0].Lat);
            //map.centerAndZoom(point, 15);
            //map.enableScrollWheelZoom();
            $("#txtlatitude").val("").val(e.point.lat);
            $("#txtlongitude").val("").val(e.point.lng);
            marker = new BMap.Marker(point);  // 创建标注
            map.addOverlay(marker);              // 将标注添加到地图中
            //marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画


        });

        if (ClikcLng_Lat.length > 0) {
            $("#txtlatitude").val("").val(ClikcLng_Lat[0].Lat);
            $("#txtlongitude").val("").val(ClikcLng_Lat[0].Lng);
        }
        
        //坐标拾取--确定
        $("#btn_xy").click(function () {
            if (ClikcLng_Lat != null && ClikcLng_Lat.length > 0) {
                $("#X").val("").val(ClikcLng_Lat[0].Lng);
                $("#Y").val("").val(ClikcLng_Lat[0].Lat);
                map.clearOverlays();
            }
            layer.close(clonum);
        });
        //坐标拾取--取消
        $("#btnClrearCallout").click(function () {
            ClikcLng_Lat = [];
            map.clearOverlays();
            layer.close(clonum);
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
                    map.clearOverlays();
                    map.centerAndZoom(point, 15);
                    if (currPage == "GetCoordinate") {
                        $("#txtlatitude").val('').val(point.lat);
                        $("#txtlongitude").val('').val(point.lng);
                    }
                    point = new BMap.Point(point.lng, point.lat); // 创建标注的坐标
                    marker = new BMap.Marker(point);  // 创建标注
                    map.addOverlay(marker);
                    map.setCurrentCity(searchTxt);
                }
                else {
                    layer.alert("搜索不到结果");
                    return false;
                }
            }, "全国");

        });

    });

    //地图标注
    $("#MapCallout").click(function () {

        var clonum = $.layer({
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

        // currPage = "MapCallout";
        $(".GetCoordinate").hide();
        $(".MapCallout").show();

        var map = new BMap.Map("container");
        map.clearOverlays();//清除除标注
        var navigationControl = new BMap.NavigationControl({
            // 靠左上角位置
            anchor: BMAP_ANCHOR_TOP_LEFT,
            // LARGE类型
            type: BMAP_NAVIGATION_CONTROL_LARGE
        });
        map.addControl(navigationControl);
        map.addControl(new BMap.MapTypeControl());


        LngOrLat = $("#LngOrLat").val();

        if (LngOrLat != null && LngOrLat != "") {
            MapLineLng_Lat = [];
            if (LngOrLat.indexOf("|") > 0) {
                var str_arr = LngOrLat.split('|');
                for (var i = 0; i < str_arr.length; i++) {
                    MapLineLng_Lat.push({ Lng: str_arr[i].split(',')[0], Lat: str_arr[i].split(',')[1] });
                }
            } else {
                if (LngOrLat.indexOf(",") > 0) {
                    MapLineLng_Lat.push({ Lng: LngOrLat.split(',')[0], Lat: LngOrLat.split(',')[1] });
                }
            }
            for (var i = 0; i < MapLineLng_Lat.length; i++) {

                var point = new BMap.Point(MapLineLng_Lat[i].Lng, MapLineLng_Lat[i].Lat);
                map.centerAndZoom(point, 15);
                map.enableScrollWheelZoom();
                var marker = new BMap.Marker(point);  // 创建标注
                map.addOverlay(marker);              // 将标注添加到地图中
            }
            PolygonLine();//画线
        } else {
            var point = new BMap.Point(InitialLng_Lat[0].Lng, InitialLng_Lat[0].Lat);
            map.centerAndZoom(point, 15);
            map.enableScrollWheelZoom();
        }

        map.addEventListener("click", function (e) {
            map.setDefaultCursor("pointer");//设置鼠标手势
            ClikcLng_Lat = [];

            var json_str = { Lng: e.point.lng, Lat: e.point.lat };
            ClikcLng_Lat.push(json_str);//保存当前点
            MapLineLng_Lat.push(json_str);//保存搜有点
            point = new BMap.Point(ClikcLng_Lat[0].Lng, ClikcLng_Lat[0].Lat);
            marker = new BMap.Marker(point);  // 创建标注
            map.addOverlay(marker);              // 将标注添加到地图中
            
        });



        //地图标注--画线
        $("#btnPaintline").click(function () {
            PolygonLine();
        });
        //地图标注--重置
        $("#btnlinereset").click(function () {
            //保存地图标注的数组
            MapLineLng_Lat = [];
            map.clearOverlays();//清除除标注
        });
        $("#btnlineok").click(function () {
            if (MapLineLng_Lat != null && MapLineLng_Lat.length > 0) {
                var str = "";
                for (var j = 0; j < MapLineLng_Lat.length; j++) {
                    str += MapLineLng_Lat[j].Lng + "," + MapLineLng_Lat[j].Lat + "|"
                }
                if (str.length > 0) {
                    str = str.substring(0, str.length - 1);
                }
                $("#LngOrLat").val("").val(str);
                map.clearOverlays();
            }
            MapLineLng_Lat = [];
            layer.close(clonum);
        });
        $("#btnClrearLine").click(function () {

            MapLineLng_Lat = [];
            map.clearOverlays();//清除除标注
            layer.close(clonum);
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
                    map.clearOverlays();
                    map.centerAndZoom(point, 15);
                    if (currPage == "GetCoordinate") {
                        $("#txtlatitude").val('').val(point.lat);
                        $("#txtlongitude").val('').val(point.lng);
                    }
                    point = new BMap.Point(point.lng, point.lat); // 创建标注的坐标
                    marker = new BMap.Marker(point);  // 创建标注
                    map.addOverlay(marker);
                }
                else {
                    layer.alert("搜索不到结果");
                    return false;
                }
            }, "全国");

        });

        //地图标注--画多边形
        function PolygonLine() {
            if (MapLineLng_Lat != null && MapLineLng_Lat.length > 0) {
                var line = [];
                for (var i = 0; i < MapLineLng_Lat.length; i++) {
                    line.push(new BMap.Point(MapLineLng_Lat[i].Lng, MapLineLng_Lat[i].Lat));
                }
                if (line != null && line.length > 0) {
                    //创建多边形  
                    var curve = new BMap.Polygon(line, { strokeColor: "blue", strokeWeight: 3, strokeOpacity: 0.5 });
                    map.addOverlay(curve); //添加到地图中
                    //curve.enableEditing(); //开启编辑功能
                }
            }
        }


    });



});