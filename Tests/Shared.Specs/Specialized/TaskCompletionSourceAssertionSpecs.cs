﻿using System;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class TaskCompletionSourceAssertionSpecs
    {
        [Fact]
        public void When_TCS_completes_in_time_it_should_succeed()
        {
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds());
            subject.SetResult(true);
            timer.Complete();

            action.Should().NotThrow();
        }

        [Fact]
        public void When_TCS_completes_in_time_and_result_is_expected_it_should_succeed()
        {
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            Func<Task> action = async() => (await subject.Should(timer).CompleteWithinAsync(1.Seconds())).Which.Should().Be(42);
            subject.SetResult(42);
            timer.Complete();

            action.Should().NotThrow();
        }

        [Fact]
        public void When_TCS_completes_in_time_and_result_is_not_expected_it_should_fail()
        {
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            Func<Task> action = async() => (await subject.Should(timer).CompleteWithinAsync(1.Seconds())).Which.Should().Be(42);
            subject.SetResult(99);
            timer.Complete();

            action.Should().Throw<XunitException>()
                .WithMessage("Expected * to be 42, but found 99.");
        }

        [Fact]
        public void When_TCS_did_not_complete_in_time_it_should_fail()
        {
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds(), "test {0}", "testArg");
            timer.Complete();

            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject to complete within 1s because test testArg.");
        }

        [Fact]
        public void When_TCS_completes_it_should_succeed()
        {
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            Func<Task> action = () => subject.Should(timer).CompleteAsync();
            subject.SetResult(true);
            timer.Complete();

            action.Should().NotThrow();
        }

        [Fact]
        public void When_TCS_completes_and_result_is_expected_it_should_succeed()
        {
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            Func<Task> action = async() => (await subject.Should(timer).CompleteAsync()).Which.Should().Be(42);
            subject.SetResult(42);
            timer.Complete();

            action.Should().NotThrow();
        }

        [Fact]
        public void When_TCS_completes_and_result_is_not_expected_it_should_fail()
        {
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            Func<Task> action = async() => (await subject.Should(timer).CompleteAsync()).Which.Should().Be(42);
            subject.SetResult(99);
            timer.Complete();

            action.Should().Throw<XunitException>()
                .WithMessage("Expected * to be 42, but found 99.");
        }

        [Fact]
        public void When_TCS_did_not_complete_it_should_fail()
        {
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            Func<Task> action = () => subject.Should(timer).CompleteAsync("test {0}", "testArg");
            timer.Complete();

            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject to complete because test testArg.");
        }
    }
}
