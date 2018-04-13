//区域互斥
//动态加载区域（暂为新增设置区域按钮方法）

var mastername = "";


var masterId = getQueryString("Id");

$(function () {
    $.post("/Employee/GetMasterById", { "Id": masterId }, function (jsondata) {
        if (jsondata != null && jsondata != "") {
            mastername = jsondata.mastername;
        }
    });
});



$("#searchbtn").click(function () {
    //alert($("#scroll").scrollTop())
    var text = $("#searchkey").val();
    if (text) {
        var ts = $('#treeview-checkable-school').treeview("getAllNode");
        if (ts.length == 0) {
            bootbox.alert("请先选择学校所在区域。");
        } else {
            //$('#treeview-checkable-school').treeview('search', [text, {
            //    ignoreCase: true,     // case insensitive
            //    exactMatch: false,    // like or equals
            //    revealResults: true   // reveal matching nodes
            //}])

            //var firstSearchResult = $(".search-result").first();
            //var nodeNumber = firstSearchResult.attr("data-nodeid");
            //var offset = nodeNumber * 38;
            //$("#scroll").scrollTop(offset);

            var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
            var parentAreaId = parentNode.AreaId;
            var parentId = parentNode.ParentId;
            var areaParentNode = $('#treeview-checkable').treeview('getNode', parentId);
            var showcheckbox = true;
            if (areaParentNode.state.showcheckbox == false) {
                showcheckbox = false;
            }
            $.post("/CompDepart/CompDepart_GetSearchSchools", { searchKey: text, areaId: parentAreaId }, function (data) {
                if (data.Success) {
                    $('#treeview-checkable-school').treeview({
                        data: [{ text: "全选", nodes: data.Data, ParentId: parentId, AreaId: parentAreaId, state: { showcheckbox: showcheckbox } }],
                        showIcon: false,
                        showCheckbox: true,
                        onNodeChecked: function (event, node) {
                            var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                            if (node.nodes) {//全选
                                //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                if (parentNode.nodes) {//全选
                                    for (x in parentNode.nodes) {
                                        if (!parentNode.nodes[x].state.showcheckbox) {
                                            $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                                        }
                                    }
                                }
                                //if (!parentNode) {//当前点击的节点可被选中 则选中该节点
                                //    $('#treeview-checkable').treeview('checkNode', [node.ParentId, { silent: true }]);
                                //    checkParentNode(areaParentNode, $('#treeview-checkable'));
                                //}

                            } else {
                                var otherChecked = true;
                                if (parentNode.nodes) {
                                    for (x in parentNode.nodes) {
                                        if (parentNode.nodes[x].state.checked == false) {
                                            otherChecked = false;
                                        }
                                    }
                                }
                                if (otherChecked) {
                                    $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                    //if (!isschoolselted) {
                                    //$('#treeview-checkable').treeview('checkNode', [areaParentNode.nodeId, { silent: true }]);
                                    //checkParentNode(areaParentNode, $('#treeview-checkable'));
                                    //}
                                }
                            }
                        },
                        onNodeUnchecked: function (event, node) {
                            if (node.nodes) {
                                var selectNodes = getNodeIdArr(node);
                                $('#treeview-checkable-school').treeview('uncheckNode', [selectNodes, { silent: true }]);
                                $('#treeview-checkable').treeview('uncheckNode', [node.ParentId, { silent: true }]);
                            } else {
                                $('#treeview-checkable-school').treeview('uncheckNode', [0, { silent: true }]);
                            }
                            //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            //var parentAreaNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId)
                            //$('#treeview-checkable').treeview('uncheckNode', [parentAreaNode.nodeId, { silent: true }]);
                            //uncheckParentNode(parentAreaNode, $('#treeview-checkable'));
                        }
                    });
                }
            });
        }
    } else {
        bootbox.alert("请输入要搜索的学校。");
    }
});


//默认树的节点点击事件
function itemOnclick(target) {

}

function uncheckedNodes(node, $tree) {
    if (node.nodes) {
        var selectNodes = getNodeIdArr(node);
        if (selectNodes) {
            $tree.treeview('uncheckNode', [selectNodes, { silent: true }]);
        }
        for (x in node.nodes) {
            uncheckedNodes(node.nodes[x], $tree);
        }
    }
}

function checkedNodes(node, $tree) {
    if (node.nodes) {
        var selectNodes = getNodeIdArr(node);
        if (selectNodes) {
            $tree.treeview('checkNode', [selectNodes, {
                silent: true
            }]);
        }
        for (x in node.nodes) {
            checkedNodes(node.nodes[x], $tree);
        }
    }
}

function getNodeIdArr(node) {
    var ts = [];
    if (node.nodes) {
        for (x in node.nodes) {
            ts.push(node.nodes[x].nodeId)
        }
    } else {
        ts.push(node.nodeId);
    }
    return ts;
}

//设置区域确定按钮事件（暂为新增设置区域确定按钮方法）
$("#btn_confirmArea").click(function () {
    var ts = $('#treeview-checkable-checked').treeview("getAllNode");
    if (ts.length <= 1) {
        bootbox.alert("请选择区域。");
    } else {
        var parentid = $("#p4deptid").val();//新增节点的父id,是父节点的deptid
        var areas = "";
        var districtid = "";
        var node = $("#treeview-checkable-checked").treeview('getNode', 0);
        if (node.nodes) {
            for (z in node.nodes) {
                areas += "<p>" + node.nodes[z].text + "</p>";
                districtid += node.nodes[z].tag +  "@";
            }
        }
        var p4deptid = $.trim($("#p4deptid").val());
        //if (districtid != "") {
        //    $.post("/Employee/JudgmentSelectedArea", { Area: districtid, mastername: mastername, deptid: p4deptid }, function (data) {
        //        if (data.Success) {
        //            bootbox.alert(data.ErrorMsg);
        //        }
        //    });
        //}
        $("#Area").html(areas);
        $("#EmployeeDeptAreaIds").val(districtid);
        $('#setAreaModal').modal('hide');
    }
});
//右移加载已选区域按钮事件
$("#addNode").click(function () {
    //-------------------第一颗树---------------------------
    //获取全部节点，如果全部选中，提示错误
    var nodes = [];
    nodes = $('#treeview-checkable').treeview("getAllNode");
    var allNodeCheck = true;
    for (n in nodes) {
        if (nodes[n].state.checked == false) {
            allNodeCheck = false;
            break;
        }
    }
    if (allNodeCheck == true) {
        bootbox.alert("子区域不能全部选中！");
        return;
    }

    var checkeds = [];
    checkeds = $('#treeview-checkable').treeview("getChecked");
    //去重复
    var uniqueCheckeds = [];
    $.each(checkeds, function (i, el) {
        if ($.inArray(el, uniqueCheckeds) === -1) uniqueCheckeds.push(el);
    });
    var needCheckeds = [];
    //排除逻辑, 1 所有的兄弟节点都选中,所有相关节点(这个逻辑需再考察)
    for (x in uniqueCheckeds) {
        var parentNode = $('#treeview-checkable').treeview('getParent', uniqueCheckeds[x]);
        var allSiblingsChecked = true;
        if (parentNode.nodes) {
            for (y in parentNode.nodes) {
                if (parentNode.nodes[y].state.checked == false) {
                    allSiblingsChecked = false;
                    break;
                }
            }
            if (allSiblingsChecked == false) {
                needCheckeds.push(uniqueCheckeds[x]);
            } else {
                needCheckeds.push(parentNode);
            }
        } else {
            needCheckeds.push(uniqueCheckeds[x]);
        }
    }

    var uniqueNeedCheckeds = [];
    $.each(needCheckeds, function (i, el) {
        if ($.inArray(el, uniqueNeedCheckeds) === -1) uniqueNeedCheckeds.push(el);
    });

    var uniqueCheckedsSchool = [];
    //--------学校树中的选中节点---------------------------
    var schoolParentNode = $('#treeview-checkable-school').treeview('getNode', 0);
    if (schoolParentNode.nodeId != undefined) {
        var areaParentNode = $('#treeview-checkable').treeview('getNode', schoolParentNode.ParentId);//是否有学校已被选择 
        if (areaParentNode.state.checked == false) {
            if (schoolParentNode.state.checked == true) {
                if (schoolParentNode.nodes != null) {
                    for (var n in schoolParentNode.nodes) {
                        if (schoolParentNode.nodes[n].state.checked == true)
                            uniqueNeedCheckeds.push(schoolParentNode.nodes[n]);
                    }
                }
            } else {
                var checkedsSchool = $('#treeview-checkable-school').treeview("getChecked");
                //去重复
                $.each(checkedsSchool, function (i, el) {
                    if ($.inArray(el, uniqueCheckedsSchool) === -1 && el.nodeId != 0) uniqueCheckedsSchool.push(el);
                });
            }
        }
    }

    var removeChildCheckeds = [];//第一颗树的选中节点
    //去除父级的子级,第一颗树------------------------
    for (var k = 0; k < uniqueNeedCheckeds.length; k++) {
        var isNotChild = true;
        for (var l = 0; l < uniqueNeedCheckeds.length; l++) {
            if (uniqueNeedCheckeds[k].parentId == uniqueNeedCheckeds[l].nodeId) {
                isNotChild = false;
            }
        }
        if (isNotChild) {
            removeChildCheckeds.push(uniqueNeedCheckeds[k]);
        }
    }

    var needAddNodes = [];
    if (removeChildCheckeds.length > 0) {
        for (var i = 0; i < removeChildCheckeds.length; i++) {
            needAddNodes.push(removeChildCheckeds[i]);
        }
    }

    if (uniqueCheckedsSchool.length > 0) {
        for (var i = 0; i < uniqueCheckedsSchool.length; i++) {
            needAddNodes.push(uniqueCheckedsSchool[i]);
        }
    }

    var newTreeData = [];
    for (var i = 0; i < needAddNodes.length; i++) {
        newTreeData[i] = { tag: needAddNodes[i].tag, text: needAddNodes[i].text, state: needAddNodes[i].state };
    }

    //$.each(checkeds, function (i, el) {
    //    el.state.checked = false;
    //});

    //-------------加载treeview-checkable-checked------
    //覆盖
    if (newTreeData.length == 0) {
        bootbox.alert("请选择区域。");
    } else {
        var firstChildNode = $('#treeview-checkable-checked').treeview('getNode', 1);
        if (!firstChildNode.nodeId) {
            $('#treeview-checkable-checked').treeview({
                data: [{ text: "已选区域", nodes: newTreeData, state: { checked: true }, nodeId: 0 }],
                showIcon: false,
                showCheckbox: true,
                onNodeChecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('checkNode', [selectNodes, { silent: true }]);
                    } else {
                        var parentNode = $('#treeview-checkable-checked').treeview('getNode', 0);
                        var otherChecked = true;
                        if (parentNode.nodes) {
                            for (x in parentNode.nodes) {
                                if (parentNode.nodes[x].state.checked == false) {
                                    otherChecked = false;
                                }
                            }
                        }
                        if (otherChecked) {
                            $('#treeview-checkable-checked').treeview('checkNode', [0, { silent: true }]);
                        }
                    }
                },
                onNodeUnchecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('uncheckNode', [selectNodes, { silent: true }]);
                    } else {
                        $('#treeview-checkable-checked').treeview('uncheckNode', [0, { silent: true }]);
                    }
                }
            });
        } else {
            //追加node
            //判断追加的，在树中有没有相同的，有没有子级，有没有父级
            var checkedParentNode = $('#treeview-checkable-checked').treeview('getNode', 0);
            var uniqueNewTreeData = [];
            for (a in newTreeData) {
                var needAdd = true;
                for (s in checkedParentNode.nodes) {
                    if (checkedParentNode.nodes[s].text == newTreeData[a].text) {
                        needAdd = false;//给出提示
                        //bootbox.alert("不能添加相同的区域或学校！" + newTreeData[a].text + "！");
                        //return;
                        break;
                    }

                    //xuexiao
                    var schoolinfo = checkedParentNode.nodes[s].tag.split("?");
                    var re = schoolinfo[0].split("|");
                    var reCompare2 = newTreeData[a].tag.split("?");

                    if (schoolinfo.length >= 3) {
                        var reSch = newTreeData[a].tag.split("|");
                        if (reCompare2.length < 3) {
                            if (re[0] == reSch[0]) {
                                needAdd = false;//给出提示，学校已选择,qu
                                bootbox.alert("区域内学校已选择！不能再添加" + newTreeData[a].text + "！");
                                return;
                            }
                            if (re[0].substring(0, 4) + "00000" == reSch[0]) {
                                needAdd = false;//给出提示，学校已选择,shi
                                bootbox.alert("区域内学校已选择！不能再添加" + newTreeData[a].text + "！");
                                return;
                            }
                            if (re[0].substring(0, 2) + "0000000" == reSch[0]) {
                                needAdd = false;//给出提示，学校已选择,shen
                                bootbox.alert("区域内学校已选择！不能再添加" + newTreeData[a].text + "！");
                                return;
                            }
                        }
                    } else {
                        //shen
                        if (re[0].substring(2, 9) == '0000000') {
                            var rePro = reCompare2[0].split("|");
                            if (reCompare2.length >= 3) {
                                if (re[0].substring(0, 6) == rePro[0].substring(0, 6)) {
                                    needAdd = false;//xue
                                    bootbox.alert("省份已选择！不能再添加省份内的学校，" + newTreeData[a].text + "！");
                                    return;
                                }
                            } else {
                                if (rePro[0].substring(4, 9) == '00000') {
                                    if (re[0].substring(0, 2) == rePro[0].substring(0, 2)) {
                                        needAdd = false;//给出提示，学校已选择,shi
                                        bootbox.alert("省份已选择！不能再添加省份内的城市，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                                if (rePro[0].substring(6, 9) == '000') {
                                    if (re[0].substring(0, 2) == rePro[0].substring(0, 2)) {
                                        needAdd = false;//给出提示，学校已选择,qu
                                        bootbox.alert("省份已选择！不能再添加省份内的区域，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                            }
                        }
                        //shi
                        if (re[0].substring(4, 9) == '00000') {
                            var reCity = reCompare2[0].split("|");
                            if (reCompare2.length >= 3) {
                                if (re[0].substring(0, 4) == reCity[0].substring(0, 4)) {
                                    needAdd = false;//xue
                                    bootbox.alert("城市已选择！不能再添加城市内的学校，" + newTreeData[a].text + "！");
                                    return;
                                }
                            } else {
                                if (reCity[0].substring(2, 9) == '0000000') {
                                    if (re[0].substring(0, 2) + "0000000" == reCity[0]) {
                                        needAdd = false;//给出提示，学校已选择,shen
                                        bootbox.alert("城市已选择！不能再添加城市所在省份，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                                if (reCity[0].substring(6, 9) == '000') {
                                    if (re[0].substring(0, 4) == reCity[0].substring(0, 4)) {
                                        needAdd = false;//给出提示，学校已选择,qu
                                        bootbox.alert("城市已选择！不能再添加城市内的区域，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                            }
                        }
                        //qu          
                        if (re[0].substring(6, 9) == '000') {
                            var reDir = reCompare2[0].split("|");
                            if (reCompare2.length >= 3) {
                                if (reDir[0] == re[0]) {
                                    needAdd = false;
                                    bootbox.alert("区域已选择！不能再添加区域内的学校，" + newTreeData[a].text + "！");
                                    return;
                                }
                            } else {
                                if (reDir[0].substring(2, 9) == '0000000') {
                                    if (re[0].substring(0, 2) + "0000000" == reDir[0]) {
                                        needAdd = false;//给出提示，学校已选择,shen
                                        bootbox.alert("区域已选择！不能再添加区域所在省份，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                                if (reDir[0].substring(4, 9) == '00000') {
                                    if (re[0].substring(0, 4) + '00000' == reDir[0]) {
                                        needAdd = false;//给出提示，学校已选择,shi
                                        bootbox.alert("区域已选择！不能再添加区域所在城市，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                if (needAdd) {
                    uniqueNewTreeData.push(newTreeData[a]);
                }
            }
            if (uniqueNewTreeData.length > 0) {
                $('#treeview-checkable-checked').treeview('checkNode', [0, { silent: true }]);
                $('#treeview-checkable-checked').treeview('addNode', [0, { node: uniqueNewTreeData }]);
            }
        }
    }
});

//左移清空已选区域按钮事件
$("#removeNode").click(function () {
    //移除选中的节点
    var checkeds = $('#treeview-checkable-checked').treeview("getChecked");
    //去重复
    var uniqueCheckeds = [];
    $.each(checkeds, function (i, el) {
        if ($.inArray(el, uniqueCheckeds) === -1) uniqueCheckeds.push(el);  
    });
    for (var i = 0; i < uniqueCheckeds.length; i++) {
        if (uniqueCheckeds[i].nodeId!=0) {
            $('#treeview-checkable-checked').treeview("deleteNode", uniqueCheckeds[i].nodeId);
        }
    }
    $('#treeview-checkable-checked').treeview('uncheckNode', [0, { silent: true }]);
});


function checkParentNode(node, $tree) {
    var parentNode = $tree.treeview('getParent', node);
    if (parentNode.nodeId) {
        var otherChecked = true;
        if (parentNode.nodes) {
            for (x in parentNode.nodes) {
                if (parentNode.nodes[x].state.checked == false) {
                    otherChecked = false;
                }
            }
        }
        if (otherChecked) {
            $tree.treeview('checkNode', [parentNode.nodeId, { silent: true }]);
            checkParentNode(parentNode, $tree);
        }
    }
}

function uncheckParentNode(node, $tree) {
    var parentNode = $tree.treeview('getParent', node);
    if (parentNode.nodeId) {
        $tree.treeview('uncheckNode', [parentNode.nodeId, { silent: true }]);
        uncheckParentNode(parentNode, $tree);
    }
}

