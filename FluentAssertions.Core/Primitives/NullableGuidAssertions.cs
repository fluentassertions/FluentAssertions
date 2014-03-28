﻿using System;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="Guid"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableGuidAssertions : GuidAssertions
    {
        public NullableGuidAssertions(Guid? value)
            : base(value)
        {
        }

        /// <summary>
        /// Asserts that a nullable <see cref="Guid"/> value is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public AndConstraint<NullableGuidAssertions> HaveValue(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<NullableGuidAssertions>(this);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="Guid"/> value is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public AndConstraint<NullableGuidAssertions> NotHaveValue(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<NullableGuidAssertions>(this);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NullableGuidAssertions> Be(Guid? expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:Guid} to be {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<NullableGuidAssertions>(this);
        }
    }
}