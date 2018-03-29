var NowCityId = 0;
var NowProvinceId = 0;
var NowIsUpdateRight = 0;
var houseX = 114.045082;
var houseY = 22.559325;
var userX = 114.045082;
var userY = 22.559325;
var map = null;
var NowProjectId = 0;
var NowAllotId = 0;
$(function () {
    NowCityId = $("#hdCityId").val() * 1;
    NowProvinceId = $("#hdProvinceId").val() * 1;
    NowIsUpdateRight = $("#hdIsUpdateRight").val() * 1;
    houseX = $("#hdHouseX").val() * 1;
    houseY = $("#hdHouseY").val() * 1;
    userX = $("#hdUserX").val() * 1;
    userY = $("#hdUserY").val() * 1;
    NowProjectId = $("#hdProjectId").val();
    NowAllotId = $("#hdAllotId").val();
    if (houseX > 0 && houseY > 0) {
        $("#dingwei").html("已定位");
        $("#btnlocation").hide();
        $("#btnSavelocation").hide();
    }
    else {
        $("#dingwei").html("未定位");
    }
    //BindMap();/**初始化绑定地图对象***/
    //HousePosition();/***默认绑定物业位置***/
    $("#btnUserPoint").bind("click", function () {
        $("#MapPointBtn").find(".mapBtn").removeClass("btn-white");
        $("#MapPointBtn").find(".mapBtn").addClass("btn-white");
        $(this).removeClass("btn-white");
        UserPosition();
    });
    $("#btnHousePoint").bind("click", function () {
        $("#MapPointBtn").find(".mapBtn").removeClass("btn-white");
        $("#MapPointBtn").find(".mapBtn").addClass("btn-white");
        $(this).removeClass("btn-white");
        HousePosition();
    });
    $("#searchAroundMap").change(function () {
        var searchAround = $("#searchAroundMap").find("option:selected").val();
        SearchAroundPosition(searchAround);
    });
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });
    BindProjectInfo();
    /**点击小图显示大图**/
    $("a.projPhoto").fancybox({
        'opacity': true,
        'overlayShow': false,
        'transitionIn': 'elastic',
        'transitionOut': 'none'
    });

    //行政区联动片区
    $("#txt_areaid").change(function () {
        select_subarea();
    });
    $("#dropdown_map").click(function () {
        setTimeout(function () {
            BindMap();/**初始化绑定地图对象***/
        }, 200);
    });
    $("#btnlocation").click(function () {
        Projectlocation();
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

    //坐标拾取--确定
    $("#btnSavelocation").click(function () {
        if (lng != null && lng > 0) {
            $.post("/AllotFlowInfo/UploadProjectXY", { projectId: NowProjectId, x: lng, y: lat }, function (data) {
                if (data.result) {
                    AlertFancybox2("定位成功", function () {
                        $("#btnlocation").hide();
                        $("#btnSavelocation").hide();
                        $("#dingwei").html("已定位");
                    });
                } else {
                    alert(data.message);
                }
            })
        }
    });
}

/****初始化绑定地图对象****/
function BindMap() {
    //$("#dropdown14").addClass("active");
    if (map == null) {
        map = new BMap.Map("mapContent");/***创建地图实例  ***/
        map.enableDragging(); //启用地图拖拽事件，默认启用(可不写)
        map.enableScrollWheelZoom(); //启用地图滚轮放大缩小            
        map.disableDoubleClickZoom();//禁用鼠标双击放大
        map.enableKeyboard(); //启用键盘上下左右键移动地图 
        /**向地图中添加缩放控件**/
        var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
        map.addControl(ctrl_nav);
        /**向地图中添加缩略图控件**/
        var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
        map.addControl(ctrl_ove);
        /**向地图中添加比例尺控件**/
        var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
        map.addControl(ctrl_sca);

        if (!(houseX > 0 && houseY > 0)) {
            function myFun(result) {
                //lng = result.center.lng;
                //lat = result.center.lat;
                var point = new BMap.Point(result.center.lng, result.center.lat);
                map.centerAndZoom(point, 17);
            }
            var myCity = new BMap.LocalCity();
            myCity.get(myFun);
        } else {
            HousePosition();/***默认绑定物业位置***/
        }
    }
    //$("#dropdown14").removeClass("active");
}
/***定位物业位置****/
function HousePosition() {
    if (!(houseX > 0 && houseY > 0)) {
        //map.clearOverlays();
        return;
    }
    /** 创建点坐标  **/
    var point = new BMap.Point(houseX, houseY);
    map.centerAndZoom(point, 17);
    /**"物业位置"图片***/
    var myIcon = new BMap.Icon("http://api.map.baidu.com/images/marker_red_sprite.png", new BMap.Size(150, 80), {
        anchor: new BMap.Size(0, 0)
    });
    marker = new BMap.Marker(point);
    //marker = new BMap.Marker(point, { icon: myIcon });
    map.addOverlay(marker);

    //setTimeout(function () {
    //    map.setCenter(point);
    //}, 200);
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
function BindProjectInfo() {
    /****绑定城市名称*****/
    //var cityName = GetCityNameByProvinceIdAndCityId(NowProvinceId, NowCityId);
    //$("#txt_cityid").val(cityName);
    /*****绑定企业*********/
    $("#SubmitPanel").find(".txt_pc").each(function () {
        var code = $(this).attr("data-code");
        var codeDom = $("#codeLnkPCList").find(".code_" + code);
        if (codeDom.length > 0) {
            $(this).val(codeDom.html());
        }
    });
    /*****绑定配套*********/
    $("#SubmitPanel").find(".txt_ap").each(function () {
        var code = $(this).attr("data-code");
        var codeDom = $("#codeLnkPAList").find(".code_" + code);
        if (codeDom.length > 0) {
            $(this).val(codeDom.html());
        }
        var classCode = codeDom.attr("data-class");
        var selectDom = GetClassCodeSelect(classCode, code);
        $(this).after(selectDom);
    });
    /*****绑定照片类型名称*******/
    for (var i = 0; i < photoTypeCodeListJson.length; i++) {
        var codeObj = photoTypeCodeListJson[i];
        $("#projPhotoList").find(".photoTypeName_" + codeObj.code).html(codeObj.codename);
    }

}
function GetCityNameByProvinceIdAndCityId(provinceId, cityId) {
    var cityName = "";
    for (var i = 0; i < province_json_city.length; i++) {
        var obj = province_json_city[i];
        if (obj.provinceid == provinceId * 1) {
            var citylist = obj.citylist;
            for (var j = 0; j < citylist.length; j++) {
                var obj2 = citylist[j];
                if (cityId == obj2.cityid) {
                    cityName = obj2.cityname;
                    return cityName;
                }
            }
            return cityName;
        }
    }
    return cityName;
}

function GetClassCodeSelect(nowClass, nowCode) {
    var selecthtml = "<select id=\"classcode_" + nowCode + "\"><option value=\"0\" class=\"statusInfo statusInfo_0\" >等级</option>";
    for (var i = 0; i < classCodeListJson.length; i++) {
        var obj = classCodeListJson[i];
        var select = "";
        var nowId = nowClass
        if (nowId == obj.code) {
            select = "selected=\"selected\"";
        }
        var ophtml = "<option value=\"" + obj.code + "\" class=\"statusInfo statusInfo_" + obj.code + "\" " + select + ">" + obj.codename + "</option>";
        selecthtml = selecthtml + ophtml;
    }
    selecthtml = selecthtml + "</select>";
    return selecthtml;
}


/*********提交数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();
    var projectname = $("#txt_projectname").val();
    var address = $("#txt_address").val();
    var areaid = $("#txt_areaid").val();
    //var buildingarea = $("#txt_buildingarea").val();/*建筑面积*/
    //var landarea = $("#txt_landarea").val();/*占地面积*/
    //var cubagerate = $("#txt_cubagerate").val();/*容积率*/
    //var greenrate = $("#txt_greenrate").val();/*绿化率*/
    //var managerprice = $("#txt_managerprice").val();/*物业管理费*/
    var parkingnumber = $("#txt_parkingnumber").val();/*车位数*/
    var totalnum = $("#txt_totalnum").val();/*总户数*/
    var buildingnum = $("#txt_buildingnum").val();/*总栋数*/
    var purposecode = $("#txt_purposecode").val();/*主用途*/
    var rightcode = $("#txt_rightcode").val();/*主用途*/
    //if (!VeriDecimal(buildingarea)) {
    //    VerifyErrorStyle($("#txt_buildingarea"));
    //    alert("建筑面积格式填写错误");
    //    return false;
    //}
    //if (!VeriDecimal(landarea)) {
    //    VerifyErrorStyle($("#txt_landarea"));
    //    alert("占地面积格式写填错误");
    //    return false;
    //}
    //if (!VeriDecimal(cubagerate)) {
    //    VerifyErrorStyle($("#txt_cubagerate"));
    //    alert("容积率格式写填错误");
    //    return false;
    //}
    //if (!VeriDecimal(greenrate)) {
    //    VerifyErrorStyle($("#txt_greenrate"));
    //    alert("绿化率格式写填错误");
    //    return false;
    //}
    //if (!VeriDecimal(managerprice)) {
    //    VerifyErrorStyle($("#txt_managerprice"));
    //    alert("物业管理费格式写填错误");
    //    return false;
    //}
    if (!VeriInt(parkingnumber)) {
        VerifyErrorStyle($("#txt_parkingnumber"));
        alert("车位数必须为整数");
        return false;
    }
    if (!VeriInt(totalnum)) {
        VerifyErrorStyle($("#txt_totalnum"));
        alert("总户数必须为整数");
        return false;
    }
    if (buildingnum == "" || buildingnum == null) {
        VerifyErrorStyle($("#txt_buildingnum"));
        alert("总栋数不能为空");
        return false;
    }
    if (!VeriInt(buildingnum)) {
        VerifyErrorStyle($("#txt_buildingnum"));
        alert("总栋数必须为整数");
        return false;
    }
    if (projectname == "" || projectname == null) {
        VerifyErrorStyle($("#txt_projectname"));
        alert("楼盘名不能为空");
        return false;
    }
    if (areaid == "" || areaid == "0") {
        VerifyErrorStyle($("#txt_areaid"));
        alert("请选择行政区");
        return false;
    }
    if (address == "" || address == null) {
        VerifyErrorStyle($("#txt_address"));
        alert("地址不能为空");
        return false;
    }
    if (purposecode == 0) {
        VerifyErrorStyle($("#txt_purposecode"));
        alert("请选择主用途");
        return false;
    }
    if (rightcode == 0) {
        VerifyErrorStyle($("#txt_rightcode"));
        alert("请选择产权形式");
        return false;
    }

    /**********楼盘信息************/
    var projectColDom = $("#SubmitPanel").find(".datproject");
    /**楼盘信息**/
    var projJson = "{";
    for (var i = 0; i < projectColDom.length; i++) {
        var key = $(projectColDom.get(i)).attr("id");
        key = key.replace("txt_", "");
        var value = $(projectColDom.get(i)).val();
        projJson = projJson + "\"" + key + "\":\"" + value + "\","
    }
    projJson = projJson.TrimEnd(',') + "}";
    /**配套信息**/
    var lnkpaJson = "[";
    $("#SubmitPanel").find(".txt_ap").each(function () {
        var p_aname = $(this).val();
        var p_code = $(this).attr("data-code");
        var p_class = $("#classcode_" + p_code).val();
        lnkpaJson = lnkpaJson + "{\"appendagecode\":" + p_code + ",\"p_aname\":\"" + encodeURIComponent(p_aname) + "\",\"classcode\":" + (p_class == "0" ? "null" : p_class) + "},";
    });
    lnkpaJson = lnkpaJson.TrimEnd(',') + "]";
    /**关联公司信息**/
    var developersCompany = encodeURIComponent($("#txt_developerscompany").val());
    var managerCompany = encodeURIComponent($("#txt_managercompany").val());
    var paraJson = { projectId: NowProjectId, cityId: NowCityId, projectJson: projJson, lnkpaListJson: lnkpaJson, developersCompany: developersCompany, managerCompany: managerCompany };
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/EditProject_SubmitData_Api",
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
                            AlertFancybox2("修改成功", function () {
                                parent.$.fancybox.close();
                            });
                            //parent.location.reload(true);
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



//片区下拉列表
function select_subarea() {
    var areaid = $("#txt_areaid").val();
    $.get('/AllotFlowInfo/GetSubAreaSelect', { areaid: areaid }, function (data) {
        $("#txt_subareaid").val(null).trigger("change");
        $("#txt_subareaid").html("");
        $.each(data, function (i, item) {
            if (item.Selected) {
                $("#txt_subareaid").append($("<option selected=\"selected\"></option>").val(item.Value).html(item.Text));
            } else {
                $("#txt_subareaid").append($("<option></option>").val(item.Value).html(item.Text));
            }
        });
    });
}

/***上传楼盘图片****/
function UploadProjectImage(projectid) {
    var url = "/AllotFlowInfo/UploadProjectImage?projectId=" + projectid;
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
            window.location.href = "/AllotFlowInfo/EditProject?allotId=" + NowAllotId + "&projectId=" + projectid + "&type=1";
        }
    });
}
/***删除楼盘图片****/
function DeleteProjectImage(projectid) {
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
                    window.location.href = "/AllotFlowInfo/EditProject?allotId=" + NowAllotId + "&projectId=" + projectid + "&type=1";
                } else {

                }
            });
        }
    });
}