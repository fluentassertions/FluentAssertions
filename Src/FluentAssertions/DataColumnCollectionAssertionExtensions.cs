﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    public static class DataColumnCollectionAssertionExtensions
    {
        /// <summary>
        /// Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> BeSameAs(
            this GenericCollectionAssertions<DataColumn> assertion, DataColumnCollection expected, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                expected, nameof(expected), "Cannot verify same reference against a <null> collection (use BeNull instead?).");

            if (assertion.Subject is ReadOnlyNonGenericCollectionWrapper<DataColumnCollection, DataColumn> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(ReferenceEquals(actualSubject, expected))
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:column collection} to refer to {0}{reason}, but found {1} (different underlying object).",
                        expected, actualSubject);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Invalid expectation: Expected {context:column collection} to refer to an instance of " +
                        "DataColumnCollection{reason}, but found {0}.",
                        assertion.Subject);
            }

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        /// <param name="unexpected">The unexpected object</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> NotBeSameAs(
            this GenericCollectionAssertions<DataColumn> assertion, DataColumnCollection unexpected, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                unexpected, nameof(unexpected), "Cannot verify same reference against a <null> collection (use NotBeNull instead?).");

            if (assertion.Subject is ReadOnlyNonGenericCollectionWrapper<DataColumnCollection, DataColumn> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(!ReferenceEquals(actualSubject, unexpected))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:column collection} to refer to {0}{reason}.", unexpected);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Invalid expectation: Expected {context:column collection} to refer to a different instance of " +
                        "DataColumnCollection{reason}, but found {0}.",
                        assertion.Subject);
            }

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other collection with the same expected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> HaveSameCount(
            this GenericCollectionAssertions<DataColumn> assertion, DataColumnCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to have ")
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("the same count as {0}{reason}, but found <null>.", otherCollection)
                .Then
                .Given((subject) => (actual: subject.Count(), expected: otherCollection.Count))
                .ForCondition(count => count.actual == count.expected)
                .FailWith("{0} column(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Assert that the current collection of <see cref="DataColumn"/>s does not have the same number of columns as
        /// <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other <see cref="DataColumnCollection"/> with the unexpected number of
        /// elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> NotHaveSameCount(
            this GenericCollectionAssertions<DataColumn> assertion, DataColumnCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to not have ")
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("the same count as {0}{reason}, but found <null>.", otherCollection)
                .Then
                .Given((subject) => (actual: subject.Count(), expected: otherCollection.Count))
                .ForCondition(count => count.actual != count.expected)
                .FailWith("{0} column(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Asserts that the current collection of <see cref="DataColumn"/>s contains a <see cref="DataColumn"/> with the
        /// specified <paramref name="expectedColumnName"/> name.
        /// </summary>
        /// <param name="expectedColumnName">A name for a <see cref="DataColumn"/> that is expected to be in the
        /// <see cref="DataColumnCollection"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> ContainColumnWithName(
            this GenericCollectionAssertions<DataColumn> assertion, string expectedColumnName, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                expectedColumnName, nameof(expectedColumnName), "Cannot verify that the collection contains a <null> DataColumn.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to contain column named {0}{reason}, ", expectedColumnName)
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found {0}.", assertion.Subject)
                .Then
                .ForCondition(
                    (subject) => (subject is ReadOnlyNonGenericCollectionWrapper<DataColumnCollection, DataColumn> wrapper)
                    ? wrapper.UnderlyingCollection.Contains(expectedColumnName)
                    : assertion.Subject.Any(column => column.ColumnName == expectedColumnName))
                .FailWith("but it does not.")
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Asserts that the current collection of <see cref="DataColumn"/>s does not contain a column with the the supplied
        /// <paramref name="unexpectedColumnName" />.
        /// </summary>
        /// <param name="unexpectedColumnName">The column name that is not expected to be in the
        /// <see cref="DataColumnCollection" /></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> NotContainColumnWithName(
            this GenericCollectionAssertions<DataColumn> assertion, string unexpectedColumnName, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                unexpectedColumnName, nameof(unexpectedColumnName),
                "Cannot verify that the collection does not contain a <null> DataColumn.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to not contain column named {0}{reason}, ", unexpectedColumnName)
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found {0}.", assertion.Subject)
                .Then
                .ForCondition(
                    (subject) => (subject is ReadOnlyNonGenericCollectionWrapper<DataColumnCollection, DataColumn> wrapper)
                    ? !wrapper.UnderlyingCollection.Contains(unexpectedColumnName)
                    : !assertion.Subject.Any(column => column.ColumnName == unexpectedColumnName))
                .FailWith("but it does.")
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }
    }
}
