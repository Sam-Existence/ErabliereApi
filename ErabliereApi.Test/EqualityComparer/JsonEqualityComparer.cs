using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ErabliereApi.Test.EqualityComparer;

public class JsonComparer<T> : IEqualityComparer<T> where T : class
{
    public JsonSerializerOptions JsonSerializerOptions { get; }

    public JsonComparer()
    {
        JsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public JsonComparer(JsonSerializerOptions jsonSerializerOptions)
    {
        JsonSerializerOptions = jsonSerializerOptions;
    }

    public bool Equals([AllowNull] T x, [AllowNull] T y)
    {
        var a = JsonSerializer.Serialize(x, JsonSerializerOptions);
        var b = JsonSerializer.Serialize(y, JsonSerializerOptions);

        return a == b;
    }

    public int GetHashCode([DisallowNull] T obj)
    {
        return obj.GetHashCode();
    }
}
