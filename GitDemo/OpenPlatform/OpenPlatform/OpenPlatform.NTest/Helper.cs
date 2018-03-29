using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
/*
 * Newtonsoft.Json 与 Newtonsoft.Json.Schema 是不同的包，通过NuGet分别引用
 */

namespace OpenPlatform.NTest
{
    sealed public class JsonHelper
    {
        public static bool IsJsonFormat(string str)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(str));
            bool isJson = false;
            if (reader.Read())
            {
                isJson = JsonToken.StartObject == reader.TokenType;
            }
            return isJson;
        }
    }
}

