$(function(){
    $("#selectProvince").bind("change",function(){
        GetCitySelectProvince();
    });
    $("#selectCity").bind("change",function(){
        GetAreaSelectCity();
    });
    $("#btnSubmit").bind("click",function(){
        SubmitAddProject();
    });
});
/**选择省份获取城市**/
function GetCitySelectProvince()
{
    var provinceId=$("#selectProvince").val();
    $("#selectCity").find(".city").remove();
    $("#selectCity").select2()
    GetFxtCityByProvinceId(provinceId, function(data){
        if(data!=null)
        {
            var optionHtml="<option value=\"{0}\" class=\"city\">{1}</option>";
            for(var i=0;i<data.length;i++)
            {
                var id=data[i].CityId;
                var name=decodeURIComponent(data[i].CityName);
                var html=optionHtml._StringFormat(id,name);
                $("#selectCity").append(html);
            }
        }
            
   });
}
/**选择城市获取行政区**/
function GetAreaSelectCity()
{
    var cityId=$("#selectCity").val();
    $("#selectArea").find(".area").remove();
    $("#selectArea").select2()
    GetFxtAreaByCityId(cityId,function(data){
        if(data!=null)
        {
            var optionHtml="<option value=\"{0}\" class=\"area\">{1}</option>";
            for(var i=0;i<data.length;i++)
            {
                var id=data[i].AreaId;
                var name=decodeURIComponent(data[i].AreaName);
                var html=optionHtml._StringFormat(id,name);
                $("#selectArea").append(html);
            }
        }
            
   });
}
function SubmitAddProject()
{
    CloseError();
    var _cityId=$("#selectCity").val();
    var _areaId=$("#selectArea").val();
    var _purposeCode=$("#selectPurposeCode").val();
    var _projectName=$("#txtProjectName").val();
    var _address=$("#txtAddress").val();
    if(_cityId==0||_cityId=="0")
    {
        ShowError("请选择城市和行政区!",".selectCity");
        return;
    }
    if(_areaId==0||_areaId=="0")
    {
        ShowError("请选择城市和行政区!",".selectCity");
        return;
    }
    if(_purposeCode==0||_purposeCode=="0")
    {
        ShowError("请选择用途!",".selectPurposeCode");
        return;
    }
    if(_projectName==null||_projectName=="")
    {
        ShowError("请填写楼盘名",".txtProjectName");
        return;
    }
    var paraJson={
            projectName:encodeURIComponent(_projectName),
            cityId:_cityId,
            areaId:_areaId,
            purposeCode:_purposeCode,
            address:encodeURIComponent(_address)
    };
    SubmitLoading();
    $.extendAjax( 
                { url: "/Project/AddProject_FancyboxSubmit_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    SubmitOver();
                    if(data!=null)
                    {
                        if(data.result!=1&&data.result!="1")
                        {
                            ShowError(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            alert("添加成功!");
                            parent.$.fancybox.close();
                        }
                    }
                },{dom:""});
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
function SubmitLoading()
{
  $("#btnSubmit").val("提交中...");
  $("#btnSubmit").attr("disabled",true);
}
function SubmitOver()
{
  $("#btnSubmit").attr("disabled",false);
  $("#btnSubmit").val("确定");
}