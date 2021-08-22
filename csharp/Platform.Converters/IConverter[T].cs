namespace Platform.Converters
{
    /// <summary>
    /// <para>Defines a converter between two values of the same <typeparamref name="T"/> type.</para>
    /// <para>Определяет конвертер между двумя значениями одного типа <typeparamref name="T"/>.</para>
    /// </summary>
    /// <typeparam name="T"><para>The type of value to convert.</para><para>Тип преобразуемого значения.</para></typeparam>
    public interface IConverter<T> : IConverter<T, T>
    {
    }
}
