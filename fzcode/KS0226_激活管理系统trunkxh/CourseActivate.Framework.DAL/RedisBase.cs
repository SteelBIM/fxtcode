using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Framework.DAL
{
    public class RedisBase : IDisposable
    {
        public static IRedisClient _Core;
        private static object _locker = new object();
        private bool _disposed = false;
        //public RedisBase()
        //{
        //    Core = RedisRepository.GetClient();
        //}

        public static IRedisClient Core
        {
            get
            {
                if (_Core == null)
                {
                    lock (_locker)
                    {
                        if (_Core != null)
                            return _Core;

                        _Core = RedisRepository.GetClient();
                        return _Core;
                    }
                }
                return _Core;
            }
            private set
            {
                _Core = value;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Core.Dispose();
                    Core = null;
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 保存数据DB文件到硬盘
        /// </summary>
        public void Save()
        {
            Core.Save();
        }
        /// <summary>
        /// 异步保存数据DB文件到硬盘
        /// </summary>
        public void SaveAsync()
        {
            Core.SaveAsync();
        }

        /// <summary>
        /// 清除本数据库所有的数据
        /// </summary>
        public void FlushDB()
        {
            Core.FlushDb();
        }
    }
}
