function addcloud() {//增加页面遮罩
    var bodyWidth = document.documentElement.clientWidth;
    var bodyHeight = window.screen.availHeight; //Math.max(document.documentElement.clientHeight, document.body.scrollHeight);
    var bgObj = document.createElement("div");
    bgObj.setAttribute('id', 'bgDivload');
    bgObj.style.position = "fixed";
    bgObj.style.top = "0px";
    bgObj.style.background = "#000000";
    bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
    bgObj.style.opacity = "0.2";
    bgObj.style.left = "0";
    bgObj.style.width = bodyWidth + "px";
    bgObj.style.height = bodyHeight + "px";
    bgObj.style.zIndex = "10000"; //设置它的zindex属性，让这个div在z轴最大，用户点击页面任何东西都不会有反应|
    document.body.appendChild(bgObj); //添加遮罩

    var loadingObj = document.createElement("div");
    loadingObj.setAttribute('id', 'loadingDiv');
    loadingObj.style.position = "fixed";
    loadingObj.style.top = bodyHeight / 2 + "px";
    loadingObj.style.left = bodyWidth / 2 + "px";
    //loadingObj.style.background = "white";//url(/images/loading.gif)
    loadingObj.innerHTML = "<img style='width:36px;' src=\"/Images/common/loading.gif\" /><p style='color:white'>" + "" + "</P>";//showName
    loadingObj.style.width = "36px";
    loadingObj.style.height = "36px";
    loadingObj.style.zIndex = "10000";
    document.body.appendChild(loadingObj); //添加loading动画-
}
function removecloud() {//移除页面遮罩
    $("#loadingDiv").remove();
    $("#bgDivload").remove();
}
