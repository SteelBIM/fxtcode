//通用列表，用于下拉框、弹出框返回选择数据 kevin
(function ($) {
    $.fn.list = function (args) {
        args = $.extend({
            height: 200, //flexigrid插件的高度，单位为px
            width: 'auto', //宽度值，auto表示根据每列的宽度自动计算
            striped: true, //是否显示斑纹效果，默认是奇偶交互的形式
            novstripe: false,
            minwidth: 30, //列的最小宽度
            minheight: 80, //列的最小高度            
            url: false, //ajax url,ajax方式对应的url地址
            urlType: "api", //默认使用api kevin
            method: 'POST', // data sending method,数据发送方式
            dataType: 'json', // type of data loaded,数据加载的类型，xml,json
            usepager: false, //是否分页
            nowrap: true, //是否不换行
            page: 1, //current page,默认当前页
            total: 1, //total pages,总页面数
            useRp: true, //use the results per page select box,是否可以动态设置每页显示的结果数
            rp: 25, // results per page,每页默认的结果数
            rpOptions: [10, 15, 20, 25, 40, 100], //可选择设定的每页结果数
            title: false, //是否包含标题
            pagestat: '显示记录从{from}到{to}，总数 {total} 条', //显示当前页和总页面的样式
            procmsg: '正在处理数据，请稍候 ...', //正在处理的提示信息
            query: '', //搜索查询的条件
            qtype: '', //搜索查询的类别
            qop: "Eq", //搜索的操作符
            nomsg: '没有符合条件的记录存在', //无结果的提示信息
            minColToggle: 1, //minimum allowed column to be hidden
            showToggleBtn: false, //show or hide column toggle popup
            hideOnSubmit: true, //显示遮盖
            showTableToggleBtn: false, //显示隐藏Grid 
            autoload: true, //自动加载
            blockOpacity: 0.5, //透明度设置
            onToggleCol: false, //当在行之间转换时
            onChangeSort: false, //当改变排序时
            onSuccess: false, //成功后执行
            onSubmit: false, // using a custom populate function,调用自定义的计算函数
            showcheckbox: false, //是否显示checkbox       
            rowdblclick: false, //是否启用行的扩展事情功能
            rowclick: false, //是否启用行的扩展事情功能
            rowbinddata: true,
            extParam: {},
            //Style
            gridClass: "bbit-grid",
            onrowchecked: false
        }, args);
        return this.each(function () {
            var data = args.data;
            var mode = args.mode || "flat"; //显示模式，弹出或者平面
            var mul = args.mul; //选择模式，多选或单选

            var obj = $("<div/>").appendTo($("body"));

            obj.css({ position: "absolute" });
            return this;
        });
    }
})(JQuery)