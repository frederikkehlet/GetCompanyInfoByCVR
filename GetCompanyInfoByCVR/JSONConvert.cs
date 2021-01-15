using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace GetCompanyInfoByCVR
{
    public static class JSONConvert
    {
        public static T Deserialize<T>(string content)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true
                };

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), settings);
                T response = (T)serializer.ReadObject(stream);

                return response;
            }
        }

        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0;

            StreamReader streamReader = new StreamReader(stream);
            string result = streamReader.ReadToEnd();

            return result;
        }
    }
}
