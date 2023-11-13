﻿using System;
using FluentAssertions.Execution;
using Xunit;

namespace FluentAssertions.Specs.Specialized;

public class DelegateAssertionSpecs
{
    public class Throw
    {
        [Fact]
        public void Allow_additional_assertions_on_return_value()
        {
            // Arrange
            var exception = new Exception("foo");
            Action subject = () => throw exception;

            // Act / Assert
            subject.Should().Throw<Exception>()
                .Which.Message.Should().Be("foo");
        }
    }

    public class ThrowExactly
    {
        [Fact]
        public void Does_not_continue_assertion_on_exact_exception_type()
        {
            // Arrange
            var a = () => { };

            // Act
            using var scope = new AssertionScope();
            a.Should().ThrowExactly<InvalidOperationException>();

            // Assert
            scope.Discard().Should().ContainSingle()
                .Which.Should().Match("*InvalidOperationException*no exception*");
        }
    }
}
