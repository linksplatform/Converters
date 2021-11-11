using System;
using System.Runtime.CompilerServices;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>
    /// Represents the unchecked sign extending converter.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="ConverterBase{TSource, TTarget}"/>
    public abstract class UncheckedSignExtendingConverter<TSource, TTarget> : ConverterBase<TSource, TTarget>
    {
        /// <summary>
        /// <para>
        /// Gets the default value.
        /// </para>
        /// <para></para>
        /// </summary>
        public static UncheckedSignExtendingConverter<TSource, TTarget> Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = CompileUncheckedConverter();
[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UncheckedSignExtendingConverter<TSource, TTarget> CompileUncheckedConverter()
        {
            var type = CreateTypeInheritedFrom<UncheckedSignExtendingConverter<TSource, TTarget>>();
            EmitConvertMethod(type, il => il.UncheckedConvert<TSource, TTarget>(extendSign: true));
            return (UncheckedSignExtendingConverter<TSource, TTarget>)Activator.CreateInstance(type.CreateTypeInfo());
        }
    }
}
