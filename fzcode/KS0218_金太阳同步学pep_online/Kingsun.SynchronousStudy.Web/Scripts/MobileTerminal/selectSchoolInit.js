var SelectSchoolInit = function () {
    var Current = this;
    this.schoolID = "";
    this.districtID = "";
    this.cityID = "";
    this.provinceID = "";


    this.Init = function () {
        Current.UserID = getQueryString("UserID");
        Current.provinceID = getQueryString("provinceID");
        Current.cityID = getQueryString("cityID");
        Current.districtID = getQueryString("districtID");
        Current.schoolID = getQueryString("schoolID");
        Current.eType = getQueryString("eType");
        Current.TrueName = getQueryString("TrueName");

        if (Current.eType == "23") {
            Current.InitStandBook();
        } else {
            Current.InitProvinceInfo();
        }

    };

    this.InitProvinceInfo = function () {
        //初始化学校数据
        var html = "";
        if (Current.districtID != "" && Current.districtID != "undefined" && Current.districtID != null) {
            Common.District.GetSchool(parseInt(Current.districtID.split('_')[1]), null, "", function (school) {
                var provinceArr = school;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, school) {
                        html += "<div class=\"sort_list\">";
                        html += " <blockquote class=\"num_name\" id=\"" + school.ID + "\">" + school.SchoolName + "</blockquote>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                } else {
                    window.location.href =  Constant.classInfo_Url + "StudyReportManagement/AddInformation.aspx?provinceID=" + Current.provinceID + "&cityID=" + Current.cityID + "&districtID=" + Current.districtID + "&UserID=" + Current.UserID + "&TrueName=" + Current.TrueName;
                }
            });
        }//初始化地区数据
        else if (Current.cityID != "" && Current.cityID != "undefined" && Current.cityID != null) {
            Common.CodeAjax_City("do.area", parseInt(Current.cityID.split('_')[1]), function (data) {
                var provinceArr = data.Data;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, province) {
                        html += "<div class=\"sort_list\">";
                        html += " <blockquote class=\"num_name\" id=\"" + province.ID + "\">" + province.CodeName + "</blockquote>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                } else {
                    window.location.href =  Constant.classInfo_Url + "StudyReportManagement/AddInformation.aspx?provinceID=" + Current.provinceID + "&cityID=" + Current.cityID + "&UserID=" + Current.UserID + "&TrueName=" + Current.TrueName;
                }
            });
        }//初始化城市数据
        else if (Current.provinceID != "" && Current.provinceID != "undefined" && Current.provinceID != null) {
            Common.CodeAjax_City("do.area", parseInt(Current.provinceID.split('_')[1]), function (data) {
                var provinceArr = data.Data;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, province) {
                        html += "<div class=\"sort_list\">";
                        html += " <blockquote class=\"num_name\" id=\"" + province.ID + "\">" + province.CodeName + "</blockquote>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                } else {
                    window.location.href =  Constant.classInfo_Url + "StudyReportManagement/AddInformation.aspx?provinceID=" + Current.provinceID + "&UserID=" + Current.UserID + "&TrueName=" + Current.TrueName;
                }
            });//初始化省份数据
        } else if (Current.provinceID == "" || Current.provinceID == "undefined" || Current.provinceID == null) {
            Common.CodeAjax_City("do.area", 0, function (data) {
                var provinceArr = data.Data;
                if (provinceArr != null) {
                    $.each(provinceArr, function (p, province) {
                        html += "<div class=\"sort_list\">";
                        html += " <blockquote class=\"num_name\" id=\"" + province.ID + "\">" + province.CodeName + "</blockquote>";
                        html += "</div>";
                    });

                    $("#div_info").html(html);
                    var sortinit = new sort();
                } else {
                    window.location.href =  Constant.classInfo_Url + "StudyReportManagement/AddInformation.aspx?UserID=" + Current.UserID + "&TrueName=" + Current.TrueName;
                }
            });
        }
    }


    $("#close").click(function () {
        //调用移动端接口
        var data = {

        };
        //调用移动端的方法
        window.WebViewJavascriptBridge.callHandler(
            'finish', data, function (responseData) {

            }
        );
    });

    this.InitStandBook = function () {
        var html = "";
        $.post("../Handler/WeChatHandler.ashx?queryKey=getAppList", null, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    var appList = result.AppList;
                    if (appList != null && appList.length > 0) {
                        for (var i = 0; i < appList.length; i++) {
                            html += "<div class=\"sort_list\">";
                            html += " <blockquote class=\"num_name\" id=\"" + appList[i].VersionID + "\">" + appList[i].VersionName + "</blockquote>";
                            html += "</div>";
                        }
                        $("#div_info").html(html);
                        var sortinit = new sort();
                    }
                }
            }
        });
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
