﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    
    public class MethodInfoAssertionSpecs
    {
        #region BeVirtual

        [Fact]
        public void When_asserting_a_method_is_virtual_and_it_is_then_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsVirtual).GetParameterlessMethod("PublicVirtualDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_virtual_but_it_is_not_then_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithNonVirtualPublicMethods).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeVirtual("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected method Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.PublicDoNothing" +
                    " to be virtual because we want to test the error message," +
                    " but it is not virtual.");
        }

        #endregion

        #region NotBeVirtual

        [Fact]
        public void When_asserting_a_method_is_not_virtual_and_it_is_not_then_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithNonVirtualPublicMethods).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_virtual_but_it_is_then_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsVirtual).GetParameterlessMethod("PublicVirtualDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeVirtual("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected method *ClassWithAllMethodsVirtual.PublicVirtualDoNothing" +
                    " not to be virtual because we want to test the error message," +
                    " but it is.");
        }

        #endregion

        #region BeDecoratedWithOfT

        [Fact]
        public void When_asserting_a_method_is_decorated_with_attribute_and_it_is_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_a_method_is_decorated_with_an_attribute_it_should_allow_chaining_assertions_on_it()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>().Which.Filter.Should().BeFalse();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_an_attribute_but_it_is_not_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing to be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_attribute_matching_a_predicate_and_it_is_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>(d => d.Filter);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_an_attribute_matching_a_predeicate_but_it_is_not_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>(d => !d.Filter, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing to be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_an_attribute_and_multiple_attributes_match_continuation_using_the_matched_value_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothingWithSameAttributeTwice");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    methodInfo.Should()
                        .BeDecoratedWith<DummyMethodAttribute>()
                        .Which.Filter.Should()
                        .BeTrue();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>();
        }

        #endregion


        #region NotBeDecoratedWithOfT

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_attribute_and_it_is_not_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_an_attribute_but_it_is_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothing to not be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was found.");
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_attribute_matching_a_predicate_and_it_is_not_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>(d => !d.Filter);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_an_attribute_matching_a_predeicate_but_it_is_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>(d => d.Filter, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothing to not be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was found.");
        }

        #endregion

        #region BeAsync

        [Fact]
        public void When_asserting_a_method_is_async_and_it_is_then_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsAsync).GetParameterlessMethod("PublicAsyncDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeAsync();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_async_but_it_is_not_then_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithNonAsyncMethods).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeAsync("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected method Task FluentAssertions.Specs.ClassWithNonAsyncMethods.PublicDoNothing" +
                    " to be async because we want to test the error message," +
                    " but it is not.");
        }

        #endregion

        #region NotBeAsync

        [Fact]
        public void When_asserting_a_method_is_not_async_and_it_is_not_then_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithNonAsyncMethods).GetParameterlessMethod("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeAsync();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_async_but_it_is_then_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsAsync).GetParameterlessMethod("PublicAsyncDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().NotBeAsync("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("*ClassWithAllMethodsAsync.PublicAsyncDoNothing*" +
                    "not to be async*because we want to test the error message*");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    internal class ClassWithAllMethodsVirtual
    {
        public virtual void PublicVirtualDoNothing()
        {
        }

        internal virtual void InternalVirtualDoNothing()
        {
        }

        protected virtual void ProtectedVirtualDoNothing()
        {
        }
    }

    internal interface IInterfaceWithPublicMethod
    {
        void PublicDoNothing();
    }

    internal class ClassWithNonVirtualPublicMethods : IInterfaceWithPublicMethod
    {
        public void PublicDoNothing()
        {
        }

        internal void InternalDoNothing()
        {
        }

        protected void ProtectedDoNothing()
        {
        }
    }

    internal class ClassWithAllMethodsDecoratedWithDummyAttribute
    {
        [DummyMethod(Filter = true)]
        public void PublicDoNothing()
        {
        }

        [DummyMethod(Filter = true)]
        [DummyMethod(Filter = false)]
        public void PublicDoNothingWithSameAttributeTwice()
        {
        }

        [DummyMethod]
        protected void ProtectedDoNothing()
        {
        }

        [DummyMethod]
        private void PrivateDoNothing()
        {
        }
    }

    internal class ClassWithMethodsThatAreNotDecoratedWithDummyAttribute
    {
        public void PublicDoNothing()
        {
        }

        protected void ProtectedDoNothing()
        {
        }

        private void PrivateDoNothing()
        {
        }
    }

    internal class ClassWithAllMethodsAsync
    {
        public async Task PublicAsyncDoNothing()
        {
            await Task.Factory.StartNew(() => { });
        }

        internal async Task InternalAsyncDoNothing()
        {
            await Task.Factory.StartNew(() => { });
        }

        protected async Task ProtectedAsyncDoNothing()
        {
            await Task.Factory.StartNew(() => { });
        }
    }

    internal class ClassWithNonAsyncMethods
    {
        public Task PublicDoNothing()
        {
            return null;
        }

        internal Task InternalDoNothing()
        {
            return null;
        }

        protected Task ProtectedDoNothing()
        {
            return null;
        }
    }

    #endregion
}
