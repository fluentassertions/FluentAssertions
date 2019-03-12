﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class AsyncFunctionExceptionAssertionSpecs
    {
        [Fact]
        public void When_subject_throws_subclass_of_expected_exact_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentNullException>())
                .Should().ThrowExactly<ArgumentException>("because {0} should do that", "IFoo.Do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.ArgumentNullException.");
        }

        [Fact]
        public void When_subject_throws_the_expected_exact_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentNullException>())
                .Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void When_async_method_throws_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentException>())
                .Should().Throw<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public async void When_async_method_throws_async_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () => await asyncObject.ThrowAsync<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void When_async_method_does_not_throw_expected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.SucceedAsync())
                .Should().Throw<InvalidOperationException>("because {0} should do that", "IFoo.Do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected System.InvalidOperationException because IFoo.Do should do that, but no exception was thrown*");
        }

        [Fact]
        public void When_async_method_throws_unexpected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentException>())
                .Should().Throw<InvalidOperationException>("because {0} should do that", "IFoo.Do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected System.InvalidOperationException because IFoo.Do should do that, but found*System.ArgumentException*");
        }

        [Fact]
        public void When_async_method_does_not_throw_exception_and_that_was_expected_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.SucceedAsync())
                .Should().NotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public async Task When_async_method_does_not_throw_async_exception_and_that_was_expected_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () => await asyncObject.SucceedAsync();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_subject_throws_subclass_of_expected_async_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () => await asyncObject.ThrowAsync<ArgumentNullException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            await action.Should().ThrowAsync<ArgumentException>("because {0} should do that", "IFoo.Do");
        }

        [Fact]
        public void When_function_of_task_int_in_async_method_throws_the_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Func<Task<int>> f = () => asyncObject.ThrowTaskIntAsync<ArgumentNullException>(true);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task When_function_of_task_int_returns_value_result_can_be_asserted()
        {
            // Arrange/Act
            Func<Task<int>> func = () => Task.FromResult(42);

            // Assert
            func.Should().NotThrow().Which.Should().Be(42);
            (await func.Should().NotThrowAsync()).Which.Should().Be(42);
        }

        [Fact]
        public void When_function_of_task_int_in_async_method_throws_not_excepted_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Func<Task<int>> f = () => asyncObject.ThrowTaskIntAsync<InvalidOperationException>(true);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task When_action_throws_subclass_of_expected_async_exact_exception_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () => await asyncObject.ThrowAsync<ArgumentNullException>();
            Func<Task> testAction = async () => await action.Should().ThrowExactlyAsync<ArgumentException>("ABCDE");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            (await testAction.Should().ThrowAsync<XunitException>())
                .WithMessage("*ArgumentException*ABCDE*ArgumentNullException*");
        }

        [Fact]
        public async Task When_func_throws_subclass_of_expected_async_exact_exception_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> action = async () => await asyncObject.ThrowTaskIntAsync<ArgumentNullException>(true);
            Func<Task> testAction = async () => await action.Should().ThrowExactlyAsync<ArgumentException>("ABCDE");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            (await testAction.Should().ThrowAsync<XunitException>())
                .WithMessage("*ArgumentException*ABCDE*ArgumentNullException*");
        }

        [Fact]
        public async Task When_subject_throws_expected_async_exact_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () => await asyncObject.ThrowAsync<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            await action.Should().ThrowExactlyAsync<ArgumentException>("because {0} should do that", "IFoo.Do");
        }

        [Fact]
        public void When_async_method_throws_exception_and_no_exception_was_expected_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentException>())
                .Should().NotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect any exception, but found a System.ArgumentException*");
        }

        [Fact]
        public void When_async_method_throws_exception_and_expected_not_to_throw_another_one_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentException>())
                .Should().NotThrow<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public async Task When_async_method_throws_exception_and_expected_not_to_throw_async_another_one_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () => await asyncObject.ThrowAsync<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            await action.Should().NotThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public void When_async_method_succeeds_and_expected_not_to_throw_particular_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => asyncObject.SucceedAsync())
                .Should().NotThrow<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_async_method_throws_exception_expected_not_to_be_thrown_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentException>())
                .Should().NotThrow<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect System.ArgumentException, but found one*");
        }

        [Fact]
        public void When_async_method_throws_the_expected_inner_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> task = async () =>
            {
                await Task.Delay(100);
                throw new InvalidOperationException();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => task
                .Should().Throw<AggregateException>()
                .WithInnerException<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_async_method_throws_the_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> task = async () =>
            {
                await Task.Delay(100);
                throw new InvalidOperationException();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => task
                .Should().Throw<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_async_method_does_not_throw_the_expected_inner_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> task = async () =>
            {
                await Task.Delay(100);
                throw new ArgumentException();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => task
                .Should().Throw<AggregateException>()
                .WithInnerException<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage("*InvalidOperation*Argument*");
        }

        [Fact]
        public void When_async_method_does_not_throw_the_expected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> task = async () =>
            {
                await Task.Delay(100);
                throw new ArgumentException();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => task
                .Should().Throw<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage("*InvalidOperation*Argument*");
        }

        [Fact]
        public void When_asserting_async_void_method_should_throw_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncVoidMethod.Should().Throw<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<InvalidOperationException>("*async*void*");
        }

        [Fact]
        public void When_asserting_async_void_method_should_throw_exactly_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncVoidMethod.Should().ThrowExactly<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<InvalidOperationException>("*async*void*");
        }

        [Fact]
        public void When_asserting_async_void_method_should_not_throw_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncVoidMethod.Should().NotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<InvalidOperationException>("*async*void*");
        }

        [Fact]
        public void When_asserting_async_void_method_should_not_throw_specific_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncVoidMethod.Should().NotThrow<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<InvalidOperationException>("*async*void*");
        }

        #region NotThrowAfter

        [Fact]
        public void When_wait_time_is_negative_for_async_func_executed_with_wait_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var waitTime = -1.Milliseconds();
            var pollInterval = 10.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task> someFunc = async () => await asyncObject.SucceedAsync();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someFunc.Should().NotThrowAfter(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of waitTime must be non-negative*");
        }

        [Fact]
        public void When_poll_interval_is_negative_for_async_func_executed_with_wait_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var waitTime = 10.Milliseconds();
            var pollInterval = -1.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task> someFunc = async () => await asyncObject.SucceedAsync();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someFunc.Should().NotThrowAfter(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of pollInterval must be non-negative*");
        }

        [Fact]
        public void
            When_no_exception_should_be_thrown_for_async_func_executed_with_wait_after_wait_time_but_it_was_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var watch = Stopwatch.StartNew();
            var waitTime = 2.Seconds();
            var pollInterval = 10.Milliseconds();

            Func<Task> throwLongerThanWaitTime = async () =>
            {
                if (watch.ElapsedMilliseconds <= waitTime.TotalMilliseconds * 1.5)
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Delay(0);
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => throwLongerThanWaitTime.Should()
                .NotThrowAfter(waitTime, pollInterval, "we passed valid arguments");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect any exceptions after 2s because we passed valid arguments*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_for_async_func_executed_with_wait_after_wait_time_and_none_was_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var watch = Stopwatch.StartNew();
            var waitTime = 6.Seconds();
            var pollInterval = 10.Milliseconds();

            Func<Task> throwShorterThanWaitTime = async () =>
            {
                if (watch.ElapsedMilliseconds <= waitTime.TotalMilliseconds / 12)
                {
                    throw new ArgumentException("An exception was forced");
                }
                await Task.Delay(0);
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => throwShorterThanWaitTime.Should().NotThrowAfter(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        #endregion

        #region NotThrowAfterAsync
        [Fact]
        public void When_wait_time_is_negative_for_async_func_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var waitTime = -1.Milliseconds();
            var pollInterval = 10.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task> someFunc = async () => await asyncObject.SucceedAsync();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> act = async () =>
                await someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of waitTime must be non-negative*");
        }

        [Fact]
        public void When_poll_interval_is_negative_for_async_func_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var waitTime = 10.Milliseconds();
            var pollInterval = -1.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task> someFunc = async () => await asyncObject.SucceedAsync();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> act = async () =>
                await someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of pollInterval must be non-negative*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_for_async_func_after_wait_time_but_it_was_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var watch = Stopwatch.StartNew();
            var waitTime = 2.Seconds();
            var pollInterval = 10.Milliseconds();

            Func<Task> throwLongerThanWaitTime = async () =>
            {
                if (watch.ElapsedMilliseconds <= waitTime.TotalMilliseconds * 1.5)
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Delay(0);
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () =>
                await throwLongerThanWaitTime.Should()
                    .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect any exceptions after 2s because we passed valid arguments*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_for_async_func_after_wait_time_and_none_was_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var watch = Stopwatch.StartNew();
            var waitTime = 6.Seconds();
            var pollInterval = 10.Milliseconds();

            Func<Task> throwShorterThanWaitTime = async () =>
            {
                if (watch.ElapsedMilliseconds <= waitTime.TotalMilliseconds / 12)
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Delay(0);
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> act = async () =>
                await throwShorterThanWaitTime.Should().NotThrowAfterAsync(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        #endregion
    }

    internal class AsyncClass
    {
        public async Task ThrowAsync<TException>()
            where TException : Exception, new()
        {
            await Task.Factory.StartNew(() => throw new TException());
        }

        public async Task SucceedAsync()
        {
            await Task.FromResult(42);
        }

        public Task IncompleteTask()
        {
            return new TaskCompletionSource<bool>().Task;
        }

        public async Task<int> ThrowTaskIntAsync<TException>(bool throwException)
            where TException : Exception, new()
        {
            if (throwException)
            {
                throw new TException();
            }

            return await Task.FromResult(123);
        }
    }
}
