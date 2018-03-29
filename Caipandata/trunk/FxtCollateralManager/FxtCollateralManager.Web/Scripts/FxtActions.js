/**
* 作者:李晓东
* 摘要:2013.12.31 新建
*  修改人：李晓东 2014.01.02 修改ActionForm的验证列表
                             把FxtUsers.js更改成FxtActions.js
                             新增ActionOperta,修改ActionList,且加上分页
   修改人：李晓东 2014.01.03 ActionOperta的修改,做到其共用
   修改人:李晓东 2014.01.20 新增ActionNext 新增用户后可直接赋予权限,也可直接修改查看 BindingSelect稍作修改
   修改人:李晓东 2014.01.22 ActionFrom稍作修改
   修改人:李晓东 2014.01.24 ActionFrom不满足需要及bug的出现修复进行整理
   修改人:李晓东 2014.01.28 ActionNext,ActionSetForm,BindingSelect因需求和整理做出改动
   修改人:李晓东 2014.02.25 删除:ActionPage
   修改人:李晓东 2014.02.27 删除:ActionSetForm
   修改人:李晓东 2014.03.06 删除:ActionList,ActionOperta
   修改人:李晓东 2014.03.07 删除:LoadDialogClose,ActionFrom
* **/


(function ($) {
    $.extend({        
        ActionSelectList: [],
        ActionListDetailsArray: [],
        mainContent: $("#main_content"),
        ActionNext: function (userid) {
            $.ClearCookie();
            $.extendAjax({
                url: '/Users/ProductList',
                type: 'POST'
            }, function (rdata) {
                                
                /*弹出权限窗口*/
                $.Opertaings({
                    ActionType: 'U',
                    title: '权限',
                    AOoptions: {
                        ActionFormId: '#formpurview',
                        TmplId: '#template-formpurview',
                        ActionUrl: '/Users/GetUserPurview/?userid={0}'.format(userid),
                        Url: '/Users/UserPurview/',
                        ActionFormRules: $.userrules,
                        ReloadUrl: '/Users/Index'
                    },
                    callSetForm:false,
                    complete: function (calldata) {
                        /*设置用户ID*/
                        $('input[name=userid]').val(userid);

                        var tabs = $('.nav-tabs'),
                        data = rdata.data, content = '';

                        for (var item in data) {
                            content +=
                                '<li class="{1}" Id="{2}"><a href="#tabs" data-toggle="tab">{0}</a></li>'
                                .format(data[item].ProductName, item == 0 ? "active" : "", data[item].Id);
                        }
                        tabs.html(content);
                        /*产品权限切换*/
                        $.each(tabs.find('li'), function (i, field) {
                            $(this).click(function () {
                                var _tId = $(this).attr('Id');
                                $('input[name=productid]').val(_tId);
                                if (!ExistsASL('purview')) {
                                    $.ActionSelectList.push({
                                        name: 'purview',
                                        url: '/Users/GetProductMenuByPurview/?id={0}'.format(_tId),
                                        type: 'checkbox',
                                        target: 'customdiv_user'
                                    });
                                }
                                else {
                                    $.ActionSelectList[1].url = '/Users/GetProductMenuByPurview/?id={0}'.format(_tId);
                                }
                                $.BindingSelect(function () {
                                    $.ActionSetForm(calldata);
                                });
                            });
                            if (i == 0) {
                                $(this).click();
                            }
                        });
                    }
                });               
                
            });
            function ExistsASL(v) {
                var flag = false, d = $.ActionSelectList;
                for (var item in d) {
                    if (d[item].name == v) {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
        },
        ActionListExpanDetails: function (options) {/*列表显示,点击可看详情*/
            options = $.extend({
                tourl: '',
                toid: 'list>tbody',
                tmplid: 'template',
                tableid: '#list'
            }, options || {});

            $(".portlet > .portlet-title > .tools > a.reload").click(function () {
                $.ActionListExpanDetails(options);
            });
            $.extendAjax(
            {
                url: options.tourl,
                type: 'POST'
            },
             function (rdata) {
                 var expandata = [];
                 for (var item in rdata.data) {
                     if (rdata.data[item].ParentId == 0)
                         expandata.push(rdata.data[item]);
                 }
                 /** 数据展示 **/
                 $("#" + options.toid).html(''); //清空已有数据
                 //绑定模版
                 $.ActionTmpl(options.tmplid, expandata, options.toid);
                 $.ActionOperta({ toid: "#" + options.toid, options: options });

                 /**创建列**/
                 var nCloneTh = document.createElement('th');
                 var nCloneTd = document.createElement('td');
                 nCloneTd.innerHTML =
                 '<span class="row-details row-details-close" title="点击展开或收缩子集"></span>';

                 $(options.tableid + ' thead tr').each(function () {
                     if (this.childNodes[0].nodeName == '#text')
                         this.insertBefore(nCloneTh, this.childNodes[0]);
                 });

                 $(options.tableid + ' tbody tr').each(function () {
                     if (this.childNodes[0].nodeName == '#text')
                         this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
                 });
                 $(options.tableid).find('tbody td .row-details').click(function () {
                     var nTr = $(this).parents('tr')[0];
                     var parentid = $(nTr).attr('data-id');
                     var detailsData = [];
                     for (var item in rdata.data) {
                         if (rdata.data[item].ParentId == parentid) {
                             detailsData.push(rdata.data[item]);
                         }
                     }
                     var nNewRow = document.createElement("tr");
                     var nNewCell = document.createElement("td");
                     var nNewCell1 = document.createElement("td");
                     nNewRow.appendChild(nNewCell);
                     nNewRow.appendChild(nNewCell1);
                     //nNewCell.className = 'details';
                     nNewCell1.colSpan = 5;
                     if ($(this).attr('class') == 'row-details row-details-close') {
                         $(this).addClass("row-details-open").removeClass("row-details-close");
                         if (typeof mHtml === "string") {
                             nNewCell1.innerHTML = fnFormatDetails(detailsData);
                         }
                         else {
                             $(nNewCell1).html(fnFormatDetails(detailsData));
                         }
                         var tbody = $(options.tableid).children('tbody')[0];
                         var nTrs = $('tr', tbody);
                         if ($.inArray(nTr, nTrs) != -1) {
                             $(nNewRow).insertAfter(nTr);
                         }
                         $.ActionListDetailsArray.push({
                             "nTr": nNewRow,
                             "nParent": nTr
                         });
                         $.ActionOperta({ toid: "#" + options.toid, options: options });
                     } else {
                         $(this).addClass("row-details-close").removeClass("row-details-open");
                         for (var i = 0; i < $.ActionListDetailsArray.length; i++) {
                             if ($.ActionListDetailsArray[i].nParent == nTr) {

                                 var nTrParent = $.ActionListDetailsArray[i].nTr.parentNode;
                                 if (nTrParent) {
                                     nTrParent.removeChild($.ActionListDetailsArray[i].nTr);
                                 }
                                 $.ActionListDetailsArray.splice(i, 1);
                             }
                         }
                     }

                 });
             });
            function fnFormatDetails(data) {
                var sOut = '<div style="color:red">暂无子集!</div>';
                if (data.length > 0) {
                    var sOut = '<table class="table table-striped table-hover table-bordered" style="padding:0px;margin-left:-9px"><tbody>';
                    $("#Expand_Details").html('');
                    $.ActionTmpl(options.tmplid, data, 'Expand_Details');
                    sOut += $("#Expand_Details").html() + '</tbody></table>';
                    //console.log(sOut);
                }
                return sOut;
            }
        },        
        CustomTree: function (options) {
            options = $.extend({
                url: '/Users/ProductList',
                id: 'tree',
                menus: false,
                menuid: 'menu',
                tvoptions: {
                    animated: "fast",
                    collapsed: true
                },
                menuoptions: {},
                click: function () { }
            }, options || {});
            $.extendAjax({
                url: options.url,
                type: 'post'
            }, function (rdata) {
                var treeNodes = rdata.data;
                /*console.log(JSON.stringify(treeNodes));*/
                $('#' + options.id).empty().append(CreateTree(treeNodes));
                /*$('body').data('ptree', treeNodes);*/
                $('#' + options.id + ' span').click(function () {
                    options.click.call(this);
                });
                if (options.menus) {
                    $('#' + options.id + ' span')
                    .contextMenu(options.menuid, options.menuoptions);
                }

                $('#' + options.id).treeview(options.tvoptions);
            });
            function CreateTree(d) {
                var list = '';
                $.each(d, function (i, n) {
                    list += '<li><span ref="{0}" pid="{1}" id="{2}" imenu="{4}"><a href="#">{3}</a></span>'
                    .format(n.Id, n.ParentId, n.Id, n.Text, n.IsMenu);
                    if (n.Children.length > 0) {
                        list += "<ul>";
                        list += CreateTree(n.Children);
                        list += "</ul>"
                    }
                    list += "</li>";
                });
                return list;
            }
        }        
    });
    
})(jQuery);
$(document).ready(function () {
    $.ClearCookie();
});