using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Platform.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    public abstract class CheckedConverter<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        public static CheckedConverter<TSource, TTarget> Default { get; }

        static CheckedConverter()
        {
            AssemblyName assemblyName = new AssemblyName(GetNewName());
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(GetNewName());
            var type = module.DefineType(GetNewName(), TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed, typeof(CheckedConverter<TSource, TTarget>));
            EmitMethod<Converter<TSource, TTarget>>(type, "Convert", (il) =>
            {
                il.LoadArgument(1);
                if (typeof(TSource) != typeof(TTarget))
                {
                    CheckedConvert(il);
                }
                il.Return();
            });
            var typeInfo = type.CreateTypeInfo();
            Default = (CheckedConverter<TSource, TTarget>)Activator.CreateInstance(typeInfo);
        }

        private static void CheckedConvert(ILGenerator generator)
        {
            var type = typeof(TTarget);
            if (type == typeof(short))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I2);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I2_Un);
                }
            }
            else if (type == typeof(ushort))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_U2);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_U2_Un);
                }
            }
            else if (type == typeof(sbyte))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I1);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I1_Un);
                }
            }
            else if (type == typeof(byte))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_U1);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_U1_Un);
                }
            }
            else if (type == typeof(int))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I4);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I4_Un);
                }
            }
            else if (type == typeof(uint))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_U4);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_U4_Un);
                }
            }
            else if (type == typeof(long))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I8);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I8_Un);
                }
            }
            else if (type == typeof(ulong))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_U8);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_U8_Un);
                }
            }
            else if (type == typeof(float))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_R4);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_R_Un);
                }
            }
            else if (type == typeof(double))
            {
                generator.Emit(OpCodes.Conv_R8);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static void EmitMethod<TDelegate>(TypeBuilder type, string methodName, Action<ILGenerator> emitCode)
        {
            var delegateType = typeof(TDelegate);
            var invoke = delegateType.GetMethod("Invoke");
            var returnType = invoke.ReturnType;
            var parameterTypes = invoke.GetParameters().Select(s => s.ParameterType).ToArray();
            MethodBuilder method = type.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig, returnType, parameterTypes);
            method.SetImplementationFlags(MethodImplAttributes.IL | MethodImplAttributes.Managed | MethodImplAttributes.AggressiveInlining);
            var generator = method.GetILGenerator();
            emitCode(generator);
        }

        private static string GetNewName() => Guid.NewGuid().ToString("N");

        public abstract TTarget Convert(TSource source);
    }
}
