using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.InterestDubbingGame.Web
{
    public class TimedTask : IJob, IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;
        public Registry Start()
        {
            Dubbing();
            HostingEnvironment.RegisterObject(this);
            Registry registry = new Registry();
            registry.Schedule(() => Execute()).ToRunEvery(1).Hours();//每天几点执行一次代码
            return registry;
        }
        /// <summary>
        /// 配音
        /// </summary>
        public void Dubbing()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                else
                {
                    //SyncData.DubbingData();//执行
                }
            }
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
                    //SyncData.SyncAllData();//执行
                    //SyncData.AddUserUseRecord();
                    //SyncData.DubbingData();
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