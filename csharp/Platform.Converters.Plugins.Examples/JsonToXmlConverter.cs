using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Platform.Converters.Plugins.Examples
{
    /// <summary>
    /// <para>Converts JSON files to XML format.</para>
    /// <para>Конвертирует JSON файлы в XML формат.</para>
    /// </summary>
    public class JsonToXmlConverter : IFileConverter
    {
        /// <inheritdoc />
        public string Name => "JSON to XML Converter";

        /// <inheritdoc />
        public string Description => "Converts JSON files to XML format";

        /// <inheritdoc />
        public string[] SupportedSourceExtensions => new[] { ".json" };

        /// <inheritdoc />
        public string[] SupportedTargetExtensions => new[] { ".xml" };

        /// <inheritdoc />
        public bool CanConvert(string sourceExtension, string targetExtension)
        {
            return sourceExtension.Equals(".json", StringComparison.OrdinalIgnoreCase) &&
                   targetExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public async Task ConvertAsync(string sourcePath, string targetPath)
        {
            using var sourceStream = File.OpenRead(sourcePath);
            using var targetStream = File.Create(targetPath);
            
            await ConvertAsync(sourceStream, targetStream, ".json", ".xml");
        }

        /// <inheritdoc />
        public async Task ConvertAsync(Stream sourceStream, Stream targetStream, string sourceExtension, string targetExtension)
        {
            if (!CanConvert(sourceExtension, targetExtension))
            {
                throw new NotSupportedException($"Conversion from {sourceExtension} to {targetExtension} is not supported.");
            }

            var jsonDocument = await JsonDocument.ParseAsync(sourceStream);
            var xmlDocument = ConvertJsonToXml(jsonDocument.RootElement, "root");
            
            await using var writer = XmlWriter.Create(targetStream, new XmlWriterSettings 
            { 
                Async = true, 
                Indent = true,
                IndentChars = "  ",
                CloseOutput = false
            });
            
            await xmlDocument.WriteToAsync(writer, default);
        }

        private static XDocument ConvertJsonToXml(JsonElement element, string elementName)
        {
            var xmlDoc = new XDocument();
            xmlDoc.Add(CreateXElement(element, elementName));
            return xmlDoc;
        }

        private static XElement CreateXElement(JsonElement element, string name)
        {
            var xElement = new XElement(SanitizeXmlName(name));

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        xElement.Add(CreateXElement(property.Value, property.Name));
                    }
                    break;

                case JsonValueKind.Array:
                    var index = 0;
                    foreach (var item in element.EnumerateArray())
                    {
                        xElement.Add(CreateXElement(item, $"item_{index++}"));
                    }
                    break;

                case JsonValueKind.String:
                    xElement.Value = element.GetString() ?? "";
                    break;

                case JsonValueKind.Number:
                    xElement.Value = element.ToString();
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    xElement.Value = element.GetBoolean().ToString().ToLowerInvariant();
                    break;

                case JsonValueKind.Null:
                    xElement.SetAttributeValue("null", "true");
                    break;

                default:
                    xElement.Value = element.ToString();
                    break;
            }

            return xElement;
        }

        private static string SanitizeXmlName(string name)
        {
            // Replace invalid XML name characters with underscores
            var sanitized = System.Text.RegularExpressions.Regex.Replace(name, @"[^\w\-_.]", "_");
            
            // Ensure name starts with letter or underscore
            if (!char.IsLetter(sanitized[0]) && sanitized[0] != '_')
            {
                sanitized = "_" + sanitized;
            }

            return sanitized;
        }
    }
}