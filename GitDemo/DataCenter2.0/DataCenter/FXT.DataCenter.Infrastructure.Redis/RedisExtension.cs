using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace FXT.DataCenter.Infrastructure.Redis
{
    public static class RedisExtension
    {
       
        public static TResult Get<TResult>(this IDatabase cache, string key)
        {
            //return default(TResult);
            return Deserialize<TResult>(cache.StringGet(key));
        }

        public static void Set<TIn>(this IDatabase cache, string key, TIn value)
        {
            cache.StringSet(key, Serialize(value));
        }

        public static void Set<TIn>(this IDatabase cache, string key, TIn value,TimeSpan expireTime)
        {
             cache.StringSet(key, Serialize(value),expireTime);
        }

        static byte[] Serialize(object o)
        {
            if (o == null)
                return null;
            var binaryFormatter = new BinaryFormatter();
            using (var mStream = new MemoryStream())
            {
                binaryFormatter.Serialize(mStream, o);
                byte[] objectDataAsStream = mStream.ToArray();
                return objectDataAsStream;
            }
        }

        static TResult Deserialize<TResult>(byte[] stream)
        {
            if (stream == null)
                return default(TResult);
            var formatter = new BinaryFormatter();
            using (var memStream = new MemoryStream(stream))
            {
                var result = (TResult)formatter.Deserialize(memStream);
                return result;
            }

        }


    }
}
