﻿using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    /// <content>
    /// The HaveCountLessOrEqualTo specs.
    /// </content>
    public partial class CollectionAssertionSpecs
    {
        #region Have Count Less Or Equal To

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_less_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountLessOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_less_or_equal_to_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountLessOrEqualTo(2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_less_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountLessOrEqualTo(2, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*at most*2*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_count_is_less_or_equal_to_and_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveCountLessOrEqualTo(1, "we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*at most*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion
    }
}
