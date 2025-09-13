using System;
using System.IO;
using System.Linq;
using Platform.Converters.CLI;
using Platform.Converters.Plugins.Examples;
using Xunit;

namespace Platform.Converters.CLI.Tests
{
    /// <summary>
    /// <para>Tests for the PluginLoader class.</para>
    /// <para>Тесты для класса PluginLoader.</para>
    /// </summary>
    public class PluginLoaderTests
    {
        /// <summary>
        /// <para>Tests that converters can be found by file extensions.</para>
        /// <para>Тестирует что конвертеры могут быть найдены по расширениям файлов.</para>
        /// </summary>
        [Fact]
        public void FindConverters_ShouldReturnCompatibleConverters()
        {
            // Arrange
            var loader = new PluginLoader();
            var jsonToXmlConverter = new JsonToXmlConverter();
            var xmlToJsonConverter = new XmlToJsonConverter();
            var textConverter = new TextToUppercaseConverter();
            
            // Simulate loaded converters by using reflection to add them
            var loadedConvertersField = typeof(PluginLoader)
                .GetField("_loadedConverters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var loadedConverters = (System.Collections.Generic.List<IFileConverter>)loadedConvertersField!.GetValue(loader)!;
            
            loadedConverters.Add(jsonToXmlConverter);
            loadedConverters.Add(xmlToJsonConverter);
            loadedConverters.Add(textConverter);

            // Act
            var jsonToXmlConverters = loader.FindConverters(".json", ".xml");
            var xmlToJsonConverters = loader.FindConverters(".xml", ".json");
            var textConverters = loader.FindConverters(".txt", ".txt");
            var noConverters = loader.FindConverters(".pdf", ".doc");

            // Assert
            Assert.Single(jsonToXmlConverters);
            Assert.Contains(jsonToXmlConverter, jsonToXmlConverters);
            
            Assert.Single(xmlToJsonConverters);
            Assert.Contains(xmlToJsonConverter, xmlToJsonConverters);
            
            Assert.Single(textConverters);
            Assert.Contains(textConverter, textConverters);
            
            Assert.Empty(noConverters);
        }

        /// <summary>
        /// <para>Tests that plugin loading handles non-existent directories gracefully.</para>
        /// <para>Тестирует что загрузка плагинов корректно обрабатывает несуществующие директории.</para>
        /// </summary>
        [Fact]
        public void LoadPlugins_WithNonExistentDirectory_ShouldNotThrow()
        {
            // Arrange
            var loader = new PluginLoader();
            var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            // Act & Assert
            var exception = Record.Exception(() => loader.LoadPlugins(nonExistentPath));
            Assert.Null(exception);
        }

        /// <summary>
        /// <para>Tests that the loader starts with no converters loaded.</para>
        /// <para>Тестирует что загрузчик начинает без загруженных конвертеров.</para>
        /// </summary>
        [Fact]
        public void LoadedConverters_InitialState_ShouldBeEmpty()
        {
            // Arrange & Act
            var loader = new PluginLoader();

            // Assert
            Assert.Empty(loader.LoadedConverters);
        }
    }
}