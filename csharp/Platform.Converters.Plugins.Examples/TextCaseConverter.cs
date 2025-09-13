using System;
using System.IO;
using System.Threading.Tasks;

namespace Platform.Converters.Plugins.Examples
{
    /// <summary>
    /// <para>Converts text files between different case formats (uppercase, lowercase).</para>
    /// <para>Конвертирует текстовые файлы между различными форматами регистра (верхний, нижний).</para>
    /// </summary>
    public class TextCaseConverter : IFileConverter
    {
        private readonly bool _toUpperCase;

        /// <summary>
        /// <para>Initializes a new instance of the TextCaseConverter class.</para>
        /// <para>Инициализирует новый экземпляр класса TextCaseConverter.</para>
        /// </summary>
        /// <param name="toUpperCase"><para>If true, converts to uppercase; otherwise, converts to lowercase.</para><para>Если true, конвертирует в верхний регистр; иначе в нижний регистр.</para></param>
        public TextCaseConverter(bool toUpperCase = false)
        {
            _toUpperCase = toUpperCase;
        }

        /// <inheritdoc />
        public string Name => _toUpperCase ? "Text to Uppercase Converter" : "Text to Lowercase Converter";

        /// <inheritdoc />
        public string Description => _toUpperCase 
            ? "Converts text files to uppercase" 
            : "Converts text files to lowercase";

        /// <inheritdoc />
        public string[] SupportedSourceExtensions => new[] { ".txt", ".text", ".log" };

        /// <inheritdoc />
        public string[] SupportedTargetExtensions => new[] { ".txt", ".text", ".log" };

        /// <inheritdoc />
        public bool CanConvert(string sourceExtension, string targetExtension)
        {
            var supportedExtensions = new[] { ".txt", ".text", ".log" };
            
            return Array.Exists(supportedExtensions, ext => 
                ext.Equals(sourceExtension, StringComparison.OrdinalIgnoreCase)) &&
                   Array.Exists(supportedExtensions, ext => 
                ext.Equals(targetExtension, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        public async Task ConvertAsync(string sourcePath, string targetPath)
        {
            using var sourceStream = File.OpenRead(sourcePath);
            using var targetStream = File.Create(targetPath);
            
            await ConvertAsync(sourceStream, targetStream, 
                Path.GetExtension(sourcePath), 
                Path.GetExtension(targetPath));
        }

        /// <inheritdoc />
        public async Task ConvertAsync(Stream sourceStream, Stream targetStream, string sourceExtension, string targetExtension)
        {
            if (!CanConvert(sourceExtension, targetExtension))
            {
                throw new NotSupportedException($"Conversion from {sourceExtension} to {targetExtension} is not supported.");
            }

            using var reader = new StreamReader(sourceStream, leaveOpen: true);
            await using var writer = new StreamWriter(targetStream, leaveOpen: true);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var convertedLine = _toUpperCase ? line.ToUpperInvariant() : line.ToLowerInvariant();
                await writer.WriteLineAsync(convertedLine);
            }
            
            await writer.FlushAsync();
        }
    }

    /// <summary>
    /// <para>Converts text files to uppercase.</para>
    /// <para>Конвертирует текстовые файлы в верхний регистр.</para>
    /// </summary>
    public class TextToUppercaseConverter : TextCaseConverter
    {
        /// <summary>
        /// <para>Initializes a new instance of the TextToUppercaseConverter class.</para>
        /// <para>Инициализирует новый экземпляр класса TextToUppercaseConverter.</para>
        /// </summary>
        public TextToUppercaseConverter() : base(true) { }
    }

    /// <summary>
    /// <para>Converts text files to lowercase.</para>
    /// <para>Конвертирует текстовые файлы в нижний регистр.</para>
    /// </summary>
    public class TextToLowercaseConverter : TextCaseConverter
    {
        /// <summary>
        /// <para>Initializes a new instance of the TextToLowercaseConverter class.</para>
        /// <para>Инициализирует новый экземпляр класса TextToLowercaseConverter.</para>
        /// </summary>
        public TextToLowercaseConverter() : base(false) { }
    }
}