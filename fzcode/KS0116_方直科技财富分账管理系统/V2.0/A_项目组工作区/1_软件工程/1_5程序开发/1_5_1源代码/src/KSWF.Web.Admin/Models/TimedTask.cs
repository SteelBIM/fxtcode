using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using KSWF.Web.Admin.Controllers;
using KSWF.Framework.BLL;
using KSWF.WFM.Constract.Models;

namespace KSWF.Web.Admin.Models
{
    public class TimedTask : IJob, IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;
        public Registry Start()
        {
            HostingEnvironment.RegisterObject(this);
            Registry registry = new Registry();
            registry.Schedule(() => Execute()).ToRunEvery(1).Days().At(1,30);//每天几点执行一次代码
            //registry.Schedule(() => Execute()).ToRunEvery(1).Seconds();//每小时执行一次
            return registry;
        }
        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                else
                {
                    ClassStatisController classstatis = new ClassStatisController();
                    classstatis.TimedSynchronization();
                }
            }
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

    }
}
