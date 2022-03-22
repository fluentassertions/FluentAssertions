﻿using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="ushort"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableUShortAssertions : NullableNumericAssertions<ushort>
    {
        public NullableUShortAssertions(ushort? value)
            : base(value)
        {
        }

        private protected override ushort? CalculateDifferenceForFailureMessage(ushort expected)
        {
            if (Subject!.Value < 10 && expected < 10)
            {
                return null;
            }

            var difference = (ushort?)(Subject - expected);
            return difference != 0 ? difference : null;
        }
    }
}
