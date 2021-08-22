namespace Platform.Converters
{
    /// <summary>
    /// <para>Defines a value converter from the <typeparamref name="TSource"/> type to the <typeparamref name="TTarget"/> type.</para>
    /// <para>Определяет конвертер значений из типа <typeparamref name="TSource"/> в тип <typeparamref name="TTarget"/>.</para>
    /// </summary>
    /// <typeparam name="TSource"><para>Source type of conversion.</para><para>Исходный тип конверсии.</para></typeparam>
    /// <typeparam name="TTarget"><para>Target type of conversion.</para><para>Целевой тип конверсии.</para></typeparam>
    public interface IConverter<in TSource, out TTarget>
    {
        /// <summary>
        /// <para>Converts the value of the <typeparamref name="TSource"/> type to the value of the <typeparamref name="TTarget"/> type.</para>
        /// <para>Конвертирует значение типа <typeparamref name="TSource"/> в значение типа <typeparamref name="TTarget"/>.</para>
        /// </summary>
        /// <param name="source"><para>The <typeparamref name=="TSource"/> type value.</para><para>Значение типа <typeparamref name="TSource"/>.</para></param>
        /// <returns><para>The converted value of the <typeparamref name="TTarget"/> type.</para><para>Значение конвертированное в тип <typeparamref name="TTarget"/>.</para></returns>
        TTarget Convert(TSource source);
    }
}
