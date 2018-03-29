using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace FXT.DataCenter.Infrastructure.Redis
{
    public class RedisConnection
    {

        static readonly string RedisIp = Convert.ToString(ConfigurationManager.AppSettings["RedisIp"]);

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(RedisIp));

        public static  ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
