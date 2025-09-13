using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Converters.CLI
{
    /// <summary>
    /// <para>Main program class for the CLI tool.</para>
    /// <para>Основной класс программы для CLI инструмента.</para>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// <para>Entry point for the CLI application.</para>
        /// <para>Точка входа для CLI приложения.</para>
        /// </summary>
        /// <param name="args"><para>Command line arguments.</para><para>Аргументы командной строки.</para></param>
        /// <returns><para>Exit code.</para><para>Код завершения.</para></returns>
        public static async Task<int> Main(string[] args)
        {
            var pluginLoader = new PluginLoader();
            
            // Load plugins from default directory or environment variable
            var pluginDirectory = Environment.GetEnvironmentVariable("CONVERTERS_PLUGIN_DIR") 
                                 ?? Path.Combine(AppContext.BaseDirectory, "plugins");
            
            if (Directory.Exists(pluginDirectory))
            {
                pluginLoader.LoadPlugins(pluginDirectory);
            }

            if (args.Length == 0)
            {
                ShowHelp();
                return 0;
            }

            var command = args[0].ToLowerInvariant();

            try
            {
                return command switch
                {
                    "convert" => await HandleConvertCommand(args, pluginLoader),
                    "list" => HandleListCommand(pluginLoader),
                    "load" => HandleLoadCommand(args, pluginLoader),
                    "help" or "--help" or "-h" => ShowHelp(),
                    _ => ShowUnknownCommandError(command)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        private static async Task<int> HandleConvertCommand(string[] args, PluginLoader pluginLoader)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: convert <source> <target> [--converter <name>]");
                return 1;
            }

            var source = args[1];
            var target = args[2];
            string? converterName = null;

            // Parse optional converter argument
            for (int i = 3; i < args.Length - 1; i++)
            {
                if (args[i] == "--converter" || args[i] == "-c")
                {
                    converterName = args[i + 1];
                    break;
                }
            }

            if (!File.Exists(source))
            {
                Console.WriteLine($"Source file '{source}' not found.");
                return 1;
            }

            var sourceExtension = Path.GetExtension(source).ToLowerInvariant();
            var targetExtension = Path.GetExtension(target).ToLowerInvariant();

            var availableConverters = pluginLoader.FindConverters(sourceExtension, targetExtension);

            if (availableConverters.Count == 0)
            {
                Console.WriteLine($"No converter found for {sourceExtension} -> {targetExtension}");
                Console.WriteLine("Use 'list' command to see available converters.");
                return 1;
            }

            var selectedConverter = availableConverters.First();

            if (!string.IsNullOrEmpty(converterName))
            {
                var namedConverter = availableConverters.FirstOrDefault(c => 
                    c.Name.Equals(converterName, StringComparison.OrdinalIgnoreCase));
                
                if (namedConverter == null)
                {
                    Console.WriteLine($"Converter '{converterName}' not found or not compatible.");
                    Console.WriteLine("Available converters for this conversion:");
                    foreach (var conv in availableConverters)
                    {
                        Console.WriteLine($"  - {conv.Name}: {conv.Description}");
                    }
                    return 1;
                }

                selectedConverter = namedConverter;
            }

            Console.WriteLine($"Converting '{source}' to '{target}' using '{selectedConverter.Name}'...");

            try
            {
                // Ensure target directory exists
                var targetDir = Path.GetDirectoryName(target);
                if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                await selectedConverter.ConvertAsync(source, target);
                Console.WriteLine("Conversion completed successfully.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Conversion failed: {ex.Message}");
                return 1;
            }
        }

        private static int HandleListCommand(PluginLoader pluginLoader)
        {
            pluginLoader.ListConverters();
            return 0;
        }

        private static int HandleLoadCommand(string[] args, PluginLoader pluginLoader)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: load <plugin-path>");
                return 1;
            }

            var pluginFile = args[1];

            if (!File.Exists(pluginFile))
            {
                Console.WriteLine($"Plugin file '{pluginFile}' not found.");
                return 1;
            }

            pluginLoader.LoadPlugin(pluginFile);
            Console.WriteLine($"Plugin loaded successfully from '{Path.GetFileName(pluginFile)}'");
            return 0;
        }

        private static int ShowHelp()
        {
            Console.WriteLine("Platform Converters CLI - File conversion tool with plugin support");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  convert <source> <target> [--converter <name>]  Convert a file");
            Console.WriteLine("  list                                           List available converters");
            Console.WriteLine("  load <plugin-path>                            Load a plugin DLL");
            Console.WriteLine("  help                                          Show this help");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  convert input.json output.xml");
            Console.WriteLine("  convert input.json output.xml --converter \"JSON to XML Converter\"");
            Console.WriteLine("  list");
            Console.WriteLine("  load MyConverter.dll");
            Console.WriteLine();
            Console.WriteLine("Environment Variables:");
            Console.WriteLine("  CONVERTERS_PLUGIN_DIR   Directory to load plugins from (default: ./plugins)");
            
            return 0;
        }

        private static int ShowUnknownCommandError(string command)
        {
            Console.WriteLine($"Unknown command: {command}");
            Console.WriteLine("Use 'help' to see available commands.");
            return 1;
        }
    }
}