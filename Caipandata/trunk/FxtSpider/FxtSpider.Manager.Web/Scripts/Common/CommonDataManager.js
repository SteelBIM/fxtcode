/**根据省份ID_获取房讯通的城市列表**/
function GetFxtCityByProvinceId(_provinceId,_function)
{
    $.extendAjax( 
                 { url: "/Common/GetFxtSysCityByProvinceId",
                     data: {provinceId:encodeURIComponent(_provinceId)},
                     type: "post",
                     dataType: "json"
                 },
                 function (data) {
                     var list=null;
                     if(data!=null)
                     {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            list=data.detail;
                        }
                     }
                     _function(list);
                 });
}

/**根据城市ID_获取房讯通的行政区列表**/
function GetFxtAreaByCityId(_cityId,_function)
{
    $.extendAjax( 
                 { url: "/Common/GetFxtSysAreaByCityId_Api",
                     data: {cityId:encodeURIComponent(_cityId)},
                     type: "post",
                     dataType: "json"
                 },
                 function (data) {
                     var list=null;
                     if(data!=null)
                     {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            list=data.detail;
                        }
                     }
                     _function(list);
                 });
}
/**根据城市名称_获取房讯通的行政区列表**/
function GetFxtAreaByCityName(_cityName,_function)
{
    $.extendAjax( 
                 { url: "/Common/GetFxtSysAreaByCityName_Api",
                     data: {cityName:encodeURIComponent(_cityName)},
                     type: "post",
                     dataType: "json"
                 },
                 function (data) {
                     var list=null;
                     if(data!=null)
                     {
                        if(data.result!=1&&data.result!="1")
                        {
                            alert(decodeURIComponent(data.message));                            
                        }
                        else
                        {
                            list=data.detail;
                        }
                     }
                     _function(list);
                 });
}