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
    BindBuildingInfo();
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
    /***绑定建筑结构***/
    var nowStructureCode = $("#hdStructureCode").val() * 1;
    for (var i = 0; i < structureCodeListJson.length; i++) {
        var isSelect = "";
        var obj = structureCodeListJson[i];
        if (nowStructureCode == obj.code) {
            isSelect = "selected=\"selected\"";
        }
        $("#txt_structurecode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    }
    /***绑定位置***/
    var nowLocationCode = $("#hdLocationCode").val() * 1;
    for (var i = 0; i < locationCodeListJson.length; i++) {
        var isSelect = "";
        var obj = locationCodeListJson[i];
        if (nowLocationCode == obj.code) {
            isSelect = "selected=\"selected\"";
        }
        $("#txt_locationcode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    }
    /***绑定景观***/
    var nowSightCode = $("#hdSightCode").val() * 1;
    for (var i = 0; i < sightCodeListJson.length; i++) {
        var isSelect = "";
        var obj = sightCodeListJson[i];
        if (nowSightCode == obj.code) {
            isSelect = "selected=\"selected\"";
        }
        $("#txt_sightcode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    }
}

/*********提交数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();
    var buildingname = $("#txt_buildingname").val();
    var averageprice = $("#txt_averageprice").val();/*楼栋均价*/
    var totalfloor = $("#txt_totalfloor").val();/*总层数*/
    var purposecode = $("#txt_purposecode").val();/*楼栋用途*/
    var iselevator = $("#txt_iselevator").val();/*是否带电梯*/
    var maintenancecode = $("#txt_maintenancecode").val();/*是否带电梯*/

    if (!VeriDecimal(averageprice)) {
        VerifyErrorStyle($("#txt_averageprice"));
        alert("楼栋均价格式填写错误");
        return false;
    }
    if (!VeriInt(totalfloor)) {
        VerifyErrorStyle($("#txt_totalfloor"));
        alert("总层数必须为整数");
        return false;
    }
    if (totalfloor == "" || totalfloor == null) {
        VerifyErrorStyle($("#txt_totalfloor"));
        alert("总层数不能为空");
        return false;
    }
    if (buildingname == "" || buildingname == null) {
        VerifyErrorStyle($("#txt_buildingname"));
        alert("楼栋名不能为空");
        return false;
    }
    if (purposecode == 0) {
        VerifyErrorStyle($("#txt_purposecode"));
        alert("请选择楼栋用途");
        return false;
    }
    if (iselevator == "" || iselevator == null) {
        VerifyErrorStyle($("#txt_iselevator"));
        alert("请选择是否带电梯");
        return false;
    }
    if (maintenancecode == 0) {
        VerifyErrorStyle($("#txt_maintenancecode"));
        alert("请选择维护情况");
        return false;
    }


    /**********楼盘信息************/
    var projectColDom = $("#SubmitPanel").find(".datbuilding");
    /**楼盘信息**/
    var builJson = "{";
    for (var i = 0; i < projectColDom.length; i++) {
        var key = $(projectColDom.get(i)).attr("id");
        key = key.replace("txt_", "");
        var value = $(projectColDom.get(i)).val();
        builJson = builJson + "\"" + key + "\":\"" + value + "\","
    }
    builJson = builJson.TrimEnd(',') + "}";
    var paraJson = { projectId: NowProjectId, buildingId: NowBuildingId, cityId: NowCityId, buildingJson: builJson };
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/EditBuilding_SubmitData_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (data != null) {
                        if (data.result != 1) {

                            if (data.errorType == 0) {
                                alert(decodeURIComponent(data.message));
                            }
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
                window.location.href = "/AllotFlowInfo/EditBuilding?allotId=" + NowAllotId + "&buildingId=" + NowBuildingId + "&type=1";
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