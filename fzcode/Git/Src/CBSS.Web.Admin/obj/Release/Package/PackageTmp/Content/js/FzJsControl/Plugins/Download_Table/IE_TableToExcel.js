////////支持IE FF  Js导出
function toExcel(inTblId, inWindow, titleName, sheetName, fileName) {
    //if (Common.Browser.msie) {
    if (window.ActiveXObject !== undefined) {
        try {
            var allStr = ""; var curStr = "";
            if (inTblId != null && inTblId != "" && inTblId != "null") {
                curStr = getTblData(inTblId, inWindow, titleName, sheetName);
            }
            if (curStr != "" && curStr != null) {
                allStr += curStr;
            }
            else {
//                alert("数据源没有没有相应的信息！");
                art.dialog.tips('数据源没有没有相应的信息！');
                return;
            }
            var fileName = getExcelFileName(fileName);
            doFileExport(fileName, allStr);

        }
        catch (e) {
            alert("导出发生异常:" + e.name + "->" + e.description + "!");
        }
    }
}

function getTblData(inTbl, inWindow, titleName, sheetName) {
    var rows = 0; var tblDocument = document;
    if (!!inWindow && inWindow != "") {
        if (!document.all(inWindow)) {
            return null;
        } else {
            tblDocument = eval(inWindow).document;
        }
    }
    var curTbl = tblDocument.getElementById(inTbl);
    if (curTbl.rows.length > 65000) {
        alert('源行数不能大于65000行');
        return false;
    }
    if (curTbl.rows.length <= 2) {
//        alert('数据源没有数据');
        return false;
    }
    var outStr = "";
    var rowc = 0;
    if (curTbl != null) {
        var el = curTbl;
        var excel = "";
        $(el).find('thead').find('tr').each(function () {
            excel += "<tr>";
            $(this).filter(':visible').find('td').each(function (index, data) {
                if ($(this).css('display') != 'none') {
                    rowc++;
                    /////5-25修改 判断跨行跨列
                    var temp = "";
                    //if (this.rowSpan != undefined || this.colSpan != undefined) {
                    temp = "rowSpan='" + this.rowSpan + "'" + " colSpan='" + this.colSpan + "'";
                    //}
                    excel += "<td " + temp + " style=\" text-align:center;\">" + parseString($(this)) + "</td>";

                }
            });
            excel += '</tr>';
        });

        
        var rowCount = 1;
        $(el).find('tbody').find('tr').each(function () {
            if (rowCount != $(el).find('tbody').find('tr').length - 1) {
                excel += "<tr>";
                var colCount = 0;
                $(this).filter(':visible').find('td').each(function (index, data) {
                    if ($(this).css('display') != 'none') {
                        /////5-25修改 判断跨行跨列
                        var temp = "";
                        //if (this.rowSpan != undefined || this.colSpan != undefined) {
                        temp = "rowSpan='" + this.rowSpan + "'" + " colSpan='" + this.colSpan + "'";
                        //}
//                        excel += "<td " + temp + " style=\" text-align:center;\">" + parseString($(this)) + "</td>";
                        if (index != 1) {
                            excel += "<td " + temp + " style=\" text-align:center;\">" + parseString($(this)) + "</td>";
                        } else {
                            excel += "<td " + temp + " style=\" text-align:left;\">" + parseString($(this)) + "</td>";
                        }
                    }
                    colCount++;
                });
                rowCount++;
                excel += '</tr>';
            }
        });

        // tfoot 合计
        $(el).find('tfoot').find('tr').each(function () {
            excel += "<tr>";
            $(this).filter(':visible').find('td').each(function (index, data) {
                if ($(this).css('display') != 'none') {
                   
                    /////5-25修改 判断跨行跨列
                    var temp = "";
                    //if (this.rowSpan != undefined || this.colSpan != undefined) {
                    temp = "rowSpan='" + this.rowSpan + "'" + " colSpan='" + this.colSpan + "'";
                    //}
                    excel += "<td " + temp + " style=\" text-align:center;\">" + parseString($(this)) + "</td>";

                }
            });
            excel += '</tr>';
        });
        excel = excel.replace("<tr></tr>", "");
        var tempTit = "";
        tempTit = "<tr><td colSpan=\"" + rowc + "\" style=\" text-align:center; font-size:18px;\">" + titleName + "</td></tr>";
        excel = "<table>" + tempTit + excel + "</table>";

        var excelFile = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:" + "excel" + "' xmlns='http://www.w3.org/TR/REC-html40'>";
        excelFile += "<head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />";
        excelFile += "<!--[if gte mso 9]>";
        excelFile += "<xml>";
        excelFile += "<x:ExcelWorkbook>";
        excelFile += "<x:ExcelWorksheets>";
        excelFile += "<x:ExcelWorksheet>";
        excelFile += "<x:Name>";
        //excelFile += "{worksheet}";
        //excelFile += decodeURI(defaults.sheetName);
        excelFile += sheetName;
        excelFile += "</x:Name>";
        excelFile += "<x:WorksheetOptions>";
        excelFile += "<x:DisplayGridlines/>";
        excelFile += "</x:WorksheetOptions>";
        excelFile += "</x:ExcelWorksheet>";
        excelFile += "</x:ExcelWorksheets>";
        excelFile += "</x:ExcelWorkbook>";
        excelFile += "</xml>";
        excelFile += "<![endif]-->";
        excelFile += "</head>";
        excelFile += "<body>";
        excelFile += excel;
        excelFile += "</body>";
        excelFile += "</html>";

        outStr = excelFile;
    } else {
        outStr = null; alert(inTbl + "不存在!");
    }
    //outStr = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>测试</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table><tr><td rowSpan='2' colSpan='1'>AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA</td><td rowSpan='1' colSpan='2'>B</td><td rowSpan='1' colSpan='1'>C</td><td rowSpan='1' colSpan='1'>D</td></tr><tr><td rowSpan='1' colSpan='1'>B-1</td><td rowSpan='1' colSpan='1'>B-2</td><td rowSpan='1' colSpan='1'>C-1</td><td rowSpan='1' colSpan='1'>D-1</td></tr><tr><td rowSpan='1' colSpan='1'>1</td><td rowSpan='1' colSpan='1'>2</td><td rowSpan='1' colSpan='1'>3</td><td rowSpan='1' colSpan='1'>4</td><td rowSpan='1' colSpan='1'>5</td></tr><tr><td rowSpan='1' colSpan='1'>6</td><td rowSpan='1' colSpan='1'>7</td><td rowSpan='1' colSpan='1'>8</td><td rowSpan='1' colSpan='1'>9</td><td rowSpan='1' colSpan='1'>10</td></tr></table></body></html>";
    return outStr;

}
function getExcelFileName(fileName) {
    var d = new Date(); var curYear = d.getYear(); var curMonth = "" + (d.getMonth() + 1);
    var curDate = "" + d.getDate(); var curHour = "" + d.getHours(); var curMinute = "" +
             d.getMinutes(); var curSecond = "" + d.getSeconds();
    if (curMonth.length == 1) {
        curMonth = "0" + curMonth;
    }
    if (curDate.length == 1) {
        curDate = "0" + curDate;
    }
    if (curHour.length == 1) {
        curHour = "0" + curHour;
    }
    if (curMinute.length == 1) {
        curMinute = "0" + curMinute;
    }
    if (curSecond.length == 1) {
        curSecond = "0" + curSecond;
    }
    //var fileName = fileName + curYear + curMonth + curDate + curHour + curMinute + curSecond + ".xls";
    var fileName = fileName + ".xls";
    return fileName;
}
function doFileExport(inName, inStr) {
    var xlsWin = null;
    if (!!document.all("glbHideFrm")) {
        xlsWin = glbHideFrm;
    } else {
        var width = 1; var height = 1;
        var openPara = "left=" + (window.screen.width / 2 + width / 2) + ",top=" + (window.screen.height + height / 2) +
                 ",scrollbars=no,width=" + width + ",height=" + height;
        xlsWin = window.open("", "_blank", openPara);
    }
    xlsWin.document.write(inStr);
    xlsWin.document.close();
    xlsWin.document.execCommand('Saveas', true, inName);
    xlsWin.close();
}

function parseString(data) {
    return data.html();
}