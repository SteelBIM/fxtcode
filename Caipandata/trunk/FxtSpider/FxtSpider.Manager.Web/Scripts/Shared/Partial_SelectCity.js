var citySelectDom=null;
$(function () {
    $("#selectCityPanel").find("select").select2();
    /**缓存所有城市数据到页面**/
    citySelectDom=$("#selectCity").clone();
    $("#selectCity").find(".city").remove();
    /**选择省份_获取城市**/
    $("#selectProvinc").bind("change",function(){
        var provincId=$("#selectProvinc").val();
        SelectProvinc_GetCity(provincId);       
    });
    /**选择城市_获取行政区**/
    $("#selectCity").bind("change",function(){
        var cityName=$("#selectCity").val().split(',')[1];
        SelectCity_GetArea(cityName);
    });
});
/**选择省份_获取城市**/
function SelectProvinc_GetCity(provincId)
{
    $("#selectCity").find(".city").remove();
    $("#selectCity").select2();/**调用插件**/
    $("#selectArea").find(".area").remove();
    $("#selectArea").select2();/**调用插件**/
    var cityDom=citySelectDom.find(".province_"+provincId);
    $("#selectCity").append(cityDom);

}
/**选择城市_获取行政区**/
function SelectCity_GetArea(_cityName)
{
    $("#selectArea").find(".area").remove();
    $("#select_areaList").find(".area").remove();
    $("#selectArea").select2();/**调用插件**/
    GetFxtAreaByCityName(_cityName,function(data){
        if(data!=null)
        {
            var optionHtml="<option value=\"{0}\,{1}\" class=\"area\">{1}</option>";
            var optionHtml2="<option value=\"{0}\" class=\"area\">{1}</option>";
            for(var i=0;i<data.length;i++)
            {
                var id=data[i].AreaId;
                var name=decodeURIComponent(data[i].AreaName);
                var html=optionHtml._StringFormat(id,name);
                var html2=optionHtml2._StringFormat(name,name);
                $("#selectArea").append(html);
                $("#select_areaList").append(html2);
            }
        }
    });
}