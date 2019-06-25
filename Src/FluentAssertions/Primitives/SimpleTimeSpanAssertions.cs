﻿using System;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="TimeSpan"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class SimpleTimeSpanAssertions
    {
        public SimpleTimeSpanAssertions(TimeSpan? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TimeSpan? Subject
        {
            get;
            private set;
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than zero.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BePositive(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBePositive + Resources.Common_CommaButFoundNull);

            Execute.Assertion
                .ForCondition(Subject.Value.CompareTo(TimeSpan.Zero) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBePositive + Resources.Common_CommaButFoundXFormat, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than zero.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeNegative(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeNegative + Resources.Common_CommaButFoundNull);

            Execute.Assertion
                .ForCondition(Subject.Value.CompareTo(TimeSpan.Zero) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeNegative + Resources.Common_CommaButFoundXFormat, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is equal to the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The expected time difference</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Object_ExpectedXFormat + Resources.Common_CommaButFoundNull, expected);

            Execute.Assertion
                .ForCondition(Subject.Value.CompareTo(expected) == 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Object_ExpectedXFormat + Resources.Common_CommaButFoundYFormat, expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is not equal to the
        /// specified <paramref name="unexpected"/> time.
        /// </summary>
        /// <param name="unexpected">The unexpected time difference</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || Subject.Value.CompareTo(unexpected) != 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Object_DidNotExpectXFormat, unexpected);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeLessThanXFormat + Resources.Common_CommaButFoundNull, expected);

            Execute.Assertion
                .ForCondition(Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeLessThanXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than or equal to the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeLessOrEqualToXFormat + Resources.Common_CommaButFoundNull, expected);

            Execute.Assertion
                .ForCondition(Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeLessOrEqualToXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeGreaterThanXFormat + Resources.Common_CommaButFoundNull, expected);

            Execute.Assertion
                .ForCondition(Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeGreaterThanXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than or equal to the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeGreaterOrEqualToXFormat + Resources.Common_CommaButFoundNull, expected);

            Execute.Assertion
                .ForCondition(Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeGreaterOrEqualToXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be"/>.
        /// </remarks>
        /// <param name="nearbyTime">
        /// The expected time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeCloseTo(TimeSpan nearbyTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is within the specified time
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be"/>.
        /// </remarks>
        /// <param name="nearbyTime">
        /// The expected time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of time which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeCloseTo(TimeSpan nearbyTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            var minimumValue = nearbyTime - precision;
            var maximumValue = nearbyTime + precision;

            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value >= minimumValue) && (Subject.Value <= maximumValue))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToBeWithinXFromYFormat + Resources.Common_CommaButFoundZFormat,
                    precision, nearbyTime, Subject ?? default(TimeSpan?));

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is not within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBeCloseTo(TimeSpan distantTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return NotBeCloseTo(distantTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is not within the specified time
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of time which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBeCloseTo(TimeSpan distantTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            var minimumValue = distantTime - precision;
            var maximumValue = distantTime + precision;

            Execute.Assertion
                .ForCondition(Subject.HasValue && !((Subject.Value >= minimumValue) && (Subject.Value <= maximumValue)))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedTimeToNotBeWithinXFromYFormat + Resources.Common_CommaButFoundZFormat,
                    precision, distantTime, Subject ?? default(TimeSpan?));

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }
    }
}
