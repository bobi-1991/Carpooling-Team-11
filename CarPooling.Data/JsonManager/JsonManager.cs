using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CarPooling.Data.JsonManager
{
    public class JsonManager : IJsonManager
    {
        public JsonManager()
        {

        }
        public List<T> ExtractTypesFromJson<T>(string directory)
        {
            var jsonToExtractFrom = File.ReadAllText(directory);
            var objects = JsonConvert.DeserializeObject<T[]>(jsonToExtractFrom);
            var result = new List<T>();
            foreach (var item in objects)
                result.Add(item);
            return result;
        }
    }
}
