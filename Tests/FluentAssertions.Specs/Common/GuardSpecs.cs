﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using Xunit;

namespace FluentAssertions.Specs.Common
{
    public class GuardSpecs
    {
        [Fact]
        public void When_element_is_null_it_throws_with_empty_message()
        {
            // Arrange
            object o = null;

            // Act
            Action act = () => Guard.ThrowIfArgumentIsNull(o, nameof(o), "");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void When_element_is_null_it_throws_with_no_message()
        {
            // Arrange
            object o = null;

            // Act
            Action act = () => Guard.ThrowIfArgumentIsNull(o, nameof(o));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void When_element_is_not_null_it_do_not_throw_with_empty_message()
        {
            // Arrange
            object o = new object();

            // Act
            Action act = () => Guard.ThrowIfArgumentIsNull(o, nameof(o), "");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_element_is_not_null_it_do_not_throw_with_no_message()
        {
            // Arrange
            object o = new object();

            // Act
            Action act = () => Guard.ThrowIfArgumentIsNull(o, nameof(o));

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void When_argument_is_null_or_empty_it_throws_with_empty_message(string s)
        {
            // Act
            Action act = () => Guard.ThrowIfArgumentIsNullOrEmpty(s, nameof(s), "");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void When_argument_is_null_or_empty_it_throws_with_no_message(string s)
        {
            // Act
            Action act = () => Guard.ThrowIfArgumentIsNullOrEmpty(s, nameof(s));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("\n")]
        public void When_argument_is_not_null_or_empty_it_does_not_throw_with_empty_message(string s)
        {
            // Act
            Action act = () => Guard.ThrowIfArgumentIsNullOrEmpty(s, nameof(s), "");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("\n")]
        public void When_argument_is_not_null_or_empty_it_does_not_throw_with_no_message(string s)
        {
            // Act
            Action act = () => Guard.ThrowIfArgumentIsNullOrEmpty(s, nameof(s));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_any_of_the_elements_is_null_it_throws_with_no_message()
        {
            // Arrange
            object[] o = new object[] { new object(), null };

            // Act
            Action act = () => Guard.ThrowIfArgumentContainsNull(o, nameof(o));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void When_all_of_the_elements_are_not_null_it_does_not_throw_with_no_message()
        {
            // Arrange
            object[] o = new object[] { new object(), new object() };

            // Act
            Action act = () => Guard.ThrowIfArgumentContainsNull(o, nameof(o));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_enum_member_is_not_available_it_throws()
        {
            // Arrange
            var actual = (CSharpAccessModifier)1000;

            // Act
            Action act = () => Guard.ThrowIfArgumentIsOutOfRange<CSharpAccessModifier>(actual, nameof(actual));

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void When_enum_member_is_available_it_does_not_throw()
        {
            // Arrange
            var actual = (CSharpAccessModifier)1;

            // Act
            Action act = () => Guard.ThrowIfArgumentIsOutOfRange<CSharpAccessModifier>(actual, nameof(actual));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_has_no_elements_it_throws()
        {
            // Arrange
            var actual = new object[0];

            // Act
            Action act = () => Guard.ThrowIfArgumentIsEmpty(actual, nameof(actual), "");

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_collection_is_not_empty_it_does_not_throw()
        {
            // Arrange
            var actual = new object[] { new object() };

            // Act
            Action act = () => Guard.ThrowIfArgumentIsEmpty(actual, nameof(actual), "");

            // Assert
            act.Should().NotThrow();
        }
    }
}
