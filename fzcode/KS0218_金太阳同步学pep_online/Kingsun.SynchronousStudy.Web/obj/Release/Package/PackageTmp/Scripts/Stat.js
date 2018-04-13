///////////配音赛事统计//////////// 

var GameUserListInit = function () {
    var Current = this;
    this.Init = function () {
        Current.InitGameList();
    };
    //初始化列表
    this.InitGameList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=Query",
            pagination: false,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50, 40, 50, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            columns: [[
                    {
                        field: 'Total', title: '报名总人数', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.Total + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'One', title: '一年级', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.One + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'Two', title: '二年级', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.Two + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'Three', title: '三年级', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.Three + '</a>';
                             return html;
                         }
                     },
                      {
                          field: 'Four', title: '四年级', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.Four + '</a>';
                              return html;
                          }
                      },
                      {
                          field: 'Five', title: '五年级', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.Five + '</a>';
                              return html;
                          }
                      },
                      {
                          field: 'Six', title: '六年级', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.Six + '</a>';
                              return html;
                          }
                      },
                      {
                          field: 'Other', title: '其它', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.Other + '</a>';
                              return html;
                          }
                      },
                      {
                          field: 'VoterRecordTotal', title: '投票总人数', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.VoterRecordTotal + '</a>';
                              return html;
                          }
                      }
            ]],
            onDblClickRow: function (rowIndex) {
                //$('#tbdatagrid').datagrid('selectRow', rowIndex);
                //var currentRow = $("#tbdatagrid").datagrid("getSelected");
                //alert(currentRow.ID);
                //UpdateCourse(currentRow.ID);
            }
        }); 
    }

    //通过关键字搜索
    $("#search").click(function () {
        var star = $("#start").val();
        var end = $("#end").val();
        $('#tbdatagrid').datagrid({
            url: '?action=Query&star=' + star + '&end=' + end
        });
    })
} 
var gameListInit;
$(function () {
    gameListInit = new GameUserListInit();
    gameListInit.Init();
});
