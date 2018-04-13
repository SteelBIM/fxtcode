// 常用web表单控件美化脚本
/*====================================
*基于JQuery 1.9.0表单控件组
*作者：biny
====================================*/
var _thisScript, _skin, _path;
// 获取当前JS文件的路径地址,用于适时加载控件库的样式表，拼装样式表的绝对地址  
_path = window['_controls_path'] || (function (script, i, me) {
    for (i in script) {
        // 如果通过第三方脚本加载器加载本文件，请保证文件名含有"artDialog"字符
        if (script[i].src && script[i].src.indexOf('jquery.form.controls') !== -1) me = script[i];
    };
    _thisScript = me || script[script.length - 1];
    me = _thisScript.src.replace(/\\/g, '/');
    return me.lastIndexOf('/') < 0 ? '.' : me.substring(0, me.lastIndexOf('/'));
} (document.getElementsByTagName('script')));

$(function () {
    loadSkinCss(); //加载表单皮肤样式表    
    $(".content-tab-ul-wrap li").find("A.selected").append("<i></i>");  

    //常用表单控件
    $(".rule-single-checkbox").ruleSingleCheckbox(); //开关式单选
    $(".rule-multi-checkbox").ruleMultiCheckbox(); //多项复选
    $(".rule-multi-radio").ruleMultiRadio(); //多项单选
    $(".rule-single-select").ruleSingleSelect(); //下拉单选
    $(".rule-multi-porp").ruleMultiPorp();
    $(".single-text").ruleSingleText();
    $(".rule-multihtml-radio").ruleMultiHtmlRadio(); //html radio控件组
    $(".rule-multihtml-checkbox").ruleMultiHtmlCheckbox(); //html checkbox控件组
    $(".html-checkbox").ruleSingleHtmlCheckbox(); //html checkbox控件组，单个
    $(".rule-normalhtml-radio").ruleNormalHtmlRadio(); //html radio控件组,普通风格
    $(".rule-search-text").ruleSearchText(); //搜索文本组合控件
    $(".iconbtn").ruleIconButton(); //
    $(".autobtn").ruleAutoButton(); //自适应宽按钮
    $(".rule-html-select").ruleHtmlSelect(); //下拉单选
		rowsLight();//隔行换色
	
	/*20160310-tyx*/
	/*IE6下鼠标悬浮时的颜色变化*/
	$(".single-select .select-items ul li").bind("mouseover",function(){
		$(this).addClass("on");
		}).bind("mouseout",function(){
			$(this).removeClass("on");
			});
		
});

function rowsLight(){
	$(".MainTable tbody tr").not('tr.hidTr').each(function(index){
			if(index%2==0){				
				$(this).addClass("evenRow");
			}
			else{
				$(this).addClass("oddRow");
			}			
	});
	$(".hasSub").hover(function(){
			var nextRow=$(this).parent().parent().next();
			$(".hidTr").hide();
			if(!$(nextRow).hasClass(".hidTr")){
				$(nextRow).show();
			}
	},function(){
		  var nextRow=$(this).next();
			if(!$(nextRow).hasClass("hidTr")){
				$(nextRow).hide();
			}
	})
}

//获取文本宽度
var textWidth = function (text) {
    var sensor = $('<pre>' + text + '</pre>').css({ display: 'none' });
    $('body').append(sensor);
    var width = sensor.width();
    sensor.remove();
    return width;
};
//自适应宽按钮
$.fn.ruleAutoButton = function () {
    var ruleAutoButton = function (curObj) {
        var controlType = $(curObj).attr("type");
        var curText; //按钮文字
        if (controlType == "button") {
            //input图标按钮稍有不同
            curText = curObj.val();
            $(curObj).addClass("inputAutoBtn");
        }
        else {
            //A标签图标按钮和asp:LinkButton按钮是一样的
            curText = curObj.text();
            $(curObj).html("");
        }
        var w = textWidth(curText) + 20;
        $(curObj).css({ "width": w });
        if (controlType != "button") {
            curObj.append($("<span>" + curText + "</span>"));
        }
        //鼠标移入移出事件
        $(curObj).hover(function () {
            $(this).addClass("hov");
        }, function () {
            $(this).removeClass("hov");
        });
        //检查控件是否启用
        if ($(curObj).attr("Enabled") == true) {
            $(this).addClass("dis");
            $(this).next().addClass("dis");
            return;
        }

//        $(curObj).click(function () {
//            $(".succed").removeClass("succed");
//            $(this).addClass("succed");

////            return false;
//        });
    }
    return $(this).each(function () {
        ruleAutoButton($(this));
    });
}
//全选取消按钮函数
function checkAll(chkobj) {
    if ($(chkobj).text() == "全选") {
        var str = $(chkobj).html();
        var temp = str.replace("全选", "反选");
        $(chkobj).html(temp);
        //$(chkobj).text("取消");
        $(".checkall input:enabled").prop("checked", true);
    } else {
        var str = $(chkobj).html();
        var temp = str.replace("反选", "全选");
        $(chkobj).html(temp);
        //$(chkobj).text("全选");
        $(".checkall input:enabled").prop("checked", false);
    }
}
//带图标按钮
$.fn.ruleIconButton = function () {
    var ruleIconButton = function (curObj) {
        var controlType = $(curObj).attr("type");
        var curText; //按钮文字
        if (controlType == "button") {
            //input图标按钮稍有不同
            curText = curObj.val();
            $(curObj).addClass("inputIcoBtn");
        }
        else {
            //A标签图标按钮和asp:LinkButton按钮是一样的
            curText = curObj.text();
            $(curObj).html("");
        }
        var w = textWidth(curText) + 25;
        $(curObj).css({ "width": w });
        if (controlType != "button") {        
            curObj.append($("<span><i></i><b>" + curText + "</b></span>"));
        }
        //鼠标移入移出事件
        $(curObj).hover(function () {
            $(this).addClass("hov");
        }, function () {
            $(this).removeClass("hov");
        });
        //检查控件是否启用
        if ($(curObj).attr("Enabled") == true) {
            $(this).addClass("dis");
            $(this).next().addClass("dis");
            return;
        }

//        $(curObj).click(function () {
//            alert("你点击了我");
//        });
    }
    return $(this).each(function () {
        ruleIconButton($(this));
    });
}
//从cookie中加载皮肤样式
function loadSkinCss() {
    //从Cookie中读取上一次记录的皮肤参数
    var cookie = $.cookie('skin_cookie'); //读取cookie中关于皮肤的参数  
    var curIndex = $.cookie('skin_Index'); //当前选择的序号
    if (cookie == ""||cookie==undefined) {
        loadStyleCss("black"); //如果cookie中没有值，则调用默认皮肤样式表
    }
    else {
        $(".skin ul li a").removeClass("cur");
        $(".skin ul li a").eq(curIndex).addClass("cur");
        loadStyleCss(cookie); //动态加载样式表
    }
}
//切换样式表
function switchSkin(nodeIndex, obj, skinName) {
    $.cookie('skin_cookie', skinName); //设置cookie中关于皮肤的参数
    $.cookie('skin_Index', nodeIndex); //设置cookie中关于皮肤的参数
    window.location.reload();
}
// 无阻塞动态加载CSS样式表 (如"jquery.form.controls.js?skin=mac")，参数为动态加载的样式表名称
function loadStyleCss(skinCssName) {  
    _skin = _thisScript.src.split('skin=')[1];
    var link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = _path + '/' + skinCssName + '/jquery.form.controls.css';        
    _thisScript.parentNode.insertBefore(link, _thisScript);
}

//Tab控制函数
function tabs(tabObj) {
    $(tabObj).parent().append("<i></i>");
    var tabNum = $(".content-tab-ul-wrap li").index($(tabObj).parent("li"));    
    //设置点击后的切换样式
    $(tabObj).parent().parent().find("li a").removeClass("selected");
    $(tabObj).parent().parent().find("li i").hide();
    $(tabObj).addClass("selected");
    $(tabObj).next().show();
    //根据参数决定显示内容
    $(".tab-content").hide();
    $(".tab-content").eq(tabNum).show();
}
//全选取消按钮函数
function checkAll(chkobj) {
    if ($(chkobj).text() == "全选") {
        $(chkobj).children("span").text("取消");
        $(".checkall input:enabled").prop("checked", true);
    } else {
        $(chkobj).children("span").text("全选");
        $(".checkall input:enabled").prop("checked", false);
    }
}
//======================以上基于Validform插件======================
//智能浮动层函数
$.fn.smartFloat = function () {
    var position = function (element) {
        var top = element.position().top;
        var pos = element.css("position");
        $(window).scroll(function () {
            var scrolls = $(this).scrollTop();
            if (scrolls > top) {
                if (window.XMLHttpRequest) {
                    element.css({
                        position: "fixed",
                        top: 0
                    });
                } else {
                    element.css({
                        top: scrolls
                    });
                }
            } else {
                element.css({
                    position: pos,
                    top: top
                });
            }
        });
    };
    return $(this).each(function () {
        position($(this));
    });
};

//搜索文本组合控件
$.fn.ruleSearchText = function () {
    var searchText = function (parentObj) {
        parentObj.addClass("search-control"); //添加样式
        parentObj.find("input:text").each(function () {
            var tipsTxt = $(this).attr("inputtips");
            if (tipsTxt != "") {
                $(this).css({ "color": "gray" });
            }
            //鼠标移入移出事件
            $(this).hover(function () {
                $(parentObj).addClass("hov");                
            }, function () {
                $(parentObj).removeClass("hov");                
            });
            $(this).focus(function () {
                if ($(this).val() == tipsTxt) {
                    $(this).val("");
                    $(this).css({ "color": "#000" });
                }
                else if ($(this).val() == "") {
                    $(this).val(tipsTxt);
                    $(this).css({ "color": "gray" });
                }
                $(parentObj).addClass("focus");                
            })
            $(this).blur(function () {
                if ($(this).val() == tipsTxt) {
                    $(this).val(tipsTxt);
                    $(this).css({ "color": "gray" });
                }
                else if ($(this).val() == "") {
                    $(this).val(tipsTxt);
                    $(this).css({ "color": "gray" });
                }
                $(parentObj).removeClass("focus");               
            })
            //检查控件是否启用
            if ($(this).attr("Enabled") == true) {
                $(this).addClass("dis");
                $(this).next().addClass("dis");
                return;
            }

            $(this).click(function () {

            });
        })
    }
    return $(this).each(function () {
        searchText($(this));
    });
}
//文本框
$.fn.ruleSingleText = function () {
    var singleText = function (parentObj) {
        $(parentObj).each(function () {
            var tipsTxt = $(this).attr("inputTips");
            if (tipsTxt != "") {
                $(this).css({ "color": "gray" });
            }
            //鼠标移入移出事件
            $(this).hover(function () {
                $(this).addClass("txthov");
            }, function () {
                $(this).removeClass("txthov");
            });
            $(this).focus(function () {
                if ($(this).val() == tipsTxt) {
                    $(this).val("");
                    $(this).css({ "color": "#000" });
                }
                else if ($(this).val() == "") {
                    $(this).val(tipsTxt);
                    $(this).css({ "color": "gray" });
                }
                $(this).addClass("txtfocus");
            })
            $(this).blur(function () {
                if ($(this).val() == tipsTxt) {
                    $(this).val(tipsTxt);
                    $(this).css({ "color": "gray" });
                }
                else if ($(this).val() == "") {
                    $(this).val(tipsTxt);
                    $(this).css({ "color": "gray" });
                }
                $(this).removeClass("txtfocus");
            })
            //检查控件是否启用
            if ($(this).attr("Enabled") == true) {
                $(this).addClass("txtdis");
                return;
            }

            $(this).click(function () {

            });
        })
    }
    return $(this).each(function () {
        singleText($(this));
    });
}

//复选框
$.fn.ruleSingleCheckbox = function () {
    var singleCheckbox = function (parentObj) {
        //查找复选框
        var checkObj = parentObj.children('input:checkbox').eq(0);
        parentObj.children().hide();
        //添加元素及样式
        var newObj = $('<a href="javascript:;">'
		+ '<i class="off">否</i>'
		+ '<i class="on">是</i>'
		+ '</a>').prependTo(parentObj);
        parentObj.addClass("single-checkbox");
        //判断是否选中
        if (checkObj.prop("checked") == true) {
            newObj.addClass("selected");
        }
        //检查控件是否启用
        if (checkObj.prop("disabled") == true) {
            newObj.css("cursor", "default");
            return;
        }
        //绑定事件
        $(newObj).click(function () {
            if ($(this).hasClass("selected")) {
                $(this).removeClass("selected");
                //checkObj.prop("checked", false);
            } else {
                $(this).addClass("selected");
                //checkObj.prop("checked", true);
            }
            checkObj.trigger("click"); //触发对应的checkbox的click事件
        });
    };
    return $(this).each(function () {
        singleCheckbox($(this));
    });
};

//单个复选框html checkbox模拟
$.fn.ruleSingleHtmlCheckbox = function () {
    var singleHtmlCheckbox = function (checkboxObj) {
				var parentObj = $("<div class='single-html-checkbox'></div>");        
				var divObj = $('<div class="boxwrap"></div>');
				var newObj = $('<a href="javascript:;">' + $(checkboxObj).val() + '</a>');
				parentObj.insertBefore(checkboxObj); //前插入一个DIV  
				$(parentObj).append(divObj);        
				$(newObj).appendTo($(divObj)); //查找对应Label创建选项
				checkboxObj.hide(); //隐藏内容
				
        if ($(checkboxObj).prop("checked") == true) {
            newObj.addClass("selected"); //默认选中
        }
        //检查控件是否启用
        if ($(checkboxObj).prop("disabled") == true) {
            newObj.css("cursor", "default");
            return;
        }
        //绑定事件
        $(newObj).click(function () {
					  
            if ($(this).hasClass("selected")) {
                $(this).removeClass("selected");                
            } else {
                $(this).addClass("selected");                
            }
            $(checkboxObj).trigger("click"); //触发对应的checkbox的click事件
            //alert($(checkboxObj).prop("checked"));//选中项的值
            //alert($(checkboxObj).val()); //选中项的值
        });
    };
    return $(this).each(function () {
        singleHtmlCheckbox($(this));
    });
}
//全选反选
function selectAll(){
	  var obj=$("#checkboxAll");
		var idArr="";
		if($(obj).prop("checked")){//勾选
			$("input:checkbox").not($(obj)).each(function(n){
				if(!$(this).prop("checked")){
				  $(this).attr("checked",true);
					$(this).prev().remove();//清空checkbox皮肤
					$(this).ruleSingleHtmlCheckbox();//重新加载checkbox皮肤
					idArr+=","+$(this).attr("id");
				}
				$(this).prev().children().attr("title","反不选");
			})			
			$("#checkIds").val(idArr);
		}
		else{
			$("input:checkbox").not($(obj)).each(function(n){
				 if($(this).prop("checked")){
				  $(this).attr("checked",false); 
					$(this).prev().remove();//清空checkbox皮肤
					$(this).ruleSingleHtmlCheckbox();//重新加载checkbox皮肤
				 }
				 $(this).prev().children().attr("title","全选");
			})			
			$("#checkIds").val("");
		}
}
//单选
function checkSelected(obj){
		var curId=$(obj).attr("id");
		var old=$("#checkIds").val();
		var newVal=old;	
		var dataArr=[];	
		if($(obj).prop("checked")){//勾选
			newVal+=","+curId;
		}
		else{
			var valArray=old.split(',');
			for(var i=0;i<valArray.length;i++){
				if(curId==valArray[i]){//删除已存在的
					newVal=old.replace(","+curId,"");
				}
			}
		}
		$("#checkIds").val(newVal);
}

//多项复选框html checkbox模拟
$.fn.ruleMultiHtmlCheckbox = function () {
    var multiHtmlCheckbox = function (parentObj) {
        parentObj.addClass("multi-checkbox"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find(":checkbox").each(function () {
            var indexNum = parentObj.find(":checkbox").index(this); //当前索引
            var newObj = $('<a href="javascript:;">' + $(this).val() + '</a>').appendTo(divObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                newObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                if ($(this).hasClass("selected")) {
                    $(this).removeClass("selected");
                    //parentObj.find(':checkbox').eq(indexNum).prop("checked",false);
                } else {
                    $(this).addClass("selected");
                    //parentObj.find(':checkbox').eq(indexNum).prop("checked",true);
                }
                parentObj.find(':checkbox').eq(indexNum).trigger("click"); //触发对应的checkbox的click事件
                //alert(parentObj.find(':checkbox').eq(indexNum).prop("checked"));
            });
        });
    };
    return $(this).each(function () {
        multiHtmlCheckbox($(this));
    });
}

//多项复选框
$.fn.ruleMultiCheckbox = function () {
    var multiCheckbox = function (parentObj) {
        parentObj.addClass("multi-checkbox"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find(":checkbox").each(function () {
            var indexNum = parentObj.find(":checkbox").index(this); //当前索引
            var newObj = $('<a href="javascript:;">' + parentObj.find('label').eq(indexNum).text() + '</a>').appendTo(divObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                newObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                if ($(this).hasClass("selected")) {
                    $(this).removeClass("selected");
                    //parentObj.find(':checkbox').eq(indexNum).prop("checked",false);
                } else {
                    $(this).addClass("selected");
                    //parentObj.find(':checkbox').eq(indexNum).prop("checked",true);
                }
                parentObj.find(':checkbox').eq(indexNum).trigger("click"); //触发对应的checkbox的click事件
                //alert(parentObj.find(':checkbox').eq(indexNum).prop("checked"));
            });
        });
    };
    return $(this).each(function () {
        multiCheckbox($(this));
    });
}

//多项选项PROP
$.fn.ruleMultiPorp = function () {
    var multiPorp = function (parentObj) {
        parentObj.addClass("multi-porp"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<ul></ul>').prependTo(parentObj); //前插入一个DIV
        parentObj.find(":checkbox").each(function () {
            var indexNum = parentObj.find(":checkbox").index(this); //当前索引
            var liObj = $('<li></li>').appendTo(divObj)
            var newObj = $('<a href="javascript:;">' + parentObj.find('label').eq(indexNum).text() + '</a><i></i>').appendTo(liObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                liObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                if ($(this).parent().hasClass("selected")) {
                    $(this).parent().removeClass("selected");
                } else {
                    $(this).parent().addClass("selected");
                }
                parentObj.find(':checkbox').eq(indexNum).trigger("click"); //触发对应的checkbox的click事件
                //alert(parentObj.find(':checkbox').eq(indexNum).prop("checked"));
            });
        });
    };
    return $(this).each(function () {
        multiPorp($(this));
    });
}


//html的radio按钮组
$.fn.ruleNormalHtmlRadio = function () {
    var multiRadio = function (parentObj) {
        parentObj.addClass("normal-radio"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find('input[type="radio"]').each(function () {
            var indexNum = parentObj.find('input[type="radio"]').index(this); //当前索引
            var newObj = $('<a href="javascript:;">' + $(this).val() + '</a>').appendTo(divObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                newObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected");
                parentObj.find('input[type="radio"]').prop("checked", false);
                parentObj.find('input[type="radio"]').eq(indexNum).prop("checked", true);
                parentObj.find('input[type="radio"]').eq(indexNum).trigger("click"); //触发对应的radio的click事件
            });
        });
    };
    return $(this).each(function () {
        multiRadio($(this));
    });
}

//html的radio按钮组
$.fn.ruleMultiHtmlRadio = function () {
    var multiRadio = function (parentObj) {
        parentObj.addClass("multi-radio"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find('input[type="radio"]').each(function () {
            var indexNum = parentObj.find('input[type="radio"]').index(this); //当前索引
            var newObj = $('<a href="javascript:;">' + $(this).val() + '</a>').appendTo(divObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                newObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected");
                parentObj.find('input[type="radio"]').prop("checked", false);
                parentObj.find('input[type="radio"]').eq(indexNum).prop("checked", true);
                parentObj.find('input[type="radio"]').eq(indexNum).trigger("click"); //触发对应的radio的click事件
            });
        });
    };
    return $(this).each(function () {
        multiRadio($(this));
    });
}
//五星评级控件
$.fn.evaluateStart = function () {
	var evaluateStart = function (curObj) {
		
	}
	return $(this).each(function () {
     evaluateStart($(this));
  });
};

//多项单选
$.fn.ruleMultiRadio = function () {
    var multiRadio = function (parentObj) {
        parentObj.addClass("multi-radio"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find('input[type="radio"]').each(function () {
            var indexNum = parentObj.find('input[type="radio"]').index(this); //当前索引
            var newObj = $('<a href="javascript:;">' + parentObj.find('label').eq(indexNum).text() + '</a>').appendTo(divObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                newObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected");
                parentObj.find('input[type="radio"]').prop("checked", false);
                parentObj.find('input[type="radio"]').eq(indexNum).prop("checked", true);
                parentObj.find('input[type="radio"]').eq(indexNum).trigger("click"); //触发对应的radio的click事件

                var selectVal = $(this).html();
                if (selectVal == "有") {
                    $("#linkUrlBox").show();
                }
                else {
                    $("#linkUrlBox").hide();
                }
                //alert(parentObj.find('input[type="radio"]').eq(indexNum).index());
            });
        });
    };
    return $(this).each(function () {
        multiRadio($(this));
    });
}

function selectClick() {
    //alert("点击事件");
}

function loadData() {
    var obj = $(".rule-html-select").ruleHtmlSelect();
    obj.bindData(dataArr, selectIndex);
}

function getText(objId) {
    var a = $("#" + objId).find("option:selected").text();
    alert(a);
}
function getVal(objId) {
    var a = $("#" + objId).val();
    alert(a);
}
function getIndex(objId) {
    var a = $("#" + objId).get(0).selectedIndex;
    alert(a);
}

//重新加载select控件
function refreshSelect(selectObjId, selectIndex) {
    //组装静态数据，如果是采用ajax方式获取，则先获取数据，这里是静态演示，拼装的模拟数据
    var dataArr = [];
    for (var n = 0; n < 20; n++) {
        dataArr.push("第" + n + "条内容");
    }
    reloadSelect(dataArr,selectObjId,selectIndex);
} 

//参数selectObjId为当前要刷新加载的select控件的ID
//selectIndex为选中项的序号，可以不用定义。
function reloadSelect(dataArr,selectObjId,selectIndex) {    
    //动态绑定数据,data为数组数据，selectIndex为选中项的序号，从0开始    
    if (dataArr.length == 0) { return; }   
    for (var i = 0; i < dataArr.length; i++) {
        var selectClass = "";
        var liObj;
        if (selectIndex == i && selectIndex!=undefined) {
            $('<option selected="selected">' + dataArr[i] + '</opton>').appendTo($("#" + selectObjId));                    
        }
        else {            
            $('<option>' + dataArr[i] + '</opton>').appendTo($("#" + selectObjId));             
        }
    }
    //清空其他结构
    $("#" + selectObjId).prev().remove();
    $("#" + selectObjId).unwrap();
    $("#" + selectObjId).ruleHtmlSelect(); //刷新下拉菜单
}

//新无容器select
$.fn.ruleHtmlSelect = function () {
    var ruleHtmlSelect = function (selectObj) {
        var allW = $(selectObj).width(); //select实际宽度加上内边距         
        var parentObj = $('<div class="single-select"></div>'); //父容器        
        $(selectObj).wrap(parentObj);
        var h = $(selectObj).height() + 22;
        var divObj = $('<div class="boxwrap"></div>').insertBefore(selectObj); //前插入一个DIV
        $(selectObj).attr("class", "");
        selectObj.hide(); //隐藏内容

        //创建元素        
        var titObj = $('<a class="select-tit" href="javascript:;"><span></span><i></i></a>').appendTo(divObj);
        var itemObj = $('<div class="select-items"><i class="arrow"></i><ul></ul></div>').appendTo(divObj);
        $(itemObj).css({ "top": h });
       // var arrowObj = $('').appendTo(divObj);
//        $(titObj).css({ "width": allW});//slect宽度
//        $(itemObj).children("ul").css({ "width": allW + 28 }); //slect下拉框宽度
		$(titObj).css({ "width": "auto"});//slect宽度
        $(itemObj).children("ul").css({ "width": "auto" }); //slect下拉框宽度

        //检查控件是否启用
        if ($(selectObj).prop("disabled") == true) {
            $(titObj).css({ "background": "#E8E8E8", "cursor": "pointer" });
            return;
        }

        //遍历option选项
        selectObj.find("option").each(function (i) {
            var indexNum = selectObj.find("option").index(this); //当前索引
            var liObj = $('<li>' + $(this).text() + '</li>').appendTo(itemObj.find("ul")); //创建LI
            if ($(this).prop("selected") == true) {
                liObj.addClass("selected");
                titObj.find("span").text($(this).text());
            }            
            //绑定事件
            liObj.click(function () {
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected"); //添加选中样式
                selectObj.find("option").prop("selected", false);
                selectObj.find("option").eq(indexNum).prop("selected", true); //赋值给对应的option
                titObj.find("span").text($(this).text()); //赋值选中值
                //arrowObj.hide();
                itemObj.hide(); //隐藏下拉框
                selectObj.trigger("change"); //触发select的onchange事件
                //alert(selectObj.find("option:selected").text());
                //将点击事件公布出去，以进行其它的操作
                optionSelected(selectObj, selectObj.find("option:selected").index(), selectObj.find("option:selected").text());
            });
        });



        //检查控件是否启用
        if (selectObj.prop("disabled") == true) {
            titObj.css("cursor", "default");
            return;
        }
        //绑定单击事件
        titObj.click(function (e) {
            e.stopPropagation();
            if (itemObj.is(":hidden")) {
                //隐藏其它的下位框菜单
                $(".single-select .select-items").hide();
                //$(".single-select .arrow").hide();
                //位于其它无素的上面
                //arrowObj.css("z-index", "1");
                itemObj.css("z-index", "1");
                //显示下拉框
                //arrowObj.show();
                itemObj.show();
                $(".boxwrap a.select-tit").removeClass("selected");
                $(titObj).addClass("selected"); //添加选中样式
            } else {
                //位于其它无素的上面
                //arrowObj.css("z-index", "");
                itemObj.css("z-index", "");
                //隐藏下拉框
                //arrowObj.hide();
                itemObj.hide();
            }
        });
        //绑定页面点击事件
        $(document).click(function (e) {
            selectObj.trigger("blur"); //触发select的onblure事件  
            $(".boxwrap a.select-tit").removeClass("selected");
            //arrowObj.hide();
            itemObj.hide(); //隐藏下拉框
        });
    };
    return $(this).each(function () {
        ruleHtmlSelect($(this));
    });
}

//单选下拉框，无包装容器
$.fn.ruleSingleSelect = function () {
    var singleSelect = function (parentObj) {
        parentObj.addClass("single-select"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        //创建元素
        var titObj = $('<a class="select-tit" href="javascript:;"><span></span><i></i></a>').appendTo(divObj);
        var itemObj = $('<div class="select-items"><i class="arrow"></i><ul></ul></div>').appendTo(divObj);
        var selectObj = parentObj.find("select").eq(0); //取得select对象
        //遍历option选项
        selectObj.find("option").each(function (i) {
            var indexNum = selectObj.find("option").index(this); //当前索引
            var liObj = $('<li>' + $(this).text() + '</li>').appendTo(itemObj.find("ul")); //创建LI
            if ($(this).prop("selected") == true) {
                liObj.addClass("selected");
                titObj.find("span").text($(this).text());
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                liObj.css("cursor", "default");
                return;
            }
            //绑定事件
            liObj.click(function () {
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected"); //添加选中样式
                selectObj.find("option").prop("selected", false);
                selectObj.find("option").eq(indexNum).prop("selected", true); //赋值给对应的option
                titObj.find("span").text($(this).text()); //赋值选中值
                //arrowObj.hide();
                itemObj.hide(); //隐藏下拉框
                selectObj.trigger("change"); //触发select的onchange事件
                //alert(selectObj.find("option:selected").text());
                //将点击事件公布出去，以进行其它的操作
                optionSelected(selectObj.parent("div"), selectObj.find("option:selected").index(), selectObj.find("option:selected").text());
            });
        });
        //设置样式
        //titObj.css({ "width": titObj.innerWidth(), "overflow": "hidden" });
        //itemObj.children("ul").css({ "max-height": $(document).height() - titObj.offset().top - 62 });

        //检查控件是否启用
        if (selectObj.prop("disabled") == true) {
            titObj.css("cursor", "default");
            return;
        }
        //绑定单击事件
        titObj.click(function (e) {
            e.stopPropagation();
            if (itemObj.is(":hidden")) {
                //隐藏其它的下位框菜单
                $(".single-select .select-items").hide();
                //$(".single-select .arrow").hide();
                //位于其它无素的上面
                //arrowObj.css("z-index", "1");
                itemObj.css("z-index", "1");
                //显示下拉框
                //arrowObj.show();
                itemObj.show();
                $(".boxwrap a.select-tit").removeClass("selected");
                $(titObj).addClass("selected"); //添加选中样式
            } else {
                //位于其它无素的上面
                //arrowObj.css("z-index", "");
                itemObj.css("z-index", "");
                //隐藏下拉框
                //arrowObj.hide();
                itemObj.hide();
            }
        });
        //绑定页面点击事件
        $(document).click(function (e) {
            selectObj.trigger("blur"); //触发select的onblure事件  
            $(".boxwrap a.select-tit").removeClass("selected");          
            //arrowObj.hide();
            itemObj.hide(); //隐藏下拉框
        });
    };
    return $(this).each(function () {
        singleSelect($(this));
    });
}
//下拉选择框的选择事件
//参数：curObj，为当前模拟下拉控件的顶级包裹对象，用它来和其他相同的下拉控件进行区别
//参数：selectedIndex，选中的序号
//参数：selectedText，选中的值
function optionSelected(curObj, selectedIndex, selectedText) {
    //alert("你当前选中的序号为：" + selectedIndex + " 文本为：" + selectedText);
		//alert($(curObj).attr("id"));
		if ($(curObj).attr("id") == "xueduan") {//学段
		}
		if ($(curObj).attr("id") == "xueke") {//学段
		}
		if ($(curObj).attr("id") == "banben") {//学段
		  var a1 = $("#xueduan" ).find("option:selected").text();
			var a2 = $("#xueke" ).find("option:selected").text();
			var a3 = selectedText;
			setDataList(a1+a2+a3);
		}
}

function setDataList(dataStr){
	//todo:根据dataStr查询所有版本
	//下面是静态模拟的数据列表
	$("#synList").html("");
	var str="";
	var arr=["三年级","四年级","五年级","六年级","七年级","八年级","九年级"];
	var arr2=["全册","上册","下册"];
	$("#synList").append("<li><a href='javascript:void(0)' onclick='selectAll();'>全选</a></li>");
	for(var i=0;i<6;i++){
		var m=Math.floor(Math.random()*arr.length+1)-1;
	  var n=Math.floor(Math.random()*arr2.length+1)-1;
		var conStr=dataStr+arr[m]+arr2[n];
		$("#synList").append("<li><a href='javascript:void(0)' onclick='slectSingle(this);'>"+conStr+"</a></li>");
	}
}

function slectSingle(obj){
	 if($(obj).hasClass("cur")){
			$(obj).removeClass("cur"); 
	 }else{
			$(obj).addClass("cur");
	 }
}

function selectAll(){
	$("#synList li a").each(function(){
		$(this).addClass("cur");	
	})
}