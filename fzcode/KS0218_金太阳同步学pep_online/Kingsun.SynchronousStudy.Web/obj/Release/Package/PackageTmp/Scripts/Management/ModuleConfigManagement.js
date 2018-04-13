var Kingsun = Kingsun || {}
Kingsun.AppLibrary = Kingsun.AppLibrary || {}
Kingsun.AppLibrary.ModuleConfigManagement = Kingsun.AppLibrary.ModuleConfigManagement || {}
Kingsun.AppLibrary.ModuleConfigManagement = function () {
    this.QueryModuleConfigList = function (Event) {
        var data = {};
        return Common.Ajax("ModuleConfigImplement", "QueryModuleConfigList", data, Event);
    };
    this.DeleteModule = function (data, Event) {

        return Common.Ajax("ModuleConfigImplement", "DeleteModule", data, Event);
    };

    this.QueryModule = function (data, Event) {
        return Common.Ajax("ModuleConfigImplement", "QueryModule", data, Event);
    };
    this.ModifyModule = function (data, Event) {
        return Common.Ajax("ModuleConfigImplement", "ModifyModule", data, Event);
    };
}
var ModuleConfig = new Kingsun.AppLibrary.ModuleConfigManagement();