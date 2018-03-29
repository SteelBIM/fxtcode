/*
    tb_arr:选项数组
    add_arr：用于新增记录保存的object
    update_arr:用于修改记录保存的object
    delete_arr:用于删除记录保存的object
    updatefloornum_arr:用于修改实际层保存的array
*/
var tb_arr = [
        "HouseName", "PurposeName", "SJPurposeName", "BuildingArea",
        "InnerBuildingArea", "FrontName", "SightName", "UnitPrice", "Weight", "IsEValue"
],
    add_arr = {},
    update_arr = {},
    delete_arr = {},
    updatefloornum_arr = [];
//HouseModel
var HouseAttr = {
    HouseId: 0,             //房号ID
    BuildingId: "",         //楼栋ID
    ProjectId: "",           //楼盘ID
    CityId: "",             //城市ID
    FloorNo: "",            //物理层
    FloorNum: "",           //实际层   
    UnitNo: "",             //室号
    HouseName: "",          //房号名称
    PurposeCode: "",        //证载用途
    SJPurposeCode: "",      //实际用途
    BuildingArea: "",       //建筑面积
    InnerBuildingArea: "",  //套内面积  
    FrontCode: "",          //朝向
    SightCode: "",          //景观
    UnitPrice: "",          //单价
    Weight: "",             //价格系数
    IsEValue: "",        //是否可估
    FxtCompanyId: ""          //评估机构ID

};
var HouseObj = {
    ProjectId: "",            //楼盘ID
    CityID: "",               //城市ID
    BuildingId: "",           //楼栋ID
    FxtCompanyId: "",         //评估机构Id
    TotalFloor: 0,            //总楼层
    AveragePrice: "",         //楼栋均价
    PAveragePrice: "",        //楼盘均价
    PriceDetail: "",          //价格系数说明
    ProWeight: "",            //百分比
    FloorPrjceCha: 0,         //楼层价差 
    UnitNoPriceCha: 0,        //单元价差   
    //加载初始化
    InitLoad: function (projectId, buildingId, fxtCompanyId, totalFloor, pAveragePrice, averagePrice, priceDetail, proWeight) {
        this.ProjectId = projectId;
        this.BuildingId = buildingId;//楼栋Id
        this.TotalFloor = totalFloor;
        this.PAveragePrice = pAveragePrice;
        this.AveragePrice = averagePrice;
        this.PriceDetail = priceDetail;
        this.ProWeight = proWeight;
        this.FxtCompanyId = fxtCompanyId;
    },

    /*
     修改实际层
     src:物理层
     dest:实际层
    */
    ModifyFloorNum: function (src, dest) {
        if (src == "" || dest == "") {
            alert("物理层和实际层都不能为空！");
            return;
        }
        var reg = new RegExp("^[0-9]*$");
        if (!reg.test(src)) {
            alert("物理层必须是数字类型！");
            return;
        }
        if (src > this.TotalFloor || src <= 0) {
            alert("不存在该楼层！");
            return;
        }

        var isDeleted = false;//标识当前要修改的楼层是否是已删除的楼层
        $("#sample_editable_1 tbody").each(function () {
            $(this).children("tr").each(function () {
                var $firstTd = $(this).children("td:first");
                var floorNo = $firstTd.text();

                if ($.trim(floorNo) == src) {
                    if ($firstTd.next().next() != null) {
                        if ($firstTd.next().next().text() == "") {
                            alert("该楼层已删除");
                            isDeleted = true;
                            return false;
                        }
                    }

                    $firstTd.next().text(dest);
                    return false;
                }
            });

            if (isDeleted) {
                return false;
            }

        });

        updatefloornum_arr.push({ floorNo: src, floorNum: dest });

    },

    /*
     添加楼层
     floorNum:楼层号
    */
    AddFloorNo: function (floorNum) {
        if (ValidFloorNum(floorNum)) {
            var num = parseInt(floorNum);
            if (num < 1)//地下层待实现
            {
                return false;
            }
            if (num <= this.TotalFloor) {
                alert("楼层号已经存在");
                return false;
            }
            FoeachAddFloorNo(floorNum, this.TotalFloor);//添加楼层、面积、用途。。。。。。
            this.TotalFloor = floorNum;//重置总楼层数
        }
    },
    /*
     删除楼层
     floorNum:楼层号
   */
    DelFloorNo: function (floorNum) {
        if (ValidFloorNum(floorNum)) {
            var num = parseInt(floorNum);
            if (num < 1) {
                return false;//地下层  待实现
            }
            if (num > this.TotalFloor) {
                alert("不存在该楼层号");
                return false;
            } else {
                FoeachDelFloorNo(floorNum, this.TotalFloor);//删除楼层、面积、用途。。。。。。
                this.TotalFloor = floorNum;//重置总楼层数
            }
        }
    },
    /*
     新增房号;
     unitNo:单元;A
     houseName:室号;01
     房号=单元+楼层+室号(A101,A201,A301)
    */
    AddHouse: function (unitNo, houseName) {
        FoeachAddHouse(unitNo, houseName);//添加房号
    },
    /* 
     删除房号;
     unitNo:单元;A
     houseName:室号;01
     */
    DelHouse: function (unitNo, houseName) {
        FoeachDelHouse(unitNo, houseName);//删除房号
    },
    /*
      设置
      floor:从多少层
      floorTo:到多少层
      unitNo:从多少单元
      unitNoTo:到多少单元
      setValue:值
      tableModule:table模块
    */
    CommonSetValue: function (floor, floorTo, unitNo, unitNoTo, setValue, tableModule) {
        if ($.trim(setValue) == null || $.trim(setValue) == "") {
            alert("不能输入空值");
            return false;
        }
        if (tableModule == "#tab_BuildingArea" || tableModule == "#tab_InnerBuildingArea" || tableModule == "#tab_UnitPrice" || tableModule == "#tab_Weight") {
            if (isNaN(setValue)) {
                alert("请输入数字");
                return false;
            }
        }
        //比较floor和floorTo大小、unitNo和unitNoTo并且互换
        var tempfloor, tempunitNo;//中间变量
        if (GetInt32(floor) > GetInt32(floorTo)) {
            tempfloor = GetInt32(floor);
            floor = GetInt32(floorTo);
            floorTo = tempfloor;
        }
        if (GetInt32(unitNo) > GetInt32(unitNoTo)) {
            tempunitNo = GetInt32(unitNo);
            unitNo = GetInt32(unitNoTo);
            unitNoTo = tempunitNo;
        }
        FoeachCommonSetValue(floor, floorTo, unitNo, unitNoTo, setValue, tableModule);
    },
    /*
        设置楼层均价
        avgpriceFloor:要设置均价的楼层层
    */
    SetFloorAvgPrice: function (avgpriceFloor) {
        if (this.AveragePrice == null || this.AveragePrice == "") {
            alert("楼栋均价不正确，请先到“价格系数”页设置项目均价和楼栋均价!");
            return false;
        }
        var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取table
        thead = GetTableHead(tobj),//获取table的单元列(包含列:物理层和实际层这两列)
        htobj = GetCurrTableByTId("#tab_HouseName");//获取房号table项
        for (var i = 2; i < $(thead).length; i++) {//i=2(物理层和实际层除外)
            var houName = $.trim($(htobj).find("tbody>tr").eq(avgpriceFloor - 1).find("td").eq(i).text());
            if (houName != null && houName != "") {
                $(tobj).find("tbody>tr").eq(avgpriceFloor - 1).find("td").eq(i).text(this.AveragePrice);
                SaveUpdate(houName, tobj, avgpriceFloor, i, "#tab_UnitPrice", this.AveragePrice);
            }
        }
    },
    /*
       设置楼层差
      floor:从多少层
      floorTo:到多少层
      jiajian:1：递增;0：递减
      val：1：递增;0：递减[的百分比]
   */
    SetFloorCha: function (floor, floorTo, jiajian, val) {
        SetFloorCha(floor, floorTo, jiajian, val);
    },
    /*
       设置单元差
      floor:从多少单元
      floorTo:到多少单元
      jiajian:1：递增;0：递减
      val：1：加;0：减[的百分比]
   */
    SetUnitnoCha: function (unitNo, unitNoTo, jiajian, val) {
        SetUnitNoCha(unitNo, unitNoTo, jiajian, val);
    },
    //设置价格系数
    SetPriceWeight: function () {
        SetPriceWeight();
    },
    Submit: function (thisobj) {
        $(thisobj).attr({ "disabled": "disabled" });
        var dataparam = {};
        /*
        参数模板
        {"add":[{"BuildingId":"123"},{"BuildingId":"A"}],
           "update":[{"BuildingId":"111"},{"BuildingId":"B"}],
           "delete":[{"BuildingId":"8"},{"BuildingId":"C"}]
        };
        */
        if ($.isEmptyObject(add_arr) && $.isEmptyObject(update_arr) && $.isEmptyObject(delete_arr) && updatefloornum_arr.length == 0) {
            //alert("尚无数据提交");
            $(thisobj).removeAttr("disabled");
            return false;
        }
        if (!$.isEmptyObject(add_arr)) {
            dataparam.AddHouse = JsonData(add_arr, 1);
        };
        if (!$.isEmptyObject(update_arr)) {
            dataparam.UpdateHouse = JsonData(update_arr, 1);
        }
        if (!$.isEmptyObject(delete_arr)) {
            dataparam.DeleteHouse = JsonData(delete_arr, 0);
        }
        if (updatefloornum_arr.length > 0) {
            dataparam.updatefloornum = updatefloornum_arr;
        }
        $.ajax({
            url: "/Office/OfficeHouse/Save",
            data: JSON.stringify(dataparam),
            dataType: "Json",
            contentType: "application/json",
            type: "POST",
            cache: false,
            success: function (jsondata) {
                window.location.href = "/Office/OfficeHouse/Index?buildingId=" + HouseObj.BuildingId + "&projectId=" + HouseObj.ProjectId;
            },
            error: function (jsondata) {
                $(thisobj).removeAttr("disabled");
            }
        });
    }
}
//json处理
function JsonData(obj, valid) {
    var jsonstr = [];
    for (var key in obj) {
        var json = {
            "BuildingId": obj[key]["BuildingId"],
            "HouseId": obj[key]["HouseId"],
            "HouseName": obj[key]["HouseName"],
            "BuildingArea": obj[key]["BuildingArea"],
            "InnerBuildingArea": obj[key]["InnerBuildingArea"],
            "PurposeCode": obj[key]["PurposeCode"],
            "SJPurposeCode": obj[key]["SJPurposeCode"],
            "UnitPrice": obj[key]["UnitPrice"],
            "Weight": obj[key]["Weight"],
            "FrontCode": obj[key]["FrontCode"],
            "SightCode": obj[key]["SightCode"],
            "IsEValue": obj[key]["IsEValue"],
            "FloorNo": obj[key]["FloorNo"],
            "UnitNo": obj[key]["UnitNo"],
            "HouseNo": obj[key]["HouseNo"],
            "CityID": obj[key]["CityID"],
            "ProjectId": obj[key]["ProjectId"],
            "Valid": valid,
            "FxtCompanyId": obj[key]["FxtCompanyId"],
            "FloorNum": obj[key]["FloorNum"]
        };
        jsonstr.push(json);
    }
    return jsonstr;
}
//设置价格系数
function SetPriceWeight() {

    if (window.confirm("价格系数是由房号的单价与项目均价求得的，请确认房号单价和楼盘均价设置无误，是否继续？")) {
        var projectAvgPrice = $.trim(HouseObj.PAveragePrice);
        if (projectAvgPrice == null || projectAvgPrice == "") {
            alert("项目均价不能为空");
            return false;
        }
        if (parseInt(projectAvgPrice) == 0) {
            alert("项目均价不能零");
            return false;
        }
        var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取初始单价tableItem
    hobj = GetCurrTableByTId("#tab_HouseName"),//获取房号tableItem
    weithobj = GetCurrTableByTId("#tab_Weight"),//获取房号tableItem
thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
        for (var i = 0; i < HouseObj.TotalFloor; i++) {
            for (var j = 2; j < $(thead).length; j++) {
                var houName = $.trim($(hobj).find("tbody>tr").eq(i).find("td").eq(j).text()),//房号
                 price = $.trim($(tobj).find("tbody>tr").eq(i).find("td").eq(j).text());//单价
                if ((houName != null && houName != "") && (price != null && price != "")) {
                    var weight = $.trim(parseFloat(parseFloat(price) / parseFloat(projectAvgPrice)).toFixed(4));
                    $(weithobj).find("tbody>tr").eq(i).find("td").eq(j).text(weight);
                    SaveUpdate(houName, tobj, (i + 1), j, "#tab_Weight", weight);
                }
            }
        }

    }
}
//设置单元差
function SetUnitNoCha(unitNo, unitNoTo, jiajian, val) {
    if (val == null || val == "" || val == 0) {
        alert("百分比不能为空");
        return false;
    }
    if (isNaN(val)) {
        alert("百分比只能是数字");
        return false;
    }
    val = $.trim(val); unitNo = GetInt32(unitNo); unitNoTo = GetInt32(unitNoTo);
    var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取初始单价tableItem
        hobj = GetCurrTableByTId("#tab_HouseName"),//获取房号tableItem
    thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    for (var i = 0; i < HouseObj.TotalFloor; i++) {
        if (unitNo > unitNoTo) {
            for (var j = unitNo; j >= unitNoTo; j--) {
                UnitNoPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i);
            }
        } else {
            for (var j = unitNo; j <= unitNoTo; j++) {
                UnitNoPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i);
            }
        }
        HouseObj.UnitNoPriceCha = 0;
    }
}
//设置单元差
function UnitNoPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i) {
    var price = $.trim($(tobj).find("tbody>tr").eq(i).find("td").eq(j).text()),
           houName = $.trim($(hobj).find("tbody>tr").eq(i).find("td").eq(j).text());
    if (price != null && price != "")//第floor(新增范围的首层)或者房号为空
    {
        if (HouseObj.UnitNoPriceCha <= 0)//第一次赋值
        {
            HouseObj.UnitNoPriceCha = price;
        }
    }
    if (HouseObj.UnitNoPriceCha > 0) {
        if (jiajian == 0) {
            price = (parseFloat(HouseObj.UnitNoPriceCha) - (parseFloat(HouseObj.UnitNoPriceCha) * (val / 100))).toFixed(2);
            //price = (parseFloat(HouseObj.UnitNoPriceCha) - 100).toFixed(2);
        } else if (jiajian == 1) {
            price = (parseFloat(HouseObj.UnitNoPriceCha) + (parseFloat(HouseObj.UnitNoPriceCha) * (val / 100))).toFixed(2);
            //price = (parseFloat(HouseObj.UnitNoPriceCha) + 100).toFixed(2);
        }
        if (houName != null && houName != "") {
            $(tobj).find("tbody>tr").eq(i).find("td").eq(j).text(price);
            SaveUpdate(houName, tobj, (i + 1), j, "#tab_UnitPrice", price);
        }
        HouseObj.UnitNoPriceCha = price;
    }
}
//设置楼层差
function SetFloorCha(floor, floorTo, jiajian, val) {
    if (val == null || val == "" || val == 0) {
        alert("百分比不能为空");
        return false;
    }
    if (isNaN(val)) {
        alert("百分比只能是数字");
        return false;
    }
    val = $.trim(val); floor = GetInt32(floor); floorTo = GetInt32(floorTo);
    var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取初始单价tableItem
        hobj = GetCurrTableByTId("#tab_HouseName"),//获取房号tableItem
    thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    for (var i = 2; i < $(thead).length; i++) {
        if (floor > floorTo) {
            for (var j = floor; j >= floorTo; j--) {
                FloorPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i);
            }
        } else {
            for (var j = floor; j <= floorTo; j++) {
                FloorPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i);
            }
        }
        HouseObj.FloorPrjceCha = 0;
    }
}
//房号差递增或者递减
function FloorPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i) {
    var price = $.trim($(tobj).find("tbody>tr").eq(j - 1).find("td").eq(i).text()),
        houName = $.trim($(hobj).find("tbody>tr").eq(j - 1).find("td").eq(i).text());
    if (price != null && price != "")//第floor(新增范围的首层)或者房号为空
    {
        if (HouseObj.FloorPrjceCha <= 0)//第一次赋值
        {
            HouseObj.FloorPrjceCha = price;
        }
    }
    if (HouseObj.FloorPrjceCha > 0) {
        if (jiajian == 0) {
            price = (parseFloat(HouseObj.FloorPrjceCha) - (parseFloat(HouseObj.FloorPrjceCha) * (val / 100))).toFixed(2);
            //price = (parseFloat(HouseObj.FloorPrjceCha) - 100).toFixed(2);
        } else if (jiajian == 1) {
            price = (parseFloat(HouseObj.FloorPrjceCha) + (parseFloat(HouseObj.FloorPrjceCha) * (val / 100))).toFixed(2);
            //price = (parseFloat(HouseObj.FloorPrjceCha) + 100).toFixed(2);
        }
        if (houName != null && houName != "") {
            $(tobj).find("tbody>tr").eq(j - 1).find("td").eq(i).text(price);
            SaveUpdate(houName, tobj, j, i, "#tab_UnitPrice", price);
        }
        HouseObj.FloorPrjceCha = price;
    }
}
//验证楼层
function ValidFloorNum(floorNum) {
    floorNum = $.trim(floorNum);
    if (floorNum == null || floorNum == "") {
        alert("楼层号不能为空");
        return false;
    }
    if (isNaN(floorNum)) {
        alert("楼层号只能是数字");
        return false;
    }
    return true;
}
//验证单元号是否存在z
//function ValidFloorUnitNo(unitNo) {
//    var tobj = GetCurrTableByTId("#tab_" + tb_arr[0]);//获取table
//    var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
//    var flag = true;
//    for (var i = 2; i < $(thead).length; i++) {
//        if (unitNo.toUpperCase() == $.trim($(thead[i]).text()).toUpperCase())//转换小写
//        {
//            flag = false;
//            break;
//        }
//    }
//    if (!flag) {
//        return false;
//    } else {
//        return flag;
//    }

//}
function ValidFloorUnitNo(unitNo, houseNo) {
    var tobj = GetCurrTableByTId("#tab_" + tb_arr[0]);//获取table
    var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    var flag = true;
    for (var i = 2; i < $(thead).length; i++) {
        var unitno = unitNo == "" ? "" : unitNo.toUpperCase() + "-";
        var houseno = houseNo.toUpperCase();
        if (unitno + houseno == $.trim($(thead[i]).text()).toUpperCase())//转换大写
        {
            flag = false;
            break;
        }
    }
    if (!flag) {
        return false;
    } else {
        return flag;
    }
}
//根据单元和室号获取th的索引
function GetThIndex(unitNo, houseNo) {
    var tobj = GetCurrTableByTId("#tab_" + tb_arr[0]);//获取table
    var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    var index = 0;
    for (var i = 2; i < $(thead).length; i++) {
        var unitno = unitNo == "" ? "" : unitNo.toUpperCase() + "-";
        var houseno = houseNo.toUpperCase();
        if (unitno + houseno == $.trim($(thead[i]).text()).toUpperCase()) {
            index = i;
            break;
        }
    }
    return index;

}
/*
Operation:循环添加楼层
attr:房号、面积、用途、附属房屋类型、附属房屋面积、户型、
     户型结构、初始单价、价格系数、朝向、景观、可估价、
     面积确认、通风采光、装修、是否有厨房、阳台数、洗手间数
*/
function FoeachAddFloorNo(floorNum, totalFloor) {
    for (var a = 0; a < tb_arr.length; a++) {
        var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
        var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
        for (var i = 1 ; i <= GetInt32(floorNum) - GetInt32(totalFloor) ; i++) {
            var tr = "<tr>";
            for (var j = 0; j < $(thead).length; j++) {
                if (j == 0 || j == 1) {
                    tr += "<td>";
                    tr += GetInt32(totalFloor) + i;
                    tr += "</td>";
                } else {
                    var col = $.trim((thead).eq(j).text()).split('-');
                    var colUnitNo = col.length > 1 ? $.trim(col[0]) : "";
                    var colHouseNo = col.length > 1 ? $.trim(col[1]) : $.trim(col[0]);
                    var houName = colUnitNo == "" ? (GetInt32(totalFloor) + i) + colHouseNo : colUnitNo + (GetInt32(totalFloor) + i) + colHouseNo;

                    tr += "<td>";
                    if (a == 0) {//房号项
                        tr += houName;//房号=单元+楼层+室号(A101,A201,A301)
                        /*
                     创建一个房号
                     begin
                 */
                        HouseAttr = {
                            FloorNo: $.trim(GetInt32(totalFloor) + i),      //物理楼层
                            FloorNum: $.trim(GetInt32(totalFloor) + i), //名义层（实际层）
                            HouseName: houName,                     //房号名称
                            BuildingId: HouseObj.BuildingId,        //楼栋ID
                            CityID: HouseObj.CityID,                //城市ID
                            FxtCompanyId: HouseObj.FxtCompanyId,    //评估机构ID
                            ProjectId: HouseObj.ProjectId,    //楼盘ID
                            BuildingArea: "",                          //面积
                            InnerBuildingArea: "",
                            PurposeCode: "",                        //用途
                            SJPurposeCode: "",
                            UnitPrice: "",                          //初始单价
                            Weight: "",                             //价格系数
                            FrontCode: "",                          //朝向  
                            SightCode: "",                          //景观
                            IsEValue: "",                           //是否可估
                            UnitNo: colUnitNo,                      //单元 
                            HouseNo: colHouseNo,                    //室号
                            Valid: 1                                //是否有效
                        };
                        /*
                            创建一个房号
                            end
                        */
                        add_arr[houName] = HouseAttr;//添加到数据字典
                    } else {
                        tr += "";
                    }
                    tr += "</td>";
                }
            }
            tr += "/</tr>";
            $(tobj).find("tbody").append(tr);
        }
    }
}
/*
Operation:循环删除楼层
attr:房号、面积、用途、附属房屋类型、附属房屋面积、户型、
     户型结构、初始单价、价格系数、朝向、景观、可估价、
     面积确认、通风采光、装修、是否有厨房、阳台数、洗手间数
*/
function FoeachDelFloorNo(floorNum, totalFloor) {
    for (var a = 0; a < tb_arr.length; a++) {
        var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
        var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
        for (var j = 2; j < $(thead).length; j++) {
            var tr = $(tobj).find("tbody>tr").eq(GetInt32(floorNum - 1));
            if (a == 0)//房号项
            {
                var houName = $.trim(tr.find("td").eq(j).text());//房号名称
                if (add_arr != null && add_arr[houName] != undefined) {
                    delete add_arr[houName];//如果是尚未提交的新增房号直接移除
                } else {
                    if (houName != null && houName != "") {//房号不能为空
                        /*
                             创建一个房号
                             begin
                         */

                        HouseAttr = {
                            HouseId: $.trim(tr.find("td").eq(j).attr("houseid")),
                            FloorNo: GetInt32(floorNum),                //物理楼层
                            FloorNum: $.trim(tr.find("td").eq(1).text()),   //名义层（实际层）
                            HouseName: houName,                         //房号名称
                            BuildingId: HouseObj.BuildingId,            //楼栋ID
                            CityID: HouseObj.CityID,                    //城市ID
                            ProjectId: HouseObj.ProjectId,                    //楼盘ID
                            FxtCompanyId: $.trim(tr.find("td").eq(j).attr("fxtcompanyid"))         //当前房号的fxtcompanyid
                        };
                        /*
                           创建一个房号
                           end
                       */
                        delete_arr[houName] = HouseAttr;//已经存在的房号
                    }
                }
            }
            tr.find("td").eq(j).text("");
        }
    }
}
/*
Operation:循环添加房号
*/
function FoeachAddHouse(unitNo, houseNo) {
    if (houseNo == "") {
        alert("室号不能为空");
        return false;
    }
    if (ValidFloorUnitNo(unitNo, houseNo)) {

        unitNo = unitNo.indexOf("-") >= 0 ? unitNo.substring(0, unitNo.length - 1) : unitNo;
        var column = unitNo == "" ? houseNo : unitNo + "-" + houseNo;

        for (var a = 0; a < tb_arr.length; a++) {
            var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
            $(tobj).find("thead>tr").append("<th>" + column + "</th>");//添加单元
            tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//重新获单元信息
            //var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
            for (var i = 0; i < HouseObj.TotalFloor; i++) {
                var tr = "";
                if (a == 0) {//房号项 房号=单元+楼层+室号(A101,A201,A301)
                    var houName = unitNo + (i + 1) + houseNo;
                    tr += "<td>";
                    tr += houName;
                    tr += "</td>";
                    /*
                        新增房号
                        begin
                    */
                    HouseAttr = {
                        FloorNo: $.trim(i + 1),                 //物理楼层
                        FloorNum: $.trim(i + 1),                //名义层（实际层）
                        HouseName: houName,                     //房号名称
                        BuildingId: HouseObj.BuildingId,        //楼栋ID
                        CityID: HouseObj.CityID,                //城市ID
                        FxtCompanyId: HouseObj.FxtCompanyId,    //评估机构ID
                        ProjectId: HouseObj.ProjectId,    //楼盘ID
                        BuildingArea: "",                       //面积
                        InnerBuildingArea: "",
                        PurposeCode: "",                        //用途
                        SJPurposeCode: "",
                        UnitPrice: "",                          //初始单价
                        Weight: "",                             //价格系数
                        FrontCode: "",                          //朝向  
                        SightCode: "",                          //景观
                        IsEValue: "",                           //是否可估
                        UnitNo: $.trim(unitNo),                  //单元 
                        HouseNo: $.trim(houseNo),                 //室号
                        Valid: 1                                //是否有效
                    };
                    /*
                        新增房号
                        end
                    */
                    add_arr[houName] = HouseAttr;//保存到数据字典
                } else {
                    tr += "<td></td>";
                }
                $(tobj).find("tbody tr").eq(i).append(tr);
            }
        }
    } else {
        alert("已经存在相同的单元");
        return false;
    }
}
/*
Operation:循环删除房号
*/
function FoeachDelHouse(unitNo, houseName) {
    //if (unitNo == null || unitNo == "") {
    //    alert("单元不能为空");
    //    return false;
    //}
    if (ValidFloorUnitNo(unitNo, houseName)) {
        alert("该单元不存在");
        return false;
    } else {
        var thIndex = GetThIndex(unitNo, houseName);
        for (var a = 0; a < tb_arr.length; a++) {
            var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
            for (var i = 0; i < HouseObj.TotalFloor; i++) {
                if (a == 0)//房号项
                {
                    var houName = $.trim($(tobj).find("tbody>tr").eq(i).find("td").eq(thIndex).text());
                    if (add_arr != null && add_arr[houName] != undefined) {
                        delete add_arr[houName];//如果是尚未提交的新增房号直接移除
                    } else {
                        if (houName != null && houName != "") {//房号不能为空
                            /*
                        创建一个房号
                        begin
                    */
                            HouseAttr = {
                                HouseId: $.trim($(tobj).find("tbody>tr").eq(i).find("td").eq(thIndex).attr("houseid")),
                                FloorNo: GetInt32(i + 1),                     //物理楼层
                                FloorNum: $.trim($.trim(unitNo) + $.trim(houseName)),                 //名义层（实际层）
                                HouseName: houName,                         //房号名称
                                BuildingId: HouseObj.BuildingId,            //楼栋ID
                                CityID: HouseObj.CityID,                    //城市ID
                                ProjectId: HouseObj.ProjectId,                    //楼盘ID
                                FxtCompanyId: $.trim($(tobj).find("tbody>tr").eq(i).find("td").eq(thIndex).attr("fxtcompanyid"))         //当前房号的fxtcompanyid
                            };
                            /*
                                创建一个房号
                                end
                            */

                            delete_arr[houName] = HouseAttr;//已经存在的房号
                        }
                    }
                }
                $(tobj).find("tbody>tr").eq(i).find("td").eq(thIndex).remove();
            }
            $(tobj).find("thead>tr>th").eq(thIndex).remove();

        }
    }
}
/*
Operation:设置
面积、用途、附属房屋类型、附属房屋面积、
户型、户型结构、初始单价、价格系数、朝向、
景观、可估价、面积确认、通风采光、装修、是否有厨房、阳台数、洗手间数
*/
function FoeachCommonSetValue(floor, floorTo, unitNo, unitNoTo, newVal, tableModule) {
    var tobj = GetCurrTableByTId(tableModule);//获取table
    var houtable = GetCurrTableByTId("#tab_HouseName");//获取房号Item
    for (var i = GetInt32(floor) ; i <= GetInt32(floorTo) ; i++) {
        for (var j = GetInt32(unitNo) ; j <= GetInt32(unitNoTo) ; j++) {
            var houseName = $.trim($(houtable).find("tbody>tr").eq(i - 1).find("td").eq(j).text());//房号名称
            if (houseName != null && houseName != "") {//存在该房号
                $(tobj).find("tbody>tr").eq(i - 1).find("td").eq(j).text(newVal);
                SaveUpdate(houseName, tobj, i, j, tableModule, $.trim(newVal));
            }
        }
    }
}
//获取table(取得一个包含匹配的元素集合中每一个元素紧邻的后面同辈元素的元素集合)
function GetCurrTableByTId(id) {
    return $(id).next("table");
}
//获取table的单元列(包含列:物理层和实际层这两列)
function GetTableHead(table) {
    return $(table).find("thead>tr>th");
}
//转换成整数
function GetInt32(obj) {
    try {
        return parseInt(obj);
    } catch (e) {
        alert(e.message);
    }
    return 0;
}
//去除空格
function GetTrim(obj) {
    return $.trim(obj);
}
/*
    保存更新
    houseName:房号
    tobj：当前item的tableId
    i：楼层
    j：单元(室号)
    tableModule:当前模块项
    newVal:设置的值
*/
function SaveUpdate(houseName, tobj, i, j, tableModule, newVal) {
    var ba = GetCurrTableByTId("#tab_BuildingArea"),
        pn = GetCurrTableByTId("#tab_PurposeName"),
        up = GetCurrTableByTId("#tab_UnitPrice"),
        w = GetCurrTableByTId("#tab_Weight"),
        fn = GetCurrTableByTId("#tab_FrontName"),
        sn = GetCurrTableByTId("#tab_SightName"),
        ie = GetCurrTableByTId("#tab_IsEValue"),
        iba = GetCurrTableByTId("#tab_InnerBuildingArea"),
        sj = GetCurrTableByTId("#tab_SJPurposeName");


    if (add_arr[houseName] != null || add_arr[houseName] != undefined) {
        AddOrUpdate(add_arr[houseName], tableModule, newVal);
        delete update_arr[houseName];
    } else {
        if (update_arr[houseName] == null || update_arr[houseName] == undefined) {
            var col = $(tobj).find("thead>tr").eq(0).find("th").eq(j).text().split('-');
            HouseAttr = {
                FloorNo: i,//物理楼层
                FloorNum: $.trim($(pn).find("tbody>tr").eq(i - 1).find("td").eq(1).text()),//实际层
                UnitNo: col.length > 1 ? $.trim(col[0]) : "",//单元
                HouseNo: col.length > 1 ? $.trim(col[1]) : $.trim(col[0]),//室号
                HouseId: $.trim($(ba).find("tbody>tr").eq(i - 1).find("td").eq(j).attr("houseid")),
                HouseName: houseName,                   //房号名称
                BuildingId: HouseObj.BuildingId,        //楼栋ID
                CityID: HouseObj.CityID,                //城市ID
                FxtCompanyId: $.trim($(ba).find("tbody>tr").eq(i - 1).find("td").eq(j).attr("fxtcompanyid")),//当前房号的评估机构ID
                BuildingArea: $.trim($(ba).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //面积
                InnerBuildingArea: $.trim($(iba).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //套内面积
                PurposeCode: $.trim($(pn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),        //用途
                SJPurposeCode: $.trim($(sj).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),        //用途
                UnitPrice: $.trim($(up).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //初始单价
                Weight: $.trim($(w).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),             //价格系数
                FrontCode: $.trim($(fn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //朝向  
                SightCode: $.trim($(sn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //景观
                IsEValue: $.trim($(ie).find("tbody>tr").eq(i - 1).find("td").eq(j).text())         //是否可估

            };
            update_arr[houseName] = HouseAttr;
        }
        AddOrUpdate(update_arr[houseName], tableModule, newVal);
    }
}

/*
    更新
*/
function AddOrUpdate(obj, tableModule, newVal) {
    if (tableModule == "#tab_BuildingArea") {
        obj.BuildingArea = $.trim(newVal);                 //面积
    }
    if (tableModule == "#tab_InnerBuildingArea") {
        obj.InnerBuildArea = $.trim(newVal);                 //面积
    }
    if (tableModule == "#tab_PurposeName") {
        obj.PurposeCode = $.trim(newVal);               //用途
    }
    if (tableModule == "#tab_SJPurposeName") {
        obj.SJPurposeCode = $.trim(newVal);               //实际用途
    }

    if (tableModule == "#tab_UnitPrice") {
        obj.UnitPrice = $.trim(newVal);                 //初始单价
    }
    if (tableModule == "#tab_Weight") {
        obj.Weight = $.trim(newVal);                    //价格系数
    }
    if (tableModule == "#tab_FrontName") {
        obj.FrontCode = $.trim(newVal);                 //朝向
    }
    if (tableModule == "#tab_SightName") {
        obj.SightCode = $.trim(newVal);                 //景观
    }
    if (tableModule == "#tab_IsEValue") {
        obj.IsEValue = $.trim(newVal);                  //是否可估
    }

}