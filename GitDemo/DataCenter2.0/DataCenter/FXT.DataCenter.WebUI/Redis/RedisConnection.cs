using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;
using System.Configuration;

namespace FXT.DataCenter.WebUI.Redis
{
    public class RedisConnection
    {
        static readonly string LocalHostIp = ConfigurationManager.AppSettings["RedisConStr"];

        private static readonly Lazy<ConnectionMultiplexer> _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(LocalHostIp));

        public static ConnectionMultiplexer Connection { get { return _connection.Value; } }
    }
}