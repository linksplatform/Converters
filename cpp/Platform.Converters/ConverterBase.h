namespace Platform::Converters
{
    template <typename ...> class ConverterBase;
    template <typename TSource, typename TTarget> class ConverterBase<TSource, TTarget> : public IConverter<TSource, TTarget>
    {
        public: virtual TTarget Convert(TSource source) = 0;
        
        protected: static void ConvertFromObject(ILGenerator &il)
        {
            auto returnDefault = il.DefineLabel();
            il.Emit(OpCodes.Brfalse_S, returnDefault);
            il.LoadArgument(1);
            il.Emit(OpCodes.Castclass, typeof(IConvertible));
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Callvirt, GetMethodForConversionToTargetType());
            il.Return();
            il.MarkLabel(returnDefault);
            LoadDefault(il, typeof(TTarget));
        }

        protected: static std::string GetNewName() { return Guid.NewGuid().ToString("N"); }

        protected: static TypeBuilder CreateTypeInheritedFrom<TBaseClass>()
        {
            auto assemblyName = AssemblyName(GetNewName());
            auto assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            auto module = assembly.DefineDynamicModule(GetNewName());
            auto type = module.DefineType(GetNewName(), TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed, typeof(TBaseClass));
            return type;
        }

        protected: static void EmitConvertMethod(TypeBuilder typeBuilder, std::function<void(ILGenerator)> emitConversion)
        {
            typeBuilder.EmitFinalVirtualMethod<Converter<TSource, TTarget>>("Convert", il =>
            {
                il.LoadArgument(1);
                if (typeof(TSource) == typeof(void*) && typeof(TTarget) != typeof(void*))
                {
                    ConvertFromObject(il);
                }
                else if (typeof(TSource) != typeof(void*) && typeof(TTarget) == typeof(void*))
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

        protected: static MethodInfo GetMethodForConversionToTargetType()
        {
            auto targetType = typeof(TTarget);
            auto convertibleType = typeof(IConvertible);
            auto typeParameters = Types<IFormatProvider>.Array;
            if (targetType == typeof(bool))
            {
                return convertibleType.GetMethod("ToBoolean", typeParameters);
            }
            else if (targetType == typeof(std::uint8_t))
            {
                return convertibleType.GetMethod("ToByte", typeParameters);
            }
            else if (targetType == typeof(char))
            {
                return convertibleType.GetMethod("ToChar", typeParameters);
            }
            else if (targetType == typeof(DateTime))
            {
                return convertibleType.GetMethod("ToDateTime", typeParameters);
            }
            }
            else if (targetType == typeof(double))
            {
                return convertibleType.GetMethod("ToDouble", typeParameters);
            }
            else if (targetType == typeof(std::int16_t))
            {
                return convertibleType.GetMethod("ToInt16", typeParameters);
            }
            else if (targetType == typeof(std::int32_t))
            {
                return convertibleType.GetMethod("ToInt32", typeParameters);
            }
            else if (targetType == typeof(std::int64_t))
            {
                return convertibleType.GetMethod("ToInt64", typeParameters);
            }
            else if (targetType == typeof(std::int8_t))
            {
                return convertibleType.GetMethod("ToSByte", typeParameters);
            }
            else if (targetType == typeof(float))
            {
                return convertibleType.GetMethod("ToSingle", typeParameters);
            }
            else if (targetType == typeof(std::string))
            {
                return convertibleType.GetMethod("ToString", typeParameters);
            }
            else if (targetType == typeof(std::uint16_t))
            {
                return convertibleType.GetMethod("ToUInt16", typeParameters);
            }
            else if (targetType == typeof(std::uint32_t))
            {
                return convertibleType.GetMethod("ToUInt32", typeParameters);
            }
            else if (targetType == typeof(std::uint64_t))
            {
                return convertibleType.GetMethod("ToUInt64", typeParameters);
            }
            else
            {
                throw std::logic_error("Not supported exception.");
            }
        }

        protected: static void LoadDefault(ILGenerator &il, Type targetType)
        {
            if (targetType == typeof(std::string))
            {
                il.Emit(OpCodes.Ldsfld, targetType.GetField("Empty", BindingFlags.Static | BindingFlags.Public));
            }
            else if (targetType == typeof(DateTime))
            {
                il.Emit(OpCodes.Ldsfld, targetType.GetField("MinValue", BindingFlags.Static | BindingFlags.Public));
            }
            }
            else if (targetType == typeof(float))
            {
                il.LoadConstant(0.0F);
            }
            else if (targetType == typeof(double))
            {
                il.LoadConstant(0.0D);
            }
            else if (targetType == typeof(std::int64_t) || targetType == typeof(std::uint64_t))
            {
                il.LoadConstant(0L);
            }
            else
            {
                il.LoadConstant(0);
            }
        }
    };
}
