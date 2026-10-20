using System.Text.Json;
using System.Text.Json.Serialization;
using static Game;

public class TEnumListConverterFactory : JsonConverterFactory {
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && (typeToConvert.GetGenericTypeDefinition() == typeof(TEnumList<,>));

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter)Activator.CreateInstance(
            typeof(TEnumListConverter<,>).MakeGenericType(typeToConvert.GetGenericArguments()))!;
}

public class TEnumListConverter<TKey, TValue> : JsonConverter<TEnumList<TKey, TValue>>
    where TKey : struct, Enum {
    public override TEnumList<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        Dictionary<string, TValue> raw =
            JsonSerializer.Deserialize<Dictionary<string, TValue>>(ref reader, options) ?? new();
        TEnumList<TKey, TValue> result = new();
        foreach (KeyValuePair<string, TValue> entry in raw) {
            if (Enum.TryParse(entry.Key, out TKey key)) {
                result[key] = entry.Value;
            }
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, TEnumList<TKey, TValue> value, JsonSerializerOptions options) {
        Dictionary<string, TValue> raw = Enum.GetValues<TKey>()
            .ToDictionary(key => key.ToString(), key => value[key]);
        JsonSerializer.Serialize(writer, raw, options);
    }
}
