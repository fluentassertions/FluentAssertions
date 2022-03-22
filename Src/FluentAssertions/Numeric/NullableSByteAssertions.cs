﻿using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="sbyte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableSByteAssertions : NullableNumericAssertions<sbyte>
    {
        public NullableSByteAssertions(sbyte? value)
            : base(value)
        {
        }

        private protected override sbyte? CalculateDifferenceForFailureMessage(sbyte expected)
        {
            var difference = (sbyte?)(Subject - expected);
            return difference != 0 ? difference : null;
        }
    }
}
