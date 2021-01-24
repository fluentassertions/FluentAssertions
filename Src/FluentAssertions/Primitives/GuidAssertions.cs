﻿using System;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="Guid"/> is in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class GuidAssertions : GuidAssertions<GuidAssertions>
    {
        public GuidAssertions(Guid? value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <see cref="Guid"/> is in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class GuidAssertions<TAssertions>
        where TAssertions : GuidAssertions<TAssertions>
    {
        public GuidAssertions(Guid value)
            : this((Guid?)value)
        {
        }

        private protected GuidAssertions(Guid? value)
        {
            SubjectInternal = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public Guid Subject => SubjectInternal.Value;

        private protected Guid? SubjectInternal { get; }

        #region BeEmpty / NotBeEmpty

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(SubjectInternal.HasValue && (SubjectInternal.Value == Guid.Empty))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Guid} to be empty{reason}, but found {0}.", SubjectInternal);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is not <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(SubjectInternal.HasValue && (SubjectInternal.Value != Guid.Empty))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:Guid} to be empty{reason}.");

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Be(string expected, string because = "", params object[] becauseArgs)
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Be(Guid expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(SubjectInternal.HasValue && SubjectInternal.Value == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Guid} to be {0}{reason}, but found {1}.", expected, SubjectInternal);

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBe(Guid unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!SubjectInternal.HasValue || SubjectInternal.Value != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:Guid} to be {0}{reason}.", SubjectInternal);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        #endregion
    }
}
