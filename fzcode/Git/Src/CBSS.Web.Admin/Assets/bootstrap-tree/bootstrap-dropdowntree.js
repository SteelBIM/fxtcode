//需要配合使用下拉控件，树形控件
//
//
//
//

$.fn.dropdownTree = function (result, fn ) {
    var $this = $(this);
    var id = $this.attr("id");
    $this.html("");
    var $html = ' <input type="hidden" id="hid' + id + '" value="">';
    $html += ' <input type="hidden" id="Phid' + id + '" value="">';
    $html += ' <input type="hidden" id="Lhid' + id + '" value="">';
    $html += ' <div class="btn-group bootstrap-select show-tick form-control">';
    $html += '  <button type="button" class="btn dropdown-toggle form-control selectpicker btn-default" id="btn' + id + '" title="全部">';
    $html += '     <span class="filter-option pull-left">全部</span>&nbsp;<span class="caret"></span>';
    $html += ' </button> ';
    $html += ' <div id="' + id + 'tree" class=" dropdown-menu open">';
    $html += '     <div class="divtree"></div>';
    $html += ' </div>';
    $html += '</div>';
    $($html).appendTo($this);
    var $treecont = $("#" + id + "tree")
    var $treediv = $treecont.find(".divtree");
    var $btn = $("#btn" + id);
    var $hidden = $("#hid" + id);
    var $Phidden = $("#Phid" + id);
    var $Lhidden = $("#Lhid" + id);

    if (result) {
    $treediv.treeview({
            data: result,
            showIcon: false,
            onNodeExpanded: function (event, node) {
                var tree = $treediv;
                if (node.state.expanded) {//展开  
                    tree.treeview('expandNode', node.nodeId);
                } else { //折叠  
                    tree.treeview('collapseNode', node.nodeId);
                }
            },
            onNodeSelected: function (event, data) {
                if(fn) {
                    fn(data);
                }
                $("#btn" +id).find("span").first().text(data.text);
                $hidden.val(data.Id);
                $Phidden.val(data.ParentId)
                $Lhidden.val(data.Level)

                $("#" +id + "tree").hide();
            }
        });
    $treediv.treeview('collapseAll', { silent: true
    });
         $btn.click(function () {
        if ($("#" +id + "tree").is(':visible')) {
            $("#" +id + "tree").hide();
    } else {
            $("#" +id + "tree").show();
        }
        });
    $(document).click(function (e) {
        e = window.event || e; // 兼容IE7
        obj = $(e.srcElement || e.target);
        if (!$(obj).hasClass("expand-icon") && $(obj).closest("#" +id + "tree").length == 0 && !$(obj).is("#" +id + "tree ,#btn" +id) && $(obj).closest("#btn" + id).length == 0) {
    //要执行的函数b
            $("#" + id + "tree").hide();
        }
        });
    }
   
}

$.fn.ddTreeGetValue = function () {
    var $this = $(this);
    var id = $this.attr("id");
    return $this.find("#hid" + id).val();
}

$.fn.ddTreeGetPValue = function () {
    var $this = $(this);
    var id = $this.attr("id");
    return $this.find("#Phid" + id).val();
}

$.fn.ddTreeGetLValue = function () {
    var $this = $(this);
    var id = $this.attr("id");
    return $this.find("#Lhid" + id).val();
}

$.fn.ddTreeClearValue = function () {
    var $this = $(this);
    var id = $this.attr("id");
    var $treecont = $("#" + id + "tree")
    var $treediv = $treecont.find(".divtree");
    $treediv.treeview('collapseAll', { silent: true });
    var node = $treediv.treeview('getNode', 0);
    $treediv.treeview('selectNode', node, { silent: true });
}

$.fn.ddTreeReload = function (result, fn) {
    if (result) {
         var $this = $(this);
    var id = $this.attr("id");
    var $treecont = $("#" + id + "tree")
    var $treediv = $treecont.find(".divtree");
    $treediv.treeview({
        data: result,
        showIcon: false,
        onNodeExpanded: function (event, node) {
            var tree = $treediv;
            if (node.state.expanded) {//展开  
                tree.treeview('expandNode', node.nodeId);
            } else { //折叠  
                tree.treeview('collapseNode', node.nodeId);
            }
        },
        onNodeSelected: function (event, data) {
            if (fn) {
                fn(data);
            }
            $("#btn" + id).find("span").first().text(data.text);
            $("#hid" + id).val(data.Id);
            $("#Phid" + id).val(data.ParentId);
            $("#Lhid" + id).val(data.Level)
            $("#" + id + "tree").hide();
        }
    });
    }
   
}
