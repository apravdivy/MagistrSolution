using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Json;

namespace Common
{
    public sealed class JSONHelper
    {
        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            string retVal = null;
            using (MemoryStream ms = new MemoryStream())
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
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    obj = (T)serializer.ReadObject(ms);
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
