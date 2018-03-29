using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CDI.Common
{
    public class BinarySerialization
    {

        private static byte[] Serialize(object target)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, target);
                byte[] buffer = ms.ToArray();
                return buffer;
            }
        }

        private static T Deserialize<T>(byte[] bytes) where T : class
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms) as T;
            }
        }

        public static T ReadFromBytes<T>(byte[] bytes) where T : class
        {
            var buffer = EncryptHelper.Decrypt(bytes);
            return Deserialize<T>(buffer);
        }

        public static byte[] WriteToBytes(object obj)
        {
            var buffer = Serialize(obj);
            return EncryptHelper.Encrypt(buffer);
        }
    }
}
