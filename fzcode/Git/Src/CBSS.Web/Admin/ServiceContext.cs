using System;
using System.Collections.Generic;
using CBSS.Account.Contract;
using CBSS.Core.Cache;
using CBSS.Core.Service;
using CBSS.Tbx.Contract;
using CBSS.Cfgmanager.IBLL;
using CBSS.Tbx.IBLL;
using CBSS.Account.IBLL;
using CBSS.IBS.IBLL;
using CBSS.UserOrder.IBLL;
using CBSS.ResourcesManager.IBLL;

namespace CBSS.Web
{
    public class ServiceContext
    {
        public static ServiceContext Current
        {
            get
            {
                return CacheHelper.GetItem<ServiceContext>("ServiceContext", () => new ServiceContext());
            }
        }
        
        public IAccountService AccountService
        {
            get
            {
                return ServiceHelper.CreateService<IAccountService>();
            }
        }

        public ITbxService TbxService
        {
            get
            {
                return ServiceHelper.CreateService<ITbxService>();
            }
        }
        public ICfgmanagerService CfgmanagerService
        {
            get
            {
                return ServiceHelper.CreateService<ICfgmanagerService>();
            }
        }

        public IIBSService IBSService
        {
            get
            {
                return ServiceHelper.CreateService<IIBSService>();
            }
        }
        public IUserOrderService UserOrderService
        {
            get
            {
                return ServiceHelper.CreateService<IUserOrderService>();
            }
        }

        public IResourcesService ResourcesService
        {
            get
            {
                return ServiceHelper.CreateService<IResourcesService>();
            }
        }

    }
}
