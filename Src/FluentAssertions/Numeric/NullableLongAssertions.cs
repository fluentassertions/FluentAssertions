﻿using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="long"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableLongAssertions : NullableNumericAssertions<long>
    {
        public NullableLongAssertions(long? value)
            : base(value)
        {
        }

        private protected override long? CalculateDifference(long? actual, long expected) => actual - expected;
    }
}
