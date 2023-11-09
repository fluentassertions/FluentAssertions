﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FluentAssertions;

/// <summary>
/// Since net6.0 StringBuilder has additional overloads taking an AppendInterpolatedStringHandler
/// and optionally an IFormatProvider.
/// The overload here is polyfill for older target frameworks to avoid littering the code base with #ifs
/// in order to silence analyzers about dependending on the current culture instead of an invariant culture.
/// </summary>
internal static class StringBuilderExtensions
{
    public static StringBuilder AppendLine(this StringBuilder stringBuilder, IFormatProvider _, string value) =>
        stringBuilder.AppendLine(value);

#if NET47 || NETSTANDARD2_0
    public static StringBuilder AppendJoin<T>(this StringBuilder stringBuilder, string separator, IEnumerable<T> values) =>
        stringBuilder.Append(string.Join(separator, values));
#endif
}
