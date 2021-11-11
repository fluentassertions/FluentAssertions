﻿using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives
{
    public class HttpResponseMessageAssertionSpecs
    {
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Accepted)]
        public void Should_succeed_when_asserting_statuscode_is_successful(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().BeSuccessful();
        }

        [Theory]
        [InlineData(HttpStatusCode.Moved)]
        public void Should_succeed_when_asserting_statuscode_is_redirect(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().BeRedirection();
        }

        [Theory]
        [InlineData(HttpStatusCode.Gone)]
        [InlineData(HttpStatusCode.BadRequest)]
        public void Should_succeed_when_asserting_statuscode_is_client_error(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().HaveClientError();
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void Should_succeed_when_asserting_statuscode_is_server_error(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().HaveServerError();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void Should_succeed_when_asserting_statuscode_is_error(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().HaveError();
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_error_is_successful()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.Gone);

            // Act
            Action action = () => testee.Should().BeSuccessful();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_error_is_successful()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.Gone).Should().BeSuccessful("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be successful (2xx) because we want to test the failure message, but found HttpStatusCode.Gone {value: 410}.");
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_error_is_redirection()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.Gone);

            // Act
            Action action = () => testee.Should().BeRedirection();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_error_is_redirection()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.Gone).Should().BeRedirection("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be redirection (3xx) because we want to test the failure message, but found HttpStatusCode.Gone {value: 410}.");
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_success_is_client_error()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action action = () => testee.Should().HaveClientError();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_success_is_client_error()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveClientError("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be client error (4xx) because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.");
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_success_is_server_error()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action action = () => testee.Should().HaveServerError();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_success_is_server_error()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveServerError("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be server error (5xx) because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.");
        }

        [Fact]
        public void Should_succeed_when_asserting_statuscode_to_be_equal_to_the_same_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.OK);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.Gone);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_value_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.Gone, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be HttpStatusCode.Gone {value: 410} because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.*");
        }

        [Fact]
        public void Should_succeed_when_asserting_statuscode_value_not_to_be_equal_to_the_same_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.Gone);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_value_not_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.OK);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_value_not_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.OK, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode not to be HttpStatusCode.OK {value: 200} because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.*");
        }
    }
}
