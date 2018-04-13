/// <reference path="../jquery-easyui/jquery.min.js" />
/// <reference path="../jquery-easyui/jquery.easyui.min.js" />
var easyuiMessage = {};
easyuiMessage.ShowBottom = function (message, title, timeout, showtype, fn) {
    if (!title) { title = "提示"; }
    if (!timeout) { timeout = 4000; }
    if (!showtype) { showtype = "slide"; }
    $.messager.show({
        title: title,
        msg: message,
        timeout: timeout,
        showType: showtype
    });
    if (fn) {
        setTimeout(function () {
            fn();
        }, timeout);
    }
};

easyuiMessage.ShowTopMid = function (message, title, timeout, showtype, fn) {
    if (!title) { title = "提示"; }
    if (!timeout) { timeout = 4000; }
    if (!showtype) { showtype = "slide"; }
    $.messager.show({
        title: title,
        msg: message,
        timeout: timeout,
        showType: showtype,
        style: {
            right: '',
            top: document.body.scrollTop + document.documentElement.scrollTop,
            bottom: ''
        }
    });
    if (fn) {
        setTimeout(function () {
            fn();
        }, timeout);
    }
}

easyuiMessage.Alert = function (message, title, icon, fn) {
    if (!title) { title = "警告"; }
    if (fn) {
        $.messager.alert(title, message, icon, fn);
    } else {
        $.messager.alert(title, message, icon);
    }
}

easyuiMessage.Confirm = function (message, fn, title) {
    if (!title) { title = "确认对话框"; }
    if (fn) {
        $.messager.confirm(title, message, fn);
    }
}


/**
	 * 扩展树表格级联勾选方法：
	 * @param {Object} container
	 * @param {Object} options
	 * @return {TypeName} 
	 */
$.extend($.fn.treegrid.methods, {
    /**
     * 级联选择
     * @param {Object} target
     * @param {Object} param 
     *		param包括两个参数:
     *			id:勾选的节点ID
     *			deepCascade:是否深度级联
     * @return {TypeName} 
     */
    cascadeCheck: function (target, param) {
        var opts = $.data(target[0], "treegrid").options;
        if (opts.singleSelect)
            return;
        var idField = opts.idField;//这里的idField其实就是API里方法的id参数
        var status = false;//用来标记当前节点的状态，true:勾选，false:未勾选
        var selectNodes = $(target).treegrid('getSelections');//获取当前选中项
        for (var i = 0; i < selectNodes.length; i++) {
            if (selectNodes[i][idField] == param.id)
                status = true;
        }
        //级联选择父节点
        selectParent(target[0], param.id, idField, status);
        selectChildren(target[0], param.id, idField, param.deepCascade, status);
        /**
         * 级联选择父节点
         * @param {Object} target
         * @param {Object} id 节点ID
         * @param {Object} status 节点状态，true:勾选，false:未勾选
         * @return {TypeName} 
         */
        function selectParent(target, id, idField, status) {
            var parent = $(target).treegrid('getParent', id);
            if (parent) {
                var parentId = parent[idField];
                if (status)
                    $(target).treegrid('select', parentId);
                else
                    $(target).treegrid('unselect', parentId);
                selectParent(target, parentId, idField, status);
            }
        }
        /**
         * 级联选择子节点
         * @param {Object} target
         * @param {Object} id 节点ID
         * @param {Object} deepCascade 是否深度级联
         * @param {Object} status 节点状态，true:勾选，false:未勾选
         * @return {TypeName} 
         */
        function selectChildren(target, id, idField, deepCascade, status) {
            //深度级联时先展开节点
            if (!status && deepCascade)
                $(target).treegrid('expand', id);
            //根据ID获取下层孩子节点
            var children = $(target).treegrid('getChildren', id);
            for (var i = 0; i < children.length; i++) {
                var childId = children[i][idField];
                if (status)
                    $(target).treegrid('select', childId);
                else
                    $(target).treegrid('unselect', childId);
                selectChildren(target, childId, idField, deepCascade, status);//递归选择子节点
            }
        }
    }
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////获取url参数
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
QueryString = {
    data: {},
    Initial: function () {
        var aPairs, aTmp;
        var queryString = new String(window.location.search);
        queryString = queryString.substr(1, queryString.length); //remove   "?"     
        aPairs = queryString.split("&");
        for (var i = 0; i < aPairs.length; i++) {
            aTmp = aPairs[i].split("=");
            this.data[aTmp[0]] = aTmp[1];
        }
    },
    GetValue: function (key) {
        return this.data[key];
    }
}
QueryString.Initial();
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////获取url参数
///////////////////////////////////////////////////////////////////////