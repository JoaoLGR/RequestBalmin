﻿using System.Net.Http.Json;
using System.Text.Json;

namespace RequestBalmin.Services
{
    public class ReadFileService
    {
        public static string GetDataOfFile(string fileContent, string? key, dynamic? value)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(fileContent);

            if (key == null || value == null)
            {
                return JsonSerializer.Serialize(json);
            }

            JsonElement? result = FindElement(json, key, value);

            if (result != null)
            {
                return JsonSerializer.Serialize(result.Value);
            }

            return "Não existem dados com essas especificações";
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
