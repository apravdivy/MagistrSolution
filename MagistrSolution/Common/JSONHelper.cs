using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Common
{
    public sealed class JSONHelper
    {
        public static string Serialize<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            string retVal = null;
            using (var ms = new MemoryStream())
            {
                try
                {
                    serializer.WriteObject(ms, obj);
                    retVal = Encoding.Default.GetString(ms.ToArray());
                }
                finally
                {
                    ms.Close();
                }
            }
            return retVal;
        }

        public static T Deserialize<T>(string json)
        {
            var obj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                try
                {
                    var serializer = new DataContractJsonSerializer(obj.GetType());
                    obj = (T) serializer.ReadObject(ms);
                }
                finally
                {
                    ms.Close();
                }
            }
            return obj;
        }
    }
}