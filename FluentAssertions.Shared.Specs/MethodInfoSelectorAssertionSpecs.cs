﻿using System;
using FluentAssertions.Types;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class MethodInfoSelectorAssertionSpecs
    {
        [TestMethod]
        public void When_asserting_methods_are_virtual_and_they_are_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsVirtual));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodSelector.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_methods_are_virtual_but_non_virtual_methods_are_found_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonVirtualPublicMethods));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodSelector.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_methods_are_virtual_but_non_virtual_methods_are_found_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonVirtualPublicMethods));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodSelector.Should().BeVirtual("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected all selected methods" +
                             " to be virtual because we want to test the error message," +
                             " but the following methods are not virtual:\r\n" +
                             "Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.PublicDoNothing\r\n" +
                             "Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.InternalDoNothing\r\n" +
                             "Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.ProtectedDoNothing");
        }

        [TestMethod]
        public void When_asserting_methods_are_decorated_with_attribute_and_they_are_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsDecoratedWithDummyAttribute));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_methods_are_decorated_with_attribute_but_they_are_not_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfoSelector methodSelector =
                new MethodInfoSelector(typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute))
                    .ThatArePublicOrInternal;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_methods_are_decorated_with_attribute_but_they_are_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var methodSelector = new MethodInfoSelector(typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected all selected methods to be decorated with" +
                             " FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                             " but the following methods are not:\r\n" +
                             "Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing\r\n" +
                             "Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.ProtectedDoNothing\r\n" +
                             "Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PrivateDoNothing");
        }
    }
}