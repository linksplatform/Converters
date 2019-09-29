using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    public static class Converter<TSource, TTarget>
    {
        public static IConverter<TSource, TTarget> Default { get; set; }

        static Converter()
        {
            AssemblyName assemblyName = new AssemblyName(GetNewName());
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(GetNewName());
            var type = module.DefineType(GetNewName(), TypeAttributes.Public | TypeAttributes.Class, null, Types<IConverter<TSource, TTarget>>.Array);

            EmitMethod<System.Converter<TSource, TTarget>>(type, "Convert", (il) =>
            {
                if (typeof(TSource) == typeof(TTarget))
                {
                    il.Return();
                }
                else
                {
                    throw new NotSupportedException();
                }
            });

            var typeInfo = type.CreateTypeInfo();

            Default = (IConverter<TSource, TTarget>)Activator.CreateInstance(typeInfo);
        }

        private static void EmitMethod<TDelegate>(TypeBuilder type, string methodName, Action<ILGenerator> emitCode)
        {
            var delegateType = typeof(TDelegate);
            var invoke = delegateType.GetMethod("Invoke");
            var returnType = invoke.ReturnType;
            var parameterTypes = invoke.GetParameters().Select(s => s.ParameterType).ToArray();
            MethodBuilder method = type.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Static, returnType, parameterTypes);
            method.SetImplementationFlags(MethodImplAttributes.IL | MethodImplAttributes.Managed | MethodImplAttributes.AggressiveInlining);
            var generator = method.GetILGenerator();
            emitCode(generator);
        }

        private static string GetNewName() => Guid.NewGuid().ToString("N");
    }
}
