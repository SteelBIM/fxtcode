var Kingsun = Kingsun || {}
Kingsun.AppLibrary = Kingsun.AppLibrary || {}
Kingsun.AppLibrary.OrderManagement = Kingsun.AppLibrary.OrderManagement || {}
Kingsun.AppLibrary.OrderManagement = function () {
    this.GetAllEditions = function (Event) {
        var data = {};
        return Common.Ajax("OrderImplement", "GetAllEditions", data, Event);
    }

    this.QueryCoupon = function (Condition, PageIndex, PageSize, Event) {
        var data = {
            PageIndex: PageIndex,
            PageSize: PageSize,
            Where: Condition
        };
        return Common.Ajax("OrderImplement", "QueryCoupon", data, Event);
    }
}
var orderManage = new Kingsun.AppLibrary.OrderManagement();