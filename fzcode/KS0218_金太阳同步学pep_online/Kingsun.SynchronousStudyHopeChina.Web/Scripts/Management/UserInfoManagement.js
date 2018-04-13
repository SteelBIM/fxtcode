var Kingsun = Kingsun || {}
Kingsun.AppLibrary = Kingsun.AppLibrary || {}
Kingsun.AppLibrary.UserinfoManagement = Kingsun.AppLibrary.UserinfoManagement || {}
Kingsun.AppLibrary.UserinfoManagement = function () {
    this.QueryUserinfo = function (WhereCondition, CurrentPageIndex, PageSize, Event) {
        var data = {
            Where: WhereCondition,
            PageIndex: CurrentPageIndex,
            PageSize: PageSize
        };
        return Common.Ajax("UserinfoImplement", "QueryUserinfo", data, Event);
    };
    this.StarPageByUserId = function (Userid, Event) {
        var data = {
            Userid: Userid
        };
        return Common.Ajax("UserinfoImplement", "StarPageByUserId", data, Event);
    };
    this.QueryUserinfoById = function (Userid, Event) {
        var data = {
            Userid: Userid
        };
        return Common.Ajax("UserinfoImplement", "QueryUserinfoById", data, Event);
    };
    this.AddUserinfo = function (Userid, Username, Parentname, Phone, Teachername, Teacherphone, Schoolname, Period, Grade, Isjoin, AccountName, Event) {
        var data = {
            Userid:Userid,
            Username:Username,
            Parentname:Parentname,
            Phone:Phone,
            Teachername:Teachername,
            Teacherphone:Teacherphone ,
            Schoolname:Schoolname,
            Period:Period,
            Grade:Grade ,
            Isjoin:Isjoin ,
            AccountName:AccountName
        };
        return Common.Ajax("UserinfoImplement", "AddUserinfo", data, Event);
    };
}
var UserinfoManage = new Kingsun.AppLibrary.UserinfoManagement();