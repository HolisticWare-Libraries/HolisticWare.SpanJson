using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Text;
using SpanJson.Internal;

namespace SpanJson
{
    #region -- ExceptionArgument --

    /// <summary>The convention for this enum is using the argument name as the enum name</summary>
    internal enum ExceptionArgument
    {
        o,
        s,
        t,
        doc,
        key,
        json,
        type,
        name,
        inner,
        array,
        count,
        index,
        input,
        token,
        value,
        other,
        stream,
        writer,
        reader,
        length,
        format,
        offset,
        method,
        source,
        initial,
        inArray,
        element,
        newSize,
        utf8Json,
        offsetIn,
        outArray,
        jsonData,
        property,
        utf16Json,
        offsetOut,
        container,
        fieldInfo,
        annotation,
        enumerable,
        expression,
        objectType,
        initialValue,
        propertyInfo,
        propertyName,
        configSettings,
        jsonSerializer,
        configSerializer,
        serializerSettings,
        deserializerSettings,
        genericInterfaceDefinition,
    }

    #endregion

    #region -- ExceptionResource --

    /// <summary>The convention for this enum is using the resource name as the enum name</summary>
    internal enum ExceptionResource
    {
        ArrayDepthTooLarge,
        BufferMaximumSizeExceeded,
        EndOfCommentNotFound,
        EndOfStringNotFound,
        RequiredDigitNotFoundAfterDecimal,
        RequiredDigitNotFoundAfterSign,
        RequiredDigitNotFoundEndOfData,
        ExpectedEndAfterSingleJson,
        ExpectedEndOfDigitNotFound,
        ExpectedFalse,
        ExpectedNextDigitEValueNotFound,
        ExpectedNull,
        ExpectedSeparatorAfterPropertyNameNotFound,
        ExpectedStartOfPropertyNotFound,
        ExpectedStartOfPropertyOrValueNotFound,
        ExpectedStartOfPropertyOrValueAfterComment,
        ExpectedStartOfValueNotFound,
        ExpectedTrue,
        ExpectedValueAfterPropertyNameNotFound,
        FoundInvalidCharacter,
        InvalidCharacterWithinString,
        InvalidCharacterAfterEscapeWithinString,
        InvalidHexCharacterWithinString,
        InvalidEndOfJsonNonPrimitive,
        InvalidLeadingZeroInNumber,
        MismatchedObjectArray,
        ObjectDepthTooLarge,
        ZeroDepthAtEnd,
        DepthTooLarge,
        CannotStartObjectArrayWithoutProperty,
        CannotStartObjectArrayAfterPrimitiveOrClose,
        CannotWriteValueWithinObject,
        CannotWriteValueAfterPrimitiveOrClose,
        CannotWritePropertyWithinArray,
        ExpectedJsonTokens,
        TrailingCommaNotAllowedBeforeArrayEnd,
        TrailingCommaNotAllowedBeforeObjectEnd,
        InvalidCharacterAtStartOfComment,
        UnexpectedEndOfDataWhileReadingComment,
        UnexpectedEndOfLineSeparator,
        ExpectedOneCompleteToken,
        NotEnoughData,
    }

    #endregion

    partial class ThrowHelper
    {
        #region -- Exception --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowException_UnreachedCode()
        {
            throw GetException();
            static Exception GetException()
            {
                return new Exception("unreached code.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowException_UnreachableCode()
        {
            throw GetException();
            static Exception GetException()
            {
                return new Exception("Unreachable code.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowException_InvalidMode()
        {
            throw GetException();
            static Exception GetException()
            {
                return new Exception("Invalid Mode.");
            }
        }

        #endregion

        #region -- ArgumentException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentException GetArgumentException_Guid_Pattern()
        {
            return new ArgumentException("Invalid Guid Pattern.");
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException_Guid_Pattern()
        {
            throw GetArgumentException_Guid_Pattern();
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException_Length()
        {
            throw GetException();
            static ArgumentException GetException()
            {
                return new ArgumentException("length < newSize");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException_InvalidDoubleValueForJson()
        {
            throw GetException();
            static ArgumentException GetException()
            {
                return new ArgumentException("Invalid double value for JSON", "value");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException_InvalidFloatValueForJson()
        {
            throw GetException();
            static ArgumentException GetException()
            {
                return new ArgumentException("Invalid float value for JSON", "value");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException_EnumIllegalVal(Base64FormattingOptions options)
        {
            throw GetException();
            ArgumentException GetException()
            {
                return new ArgumentException($"Illegal enum value: {options}.", nameof(options));
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException_IsNotAnInterface(Type interfaceType)
        {
            throw GetException();
            ArgumentException GetException()
            {
                return new ArgumentException($"{interfaceType} is not an interface.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentException_InvalidUTF16(int charAsInt)
        {
            throw GetException();
            ArgumentException GetException()
            {
                return new ArgumentException(string.Format("Cannot encode invalid UTF-16 text as JSON. Invalid surrogate value: '{0}'.", $"0x{charAsInt:X2}"));
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentException_InvalidUTF8(ReadOnlySpan<byte> value, int bytesWritten)
        {
            var builder = new StringBuilder();

            value = value.Slice(bytesWritten);
            int printFirst10 = Math.Min(value.Length, 10);

            for (int i = 0; i < printFirst10; i++)
            {
                byte nextByte = value[i];
                if (IsPrintable(nextByte))
                {
                    builder.Append((char)nextByte);
                }
                else
                {
                    builder.Append($"0x{nextByte:X2}");
                }
            }

            if (printFirst10 < value.Length)
            {
                builder.Append("...");
            }

            static bool IsPrintable(byte value) => value >= 0x20 && value < 0x7F;

            throw new ArgumentException($"Cannot encode invalid UTF-8 text as JSON. Invalid input: '{builder}'.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ArgumentException GetArgumentException_ReadInvalidUTF16(EncoderFallbackException innerException)
        {
            return new ArgumentException("Cannot transcode invalid UTF-16 string to UTF-8 JSON text.", innerException);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentException_ValueTooLarge(int tokenLength)
        {
            throw GetException();
            ArgumentException GetException()
            {
                return new ArgumentException(string.Format("The JSON value of length {0} is too large and not supported.", tokenLength));
            }
        }

        #endregion

        #region -- ArgumentOutOfRangeException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArgumentOutOfRangeException GetArgumentOutOfRangeException()
        {
            return new ArgumentOutOfRangeException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRange_IndexException()
        {
            throw GetException();

            static ArgumentOutOfRangeException GetException()
            {
                return new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException_Index(ExceptionArgument argument)
        {
            throw GetException();
            ArgumentOutOfRangeException GetException()
            {
                var argumentName = GetArgumentName(argument);
                return new ArgumentOutOfRangeException(argumentName, "Index was out of range. Must be non-negative and less than the size of the collection.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException_GenericPositive(ExceptionArgument argument)
        {
            throw GetException();
            ArgumentOutOfRangeException GetException()
            {
                var argumentName = GetArgumentName(argument);
                return new ArgumentOutOfRangeException(argumentName, "Value must be positive.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException_OffsetLength(ExceptionArgument argument)
        {
            throw GetException();
            ArgumentOutOfRangeException GetException()
            {
                var argumentName = GetArgumentName(argument);
                return new ArgumentOutOfRangeException(argumentName, "Offset and length must refer to a position in the string.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException_OffsetOut(ExceptionArgument argument)
        {
            throw GetException();
            ArgumentOutOfRangeException GetException()
            {
                var argumentName = GetArgumentName(argument);
                return new ArgumentOutOfRangeException(argumentName, "Either offset did not refer to a position in the string, or there is an insufficient length of destination character array.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException_Nonnegative(ExceptionArgument argument)
        {
            throw GetException();
            ArgumentOutOfRangeException GetException()
            {
                var argumentName = GetArgumentName(argument);
                return new ArgumentOutOfRangeException(argumentName, $"{argumentName} should be non negative.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeException_CommentEnumMustBeInRange(ExceptionArgument argument)
        {
            throw GetException();
            ArgumentOutOfRangeException GetException()
            {
                var argumentName = GetArgumentName(argument);
                return new ArgumentOutOfRangeException(argumentName, SR.CommentHandlingMustBeValid);
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeException_MaxDepthMustBePositive(ExceptionArgument argument)
        {
            throw GetException();
            ArgumentOutOfRangeException GetException()
            {
                var argumentName = GetArgumentName(argument);
                return new ArgumentOutOfRangeException(argumentName, SR.MaxDepthMustBePositive);
            }
        }

        #endregion

        #region -- InvalidOperationException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException()
        {
            throw GetException();
            static InvalidOperationException GetException()
            {
                return new InvalidOperationException();
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException_NullArray()
        {
            throw GetException();

            static InvalidOperationException GetException()
            {
                return new InvalidOperationException("The underlying array is null.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException_Register_Resolver_Err()
        {
            throw GetException();
            static InvalidOperationException GetException()
            {
                return new InvalidOperationException("Register must call on startup(before use GetFormatter<T>).");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException_NotSupport_Value(float value)
        {
            throw GetException();
            InvalidOperationException GetException()
            {
                return new InvalidOperationException("not support float value:" + value);
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException_NotSupport_Value(double value)
        {
            throw GetException();
            InvalidOperationException GetException()
            {
                return new InvalidOperationException("not support double value:" + value);
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException_Reached_MaximumSize()
        {
            throw GetException();
            static InvalidOperationException GetException()
            {
                return new InvalidOperationException("byte[] size reached maximum size of array(0x7FFFFFC7), can not write to single byte[]. Details: https://msdn.microsoft.com/en-us/library/system.array");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException_NestingLimitOfExceeded()
        {
            throw GetException();
            static InvalidOperationException GetException()
            {
                return new InvalidOperationException($"Nesting Limit of {JsonSharedConstant.NestingLimit} exceeded.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException_CanotMapConstructorParameterToAnyMember(ParameterInfo constructorParameter)
        {
            throw GetException();
            InvalidOperationException GetException()
            {
                return new InvalidOperationException($"Can't map constructor parameter {constructorParameter.Name} to any member.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowInvalidOperationException_ReadInvalidUTF16(int charAsInt)
        {
            throw GetException();
            InvalidOperationException GetException()
            {
                return new InvalidOperationException(string.Format("Cannot read invalid UTF-16 JSON text as string. Invalid surrogate value: '{0}'.", $"0x{charAsInt:X2}"));
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowInvalidOperationException_ReadInvalidUTF16()
        {
            throw GetException();

            static InvalidOperationException GetException()
            {
                return new InvalidOperationException("Cannot read incomplete UTF-16 JSON text as string with missing low surrogate.");
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowInvalidOperationException_AdvancedTooFar(int capacity)
        {
            throw GetException();
            InvalidOperationException GetException()
            {
                return new InvalidOperationException($"Cannot advance past the end of the buffer, which has a size of {capacity}.");
            }
        }

        #endregion

        #region -- NotSupportedException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static NotSupportedException GetNotSupportedException()
        {
            return new NotSupportedException();
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowNotSupportedException()
        {
            throw GetNotSupportedException();
        }

        #endregion

        #region -- NotImplementedException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static NotImplementedException GetNotImplementedException()
        {
            return new NotImplementedException();
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowNotImplementedException()
        {
            throw GetNotImplementedException();
        }

        #endregion

        #region -- InvalidCastException --

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static InvalidCastException GetInvalidCastException()
        {
            return new InvalidCastException();
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidCastException()
        {
            throw GetInvalidCastException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InvalidOperationException GetInvalidOperationException_ReadInvalidUTF8(DecoderFallbackException innerException)
        {
            return new InvalidOperationException("Cannot transcode invalid UTF-8 JSON text to UTF-16 string.", innerException);
        }

        #endregion

        #region -- JsonParserException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowJsonParserException(JsonParserException.ParserError error, JsonParserException.ValueType type, int position)
        {
            throw GetJsonParserException(error, type, position);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static JsonParserException GetJsonParserException(JsonParserException.ParserError error, JsonParserException.ValueType type, int position)
        {
            return new JsonParserException(error, type, position);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowJsonParserException(JsonParserException.ParserError error, int position)
        {
            throw GetJsonParserException(error, position);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static JsonParserException GetJsonParserException(JsonParserException.ParserError error, int position)
        {
            return new JsonParserException(error, position);
        }

        #endregion

        #region -- FormatException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowFormatException_BadBase64Char()
        {
            throw GetException();
            static FormatException GetException()
            {
                return new FormatException("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.");
            }
        }

        #endregion

        #region -- OutOfMemoryException --

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowOutOfMemoryException()
        {
            throw GetException();
            static OutOfMemoryException GetException()
            {
                return new OutOfMemoryException();
            }
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowOutOfMemoryException(uint capacity)
        {
            throw GetException();
            OutOfMemoryException GetException()
            {
                return new OutOfMemoryException(SR.Format(SR.BufferMaximumSizeExceeded, capacity));
            }
        }

        #endregion
    }
}
