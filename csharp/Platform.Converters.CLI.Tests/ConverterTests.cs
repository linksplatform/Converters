using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Platform.Converters.Plugins.Examples;
using Xunit;

namespace Platform.Converters.CLI.Tests
{
    /// <summary>
    /// <para>Tests for converter implementations.</para>
    /// <para>Тесты для реализаций конвертеров.</para>
    /// </summary>
    public class ConverterTests
    {
        /// <summary>
        /// <para>Tests JSON to XML conversion.</para>
        /// <para>Тестирует конверсию JSON в XML.</para>
        /// </summary>
        [Fact]
        public async Task JsonToXmlConverter_ShouldConvertJsonToXml()
        {
            // Arrange
            var converter = new JsonToXmlConverter();
            var jsonContent = """{"name": "test", "value": 42, "active": true}""";
            var sourceStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
            var targetStream = new MemoryStream();

            // Act
            await converter.ConvertAsync(sourceStream, targetStream, ".json", ".xml");

            // Assert
            targetStream.Position = 0;
            var result = Encoding.UTF8.GetString(targetStream.ToArray());
            
            Assert.Contains("<name>test</name>", result);
            Assert.Contains("<value>42</value>", result);
            Assert.Contains("<active>true</active>", result);
        }

        /// <summary>
        /// <para>Tests XML to JSON conversion.</para>
        /// <para>Тестирует конверсию XML в JSON.</para>
        /// </summary>
        [Fact]
        public async Task XmlToJsonConverter_ShouldConvertXmlToJson()
        {
            // Arrange
            var converter = new XmlToJsonConverter();
            var xmlContent = """
                <?xml version="1.0"?>
                <root>
                    <name>test</name>
                    <value>42</value>
                    <active>true</active>
                </root>
                """;
            var sourceStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));
            var targetStream = new MemoryStream();

            // Act
            await converter.ConvertAsync(sourceStream, targetStream, ".xml", ".json");

            // Assert
            targetStream.Position = 0;
            var result = Encoding.UTF8.GetString(targetStream.ToArray());
            
            Assert.Contains("\"name\": \"test\"", result);
            Assert.Contains("\"value\": 42", result);
            Assert.Contains("\"active\": true", result);
        }

        /// <summary>
        /// <para>Tests text case conversion to uppercase.</para>
        /// <para>Тестирует конверсию регистра текста в верхний.</para>
        /// </summary>
        [Fact]
        public async Task TextToUppercaseConverter_ShouldConvertToUppercase()
        {
            // Arrange
            var converter = new TextToUppercaseConverter();
            var textContent = "Hello World!\nThis is a test.";
            var sourceStream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));
            var targetStream = new MemoryStream();

            // Act
            await converter.ConvertAsync(sourceStream, targetStream, ".txt", ".txt");

            // Assert
            targetStream.Position = 0;
            var result = Encoding.UTF8.GetString(targetStream.ToArray());
            
            Assert.Contains("HELLO WORLD!", result);
            Assert.Contains("THIS IS A TEST.", result);
        }

        /// <summary>
        /// <para>Tests text case conversion to lowercase.</para>
        /// <para>Тестирует конверсию регистра текста в нижний.</para>
        /// </summary>
        [Fact]
        public async Task TextToLowercaseConverter_ShouldConvertToLowercase()
        {
            // Arrange
            var converter = new TextToLowercaseConverter();
            var textContent = "HELLO WORLD!\nTHIS IS A TEST.";
            var sourceStream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));
            var targetStream = new MemoryStream();

            // Act
            await converter.ConvertAsync(sourceStream, targetStream, ".txt", ".txt");

            // Assert
            targetStream.Position = 0;
            var result = Encoding.UTF8.GetString(targetStream.ToArray());
            
            Assert.Contains("hello world!", result);
            Assert.Contains("this is a test.", result);
        }

        /// <summary>
        /// <para>Tests converter capability checking.</para>
        /// <para>Тестирует проверку возможностей конвертера.</para>
        /// </summary>
        [Fact]
        public void Converters_CanConvert_ShouldReturnCorrectCapabilities()
        {
            // Arrange
            var jsonToXml = new JsonToXmlConverter();
            var xmlToJson = new XmlToJsonConverter();
            var textToUpper = new TextToUppercaseConverter();

            // Act & Assert
            Assert.True(jsonToXml.CanConvert(".json", ".xml"));
            Assert.False(jsonToXml.CanConvert(".xml", ".json"));
            Assert.False(jsonToXml.CanConvert(".txt", ".xml"));

            Assert.True(xmlToJson.CanConvert(".xml", ".json"));
            Assert.False(xmlToJson.CanConvert(".json", ".xml"));
            Assert.False(xmlToJson.CanConvert(".txt", ".json"));

            Assert.True(textToUpper.CanConvert(".txt", ".txt"));
            Assert.True(textToUpper.CanConvert(".text", ".txt"));
            Assert.False(textToUpper.CanConvert(".json", ".txt"));
        }

        /// <summary>
        /// <para>Tests that converters throw exceptions for unsupported conversions.</para>
        /// <para>Тестирует что конвертеры выбрасывают исключения для неподдерживаемых конверсий.</para>
        /// </summary>
        [Fact]
        public async Task Converters_UnsupportedConversion_ShouldThrowException()
        {
            // Arrange
            var converter = new JsonToXmlConverter();
            var sourceStream = new MemoryStream();
            var targetStream = new MemoryStream();

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(
                () => converter.ConvertAsync(sourceStream, targetStream, ".xml", ".json"));
        }
    }
}