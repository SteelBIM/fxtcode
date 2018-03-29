//secondmaster公共js kevin
//初始化树
function initTree(nodes) {
    $("#ltree").castree({ nodes: nodes, callback: {onNodeCreated:nodeCreated, onClick: clickTree} });
}
//自动加载选中的页面
function nodeCreated(event, treeId, treeNode) {
    if (treeNode) {
        if (treeNode.selected) goUrl(treeNode);
    }
}
//点击树
function clickTree(event, treeId, treeNode) {
    if (treeNode && treeNode.ctl) {
        goUrl(treeNode);
    }
}
//组装转向页面
function goUrl(treeNode) {
    var index = treeNode.ctl.indexOf("api:");
    if (index >= 0)
        var url = CAS.APIPage({ api: treeNode.ctl.substring(index + 4), data: treeNode.data });
    else
        var url = CAS.RootUrl + treeNode.ctl + ".aspx";
    if (treeNode.args) url += treeNode.args;
    $(this).parent().selectOnly();
    setIframe(treeNode.id, url);
}
//显示页面
function setIframe(id, url) {
    if (CAS.IframePageRefresh) {
        if (!$("#frm_frame").attr("id"))
            $('<iframe id="frm_frame" width="100%" height="100%" scrolling="no" src="' + url + '" frameborder="0"></iframe>').appendTo($("#mainContent"));
        else $("#frm_frame").attr("src", url);
    }
    else {
        if ($("#frm_" + id)[0]) {
            $("#frm_" + id).show().siblings().hide();
        }
        else {
            var frame = $('<iframe id="frm_' + id + '" width="100%" height="100%" scrolling="no" src="' + url + '" frameborder="0"></iframe>');
            frame.appendTo($("#mainContent")).siblings().hide();
        }
    }
}