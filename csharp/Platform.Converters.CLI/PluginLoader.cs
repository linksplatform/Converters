using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Platform.Converters.CLI
{
    /// <summary>
    /// <para>Loads file converter plugins from DLL files.</para>
    /// <para>Загружает плагины конвертеров файлов из DLL файлов.</para>
    /// </summary>
    public class PluginLoader
    {
        private readonly List<IFileConverter> _loadedConverters = new();

        /// <summary>
        /// <para>Gets all loaded file converters.</para>
        /// <para>Получает все загруженные конвертеры файлов.</para>
        /// </summary>
        public IReadOnlyList<IFileConverter> LoadedConverters => _loadedConverters.AsReadOnly();

        /// <summary>
        /// <para>Loads plugins from the specified directory.</para>
        /// <para>Загружает плагины из указанной директории.</para>
        /// </summary>
        /// <param name="pluginDirectory"><para>The directory containing plugin DLL files.</para><para>Директория, содержащая DLL файлы плагинов.</para></param>
        public void LoadPlugins(string pluginDirectory)
        {
            if (!Directory.Exists(pluginDirectory))
            {
                Console.WriteLine($"Plugin directory '{pluginDirectory}' does not exist.");
                return;
            }

            var dllFiles = Directory.GetFiles(pluginDirectory, "*.dll");
            
            foreach (var dllFile in dllFiles)
            {
                try
                {
                    LoadPlugin(dllFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load plugin from '{dllFile}': {ex.Message}");
                }
            }

            Console.WriteLine($"Loaded {_loadedConverters.Count} converter(s) from {dllFiles.Length} plugin file(s).");
        }

        /// <summary>
        /// <para>Loads a single plugin from the specified DLL file.</para>
        /// <para>Загружает один плагин из указанного DLL файла.</para>
        /// </summary>
        /// <param name="pluginPath"><para>The path to the plugin DLL file.</para><para>Путь к DLL файлу плагина.</para></param>
        public void LoadPlugin(string pluginPath)
        {
            if (!File.Exists(pluginPath))
            {
                throw new FileNotFoundException($"Plugin file '{pluginPath}' not found.");
            }

            var loadContext = new AssemblyLoadContext(Path.GetFileNameWithoutExtension(pluginPath), true);
            var assembly = loadContext.LoadFromAssemblyPath(pluginPath);
            
            var converterTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IFileConverter).IsAssignableFrom(t))
                .ToList();

            foreach (var converterType in converterTypes)
            {
                try
                {
                    if (Activator.CreateInstance(converterType) is IFileConverter converter)
                    {
                        _loadedConverters.Add(converter);
                        Console.WriteLine($"Loaded converter: {converter.Name} - {converter.Description}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to instantiate converter '{converterType.Name}': {ex.Message}");
                }
            }
        }

        /// <summary>
        /// <para>Finds converters that can handle the specified file extension conversion.</para>
        /// <para>Находит конвертеры, которые могут выполнить преобразование указанных расширений файлов.</para>
        /// </summary>
        /// <param name="sourceExtension"><para>The source file extension.</para><para>Расширение исходного файла.</para></param>
        /// <param name="targetExtension"><para>The target file extension.</para><para>Расширение целевого файла.</para></param>
        /// <returns><para>A list of compatible converters.</para><para>Список совместимых конвертеров.</para></returns>
        public List<IFileConverter> FindConverters(string sourceExtension, string targetExtension)
        {
            return _loadedConverters
                .Where(c => c.CanConvert(sourceExtension, targetExtension))
                .ToList();
        }

        /// <summary>
        /// <para>Lists all available converters with their supported formats.</para>
        /// <para>Перечисляет все доступные конвертеры с их поддерживаемыми форматами.</para>
        /// </summary>
        public void ListConverters()
        {
            if (_loadedConverters.Count == 0)
            {
                Console.WriteLine("No converters loaded.");
                return;
            }

            Console.WriteLine("Available converters:");
            Console.WriteLine();

            foreach (var converter in _loadedConverters)
            {
                Console.WriteLine($"Name: {converter.Name}");
                Console.WriteLine($"Description: {converter.Description}");
                Console.WriteLine($"Source formats: {string.Join(", ", converter.SupportedSourceExtensions)}");
                Console.WriteLine($"Target formats: {string.Join(", ", converter.SupportedTargetExtensions)}");
                Console.WriteLine();
            }
        }
    }
}