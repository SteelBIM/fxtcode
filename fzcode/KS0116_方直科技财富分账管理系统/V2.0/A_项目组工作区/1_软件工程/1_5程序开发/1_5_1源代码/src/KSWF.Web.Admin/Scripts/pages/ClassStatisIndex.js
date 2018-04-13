
var loadmask = new LoadMask();

var ClassStatisPage = function () {

    var Current = this;
    this.GradeClass;
    this.Init = function () {
        $("#BtnclearCondition").click(function () {
            $("#searchtype").prop("selectedIndex", 0);
            $("#searchkey").val('');
            $("#divcondition select").prop("selectedIndex", 0);
            $("#selpro").change();
            $('#selschoolid').selectpicker("refresh");
            $('#selagency').selectpicker("refresh");
            $('#selmaster').selectpicker("refresh");

            $("#seldepttree").ddTreeClearValue();
            $("#searchbtn").click();

        })
        Current.TabInit();
        Current.InitAreaSelect();
        Current.InitDeptDropdownTree();
        Current.InitTotal();
        //代理商选项卡
        $("#adaili").click(function () {
            $("#BtnclearCondition").click();
            $("#divmastername").hide();
            $("#divdept").hide();
            $("#divdaili").show();
            $("#hidtab").val("1");
            $("#classcount").text('');
            $("#stucount").text('');
            $('#tb_yunying').bootstrapTable("destroy");
            Current.TabInit();
        })
        //员工选项卡
        $("#amaster").click(function () {
            $("#BtnclearCondition").click();
            $("#divmastername").show();
            $("#divdept").show();
            $("#divdaili").hide();
            $("#hidtab").val("0");
            $("#classcount").text('');
            $("#stucount").text('');
            $('#tb_yunying').bootstrapTable("destroy");
            Current.TabInit();
        })
        //搜索
        $("#searchbtn").click(function () {
            $('#tb_yunying').bootstrapTable('selectPage', 1);

        });

        //同步数据
        $("#BtnReloadMetaData").click(function () {
            loadmask.Show();
            $.ajax({
                type: "post",
                url: "ReloadMetaData",
                dataType: 'json',
                success: function (data) {
                    if (data.Success) {
                        alert("同步成功");
                        $("#searchbtn").click();
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    loadmask.Hide();
                },
                error: function () { }
            });




        })

        //导出表格
        $("#exportbtn").click(function () {
            var params = { limit: 0, offset: 0 };
            var obj = Current.GetParams(params);
            var $form = $('<form target="down-file-iframe" method="post" />');
            $form.attr('action', "/ClassStatis/ExportStatisXls");
            for (var key in obj) {
                $form.append('<input type="hidden" name="' + key + '" value="' + obj[key] + '" />');
            }
            $(document.body).append($form);
            $form.submit();
            //$.post("/Order/ExportOrderXls", obj);
            $form.remove();
        })

        var t = Common.QueryString.GetValue("t");
        if (t == "0") {
            $("#amaster").click();
        } else if (t == "1") {
            $("#adaili").click();
        }

    }

    this.InitAreaSelect = function () {
        $("#selpro").change(function () {
            $("#selcity").html('<option value="">全部</option>');
            $("#selcity").change();
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { parentid: v };
            $.post("/Order/GetAreaList", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var result = data.Data[i];
                            var html = '';
                            html += ' <option value="' + result.Seq + '">' + result.CodeName + '</option>';
                            $(html).appendTo("#selcity");
                        }
                    }
                }
            });
        })
        $("#selcity").change(function () {
            $("#selarea").html('<option value="">全部</option>');
            $("#selarea").change();
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { parentid: v };
            $.post("/Order/GetAreaList", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var result = data.Data[i];
                            var html = '';
                            html += ' <option value="' + result.Seq + '">' + result.CodeName + '</option>';
                            $(html).appendTo("#selarea");
                        }
                    }
                }
            });
        })
        $("#selarea").change(function () {
            $("#selschoolid").html('<option value="">全部</option>');
            $('#selschoolid').selectpicker("refresh");
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { areaID: v };
            $.post("/Order/GetSchoolInfo", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var result = data.Data[i];
                            var html = '';
                            html += ' <option value="' + result.ID + '">' + result.SchoolName + '</option>';
                            $(html).appendTo("#selschoolid");
                        }
                        $('#selschoolid').selectpicker("refresh");
                    }
                }
            });
        })
        $('#selschoolid').change(function () {
            $("#selgradeid").html('<option value="">全部</option>');
            $("#selclass").html('<option value="">全部</option>');
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { schoolID: v };
            $.post("/Order/GetSchollGradeClass", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        Current.GradeClass = data.Data;
                        for (var i = 0; i < Current.GradeClass.length; i++) {
                            var result = Current.GradeClass[i];
                            var html = '';
                            html += ' <option value="' + result.GradeID + '">' + result.GradeName + '</option>';
                            $(html).appendTo("#selgradeid");
                        }
                    }
                }
            });
        })
        $("#selgradeid").change(function () {
            $("#selclass").html('<option value="">全部</option>');
            var gid = $(this).val();
            if (gid == "") {
                return;
            }
            for (var i = 0; i < Current.GradeClass.length; i++) {

                var result = Current.GradeClass[i];
                if (result.GradeID == gid) {
                    for (var j = 0; j < result.ClassList.length; j++) {
                        var html = '';
                        html += ' <option value="' + result.ClassList[j].ID + '">' + result.ClassList[j].ClassName + '</option>';
                        $(html).appendTo("#selclass");
                    }
                }
            }
        })

        var obj = { parentid: 0 };

        //根据数据查看权限，动态加载部门负责区域或个人区域
        //var obj = { }
        //$.post("/Order/GetAreaId", "", function (data) {
        //    if (data.Success) {
        //        if (data.Data) {
        //            obj = { parentid: data.Data.areaID
        //        };
        //    }
        //}
        //})

        $.post("/Order/GetAreaList", obj, function (data) {
            if (data.Success) {
                if (data.Data) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var result = data.Data[i];
                        var html = '';
                        html += ' <option value="' + result.Seq + '">' + result.CodeName + '</option>';
                        $(html).appendTo("#selpro");

                    }
                }
            }
        })

    }

    this.InitDeptDropdownTree = function () {
        $.post("GetCurrentDeptList", null, function (result) {
            if (!result || result.length == 0) {
                result.push({ tag: "", ParentId: "", text: "全部", nodes: null, isContainNods: false })
            }
            $("#seldepttree").dropdownTree(result);
        })
    }

    this.InitTotal = function () {
        $("#classcount").text('');
        $("#stucount").text('');
        var param = { limit: 0, offset: 0 };
        $.post("GetTotalClassCount", Current.GetParams(param), function (data) {
            $("#classcount").text(data.ClassCount);
            $("#stucount").text(data.StuCount);
        })
    }

    this.TabInit = function () {
        $('#tb_yunying').bootstrapTable({
            url: '/ClassStatis/GetPageList',     //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: Current.GetParams,     //传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            //search: true,                     //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            //strictSearch: true,
            showColumns: false,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            // height: 500,                     //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度 设置之后表头内容无法对齐。
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            //showToggle: true,                 //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: Current.GetColumn()
        }).on('load-success.bs.table', function (data) {
            Current.InitTotal();
        });


    }

    this.GetColumn = function () {

        var column1 = [{ field: 'DeptName', title: '部门' },
            { field: 'TrueName', title: '员工姓名' },
           {
               field: 'Path',
               title: '省',
               formatter: function (value, row, index) {
                   if (value != undefined) {
                       var v = value.split(' ');
                       return v[0];
                   }
               }
           }, {
               field: 'Path',
               title: '市',
               formatter: function (value, row, index) {
                   if (value != undefined) {
                       var v = value.split(' ');
                       if (v.length > 2) {
                           return v[1];
                       }
                       else {
                           return v[0];
                       }
                   }
               }
           }, {
               field: 'Path',
               title: '区/县',
               formatter: function (value, row, index) {
                   if (value != undefined) {
                       var v = value.split(' ');
                       if (v.length > 2) {
                           return v[2];
                       }
                       else {
                           return v[1];
                       }
                   }
               }
           }, {
               field: 'SchoolName',
               title: '学校'
           }
                , {
                    field: 'ClassName',
                    title: '班级'
                },
                {
                    field: 'yuwen',
                    title: '语文老师信息',
                    formatter: function (value, row, index) {
                        var v = value;
                        if (row.yuwenMobile) {
                            v += "/" + row.yuwenMobile;
                        }
                        return v;
                    }
                }, {
                    field: 'shuxue',
                    title: '数学老师信息',
                    formatter: function (value, row, index) {
                        var v = value;
                        if (row.shuxueMobile) {
                            v += "/" + row.shuxueMobile;
                        }
                        return v;
                    }
                }, {
                    field: 'yingyu',
                    title: '英语老师信息',
                    formatter: function (value, row, index) {
                        var v = value;
                        if (row.yingyuMobile) {
                            v += "/" + row.yingyuMobile;
                        }
                        return v;
                    }
                }, {
                    field: 'StuCount',
                    title: '绑定人数'
                }, {
                    field: 'xx',
                    title: '操作',
                    formatter: function (value, row, index) {
                        if (detailAction == "True" && row.StuCount) {
                            var v = "<a href='StudentList?CID=" + row.ClassID + "&t=0'>学生详情</a>";
                            return v;
                        }
                        else {
                            return "";
                        }
                    }
                }
        ];
        var column2 = [
            {
                field: 'AgentName', title: '代理商', formatter: function (value, row, index) {
                    if (!value) {
                        return row.DeptName;
                    }
                    return value;
                }
            },
            { field: 'TrueName', title: '员工姓名' },
              {
                  field: 'Path',
                  title: '省',
                  formatter: function (value, row, index) {
                      if (value != undefined) {
                          var v = value.split(' ');
                          return v[0];
                      }
                  }
              }, {
                  field: 'Path',
                  title: '市',
                  formatter: function (value, row, index) {
                      if (value != undefined) {
                          var v = value.split(' ');
                          if (v.length > 2) {
                              return v[1];
                          }
                          else {
                              return v[0];
                          }
                      }
                  }
              }, {
                  field: 'Path',
                  title: '区/县',
                  formatter: function (value, row, index) {
                      if (value != undefined) {
                          var v = value.split(' ');
                          if (v.length > 2) {
                              return v[2];
                          }
                          else {
                              return v[1];
                          }
                      }
                  }
              }, {
                  field: 'SchoolName',
                  title: '学校'
              }
                , {
                    field: 'ClassName',
                    title: '班级'
                },
                  {
                      field: 'yuwen',
                      title: '语文老师信息',
                      formatter: function (value, row, index) {
                          var v = value;
                          if (row.yuwenMobile) {
                              v += "/" + row.yuwenMobile;
                          }
                          return v;
                      }
                  }, {
                      field: 'shuxue',
                      title: '数学老师信息',
                      formatter: function (value, row, index) {
                          var v = value;
                          if (row.shuxueMobile) {
                              v += "/" + row.shuxueMobile;
                          }
                          return v;
                      }
                  }, {
                      field: 'yingyu',
                      title: '英语老师信息',
                      formatter: function (value, row, index) {
                          var v = value;
                          if (row.yingyuMobile) {
                              v += "/" + row.yingyuMobile;
                          }
                          return v;
                      }
                  }, {
                      field: 'StuCount',
                      title: '绑定人数'
                  }, {
                      field: 'xx',
                      title: '操作',
                      formatter: function (value, row, index) {
                          if (detailAction == "True" && row.StuCount) {
                              var v = "<a href='StudentList?CID=" + row.ClassID + "&t=1'>学生详情</a>";
                              return v;
                          }
                          else {
                              return "";
                          }
                      }
                  }
        ];
        var tab = $("#hidtab").val();
        if (tab == "0") {
            return column1;
        } else {
            return column2;
        }
    }

    this.GetParams = function (params) {
        var areacode = '';
        if ($("#selpro").val()) {
            areacode = $("#selpro").val();
        }
        if ($("#selcity").val()) {
            areacode = $("#selcity").val();
        }
        if ($("#selarea").val()) {
            areacode = $("#selarea").val();
        }
        var mType = $("#hidtab").val();
        var mName;
        if (mType == "0") {
            mName = $("#selmaster").val();
        }
        else {
            mName = $("#selagency").val();
        }
        var deptid = $("#seldepttree").ddTreeGetValue();
        if (deptid == "0") { deptid = ""; }
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset  //页码
            , SearchKey: $("#searchkey").val()
            , AreaCode: areacode
            , SchoolID: $("#selschoolid").val()
            , GradeID: $("#selgradeid").val()
            , ClassID: $("#selclass").val()
            , Dept: deptid
            , MasterName: mName
            , DailiOrYuangong: $("#hidtab").val()
            , MasterType: $("#hidtab").val()
        };
        return temp;
    }
}
