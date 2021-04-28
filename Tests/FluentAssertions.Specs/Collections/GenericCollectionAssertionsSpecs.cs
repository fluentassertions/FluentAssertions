﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Specs.Equivalency;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    public class GenericCollectionAssertionsSpecs
    {
        [Fact]
        public void When_asserting_equality_with_a_collection_built_from_params_arguments_that_are_assignable_to_the_subjects_type_parameter_it_should_succeed_by_treating_the_arguments_as_of_that_type()
        {
            // Arrange
            byte[] byteArray = { 0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10 };

            // Act
            Action act = () => byteArray.Should().Equal(0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10);

            // Assert
            act.Should().NotThrow();
        }

        #region (Not) Contain

        [Fact]
        public void When_injecting_a_null_predicate_into_Contain_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new int[] { };

            // Act
            Action act = () => collection.Should().Contain(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void When_collection_does_not_contain_an_expected_item_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(item => item > 3, "at least {0} item should be larger than 3", 1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to have an item matching (item > 3) because at least 1 item should be larger than 3.");
        }

        [Fact]
        public void When_collection_does_contain_an_expected_item_matching_a_predicate_it_should_allow_chaining_it()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(item => item == 2).Which.Should().BeGreaterThan(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*greater*4*2*");
        }

        [Fact]
        public void When_collection_does_contain_an_expected_item_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().Contain(item => item == 2);
        }

        [Fact]
        public void When_a_collection_of_strings_contains_the_expected_string_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> strings = new[] { "string1", "string2", "string3" };

            // Act / Assert
            strings.Should().Contain("string2");
        }

        [Fact]
        public void When_a_collection_of_strings_does_not_contain_the_expected_string_it_should_throw()
        {
            // Arrange
            IEnumerable<string> strings = new[] { "string1", "string2", "string3" };

            // Act
            Action act = () => strings.Should().Contain("string4", "because {0} is required", "4");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings {\"string1\", \"string2\", \"string3\"} to contain \"string4\" because 4 is required.");
        }

        [Fact]
        public void When_asserting_collection_contains_some_values_but_collection_is_null_it_should_throw()
        {
            // Arrange
            const IEnumerable<string> strings = null;

            // Act
            Action act = () => strings.Should().Contain("string4", "because we're checking how it reacts to a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings to contain \"string4\" because we're checking how it reacts to a null subject, but found <null>.");
        }

        [Fact]
        public void When_the_multiple_matching_objects_exists_it_continuation_using_the_matched_value_should_fail()
        {
            // Arrange
            DateTime now = DateTime.Now;

            IEnumerable<DateTime> collection = new[] { now, DateTime.SpecifyKind(now, DateTimeKind.Unspecified) };

            // Act
            Action act = () => collection.Should().Contain(now).Which.Kind.Should().Be(DateTimeKind.Local);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_contains_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            // Arrange
            const IEnumerable<string> strings = null;

            // Act
            Action act = () => strings.Should().Contain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_doesnt_contain_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            // Arrange
            const IEnumerable<string> strings = null;

            // Act
            Action act =
                () => strings.Should().NotContain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings not to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_the_expected_item_it_should_not_be_enumerated_twice()
        {
            // Arrange
            var collection = new OneTimeEnumerable<int>(1, 2, 3);

            // Act
            Action act = () => collection.Should().Contain(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection*to contain 4.");
        }

        [Fact]
        public void When_a_collection_contains_the_unexpected_item_it_should_not_be_enumerated_twice()
        {
            // Arrange
            var collection = new OneTimeEnumerable<int>(1, 2, 3);

            // Act
            Action act = () => collection.Should().NotContain(2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection*to not contain 2.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_the_unexpected_items_it_should_not_be_enumerated_twice()
        {
            // Arrange
            var collection = new OneTimeEnumerable<int>(1, 2, 3);

            // Act
            Action act = () => collection.Should().OnlyContain(i => i > 3);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain only items matching*");
        }

        private class OneTimeEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> items;
            private int enumerations;

            public OneTimeEnumerable(params T[] items) => this.items = items;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<T> GetEnumerator()
            {
                if (enumerations++ > 0)
                {
                    throw new InvalidOperationException("OneTimeEnumerable can be enumerated one time only");
                }

                return items.GetEnumerator();
            }
        }

        #endregion

        #region Only Contain (Predicate)

        [Fact]
        public void When_injecting_a_null_predicate_into_OnlyContain_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new int[] { };

            // Act
            Action act = () => collection.Should().OnlyContain(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void When_a_collection_contains_items_not_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 2, 12, 3, 11, 2 };

            // Act
            Action act = () => collection.Should().OnlyContain(i => i <= 10, "10 is the maximum");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain only items matching (i <= 10) because 10 is the maximum, but {12, 11} do(es) not match.");
        }

        [Fact]
        public void When_a_collection_is_empty_and_should_contain_only_items_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<string> strings = Enumerable.Empty<string>();

            // Act
            Action act = () => strings.Should().OnlyContain(e => e.Length > 0);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected strings to contain only items matching (e.Length > 0), but the collection is empty.");
        }

        [Fact]
        public void When_a_collection_contains_only_items_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 2, 9, 3, 8, 2 };

            // Act
            Action act = () => collection.Should().OnlyContain(i => i <= 10);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Contain Single

        [Fact]
        public void When_injecting_a_null_predicate_into_ContainSingle_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new int[] { };

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
            IEnumerable<int> collection = new[] { 1, 2, 3 };
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
            IEnumerable<int> collection = Enumerable.Empty<int>();
            Expression<Func<int, bool>> expression = item => item == 2;

            // Act
            Action act = () => collection.Should().ContainSingle(expression);

            // Assert
            string expectedMessage =
                $"Expected collection to contain a single item matching {expression.Body}, but the collection is empty.";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_asserting_a_null_collection_contains_a_single_item_matching_a_predicate_it_should_throw()
        {
            // Arrange
            const IEnumerable<int> collection = null;
            Expression<Func<int, bool>> expression = item => item == 2;

            // Act
            Action act = () => collection.Should().ContainSingle(expression);

            // Assert
            string expectedMessage =
                $"Expected collection to contain a single item matching {expression.Body}, but found <null>.";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_non_empty_collection_does_not_contain_a_single_item_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 3 };
            Expression<Func<int, bool>> expression = item => item == 2;

            // Act
            Action act = () => collection.Should().ContainSingle(expression);

            // Assert
            string expectedMessage =
                $"Expected collection to contain a single item matching {expression.Body}, but no such item was found.";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_non_empty_collection_contains_more_than_a_single_item_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 2, 2, 3 };
            Expression<Func<int, bool>> expression = item => item == 2;

            // Act
            Action act = () => collection.Should().ContainSingle(expression);

            // Assert
            string expectedMessage =
                $"Expected collection to contain a single item matching {expression.Body}, but 3 such items were found.";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_single_item_matching_a_predicate_is_found_it_should_allow_continuation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().ContainSingle(item => item == 2).Which.Should().BeGreaterThan(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*greater*4*2*");
        }

        [Fact]
        public void When_single_item_contains_brackets_it_should_format_them_properly()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "" };

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
            IEnumerable<string> collection = new[] { "" };

            // Act
            Action act = () => collection.Should().ContainSingle(item => item == $"{123}");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain a single item matching (item == Format(\"{0}\", Convert(123*))), but no such item was found.");
        }

        #endregion

        #region Contain Single

        [Fact]
        public void When_a_collection_contains_a_single_item_it_should_succeed()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1 };

            // Act
            Action act = () => collection.Should().ContainSingle();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_empty_collection_contains_a_single_item_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = Enumerable.Empty<int>();

            // Act
            Action act = () => collection.Should().ContainSingle("more is not allowed");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain a single item because more is not allowed, but the collection is empty.");
        }

        [Fact]
        public void When_asserting_a_null_collection_contains_a_single_item_it_should_throw()
        {
            // Arrange
            const IEnumerable<int> collection = null;

            // Act
            Action act = () => collection.Should().ContainSingle("more is not allowed");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain a single item because more is not allowed, but found <null>.");
        }

        [Fact]
        public void When_non_empty_collection_does_not_contain_a_single_item_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 3 };

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
            IEnumerable<int> collection = new[] { 1, 2 };

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
            IEnumerable<int> collection = new[] { 3 };

            // Act
            Action act = () => collection.Should().ContainSingle().Which.Should().BeGreaterThan(4);

            // Assert
            const string expectedMessage = "Expected collection to be greater than 4, but found 3.";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        #endregion

        #region Be In Ascending/Descending Order

        #region Empty Collection - is always ordered, in both directions

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder("because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order because I say so, but found {empty}.");
        }

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder("because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order because I say so, but found {empty}.");
        }

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_to_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection {empty} to not be ordered \"by Number\" and not result in {empty}.");
        }

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_to_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection {empty} to not be ordered \"by Number\" and not result in {empty}.");
        }

        #endregion

        #region Single Element Collection - is always ordered, in both directions

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order, but found {42}.");
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order, but found {42}.");
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_to_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_to_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #region Multi Element Collection - No Parameter / Comparer

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 2, 3, 1 };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<int>.Default, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be in ascending order*should be sorted*2*3*1*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(Comparer<int>.Default, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be in descending order*should be sorted*1*2*3*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 3, 2, 1 };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_ordered_descending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 3, 2, 1 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(Comparer<int>.Default, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order*should not be sorted*3*2*1*");
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_a_ascending_ordered_collection_are_not_ordered_ascending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(Comparer<int>.Default, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order*should not be sorted*1*2*3*");
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 3, 2, 1 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Multi Element Collection - Property Expression

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*a*b*c*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*a*b*c*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Text, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*c*b*a*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*c*b*a*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_ordered_descending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "c", Numeric = 3 },
                new { Text = "b", Numeric = 1 },
                new { Text = "a", Numeric = 2 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Text, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*not be ordered*Text*should not be sorted*c*b*a*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ordered_collection_are_not_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "C", Numeric = 1 },
                new { Text = "b", Numeric = 2 },
                new { Text = "A", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*C*b*A*not be ordered*Text*should not be sorted*C*b*A*");
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_a_ascending_ordered_collection_are_not_ordered_ascending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "a", Numeric = 3 },
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Text, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*a*b*c*not be ordered*Text*should not be sorted*a*b*c*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ordered_collection_are_not_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "A", Numeric = 1 },
                new { Text = "b", Numeric = 2 },
                new { Text = "C", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*A*b*C*not be ordered*Text*should not be sorted*A*b*C*");
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Multi Element Collection - Using Lambda

        #region Be In Ascending Order

        [Fact]
        public void When_strings_are_in_ascending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_it_should_throw()
        {
            // Arrange
            string[] strings = { "theta", "alpha", "beta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*ascending*of reasons*index 0*");
        }

        [Fact]
        public void When_strings_are_in_ascending_order_according_to_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "thomas" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*ascending*of reasons*index 1*");
        }

        [Fact]
        public void When_strings_are_in_ascending_order_according_to_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "thomas" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*ascending*of reasons*index 1*");
        }

        #endregion

        #region Not Be In Ascending Order

        [Fact]
        public void When_strings_are_not_in_ascending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "beta", "alpha", "theta" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_in_ascending_order_unexpectedly_it_should_throw()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*ascending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_ascending_order_according_to_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "thomas", "roy" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*ascending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "thomas" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_ascending_order_according_to_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "barbara", "dennis", "roy" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*ascending*of reasons*but found*");
        }

        #endregion

        #region Be In Descending Order

        [Fact]
        public void When_strings_are_in_descending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "theta", "beta", "alpha" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_it_should_throw()
        {
            // Arrange
            string[] strings = { "theta", "alpha", "beta" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*descending*of reasons*index 1*");
        }

        [Fact]
        public void When_strings_are_in_descending_order_based_on_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*descending*of reasons*index 0*");
        }

        [Fact]
        public void When_strings_are_in_descending_order_based_on_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*descending*of reasons*index 0*");
        }

        #endregion

        #region Not Be In Descending Order

        [Fact]
        public void When_strings_are_not_in_descending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "beta", "theta", "alpha" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_descending_order_it_should_throw()
        {
            // Arrange
            string[] strings = { "theta", "beta", "alpha" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*descending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "barbara", "dennis" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_descending_order_based_on_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*descending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_descending_order_based_on_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*descending*of reasons*but found*");
        }

        private class ByLastCharacterComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return x.Last().CompareTo(y.Last());
            }
        }

        #endregion

        #endregion

        #region Null Collection

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_not_ordered_using_the_specified_property_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Text);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Text*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_not_ordered_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(Comparer<SomeClass>.Default);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_not_ordered_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Text*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_specified_property_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Text*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<SomeClass>.Default);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*Text*found*null*");
        }

        #endregion

        #region Null Parameter

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_ordered_and_the_specified_property_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder((Expression<Func<SomeClass, string>>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a property*")
                .WithParameterName("propertyExpression");
        }

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_ordered_and_the_given_comparer_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a comparer*")
                .WithParameterName("comparer");
        }

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_not_ordered_and_the_specified_property_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder((Expression<Func<SomeClass, string>>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a property*propertyExpression*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_not_ordered_and_the_given_comparer_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a comparer*comparer*");
        }

        #endregion

        #region Invalid Expression

        [Fact]
        public void When_asserting_the_items_in_ay_collection_are_ordered_using_an_invalid_property_expression_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.GetHashCode());

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Expression*o.GetHashCode()*cannot be used to select a member*");
        }

        [Fact]
        public void When_asserting_the_items_in_ay_collection_are_not_ordered_using_an_invalid_property_expression_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.GetHashCode());

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Expression*o.GetHashCode()*cannot be used to select a member*");
        }

        #endregion

        #region Then be in order

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_ascending_assertion_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_not_ordered_according_to_the_subsequent_ascending_assertion_it_should_fail()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .BeInAscendingOrder(x => x.Item2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection * to be ordered \"by Item2\"*");
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_ascending_assertion_with_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "B"),
                (2, "b"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_multiple_subsequent_ascending_assertions_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a", 1.1),
                (2, "b", 1.2),
                (2, "c", 1.3),
                (3, "a", 1.1)
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2)
                .And
                .ThenBeInAscendingOrder(x => x.Item3);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_descending_assertion_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_not_ordered_according_to_the_subsequent_descending_assertion_it_should_fail()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .BeInDescendingOrder(x => x.Item2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection * to be ordered \"by Item2\"*");
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_descending_assertion_with_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "b"),
                (2, "B"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_multiple_subsequent_descending_assertions_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a", 1.1),
                (2, "c", 1.3),
                (2, "b", 1.2),
                (1, "a", 1.1)
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2)
                .And
                .ThenBeInDescendingOrder(x => x.Item3);

            // Assert
            action.Should().NotThrow();
        }

        #endregion

        private class SomeClass
        {
            public string Text { get; set; }

            public int Number { get; set; }
        }

        #endregion

        #region Not Contain Nulls (Predicate)

        [Fact]
        public void When_injecting_a_null_predicate_into_NotContainNulls_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new SomeClass[] { };

            // Act
            Action act = () => collection.Should().NotContainNulls<string>(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" }
            };

            // Act / Assert
            collection.Should().NotContainNulls(e => e.Text);
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "" },
                new SomeClass { Text = null }
            };

            // Act
            Action act = () => collection.Should().NotContainNulls(e => e.Text, "because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*on e.Text*because they are evil*Text = <null>*");
        }

        [Fact]
        public void When_collection_contains_multiple_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "" },
                new SomeClass { Text = null },
                new SomeClass { Text = "" },
                new SomeClass { Text = null }
            };

            // Act
            Action act = () => collection.Should().NotContainNulls(e => e.Text, "because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*on e.Text*because they are evil*Text = <null>*Text = <null>*");
        }

        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().NotContainNulls(e => e.Text, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because we want to test the behaviour with a null subject, but collection is <null>.");
        }

        #endregion

        #region Only Have Unique Items (Predicate)

        [Fact]
        public void When_injecting_a_null_predicate_into_OnlyHaveUniqueItems_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new SomeClass[] { };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems<string>(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" },
                new SomeClass { Text = "four" }
            };

            // Act / Assert
            collection.Should().OnlyHaveUniqueItems(e => e.Text);
        }

        [Fact]
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" },
                new SomeClass { Text = "three" }
            };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems(e => e.Text, "{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items*on e.Text*because we don't like duplicates, but item*three*is not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" },
                new SomeClass { Text = "three" }
            };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems(e => e.Text, "{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items*on e.Text*because we don't like duplicates, but items*two*two*three*three*are not unique.");
        }

        [Fact]
        public void When_asserting_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = null;

            // Act
            Action act =
                () => collection.Should().OnlyHaveUniqueItems(e => e.Text, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Satisfy Respectively

        [Fact]
        public void When_collection_asserting_against_null_inspectors_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().SatisfyRespectively(null);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify against a <null> collection of inspectors*");
        }

        [Fact]
        public void When_collection_asserting_against_empty_inspectors_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().SatisfyRespectively();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify against an empty collection of inspectors*");
        }

        [Fact]
        public void When_collection_which_is_asserting_against_inspectors_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = null;

            // Act
            Action act = () => collection.Should().SatisfyRespectively(
                new Action<int>[]
                {
                    x => x.Should().Be(1)
                }, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all inspectors because we want to test the failure message, but collection is <null>.");
        }

        [Fact]
        public void When_collection_which_is_asserting_against_inspectors_is_empty_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<int>();

            // Act
            Action act = () => collection.Should().SatisfyRespectively(new Action<int>[]
            {
                x => x.Should().Be(1)
            }, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all inspectors because we want to test the failure message, but collection is empty.");
        }

        [Fact]
        public void When_asserting_collection_satisfies_all_inspectors_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new Customer { Age = 21, Name = "John" },
                new Customer { Age = 22, Name = "Jane" }
            };

            // Act / Assert
            collection.Should().SatisfyRespectively(
                value =>
                {
                    value.Age.Should().Be(21);
                    value.Name.Should().Be("John");
                },
                value =>
                {
                    value.Age.Should().Be(22);
                    value.Name.Should().Be("Jane");
                });
        }

        private class CustomerWithItems : Customer
        {
            public int[] Items { get; set; }
        }

        [Fact]
        public void When_asserting_collection_does_not_satisfy_any_inspector_it_should_throw()
        {
            // Arrange
            var customers = new[]
            {
                new CustomerWithItems { Age = 21, Items = new[] { 1, 2 } },
                new CustomerWithItems { Age = 22, Items = new[] { 3 } }
            };

            // Act
            Action act = () => customers.Should().SatisfyRespectively(
                new Action<CustomerWithItems>[]
                {
                    customer =>
                    {
                        customer.Age.Should().BeLessThan(21);
                        customer.Items.Should().SatisfyRespectively(
                            item => item.Should().Be(2),
                            item => item.Should().Be(1));
                    },
                    customer =>
                    {
                        customer.Age.Should().BeLessThan(22);
                        customer.Items.Should().SatisfyRespectively(item => item.Should().Be(2));
                    }
                }, "because we want to test {0}", "nested assertions");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
@"Expected customers to satisfy all inspectors because we want to test nested assertions, but some inspectors are not satisfied:
*At index 0:
*Expected customer.Age to be less than 21, but found 21
*Expected customer.Items to satisfy all inspectors, but some inspectors are not satisfied:
*At index 0:
*Expected item to be 2, but found 1
*At index 1:
*Expected item to be 1, but found 2
*At index 1:
*Expected customer.Age to be less than 22, but found 22
*Expected customer.Items to satisfy all inspectors, but some inspectors are not satisfied:
*At index 0:
*Expected item to be 2, but found 3"
);
        }

        [Fact]
        public void When_inspector_message_is_not_reformatable_it_should_not_throw()
        {
            // Arrange
            byte[][] subject = { new byte[] { 1 } };

            // Act
            Action act = () => subject.Should().SatisfyRespectively(e => e.Should().BeEquivalentTo(new byte[] { 2, 3, 4 }));

            // Assert
            act.Should().NotThrow<FormatException>();
        }

        [Fact]
        public void When_inspectors_count_does_not_equal_asserting_collection_length_it_should_throw_with_a_useful_message()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().SatisfyRespectively(
                new Action<int>[]
                {
                    value => value.Should().Be(1),
                    value => value.Should().Be(2)
                }, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain exactly 2 items*we want to test the failure message*, but it contains 3 items");
        }

        [Fact]
        public void When_inspectors_count_does_not_equal_asserting_collection_length_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var collection = new int[0];

            // Act
            Action act = () => collection.Should().SatisfyRespectively(
                new Action<int>[]
                {
                    value => value.Should().Be(1),
                }, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*because we want to test the failure*");
        }
        #endregion
    }
}
