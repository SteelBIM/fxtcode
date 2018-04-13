//学生端错题集页面处理Js
//创建:2016-03-08

var StudentWrongQuestionInit = function () {
    var Current = this;
    this.PageIndex = 1;//当前页码
    this.PageSize = 10; //每页显示数
    this.PageCount = 0; //总页码数
    this.$window = null;
    this.$document = null;
    this.QueNo = 1;     //数据序号 从1开始
    this.StopLoad = true; //中断加载

    this.dBid = Common.QueryString.GetValue("dBid");   //默认册ID
    this.dUid = Common.QueryString.GetValue("UnitID"); //默认单元ID
    this.SubID = Common.QueryString.GetValue("SubID"); //默认单元ID
    //页面初始化方法
    this.Init = function () {
        $("#divLoadLast").hide();
        $("#aOneKeyStart").hide();

        Current.$window = $(window);
        Current.$document = $(document);

        Current.$window.bind('scroll', Current.onScroll);

        if (Current.SubID == undefined || Current.SubID == "" || Current.SubID == null) {
            Current.SubID = 3;
        }
        //赋值学科下拉框
        //$("#selSub").select();
        $("#selSub select").val(Current.SubID);

        if ($("#selBookReel select").val() == null) {
            $("#divLoadLast").hide();
            $("#aOneKeyStart").hide();
            //$("#aOneKeyStart").hide();
        }

        $('.wrongList li').bind('mouseover', function () {
            $(this).parent().find('li').removeClass('on');
            $(this).addClass('on');
        })
        $('.unitList li').bind('click', function () {
            $(this).parent().find('li').removeClass('on');
            $(this).addClass('on');
        })


        //读取默认册别的单元列表
        Current.dBid = $("#selBookReel select").val();
        Current.GetUnitList(Current.dBid);

        $("#selBookReel").click(function () {
            Current.QueNo = 1;
            Current.PageIndex = 1;
            $("#ulQueLs").html("");
            Current.dBid = $("#selBookReel select").val();
            Current.dUid = "";
            Current.GetUnitList(Current.dBid);
        });

        //一键开始点击事件
        $("#aOneKeyStart").click(function () {
            var UnitID = "";
            var QueNum = 0;
            var allLi = $("#ulUnitList li")
            $.each(allLi, function (i) {
                if ($(allLi[i]).attr("Class") == "on") {
                    UnitID = $(allLi[i]).attr("ids");
                    QueNum = $(allLi[i]).attr("QueNum");
                }
            });
            //alert(UnitID);
            if (QueNum == undefined || parseInt(QueNum) == 0) {
                alert("当前单元没有错题");
            }
            else {
                Current.dBid = $("#selBookReel select").val();
                if (Current.SubID == 1) {
                    location.replace("../ChineseModels/DoQuestion.aspx?AccessType=5&dBid=" + Current.dBid + "&UnitID=" + Current.dUid + "&SubID=" + Current.SubID);
                } else if (Current.SubID == 2) {
                    location.replace("../MathModels/DoQuestion.aspx?AccessType=5&dBid=" + Current.dBid + "&UnitID=" + Current.dUid + "&SubID=" + Current.SubID);
                } else {
                    location.replace("../QuestionModels/DoQuestion.aspx?AccessType=5&dBid=" + Current.dBid + "&UnitID=" + Current.dUid + "&SubID=" + Current.SubID);
                }
            }
        });

        //加载更多点击事件
        $("#aLoadLastData").click(function () {
            if (Current.StopLoad) {
                Current.StopLoad = false; //标记判断是不是正在加载。 防止在一次数据读取过程中多次触发事件
                setTimeout("stuWrong.LoadLastData()", 2000);
                //setTimeout("stuWrong.LoadLastData('" + Current.StopLoad + "')", 10000);  //可以传参数的调用
            }
        });
        //切换学科
        $("#selSub").click(function () {
            //学科： $("#selSub select").val() 
            Current.QueNo = 1;
            Current.PageIndex = 1;
            $("#ulUnitList").html("");
            $("#ulQueLs").html("");
            Current.dBid = $("#selBookReel select").val();
            Current.dUid = "";
            Current.SubID = $("#selSub select").val();
            //Current.GetStuBookReel();
            Current.GetUnitList(Current.dBid);
        });
    };

    //读取学生年级册别
    this.GetStuBookReel = function () {
        //var SubID = Common.QueryString.GetValue("SubID");
        if (Current.SubID == undefined || Current.SubID == "" || Current.SubID == null) {
            Current.SubID = 3;
        }
        $.post("?action=GetStuWrongQueBookReel&rand=" + Math.random(), { SubID: Current.SubID }, function (data) {
            if (data) {
                data = eval("(" + data + ")");
                var t = "<option value=\"" + data[0].BooKReelID + "\" "
                       + (data[0].BooKReelName == "四年级下册" ? "selected=\"selected\"" : "") + ">" + data[0].BooKReelName + "</option>";
                $("#selBookRell").html(t);
            }
            else {
                //加载没有查询到班级的显示
                Common.AutoPosition();
            }
        });
    };

    //根据学生年级册别ID 1,2,3 读取册下面的单元列表
    this.GetUnitList = function (Bid) {
        //var SubID = Common.QueryString.GetValue("SubID");
        if (Current.SubID == undefined || Current.SubID == "" || Current.SubID == null) {
            Current.SubID = 3;
        }

        $.post("?action=GetUnitList&rand=" + Math.random(), { Bid: Bid, SubID: Current.SubID }, function (data) {
            if (data) {
                data = Current.EvalData(data);
                if (data.Success) {
                    //加载单元列表[包括对应单元下的错题总数]
                    var tempHtml = "";
                    $.each(data.Data, function () {
                        //是否需要加载默认单元
                        if (Bid == Current.dBid && Current.dUid != "undefined" && Current.dUid != "") {
                            if (this.UnitID == Current.dUid) {
                                tempHtml = tempHtml + "<li class=\"on\" onclick=\"stuWrong.ClickUnitName(this)\" ids=\"" + this.UnitID + "\" QueNum=\"" + this.QueCount + "\"><a title=\"" + this.UnitName + "\">"
                                                    + (Current.SubID == "3" ? this.UnitName : Current.SetValueByLen(this.UnitName)) + " (<b>" + this.QueCount + "</b>题)</a></li>";
                                Current.dUid = this.UnitID;
                                Current.GetStuWrongQueList(this.UnitID);
                            }
                            else {
                                tempHtml = tempHtml + "<li onclick=\"stuWrong.ClickUnitName(this)\" ids=\"" + this.UnitID + "\" QueNum=\"" + this.QueCount + "\"><a title=\"" + this.UnitName + "\">"
                                                    + (Current.SubID == "3" ? this.UnitName : Current.SetValueByLen(this.UnitName)) + " (<b>" + this.QueCount + "</b>题)</a></li>";
                            }
                        }
                        else {
                            if (this == data.Data[0]) {
                                tempHtml = tempHtml + "<li class=\"on\" onclick=\"stuWrong.ClickUnitName(this)\" ids=\"" + this.UnitID + "\" QueNum=\"" + this.QueCount + "\"><a title=\"" + this.UnitName + "\">"
                                                    + (Current.SubID == "3" ? this.UnitName : Current.SetValueByLen(this.UnitName)) + " (<b>" + this.QueCount + "</b>题)</a></li>";
                                Current.dUid = this.UnitID;
                                Current.GetStuWrongQueList(this.UnitID);
                            }
                            else {
                                tempHtml = tempHtml + "<li onclick=\"stuWrong.ClickUnitName(this)\" ids=\"" + this.UnitID + "\" QueNum=\"" + this.QueCount + "\"><a title=\"" + this.UnitName + "\">"
                                                    + (Current.SubID == "3" ? this.UnitName : Current.SetValueByLen(this.UnitName)) + " (<b>" + this.QueCount + "</b>题)</a></li>";
                            }
                        }
                    });
                    $("#ulUnitList").html(tempHtml);
                }
                else {
                    $(".leftMenu1").hide();
                    Current.ChangeHtml(0);
                    Common.AutoPosition();
                }
            }
        })
    }

    //点击单元名称
    this.ClickUnitName = function (obj) {
        $("#ulQueLs").html("");
        Current.QueNo = 1;
        //alert($(obj).attr("ids"));
        var allLi = $("#ulUnitList li")
        $.each(allLi, function () {
            $(this).removeClass();
        });
        $(obj).addClass("on");
        $("#ulQueLs").html("");
        Current.PageIndex = 1;
        Current.dUid = $(obj).attr("ids");
        Current.GetStuWrongQueList(Current.dUid);
    }

    this.ChangeHtml = function (flag) {
        //flag=0,显示缺省页
        if (flag == 0) {
            document.getElementById("aOneKeyStart").style.display = "none";
            document.getElementById("ulQueLs").style.display = "none";
            document.getElementById("divLoadLast").style.display = "none";
            document.getElementById("defaultPage").style.display = "block";
        }
            //flag=1，显示内容页
        else {
            document.getElementById("defaultPage").style.display = "none";
            document.getElementById("aOneKeyStart").style.display = "block";
            document.getElementById("ulQueLs").style.display = "block";
            document.getElementById("divLoadLast").style.display = "block";
        }
    }

    //根据点击的单元名称读取题目列表
    this.GetStuWrongQueList = function (UnitIds) {
        var obj = { UnitIds: UnitIds, PageIndex: Current.PageIndex, PageSize: Current.PageSize };
        $.post("?action=GetWrongQuesList&rand=" + Math.random(), obj, function (data) {
            if (data) {
                data = Current.EvalData(data);
                if (data.Success) {
                    if (data.Data.QueList.length == 0) {
                        Current.ChangeHtml(0);
                    } else {
                        Current.ChangeHtml(1);
                        //根据查询的结果集填充列表Html
                        Current.InsertQueHtml(data.Data);
                        Current.PageCount = data.Data.PageSum;

                        Current.StopLoad = true; //标记判断是不是正在加载。 防止在一次数据读取过程中多次触发事件
                        //当前是最后一页  移除滚动事件
                        if (Current.PageIndex >= data.Data.PageSum) {
                            Current.$window.unbind();
                            $("#divLoadLast").hide();
                        }
                        else {
                            //$("#divLoadLast").show();
                            $("#divLoadLast").hide();
                            Current.$window.bind('scroll', Current.onScroll);
                        }
                    }
                    Common.AutoPosition();
                }
                else {
                    Current.$window.unbind();
                    $("#divLoadLast").hide();
                    $("#ulQueLs").html("");
                    Common.AutoPosition();
                }
            }
            else {
                //查询失败
                $("#divLoadLast").hide();
                Common.AutoPosition();
            }
        });
    };

    //加载题目列表
    this.InsertQueHtml = function (data) {
        //$("#ulQueLs").html("");
        var tempHtml = "";
        //var StartNum = (Current.PageIndex - 1) * 10;
        $.each(data.QueList, function (i) {
            var rowClass = " class=\"odd\"";
            //偶数行
            if ((i + 1) % 2 == 0) {
                rowClass = " class=\"even\"";
            }
            //var tempNo = parseInt(StartNum) + i + 1;
            var tempTitle = this.queTitle + ": " + this.queUnitMsg;
            if (tempTitle.length > 45) {
                tempTitle = tempTitle.substring(0, 45) + "...";
            }
            tempHtml = tempHtml + "<li " + rowClass + " id=\"li_Que_" + this.QueID + "\"><a title=\"" + this.queTitle + ": " + this.queUnitMsg
                       + "\" onclick=\"stuWrong.ClickQueTitle('" + this.QueID + "')\" class=\"tName\" ><b>" + Current.QueNo + "</b>、" + tempTitle
                       + "</a><a class=\"deleA\" title=\"移除\" queid=\"" + this.QueID + "\" onclick=\"stuWrong.DelWrongQue('" + this.QueID + "')\">&nbsp;</a></li>";

            Current.QueNo++;
        });
        $("#ulQueLs").append(tempHtml);

        $('.wrongList li').bind('mouseover', function () {
            $(this).parent().find('li').removeClass('on');
            $(this).addClass('on');
        })
        $('.unitList li').bind('click', function () {
            $(this).parent().find('li').removeClass('on');
            $(this).addClass('on');
        })
    }

    //点击单个题目做题
    this.ClickQueTitle = function (QueID) {
        if (Current.SubID == 1) {
            location.replace("../ChineseModels/DoQuestion.aspx?AccessType=5&dBid=" + Current.dBid + "&UnitID=" + Current.dUid + "&QuestionID=" + QueID + "&SubID=" + Current.SubID);
        } else if (Current.SubID == 2) {
            location.replace("../MathModels/DoQuestion.aspx?AccessType=5&dBid=" + Current.dBid + "&UnitID=" + Current.dUid + "&QuestionID=" + QueID + "&SubID=" + Current.SubID);
        } else {
            location.replace("../QuestionModels/DoQuestion.aspx?AccessType=5&dBid=" + Current.dBid + "&UnitID=" + Current.dUid + "&QuestionID=" + QueID + "&SubID=" + Current.SubID);
        }
    }

    //删除题目
    this.DelWrongQue = function (QueID) {
        if (QueID != undefined) {
            if (confirm("确定要删除这个错题吗?")) {
                $.post("?action=DelWrongQue&rand=" + Math.random(), { DelQueID: QueID }, function (data) {
                    if (data) {
                        data = Current.EvalData(data);
                        if (data.Success) {
                            //alert("删除成功");
                            $("#li_Que_" + QueID).remove();
                            //删除成功之后 重置所有数据的序号..
                            Current.SetQueListNo();
                        }
                    }
                });
            }
            else { }
        }
    }

    //删除成功之后 重置所有数据的序号.. 更新单元的错题总数
    this.SetQueListNo = function () {
        Current.QueNo = 1;
        var allLi = $("#ulQueLs li b")
        $.each(allLi, function () {
            if ($(this) != undefined) {
                $(this).html(Current.QueNo);
                Current.QueNo++;
            }
        });

        var tempCount = 0;
        var UnitList = $("#ulUnitList li")
        $.each(UnitList, function () {
            if ($(this).attr("Class") == "on") {
                tempCount = parseInt($(this).find('a b').html()) - 1;
                if (tempCount >= 0) { //错题总数大于等于零才修改显示
                    $(this).find("b").html(tempCount);
                }
            }
        });
        if (tempCount <= 0) {//没有错题 显示缺省页
            Current.ChangeHtml(0);
        }
    }

    //鼠标滚动控制
    this.onScroll = function () {
        // 如果窗口底部小于100像素，就执行加载事件  
        var winHeight = window.innerHeight ? window.innerHeight : Current.$window.height(); // iphone fix
        var closeToBottom = ((Current.$window.scrollTop() + winHeight) > (Current.$document.height() - 25));
        if (closeToBottom) {
            if (Current.PageIndex >= Current.PageCount) {
                return false;
            } else {
                if (Current.StopLoad) {
                    Current.StopLoad = false; //标记判断是不是正在加载。 防止在一次数据读取过程中多次触发事件
                    $("#divLoadLast").show();
                    setTimeout("stuWrong.LoadLastData()", 2000);
                    //setTimeout("stuWrong.LoadLastData('" + Current.StopLoad + "')", 10000);  //可以传参数的调用
                }
            }
        }
    }

    //加载更多数据
    this.LoadLastData = function () {
        //加载下一页的数据到页面
        Current.PageIndex = Current.PageIndex + 1;
        var UnitID = "";
        var allLi = $("#ulUnitList li")
        $.each(allLi, function () {
            if ($(this).attr("Class") == "on") {
                UnitID = $(this).attr("ids");
            }
        });
        Current.GetStuWrongQueList(UnitID);
    }

    //数据转换json
    this.EvalData = function (data) {
        return eval("(" + data + ")");
    };

    //根据内容长度 返回显示字符
    this.SetValueByLen = function (str) {
        var s = "";
        if (str != undefined) {
            if (str.length > 8) {
                s = str.substring(0, 8) + "...";
            }
            else {
                s = str;
            }
        }
        return s;
    }
};
