﻿using FluentAssertions.Formatting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class XmlElementAssertionSpecs
    {
        #region BeEquivalent

        [TestMethod]
        public void When_asserting_xml_element_is_equivalent_to_another_xml_element_with_same_contents_it_should_succeed()
        {
            // This test is basically just a check that the BeEquivalent method
            // is available on XmlElementAssertions, which it should be if
            // XmlElementAssertions inherits XmlNodeAssertions.

            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user>grega</user>");
            var element = xmlDoc.DocumentElement;
            var expectedDoc = new XmlDocument();
            expectedDoc.LoadXml(@"<user>grega</user>");
            var expected = expectedDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().BeEquivalentTo(expected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();

        }
        #endregion

        #region HaveValue

        [TestMethod]
        public void When_asserting_xml_element_has_a_specific_inner_text_and_it_does_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user>grega</user>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveInnerText("grega");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_a_specific_inner_text_but_it_has_a_different_inner_text_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user>grega</user>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveInnerText("stamac");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_a_specific_inner_text_but_it_has_a_different_inner_text_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user>grega</user>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveInnerText("stamac", "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected XML element \"user\" to have value \"stamac\"" +
                    " because we want to test the failure message" +
                        ", but found \"grega\".");
        }

        #endregion

        #region HaveAttribute

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_specific_value_and_it_does_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttribute("name", "martin");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_ns_and_specific_value_and_it_does_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("name", "http://www.example.com/2012/test", "martin");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_does_not_exist_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttribute("age", "36");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_does_not_exist_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("age", "http://www.example.com/2012/test", "36");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_does_not_exist_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttribute("age", "36", "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected XML element to have attribute \"age\" with value \"36\"" +
                    " because we want to test the failure message" +
                        ", but found no such attribute in <user name=\\\"martin\\\"*");
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_does_not_exist_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("age", "http://www.example.com/2012/test", "36", "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected XML element to have attribute \"{http://www.example.com/2012/test}age\" with value \"36\"" +
                    " because we want to test the failure message" +
                        ", but found no such attribute in <user xmlns:a=\\\"http:...");
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_has_different_value_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttribute("name", "dennis");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_has_different_value_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("name", "http://www.example.com/2012/test", "dennis");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_specific_value_but_attribute_has_different_value_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttribute("name", "dennis", "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected XML attribute \"name\" to have value \"dennis\"" +
                    " because we want to test the failure message" +
                        ", but found \"martin\".");
        }

        [TestMethod]
        public void When_asserting_xml_element_has_attribute_with_ns_and_specific_value_but_attribute_has_different_value_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<user xmlns:a=""http://www.example.com/2012/test"" a:name=""martin"" />");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveAttributeWithNamespace("name", "http://www.example.com/2012/test", "dennis", "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected XML attribute \"{http://www.example.com/2012/test}name\" to have value \"dennis\"" +
                    " because we want to test the failure message" +
                        ", but found \"martin\".");
        }

        #endregion

        #region HaveElement

        [TestMethod]
        public void When_asserting_xml_element_has_child_element_and_it_does_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xml = new XmlDocument();
            xml.LoadXml(
                @"<parent>
                    <child />
                  </parent>");
            var element = xml.DocumentElement;


            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveElement("child");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }


        [TestMethod]
        public void When_asserting_xml_element_has_child_element_with_ns_and_it_does_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                @"<parent xmlns:c='http://www.example.com/2012/test'>
                    <c:child />
                  </parent>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveElementWithNamespace("child", "http://www.example.com/2012/test");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_child_element_but_it_does_not_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                @"<parent>
                    <child />
                  </parent>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveElement("unknown");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_child_element_with_ns_but_it_does_not_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                @"<parent>
                    <child />
                  </parent>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveElementWithNamespace("unknown", "http://www.example.com/2012/test");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_xml_element_has_child_element_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                @"<parent>
                    <child />
                  </parent>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveElement("unknown", "because we want to test the failure message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            string expectedMessage = string.Format("Expected XML element {0} to have child element \"unknown\"" +
                " because we want to test the failure message" +
                    ", but no such child element was found.", Formatter.ToString(element));

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_asserting_xml_element_has_child_element_with_ns_but_it_does_not_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                @"<parent>
                    <child />
                  </parent>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                element.Should().HaveElementWithNamespace("unknown", "http://www.example.com/2012/test", "because we want to test the failure message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            string expectedMessage = string.Format("Expected XML element {0} to have child element \"{{http://www.example.com/2012/test}}unknown\"" +
                " because we want to test the failure message" +
                    ", but no such child element was found.", Formatter.ToString(element));

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }


        [TestMethod]
        public void When_asserting_xml_element_has_child_element_it_should_return_the_matched_element_in_the_which_property()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                @"<parent>
                    <child attr='1' />
                  </parent>");
            var element = xmlDoc.DocumentElement;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            var matchedElement = element.Should().HaveElement("child").Subject;

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            matchedElement.Should().BeOfType<XmlElement>()
                .And.HaveAttribute("attr", "1");
            matchedElement.Name.Should().Be("child");
        }

        #endregion
    }
}
