using System;
using System.Reflection;
using Platform.Exceptions;
using Platform.Reflection;
using Platform.Reflection.Sigil;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Converters
{
    public sealed class To<T>
    {
        public static readonly Func<T, object> Signed;
        public static readonly Func<T, object> Unsigned;
        public static readonly Func<object, T> UnsignedAs;

        static To()
        {
            Signed = DelegateHelpers.Compile<Func<T, object>>(emiter =>
            {
                Ensure.Always.IsUnsignedInteger<T>();
                emiter.LoadArgument(0);
                var method = typeof(To).GetTypeInfo().GetMethod("Signed", Types<T>.Array);
                emiter.Call(method);
                emiter.Box(method.ReturnType);
                emiter.Return();
            });
            Unsigned = DelegateHelpers.Compile<Func<T, object>>(emiter =>
            {
                Ensure.Always.IsSignedInteger<T>();
                emiter.LoadArgument(0);
                var method = typeof(To).GetTypeInfo().GetMethod("Unsigned", Types<T>.Array);
                emiter.Call(method);
                emiter.Box(method.ReturnType);
                emiter.Return();
            });
            UnsignedAs = DelegateHelpers.Compile<Func<object, T>>(emiter =>
            {
                Ensure.Always.IsUnsignedInteger<T>();
                emiter.LoadArgument(0);
                var signedVersion = CachedTypeInfo<T>.SignedVersion;
                emiter.UnboxAny(signedVersion);
                var method = typeof(To).GetTypeInfo().GetMethod("Unsigned", new[] { signedVersion });
                emiter.Call(method);
                emiter.Return();
            });
        }

        private To()
        {
        }
    }
}
