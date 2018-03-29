
function BindLandCode(codeid, id) {
    $.ajax({
        url: '/land/land/GetLandPurpose',
        dataType: "Json",
        type: "POST",
        data: { code: 1002 },
        success: function (item) {
            var str = "";
            for (var i = 0; i < item.data.length; i++) {
                if (codeid == item.data[i].Value) {
                    str += "<option value='" + item.data[i].Value + "' selected='selected'>" + item.data[i].Text + "</option>";
                } else {
                    str += "<option value='" + item.data[i].Value + "'>" + item.data[i].Text + "</option>";
                }
            }
            $(id).empty().append(str);
        }
    });
}
function BindCity(cid, aid) {
    $.ajax({
        url: '/land/land/BindArea',
        dataType: "Json",
        type: "POST",
        data: { cityId: cid },
        success: function (item) {
            var str = "";
            for (var i = 0; i < item.data.length; i++) {
                if (aid == item.data[i].Value) {
                    str += "<option value='" + item.data[i].Value + "' selected='selected'>" + item.data[i].Text + "</option>";
                } else {
                    str += "<option value='" + item.data[i].Value + "'>" + item.data[i].Text + "</option>";
                }
            }
            $("#areaid").empty().append(str);
        }
    });
}
function BindSubArea(aid, subaid) {
    $.ajax({
        url: '/land/land/BingSubArean',
        dataType: "Json",
        type: "POST",
        data: { areaId: aid },
        success: function (item) {
            var str = "";
            for (var i = 0; i < item.data.length; i++) {
                if (subaid == item.data[i].Value) {
                    str += "<option value='" + item.data[i].Value + "' selected='selected'>" + item.data[i].Text + "</option>";
                } else {
                    str += "<option value='" + item.data[i].Value + "'>" + item.data[i].Text + "</option>";
                }
            }
            $("#subareaid").empty().append(str);
        }
    });
}
