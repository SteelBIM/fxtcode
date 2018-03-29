$(function(){
    $("#btnSubmit").bind("click",function(){
        SubmitCase();
    });
    $("#txtTotalPrice").bind("focus",function(){
        ComputeTotalPrice();
    });
    $("#txtBuildingArea").bind("input",function(){
    
        ComputeTotalPrice();
    });
    $("#txtUnitPrice").bind("input",function(){
        ComputeTotalPrice();
    });

});
function ComputeTotalPrice()
{        
    var _Pat = /^\d+\.*\d*$/;
    var utPrice=$("#txtUnitPrice").val();
    var area=$("#txtBuildingArea").val();
    if(_Pat.test(utPrice)&&_Pat.test(area))
    {
        $("#txtTotalPrice").val(utPrice*area);
    }
}
function SubmitCase()
{
    CloseError();
    var _fxtCityId=$("#txtProjectName").attr("data-fxtCityId");
    var _projectId=$("#txtProjectName").attr("data-projectId");
    var _buildingId=null;var _houseNo=null;
    var _caseDate=$("#txtCaseDate").val();
    var _purposeCode=$("#selectPurposeCode").val();
    var _buildingArea=$("#txtBuildingArea").val();
    var _unitPrice=$("#txtUnitPrice").val();
    var _totalPrice=$("#txtTotalPrice").val();
    var _caseTypeCode=$("#selectCaseTypeCode").val();
    var _structureCode=$("#selectStructureCode").val();
    var _buildingTypeCode=$("#selectBuildingTypeCode").val();
    var _floorNumber=$("#txtFloorNumber").val();
    var _totalFloor=$("#txtTotalFloor").val();
    var _houseTypeCode=$("#selectHouseTypeCode").val();
    var _frontCode=$("#selectFrontCode").val();
    var _moneyUnitCode=$("#selectMoneyUnitCode").val();
    var _remark=$("#txtRemark").val();
    var _areaId=$("#selectArea").val();
    var _buildingDate=$("#txtBuildingDate").val();
    var _fitmentCode=$("#selectFitmentCode").val();
    var _subHouse=$("#txtSubHouse").val();
    var _peiTao=$("#txtPeiTao").val();
    var _createUser=$("#txtCreateUser").val();
    var _sourceName=$("#txtSourceName").val();
    var _sourceLink=$("#txtSourceLink").val();
    var _sourcePhone=$("#txtSourcePhone").val();
    var _Pat = /^\d+\.*\d*$/;
    var _Pat2 = /^\d*$/;
    var bol =false;
    var _actionType=$("#btnSubmit").attr("data-actionType");
    var _caseId=null;
    if(_actionType=="update")
    {
        _caseId=$("#btnSubmit").attr("data-caseId");
    }
    /**验证非空**/
    if(_purposeCode=="0")
    {
        ShowError("请选择用途!",".selectPurposeCode");
        return;
    }
    if(_buildingArea=="")
    {
        ShowError("面积不能为空!",".txtBuildingArea");
        return;
    }
    if(_unitPrice=="")
    {
        ShowError("单价不能为空!",".txtUnitPrice");
        return;
    }
    if(_totalPrice=="")
    {
        ShowError("总价不能为空!",".txtTotalPrice");
        return;
    }  
    if(_caseTypeCode=="0")
    {
        ShowError("请选择案例类型!",".selectCaseTypeCode");
        return;
    }
    if(_caseDate=="")
    {
        ShowError("请填写案例时间!",".txtCaseDate");
        return;
    }
    /**验证字符**/
    bol = _Pat.test(_buildingArea);
    if(!bol)
    {
        ShowError("请正确填写面积!",".txtBuildingArea");
        return;
    }
    bol = _Pat.test(_unitPrice);
    if(!bol)
    {
        ShowError("请正确填写单价!",".txtUnitPrice");
        return;
    }  
    bol = _Pat.test(_unitPrice);
    if(!bol)
    {
        ShowError("请正确填写总价!",".txtTotalPrice");
        return;
    }
    if(_floorNumber!="")
    {
        bol = _Pat2.test(_floorNumber);
        if(!bol)
        {
            ShowError("楼层填写不正确!",".txtFloorNumber");
            return;
        }
    }
    if(_totalFloor!="")
    {
        bol = _Pat2.test(_totalFloor);
        if(!bol)
        {
            ShowError("总楼层填写不正确!",".txtTotalFloor");
            return;
        }
    }
    /*
    if(new Date(_caseDate)=="Invalid Date")
    {
        ShowError("案例时间格式错误!",".txtCaseDate");
        return;
    }
    */
    $("#btnSubmit").val("提交中...");
    var paraJson={
            actionType:_actionType,
            caseId:_caseId,
            fxtCityId:_fxtCityId,
            projectId:_projectId,
            caseDate:encodeURIComponent(_caseDate),
            purposeCode:_purposeCode,
            buildingArea:_buildingArea,
            unitPrice:_unitPrice,
            totalPrice:_totalPrice,
            caseTypeCode:_caseTypeCode,
            structureCode:_structureCode,
            buildingTypeCode:_buildingTypeCode,
            floorNumber:_floorNumber,
            totalFloor:_totalFloor,
            houseTypeCode:_houseTypeCode,
            frontCode:_frontCode,
            moneyUnitCode:_moneyUnitCode,
            remark:encodeURIComponent(_remark),
            areaId:_areaId,
            buildingDate:_buildingDate,
            fitmentCode:_fitmentCode,
            subHouse:encodeURIComponent(_subHouse),
            peiTao:encodeURIComponent(_peiTao),
            createUser:encodeURIComponent(_createUser),
            sourceName:encodeURIComponent(_sourceName),
            sourceLink:encodeURIComponent(_sourceLink),
            sourcePhone:encodeURIComponent(_sourcePhone)
    }
    $.extendAjax(
                {   
                    url: "/OperationMaintenance/SetCase_Submit_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function(data)
                {
                    $("#btnSubmit").val("确定");
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            ShowError(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            alert("保存成功!");
                            parent.SetCase_Fancybox_Response(data.detail.CaseObj,data.detail.AreaList,_actionType);
                            parent.$.fancybox.close();
                        }
                    }
                },
               {dom:"#formPanel"});
}


function ShowError(txt,dom)
{
   $(dom).removeClass("error").addClass("error");
   $("#titleError").show();
   $("#titleError").html(txt);
}
function CloseError()
{
   $(".error").removeClass("error");
   $("#titleError").html("");
   $("#titleError").hide();
}