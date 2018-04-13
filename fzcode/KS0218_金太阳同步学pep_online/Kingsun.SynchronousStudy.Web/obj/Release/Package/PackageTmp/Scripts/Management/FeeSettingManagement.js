var Kingsun = Kingsun || {}
Kingsun.AppLibrary = Kingsun.AppLibrary || {}
Kingsun.AppLibrary.FeeSettingManagement = Kingsun.AppLibrary.FeeSettingManagement || {}
Kingsun.AppLibrary.FeeSettingManagement = function () {
    this.QueryAppID = function (Event) {
        var data = {};
        return Common.Ajax("FeeSettingImplement", "QueryAppID", data, Event);
    };
    this.AddCombo = function (data, Event) {
        return Common.Ajax("FeeSettingImplement", "AddCombo", data, Event);
    };
    this.ModifyFeeCombo = function (data, Event) {
        return Common.Ajax("FeeSettingImplement", "ModifyFeeCombo", data, Event);
    };
    this.QueryFee = function (data, Event) {
        return Common.Ajax("FeeSettingImplement", "QueryFee", data, Event);
    };
    this.JFeeCombo = function (data, Event) {
        return Common.Ajax("FeeSettingImplement", "JFeeCombo", data, Event);
    };

    this.QueryUserInfo = function (where, pagesize, pageindex, Event) {
        var data = {
            Where: where,
            PageIndex: pageindex,
            PageSize: pagesize
        }
        return Common.Ajax("FeeSettingImplement", "QueryUserInfo", data, Event);
    }


}
var Feemanager = new Kingsun.AppLibrary.FeeSettingManagement();