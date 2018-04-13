var SelectSchoolInit = function () {
    var Current = this;
    this.schoolID = "";
    this.districtID = "";
    this.cityID = "";
    this.provinceID = "";
    this.TrueName = "";
    this.UserId = "";


    this.Init = function () {
        Current.UserID = getQueryString("UserID");
        Current.provinceID = getQueryString("provinceID");
        Current.cityID = getQueryString("cityID");
        Current.districtID = getQueryString("districtID");
        Current.schoolID = getQueryString("schoolID");
        Current.TrueName = getQueryString("TrueName");
        Current.UserId = getQueryString("UserId");

        Current.InitProvinceInfo();

    };

    this.InitProvinceInfo = function () {
        //初始化学校数据
        var html = "";
        if (Current.districtID !== "" && Current.districtID !== "undefined" && Current.districtID != null) {
            window.Common.District.GetSchool(parseInt(Current.districtID), null, "", function (school) {
                var provinceArr = school;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, school) {
                        html += "<div class=\"sort_list\">";
                        html += " <a class=\"num_name\" id=\"" + school.ID + "\" href=\"../ClassManagement/PerfectData.aspx?schoolID=" + school.ID + "&schoolName=" + school.SchoolName + "&districtID=" + Current.districtID + "&cityID=" + Current.cityID + "&provinceID=" + Current.provinceID + "&UserId=" + Current.UserId + "&TrueName=" + Current.TrueName + "\">" + school.SchoolName + "</a>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                }
            });
        }//初始化地区数据
        else if (Current.cityID != "" && Current.cityID != "undefined" && Current.cityID != null) {
            window.Common.CodeAjax_City("do.area", parseInt(Current.cityID), function (data) {
                var provinceArr = data.Data;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, district) {
                        html += "<div class=\"sort_list\">";
                        html += " <a class=\"num_name\" id=\"" + district.ID + "\"  href=\"../ClassManagement/SelectSchool.aspx?districtID=" + district.ID + "&cityID=" + Current.cityID + "&provinceID=" + Current.provinceID + "&UserId=" + Current.UserId + "&TrueName=" + Current.TrueName + "\">" + district.CodeName + "</a>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                }
            });
        }//初始化城市数据
        else if (Current.provinceID != "" && Current.provinceID != "undefined" && Current.provinceID != null) {
            window.Common.CodeAjax_City("do.area", parseInt(Current.provinceID), function (data) {
                var provinceArr = data.Data;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, city) {
                        html += "<div class=\"sort_list\">";
                        html += " <a class=\"num_name\" id=\"" + city.ID + "\" href=\"../ClassManagement/SelectSchool.aspx?cityID=" + city.ID + "&provinceID=" + Current.provinceID + "&UserId=" + Current.UserId + "&TrueName=" + Current.TrueName + "\">" + city.CodeName + "</a>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                }
            });//初始化省份数据
        } else if (Current.provinceID == "" || Current.provinceID == "undefined" || Current.provinceID == null) {
            window.Common.CodeAjax_City("do.area", 0, function (data) {
                var provinceArr = data.Data;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, province) {
                        html += "<div class=\"sort_list\">";
                        html += " <a class=\"num_name\" id=\"" + province.ID + "\" href=\"../ClassManagement/SelectSchool.aspx?provinceID=" + province.ID + "&UserId=" + Current.UserId + "&TrueName=" + Current.TrueName + "\" >" + province.CodeName + "</a>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                }
            });
        }
    }
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


$(function () {
    var selectSchool = new SelectSchoolInit();
    selectSchool.Init();
})
