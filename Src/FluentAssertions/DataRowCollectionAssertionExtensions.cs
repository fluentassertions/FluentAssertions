﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Data;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    public static class DataRowCollectionAssertionExtensions
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
        public static AndConstraint<GenericCollectionAssertions<DataRow>> BeSameAs(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection expected, string because = "",
            params object[] becauseArgs)
        {
            if (assertion.Subject is ReadOnlyNonGenericCollectionWrapper<DataRowCollection, DataRow> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(ReferenceEquals(actualSubject, expected))
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:row collection} to refer to {0}{reason}, but found {1} (different underlying object).",
                        expected, actualSubject);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Invalid expectation: Expected {context:column collection} to refer to an instance of " +
                        "DataRowCollection{reason}, but found {0}.",
                        assertion.Subject);
            }

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
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
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotBeSameAs(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection unexpected, string because = "",
            params object[] becauseArgs)
        {
            if (assertion.Subject is ReadOnlyNonGenericCollectionWrapper<DataRowCollection, DataRow> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(!ReferenceEquals(actualSubject, unexpected))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:row collection} to refer to {0}{reason}.", unexpected);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Invalid expectation: Expected {context:column collection} to refer to a different instance of " +
                        "DataRowCollection{reason}, but found {0}.",
                        assertion.Subject);
            }

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Assert that the current collection of <see cref="DataRow"/>s has the same number of rows as
        /// <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other collection with the same expected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> HaveSameCount(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection otherCollection, string because = "",
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
                .FailWith("{0} row(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Assert that the current collection of <see cref="DataRow"/>s does not have the same number of rows as
        /// <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other <see cref="DataRowCollection"/> with the unexpected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotHaveSameCount(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection otherCollection, string because = "",
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
                .FailWith("{0} row(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        private static bool RowsAreEquivalent(AssertionScope scope, EquivalencyAssertionOptions<DataRow> options,
            DataRow actualItem, DataRow expectedItem, string because, object[] becauseArgs)
        {
            var context = new EquivalencyValidationContext(
                Node.From<DataRow>(() => CallerIdentifier.DetermineCallerIdentity()), options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter
            };

            var comparands = new Comparands
            {
                Subject = actualItem,
                Expectation = expectedItem,
                CompileTimeType = typeof(DataRow),
            };

            new EquivalencyValidator().AssertEquality(comparands, context);

            string[] failures = scope.Discard();

            return !failures.Any();
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> is a subset of the <paramref name="expectedSuperset" />.
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </summary>
        /// <param name="expectedSuperset">A <see cref="DataRowCollection"/> with the expected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> BeSubsetOf(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection expectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                expectedSuperset, nameof(expectedSuperset), "Cannot verify a subset against a <null> collection.");

            return BeSubsetOf(assertion, expectedSuperset.OfType<DataRow>(), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> is a subset of the <paramref name="expectedSuperset" />.
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </summary>
        /// <param name="expectedSuperset">An <see cref="IEnumerable{DataRow}"/> with the expected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> BeSubsetOf(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> expectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            return BeSubsetOf(assertion, expectedSuperset, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> is a subset of the <paramref name="expectedSuperset" />.
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </summary>
        /// <param name="expectedSuperset">An <see cref="IEnumerable{DataRow}"/> with the expected superset.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{DataRow}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{DataRow}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> BeSubsetOf(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> expectedSuperset,
            Func<EquivalencyAssertionOptions<DataRow>, EquivalencyAssertionOptions<DataRow>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                expectedSuperset, nameof(expectedSuperset), "Cannot verify a subset against a <null> collection.");
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to be a subset of {0}{reason}, ", expectedSuperset)
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found {0}.", assertion.Subject)
                .Then
                .Given(
                    (subject) =>
                    {
                        EquivalencyAssertionOptions<DataRow> options = config(AssertionOptions.CloneDefaults<DataRow>());

                        var actualItems = new List<DataRow>(assertion.Subject.OfType<DataRow>());
                        var expectedItems = new List<DataRow>(expectedSuperset.OfType<DataRow>());

                        using (var scope = new AssertionScope())
                        {
                            scope.AddReportable("configuration", options.ToString());

                            int actualIndex = 0;

                            while (actualIndex < actualItems.Count)
                            {
                                var actualItem = actualItems[actualIndex];

                                for (int expectedIndex = 0; expectedIndex < expectedItems.Count; expectedIndex++)
                                {
                                    var expectedItem = expectedItems[expectedIndex];

                                    if (RowsAreEquivalent(scope, options, actualItem, expectedItem, because, becauseArgs))
                                    {
                                        actualItems.RemoveAt(actualIndex);
                                        expectedItems.RemoveAt(expectedIndex);

                                        actualIndex--;

                                        break;
                                    }
                                }

                                actualIndex++;
                            }
                        }

                        return (actualItemsNotInExpected: actualItems, expectedItems);
                    })
                .ForCondition(results => !results.actualItemsNotInExpected.Any())
                .FailWith("but item(s) {0} are not part of the superset.", results => results.actualItemsNotInExpected)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> is not a subset of the <paramref name="unexpectedSuperset" />.
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </summary>
        /// <param name="unexpectedSuperset">A <see cref="DataRowCollection"/> with the unexpected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotBeSubsetOf(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection unexpectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                unexpectedSuperset, nameof(unexpectedSuperset), "Cannot verify a subset against a <null> collection.");

            return NotBeSubsetOf(assertion, unexpectedSuperset.OfType<DataRow>(), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> is not a subset of the <paramref name="unexpectedSuperset" />.
        /// </summary>
        /// <param name="unexpectedSuperset">An <see cref="IEnumerable{DataRow}"/> with the unexpected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotBeSubsetOf(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> unexpectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            return NotBeSubsetOf(assertion, unexpectedSuperset, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> is not a subset of the <paramref name="unexpectedSuperset" />.
        /// </summary>
        /// <param name="unexpectedSuperset">An <see cref="IEnumerable{DataRow}"/> with the unexpected superset.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{DataRow}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{DataRow}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotBeSubsetOf(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> unexpectedSuperset,
            Func<EquivalencyAssertionOptions<DataRow>, EquivalencyAssertionOptions<DataRow>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(
                unexpectedSuperset, nameof(unexpectedSuperset), "Cannot verify a subset against a <null> collection.");
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to not be a subset of {0}{reason}, ", unexpectedSuperset)
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found {0}.", assertion.Subject)
                .Then
                .Given(
                    subject =>
                    {
                        EquivalencyAssertionOptions<DataRow> options = config(AssertionOptions.CloneDefaults<DataRow>());

                        var actualItems = new List<DataRow>(assertion.Subject.OfType<DataRow>());
                        var unexpectedItems = new List<DataRow>(unexpectedSuperset.OfType<DataRow>());

                        using (var scope = new AssertionScope())
                        {
                            scope.AddReportable("configuration", options.ToString());

                            int actualIndex = 0;

                            while (actualIndex < actualItems.Count)
                            {
                                var actualItem = actualItems[actualIndex];

                                bool foundMatch = false;

                                for (int unexpectedIndex = 0; unexpectedIndex < unexpectedItems.Count; unexpectedIndex++)
                                {
                                    var unexpectedItem = unexpectedItems[unexpectedIndex];

                                    if (RowsAreEquivalent(scope, options, actualItem, unexpectedItem, because, becauseArgs))
                                    {
                                        actualItems.RemoveAt(actualIndex);
                                        unexpectedItems.RemoveAt(unexpectedIndex);

                                        actualIndex--;

                                        foundMatch = true;

                                        break;
                                    }
                                }

                                if (!foundMatch)
                                {
                                    break;
                                }

                                actualIndex++;
                            }
                        }

                        return (actualItemsNotInExpected: actualItems, unexpectedItems);
                    })
                .ForCondition(results => results.actualItemsNotInExpected.Any())
                .FailWith("but all items are part of the superset.")
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> shares one or more rows with the specified
        /// <paramref name="otherCollection"/>.
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </summary>
        /// <param name="otherCollection">The <see cref="DataRowCollection"/> with the expected shared
        /// <see cref="DataRow"/> objects.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> IntersectWith(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            return IntersectWith(assertion, otherCollection.OfType<DataRow>(), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> shares one or more rows with the specified
        /// <paramref name="otherRows"/>.
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </summary>
        /// <param name="otherRows">The <see cref="IEnumerable{DataRow}"/> with the expected shared
        /// <see cref="DataRow"/> objects.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> IntersectWith(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> otherRows, string because = "",
            params object[] becauseArgs)
        {
            return IntersectWith(assertion, otherRows, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> shares one or more rows with the specified
        /// <paramref name="otherRows"/>.
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </summary>
        /// <param name="otherRows">The <see cref="IEnumerable{DataRow}"/> with the expected shared <see cref="DataRow"/>
        /// objects.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{DataRow}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{DataRow}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> IntersectWith(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> otherRows,
            Func<EquivalencyAssertionOptions<DataRow>, EquivalencyAssertionOptions<DataRow>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherRows, nameof(otherRows), "Cannot verify intersection against a <null> collection.");
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("Expected {context:collection} to intersect {0}{reason}, but found {1}.", otherRows,
                    assertion.Subject)
                .Then
                .Given(
                    subject =>
                    {
                        EquivalencyAssertionOptions<DataRow> options = config(AssertionOptions.CloneDefaults<DataRow>());

                        var actualItems = new List<DataRow>(assertion.Subject.OfType<DataRow>());
                        var expectedItems = new List<DataRow>(otherRows);

                        bool foundMatch = false;

                        using (var scope = new AssertionScope())
                        {
                            scope.AddReportable("configuration", options.ToString());

                            for (int actualIndex = 0; actualIndex < actualItems.Count; actualIndex++)
                            {
                                var actualItem = actualItems[actualIndex];

                                for (int unexpectedIndex = 0; unexpectedIndex < expectedItems.Count; unexpectedIndex++)
                                {
                                    var unexpectedItem = expectedItems[unexpectedIndex];

                                    if (RowsAreEquivalent(scope, options, actualItem, unexpectedItem, because, becauseArgs))
                                    {
                                        foundMatch = true;

                                        break;
                                    }
                                }

                                if (foundMatch)
                                {
                                    break;
                                }
                            }
                        }

                        return foundMatch;
                    })
                .ForCondition(foundAtLeastOneMatchingItem => foundAtLeastOneMatchingItem)
                .FailWith(
                    "Expected {context:collection} to intersect the supplied set{reason}, but couldn't find any items in common.")
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> does not share any items with the specified
        /// <paramref name="otherCollection"/>.
        /// </summary>
        /// <param name="otherCollection">The <see cref="DataRowCollection"/> to compare to.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotIntersectWith(
            this GenericCollectionAssertions<DataRow> assertion, DataRowCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            return NotIntersectWith(assertion, otherCollection.OfType<DataRow>(), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> does not share any items with the specified
        /// <paramref name="otherRows"/>.
        /// </summary>
        /// <param name="otherRows">The <see cref="IEnumerable{DataRow}"/> to compare to.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotIntersectWith(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> otherRows, string because = "",
            params object[] becauseArgs)
        {
            return NotIntersectWith(assertion, otherRows, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DataRowCollection"/> does not share any items with the specified
        /// <paramref name="otherRows"/>.
        /// </summary>
        /// <param name="otherRows">The <see cref="IEnumerable{DataRow}"/> to compare to.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{DataRow}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{DataRow}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotIntersectWith(
            this GenericCollectionAssertions<DataRow> assertion, IEnumerable<DataRow> otherRows,
            Func<EquivalencyAssertionOptions<DataRow>, EquivalencyAssertionOptions<DataRow>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherRows, nameof(otherRows), "Cannot verify intersection against a <null> collection.");
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("Expected {context:collection} to not intersect {0}{reason}, but found {1}.", otherRows,
                    assertion.Subject)
                .Then
                .Given(
                    subject =>
                    {
                        EquivalencyAssertionOptions<DataRow> options = config(AssertionOptions.CloneDefaults<DataRow>());

                        var actualItems = new List<DataRow>(assertion.Subject.OfType<DataRow>());
                        var expectedItems = new List<DataRow>(otherRows);

                        var matchingRow = default(DataRow);

                        using (var scope = new AssertionScope())
                        {
                            scope.AddReportable("configuration", options.ToString());

                            for (int actualIndex = 0; actualIndex < actualItems.Count; actualIndex++)
                            {
                                var actualItem = actualItems[actualIndex];

                                for (int unexpectedIndex = 0; unexpectedIndex < expectedItems.Count; unexpectedIndex++)
                                {
                                    var unexpectedItem = expectedItems[unexpectedIndex];

                                    if (RowsAreEquivalent(scope, options, actualItem, unexpectedItem, because, becauseArgs))
                                    {
                                        matchingRow = actualItem;

                                        break;
                                    }
                                }

                                if (matchingRow is not null)
                                {
                                    break;
                                }
                            }
                        }

                        return matchingRow;
                    })
                .ForCondition(matchingRow => matchingRow is null)
                .FailWith(
                    "Expected {context:collection} to not intersect the supplied set{reason}, but found row in common: {0}.",
                    matchingRow => matchingRow)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }
    }
}
