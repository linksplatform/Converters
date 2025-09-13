using System.IO;
using System.Threading.Tasks;

namespace Platform.Converters
{
    /// <summary>
    /// <para>Defines a file converter interface for converting files from one format to another.</para>
    /// <para>Определяет интерфейс конвертера файлов для преобразования файлов из одного формата в другой.</para>
    /// </summary>
    public interface IFileConverter
    {
        /// <summary>
        /// <para>Gets the name of the converter.</para>
        /// <para>Получает имя конвертера.</para>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// <para>Gets the description of the converter.</para>
        /// <para>Получает описание конвертера.</para>
        /// </summary>
        string Description { get; }

        /// <summary>
        /// <para>Gets the source file extensions supported by this converter.</para>
        /// <para>Получает расширения исходных файлов, поддерживаемые этим конвертером.</para>
        /// </summary>
        string[] SupportedSourceExtensions { get; }

        /// <summary>
        /// <para>Gets the target file extensions this converter can produce.</para>
        /// <para>Получает расширения целевых файлов, которые может создать этот конвертер.</para>
        /// </summary>
        string[] SupportedTargetExtensions { get; }

        /// <summary>
        /// <para>Determines whether this converter can convert from the source extension to the target extension.</para>
        /// <para>Определяет, может ли этот конвертер выполнить преобразование из исходного расширения в целевое расширение.</para>
        /// </summary>
        /// <param name="sourceExtension"><para>The source file extension.</para><para>Расширение исходного файла.</para></param>
        /// <param name="targetExtension"><para>The target file extension.</para><para>Расширение целевого файла.</para></param>
        /// <returns><para>True if the conversion is supported, false otherwise.</para><para>True, если преобразование поддерживается, иначе false.</para></returns>
        bool CanConvert(string sourceExtension, string targetExtension);

        /// <summary>
        /// <para>Converts a file from the source path to the target path.</para>
        /// <para>Конвертирует файл из исходного пути в целевой путь.</para>
        /// </summary>
        /// <param name="sourcePath"><para>The source file path.</para><para>Путь к исходному файлу.</para></param>
        /// <param name="targetPath"><para>The target file path.</para><para>Путь к целевому файлу.</para></param>
        /// <returns><para>A task representing the asynchronous conversion operation.</para><para>Задача, представляющая асинхронную операцию преобразования.</para></returns>
        Task ConvertAsync(string sourcePath, string targetPath);

        /// <summary>
        /// <para>Converts a file stream to another stream.</para>
        /// <para>Конвертирует поток файла в другой поток.</para>
        /// </summary>
        /// <param name="sourceStream"><para>The source stream.</para><para>Исходный поток.</para></param>
        /// <param name="targetStream"><para>The target stream.</para><para>Целевой поток.</para></param>
        /// <param name="sourceExtension"><para>The source file extension.</para><para>Расширение исходного файла.</para></param>
        /// <param name="targetExtension"><para>The target file extension.</para><para>Расширение целевого файла.</para></param>
        /// <returns><para>A task representing the asynchronous conversion operation.</para><para>Задача, представляющая асинхронную операцию преобразования.</para></returns>
        Task ConvertAsync(Stream sourceStream, Stream targetStream, string sourceExtension, string targetExtension);
    }
}