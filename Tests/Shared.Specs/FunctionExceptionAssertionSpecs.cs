﻿using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class FunctionExceptionAssertionSpecs
    {
        #region Throw
        [Fact]
        public void When_subject_throws_the_expected_exact_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_subject_throws_the_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_subject_does_not_throw_expected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<InvalidCastException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*but*ArgumentNullException*");
        }

        [Fact]
        public void When_subject_does_not_throw_any_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => 12;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<InvalidCastException>("that's what I {0}", "said");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*that's what I said*but*no exception*");
        }
        
        #endregion

        #region NotThrow
        [Fact]
        public void When_subject_does_not_throw_exception_and_that_was_expected_it_should_succeed_then_continue_assertion()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => 12;

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().NotThrow().Which.Should().Be(12);
        }

        [Fact]
        public void When_subject_throw_exception_and_that_was_not_expected_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow("that's what he {0}", "told me");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("*no*exception*that's what he told me*but*ArgumentNullException*");
        }

        #endregion

        #region NotThrow<T>
        [Fact]
        public void When_subject_does_not_throw_at_all_when_some_particular_exception_was_not_expected_it_should_succeed_but_then_cannot_continue_assertion()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => 12;

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().NotThrow<ArgumentException>();
            //.Which.Should().Be(12); <- this is invalid, because NotThrow<T> does not guarantee that no exception was thrown.
        }

        [Fact]
        public void When_subject_does_throw_exception_and_that_exception_was_not_expected_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new InvalidOperationException("custom message");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow<InvalidOperationException>("it was so {0}", "fast");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("*Did not expect System.InvalidOperationException because it was so fast, but found one with message*custom message*");
        }

        [Fact]
        public void When_subject_throw_one_exception_but_other_was_not_expected_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        #endregion
    }
}
