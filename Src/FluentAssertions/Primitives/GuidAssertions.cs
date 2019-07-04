﻿using System;
using System.Diagnostics;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="Guid"/> is in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class GuidAssertions
    {
        public GuidAssertions(Guid? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public Guid? Subject { get; private set; }

        #region BeEmpty / NotBeEmpty

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GuidAssertions> BeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition((Subject.HasValue) && (Subject.Value == Guid.Empty))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Guid_ExpectedGuidToBeEmpty + Resources.Common_CommaButFoundX0Format, Subject);

            return new AndConstraint<GuidAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is not <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GuidAssertions> NotBeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition((Subject.HasValue) && (Subject.Value != Guid.Empty))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Guid_DidNotExpectGuidToBeEmpty);

            return new AndConstraint<GuidAssertions>(this);
        }

        #endregion

        #region Be / NotBe

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
        /// </summary>
        /// <param name="expected">The expected <see cref="string"/> value to compare the actual value with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GuidAssertions> Be(string expected, string because = "", params object[] becauseArgs)
        {
            var expectedGuid = new Guid(expected);
            return Be(expectedGuid, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
        /// </summary>
        /// <param name="expected">The expected value to compare the actual value with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GuidAssertions> Be(Guid expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Guid_ExpectedGuidToBeX0Format + Resources.Common_CommaButFoundX1Format, expected, Subject);

            return new AndConstraint<GuidAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is not equal to the <paramref name="unexpected"/> GUID.
        /// </summary>
        /// <param name="unexpected">The unexpected value to compare the actual value with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GuidAssertions> NotBe(Guid unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.Equals(unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Guid_DidNotExpectGuidToBeX0Format, Subject);

            return new AndConstraint<GuidAssertions>(this);
        }

        #endregion
    }
}
