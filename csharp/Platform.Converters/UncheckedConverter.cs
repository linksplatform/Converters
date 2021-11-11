using System;
using System.Runtime.CompilerServices;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>
    /// Represents the unchecked converter.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="ConverterBase{TSource, TTarget}"/>
    public abstract class UncheckedConverter<TSource, TTarget> : ConverterBase<TSource, TTarget>
    {
        /// <summary>
        /// <para>
        /// Gets the default value.
        /// </para>
        /// <para></para>
        /// </summary>
        public static UncheckedConverter<TSource, TTarget> Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = CompileUncheckedConverter();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UncheckedConverter<TSource, TTarget> CompileUncheckedConverter()
        {
            var type = CreateTypeInheritedFrom<UncheckedConverter<TSource, TTarget>>();
            EmitConvertMethod(type, il => il.UncheckedConvert<TSource, TTarget>());
            return (UncheckedConverter<TSource, TTarget>)Activator.CreateInstance(type.CreateTypeInfo());
        }
    }
}
