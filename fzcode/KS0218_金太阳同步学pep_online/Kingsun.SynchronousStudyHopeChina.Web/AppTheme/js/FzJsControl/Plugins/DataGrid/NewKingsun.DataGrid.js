
var Kingsun = Kingsun || {};
Kingsun.DataGrid = function (struct, setting) {
    this.Struct = struct;
    this.TagNames = { Table: "table", Thead: "thead", TBody: "tbody", TR: "tr", TH: "td", TD: "td", A: "a", Input: "input" }; //HTML Tag
    this.Align = { Left: "left", Center: "center", Right: "right" }; //横向对齐方式
    this.TagTypes = { Begin: 0, End: 1 }; //构造Tag的类型(开始或结束)
    this.DataSource = null; //数据源
    this.MaxPageCount = 9999; //显示最大的页数
    this.Page = { PageBoxNum: 8, PageIndex: 1, PageSize: 20, TotalRows: 0, PageCount: 0 }; //分页数据
    this.Options = { //默认配置
        Container: null, //列表HTML容器
        DataFunc: null, //获取数据函数(同步)
        DataSyncFunc: null, //获取数据函数(异步)
        ClassNames: { TableClass: "tbdiv", PageClass: "FontTable font_page", FirstPage: "FirstPage", PrevPage: "PrevPage", NextPage: "NextPage", LastPage: "LastPage" }, //数据列表相关CSS
        Columns: [], //数据列表列配置(TName:列头名称,SName:列数据源名称,Template:自定义生成列方法)
        ColumnsCount: 0,
        IsPage: true, //是否分页
        IsHeader: true, //是否显示表头
        EmptyTip: '<img src="/JsControls/Plugins/DataGrid/i_ico_19.png" /> 没有相应的信息'//无数据提示信息,
    };

    this.Options = $.extend(this.Options, setting); //加载自定义配置
    this.DataBind = function () {//绑定数据
        if (this.Options.Columns == null || this.Options.Columns.length == 0) return;
        if (this.Options.DataFunc != null && typeof this.Options.DataFunc == "function") {
            var data = this.Options.DataFunc();
            this.Page.TotalRows = data.TotalRows;
            this.DataSource = data.DataSource;
            this.Render();
        }
        else if (this.Options.DataSyncFunc != null && typeof this.Options.DataSyncFunc == "function") {
            this.Options.DataSyncFunc();
        }
        else {
            this.Render();
        }
    }

    this.SyncRender = function (data) {
        this.Page.TotalRows = data.TotalRows;
        this.DataSource = data.DataSource;
        this.Render();
    }

    this.Render = function () {//渲染列表
        if (this.DataSource != null && (this.Options.DataFunc != null || this.Options.DataSyncFunc != null)) {
            if (this.DataSource.length == 0 && this.Page.TotalRows > 0 && this.Page.PageIndex > 1) {
                var tempIndex = Math.ceil(this.Page.TotalRows / this.Page.PageSize);
                if (tempIndex > 0) {
                    this.Page.PageIndex = tempIndex;
                } else {
                    this.Page.PageIndex = 1;
                }
                this.DataBind();
                return;
            }
        }

        if (this.Options.ContainerID != undefined) this.Options.Container = document.getElementById(this.Options.ContainerID);
        this.Options.ColumnsCount = 0;
        var tbID = this.Options.ID ? this.Options.ID : this.Options.Container.id + "_datagrid";
        var html = [];
        html.push(this.GetTagHTML({
            tagName: this.TagNames.Table,
            attrs: { id: tbID, className: this.Options.ClassNames.TableClass }
        }));
        if (this.Options.IsHeader) {
            html.push(this.GetTagHTML(this.TagNames.Thead, this.TagTypes.Begin));
            for (var i = 0; i < this.Options.Columns.length; i++) {
                var column = this.Options.Columns[i];
                if (column.Visible === false) continue; //隐藏列不显示
                this.Options.ColumnsCount += 1;
                if (column.Attrs == undefined)
                    html.push(this.GetTagHTML(this.TagNames.TH, this.TagTypes.Begin));
                else
                    html.push(this.GetTagHTML({
                        tagName: this.TagNames.TH,
                        attrs: column.Attrs
                    }));
                var tName;
                if (typeof column.TName == 'function') tName = column.TName();
                else tName = column.TName;
                html.push(tName + this.GetTagHTML(this.TagNames.TH, this.TagTypes.End));
            }
            html.push(this.GetTagHTML(this.TagNames.Thead, this.TagTypes.End));
        }
        html.push(this.GetTagHTML(this.TagNames.TBody, this.TagTypes.Begin));
        if (typeof this.DataSource == 'undefined' || this.DataSource.length == 0) {
            html.push(this.GetTagHTML(this.TagNames.TR, this.TagTypes.Begin));
            html.push(this.GetTagHTML({
                tagName: this.TagNames.TD,
                attrs: { colspan: this.Options.ColumnsCount, align: this.Align.Center }
            }));
            html.push(this.Options.EmptyTip);
            html.push(this.GetTagHTML(this.TagNames.TD, this.TagTypes.End));
            html.push(this.GetTagHTML(this.TagNames.TR, this.TagTypes.End));
        }
        else {
            html.push(this.GetTagHTML(this.TagNames.TR, this.TagTypes.Begin));
            html.push(this.GetTagHTML(this.TagNames.TD, this.TagTypes.Begin));
            for (var i = 0; i < this.DataSource.length; i++) {
                var dataItem = this.DataSource[i]; 
                for (var j = 0; j < this.Options.Columns.length; j++) {
                    var column = this.Options.Columns[j];
                    if (column.Visible === false) continue; //隐藏列不显示
                    
                    if (column.Template != undefined) {
                        dataItem.RowIndex = i;
                        html.push(column.Template(dataItem));
                    }
                    else if (column.SName != undefined) {
                        if (column.SName.length == 0)
                            html.push(Common.HtmlEncode(dataItem));
                        else
                            html.push(Common.HtmlEncode(dataItem[column.SName]));
                    } 
                } 
            }
            html.push(this.GetTagHTML(this.TagNames.TD, this.TagTypes.End));
            html.push(this.GetTagHTML(this.TagNames.TR, this.TagTypes.End));

            if (this.Options.IsPage === true) {
                var pageHTML = this.GetPageHTML();
                html.push(this.GetTagHTML(this.TagNames.TR, this.TagTypes.Begin));
                html.push(this.GetTagHTML({
                    tagName: this.TagNames.TD,
                    attrs: { colspan: this.Options.ColumnsCount }
                }));
                html.push(pageHTML);
                html.push(this.GetTagHTML(this.TagNames.TD, this.TagTypes.End));
                html.push(this.GetTagHTML(this.TagNames.TR, this.TagTypes.End));
            }
        }
        html.push(this.GetTagHTML(this.TagNames.TBody, this.TagTypes.End));
        html.push(this.GetTagHTML(this.TagNames.Table, this.TagTypes.End));
        this.Options.Container.innerHTML = html.join('');
    }
    this.GetPageHTML = function () {//获取分页HTML
        if (this.Page.TotalRows == 0) return "";

        var THIS = this;
        var DefaultHref = "javascript:";
        var emText = "&nbsp;<b></b>", bText = "&nbsp;<em></em>";

        var getEmpty = function (count) { var eHTML = ""; for (var i = 0; i < count; i++) { eHTML += "&nbsp;"; } return eHTML; }
        var getChangePageHTML = function (pageIndex) { return THIS.Struct + ".Changed(" + pageIndex + ")"; }

        this.Page.PageCount = this.GetMaxIndex(this.Page.PageSize, this.Page.TotalRows);
        if (this.Page.PageCount > this.MaxPageCount) this.Page.PageCount = this.MaxPageCount;
        var left = "", right = "";
        var pageHTML = this.GetTagHTML({ tagName: this.TagNames.Table, attrs: { width: "95%", className: this.Options.ClassNames.PageClass} });
        pageHTML += this.GetTagHTML(this.TagNames.TR, this.TagTypes.Begin);
        pageHTML += this.GetTagHTML({ tagName: this.TagNames.TD, attrs: { align: this.Align.Left} });
        pageHTML += "共" + this.Page.TotalRows + " 条" + getEmpty(2) + "每页：" + this.Page.PageSize + " 条" + getEmpty(2) + "页码：" + this.Page.PageIndex + "/" + this.Page.PageCount;
        pageHTML += this.GetTagHTML(this.TagNames.TD, this.TagTypes.End);

        left += this.GetTagHTML({ tagName: this.TagNames.TD, attrs: { align: this.Align.Right} });

        var getRight = function () {
            var rightHTML = "";

            rightHTML += THIS.GetTagHTML({ tagName: THIS.TagNames.A, attrs: { href: DefaultHref + getChangePageHTML(THIS.Page.PageIndex + 1), className: THIS.Options.ClassNames.NextPage} });
            rightHTML += emText;
            rightHTML += THIS.GetTagHTML(THIS.TagNames.A, THIS.TagTypes.End);

            rightHTML += THIS.GetTagHTML({ tagName: THIS.TagNames.A, attrs: { href: DefaultHref + getChangePageHTML(THIS.Page.PageCount), className: THIS.Options.ClassNames.LastPage} });
            rightHTML += bText;
            rightHTML += THIS.GetTagHTML(THIS.TagNames.A, THIS.TagTypes.End);

            return rightHTML;
        }

        var getLeft = function () {
            var leftHTML = "";

            leftHTML += THIS.GetTagHTML({ tagName: THIS.TagNames.A, attrs: { href: DefaultHref + getChangePageHTML(1), className: THIS.Options.ClassNames.FirstPage} });
            leftHTML += bText;
            leftHTML += THIS.GetTagHTML(THIS.TagNames.A, THIS.TagTypes.End);

            leftHTML += THIS.GetTagHTML({ tagName: THIS.TagNames.A, attrs: { href: DefaultHref + getChangePageHTML(THIS.Page.PageIndex - 1), className: THIS.Options.ClassNames.PrevPage} });
            leftHTML += emText;
            leftHTML += THIS.GetTagHTML(THIS.TagNames.A, THIS.TagTypes.End);

            return leftHTML;
        }

        if (this.Page.PageCount <= 1) {
        }
        else if (this.Page.PageIndex == 1) {
            right += getRight();
        }
        else if (this.Page.PageIndex == this.Page.PageCount) {
            left += getLeft();
        }
        else {
            right += getRight();
            left += getLeft();
        }

        pageHTML += left;


        var currentAHTML = function (i) {
            return THIS.GetTagHTML({ tagName: THIS.TagNames.A, attrs: { className: "Current"} }) + i + THIS.GetTagHTML(THIS.TagNames.A, THIS.TagTypes.End);
        }

        var pageAHTML = function (i) {
            return THIS.GetTagHTML({ tagName: THIS.TagNames.A, attrs: { href: DefaultHref + getChangePageHTML(i)} }) + i + THIS.GetTagHTML(THIS.TagNames.A, THIS.TagTypes.End);
        }

        if (this.Page.PageCount > this.Page.PageBoxNum) {
            var pageArea = parseInt(this.Page.PageIndex / this.Page.PageBoxNum);
            if (this.Page.PageIndex % this.Page.PageBoxNum == 0) {
                for (var i = this.Page.PageIndex - this.Page.PageBoxNum + 1; i <= this.Page.PageIndex; i++) {
                    if (i == this.Page.PageIndex) {
                        pageHTML += currentAHTML(i);
                    }
                    else {
                        pageHTML += pageAHTML(i);
                    }
                }
                if (this.Page.PageIndex * this.Page.PageSize < this.Page.TotalRows) {

                }
            }
            else if ((pageArea + 1) * this.Page.PageBoxNum <= this.Page.PageCount) {
                for (var i = pageArea * this.Page.PageBoxNum + 1; i <= (pageArea + 1) * this.Page.PageBoxNum; i++) {
                    if (i == this.Page.PageIndex) {
                        pageHTML += currentAHTML(i);
                    }
                    else {
                        pageHTML += pageAHTML(i);
                    }
                }
            }
            else {
                for (var i = pageArea * this.Page.PageBoxNum + 1; i <= this.Page.PageCount; i++) {
                    if (i == this.Page.PageIndex) {
                        pageHTML += currentAHTML(i);
                    }
                    else {
                        pageHTML += pageAHTML(i);
                    }
                }
            }
        }
        else {
            for (var i = 1; i <= this.Page.PageCount; i++) {
                if (i == this.Page.PageIndex) {
                    pageHTML += currentAHTML(i);
                }
                else {
                    pageHTML += pageAHTML(i);
                }
            }
        }
        pageHTML += right;
        if (this.Page.PageCount > 8) {
            pageHTML += '<input id="page_input" type="text" class="PageInput font12" onkeyup="Common.Number(this,false)" onkeypress="' + this.Struct + '.Goto(this,event)" /><a class="goto" href="javascript:' + this.Struct + '.GO()">GO</a>';
        }
        pageHTML += this.GetTagHTML(this.TagNames.TD, this.TagTypes.End);
        pageHTML += this.GetTagHTML(this.TagNames.TR, this.TagTypes.End);
        pageHTML += this.GetTagHTML(this.TagNames.Table, this.TagTypes.End);
        return pageHTML;
    }
    this.GetTagHTML = function () {//获取TagHTML
        if (arguments.length == 0) return '';
        var opts = arguments[0];
        if (typeof opts == "string" && arguments.length == 2) { //tagName,tagType
            return '<' + (arguments[1] == this.TagTypes.Begin ? '' : '/') + arguments[0] + '>';
        }

        if (typeof opts == "object") {//{tagName:,tagType:,attrs:{}}
            var tagHTML = '<' + opts.tagName;
            if (opts.attrs != undefined) {
                for (var attrName in opts.attrs) {
                    var attrValue = "";
                    var itemAttr = opts.attrs[attrName];
                    if (typeof itemAttr == "function") {
                        if (opts.item)
                            attrValue = Common.HtmlEncode(itemAttr(opts.item));
                    } else {
                        attrValue = Common.HtmlEncode(itemAttr);
                    }

                    if (attrName == "className")
                        tagHTML += ' class="' + attrValue + '"';
                    else
                        tagHTML += ' ' + attrName + '="' + attrValue + '"';
                }
            }
            tagHTML += '>';
            return tagHTML;
        }
    }
    this.GetMaxIndex = function (pageSize, totalRows) {//获取最大页数
        if (totalRows == 0) return 1;
        if (totalRows % pageSize == 0) return totalRows / pageSize;
        if (totalRows < pageSize) return 1;
        return parseInt(totalRows / pageSize) + 1;
    }
    this.Changed = function (pageIndex) {//分页跳转
        if (pageIndex > this.Page.PageCount) pageIndex = 1;
        this.Page.PageIndex = pageIndex;
        this.DataBind();
    }

    this.Goto = function (txt, evt) {
        var e = window.event || evt;
        if (e.keyCode == 13) {
            var pageIndex = parseInt($(txt).val());
            if (isNaN(pageIndex)) {
                return;
            }
            if (pageIndex > 0) {
                this.Changed(pageIndex);
            } else {
                this.Changed(1);
            }
        }
    }

    this.GO = function (txt) {
        var txtid = txt || "#page_input";
        var pageIndex = parseInt($(txtid).val());
        if (isNaN(pageIndex)) {
            return;
        }
        if (pageIndex > 0) {
            this.Changed(pageIndex);
        } else {
            this.Changed(1);
        }
    }
};