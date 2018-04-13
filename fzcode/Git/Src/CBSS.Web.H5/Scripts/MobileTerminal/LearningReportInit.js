/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var LearningReportInit = function () {
    var Current = this;

    this.Init = function () {
        //var loginState = $("#user").html();
        //if (!loginState) {
        //    window.location.href = "Login.aspx?Type=1";
        //}
    };
}


var learningReportInit;
$(function () {
    learningReportInit = new LearningReportInit();
    learningReportInit.Init();
});