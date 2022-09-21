using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using SpanJson.Internal;

namespace SpanJson
{
    internal static partial class ThrowHelper
    {
        #region -- Throw ArgumentException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException()
        {
            throw GetArgumentException();
            static ArgumentException GetArgumentException()
            {
                return new ArgumentException();
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException_SerializerSettings_same_instance()
        {
            throw GetArgumentException();
            static ArgumentException GetArgumentException()
            {
                return new ArgumentException("The serializerSettings and deserializerSettings");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException(ExceptionResource resource)
        {
            throw GetArgumentException(resource);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
        {
            throw GetArgumentException(resource, argument);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException(string message, ExceptionArgument argument)
        {
            throw GetArgumentException();
            ArgumentException GetArgumentException()
            {
                return new ArgumentException(message, GetArgumentName(argument));

            }
        }

        #endregion

        #region -- Get ArgumentException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentException GetArgumentException(ExceptionResource resource)
        {
            return new ArgumentException(GetResourceString(resource));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentException GetArgumentException(ExceptionResource resource, ExceptionArgument argument)
        {
            return new ArgumentException(GetResourceString(resource), GetArgumentName(argument));
        }

        #endregion


        #region -- Throw ArgumentOutOfRangeException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException()
        {
            throw GetArgumentOutOfRangeException();

            static ArgumentOutOfRangeException GetArgumentOutOfRangeException()
            {
                return new ArgumentOutOfRangeException();
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        {
            throw GetArgumentOutOfRangeException(argument);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
        {
            throw GetArgumentOutOfRangeException(argument, resource);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, int paramNumber, ExceptionResource resource)
        {
            throw GetArgumentOutOfRangeException(argument, paramNumber, resource);
        }

        #endregion

        #region -- Get ArgumentOutOfRangeException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceString(resource));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, int paramNumber, ExceptionResource resource)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument) + "[" + paramNumber.ToString() + "]", GetResourceString(resource));
        }

        #endregion


        #region -- Throw ArgumentNullException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw GetArgumentNullException(argument);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentNullException(ExceptionResource resource)
        {
            throw GetArgumentNullException(resource);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentNullException(ExceptionArgument argument, ExceptionResource resource)
        {
            throw GetArgumentNullException(argument, resource);
        }

        #endregion

        #region -- Get ArgumentNullException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentNullException GetArgumentNullException(ExceptionArgument argument)
        {
            return new ArgumentNullException(GetArgumentName(argument));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentNullException GetArgumentNullException(ExceptionResource resource)
        {
            return new ArgumentNullException(GetResourceString(resource), innerException: null);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentNullException GetArgumentNullException(ExceptionArgument argument, ExceptionResource resource)
        {
            return new ArgumentNullException(GetArgumentName(argument), GetResourceString(resource));
        }

        #endregion


        #region -- IndexOutOfRangeException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowIndexOutOfRangeException()
        {
            throw GetIndexOutOfRangeException();

            static IndexOutOfRangeException GetIndexOutOfRangeException()
            {
                return new IndexOutOfRangeException();
            }
        }

        #endregion

        #region -- Throw InvalidOperationException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException(ExceptionResource resource)
        {
            throw GetInvalidOperationException(resource);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException(ExceptionResource resource, Exception e)
        {
            throw GetInvalidOperationException();
            InvalidOperationException GetInvalidOperationException()
            {
                return new InvalidOperationException(GetResourceString(resource), e);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static InvalidOperationException GetInvalidOperationException(ExceptionResource resource)
        {
            return new InvalidOperationException(GetResourceString(resource));
        }

        #endregion

        #region ** GetArgumentName **

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static string GetArgumentName(ExceptionArgument argument)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionArgument), argument),
                "The enum value is not defined, please check the ExceptionArgument Enum.");

            return argument.ToString();
        }

        #endregion

        #region ** GetResourceString **

        // This function will convert an ExceptionResource enum value to the resource string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ExceptionResource resource)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionResource), resource),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            return SR.GetResourceString(resource.ToString());
        }

        #endregion
    }
}
