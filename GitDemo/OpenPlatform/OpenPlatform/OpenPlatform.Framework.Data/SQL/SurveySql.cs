using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Framework.Data.SQL
{
   public struct SurveySql
   {
       /// <summary>
       ///  获取评估机构ID（SId修改为身份证ID 13805）
       /// </summary>
       public const string GetCompanyId = @"select FxtCompanyId from FxtSurveyCenter.dbo.Dat_Survey where IDCards like  @idNum";

       /// <summary>
       ///  获取照片(s.objectId修改为身份证ID)
       /// </summary>
       public const string GetPicturesById = @"SELECT f.Path,f.Name,f.Smallimgpath,f.Annextypecode,f.Annextypesubcode,f.Imagetype,f.Filesize,f.Flietypecode,f.Filesubtypecode,f.Remark,f.Filecreatedate,f.UpTime,f.CreateDate
FROM FxtSurveyCenter.dbo.Dat_Files f WITH (NOLOCK)
inner join FxtSurveyCenter.dbo.Dat_Survey s with(nolock)
on s.ObjectId = f.ObjectId
WHERE s.ObjectId = @objectId";

       /// <summary>
       /// 获取查勘信息
       /// </summary>
       public const string GetSurveyInfoById = @"SELECT CASE 
		WHEN (
				X IS NOT NULL
				OR Y IS NOT NULL
				)
			THEN 1
		ELSE 0
		END AS IsSurvey
	,UserId AS Surveyor
	,CustomFields
    ,IDCards as IdNum
    ,BeginTime as SurveyBeginTime
    ,CompleteTime as SurveyEndTime
FROM FxtSurveyCenter.dbo.Dat_Survey WITH (NOLOCK)
WHERE ObjectId = @objectId";

   }
}
