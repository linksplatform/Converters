using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Platform.Converters.Plugins.Examples
{
    /// <summary>
    /// <para>Converts XML files to JSON format.</para>
    /// <para>Конвертирует XML файлы в JSON формат.</para>
    /// </summary>
    public class XmlToJsonConverter : IFileConverter
    {
        /// <inheritdoc />
        public string Name => "XML to JSON Converter";

        /// <inheritdoc />
        public string Description => "Converts XML files to JSON format";

        /// <inheritdoc />
        public string[] SupportedSourceExtensions => new[] { ".xml" };

        /// <inheritdoc />
        public string[] SupportedTargetExtensions => new[] { ".json" };

        /// <inheritdoc />
        public bool CanConvert(string sourceExtension, string targetExtension)
        {
            return sourceExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase) &&
                   targetExtension.Equals(".json", StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public async Task ConvertAsync(string sourcePath, string targetPath)
        {
            using var sourceStream = File.OpenRead(sourcePath);
            using var targetStream = File.Create(targetPath);
            
            await ConvertAsync(sourceStream, targetStream, ".xml", ".json");
        }

        /// <inheritdoc />
        public async Task ConvertAsync(Stream sourceStream, Stream targetStream, string sourceExtension, string targetExtension)
        {
            if (!CanConvert(sourceExtension, targetExtension))
            {
                throw new NotSupportedException($"Conversion from {sourceExtension} to {targetExtension} is not supported.");
            }

            var xmlDocument = await XDocument.LoadAsync(sourceStream, LoadOptions.None, default);
            var jsonObject = ConvertXmlToJson(xmlDocument.Root!);
            
            await using var writer = new Utf8JsonWriter(targetStream, new JsonWriterOptions 
            { 
                Indented = true 
            });
            
            JsonSerializer.Serialize(writer, jsonObject);
        }

        private static object ConvertXmlToJson(XElement element)
        {
            var result = new Dictionary<string, object>();

            // Add attributes as properties
            foreach (var attribute in element.Attributes())
            {
                result[$"@{attribute.Name}"] = attribute.Value;
            }

            // Group child elements by name
            var childGroups = new Dictionary<string, List<XElement>>();
            foreach (var child in element.Elements())
            {
                var name = child.Name.LocalName;
                if (!childGroups.ContainsKey(name))
                {
                    childGroups[name] = new List<XElement>();
                }
                childGroups[name].Add(child);
            }

            // Convert child elements
            foreach (var group in childGroups)
            {
                if (group.Value.Count == 1)
                {
                    // Single element
                    var child = group.Value[0];
                    if (child.HasElements || child.Attributes().Any())
                    {
                        result[group.Key] = ConvertXmlToJson(child);
                    }
                    else
                    {
                        result[group.Key] = ConvertValue(child.Value);
                    }
                }
                else
                {
                    // Multiple elements with same name - create array
                    var array = new List<object>();
                    foreach (var child in group.Value)
                    {
                        if (child.HasElements || child.Attributes().Any())
                        {
                            array.Add(ConvertXmlToJson(child));
                        }
                        else
                        {
                            array.Add(ConvertValue(child.Value));
                        }
                    }
                    result[group.Key] = array;
                }
            }

            // If element has no children or attributes, return the text value
            if (!element.HasElements && !element.Attributes().Any())
            {
                return ConvertValue(element.Value);
            }

            // If element has text content along with children/attributes
            if (!string.IsNullOrWhiteSpace(element.Value) && (element.HasElements || element.Attributes().Any()))
            {
                result["#text"] = ConvertValue(element.Value.Trim());
            }

            return result;
        }

        private static object ConvertValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Try to parse as number
            if (long.TryParse(value, out var longValue))
                return longValue;

            if (double.TryParse(value, out var doubleValue))
                return doubleValue;

            // Try to parse as boolean
            if (bool.TryParse(value, out var boolValue))
                return boolValue;

            // Return as string
            return value;
        }
    }
}