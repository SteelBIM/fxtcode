var NowCityId = 0;
var NowBuildingId = 0;
var NowProjectId = 0;
var NowAllotId = 0;
var map = null;
$(function () {
    NowAllotId = $("#hdAllotId").val();
    NowCityId = $("#hdCityId").val() * 1;
    NowBuildingId = $("#hdBuildingId").val();
    NowProjectId = $("#hdProjectId").val();
    //BindBuildingInfo();
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });

    houseX = $("#hdHouseX").val() * 1;
    houseY = $("#hdHouseY").val() * 1;
    if (houseX > 0 && houseY > 0) {
        $("#dingwei").html("已定位");
        $("#btnlocation").hide();
    }
    else {
        $("#dingwei").html("未定位");
    }
    //BindMap();/**初始化绑定地图对象***/
    //HousePosition();/***默认绑定物业位置***/
    /*****绑定照片类型名称*******/
    for (var i = 0; i < photoTypeCodeListJson.length; i++) {
        var codeObj = photoTypeCodeListJson[i];
        $("#projPhotoList").find(".photoTypeName_" + codeObj.code).html(codeObj.codename);
    }
    $("#searchAroundMap").change(function () {
        var searchAround = $("#searchAroundMap").find("option:selected").val();
        SearchAroundPosition(searchAround);
    });
    /**点击小图显示大图**/
    $("a.projPhoto").fancybox({
        'opacity': true,
        'overlayShow': false,
        'transitionIn': 'elastic',
        'transitionOut': 'none'
    });
    $("#dropdown_map").die();
    $("#dropdown_map").click(function () {
        if (NowBuildingId < 1) {
            alert("请先保存楼栋数据！");
            return false;
        }
        setTimeout(function () {
            BindMap();/**初始化绑定地图对象***/
        }, 200);
    });
    $("#dropdown_photo").click(function () {
        if (NowBuildingId < 1) {
            alert("请先保存楼栋数据！");
            return false;
        }
    });

    $("#btnlocation").click(function () {
        Projectlocation();
    });

    //坐标拾取--确定
    $("#btnSavelocation").click(function () {
        if (lng != null && lng > 0) {
            $.post("/AllotFlowInfo/UploadBuildingXY", { buildingId: NowBuildingId, x: lng, y: lat }, function (data) {
                if (data.result) {
                    AlertFancybox2("定位成功", function () {
                        $("#btnlocation").hide();
                        $("#dingwei").html("已定位");
                    });
                } else {
                    alert(data.message);
                }
            })
        }
    });
});

var lng;
var lat
/***开始定位****/
function Projectlocation() {
    function myFun(result) {
        lng = result.center.lng;
        lat = result.center.lat;
        var point = new BMap.Point(result.center.lng, result.center.lat);
        //map.centerAndZoom(point, 17);
        marker = new BMap.Marker(point);
        map.addOverlay(marker);
    }
    if (!lng) {
        var myCity = new BMap.LocalCity();
        myCity.get(myFun);
    }

    map.addEventListener("click", function (e) {
        map.setDefaultCursor("pointer"); //设置鼠标手势
        map.clearOverlays(); //清除除标注
        lng = e.point.lng;
        lat = e.point.lat;
        point = new BMap.Point(lng, lat);
        var marker = new BMap.Marker(point); // 创建标注
        map.addOverlay(marker); // 将标注添加到地图中
    });
}

function BindBuildingInfo() {
    ///***绑定建筑结构***/
    //var nowStructureCode = $("#hdStructureCode").val() * 1;
    //for (var i = 0; i < structureCodeListJson.length; i++) {
    //    var isSelect = "";
    //    var obj = structureCodeListJson[i];
    //    if (nowStructureCode == obj.code) {
    //        isSelect = "selected=\"selected\"";
    //    }
    //    $("#txt_structurecode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    //}
    ///***绑定位置***/
    //var nowLocationCode = $("#hdLocationCode").val() * 1;
    //for (var i = 0; i < locationCodeListJson.length; i++) {
    //    var isSelect = "";
    //    var obj = locationCodeListJson[i];
    //    if (nowLocationCode == obj.code) {
    //        isSelect = "selected=\"selected\"";
    //    }
    //    $("#txt_locationcode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    //}
    /***绑定景观***/
    //var nowSightCode = $("#hdSightCode").val() * 1;
    //for (var i = 0; i < sightCodeListJson.length; i++) {
    //    var isSelect = "";
    //    var obj = sightCodeListJson[i];
    //    if (nowSightCode == obj.code) {
    //        isSelect = "selected=\"selected\"";
    //    }
    //    $("#txt_sightcode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    //}
}

/*********提交数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();


    var buildingname = $("#BuildingName");
    if (buildingname) {
        if (buildingname.val() == "" || buildingname.val() == null) {
            VerifyErrorStyle($("#BuildingName"));
            alert("楼栋名不能为空");
            return false;
        }
    }

    /*总层数*/
    var totalfloor = $("#TotalFloor");
    if (totalfloor) {
        if (!VeriInt(totalfloor.val())) {
            VerifyErrorStyle($("#TotalFloor"));
            alert("总层数必须为整数");
            return false;
        }
        if (totalfloor.val() == "" || totalfloor.val() == null) {
            VerifyErrorStyle($("#TotalFloor"));
            alert("总层数不能为空");
            return false;
        }
    }

    /*楼栋用途*/
    var purposecode = $("#PurposeCode");
    if (purposecode.val()) {
        if (purposecode.val() == 0) {
            VerifyErrorStyle($("#PurposeCode"));
            alert("请选择楼栋用途");
            return false;
        }
    }

    /*是否带电梯*/
    var iselevator = $("#IsElevator");
    if (iselevator) {
        if (iselevator.val() == "" || iselevator.val() == null) {
            VerifyErrorStyle($("#IsElevator"));
            alert("请选择是否带电梯");
            return false;
        }
    }

    /*维护情况*/
    var maintenancecode = $("#MaintenanceCode");
    if (maintenancecode) {
        if (maintenancecode.val() == 0) {
            VerifyErrorStyle($("#MaintenanceCode"));
            alert("请选择维护情况");
            return false;
        }
    }

    /*楼栋均价*/
    var averageprice = $("#averageprice");
    if (averageprice) {
        if (!VeriDecimal(averageprice.val())) {
            VerifyErrorStyle(averageprice);
            alert("楼栋均价格式填写错误");
            return false;
        }
    }

    /*层高*/
    var FloorHigh = $("#FloorHigh");
    if (FloorHigh) {
        if (!VeriDecimal(FloorHigh.val())) {
            VerifyErrorStyle(FloorHigh);
            alert("层高格式填写错误");
            return false;
        }
    }

    /*单元数*/
    var UnitsNumber = $("#UnitsNumber");
    if (UnitsNumber) {
        if (!VeriInt(UnitsNumber.val())) {
            VerifyErrorStyle(UnitsNumber);
            alert("单元数必须为整数");
            return false;
        }
    }

    /*总户数*/
    var TotalNumber = $("#TotalNumber");
    if (TotalNumber) {
        if (!VeriInt(TotalNumber.val())) {
            VerifyErrorStyle(TotalNumber);
            alert("总户数必须为整数");
            return false;
        }
    }

    /*建筑面积*/
    var TotalBuildArea = $("#TotalBuildArea");
    if (TotalBuildArea) {
        if (!VeriDecimal(TotalBuildArea.val())) {
            VerifyErrorStyle(TotalBuildArea);
            alert("建筑面积格式填写错误");
            return false;
        }
    }

    /*销售均价*/
    var AveragePrice = $("#AveragePrice");
    if (AveragePrice) {
        if (!VeriDecimal(AveragePrice.val())) {
            VerifyErrorStyle(AveragePrice);
            alert("销售均价格式填写错误");
            return false;
        }
    }

    /*均价层*/
    var AverageFloor = $("#AverageFloor");
    if (AverageFloor) {
        if (!VeriDecimal(AverageFloor.val())) {
            VerifyErrorStyle(AverageFloor);
            alert("均价层格式填写错误");
            return false;
        }
    }

    /*附属房屋均价*/
    var SubAveragePrice = $("#SubAveragePrice");
    if (SubAveragePrice) {
        if (!VeriDecimal(SubAveragePrice.val())) {
            VerifyErrorStyle(SubAveragePrice);
            alert("附属房屋均价格式填写错误");
            return false;
        }
    }

    /*地下层数*/
    var Basement = $("#Basement");
    if (Basement) {
        if (!VeriInt(Basement.val())) {
            VerifyErrorStyle(Basement);
            alert("地下层数必须为整数");
            return false;
        }
    }

    /*裙楼层数*/
    var PodiumBuildingFloor = $("#PodiumBuildingFloor");
    if (PodiumBuildingFloor) {
        if (!VeriInt(PodiumBuildingFloor.val())) {
            VerifyErrorStyle(PodiumBuildingFloor);
            alert("裙楼层数必须为整数");
            return false;
        }
    }

    /*裙楼面积*/
    var PodiumBuildingArea = $("#PodiumBuildingArea");
    if (PodiumBuildingArea) {
        if (!VeriDecimal(PodiumBuildingArea.val())) {
            VerifyErrorStyle(PodiumBuildingArea);
            alert("裙楼面积格式填写错误");
            return false;
        }
    }

    /*塔楼面积*/
    var TowerBuildingArea = $("#TowerBuildingArea");
    if (TowerBuildingArea) {
        if (!VeriDecimal(TowerBuildingArea.val())) {
            VerifyErrorStyle(TowerBuildingArea);
            alert("塔楼面积格式填写错误");
            return false;
        }
    }

    /*地下室总面积*/
    var BasementArea = $("#BasementArea");
    if (BasementArea) {
        if (!VeriDecimal(BasementArea.val())) {
            VerifyErrorStyle(BasementArea);
            alert("地下室总面积格式填写错误");
            return false;
        }
    }

    /*住宅套数*/
    var HouseNumber = $("#HouseNumber");
    if (HouseNumber) {
        if (!VeriInt(HouseNumber.val())) {
            VerifyErrorStyle(HouseNumber);
            alert("住宅套数必须为整数");
            return false;
        }
    }

    /*住宅总面积*/
    var HouseArea = $("#HouseArea");
    if (HouseArea) {
        if (!VeriDecimal(HouseArea.val())) {
            VerifyErrorStyle(HouseArea);
            alert("住宅总面积格式填写错误");
            return false;
        }
    }

    /*非住宅套数*/
    var OtherNumber = $("#OtherNumber");
    if (OtherNumber) {
        if (!VeriInt(OtherNumber.val())) {
            VerifyErrorStyle(OtherNumber);
            alert("非住宅套数必须为整数");
            return false;
        }
    }

    /*非住宅总面积*/
    var OtherArea = $("#OtherArea");
    if (OtherArea) {
        if (!VeriDecimal(OtherArea.val())) {
            VerifyErrorStyle(OtherArea);
            alert("非住宅总面积积格式填写错误");
            return false;
        }
    }

    /*单层户数*/
    var FloorHouseNumber = $("#FloorHouseNumber");
    if (FloorHouseNumber) {
        if (!VeriInt(FloorHouseNumber.val())) {
            VerifyErrorStyle(FloorHouseNumber);
            alert("单层户数必须为整数");
            return false;
        }
    }

    /*电梯数量*/
    var LiftNumber = $("#LiftNumber");
    if (LiftNumber) {
        if (!VeriInt(LiftNumber.val())) {
            VerifyErrorStyle(LiftNumber);
            alert("电梯数量必须为整数");
            return false;
        }
    }

    /*楼间距*/
    var Distance = $("#Distance");
    if (Distance) {
        if (!VeriInt(Distance.val())) {
            VerifyErrorStyle(Distance);
            alert("楼间距必须为整数");
            return false;
        }
    }

    /*价格系数*/
    var Weight = $("#Weight");
    if (Weight) {
        if (!VeriDecimal(Weight.val())) {
            VerifyErrorStyle(Weight);
            alert("价格系数格式填写错误");
            return false;
        }
    }

    /**********楼盘信息************/

    var paraJson = $("form").serializeArray();
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/EditBuilding_SubmitData_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (!data.result) {

                        alert(decodeURIComponent(data.message));
                        return;
                    }
                    else {
                        if (NowBuildingId > 1) {
                            AlertFancybox2("修改成功", function () {
                                parent.OpenBuildingInfo_Response(data.detail, NowBuildingId);
                                parent.$.fancybox.close();
                            });
                            parent.location.reload(true);
                        } else {
                            AlertFancybox2("新增成功", function () {
                                NowBuildingId = parseInt(data.detail);
                            });
                        }
                    }
                },
               { dom: "#SubmitPanel" });
}
function VerifyErrorStyle(dom) {
    $(dom).css("borderColor", "red").removeClass("vererror").addClass("vererror");
}
function VerifyErrorStyleRemove() {
    $("#SubmitPanel").find(".vererror").css("borderColor", "#B5B5B5");
}
/*****验证带小数类型数据格式******/
function VeriDecimal(val) {
    if (val != null && val != "") {
        var patrn = /^-?\d+\.{0,}\d{0,}$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}
/*****验证整数数据格式******/
function VeriInt(val) {
    if (val != null && val != "") {
        var patrn = /^\d+$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}

/****初始化绑定地图对象****/
function BindMap() {
    //$("#dropdown14").addClass("active");
    if (map == null) {
        map = new BMap.Map("mapContent");/***创建地图实例  ***/
        //map.enableDragging(); //启用地图拖拽事件，默认启用(可不写)
        map.enableScrollWheelZoom(); //启用地图滚轮放大缩小            
        //map.disableDoubleClickZoom();//禁用鼠标双击放大
        //map.enableKeyboard(); //启用键盘上下左右键移动地图 
        /**向地图中添加缩放控件**/
        var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
        map.addControl(ctrl_nav);
        ///**向地图中添加缩略图控件**/
        //var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
        //map.addControl(ctrl_ove);
        ///**向地图中添加比例尺控件**/
        //var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
        //map.addControl(ctrl_sca);

        if (!(houseX > 0 && houseY > 0)) {
            function myFun(result) {
                var point = new BMap.Point(result.center.lng, result.center.lat);
                map.centerAndZoom(point, 17);
            }
            var myCity = new BMap.LocalCity();
            myCity.get(myFun);
        } else {
            HousePosition();/***默认绑定物业位置***/

            map.addEventListener("click", function (e) {
                map.setDefaultCursor("pointer"); //设置鼠标手势
                map.clearOverlays(); //清除除标注
                lng = e.point.lng;
                lat = e.point.lat;
                point = new BMap.Point(lng, lat);
                var marker = new BMap.Marker(point); // 创建标注
                map.addOverlay(marker); // 将标注添加到地图中
            });
        }
    }
    //$("#dropdown14").removeClass("active");
}
/***定位物业位置****/
function HousePosition() {
    if (!(houseX > 0 && houseY > 0)) {
        map.clearOverlays();
        return;
    }
    /** 创建点坐标  **/
    var point = new BMap.Point(houseX, houseY);
    map.centerAndZoom(point, 17);
    /**"物业位置"图片***/
    var myIcon = new BMap.Icon("http://www.yungujia.com/v3/Images/housePosition.png", new BMap.Size(150, 80), {
        anchor: new BMap.Size(0, 0)
    });
    marker = new BMap.Marker(point);
    //marker = new BMap.Marker(point, { icon: myIcon });
    map.addOverlay(marker);
}
/****定位周边位置****/
function SearchAroundPosition(searchAround) {
    map.clearOverlays();
    if (!(houseX > 0 && houseY > 0)) {
        map.clearOverlays();
        return;
    }
    /** 创建点坐标  **/
    var point = new BMap.Point(houseX, houseY);
    map.centerAndZoom(point, 17);
    var local = new BMap.LocalSearch(map, {
        renderOptions: { map: map, autoViewport: true }
    });
    local.searchInBounds(searchAround, map.getBounds());
}
/****定位查勘员位置*****/
function UserPosition() {
    if (!(userX > 0 && userY > 0)) {
        map.clearOverlays();
        return;
    }
    /** 创建点坐标  **/
    var point = new BMap.Point(userX, userY);
    map.centerAndZoom(point, 15);
    map.panTo(point);
    /**查勘员范围定位**/
    var circle = new BMap.Circle(point, 300, { fillOpacity: 0.3 });
    map.addOverlay(circle);
}

/***上传楼栋图片****/
function UploadBuildingImage(projectid, buildingid) {
    var url = "/AllotFlowInfo/UploadBuildingImage?buildingId=" + NowBuildingId + "&projectId=" + projectid;
    $.fancybox({
        'href': url,
        'width': 750,
        'height': 450,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
            window.location.href = "/AllotFlowInfo/EditBuilding?allotId=" + NowAllotId + "&id=" + NowBuildingId + "&type=1";
        }
    });
}

/***删除楼盘图片****/
function DeleteBuildingImage(projectid, buildingid) {
    var ids = [];
    var checks = $("input[name='ids']:checked");
    if (checks.length == 0) {
        alert("请选择要删除的图片！");
        return;
    }
    checks.each(function () {
        ids.push($(this).val());
    });

    var url = "/AllotFlowInfo/DeleteProjectImage";
    DA_Confirm("提示", "确定要删除选中的图片吗？", function (ok) {
        if (ok) {
            $.post(url, { ids: ids }, function (data) {
                if (data.result) {
                    alert(data.message);
                    window.location.href = "/AllotFlowInfo/EditBuilding?allotId=" + NowAllotId + "&buildingId=" + buildingid + "&type=1";
                } else {

                }
            });
        }
    });
}