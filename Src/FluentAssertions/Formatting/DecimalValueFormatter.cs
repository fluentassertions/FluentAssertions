﻿using System.Globalization;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class DecimalValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is decimal;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            Guard.ThrowIfArgumentIsNull(value, nameof(value));
            Guard.ThrowIfArgumentIsNull(formattedGraph, nameof(formattedGraph));

            formattedGraph.AddFragment(((decimal)value).ToString(CultureInfo.InvariantCulture) + "M");
        }
    }
}
