﻿using System;
using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs
{
    public class DateTimeOffsetValueFormatterSpecs
    {
        [Fact]
        public void When_time_is_not_relevant_it_should_not_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = formatter.Format(new DateTime(1973, 9, 20), new FormattingContext(), null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<1973-09-20>");
        }

        [Fact]
        public void When_the_offset_is_not_relevant_it_should_not_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = formatter.Format(new DateTimeOffset(1973, 9, 20, 12, 59, 59, 0.Hours()), new FormattingContext(),
                null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<1973-09-20 12:59:59>");
        }

        [Fact]
        public void When_the_offset_is_negative_it_should_include_it_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = Formatter.ToString(new DateTimeOffset(1973, 9, 20, 12, 59, 59, -3.Hours()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<1973-09-20 12:59:59 -3h>");
        }

        [Fact]
        public void When_the_offset_is_positive_it_should_include_it_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = Formatter.ToString(new DateTimeOffset(1973, 9, 20, 12, 59, 59, 3.Hours()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<1973-09-20 12:59:59 +3h>");
        }

        [Fact]
        public void When_date_is_not_relevant_it_should_not_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime emptyDate = 1.January(0001);
            var dateTime = emptyDate.At(08, 20, 01);
            string result = formatter.Format(dateTime, new FormattingContext(), null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<08:20:01>");
        }

        [Fact]
        public void When_a_full_date_and_time_is_specified_all_parts_should_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var dateTime = 1.May(2012).At(20, 15, 30, 318);
            string result = formatter.Format(dateTime, new FormattingContext(), null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(dateTime.ToString("<yyyy-MM-dd HH:mm:ss.fff>"));
        }

        [Fact]
        public void When_milliseconds_are_not_relevant_they_should_not_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var dateTime = 1.May(2012).At(20, 15, 30);
            string result = formatter.Format(dateTime, new FormattingContext(), null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<2012-05-01 20:15:30>");
        }

        [Fact]
        public void
            When_a_DateTime_is_used_it_should_format_the_same_as_a_DateTimeOffset()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            var dateOnly = ToUtcWithoutChangingTime(new DateTime(1973, 9, 20));
            var timeOnly = ToUtcWithoutChangingTime(1.January(0001).At(08, 20, 01));
            var witoutMilliseconds = ToUtcWithoutChangingTime(1.May(2012).At(20, 15, 30));
            var withMilliseconds = ToUtcWithoutChangingTime(1.May(2012).At(20, 15, 30, 318));

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            formatter.Format(dateOnly, new FormattingContext(), null)
                .Should().Be(formatter.Format((DateTimeOffset)dateOnly, new FormattingContext(), null));

            formatter.Format(timeOnly, new FormattingContext(), null).Should()
                .Be(formatter.Format((DateTimeOffset)timeOnly, new FormattingContext(), null));

            formatter.Format(witoutMilliseconds, new FormattingContext(), null).Should()
                .Be(formatter.Format((DateTimeOffset)witoutMilliseconds, new FormattingContext(), null));

            formatter.Format(withMilliseconds, new FormattingContext(), null).Should()
                .Be(formatter.Format((DateTimeOffset)withMilliseconds, new FormattingContext(), null));
        }

        private static DateTime ToUtcWithoutChangingTime(DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }
    }
}