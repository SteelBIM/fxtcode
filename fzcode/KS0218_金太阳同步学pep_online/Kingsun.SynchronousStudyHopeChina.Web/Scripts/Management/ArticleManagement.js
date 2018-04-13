var Kingsun = Kingsun || {}
Kingsun.AppLibrary = Kingsun.AppLibrary || {}
Kingsun.AppLibrary.ArticleManagement = Kingsun.AppLibrary.ArticleManagement || {}
Kingsun.AppLibrary.ArticleManagement = function () {
    this.QueryCourse = function (WhereCondition, CurrentPageIndex, PageSize, Event) {
        var data = {
            Where: WhereCondition,
            PageIndex: CurrentPageIndex,
            PageSize: PageSize
        };
        return Common.Ajax("ArticleImplement", "QueryArticle", data, Event);
    };
}
var articleManage = new Kingsun.AppLibrary.ArticleManagement();