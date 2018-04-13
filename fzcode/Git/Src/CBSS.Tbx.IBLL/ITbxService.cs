using CBSS.Tbx.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
	/// <summary>
	/// Tbx功能接口注册
	/// </summary>
	public interface ITbxService:ITbxApiService ,IAppService, IAppVersionService, IEnumSetService, IModelService, IModuleService, IMarketBookService, IMarketClassifyService,IMarketMarketBookCatalogService, IModelImgLibrary, IGoodService, IGoodPriceService, IUserInfoService,IAppModuleItemService,IAppSkinVersionService, IAppBookCatalogModuleItemService, ITbxFsService, IHearResourceService, IInterestDubbingRecordService
    {
	}
}
