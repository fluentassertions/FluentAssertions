﻿using System;
using System.Reflection;
using AssemblyA;
using AssemblyB;
using FluentAssertions.Specs.Types;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized;

public class AssemblyAssertionSpecs
{
    public class NotReference
    {
        [Fact]
        public void When_an_assembly_is_not_referenced_and_should_not_reference_is_asserted_it_should_succeed()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyB.Should().NotReference(assemblyA);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_not_referenced_it_should_allow_chaining()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyB.Should().NotReference(assemblyA)
                .And.NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_referenced_and_should_not_reference_is_asserted_it_should_fail()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().NotReference(assemblyB);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_subject_is_null_not_reference_should_fail()
        {
            // Arrange
            Assembly assemblyA = null;
            Assembly assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().NotReference(assemblyB, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected assembly not to reference assembly \"AssemblyB\" *failure message*, but assemblyA is <null>.");
        }

        [Fact]
        public void When_an_assembly_is_not_referencing_null_it_should_throw()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();

            // Act
            Action act = () => assemblyA.Should().NotReference(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("assembly");
        }
    }

    public class Reference
    {
        [Fact]
        public void When_an_assembly_is_referenced_and_should_reference_is_asserted_it_should_succeed()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().Reference(assemblyB);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_referenced_it_should_allow_chaining()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().Reference(assemblyB)
                .And.NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_not_referenced_and_should_reference_is_asserted_it_should_fail()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyB.Should().Reference(assemblyA);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_subject_is_null_reference_should_fail()
        {
            // Arrange
            Assembly assemblyA = null;
            Assembly assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().Reference(assemblyB, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected assembly to reference assembly \"AssemblyB\" *failure message*, but assemblyA is <null>.");
        }

        [Fact]
        public void When_an_assembly_is_referencing_null_it_should_throw()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();

            // Act
            Action act = () => assemblyA.Should().Reference(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("assembly");
        }
    }

    public class DefineType
    {
        [Fact]
        public void When_an_assembly_defines_a_type_and_Should_DefineType_is_asserted_it_should_succeed()
        {
            // Arrange
            var thisAssembly = GetType().Assembly;

            // Act
            Action act = () => thisAssembly
                .Should().DefineType(GetType().Namespace, typeof(WellKnownClassWithAttribute).Name)
                .Which.Should().BeDecoratedWith<DummyClassAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_an_assembly_does_not_define_a_type_and_Should_DefineType_is_asserted_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var thisAssembly = GetType().Assembly;

            // Act
            Action act = () => thisAssembly.Should().DefineType("FakeNamespace", "FakeName",
                "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected assembly \"{thisAssembly.FullName}\" " +
                    "to define type \"FakeNamespace\".\"FakeName\" " +
                    "because we want to test the failure message, but it does not.");
        }

        [Fact]
        public void When_subject_is_null_define_type_should_fail()
        {
            // Arrange
            Assembly thisAssembly = null;

            // Act
            Action act = () =>
                thisAssembly.Should().DefineType(GetType().Namespace, "WellKnownClassWithAttribute",
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected assembly to define type *.\"WellKnownClassWithAttribute\" *failure message*" +
                    ", but thisAssembly is <null>.");
        }

        [Fact]
        public void When_an_assembly_defining_a_type_with_a_null_name_it_should_throw()
        {
            // Arrange
            var thisAssembly = GetType().Assembly;

            // Act
            Action act = () => thisAssembly.Should().DefineType(GetType().Namespace, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_an_assembly_defining_a_type_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var thisAssembly = GetType().Assembly;

            // Act
            Action act = () => thisAssembly.Should().DefineType(GetType().Namespace, string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }

    public class BeNull
    {
        [Fact]
        public void When_an_assembly_is_null_and_Should_BeNull_is_asserted_it_should_succeed()
        {
            // Arrange
            Assembly thisAssembly = null;

            // Act
            Action act = () => thisAssembly
                .Should().BeNull();

            // Assert
            act.Should().NotThrow();
        }
    }

    public class BeUnsigned
    {
        [Fact]
        public void Guards_for_unsigned_assembly()
        {
            // Arrange
            var unsignedAssembly = FindAssembly.Stub();

            // Act & Assert
            unsignedAssembly.Should().BeUnsigned();
        }

        [Fact]
        public void Throws_for_signed_assembly()
        {
            // Arrange
            var signedAssembly = FindAssembly.Stub("0123456789ABCEF007");

            // Act
            Action act = () => signedAssembly.Should().BeUnsigned();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the assembly * to be signed, but it is.");
        }

        [Fact]
        public void Throws_for_null_subject()
        {
            // Arrange
            Assembly nullAssembly = null;

            // Act
            Action act = () => nullAssembly.Should().BeUnsigned();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected assembly to be unsigned, but nullAssembly is <null>.");
        }
    }

    public class BeSignedWithPublicKey
    {
        [Theory]
        [InlineData("0123456789ABCEF007")]
        [InlineData("0123456789abcef007")]
        [InlineData("0123456789ABcef007")]
        public void Guards_for_signed_assembly_with_expected_public_key(string publicKey)
        {
            // Arrange
            var signedAssembly = FindAssembly.Stub("0123456789ABCEF007");

            // Act & Assert
            signedAssembly.Should().BeSignedWithPublicKey(publicKey);
        }

        [Fact]
        public void Throws_for_unsigned_assembly()
        {
            // Arrange
            var unsignedAssembly = FindAssembly.Stub();

            // Act
            Action act = () => unsignedAssembly.Should().BeSignedWithPublicKey("1234");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected assembly * to have public key *, but it unsigned.");
        }

        [Fact]
        public void Throws_signed_assembly_with_different_public_key()
        {
            // Arrange
            var signedAssembly = FindAssembly.Stub("0123456789ABCEF007");

            // Act
            Action act = () => signedAssembly.Should().BeSignedWithPublicKey("1234");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected assembly * to have public key *, but it has * instead.");
        }

        [Fact]
        public void Throws_for_null_assembly()
        {
            // Arrange
            Assembly nullAssembly = null;

            // Act
            Action act = () => nullAssembly.Should().BeSignedWithPublicKey("1234");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected assembly to have public key *, but nullAssembly is <null>.");
        }
    }
}

[DummyClass("name", true)]
public class WellKnownClassWithAttribute
{
}
