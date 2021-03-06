using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable{T}"/> is in the expectation state.
    /// </summary>
    public class SelfReferencingCollectionAssertions<T, TAssertions> : SelfReferencingCollectionAssertions<IEnumerable<T>, T, TAssertions>
        where TAssertions : SelfReferencingCollectionAssertions<IEnumerable<T>, T, TAssertions>
    {
        public SelfReferencingCollectionAssertions(IEnumerable<T> actualValue)
            : base(actualValue)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that an <typeparamref name="TAssertions"/> is in the expectation state.
    /// </summary>
    public class SelfReferencingCollectionAssertions<TCollection, T, TAssertions> :
        CollectionAssertions<TCollection, TAssertions>
        where TCollection : IEnumerable<T>
        where TAssertions : SelfReferencingCollectionAssertions<TCollection, T, TAssertions>
    {
        public SelfReferencingCollectionAssertions(TCollection actualValue)
            : base(actualValue)
        {
        }

        /// <summary>
        /// Asserts that the number of items in the collection matches the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The expected number of items in the collection.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCount(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to contain {1} item(s){reason}, but found {2}.", Subject, expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection matches a condition stated by the <paramref name="countPredicate"/>.
        /// </summary>
        /// <param name="countPredicate">A predicate that yields the number of items that is expected to be in the collection.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(countPredicate, nameof(countPredicate), "Cannot compare collection count against a <null> predicate.");

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0} items{reason}, but found {1}.", countPredicate.Body, Subject);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject.Count();

            if (!compiledPredicate(actualCount))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to have a count {1}{reason}, but count is {2}.",
                        Subject, countPredicate.Body, actualCount);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection does not match the supplied <paramref name="unexpected" /> amount.
        /// </summary>
        /// <param name="unexpected">The unexpected number of items in the collection.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveCount(int unexpected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to not contain {0} item(s){reason}, but found <null>.", unexpected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to not contain {0} item(s){reason}, but found {1}.", unexpected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection is greater than the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountGreaterThan(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain more than {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount > expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain more than {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection is greater or equal to the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountGreaterOrEqualTo(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain at least {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount >= expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain at least {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection is less than the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountLessThan(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain fewer than {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount < expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain fewer than {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection is less or equal to the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountLessOrEqualTo(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain at most {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount <= expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain at most {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="elements" />. Elements are compared using their <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<TAssertions> Equal(params T[] elements)
        {
            Func<T, T, bool> comparer = GetComparer();

            AssertSubjectEquality(elements, comparer, string.Empty);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static Func<T, T, bool> GetComparer()
        {
            if (typeof(T).IsValueType)
            {
                return (T s, T e) => s.Equals(e);
            }

            return (T s, T e) => Equals(s, e);
        }

        /// <summary>
        /// Asserts that two collections contain the same items in the same order, where equality is determined using a
        /// <paramref name="equalityComparison"/>.
        /// </summary>
        /// <param name="expectation">
        /// The collection to compare the subject with.
        /// </param>
        /// <param name="equalityComparison">
        /// A equality comparison the is used to determine whether two objects should be treated as equal.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Equal<TExpected>(
            IEnumerable<TExpected> expectation, Func<T, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            AssertSubjectEquality(expectation, equalityComparison, because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection starts with same elements in the same order as the collection identified by
        /// <paramref name="expectation" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expectation">
        /// A collection of expected elements.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> StartWith(IEnumerable<T> expectation, string because = "", params object[] becauseArgs)
        {
            if (expectation is null)
            {
                return base.StartWith(null, because, becauseArgs);
            }

            AssertCollectionStartsWith(Subject, expectation.ConvertOrCastToCollection(), EqualityComparer<T>.Default.Equals, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection starts with same elements in the same order as the collection identified by
        /// <paramref name="expectation" />. Elements are compared using <paramref name="equalityComparison"/>.
        /// </summary>
        /// <param name="expectation">
        /// A collection of expected elements.
        /// </param>
        /// <param name="equalityComparison">
        /// A equality comparison the is used to determine whether two objects should be treated as equal.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> StartWith<TExpected>(
            IEnumerable<TExpected> expectation, Func<T, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

            AssertCollectionStartsWith(Subject, expectation.ConvertOrCastToCollection(), equalityComparison, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection ends with same elements in the same order as the collection identified by
        /// <paramref name="expectation" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expectation">
        /// A collection of expected elements.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> EndWith(IEnumerable<T> expectation, string because = "", params object[] becauseArgs)
        {
            if (expectation is null)
            {
                return base.EndWith(null, because, becauseArgs);
            }

            AssertCollectionEndsWith(Subject, expectation.ConvertOrCastToCollection(), EqualityComparer<T>.Default.Equals, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection ends with same elements in the same order as the collection identified by
        /// <paramref name="expectation" />. Elements are compared using <paramref name="equalityComparison"/>.
        /// </summary>
        /// <param name="expectation">
        /// A collection of expected elements.
        /// </param>
        /// <param name="equalityComparison">
        /// A equality comparison the is used to determine whether two objects should be treated as equal.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> EndWith<TExpected>(
            IEnumerable<TExpected> expectation, Func<T, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

            AssertCollectionEndsWith(Subject, expectation.ConvertOrCastToCollection(), equalityComparison, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection contains the specified item.
        /// </summary>
        /// <param name="expected">The expectation item.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndWhichConstraint<TAssertions, T> Contain(T expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0}{reason}, but found {1}.", expected, Subject);
            }

            var collection = Subject.ConvertOrCastToCollection();
            if (!collection.Contains(expected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", collection, expected);
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this,
                collection.Where(
                    item => EqualityComparer<T>.Default.Equals(item, expected)));
        }

        /// <summary>
        /// Asserts that the collection contains some extra items in addition to the original items.
        /// </summary>
        /// <param name="expectedItemsList">An <see cref="IEnumerable{T}"/> of expectation items.</param>
        /// <param name="additionalExpectedItems">Additional items that are expectation to be contained by the collection.</param>
        public AndConstraint<TAssertions> Contain(IEnumerable<T> expectedItemsList,
            params T[] additionalExpectedItems)
        {
            var list = new List<T>(expectedItemsList);
            list.AddRange(additionalExpectedItems);

            return Contain((IEnumerable)list);
        }

        /// <summary>
        /// Asserts that the collection contains at least one item that matches the predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndWhichConstraint<TAssertions, T> Contain(Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0}{reason}, but found {1}.", predicate.Body, Subject);
            }

            Func<T, bool> func = predicate.Compile();

            Execute.Assertion
                .ForCondition(Subject.Any(func))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to have an item matching {1}{reason}.", Subject, predicate.Body);

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, Subject.Where(func));
        }

        /// <summary>
        /// Asserts that the collection only contains items that match a predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> OnlyContain(
            Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            Func<T, bool> compiledPredicate = predicate.Compile();

            var collection = Subject.ConvertOrCastToCollection();
            Execute.Assertion
                .ForCondition(collection.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain only items matching {0}{reason}, but the collection is empty.",
                    predicate.Body);

            IEnumerable<T> mismatchingItems = collection.Where(item => !compiledPredicate(item));
            if (mismatchingItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain only items matching {0}{reason}, but {1} do(es) not match.",
                        predicate.Body, mismatchingItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection does not contain the supplied <paramref name="unexpected" /> item.
        /// </summary>
        /// <param name="unexpected">The element that is not expected to be in the collection</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndWhichConstraint<TAssertions, T> NotContain(T unexpected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to not contain {0}{reason}, but found <null>.", unexpected);
            }

            var collection = Subject.ConvertOrCastToCollection();
            if (collection.Contains(unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to not contain {1}{reason}.", collection, unexpected);
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this,
                collection.Where(
                    item => !EqualityComparer<T>.Default.Equals(item, unexpected)));
        }

        /// <summary>
        /// Asserts that the collection does not contain some extra items in addition to the original items.
        /// </summary>
        /// <param name="unexpectedItemsList">An <see cref="IEnumerable{T}"/> of unexpected items.</param>
        /// <param name="additionalUnexpectedItems">Additional items that are not expected to be contained by the collection.</param>
        public AndConstraint<TAssertions> NotContain(IEnumerable<T> unexpectedItemsList, params T[] additionalUnexpectedItems)
        {
            var list = new List<T>(unexpectedItemsList);
            list.AddRange(additionalUnexpectedItems);
            return NotContain((IEnumerable)list);
        }

        /// <summary>
        /// Asserts that the collection does not contain any items that match the predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> NotContain(Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} not to contain {0}{reason}, but found {1}.", predicate.Body, Subject);
            }

            Func<T, bool> compiledPredicate = predicate.Compile();
            IEnumerable<T> unexpectedItems = Subject.Where(item => compiledPredicate(item));

            if (unexpectedItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to not have any items matching {1}{reason}, but found {2}.",
                        Subject, predicate.Body, unexpectedItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain only a single item.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, T> ContainSingle(string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain a single item{reason}, but found <null>.");
            }

            switch (Subject.Count())
            {
                case 0: // Fail, Collection is empty
                    Execute.Assertion.BecauseOf(because, becauseArgs).FailWith("Expected {context:collection} to contain a single item{reason}, but the collection is empty.");
                    break;
                case 1: // Success Condition
                    break;
                default: // Fail, Collection contains more than a single item
                    Execute.Assertion.BecauseOf(because, becauseArgs).FailWith("Expected {context:collection} to contain a single item{reason}, but found {0}.", Subject);
                    break;
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, Subject.SingleOrDefault());
        }

        /// <summary>
        /// Expects the current collection to contain only a single item matching the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate that will be used to find the matching items.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, T> ContainSingle(Expression<Func<T, bool>> predicate,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            string expectationPrefix =
                "Expected {context:collection} to contain a single item matching {0}{reason}, ";

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectationPrefix + "but found {1}.", predicate.Body, Subject);
            }

            ICollection<T> actualItems = Subject.ConvertOrCastToCollection();
            Execute.Assertion
                .ForCondition(actualItems.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(expectationPrefix + "but the collection is empty.", predicate.Body);

            T[] matchingElements = actualItems.Where(predicate.Compile()).ToArray();
            int count = matchingElements.Length;
            if (count == 0)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectationPrefix + "but no such item was found.", predicate.Body);
            }
            else if (count > 1)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectationPrefix + "but " + count.ToString(CultureInfo.InvariantCulture) + " such items were found.", predicate.Body);
            }
            else
            {
                // Exactly 1 item was expected
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matchingElements);
        }

        /// <summary>
        /// Asserts that a collection contains exactly a given number of elements, which meet
        /// the criteria provided by the element inspectors.
        /// </summary>
        /// <param name="elementInspectors">
        /// The element inspectors, which inspect each element in turn. The
        /// total number of element inspectors must exactly match the number of elements in the collection.
        /// </param>
        public AndConstraint<TAssertions> SatisfyRespectively(params Action<T>[] elementInspectors)
        {
            return SatisfyRespectively(elementInspectors, string.Empty);
        }

        /// <summary>
        /// Asserts that a collection contains exactly a given number of elements, which meet
        /// the criteria provided by the element inspectors.
        /// </summary>
        /// <param name="expected">
        /// The element inspectors, which inspect each element in turn. The
        /// total number of element inspectors must exactly match the number of elements in the collection.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> SatisfyRespectively(IEnumerable<Action<T>> expected, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify against a <null> collection of inspectors");

            ICollection<Action<T>> elementInspectors = expected.ConvertOrCastToCollection();
            if (!elementInspectors.Any())
            {
                throw new ArgumentException("Cannot verify against an empty collection of inspectors", nameof(expected));
            }

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to satisfy all inspectors{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but collection is <null>.")
                .Then
                .ForCondition(Subject.Any())
                .FailWith("but collection is empty.")
                .Then
                .ClearExpectation();

            int elementsCount = Subject.Count();
            int inspectorsCount = elementInspectors.Count;
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(elementsCount == inspectorsCount)
                .FailWith("Expected {context:collection} to contain exactly {0} items{reason}, but it contains {1} items",
                    inspectorsCount, elementsCount);

            string[] failuresFromInspectors;

            using (CallerIdentifier.OverrideStackSearchUsingCurrentScope())
            {
                failuresFromInspectors = CollectFailuresFromInspectors(elementInspectors);
            }

            if (failuresFromInspectors.Any())
            {
                string failureMessage = Environment.NewLine
                    + string.Join(Environment.NewLine, failuresFromInspectors.Select(x => x.IndentLines()));

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to satisfy all inspectors{reason}, but some inspectors are not satisfied:")
                    .FailWithPreFormatted(failureMessage)
                    .Then
                    .ClearExpectation();
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a collection contains exactly a given number of elements which meet
        /// the criteria provided by the element predicates. Assertion fails if it is not possible
        /// to find a one to one mapping between the elements of the collection and the predicates.
        /// The order of the predicates does not need to match the order of the elements.
        /// </summary>
        /// <param name="predicates">
        /// The predicates that the elements of the collection must match.
        /// The total number of predicates must exactly match the number of elements in the collection.
        /// </param>
        public AndConstraint<TAssertions> Satisfy(params Expression<Func<T, bool>>[] predicates)
        {
            return Satisfy(predicates, because: string.Empty);
        }

        /// <summary>
        /// Asserts that a collection contains exactly a given number of elements which meet
        /// the criteria provided by the element predicates. Assertion fails if it is not possible
        /// to find a one to one mapping between the elements of the collection and the predicates.
        /// </summary>
        /// <param name="predicates">
        /// The predicates that the elements of the collection must match.
        /// The total number of predicates must exactly match the number of elements in the collection.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> Satisfy(IEnumerable<Expression<Func<T, bool>>> predicates, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicates, nameof(predicates), "Cannot verify against a <null> collection of predicates");

            var predicatesList = predicates.ConvertOrCastToList();
            if (predicatesList.Count == 0)
            {
                throw new ArgumentException("Cannot verify against an empty collection of predicates", nameof(predicates));
            }

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to satisfy all predicates{reason}, ")
                .ForCondition(Subject is not null)
                .FailWith("but collection is <null>.")
                .Then
                .ForCondition(Subject.Any())
                .FailWith("but collection is empty.")
                .Then
                .ClearExpectation();

            var elements = Subject.ConvertOrCastToList();
            var maximumMatchingSolution = new MaximumMatchingProblem<T>(predicatesList, elements).Solve();

            if (maximumMatchingSolution.UnmatchedPredicatesExist || maximumMatchingSolution.UnmatchedElementsExist)
            {
                string message = string.Empty;
                var doubleNewLine = Environment.NewLine + Environment.NewLine;

                var notMatchedPredicates = maximumMatchingSolution.GetUnmatchedPredicates();
                if (notMatchedPredicates.Any())
                {
                    message += doubleNewLine + "The following predicates did not have matching elements:";
                    message += doubleNewLine + string.Join(Environment.NewLine, notMatchedPredicates.Select(predicate => Formatter.ToString(predicate)));
                }

                var notMatchedElementIndices = maximumMatchingSolution.GetUnmatchedElementIndices();
                if (notMatchedElementIndices.Any())
                {
                    message += doubleNewLine + "The following elements did not match any predicate:";

                    var elementDescriptions = notMatchedElementIndices.Select(index => $"Index: {index}, Element: {Formatter.ToString(elements[index])}");
                    message += doubleNewLine + string.Join(doubleNewLine, elementDescriptions);
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to satisfy all predicates{reason}, but:")
                    .FailWithPreFormatted(message);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private string[] CollectFailuresFromInspectors(IEnumerable<Action<T>> elementInspectors)
        {
            string[] collectionFailures;
            using (var collectionScope = new AssertionScope())
            {
                int index = 0;
                foreach ((T element, Action<T> inspector) in Subject.Zip(elementInspectors, (element, inspector) => (element, inspector)))
                {
                    string[] inspectorFailures;
                    using (var itemScope = new AssertionScope())
                    {
                        inspector(element);
                        inspectorFailures = itemScope.Discard();
                    }

                    if (inspectorFailures.Length > 0)
                    {
                        // Adding one tab and removing trailing dot to allow nested SatisfyRespectively
                        string failures = string.Join(Environment.NewLine, inspectorFailures.Select(x => x.IndentLines().TrimEnd('.')));
                        collectionScope.AddPreFormattedFailure($"At index {index}:{Environment.NewLine}{failures}");
                    }

                    index++;
                }

                collectionFailures = collectionScope.Discard();
            }

            return collectionFailures;
        }

        protected static IEnumerable<TExpectation> RepeatAsManyAs<TExpectation>(TExpectation value, IEnumerable<T> enumerable)
        {
            if (enumerable is null)
            {
                return Enumerable.Empty<TExpectation>();
            }

            return RepeatAsManyAsIterator(value, enumerable);
        }

        private static IEnumerable<TExpectation> RepeatAsManyAsIterator<TExpectation>(TExpectation value, IEnumerable<T> enumerable)
        {
            using IEnumerator<T> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return value;
            }
        }
    }
}
