using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace RequestBalmin.Services
{
    public static class ReadFileService
    {
        public static string GetDataOfFile(string fileContent, string? key, dynamic? value)
        {
            var json = JToken.Parse(fileContent);

            if (key == null || value == null)
            {
                return json.ToString(Formatting.Indented);
            }

            JToken? result = FindJsonElement(json, key, value);

            if (result != null)
            {
                return result.ToString(Formatting.Indented);
            }

            return "Não existem dados com essas especificações";
        }

        private static JToken? FindJsonElement(JToken token, string key, dynamic value)
        {
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                if (obj.ContainsKey(key) && obj[key]?.ToString() == value.ToString())
                {
                    return obj;
                }

                foreach (var property in obj.Properties())
                {
                    JToken? foundToken = FindJsonElement(property.Value, key, value);
                    if (foundToken != null)
                    {
                        return foundToken;
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (var item in token)
                {
                    JToken? foundToken = FindJsonElement(item, key, value);
                    if (foundToken != null)
                    {
                        return foundToken;
                    }
                }
            }

            return null;
        }

        public static string RemoveDataOfFile(string fileContent, string key, dynamic value)
        {
            var json = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(fileContent);

            JsonElement? result = FindElement(json, key, value);

            if (fileContent.Contains(result.Value.ToString()))
            {
                fileContent = fileContent.Replace($"{result.Value},", "");
            }

            return fileContent;
        }

        private static JsonElement? FindElement(JsonElement element, string key, dynamic value)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    if (element.TryGetProperty(key, out var itemValue))
                    {
                        var itemValueString = itemValue.ToString();
                        if (itemValueString != null && itemValueString.Equals(value, StringComparison.Ordinal))
                        {
                            return element;
                        }
                    }

                    foreach (var property in element.EnumerateObject())
                    {
                        JsonElement? foundElement = FindElement(property.Value, key, value);
                        if (foundElement != null)
                        {
                            return foundElement;
                        }
                    }
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        JsonElement? foundElement = FindElement(item, key, value);
                        if (foundElement != null)
                        {
                            return foundElement;
                        }
                    }
                    break;
            }

            return null;
        }
    }
}

