$(function () {
    LoadTreeDept();
    
});

//加载树形菜单
function LoadTreeDept() {
    $.ajax({
        type: "Post",
        url: "/Employee/GetDept",
        dataType: "json",
        success: function (result) {
            $('#tree').treeview({
                data: result,
                showIcon: false,
                onNodeExpanded: function (event, node) {
                    var tree = $('#tree');
                    if (node.state.expanded) {//展开  
                        tree.treeview('expandNode', node.nodeId);
                    } else { //折叠  
                        tree.treeview('collapseNode', node.nodeId);
                    }
                }
            });
        },
        error: function () {
            alert("树形结构加载失败！")
        }
    });
}