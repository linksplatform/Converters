using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    public abstract class ConverterBase<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TTarget Convert(TSource source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void ConvertAndUnbox(ILGenerator il)
        {
            var typeContainer = typeof(NumericType<TTarget>).GetField(nameof(NumericType<TTarget>.Type), BindingFlags.Static | BindingFlags.Public);
            il.Emit(OpCodes.Ldsfld, typeContainer);
            il.Call(typeof(Convert).GetMethod(nameof(System.Convert.ChangeType), Types<object, Type>.Array));
            il.UnboxValue(typeof(TTarget));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static string GetNewName() => Guid.NewGuid().ToString("N");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static TypeBuilder CreateTypeInheritedFrom<TBaseClass>()
        {
            var assemblyName = new AssemblyName(GetNewName());
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(GetNewName());
            var type = module.DefineType(GetNewName(), TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed, typeof(TBaseClass));
            return type;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void EmitConvertMethod(TypeBuilder typeBuilder, Action<ILGenerator> emitConversion)
        {
            typeBuilder.EmitFinalVirtualMethod<Converter<TSource, TTarget>>(nameof(IConverter<TSource, TTarget>.Convert), il =>
            {
                il.LoadArgument(1);
                if (typeof(TSource) == typeof(object) && typeof(TTarget) != typeof(object))
                {
                    ConvertAndUnbox(il);
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
    }
}
