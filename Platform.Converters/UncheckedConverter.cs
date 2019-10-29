using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    public abstract class UncheckedConverter<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        public static UncheckedConverter<TSource, TTarget> Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UncheckedConverter()
        {
            var assemblyName = new AssemblyName(GetNewName());
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(GetNewName());
            var type = module.DefineType(GetNewName(), TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed, typeof(UncheckedConverter<TSource, TTarget>));
            type.EmitVirtualMethod<Converter<TSource, TTarget>>("Convert", il =>
            {
                il.LoadArgument(1);
                if (typeof(TSource) != typeof(TTarget))
                {
                    il.UncheckedConvert<TSource, TTarget>();
                }
                il.Return();
            });
            var typeInfo = type.CreateTypeInfo();
            Default = (UncheckedConverter<TSource, TTarget>)Activator.CreateInstance(typeInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetNewName() => Guid.NewGuid().ToString("N");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TTarget Convert(TSource source);
    }
}
