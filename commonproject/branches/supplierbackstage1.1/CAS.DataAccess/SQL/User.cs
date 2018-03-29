using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.DataAccess.SQL
{
    public class User
    {
        /// <summary>
        /// 云查勘手机登录
        /// </summary>
        public const string LoginBySurveyMobile = @"
            if not exists(select FK_UserId from dbo.Privi_Group_User_City with(nolock) where FK_UserId = @userid and FK_SysTypeCode = @systypecode)
                begin
                select -1
                end
            else
                begin
	            select 1 [state], u.userid,u.username,u.usertoken,u.FK_CityId as cityid,u.FK_FXT_CompanyId as fxtcompanyId , c.ProvinceId, P.ProvinceName,u.FK_CompanyId as  companyId , Lower(substring(convert(varchar(50),newid()),1,10))as pascode,1 online from dbo.Privi_User u with(nolock)   join dbo.Privi_Group_User_City guc with(nolock) 
                on u.userid = guc.FK_UserId and guc.FK_SysTypeCode = @SysTypeCode and u.password =  @password  and u.userid =  @userid  and u.FK_CompanyId=u.FK_FXT_CompanyId   and u.Valid=1
	            join dbo.SYS_City c with(nolock)  on u.FK_CityId=c.cityid
	            join dbo.SYS_Province P with(nolock)  on P.ProvinceId=c.ProvinceId;
                end
            ";
    }
}
