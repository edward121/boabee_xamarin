using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BoaBeeLogic
{
    public static class extensions
    {
        public static List<T> CopyList<T>(this List<T> source)
        {
            List<T> lstCopy = new List<T>();
            foreach (var item in source)
            {
                T itemCopy = item.Copy();
                lstCopy.Add(itemCopy);
            }
            return lstCopy;
        }

        public static bool isNullOrEmpty<T>(this List<T> source)
        {
            return (source == null || source.Count == 0);
        }

        public static T Copy<T>(this T source)
        {
            T copy = default(T);
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.AutoFlush = true;
                StreamReader reader = new StreamReader(stream);

                writer.Write(JsonConvert.SerializeObject(source));
                stream.Position = 0;

                copy = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }

            return copy;
        }
    }
}

