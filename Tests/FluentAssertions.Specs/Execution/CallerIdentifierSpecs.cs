﻿using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Execution
{
    public class CallerIdentifierSpecs
    {
        [Fact]
        public void When_namespace_is_exactly_System_caller_should_be_unknown()
        {
            // Act
            Action act = () => System.SystemNamespaceClass.DetermineCallerIdentityInNamespace();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected function to be*");
        }

        [Fact]
        public void When_namespace_is_nested_under_System_caller_should_be_unknown()
        {
            // Act
            Action act = () => System.Data.NestedSystemNamespaceClass.DetermineCallerIdentityInNamespace();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected function to be*");
        }

        [Fact]
        public void When_namespace_is_prefixed_with_System_caller_should_be_known()
        {
            // Act
            Action act = () => SystemPrefixed.SystemPrefixedNamespaceClass.DetermineCallerIdentityInNamespace();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected actualCaller to be*");
        }

        [Fact]
        public void When_variable_name_contains_Should_it_should_identify_the_entire_variable_name_as_the_caller()
        {
            // Arrange
            string fooShould = "bar";

            // Act
            Action act = () => fooShould.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected fooShould to be <null>*");
        }
    }
}

namespace System
{
    public static class SystemNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}

namespace SystemPrefixed
{
    public static class SystemPrefixedNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}

namespace System.Data
{
    public static class NestedSystemNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}
