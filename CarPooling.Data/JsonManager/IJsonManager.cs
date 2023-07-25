using System.Collections.Generic;

namespace CarPooling.Data.JsonManager
{
    public interface IJsonManager
    {
        List<T> ExtractTypesFromJson<T>(string directory);
    }
}
