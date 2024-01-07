﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The ContainSingle specs.
/// </content>
public partial class AsyncEnumerableAssertionSpecs
{
    [Fact]
    public void When_injecting_a_null_predicate_into_ContainSingle_it_should_throw()
    {
        // Arrange
        IEnumerable<int> array = [];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle(predicate: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void When_a_collection_contains_a_single_item_matching_a_predicate_it_should_succeed()
    {
        // Arrange
        IEnumerable<int> array = [1, 2, 3];
        var collection = array.ToAsyncEnumerable();
        Expression<Func<int, bool>> expression = item => item == 2;

        // Act
        Action act = () => collection.Should().ContainSingle(expression);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_an_empty_collection_contains_a_single_item_matching_a_predicate_it_should_throw()
    {
        // Arrange
        IEnumerable<int> array = [];
        var collection = array.ToAsyncEnumerable();
        Expression<Func<int, bool>> expression = item => item == 2;

        // Act
        Action act = () => collection.Should().ContainSingle(expression);

        // Assert
        string expectedMessage =
            "Expected collection to contain a single item matching (item == 2), but the collection is empty.";

        act.Should().Throw<XunitException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void When_asserting_a_null_collection_contains_a_single_item_matching_a_predicate_it_should_throw()
    {
        // Arrange
        const IAsyncEnumerable<int> collection = null;
        Expression<Func<int, bool>> expression = item => item == 2;

        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            collection.Should().ContainSingle(expression);
        };

        // Assert
        string expectedMessage =
            "Expected collection to contain a single item matching (item == 2), but found <null>.";

        act.Should().Throw<XunitException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void When_non_empty_collection_does_not_contain_a_single_item_matching_a_predicate_it_should_throw()
    {
        // Arrange
        IEnumerable<int> array = [1, 3];
        var collection = array.ToAsyncEnumerable();
        Expression<Func<int, bool>> expression = item => item == 2;

        // Act
        Action act = () => collection.Should().ContainSingle(expression);

        // Assert
        string expectedMessage =
            "Expected collection to contain a single item matching (item == 2), but no such item was found.";

        act.Should().Throw<XunitException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void When_non_empty_collection_contains_more_than_a_single_item_matching_a_predicate_it_should_throw()
    {
        // Arrange
        IEnumerable<int> array = [1, 2, 2, 2, 3];
        var collection = array.ToAsyncEnumerable();
        Expression<Func<int, bool>> expression = item => item == 2;

        // Act
        Action act = () => collection.Should().ContainSingle(expression);

        // Assert
        string expectedMessage =
            "Expected collection to contain a single item matching (item == 2), but 3 such items were found.";

        act.Should().Throw<XunitException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void When_single_item_matching_a_predicate_is_found_it_should_allow_continuation()
    {
        // Arrange
        IEnumerable<int> array = [1, 2, 3];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle(item => item == 2).Which.Should().BeGreaterThan(4);

        // Assert
        act.Should().Throw<XunitException>().WithMessage("Expected*greater*4*2*");
    }

    [Fact]
    public void When_single_item_contains_brackets_it_should_format_them_properly()
    {
        // Arrange
        IEnumerable<string> array = [""];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle(item => item == "{123}");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected collection to contain a single item matching (item == \"{123}\"), but no such item was found.");
    }

    [Fact]
    public void When_single_item_contains_string_interpolation_it_should_format_brackets_properly()
    {
        // Arrange
        IEnumerable<string> array = [""];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle(item => item == $"{123}");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected collection to contain a single item matching (item == \"123\"), but no such item was found.");
    }

    [Fact]
    public void When_a_collection_contains_a_single_item_it_should_succeed()
    {
        // Arrange
        IEnumerable<int> array = [1];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_an_empty_collection_contains_a_single_item_it_should_throw()
    {
        // Arrange
        IEnumerable<int> array = [];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle("more is not allowed");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected collection to contain a single item because more is not allowed, but the collection is empty.");
    }

    [Fact]
    public void When_asserting_a_null_collection_contains_a_single_item_it_should_throw()
    {
        // Arrange
        const IAsyncEnumerable<int> collection = null;

        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            collection.Should().ContainSingle("more is not allowed");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected collection to contain a single item because more is not allowed, but found <null>.");
    }

    [Fact]
    public void When_non_empty_collection_does_not_contain_a_single_item_it_should_throw()
    {
        // Arrange
        IEnumerable<int> array = [1, 3];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle();

        // Assert
        const string expectedMessage = "Expected collection to contain a single item, but found {1, 3}.";

        act.Should().Throw<XunitException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void When_non_empty_collection_contains_more_than_a_single_item_it_should_throw()
    {
        // Arrange
        IEnumerable<int> array = [1, 2];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle();

        // Assert
        const string expectedMessage = "Expected collection to contain a single item, but found {1, 2}.";

        act.Should().Throw<XunitException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void When_single_item_is_found_it_should_allow_continuation()
    {
        // Arrange
        IEnumerable<int> array = [3];
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle().Which.Should().BeGreaterThan(4);

        // Assert
        const string expectedMessage = "Expected collection to be greater than 4, but found 3.";

        act.Should().Throw<XunitException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void When_collection_is_IEnumerable_it_should_be_evaluated_only_once_with_predicate()
    {
        // Arrange
        IEnumerable<int> array = new OneTimeEnumerable<int>(1);
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle(_ => true);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_collection_is_IEnumerable_it_should_be_evaluated_only_once()
    {
        // Arrange
        IEnumerable<int> array = new OneTimeEnumerable<int>(1);
        var collection = array.ToAsyncEnumerable();

        // Act
        Action act = () => collection.Should().ContainSingle();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_an_assertion_fails_on_ContainSingle_succeeding_message_should_be_included()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            var values = new List<int>();
            values.ToAsyncEnumerable().Should().ContainSingle();
            values.ToAsyncEnumerable().Should().ContainSingle();
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected*to contain a single item, but the collection is empty*" +
                "Expected*to contain a single item, but the collection is empty*");
    }
}
