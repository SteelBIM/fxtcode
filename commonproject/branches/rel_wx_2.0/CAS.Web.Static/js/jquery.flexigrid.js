
(function ($) {
    $.addFlex = function (t, p) {
        if (t.grid) return false; //如果Grid已经存在则返回
        // 引用默认属性
        p = $.extend({
            height: 200, //flexigrid插件的高度，单位为px
            width: 'auto', //宽度值，auto表示根据每列的宽度自动计算
            striped: true, //是否显示斑纹效果，默认是奇偶交互的形式
            novstripe: false,
            minwidth: 20, //列的最小宽度
            minheight: 80, //列的最小高度
            resizable: false, //resizable table是否可伸缩
            url: false, //ajax url,ajax方式对应的url地址
            urlType: "api", //默认使用api kevin
            localdata: null, //本地数据
            method: 'POST', // data sending method,数据发送方式
            dataType: 'json', // type of data loaded,数据加载的类型，xml,json
            errormsg: '发生错误', //错误提升信息
            usepager: false, //是否分页
            nowrap: true, //是否不换行
            page: 1, //current page,默认当前页
            total: 1, //total pages,总页面数
            useRp: true, //use the results per page select box,是否可以动态设置每页显示的结果数
            rp: 25, // results per page,每页默认的结果数
            rpOptions: [10, 15, 20, 25, 40, 100], //可选择设定的每页结果数
            title: false, //是否包含标题
            pagestat: '{from} 到 {to} 条，总数 {total} 条', //显示当前页和总页面的样式
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
            candragtitle: false, //是否可以拖动标题 kevin
            rowbinddata: false,
            extParam: {},
            //Style
            gridClass: "flexigrid",
            onrowchecked: false,
            opendialog: false,
            contextMenu: null // 右键 kevin
        }, p);

        $(t)
		.show() //show if hidden
		.attr({ cellPadding: 0, cellSpacing: 0, border: 0 })  //remove padding and spacing
		.removeAttr('width') //remove width properties	
		;

        //create grid class
        var g = {
            hset: {},
            rePosDrag: function () {

                var cdleft = 0 - this.hDiv.scrollLeft;
                if (this.hDiv.scrollLeft > 0) cdleft -= Math.floor(p.cgwidth / 2);

                $(g.cDrag).css({ top: g.hDiv.offsetTop + 1 });
                var cdpad = this.cdpad;
                $('div', g.cDrag).hide();
                //update by xuanye ,避免jQuery :visible 无效的bug
                var i = 0;
                $('thead tr:first th:visible', this.hDiv).each(function () {
                    if ($(this).css("display") == "none") {
                        return;
                    }

                    var n = i;
                    //var n = $('thead tr:first th:visible', g.hDiv).index(this);			 	  
                    var cdpos = parseInt($('div', this).width());
                    var ppos = cdpos;
                    if (cdleft == 0)
                        cdleft -= Math.floor(p.cgwidth / 2);

                    cdpos = cdpos + cdleft + cdpad;

                    $('div:eq(' + n + ')', g.cDrag).css({ 'left': cdpos + 'px' }).show();

                    cdleft = cdpos;
                    i++;
                }
				);

            },
            fixHeight: function (newH) {
                newH = false;
                var grid = $(t).parent().parent();
                grid.height(grid.parent().height());
                $(g.bDiv).height(grid.height() - $(g.hDiv).height() - $(g.pDiv).height());
                $(g.noDiv).height($(g.bDiv).height());
                if (!newH) newH = $(g.bDiv).height();
                var hdHeight = $(this.hDiv).height();
                $('div', this.cDrag).each(
						function () {
						    $(this).height(newH + hdHeight);
						}
					);
                $(g.block).css({ height: newH, marginBottom: (newH * -1) });
                var hrH = g.bDiv.offsetTop + newH;
                if (p.height != 'auto' && p.resizable) hrH = g.vDiv.offsetTop;
                $(g.rDiv).css({ height: hrH });

            },
            dragStart: function (dragtype, e, obj) { //default drag function start

                if (dragtype == 'colresize') //column resize
                {
                    var n = $('div', this.cDrag).index(obj);
                    //var ow = $('th:visible div:eq(' + n + ')', this.hDiv).width();
                    var ow = $('th:visible:eq(' + n + ') div', this.hDiv).width();
                    $(obj).addClass('dragging').siblings().hide();
                    $(obj).prev().addClass('dragging').show();

                    this.colresize = { startX: e.pageX, ol: parseInt(obj.style.left), ow: ow, n: n };
                    $('body').css('cursor', 'col-resize');
                }
                else if (dragtype == 'vresize') //table resize
                {
                    var hgo = false;
                    $('body').css('cursor', 'row-resize');
                    if (obj) {
                        hgo = true;
                        $('body').css('cursor', 'col-resize');
                    }
                    this.vresize = { h: p.height, sy: e.pageY, w: p.width, sx: e.pageX, hgo: hgo };

                }

                else if (dragtype == 'colMove' && p.candragtitle) //column header drag
                {
                    this.hset = $(this.hDiv).offset();
                    this.hset.right = this.hset.left + $('table', this.hDiv).width();
                    this.hset.bottom = this.hset.top + $('table', this.hDiv).height();
                    this.dcol = obj;
                    this.dcoln = $('th', this.hDiv).index(obj);

                    this.colCopy = document.createElement("div");
                    this.colCopy.className = "colCopy";
                    this.colCopy.innerHTML = obj.innerHTML;
                    if ($.browser.msie) {
                        this.colCopy.className = "colCopy ie";
                    }


                    $(this.colCopy).css({ position: 'absolute', float: 'left', display: 'none', textAlign: obj.align });
                    $('body').append(this.colCopy);
                    $(this.cDrag).hide();

                }

                $('body').noSelect();

            },
            reSize: function () {
                this.gDiv.style.width = p.width;
                this.bDiv.style.height = p.height;
            },
            dragMove: function (e) {

                if (this.colresize) //column resize
                {
                    var n = this.colresize.n;
                    var diff = e.pageX - this.colresize.startX;
                    var nleft = this.colresize.ol + diff;
                    var nw = this.colresize.ow + diff;
                    if (nw > p.minwidth) {
                        $('div:eq(' + n + ')', this.cDrag).css('left', nleft);
                        this.colresize.nw = nw;
                    }
                }
                else if (this.vresize) //table resize
                {
                    var v = this.vresize;
                    var y = e.pageY;
                    var diff = y - v.sy;
                    if (!p.defwidth) p.defwidth = p.width;
                    if (p.width != 'auto' && !p.nohresize && v.hgo) {
                        var x = e.pageX;
                        var xdiff = x - v.sx;
                        var newW = v.w + xdiff;
                        if (newW > p.defwidth) {
                            this.gDiv.style.width = newW + 'px';
                            p.width = newW;
                        }
                    }
                    var newH = v.h + diff;
                    if ((newH > p.minheight || p.height < p.minheight) && !v.hgo) {
                        this.bDiv.style.height = newH + 'px';
                        p.height = newH;
                        this.fixHeight(newH);
                    }
                    v = null;
                }
                else if (this.colCopy) {
                    $(this.dcol).addClass('thMove').removeClass('thOver');
                    if (e.pageX > this.hset.right || e.pageX < this.hset.left || e.pageY > this.hset.bottom || e.pageY < this.hset.top) {
                        //this.dragEnd();
                        $('body').css('cursor', 'move');
                    }
                    else
                        $('body').css('cursor', 'default');

                    $(this.colCopy).css({ top: e.pageY + 10, left: e.pageX + 20, display: 'block' });
                }

            },
            dragEnd: function () {
                if (this.colresize) {
                    var n = this.colresize.n;
                    var nw = this.colresize.nw;
                    //$('th:visible div:eq(' + n + ')', this.hDiv).css('width', nw);
                    $('th:visible:eq(' + n + ') div', this.hDiv).css('width', nw);

                    $('tr', this.bDiv).each(
									function () {
									    //$('td:visible div:eq(' + n + ')', this).css('width', nw);
									    $('td:visible:eq(' + n + ') div', this).css('width', nw);
									}
								);
                    this.hDiv.scrollLeft = this.bDiv.scrollLeft;
                    $('div:eq(' + n + ')', this.cDrag).siblings().show();
                    $('.dragging', this.cDrag).removeClass('dragging');
                    this.rePosDrag();
                    this.fixHeight();
                    this.colresize = false;
                }
                else if (this.vresize) {
                    this.vresize = false;
                }
                else if (this.colCopy) {
                    $(this.colCopy).remove();
                    if (this.dcolt != null) {
                        if (this.dcoln > this.dcolt)
                        { $('th:eq(' + this.dcolt + ')', this.hDiv).before(this.dcol); }
                        else
                        { $('th:eq(' + this.dcolt + ')', this.hDiv).after(this.dcol); }
                        this.switchCol(this.dcoln, this.dcolt);
                        $(this.cdropleft).remove();
                        $(this.cdropright).remove();
                        this.rePosDrag();
                    }
                    this.dcol = null;
                    this.hset = null;
                    this.dcoln = null;
                    this.dcolt = null;
                    this.colCopy = null;
                    $('.thMove', this.hDiv).removeClass('thMove');
                    $(this.cDrag).show();
                }
                $('body').css('cursor', 'default');
                $('body').noSelect(false);
            },

            scroll: function () {
                this.hDiv.scrollLeft = this.bDiv.scrollLeft;
                this.rePosDrag();
            },
            hideLoading: function () {
                $('.pReload', this.pDiv).removeClass('loading');
                if (p.hideOnSubmit) $(g.block).remove();
                $('.pPageStat', this.pDiv).html(p.errormsg);
                this.loading = false;
            }
            ,
            addData: function (data) { //parse data                            
                if (p.preProcess)
                { data = p.preProcess(data); }
                if (p.usepager) {
                    $('.pReload', this.pDiv).removeClass('loading');
                }
                this.loading = false;

                if (!data) {
                    if (p.usepager) {
                        $('.pPageStat', this.pDiv).html(p.errormsg);
                    }
                    return false;
                }
                //kevin 这里因为更改了JSON返回对象的格式，所以要修改为子项里的DATA
                if (data.returntype != 1) {
                    if (data.returntype == -99)
                        ReLogin();
                    if (CAS.Debug) {
                        log(data);
                    }
                    else {
                        alert(data.returntext);
                    }
                }
                data = data.data;

                var total = 0;
                if (data && data.length > 0) {
                    total = data[0].recordcount;
                }

                var temp = p.total;
                if (p.dataType == 'xml') {
                    p.total = +$('rows total', data).text();
                }
                else {
                    p.total = total;
                }
                if (p.total < 0) {
                    p.total = temp;
                }
                if (p.total == 0) {
                    $('tr, a, td, div', t).unbind();
                    $(t).empty();
                    p.pages = 1;
                    p.page = 1;
                    this.buildpager();
                    $('.pPageStat', this.pDiv).html(p.nomsg);
                    if (p.hideOnSubmit) $(g.block).remove();

                }

                p.pages = Math.ceil(p.total / p.rp);

                if (p.dataType == 'xml')
                { p.page = +$('rows page', data).text(); }
                else
                { p.page = p.newp; }
                if (p.usepager) {
                    this.buildpager();
                }
                p.griddata = [];
                var ths = $('thead tr:first th', g.hDiv);
                var thsdivs = $('thead tr:first th div.griddiv', g.hDiv);
                var tbhtml = [];
                tbhtml.push("<tbody>");

                if (p.dataType == 'json') {
                    if (data != null && data.length > 0) {
                        $(this.noDiv).hide();
                        $(this.bDiv).show();
                        var radioname = CAS.RndVar();
                        $.each(data, function (i, row) {
                            p.griddata.push(row);
                            tbhtml.push("<tr id='", "row", row[p.key], "'");
                            tbhtml.push(" key='", row[p.key], "'");
                            var cls = "";
                            if (p.readclass && p.readclasskey && !row[p.readclasskey]) {
                                cls += " " + p.readclass;
                            }

                            if (i % 2 && p.striped) {
                                cls += " erow";
                            }
                            tbhtml.push(" class='" + cls + "' ");
                            if (p.rowbinddata) {
                                tbhtml.push("ch='", JSON2.stringify(row), "'");
                            }
                            tbhtml.push(">");
                            var trid = row[p.key];
                            $(ths).each(function (j) {
                                var tdclass = "";
                                var align = "center";
                                var col = j;
                                if (j > 0) {
                                    if (p.showcheckbox || p.showradio) col = j - 1;

                                }
                                align = p.colModel[col].align || "center";
                                tbhtml.push("<td align='", align, "'");
                                var idx = $(this).attr('axis').substr(3);

                                if (p.sortname && p.sortname == $(this).attr('abbr')) {
                                    tdclass = 'sorted';
                                }
                                if (this.hide) {
                                    tbhtml.push(" style='display:none;'");
                                }
                                var width = thsdivs[j].style.width;
                                var div = [];
                                div.push("<div style='text-align:", align, ";width:", width, ";");
                                if (p.nowrap == false) {
                                    div.push("white-space:normal");
                                }
                                div.push("'>");
                                if (idx == "-1") { //checkbox radio
                                    if (p.showcheckbox)
                                        div.push("<input type='checkbox' style='visibility:hidden' id='chk_", row[p.key], "' class='itemchk' value='", row[p.key], "'/>");
                                    else if (p.showradio)
                                        div.push("<input type='radio' name='" + radioname + "' style='visibility:hidden' id='chk_", row[p.key], "' class='itemradio' value='", row[p.key], "'/>");
                                    if (tdclass != "") {
                                        tdclass += " chboxtd";
                                    } else {
                                        tdclass += "chboxtd";
                                    }
                                }
                                else {
                                    var text = "";
                                    if (typeof row[idx] != "undefined") {
                                        text = (row[idx] != null) ? row[idx] : ''; //null-check for Opera-browser
                                    } else {
                                        text = row[p.colModel[idx].name];
                                    }
                                    //格式化输出,kevin
                                    if (p.colModel[col].format) {
                                        var format = p.colModel[col].format;
                                        switch (format.type) {
                                            //日期                                                                                                                                                                                                                                                
                                            case "date":
                                                text = CAS.JsonToDate(text, format.text || "yyyy-MM-dd");
                                                break;
                                            //数字                                                      
                                            case "number":
                                                if (parseFloat(text)) {
                                                    if (text <= 0) break;
                                                    text = CAS.CancelCommafy(text);
                                                    if (format.scale) text = CAS.Scale(text, format.scale);
                                                    if (format.afy) text = CAS.Commafy(text);
                                                }
                                                break;
                                            //链接,url使用数组分隔，如果是字段值，用@前缀，在此替换                                                                                                                                                
                                            case "link":
                                                var url = format.url;
                                                var link = [];
                                                var target = "";
                                                for (var ii = 0; ii < url.length; ii++) {
                                                    if (url[ii].indexOf("@") >= 0) {
                                                        link.push(row[url[ii].replace("@", "")]);
                                                    }
                                                    else link.push(url[ii]);
                                                }
                                                if (format.target) {
                                                    target = 'target="' + format.target + '"';
                                                }
                                                text = ['<a href="', link.join(""), '"', target, '>', text, '</a>'].join("");
                                                break;

                                            case "links":
                                                var url = format.url; //获取Url地址以,隔开
                                                var urltext = format.text; //获取连接地址以,隔开
                                                var link = [];
                                                var target = "";
                                                text = "";
                                                if (format.target) {
                                                    target = 'target="' + format.target + '"';
                                                }
                                                for (var ii = 0; ii < url.length; ii++) {
                                                    //                                                    if (url[ii].indexOf("@") >= 0) {
                                                    //                                                        link.push(row[url[ii].replace("@", "")]);
                                                    //                                                    }
                                                    //                                                    else  link.join("")
                                                    //                                                    link.push(url[ii]);
                                                    //                                                    if (urltext[ii].indexOf("undefined") < -1)
                                                    text += ['<a href="', url[ii], '"', target, '>', urltext[ii], '</a> '].join("");
                                                }
                                                break;

                                            case "dialog":
                                                var url = format.url;
                                                var link = [];
                                                for (var ii = 0; ii < url.length; ii++) {
                                                    if (url[ii].indexOf("@") >= 0) {
                                                        link.push(row[url[ii].replace("@", "")]);
                                                    }
                                                    else link.push(url[ii]);
                                                }
                                                text = ['<a href="', link.join(""), '" rel="dialog">', text, '</a>'].join("");
                                                break;
                                            //组合，group使用数组分隔，如果是字段值，用@前缀，在此替换                                                                                                                                               
                                            case "group":
                                                var url = format.group;
                                                var link = [];
                                                for (var ii = 0; ii < url.length; ii++) {
                                                    if (url[ii].indexOf("@") >= 0) {
                                                        link.push(row[url[ii].replace("@", "")]);
                                                    }
                                                    else link.push(url[ii]);
                                                }
                                                text = link.join("");
                                                break;
                                        }
                                        if (format.style) {
                                            text = '<font style="' + format.style + '">' + text + '</font>';
                                        }
                                    }
                                    var divInner = text == null ? "&nbsp;" : text;
                                    text = null;
                                    if (this.process) {
                                        divInner = this.process(divInner, trid);
                                    }

                                    div.push(divInner);
                                    divInner = null;
                                }
                                div.push("</div>");
                                if (tdclass != "") {
                                    tbhtml.push(" class='", tdclass, "'");
                                }
                                tbhtml.push(">", div.join(""), "</td>");
                                div = null;
                            });
                            tbhtml.push("</tr>");
                        }
					    );
                    } else {
                        $(this.noDiv).height($(this.bDiv).height()).show();
                        $(this.bDiv).hide();
                    }
                }
                tbhtml.push("</tbody>");
                $(t).empty().html(tbhtml.join(""));
                tbhtml = null;
                //选择框自定义 kevin
                if (p.showcheckbox) {
                    $("input[ctl=all]", $(this.hDiv)).removeAttr("checked").trigger("change");
                    $(this.hDiv).parent().cascheckbox();
                }
                else if (p.showradio) {
                    $(this.hDiv).parent().cascheckbox({ type: "radio" });
                }
                //this.rePosDrag();
                this.addRowProp();
                if (p.onSuccess) p.onSuccess();
                if (p.hideOnSubmit) $(g.block).remove();
                this.hDiv.scrollLeft = this.bDiv.scrollLeft;
                if ($.browser.opera) $(t).css('visibility', 'visible');
            },
            changeSort: function (th) { //change sortorder

                if (this.loading) return true;

                if (p.sortname == $(th).attr('abbr')) {
                    if (p.sortorder == 'asc') p.sortorder = 'desc';
                    else p.sortorder = 'asc';
                }

                $(th).addClass('sorted').siblings().removeClass('sorted');
                $('.sdesc', this.hDiv).removeClass('sdesc');
                $('.sasc', this.hDiv).removeClass('sasc');
                $('div', th).addClass('s' + p.sortorder);
                p.sortname = $(th).attr('abbr');

                if (p.onChangeSort)
                    p.onChangeSort(p.sortname, p.sortorder);
                else
                    this.populate();

            },
            buildpager: function () { //rebuild pager based on new properties

                $('.pcontrol input', this.pDiv).val(p.page);
                $('.pcontrol span', this.pDiv).html(p.pages);

                var r1 = (p.page - 1) * p.rp + 1;
                var r2 = r1 + p.rp - 1;

                if (p.total < r2) r2 = p.total;

                var stat = p.pagestat;

                stat = stat.replace(/{from}/, r1);
                stat = stat.replace(/{to}/, r2);
                stat = stat.replace(/{total}/, p.total);
                $('.pPageStat', this.pDiv).html(stat);
            },
            populate: function () { //get latest data 
                //log.trace("开始访问数据源");
                if (this.loading) return true;
                if (p.onSubmit) {
                    var gh = p.onSubmit();
                    if (!gh) return false;
                }
                this.loading = true;
                if (!p.url && !p.localdata) return false;
                $('.pPageStat', this.pDiv).html(p.procmsg);
                $('.pReload', this.pDiv).addClass('loading');
                $(g.block).css({ top: g.bDiv.offsetTop });
                if (p.hideOnSubmit) $(this.gDiv).prepend(g.block); //$(t).hide();
                if ($.browser.opera) $(t).css('visibility', 'hidden');
                if (!p.newp) p.newp = 1;
                if (p.page > p.pages) p.page = p.pages;
                var param = {
                    pageindex: p.newp,
                    pagerecords: p.rp,
                    sortname: p.sortname,
                    sortorder: p.sortorder,
                    query: p.query,
                    qtype: p.qtype
                };
                //                var param = [
                //					 { name: 'pageindex', value: p.newp }
                //					, { name: 'pagerecords', value: p.rp }
                //					, { name: 'sortname', value: p.sortname }
                //					, { name: 'sortorder', value: p.sortorder }
                //					, { name: 'query', value: p.query }
                //					, { name: 'qtype', value: p.qtype }
                //					, { name: 'qop', value: p.qop }
                //				];
                //param = jQuery.extend(param, p.extParam);

                if (p.extParam) {
                    //for (var pi = 0; pi < p.extParam.length; pi++) param[param.length] = p.extParam[pi];
                    param = $.extend(param, p.extParam);
                }
                //这里改为可以使用api kevin
                if (p.urlType == "api") {
                    CAS.API({ type: "post", api: p.url, data: param
                        , callback: function (data) {
                            //修复当删除最后一页的所有记录时，页数自动往前翻的bug kevin
                            if (data.data.length == 0 && p.newp > 1) {
                                p.newp--;
                                if (p.usepager) {
                                    $('.pReload', g.pDiv).removeClass('loading');
                                }
                                g.loading = false;
                                g.populate();
                                return;
                            }
                            g.addData(data);
                        }
                    });
                } else if (p.urlType == "localdata") { //加入本地数据列表 kevin 2012-12-12                    
                    var tdata = [];
                    for (i = (p.newp - 1) * p.rp; i < p.newp * p.rp; i++) {
                        if (i >= p.localdata.length) break;
                        p.localdata[i].recordcount = p.localdata.length;
                        tdata.push(p.localdata[i]);
                    }
                    g.addData({ returntype: 1, data: tdata });
                }
                else {
                    $.ajax({
                        type: p.method,
                        url: p.url,
                        data: param,
                        dataType: p.dataType,
                        success: function (data) { g.addData(data); },
                        error: function (data) { alert("获取数据发生异常;"); g.hideLoading(); }
                    });
                }
            },
            doSearch: function () {
                var queryType = $('select[name=qtype]', g.sDiv).val();
                var qArrType = queryType.split("$");
                var index = -1;
                if (qArrType.length != 3) {
                    p.qop = "Eq";
                    p.qtype = queryType;
                }
                else {
                    p.qop = qArrType[1];
                    p.qtype = qArrType[0];
                    index = parseInt(qArrType[2]);
                }
                p.query = $('input[name=q]', g.sDiv).val();
                //添加验证代码
                if (p.query != "" && p.searchitems && index >= 0 && p.searchitems.length > index) {
                    if (p.searchitems[index].reg) {
                        if (!p.searchitems[index].reg.test(p.query)) {
                            alert("你的输入不符合要求!");
                            return;
                        }
                    }
                }
                p.newp = 1;
                this.populate();
            },
            changePage: function (ctype) { //change page

                if (this.loading) return true;

                switch (ctype) {
                    case 'first': p.newp = 1; break;
                    case 'prev': if (p.page > 1) p.newp = parseInt(p.page) - 1; break;
                    case 'next': if (p.page < p.pages) p.newp = parseInt(p.page) + 1; break;
                    case 'last': p.newp = p.pages; break;
                    case 'input':
                        var nv = parseInt($('.pcontrol input', this.pDiv).val());
                        if (isNaN(nv)) nv = 1;
                        if (nv < 1) nv = 1;
                        else if (nv > p.pages) nv = p.pages;
                        $('.pcontrol input', this.pDiv).val(nv);
                        p.newp = nv;
                        break;
                }

                if (p.newp == p.page) return false;

                if (p.onChangePage)
                    p.onChangePage(p.newp);
                else
                    this.populate();

            },
            cellProp: function (n, ptr, pth) {
                var tdDiv = document.createElement('div');
                if (pth != null) {
                    if (p.sortname == $(pth).attr('abbr') && p.sortname) {
                        this.className = 'sorted';
                    }
                    $(tdDiv).css({ textAlign: pth.align, width: $('div:first', pth)[0].style.width });
                    if (pth.hide) $(this).css('display', 'none');
                }
                if (p.nowrap == false) $(tdDiv).css('white-space', 'normal');

                if (this.innerHTML == '') this.innerHTML = '&nbsp;';

                //tdDiv.value = this.innerHTML; //store preprocess value
                tdDiv.innerHTML = this.innerHTML;

                var prnt = $(this).parent()[0];
                var pid = false;
                if (prnt.id) pid = prnt.id.substr(3);
                if (pth != null) {
                    if (pth.process)
                    { pth.process(tdDiv, pid); }
                }

                $(this).empty().append(tdDiv).removeAttr('width'); //wrap content
                //add editable event here 'dblclick',如果需要可编辑在这里添加可编辑代码 
            },
            addCellProp: function () {
                var $gF = this.cellProp;

                $('tbody tr td', g.bDiv).each
					(
						function () {
						    var n = $('td', $(this).parent()).index(this);
						    var pth = $('th:eq(' + n + ')', g.hDiv).get(0);
						    var ptr = $(this).parent();
						    $gF.call(this, n, ptr, pth);
						}
					);
                $gF = null;
            },
            getCheckedRows: function (onlyids) {
                var ids = [];
                $(":checkbox:checked", g.bDiv).each(function () {
                    ids.push($(this).val());
                });
                $(":radio:checked", g.bDiv).each(function () {
                    ids.push($(this).val());
                });
                if (onlyids) {
                    return ids;
                } else {
                    var rtns = [];
                    if (null != p.griddata) {//修复当列表为空时修改出现js错误。by Norman
                        $.each(p.griddata, function (i, row) {
                            if (ids.contains(row[p.key]))
                                rtns.push(row);
                        });
                    }
                    return rtns;
                }
            },
            getCellDim: function (obj) // get cell prop for editable event
            {
                var ht = parseInt($(obj).height());
                var pht = parseInt($(obj).parent().height());
                var wt = parseInt(obj.style.width);
                var pwt = parseInt($(obj).parent().width());
                var top = obj.offsetParent.offsetTop;
                var left = obj.offsetParent.offsetLeft;
                var pdl = parseInt($(obj).css('paddingLeft'));
                var pdt = parseInt($(obj).css('paddingTop'));
                return { ht: ht, wt: wt, top: top, left: left, pdl: pdl, pdt: pdt, pht: pht, pwt: pwt };
            },
            rowProp: function () {

                if ($.browser.msie && $.browser.version < 7.0) {
                    $(this).hover(function () { $(this).addClass('trOver'); }, function () { $(this).removeClass('trOver'); });
                }
            },
            addRowProp: function () {
                var $gF = this.rowProp;
                $('tbody tr', g.bDiv).each(
                    function (i) {

                        $("input.itemchk", this).each(function () {
                            var ptr = $(this).parent().parent().parent();
                            $(this).change(function () {
                                if (this.checked) {
                                    ptr.addClass("trSelected").removeClass("trHighLight");
                                }
                                else {
                                    ptr.removeClass("trSelected");
                                }
                                if (p.onrowchecked) {
                                    p.onrowchecked.call(this);
                                }
                            });
                        });
                        $("input.itemradio", this).each(function () {
                            var ptr = $(this).parent().parent().parent();
                            $(this).change(function () {
                                $('tbody tr', g.bDiv).removeClass("trSelected");
                                if (this.checked) {
                                    ptr.addClass("trSelected");
                                }
                                if (p.onrowchecked) {
                                    p.onrowchecked.call(this);
                                }
                            });
                        });
                        //双击事件 kevin
                        if (p.rowdblclick) {
                            $(this).live("dblclick", function () {
                                if (p.readclass && p.readclasskey && $(this).hasClass(p.readclass)) {
                                    $(this).removeClass(p.readclass);
                                }
                                p.rowdblclick(p.griddata[i], $(this)[0]);
                            });
                        }
                        //单击事件 kevin
                        if (p.rowclick) {
                            var o = this;
                            $(this).live("click", function () { p.rowclick(o, p.griddata[i]); });
                        }
                        $(this).bind("mousedown", function (e) {
                            //用于点击列表中a链接，用弹出层展示A链接的内容
                            var curcss = selcss = "trSelected";
                            if (!p.showradio) curcss = "trHighLight";
                            if (e.button == 2) {
                                if (p.contextMenu != null && p.contextMenu.length > 0) {
                                    if ($(this).hasClass(selcss)) {

                                    } else {
                                        if (p.showradio) {
                                            $("input.itemradio", $(this)).trigger("mousedown");
                                        }
                                        if (p.showcheckbox) {
                                            g.checkAll(false);
                                            $("input.itemchk", $(this)).attr("checked", true).trigger("change");
                                        }
                                    }
                                    return;
                                }
                            };
                            var tar = e.target,
                            tag = tar.tagName;
                            if (tag === 'A' && $(tar).attr("rel") == "dialog") {
                                if (e.preventDefault) {
                                    e.preventDefault();
                                } else {
                                    e.returnValue = false;
                                }
                                p.opendialog(p.griddata[i]);
                                if (e && e.stopPropagation)
                                    e.stopPropagation();
                                else
                                    window.event.cancelBubble = false;
                            } else {
                                var o = this;
                                if ($(o).hasClass(selcss)) {
                                    if ($(o).hasClass(curcss)) $(o).removeClass("trHighLight");
                                    return;
                                }
                                $(o).selectOnly(curcss);
                                if (p.showradio) {
                                    $("input.itemradio", o).trigger("click");
                                }
                            }
                        });

                        if (p.contextMenu != null && p.contextMenu.length > 0) {
                            //添加右键 kevin
                            $.each(p.contextMenu, function (i, menu) {
                                switch (menu.text) {
                                    case "all":
                                        p.contextMenu[i] = $.extend({}, { text: '全选', icon: "iconcheckall", callback: function () { g.checkAll(true); } });
                                        break;
                                    case "allnot":
                                        p.contextMenu[i] = $.extend({}, { text: '全不选', icon: "iconcheckallnot", callback: function () { g.checkAll(false); } });
                                        break;
                                }
                            });
                            $(this).cascontextmenu({ menus: p.contextMenu });
                        }
                        $gF.call(this);
                    }
                );
                $gF = null;
            },
            checkAll: function (ischeck) {
                $("input[ctl=all]", g.hDiv).attr("checked", ischeck).trigger("change");
            },
            checkAllOrNot: function (chk) {
                var ischeck = chk.attr("checked");
                $("input.itemchk", g.bDiv).each(function () {
                    this.checked = ischeck;
                    $(this).trigger("change");
                });
            },
            getRowCount: function () {
                return $("tr", g.bDiv).size();
            },
            pager: 0
        };

        //create model if any
        if (p.colModel) {
            thead = document.createElement('thead');
            tr = document.createElement('tr');
            //p.showcheckbox ==true;
            if (p.showcheckbox) {
                var cth = jQuery('<th align="center"/>');
                var cthch = jQuery('<input type="checkbox" style="visibility:hidden" ctl="all"/>');
                cthch.addClass("noborder");
                cth.addClass("cth").attr({ 'axis': "col-1", width: "20", "isch": true }).append(cthch);
                $(tr).append(cth);
            }
            else if (p.showradio) {
                var cth = jQuery('<th/>');
                cth.addClass("cth").attr({ 'axis': "col-1", width: "20", "isch": true });
                $(tr).append(cth);
            }
            for (i = 0; i < p.colModel.length; i++) {
                var cm = p.colModel[i];
                var th = document.createElement('th');

                th.innerHTML = cm.display;

                if (cm.name && cm.sortable)
                    $(th).attr('abbr', cm.name);

                //th.idx = i;
                $(th).attr('axis', 'col' + i);

                //if (cm.align)
                //    th.align = cm.align;
                th.align = "center";
//                $(th).attr('width', '300');
//                alert($(th).attr('width'));
                if (cm.width) //列宽处处理
                {
                    //                    var aa = cm.width + "";
                    //                    aa = new Number(aa.substring(0, 2)); //去掉百分号啊  
                    //                    //                  alert(aa);  
                    //                    //                  alert(new Number(p.width));  
                    //                    //                  p.width:为你配置的表格宽度  
                    //                    //为什么-100,自己慢慢试出来的-_-!  
                    //                    aa = (aa / 100).toFixed(3) * (new Number(p.width) - 100);
                    //                    $(th).attr('width', aa);
                    $(th).attr('width', cm.width);
                }

                if (cm.hide) {
                    th.hide = true;
                }
                if (cm.toggle != undefined) {
                    th.toggle = cm.toggle;
                }
                if (cm.process) {
                    th.process = cm.process;
                }

                $(tr).append(th);
            }
            $(thead).append(tr);
            $(t).prepend(thead);
        } // end if p.colmodel	

        //init divs
        g.gDiv = document.createElement('div'); //create global container
        g.mDiv = document.createElement('div'); //create title container
        g.hDiv = document.createElement('div'); //create header container
        g.bDiv = document.createElement('div'); //create body container
        g.vDiv = document.createElement('div'); //create grip
        g.rDiv = document.createElement('div'); //create horizontal resizer
        g.cDrag = document.createElement('div'); //create column drag
        g.block = document.createElement('div'); //creat blocker
        g.iDiv = document.createElement('div'); //create editable layer
        g.tDiv = document.createElement('div'); //create toolbar
        g.sDiv = document.createElement('div');

        g.noDiv = document.createElement('div'); //没有数据时使用

        if (p.usepager) g.pDiv = document.createElement('div'); //create pager container
        g.hTable = document.createElement('table');

        //set gDiv
        g.gDiv.className = p.gridClass;

        function fixH() {
            setTimeout(function () {
                g.fixHeight();
                $(g.pDiv).show();
            }, 10);
        }
        fixH();
        //if (window.frameElement) {
        //    window.frameElement.onresize = fixH;
        //}
        //else {
        $(window).bind("resize", fixH);
        //}

        if (p.width != 'auto') g.gDiv.style.width = p.width + 'px';

        //add conditional classes
        if ($.browser.msie)
            $(g.gDiv).addClass('ie');

        if (p.novstripe)
            $(g.gDiv).addClass('novstripe');

        $(t).before(g.gDiv);
        $(g.gDiv).append(t);
        //set toolbar
        if (p.buttons) {
            g.tDiv.className = 'tDiv';
            var tDiv2 = document.createElement('div');
            tDiv2.className = 'tDiv2';

            for (i = 0; i < p.buttons.length; i++) {
                var btn = p.buttons[i];
                if (!btn.separator) {
                    var btnDiv = document.createElement('div');
                    btnDiv.className = 'fbutton';
                    btnDiv.innerHTML = ("<div><span>") + (btn.hidename ? "&nbsp;" : btn.name) + ("</span></div>");
                    if (btn.bclass) $('span', btnDiv).addClass(btn.bclass).css({
                        paddingLeft: 20
                    });
                    if (btn.bimage) // if bimage defined, use its string as an image url for this buttons style (RS)
                        $('span', btnDiv).css('background', 'url(' + btn.bimage + ') no-repeat center left');
                    $('span', btnDiv).css('paddingLeft', 20);

                    if (btn.tooltip) // add title if exists (RS)
                        $('span', btnDiv)[0].title = btn.tooltip;

                    btnDiv.onpress = btn.onpress;
                    btnDiv.name = btn.name;
                    if (btn.id) {
                        btnDiv.id = btn.id;
                    }
                    if (btn.onpress) {
                        $(btnDiv).click(function () {
                            this.onpress(this.id || this.name, g.gDiv);
                        });
                    }
                    $(tDiv2).append(btnDiv);
                    if ($.browser.msie && $.browser.version < 7.0) {
                        $(btnDiv).hover(function () {
                            $(this).addClass('fbOver');
                        }, function () {
                            $(this).removeClass('fbOver');
                        });
                    }

                } else {
                    $(tDiv2).append("<div class='btnseparator'></div>");
                }
            }
            $(g.tDiv).append(tDiv2);
            $(g.tDiv).append("<div style='clear:both'></div>");
            $(g.gDiv).prepend(g.tDiv);
        }

        //set hDiv
        g.hDiv.className = 'hDiv';

        $(t).before(g.hDiv);

        //set hTable
        g.hTable.cellPadding = 0;
        g.hTable.cellSpacing = 0;
        $(g.hDiv).append('<div class="hDivBox"></div>');
        $('div', g.hDiv).append(g.hTable);
        var thead = $("thead:first", t).get(0);
        if (thead) $(g.hTable).append(thead);
        thead = null;

        if (!p.colmodel) var ci = 0;

        //setup thead			
        $('thead tr:first th', g.hDiv).each
			(
			 	function () {
			 	    var thdiv = document.createElement('div');

			 	    if ($(this).attr('abbr')) {
			 	        $(this).click(function (e) {

			 	            if (!$(this).hasClass('thOver')) return false;
			 	            var obj = (e.target || e.srcElement);
			 	            if (obj.href || obj.type) return true;
			 	            g.changeSort(this);
			 	        });
			 	        if ($(this).attr('abbr') == p.sortname) {
			 	            this.className = 'sorted';
			 	            thdiv.className = 's' + p.sortorder;
			 	        }
			 	    }
			 	    $(thdiv).addClass("griddiv");
			 	    if (this.hide) $(this).hide();

			 	    if (!p.colmodel && !$(this).attr("isch")) {
			 	        $(this).attr('axis', 'col' + ci++);
			 	    }

			 	    //控制百分比
			 	    var Pixel = "";
			 	    if (this.width.indexOf("px") > 0) {
			 	        Pixel = "px";
			 	    }
			 	    else if (this.width.indexOf("%") > 0) {
			 	        Pixel = "%";
			 	    }
			 	    else {
			 	        Pixel = "px";
			 	    }
			 	    $(thdiv).css({ textAlign: this.align, width: this.width + Pixel });
			 	    thdiv.innerHTML = this.innerHTML;

			 	    $(this).empty().append(thdiv).removeAttr('width');
			 	    if (!$(this).attr("isch")) {
			 	        $(this).mousedown(function (e) {
			 	            g.dragStart('colMove', e, this);
			 	        })
						.hover(
							function () {
							    if (!g.colresize && !$(this).hasClass('thMove') && !g.colCopy) $(this).addClass('thOver');

							    if ($(this).attr('abbr') != p.sortname && !g.colCopy && !g.colresize && $(this).attr('abbr')) $('div', this).addClass('s' + p.sortorder);
							    else if ($(this).attr('abbr') == p.sortname && !g.colCopy && !g.colresize && $(this).attr('abbr')) {
							        var no = '';
							        if (p.sortorder == 'asc') no = 'desc';
							        else no = 'asc';
							        $('div', this).removeClass('s' + p.sortorder).addClass('s' + no);
							    }

							    if (g.colCopy) {

							        var n = $('th', g.hDiv).index(this);

							        if (n == g.dcoln) return false;



							        if (n < g.dcoln) $(this).append(g.cdropleft);
							        else $(this).append(g.cdropright);

							        g.dcolt = n;

							    } else if (!g.colresize) {
							        var thsa = $('th:visible', g.hDiv);
							        var nv = -1;
							        for (var i = 0, j = 0, l = thsa.length; i < l; i++) {
							            if ($(thsa[i]).css("display") != "none") {
							                if (thsa[i] == this) {
							                    nv = j;
							                    break;
							                }
							                j++;
							            }
							        }


							    }

							},
							function () {
							    $(this).removeClass('thOver');
							    if ($(this).attr('abbr') != p.sortname) $('div', this).removeClass('s' + p.sortorder);
							    else if ($(this).attr('abbr') == p.sortname) {
							        var no = '';
							        if (p.sortorder == 'asc') no = 'desc';
							        else no = 'asc';

							        $('div', this).addClass('s' + p.sortorder).removeClass('s' + no);
							    }
							    if (g.colCopy) {
							        $(g.cdropleft).remove();
							        $(g.cdropright).remove();
							        g.dcolt = null;
							    }
							})
						; //wrap content
			 	    }
			 	}
			);

        //set bDiv
        g.bDiv.className = 'bDiv';
        $(t).before(g.bDiv);
        $(g.bDiv)
		.css({ height: (p.height == 'auto') ? 'auto' : p.height + "px" })
		.scroll(function (e) { g.scroll(); })
		.append(t)
		;

        if (p.height == 'auto') {
            $('table', g.bDiv).addClass('autoht');
        }

        //add td properties
        if (p.url == false || p.url == "") {
            g.addCellProp();
            //add row properties
            g.addRowProp();
        }

        //set cDrag

        var cdcol = $('thead tr:first th:first', g.hDiv).get(0);

        if (cdcol != null) {
            g.cDrag.className = 'cDrag';
            g.cdpad = 0;

            g.cdpad += (isNaN(parseInt($('div', cdcol).css('borderLeftWidth'))) ? 0 : parseInt($('div', cdcol).css('borderLeftWidth')));
            g.cdpad += (isNaN(parseInt($('div', cdcol).css('borderRightWidth'))) ? 0 : parseInt($('div', cdcol).css('borderRightWidth')));
            g.cdpad += (isNaN(parseInt($('div', cdcol).css('paddingLeft'))) ? 0 : parseInt($('div', cdcol).css('paddingLeft')));
            g.cdpad += (isNaN(parseInt($('div', cdcol).css('paddingRight'))) ? 0 : parseInt($('div', cdcol).css('paddingRight')));
            g.cdpad += (isNaN(parseInt($(cdcol).css('borderLeftWidth'))) ? 0 : parseInt($(cdcol).css('borderLeftWidth')));
            g.cdpad += (isNaN(parseInt($(cdcol).css('borderRightWidth'))) ? 0 : parseInt($(cdcol).css('borderRightWidth')));
            g.cdpad += (isNaN(parseInt($(cdcol).css('paddingLeft'))) ? 0 : parseInt($(cdcol).css('paddingLeft')));
            g.cdpad += (isNaN(parseInt($(cdcol).css('paddingRight'))) ? 0 : parseInt($(cdcol).css('paddingRight')));

            $(g.bDiv).before(g.cDrag);
            $(g.noDiv).insertBefore(g.bDiv);
            $(g.noDiv).addClass("noDiv w100p").html("<table class='w100p h100p'><tr><td class='tc tm fw f20 h100p'>很抱歉，没有找到满足条件的数据...</td></tr></table>").hide();

            var cdheight = $(g.bDiv).height();
            var hdheight = $(g.hDiv).height();

            $(g.cDrag).css({ top: -hdheight + 'px' });

            $('thead tr:first th', g.hDiv).each
			(
			 	function () {
			 	    var cgDiv = document.createElement('div');
			 	    $(g.cDrag).append(cgDiv);
			 	    if (!p.cgwidth) p.cgwidth = $(cgDiv).width();
			 	    $(cgDiv).css({ height: cdheight + hdheight })
					.mousedown(function (e) { g.dragStart('colresize', e, this); })
					;
			 	    if ($.browser.msie && $.browser.version < 7.0) {
			 	        g.fixHeight($(g.gDiv).height());
			 	        $(cgDiv).hover(
							function () {
							    g.fixHeight();
							    $(this).addClass('dragging');
							},
							function () { if (!g.colresize) $(this).removeClass('dragging'); }
						);
			 	    }
			 	}
			);

            //g.rePosDrag();

        }


        //add strip		
        if (p.striped)
            $('tbody tr:odd', g.bDiv).addClass('erow');

        if (p.resizable && p.height != 'auto') {
            g.vDiv.className = 'vGrip';
            $(g.vDiv)
		.mousedown(function (e) { g.dragStart('vresize', e); })
		.html('<span></span>');
            $(g.bDiv).after(g.vDiv);
        }

        if (p.resizable && p.width != 'auto' && !p.nohresize) {
            g.rDiv.className = 'hGrip';
            $(g.rDiv)
		.mousedown(function (e) { g.dragStart('vresize', e, true); })
		.html('<span></span>')
		.css('height', $(g.gDiv).height())
		;
            if ($.browser.msie && $.browser.version < 7.0) {
                $(g.rDiv).hover(function () { $(this).addClass('hgOver'); }, function () { $(this).removeClass('hgOver'); });
            }
            $(g.gDiv).append(g.rDiv);
        }

        // add pager
        if (p.usepager) {
            g.pDiv.className = 'pDiv dn';
            g.pDiv.innerHTML = '<div class="pDiv2"></div>';
            $(g.bDiv).after(g.pDiv);
            var html = '<div class="pGroup"><div class="pFirst pButton" title="转到第一页"><span></span></div><div class="pPrev pButton" title="转到上一页"><span></span></div> </div><div class="btnseparator"></div> <div class="pGroup"><span class="pcontrol tm">当前第 <input type="text" value="1" class="pPageInput w30 tm" /> 页,总页数 <span> 1 </span></span></div><div class="btnseparator"></div><div class="pGroup"> <div class="pNext pButton" title="转到下一页"><span></span></div><div class="pLast pButton" title="转到最后一页"><span></span></div></div><div class="btnseparator"></div><div class="pGroup"> <div class="pReload pButton" title="刷新"><span></span></div> </div> <div class="btnseparator"></div><div class="pGroup"><span class="pPageStat"></span></div>';
            $('div', g.pDiv).html(html);
            $('.pReload', g.pDiv).click(function () { g.populate(); });
            $('.pFirst', g.pDiv).click(function () { g.changePage('first'); });
            $('.pPrev', g.pDiv).click(function () { g.changePage('prev'); });
            $('.pNext', g.pDiv).click(function () { g.changePage('next'); });
            $('.pLast', g.pDiv).click(function () { g.changePage('last'); });
            $('.pcontrol input', g.pDiv).keydown(function (e) { if (e.keyCode == 13) g.changePage('input'); });
            if ($.browser.msie && $.browser.version < 7) $('.pButton', g.pDiv).hover(function () { $(this).addClass('pBtnOver'); }, function () { $(this).removeClass('pBtnOver'); });

            //if (p.useRp) {
            var opt = "";
            for (var nx = 0; nx < p.rpOptions.length; nx++) {
                if (p.rp == p.rpOptions[nx]) sel = 'selected="selected"'; else sel = '';
                opt += "<option value='" + p.rpOptions[nx] + "' " + sel + " >" + p.rpOptions[nx] + "&nbsp;&nbsp;</option>";
            };
            $('.pDiv2', g.pDiv).prepend("<div class='pGroup'>每页 <select name='rp' class='w35'>" + opt + "</select> 条</div> <div class='btnseparator'></div>");
            $('select', g.pDiv).change(
					function () {
					    if (p.onRpChange)
					        p.onRpChange(+this.value);
					    else {
					        p.newp = 1;
					        p.rp = +this.value;
					        g.populate();
					    }
					}
				);
            //}

            //add search button
            if (p.searchitems) {
                $('.pDiv2', g.pDiv).prepend("<div class='pGroup'> <div class='pSearch pButton'><span></span></div> </div>  <div class='btnseparator'></div>");
                $('.pSearch', g.pDiv).click(function () { $(g.sDiv).slideToggle('fast', function () { $('.sDiv:visible input:first', g.gDiv).trigger('focus'); }); });
                //add search box
                g.sDiv.className = 'sDiv';

                sitems = p.searchitems;

                var sopt = "";
                var op = "Eq";
                for (var s = 0; s < sitems.length; s++) {
                    if (p.qtype == '' && sitems[s].isdefault == true) {
                        p.qtype = sitems[s].name;
                        sel = 'selected="selected"';
                    } else sel = '';
                    if (sitems[s].operater == "Like") {
                        op = "Like";
                    }
                    else {
                        op = "Eq";
                    }
                    sopt += "<option value='" + sitems[s].name + "$" + op + "$" + s + "' " + sel + " >" + sitems[s].display + "&nbsp;&nbsp;</option>";
                }

                if (p.qtype == '') p.qtype = sitems[0].name;

                $(g.sDiv).append("<div class='sDiv2'>快速检索：<input type='text' size='30' name='q' class='qsbox' /> <select name='qtype'>" + sopt + "</select> <input type='button' name='qclearbtn' value='清空' /></div>");

                $('input[name=q],select[name=qtype]', g.sDiv).keydown(function (e) { if (e.keyCode == 13) g.doSearch(); });
                $('input[name=qclearbtn]', g.sDiv).click(function () { $('input[name=q]', g.sDiv).val(''); p.query = ''; g.doSearch(); });
                $(g.bDiv).after(g.sDiv);

            }

        }
        $(g.pDiv, g.sDiv).append("<div style='clear:both'></div>");

        // add title
        if (p.title) {
            g.mDiv.className = 'mDiv';
            g.mDiv.innerHTML = '<div class="ftitle">' + p.title + '</div>';
            $(g.gDiv).prepend(g.mDiv);
            if (p.showTableToggleBtn) {
                $(g.mDiv).append('<div class="ptogtitle" title="Minimize/Maximize Table"><span></span></div>');
                $('div.ptogtitle', g.mDiv).click
					(
					 	function () {
					 	    $(g.gDiv).toggleClass('hideBody');
					 	    $(this).toggleClass('vsble');
					 	}
					);
            }
            //g.rePosDrag();
        }

        //setup cdrops
        g.cdropleft = document.createElement('span');
        g.cdropleft.className = 'cdropleft';
        g.cdropright = document.createElement('span');
        g.cdropright.className = 'cdropright';

        //add block
        g.block.className = 'gBlock';
        var blockloading = $("<div/>");
        blockloading.addClass("loading");
        $(g.block).append(blockloading);
        var gh = $(g.bDiv).height();
        var gtop = g.bDiv.offsetTop;
        $(g.block).css(
		{
		    width: g.bDiv.style.width,
		    height: gh,
		    position: 'relative',
		    marginBottom: (gh * -1),
		    zIndex: 1,
		    top: gtop,
		    left: '0px'
		}
		);
        //内存会泄漏?
        $(g.block).fadeTo(0, p.blockOpacity);

        // add column control
        if ($('th', g.hDiv).length) {

            var cn = 0;


            $('th div', g.hDiv).each
			(
			 	function () {
			 	    var kcol = $("th[axis='col" + cn + "']", g.hDiv)[0];
			 	    if (kcol == null) return;
			 	    var chkall = $("input[type='checkbox']", this);
			 	    if (chkall.length > 0) {
			 	        chkall.bind("change", function () { g.checkAllOrNot(chkall) });
			 	        return;
			 	    }
			 	    if (kcol.toggle == false || this.innerHTML == "") {
			 	        cn++;
			 	        return;
			 	    }
			 	    var chk = 'checked="checked"';
			 	    if (kcol.style.display == 'none') chk = '';

			 	    //$('tbody', g.nDiv).append('<tr><td class="ndcol1"><input type="checkbox" ' + chk + ' class="togCol noborder" value="' + cn + '" /></td><td class="ndcol2">' + this.innerHTML + '</td></tr>');
			 	    cn++;
			 	}
			);
            //UI初始化 kevin
            $(g.pDiv).casinput();
            $("input.pPageInput", g.pDiv).casnumber();
            $("select", $(g.pDiv)).casselect();

        }

        // add date edit layer
        $(g.iDiv)
		.addClass('iDiv')
		.css({ display: 'none' })
		;
        $(g.bDiv).append(g.iDiv);

        // add flexigrid events

        //add document events
        $(document)
		.mousemove(function (e) { g.dragMove(e); })
		.mouseup(function (e) { g.dragEnd(); })
		.hover(function () { }, function () { g.dragEnd(); })
		;
        //browser adjustments
        if (CAS.isIE6()) {
            $('.hDiv,.bDiv,.mDiv,.pDiv,.vGrip,.tDiv, .sDiv', g.gDiv)
			.css({ width: '100%' });
            $(g.gDiv).addClass('ie6');
            if (p.width != 'auto') $(g.gDiv).addClass('ie6fullwidthbug');
        }
        //扩展上下键 kevin
        $(g.bDiv).bind("keydown", function (e) {
            var selrow;
            if (!p.showradio) selrow = $("tr.trHighLight", $(g.bDiv));
            else selrow = $("tr.trSelected", $(g.bDiv));
            if (selrow.size() > 0) selrow = selrow.eq(0);
            else selrow = null;
            var currow = null;
            switch (e.keyCode) {

                case CAS.KeyCode.DOWN:
                    if (!selrow || (selrow && selrow.next().size() == 0)) {
                        currow = $("tr:first", $(g.bDiv)).trigger("mousedown");
                    } else {
                        currow = selrow.next().trigger("mousedown");
                    }
                    return setscroll();
                    break;
                case CAS.KeyCode.UP:
                    if (!selrow || (selrow && selrow.prev().size() == 0)) {
                        currow = $("tr:last", $(g.bDiv)).trigger("mousedown");
                    } else {
                        currow = selrow.prev().trigger("mousedown");
                    }
                    return setscroll();
                    break;
                case CAS.KeyCode.ENTER:
                    //单击事件 kevin
                    if (p.rowdblclick && selrow) {
                        p.rowdblclick(p.griddata[selrow[0].rowIndex]);
                    }
                    break;
            }
            function setscroll() {
                var top = currow.offset().top - currow.parent().offset().top;
                if (top + currow.height() >= $(g.bDiv).height()) {
                    $(g.bDiv)[0].scrollTop += currow.parent().height();
                } else { $(g.bDiv)[0].scrollTop = 0; }
                e.stopPropagation();
                return false;
            }
            //BUG #817 kevin
            CAS.stopPropagation(e);
            return false;
        });

        g.rePosDrag();

        //make grid functions accessible
        t.p = p;
        t.grid = g;

        // load data
        if (p.url || p.localdata) {
            g.populate();
        }
        return t;

    };

    var docloaded = false;

    $(document).ready(function () { docloaded = true });

    $.fn.flexigrid = function (p) {

        return this.each(function () {
            if (!docloaded) {
                $(this).hide();
                var t = this;
                $(document).ready
					(
						function () {
						    $.addFlex(t, p);
						}
					);
            } else {
                $.addFlex(this, p);
            }
        });

    }; //end flexigrid

    $.fn.flexReload = function (p) { // function to reload grid
        return this.each(function () {
            if (this.grid && (this.p.url || this.p.localdata)) this.grid.populate();
        });

    }; //end flexReload
    //重新指定宽度和高度
    $.fn.flexResize = function (w, h) {
        var p = { width: w, height: h };
        return this.each(function () {
            if (this.grid) {
                $.extend(this.p, p);
                this.grid.reSize();
            }
        });
    };
    $.fn.ChangePage = function (type) {
        return this.each(function () {
            if (this.grid) {
                this.grid.changePage(type);
            }
        })
    };
    $.fn.flexOptions = function (p) { //function to update general options
        return this.each(function () {
            if (this.grid) $.extend(true, this.p, p);
        });

    }; //end flexOptions
    $.fn.GetOptions = function () {
        if (this[0].grid) {
            return this[0].p;
        }
        return null;
    };
    $.fn.getCheckedRows = function () {
        if (this[0].grid) {
            return this[0].grid.getCheckedRows();
        }
        return [];
    };
    $.fn.flexToggleCol = function (cid, visible) { // function to reload grid

        return this.each(function () {
            if (this.grid) this.grid.toggleCol(cid, visible);
        });

    }; //end flexToggleCol

    $.fn.flexAddData = function (data) { // function to add data to grid

        return this.each(function () {
            if (this.grid) this.grid.addData(data);
        });

    };

    $.fn.noSelect = function (p) { //no select plugin by me :-)
        if (p == null)
            prevent = true;
        else
            prevent = p;

        if (prevent) {

            return this.each(function () {
                if ($.browser.msie || $.browser.safari) $(this).bind('selectstart', function () { return false; });
                else if ($.browser.mozilla) {
                    $(this).css('MozUserSelect', 'none');
                    $('body').trigger('focus');
                }
                else if ($.browser.opera) $(this).bind('mousedown', function () { return false; });
                else $(this).attr('unselectable', 'on');
            });

        } else {


            return this.each(function () {
                if ($.browser.msie || $.browser.safari) $(this).unbind('selectstart');
                else if ($.browser.mozilla) $(this).css('MozUserSelect', 'inherit');
                else if ($.browser.opera) $(this).unbind('mousedown');
                else $(this).removeAttr('unselectable', 'on');
            });

        };

    }; //end noSelect

    //我们自己用的扩展 kevin
    $.fn.flexCheckAll = function (ischeck) {
        return this.each(function () {
            if (this.grid) this.grid.checkAll(ischeck);
        });
    }
    $.fn.flexSearch = function (args) {
        return this.flexOptions({
            newp: 1,
            extParam: args
        }).flexReload();
    }
    //修改行，需要定义rowdblclick
    $.fn.flexEditRow = function (args) {
        var $this = this;
        args = $.extend({}, { content: "请选择要修改的项" }, args);
        var sels = $this.flexSelectRow(args);
        if (sels) $this[0].p.rowdblclick(sels[0]);
    };

    //是否有选择行
    $.fn.flexSelectRow = function (args) {
        args = $.extend({}, { content: "请选择要操作的项" }, args);
        var $this = this;
        var sels = $this.getCheckedRows();
        if (sels.length <= 0) {
            CAS.Alert(args.content);
            return false;
        }
        return sels;
    };
    //删除行
    $.fn.flexDeleteRow = function (args) {
        if (!args.content)
            args.content = "请选择要删除的项";
        if (!args.confirmcontent)
            args.confirmcontent = "确定要删除选中的项吗？";
        var $this = this;
        var sels = $this.flexSelectRow(args);
        if (sels) {
            if (sels.length == 1)
                args.data[args.key] = sels[0][args.key];
            else {
                var ids = [];
                $.each(sels, function (i, row) {
                    ids.push(row[args.key]);
                });
                args.data[args.key] = ids.join(",");
            }
            function returnVal() {
                var pro = CAS.Progress("正在处理，请稍候...");
                CAS.API({ type: "post", api: args.api, data: args.data, callback: function (data) {
                    pro.close();
                    $this.flexReload();
                }
                });
            }
            CAS.Confirm({ content: args.confirmcontent, callback: returnVal });
        }
    }

})(jQuery);