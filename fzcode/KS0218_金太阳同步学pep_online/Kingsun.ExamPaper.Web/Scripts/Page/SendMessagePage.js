//校讯通界面

var SendMessageInit = function () {
    var Current = this;
    var obj = "";    
    var PageIndex = 1;//当前页码
    var PageSize = 10; //每页显示数
    var classNames = "";//发送班级名称
    var loadDialog;
    this.Init = function () {
        Current.GetTeaClass();
    }

    //获取教师班级
    this.GetTeaClass = function () {
        var classListHtml = "";
        //navBox条件的切换
        $(".navBox").find("a").bind("click", function () {
            $(this).parent().find("a").removeClass("on");
            $(this).addClass("on");
        })
        //sidebarS侧栏的判断	
        if ($(".classUl li").length > 5) {
            $(".openA").css("display", "block");
            Current.AChange();
        } else {
            $(".openA").css("display", "none");
        }
        $.post("?action=GetTeacherClass&rand=" + Math.random(), null, function (data) {
            var data = eval("(" + data + ")");
            if (data.Success) {
                Current.toggleDefaultPage(0);
                for (var i = 0; i < data.Data.length; i++) {
                    classListHtml += '<li><a href="javascript:void(0)" id="' + data.Data[i].ClassID + '" onclick=sendMessage.InsertSendList(' + "'" + data.Data[i].ClassName + "',this,'" + data.Data[i].ClassID + "'," + data.Data[i].IsEmpty + ')>' + data.Data[i].ClassName + '</a></li>';
                }
                $(".classUl").html(classListHtml);

                //sidebarS侧栏的判断	
                if ($(".classUl li").length > 5) {
                    $(".openA").css("display", "block");
                    Current.AChange();
                } else {
                    $(".openA").css("display", "none");
                }
            }
            else {
                //alert(data.Message);
                Current.toggleDefaultPage(1);
            }
            Common.AutoPosition();
        });
    }

    //sidebarS侧栏的动作
    this.AChange = function () {
        $(".openA").bind("click", function () {
            if ($(this).attr("class") == "openA on") {
                $(this).removeClass("on");
                $(".classUl").css("height", "170px")
            } else {
                $(this).addClass("on");
                ; $(".classUl").css("height", "auto");
            }
        })
    }

    //--页面切换
    //发送页面
    this.SendMsg = function () {
        //页面显示切换
        document.getElementById("leftH").style.display = "block";
        document.getElementById("rightH").style.display = "block";
        document.getElementById("listH").style.display = "none";
        Current.GetTeaClass();
    }
    //已发送页面
    this.CheckSendMsg = function () {
        //发送页面信息清空
        $("#classTxt").val("");
        $("#msgTxt").val("");
        $(".classUl").find("a").removeClass("on");
        classNames = "";
        //页面显示切换
        document.getElementById("leftH").style.display = "none";
        document.getElementById("rightH").style.display = "none";
        document.getElementById("listH").style.display = "block";
        
        //获取已发送消息列表(页码重置)
        PageIndex = 1;
        Current.GetSendMsgList();
    }

    //获取教师已发送校讯通
    this.GetSendMsgList = function () {
        obj = { PageIndex: PageIndex, PageSize: PageSize };
        //获取教师发送的校讯通
        $.post("?action=GetSendMsg&rand=" + Math.random(), obj, function (data) {
            data = eval("(" + data + ")");
            if (data.Success) {
                $(".tipP").html("已发送（" + data.Data.Total + "封）");
                Current.MsgListHtml(data.Data);
            }
            else {
                alert(data.message);
            }
            Common.AutoPosition();
        });
    }

    //点击班级事件
    this.InsertSendList = function (className, curobj, classID,isEmpty) {
        if (isEmpty == 1)
        {
            alert("此班级没有学生，无法选择！");
            return;
        }
        if (document.getElementById(classID).className == "on") {
            //再次点击班级取消选中并从发送班级中删除
            $(curobj).removeClass("on");
            className += ";";
            classNames = classNames.replace(className, "");
        }
        else {
            //点击班级样式选中及在发送班级中显示班级名称
            $(curobj).addClass("on");
            classNames += className + ";";
        }
        $("#classTxt").val(classNames);
    }

    //点击发送
    this.SendMessage = function () {
        var classlist = "";
        for (var i = 0; i < $(".classUl").find("a").length; i++) {
            if ($(".classUl").find("a")[i].className == "on") {
                classlist += $(".classUl").find("a")[i].id+",";
            }
        }
        var message = document.all.msgTxt.value;
        if (classlist == "") {
            alert("发送班级为空");
            return;
        }
        if (message.replace(/\s/ig, '') == "") {
            alert("发送内容为空");
            return;
        }
        loadDialog = art.dialog({
            id: 'loading',
            opacity: .1,
            padding: '0',
            lock: true,
            content: '<img style="width:305px;height:304px;align:center" src="../App_Themes/images/Loading.gif" />'
        });
        $(".aui_close").hide();
        obj = { ClassList: classlist, ClassNames: $("#classTxt").val(), Message: message }
        $.post("?action=SendMessage&rand=" + Math.random(), obj, function (data) {
            if (data == "") {
                //页面清空
                $("#classTxt").val("");
                $("#msgTxt").val("");              
                loadDialog.close();
                alert("消息发送成功！");
                $(".navBox").find("a").removeClass("on");
                //页面转换
                $("#listBtn").click();
                
            }
            else {
                alert(data);
            }
        });
    }

    this.MsgListHtml=function (msgListData) {
        var msgList = msgListData.MsgList;
        var tempHtml = '';
        if (msgList != null && msgList.length > 0) {
            Current.ChangeHtml(1);
            tempHtml = '<tr class="firstTr">' +
                           '<th width="235px"><input id="checkAll" onclick="sendMessage.ck1(this)" type="checkbox"/><span class=\"borderN\">时间</span></th>' +
                           '<th width="255px"><span>班级(绑定微信的家长可收到消息）</span></th>' +
                           '<th width="470px"><span>内容</span></th>' +
                           '<th></th></tr>';
            for (var i = 0; i < msgList.length; i++) {
                var classNames = msgList[i].Title.split(';');
                var classHtml = "";
                if (classNames.length > 2) {
                    classHtml = classNames[0] + ";" + classNames[1] + "……";
                }
                else {
                    classHtml = msgList[i].Title;
                }
                tempHtml += '<tr class="' + (i % 2 == 0 ? "oddTr" : "evenTr") + '">' +
                            '<td><input id="' + msgList[i].MessageID + '" name="subBox" type="checkbox" onclick="sendMessage.ck2()"/><span class="timeSp">' + msgList[i].SendDate.replace(/-/g, '/').substring(0, 16) + '</span></td>' +
                            '<td><p class="gradeP" title="' + msgList[i].Title + '">' + classHtml + '</p></td>' +
                            '<td><p class="msgP" title="' + msgList[i].MsgContent + '">' + (msgList[i].MsgContent.length > 30 ? msgList[i].MsgContent.substring(0, 30) + "……" : msgList[i].MsgContent) + '</p></td>' +
                            '<td></td></tr>';
            }
            $(".tabS2").html(tempHtml);
            //加载分页部分
            Current.insertPageDivHtml(msgListData.PageSum, msgListData.Total);          
        }
        else {
            //$(".tabS2").html(tempHtml);
            //$("#pageDiv").html("");
            ////没有发送过校讯通
            //alert("您还没有发送过校讯通哦~");
            Current.ChangeHtml(0);
        }
        Common.AutoPosition();
    }
    //切换无班级的缺省页
    this.toggleDefaultPage = function (flag) {
        if (flag == 1) {
            $(".main").hide();
            $("#defaultPageClass").show();
            $(".span").html('您还没有班级哦，请联系学校管理员创建班级吧！');
        } else {
            $("#defaultPageClass").hide();
            $(".main").show();
        }
    }

    this.ChangeHtml = function (flag) {
        //flag=0,显示缺省页
        if (flag == 0) {
            document.getElementById("headDiv").style.display = "none";
            document.getElementById("deleteD").style.display = "none";
            document.getElementById("tabS2").style.display = "none";
            document.getElementById("pageDiv").style.display = "none";
            document.getElementById("defaultPage").style.display = "block";
        }
            //flag=1，显示内容页
        else {
            document.getElementById("defaultPage").style.display = "none";
            document.getElementById("headDiv").style.display = "block";
            document.getElementById("deleteD").style.display = "block";
            document.getElementById("tabS2").style.display = "block";
            document.getElementById("pageDiv").style.display = "block";
        }
    }

    //加载分页div:DivPage     参数 pageCount：数据总页数 dataCount：数据总条数
    this.insertPageDivHtml=function (pageCount, dataCount) {
        if (dataCount > 0) {
            if (dataCount <= PageSize) {
                //总记录小于每页显示数，只需要一页可以全部显示
                $("#pageDiv").html("<a class=\"onPage\" onclick=\"sendMessage.ClickPageNum('1')\">1</a><a class=\"totPage\">共1页</a>");
            }
            else {
                //需要多页显示
                var tempHtml = '';
                tempHtml += (PageIndex == 1 ? "" : "<a class=\"lastPage\" onclick=\"sendMessage.OperationPageBtn('-1')\">上一页</a>");
                if (pageCount <= 10) {
                    for (var i = 1; i <= pageCount; i++) {
                        if (i == PageIndex) {
                            tempHtml += "<a class=\"onPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                        }
                        else {
                            tempHtml += "<a class=\"numPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                        }
                        //tempHtml += "<a onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + (i == PageIndex ? ("[" + i + "]") : i) + "</a>";
                    }
                }
                else {
                    if (parseInt(PageIndex) - 5 > 1) {
                        tempHtml += "<a onclick=\"sendMessage.ClickPageNum('1')\">1</a><a>...</a>";
                        if (parseInt(PageIndex) + 7 >= pageCount) {
                            for (var i = parseInt(pageCount) - 9; i <= pageCount; i++) {
                                if (i == PageIndex) {
                                    tempHtml += "<a class=\"onPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                                }
                                else {
                                    tempHtml += "<a class=\"numPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                                }
                                //tempHtml += "<a onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + (i == PageIndex ? ("[" + i + "]") : i) + "</a>";
                            }
                        }
                        else {
                            for (var i = parseInt(PageIndex) - 2; i <= parseInt(PageIndex) + 7; i++) {
                                if (i == PageIndex) {
                                    tempHtml += "<a class=\"onPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                                } else {
                                    tempHtml += "<a class=\"numPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                                }
                                //tempHtml += "<a onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + (i == PageIndex ? ("[" + i + "]") : i) + "</a>";
                            }
                            tempHtml += (parseInt(PageIndex) + 8 == pageCount ? "" : "<a class=\"otherPage\">...</a>") + "<a class=\"numPage\" onclick=\"sendMessage.ClickPageNum('" + pageCount + "')\">" + pageCount + "</a>";
                        }
                    }
                    else {
                        for (var i = 1; i <= 10; i++) {
                            if (i == PageIndex) {
                                tempHtml += "<a class=\"onPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                            }
                            else {
                                tempHtml += "<a class=\"numPage\" onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + i + "</a>";
                            }
                            //tempHtml += "<a onclick=\"sendMessage.ClickPageNum('" + i + "')\">" + (i == PageIndex ? ("[" + i + "]") : i) + "</a>";
                        }
                        tempHtml += "<a class=\"otherPage\">...</a>";
                        tempHtml += "<a class=\"numPage\" onclick=\"sendMessage.ClickPageNum('" + pageCount + "')\">" + pageCount + "</a>";
                    }
                }

                tempHtml += (PageIndex == pageCount ? "" : "<a class=\"nextPage\"  onclick=\"sendMessage.OperationPageBtn('1')\">下一页</a>");
                //tempHtml += "<a style=\" width:30px;\">[末页]</a>";
                tempHtml += "<a class=\"totPage\">共" + pageCount + "页</a>";
                $("#pageDiv").html(tempHtml);
            }
        }
    }

    //翻页：上一页-1 下一页+1
    this.OperationPageBtn=function (addPage) {
        PageIndex = parseInt(PageIndex) + parseInt(addPage);
        Current.ClickPageNum(PageIndex); //调用分页方法
    }

    //切换分页
    this.ClickPageNum=function (num) {
        PageIndex = num;
        obj = { PageIndex: PageIndex, PageSize: PageSize };
        $.post("?action=GetSendMsg&rand=" + Math.random(), obj, function (data) {
            data = eval("(" + data + ")");
            if (data.Success) {
                $(".tipP").html("已发送（" + data.Data.Total + "封)");
                if (data.Data.MsgList.length == 0) {
                    if (num > 1) {
                        num -= 1;
                        Current.ClickPageNum(num);
                    }
                    else {
                        Current.MsgListHtml(data.Data);
                    }
                } else {
                    //循环当前班级的作业列表
                    Current.MsgListHtml(data.Data);
                }
            }
            else {
                alert(data.message);
            }
        });
    }

    this.ck1=function(self) {
        var a = $("input[name='subBox']");
        for (var i = 0; i < a.length; i++) {
            a[i].checked = self.checked;
        }
    }

    this.ck2 = function () {
        var $subBox = $("input[name='subBox']");
        var a = document.getElementById("checkAll");
        a.checked = ($subBox.length == $("input[name='subBox']:checked").length ? true : false);
    }

    //删除消息
    this.DelMsg=function () {
        //
        var ids = "";
        var a = $("input[name='subBox']:checked");
        for (var i = 0; i < a.length; i++) {
            ids += ids == "" ? ($(a[i]).attr("id")) : "," + ($(a[i]).attr("id"));
        }
        if (ids == "") {
            alert("请选择要删除的消息！");
        }
        else {
            obj = { MsgIDs: ids }
            $.post("?action=DelMsg&rand=" + Math.random(), obj, function (data) {
                data = eval("(" + data + ")");
                if (data.Success) {
                    alert(data.Data);
                    //刷新列表
                    Current.ClickPageNum(PageIndex);
                }
                else {
                    alert(data.Message);
                }
            });
        }
    }
}