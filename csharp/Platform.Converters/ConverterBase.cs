using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{   
    /// <summary>
    /// <para>Represents a base implementation for IConverter interface with the basic logic necessary for value converter from the <see cref="TSource"/> type to the <see cref="TTarget"/> type.</para>
    /// <para>Представляет базовую реализацию для интерфейса IConverter с основной логикой необходимой для конвертера значений из типа <see cref="TSource"/> в тип <sea cref="TTarget"/>.</para>
    /// </summary>
    /// <typeparam name="TSource"><para>Source type of conversion.</para><para>Исходный тип конверсии.</para></typeparam>
    /// <typeparam name="TTarget"><para>Target type of conversion.</para><para>Целевой тип конверсии.</para></typeparam>
    public abstract class ConverterBase<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        /// <summary>
        /// <para>Converts the value of the <see cref="TSource"/> type to the value of the <see cref="TTarget"/> type.</para>
        /// <para>Конвертирует значение типа <see cref="TSource"/> в значение типа <see cref="TTarget"/>.</para>
        /// </summary>
        /// <param name="source"><para>The <see cref="TSource"/> type value.</para><para>Значение типа <see cref="TSource"/>.</para></param>
        /// <returns><para>The value is converted to the <see cref="TTarget"/> type.</para><para>Значение конвертированное в тип <see cref="TTarget"/>.</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TTarget Convert(TSource source);
        
        /// <summary>
        /// <para>Converts the value of the source type (TSource) to the value of the target type.</para>
        /// <para>Конвертирует значение исходного типа (TSource) в значение целевого типа.</para>
        /// </summary>
        /// <param name="source"><para>The source type value (TSource).</para><para>Значение исходного типа (TSource).</para></param>
        /// <returns><para>The value is converted to the target type (TTarget).</para><para>Значение ковертированное в целевой тип (TTarget).</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void ConvertFromObject(ILGenerator il)
        {
            var returnDefault = il.DefineLabel();
            il.Emit(OpCodes.Brfalse_S, returnDefault);
            il.LoadArgument(1);
            il.Emit(OpCodes.Castclass, typeof(IConvertible));
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Callvirt, GetMethodForConversionToTargetType());
            il.Return();
            il.MarkLabel(returnDefault);
            LoadDefault(il, typeof(TTarget));
        }

        /// <summary>
        /// <para>Converts the value of the source type (TSource) to the value of the target type.</para>
        /// <para>Конвертирует значение исходного типа (TSource) в значение целевого типа.</para>
        /// </summary>
        /// <param name="source"><para>The source type value (TSource).</para><para>Значение исходного типа (TSource).</para></param>
        /// <returns><para>The value is converted to the target type (TTarget).</para><para>Значение ковертированное в целевой тип (TTarget).</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static string GetNewName() => Guid.NewGuid().ToString("N");

        /// <summary>
        /// <para>Converts the value of the source type (TSource) to the value of the target type.</para>
        /// <para>Конвертирует значение исходного типа (TSource) в значение целевого типа.</para>
        /// </summary>
        /// <param name="source"><para>The source type value (TSource).</para><para>Значение исходного типа (TSource).</para></param>
        /// <returns><para>The value is converted to the target type (TTarget).</para><para>Значение ковертированное в целевой тип (TTarget).</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static TypeBuilder CreateTypeInheritedFrom<TBaseClass>()
        {
            var assemblyName = new AssemblyName(GetNewName());
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(GetNewName());
            var type = module.DefineType(GetNewName(), TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed, typeof(TBaseClass));
            return type;
        }

        /// <summary>
        /// <para>Converts the value of the source type (TSource) to the value of the target type.</para>
        /// <para>Конвертирует значение исходного типа (TSource) в значение целевого типа.</para>
        /// </summary>
        /// <param name="source"><para>The source type value (TSource).</para><para>Значение исходного типа (TSource).</para></param>
        /// <returns><para>The value is converted to the target type (TTarget).</para><para>Значение ковертированное в целевой тип (TTarget).</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void EmitConvertMethod(TypeBuilder typeBuilder, Action<ILGenerator> emitConversion)
        {
            typeBuilder.EmitFinalVirtualMethod<Converter<TSource, TTarget>>(nameof(IConverter<TSource, TTarget>.Convert), il =>
            {
                il.LoadArgument(1);
                if (typeof(TSource) == typeof(object) && typeof(TTarget) != typeof(object))
                {
                    ConvertFromObject(il);
                }
                else if (typeof(TSource) != typeof(object) && typeof(TTarget) == typeof(object))
                {
                    il.Box(typeof(TSource));
                }
                else
                {
                    emitConversion(il);
                }
                il.Return();
            });
        }

        /// <summary>
        /// <para>Converts the value of the source type (TSource) to the value of the target type.</para>
        /// <para>Конвертирует значение исходного типа (TSource) в значение целевого типа.</para>
        /// </summary>
        /// <param name="source"><para>The source type value (TSource).</para><para>Значение исходного типа (TSource).</para></param>
        /// <returns><para>The value is converted to the target type (TTarget).</para><para>Значение ковертированное в целевой тип (TTarget).</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static MethodInfo GetMethodForConversionToTargetType()
        {
            var targetType = typeof(TTarget);
            var convertibleType = typeof(IConvertible);
            var typeParameters = Types<IFormatProvider>.Array;
            if (targetType == typeof(bool))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToBoolean), typeParameters);
            }
            else if (targetType == typeof(byte))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToByte), typeParameters);
            }
            else if (targetType == typeof(char))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToChar), typeParameters);
            }
            else if (targetType == typeof(DateTime))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToDateTime), typeParameters);
            }
            else if (targetType == typeof(decimal))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToDecimal), typeParameters);
            }
            else if (targetType == typeof(double))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToDouble), typeParameters);
            }
            else if (targetType == typeof(short))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToInt16), typeParameters);
            }
            else if (targetType == typeof(int))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToInt32), typeParameters);
            }
            else if (targetType == typeof(long))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToInt64), typeParameters);
            }
            else if (targetType == typeof(sbyte))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToSByte), typeParameters);
            }
            else if (targetType == typeof(float))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToSingle), typeParameters);
            }
            else if (targetType == typeof(string))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToString), typeParameters);
            }
            else if (targetType == typeof(ushort))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToUInt16), typeParameters);
            }
            else if (targetType == typeof(uint))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToUInt32), typeParameters);
            }
            else if (targetType == typeof(ulong))
            {
                return convertibleType.GetMethod(nameof(IConvertible.ToUInt64), typeParameters);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// <para>Converts the value of the source type (TSource) to the value of the target type.</para>
        /// <para>Конвертирует значение исходного типа (TSource) в значение целевого типа.</para>
        /// </summary>
        /// <param name="source"><para>The source type value (TSource).</para><para>Значение исходного типа (TSource).</para></param>
        /// <returns><para>The value is converted to the target type (TTarget).</para><para>Значение ковертированное в целевой тип (TTarget).</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void LoadDefault(ILGenerator il, Type targetType)
        {
            if (targetType == typeof(string))
            {
                il.Emit(OpCodes.Ldsfld, targetType.GetField(nameof(string.Empty), BindingFlags.Static | BindingFlags.Public));
            }
            else if (targetType == typeof(DateTime))
            {
                il.Emit(OpCodes.Ldsfld, targetType.GetField(nameof(DateTime.MinValue), BindingFlags.Static | BindingFlags.Public));
            }
            else if (targetType == typeof(decimal))
            {
                il.Emit(OpCodes.Ldsfld, targetType.GetField(nameof(decimal.Zero), BindingFlags.Static | BindingFlags.Public));
            }
            else if (targetType == typeof(float))
            {
                il.LoadConstant(0.0F);
            }
            else if (targetType == typeof(double))
            {
                il.LoadConstant(0.0D);
            }
            else if (targetType == typeof(long) || targetType == typeof(ulong))
            {
                il.LoadConstant(0L);
            }
            else
            {
                il.LoadConstant(0);
            }
        }
    }
}
