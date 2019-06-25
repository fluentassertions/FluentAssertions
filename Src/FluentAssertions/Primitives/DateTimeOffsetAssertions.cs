﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="DateTimeOffset"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentAssertions.Extensions.FluentDateTimeExtensions"/>
    /// for a more fluent way of specifying a <see cref="DateTimeOffset"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class DateTimeOffsetAssertions
    {
        public DateTimeOffsetAssertions(DateTimeOffset? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public DateTimeOffset? Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> Be(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value == expected))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedDateToBeXFormat + Resources.Common_CommaButItWasYFormat,
                    expected, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBe(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || (Subject.Value != unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_DidNotExpectDateToBeXFormat + Resources.Common_CommaButItWas,
                    unexpected);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be(DateTimeOffset, string, object[])"/>.
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
        public AndConstraint<DateTimeOffsetAssertions> BeCloseTo(DateTimeOffset nearbyTime, int precision = 20,
            string because = "",
            params object[] becauseArgs)
        {
            return BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is within the specified time
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be(DateTimeOffset, string, object[])"/>.
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
        public AndConstraint<DateTimeOffsetAssertions> BeCloseTo(DateTimeOffset nearbyTime, TimeSpan precision,
            string because = "",
            params object[] becauseArgs)
        {
            long distanceToMinInTicks = (nearbyTime - DateTimeOffset.MinValue).Ticks;
            DateTimeOffset minimumValue = nearbyTime.AddTicks(-Math.Min(precision.Ticks, distanceToMinInTicks));

            long distanceToMaxInTicks = (long)(DateTimeOffset.MaxValue - nearbyTime).Ticks;
            DateTimeOffset maximumValue = nearbyTime.AddTicks(Math.Min(precision.Ticks, distanceToMaxInTicks));

            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value >= minimumValue) && (Subject.Value <= maximumValue))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedDateToBeWithinXFromYFormat + Resources.Common_CommaButItWasZFormat,
                    precision, nearbyTime, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe(DateTimeOffset, string, object[])"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values must differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeCloseTo(DateTimeOffset distantTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return NotBeCloseTo(distantTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not within the specified time
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe(DateTimeOffset, string, object[])"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of time which the two values must differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeCloseTo(DateTimeOffset distantTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            long distanceToMinInTicks = (distantTime - DateTimeOffset.MinValue).Ticks;
            DateTimeOffset minimumValue = distantTime.AddTicks(-Math.Min(precision.Ticks, distanceToMinInTicks));

            long distanceToMaxInTicks = (DateTimeOffset.MaxValue - distantTime).Ticks;
            DateTimeOffset maximumValue = distantTime.AddTicks(Math.Min(precision.Ticks, distanceToMaxInTicks));

            Execute.Assertion
                .ForCondition(Subject.HasValue && ((Subject.Value < minimumValue) || (Subject.Value > maximumValue)))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_DidNotExpectDateToBeWithinXFromYButItWasZFormat,
                    precision, distantTime, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeBefore(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedDateToBeBeforeXFormat + Resources.Common_CommaButItWasYFormat,
                    expected, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/>  is not before the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/>  that the current value is not expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeBefore(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeOnOrAfter(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is either on, or before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be on or before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrBefore(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedDateToBeOnOrBeforeXFormat + Resources.Common_CommaButItWasYFormat,
                    expected, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is neither on, nor before the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/> that the current value is not expected to be on nor before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeOnOrBefore(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeAfter(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeAfter(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedDateToBeAfterXFormat + Resources.Common_CommaButItWasYFormat,
                    expected, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not after the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/> that the current value is not expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeAfter(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeOnOrBefore(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is either on, or after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be on or after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrAfter(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedDateToBeOnOrAfterXFormat + Resources.Common_CommaButItWasYFormat,
                    expected, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/>  is neither on, nor after the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/>  that the current value is expected not to be on nor after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeOnOrAfter(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeBefore(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> year.
        /// </summary>
        /// <param name="expected">The expected year of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveYear(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheYearPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Year == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value.Year);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> year.
        /// </summary>
        /// <param name="unexpected">The year that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveYear(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheYearPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Year != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> month.
        /// </summary>
        /// <param name="expected">The expected month of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveMonth(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheMonthPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Month == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value.Month);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> month.
        /// </summary>
        /// <param name="unexpected">The month that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveMonth(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheMonthPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Month != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> day.
        /// </summary>
        /// <param name="expected">The expected day of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveDay(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheDayPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Day == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value.Day);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> day.
        /// </summary>
        /// <param name="unexpected">The day that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveDay(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheDayPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Day != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> hour.
        /// </summary>
        /// <param name="expected">The expected hour of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveHour(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheHourPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Hour == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value.Hour);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> hour.
        /// </summary>
        /// <param name="unexpected">The hour that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveHour(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheHourPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Hour != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> minute.
        /// </summary>
        /// <param name="expected">The expected minutes of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveMinute(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheMinutePartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Minute == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value.Minute);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> minute.
        /// </summary>
        /// <param name="unexpected">The minute that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveMinute(int unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheMinutePartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Minute != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> second.
        /// </summary>
        /// <param name="expected">The expected seconds of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveSecond(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheSecondsPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Second == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value.Second);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> second.
        /// </summary>
        /// <param name="unexpected">The second that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveSecond(int unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheSecondsPartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Second != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> offset.
        /// </summary>
        /// <param name="expected">The expected offset of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveOffset(TimeSpan expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheOffsetOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Offset == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value.Offset);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> offset.
        /// </summary>
        /// <param name="unexpected">The offset that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveOffset(TimeSpan unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheOffsetOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpected)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Offset != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should exceed compared to another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeMoreThan(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.MoreThan, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// is equal to or exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should be equal or exceed compared to
        /// another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeAtLeast(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.AtLeast, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// differs exactly the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should differ exactly compared to another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeExactly(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.Exactly, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// is within the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should be within another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeWithin(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.Within, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// differs at maximum the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The maximum amount of time that the current <see cref="DateTimeOffset"/> should differ compared to another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeLessThan(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.LessThan, timeSpan);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> date.
        /// </summary>
        /// <param name="expected">The expected date portion of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeSameDateAs(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            var expectedDate = expected.Date;

            Execute.Assertion
                .WithExpectation(Resources.DateTime_ExpectedTheDatePartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    expectedDate)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset, expectedDate)
                .Then
                .ForCondition(Subject.Value.Date == expectedDate)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWasXFormat, Subject.Value);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not the <paramref name="unexpected"/> date.
        /// </summary>
        /// <param name="unexpected">The date that is not to match the date portion of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeSameDateAs(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            var unexpectedDate = unexpected.Date;

            Execute.Assertion
                .WithExpectation(Resources.DateTime_DidNotExpectTheDatePartOfDateToBeXFormat + Resources.Common_CommaSpace,
                    unexpectedDate)
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ButFoundANullDateTimeOffset)
                .Then
                .ForCondition(Subject.Value.Date != unexpectedDate)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Common_ButItWas);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(params DateTimeOffset?[] validValues)
        {
            return BeOneOf(validValues, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(params DateTimeOffset[] validValues)
        {
            return BeOneOf(validValues.Cast<DateTimeOffset?>());
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(IEnumerable<DateTimeOffset> validValues, string because = "", params object[] becauseArgs)
        {
            return BeOneOf(validValues.Cast<DateTimeOffset?>(), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(IEnumerable<DateTimeOffset?> validValues, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(validValues.Contains(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.DateTime_ExpectedDateToBeOneOfXFormat + Resources.Common_CommaButItWasYFormat,
                    validValues, Subject);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }
    }
}
