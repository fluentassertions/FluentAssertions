﻿using System;
using Xunit;

namespace FluentAssertions.Specs
{
    public class FluentDateTimeSpecs
    {
        [Fact]
        public void When_fluently_specifying_a_date_in_january_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.January(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 1, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_february_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.February(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 2, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_march_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.March(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 3, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_april_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.April(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 4, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_may_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.May(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 5, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_june_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.June(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 6, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_july_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.July(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 7, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_august_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.August(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 8, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_september_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.September(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 9, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_october_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.October(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 10, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_november_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.November(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 11, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_in_december_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime date = 10.December(2011);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().Be(new DateTime(2011, 12, 10));
        }

        [Fact]
        public void When_fluently_specifying_a_date_and_time_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTime = 10.December(2011).At(09, 30, 45, 999);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45, 999));
            dateTime.Should().BeIn(DateTimeKind.Unspecified);
        }

        [Fact]
        public void When_fluently_specifying_a_date_and_time_as_utc_it_should_return_the_date_time_value_with_utc_kind()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTime = 10.December(2011).At(09, 30, 45, 999).AsUtc();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45, 999));
            dateTime.Should().BeIn(DateTimeKind.Utc);
        }

        [Fact]
        public void When_fluently_specifying_a_date_and_time_as_local_it_should_return_the_date_time_value_with_local_kind()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTime = 10.December(2011).At(09, 30, 45, 999).AsLocal();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45, 999));
            dateTime.Should().BeIn(DateTimeKind.Local);
        }

        [Fact]
        public void When_fluently_specifying_a_date_and_timespan_it_should_return_the_correct_date_time_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var time = 9.Hours().And(30.Minutes()).And(45.Seconds());
            DateTime dateTime = 10.December(2011).At(time);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45));
        }
    }
}