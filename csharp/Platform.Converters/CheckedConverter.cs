using System;
using System.Runtime.CompilerServices;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    /// <summary>
    /// <para>
    /// Represents the checked converter.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="ConverterBase{TSource, TTarget}"/>
    public abstract class CheckedConverter<TSource, TTarget> : ConverterBase<TSource, TTarget>
    {
        /// <summary>
        /// <para>
        /// Gets the default value.
        /// </para>
        /// <para></para>
        /// </summary>
        public static CheckedConverter<TSource, TTarget> Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = CompileCheckedConverter();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CheckedConverter<TSource, TTarget> CompileCheckedConverter()
        {
            var type = CreateTypeInheritedFrom<CheckedConverter<TSource, TTarget>>();
            EmitConvertMethod(type, il => il.CheckedConvert<TSource, TTarget>());
            return (CheckedConverter<TSource, TTarget>)Activator.CreateInstance(type.CreateTypeInfo());
        }
    }
}
