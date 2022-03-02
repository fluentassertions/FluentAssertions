﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions.Execution;

using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections.Data
{
    public static class DataColumnCollectionAssertionExtensionsSpecs
    {
        private static DataColumn CreateTestDataColumn(int seed)
        {
            var column = new DataColumn("Column" + seed);

            var random = new Random(seed);

            column.DataType =
                random.Next(4) switch
                {
                    0 => typeof(int),
                    1 => typeof(string),
                    2 => typeof(bool),
                    _ => typeof(DateTime),
                };

            return column;
        }

        public class BeSameAs
        {
            [Fact]
            public void When_references_are_the_same_it_should_succeed()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var columnCollection1 = dataTable.Columns;
                var columnCollection2 = columnCollection1;

                // Act & Assert
                columnCollection1.Should().BeSameAs(columnCollection2);
            }

            [Fact]
            public void When_references_are_different_it_should_fail()
            {
                // Arrange
                var dataTable1 = new DataTable("Test1");
                var dataTable2 = new DataTable("Test2");

                var columnCollection1 = dataTable1.Columns;
                var columnCollection2 = dataTable2.Columns;

                // Act
                Action action =
                    () => columnCollection1.Should().BeSameAs(columnCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected columnCollection1 to refer to *, but found * (different underlying object).");
            }

            [Fact]
            public void When_generic_collection_is_tested_against_typed_collection_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var columnCollection = dataTable.Columns;

                var genericCollection = columnCollection.Cast<DataColumn>();

                // Act
                Action action =
                    () => genericCollection.Should().BeSameAs(columnCollection, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected genericCollection to refer to DataColumnCollection because we care, but found * (different type).");
            }
        }

        public class NotBeSameAs
        {
            [Fact]
            public void When_references_are_the_same_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var columnCollection1 = dataTable.Columns;
                var columnCollection2 = columnCollection1;

                // Act
                Action action =
                    () => columnCollection1.Should().NotBeSameAs(columnCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage("Did not expect columnCollection1 to refer to *.");
            }

            [Fact]
            public void When_references_are_different_it_should_succeed()
            {
                // Arrange
                var dataTable1 = new DataTable("Test1");
                var dataTable2 = new DataTable("Test2");

                var columnCollection1 = dataTable1.Columns;
                var columnCollection2 = dataTable2.Columns;

                // Act & Assert
                columnCollection1.Should().NotBeSameAs(columnCollection2);
            }
        }

        public class HaveSameCount
        {
            [Fact]
            public void When_subject_is_null_it_should_fail()
            {
                // Arrange
                var subject = default(DataColumnCollection);

                var expectation = new DataTable().Columns;

                // Act
                Action action =
                    () => subject.Should().HaveSameCount(expectation, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected * to have the same count as * because we care, but found <null>.*");
            }

            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable();

                dataTable.Columns.Add(CreateTestDataColumn(0));
                dataTable.Columns.Add(CreateTestDataColumn(1));
                dataTable.Columns.Add(CreateTestDataColumn(2));

                var nullReference = default(DataColumnCollection);

                // Act
                Action action =
                    () => dataTable.Columns.Should().HaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataColumnCollectionAssertions
            {
                [Fact]
                public void When_two_collections_have_the_same_number_of_columns_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(11));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    // Act & Assert
                    firstDataTable.Columns.Should().HaveSameCount(secondDataTable.Columns);
                }

                [Fact]
                public void When_two_collections_do_not_have_the_same_number_of_columns_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    // Act
                    Action action =
                        () => firstDataTable.Columns.Should().HaveSameCount(secondDataTable.Columns);

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataTable.Columns to have 2 column(s), but found 3.");
                }
            }

            public class GeneicCollectionAssertions
            {
                [Fact]
                public void When_collection_is_compared_with_null_it_should_fail()
                {
                    // Arrange
                    var dataTable = new DataTable();

                    dataTable.Columns.Add(CreateTestDataColumn(0));
                    dataTable.Columns.Add(CreateTestDataColumn(1));
                    dataTable.Columns.Add(CreateTestDataColumn(2));

                    List<DataColumn> nullDataColumns = null;

                    // Act
                    Action action =
                        () => nullDataColumns.Should().HaveSameCount(dataTable.Columns, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataColumns to have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_columns_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(11));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act & Assert
                    genericDataColumnCollection.Should().HaveSameCount(secondDataTable.Columns);
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_columns_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act
                    Action action =
                        () => genericDataColumnCollection.Should().HaveSameCount(secondDataTable.Columns,
                            because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataColumnCollection to have 2 column(s) because we care, but found 3.");
                }
            }
        }

        public class NotHaveSameCount
        {
            [Fact]
            public void When_subject_is_null_it_should_fail()
            {
                // Arrange
                var subject = default(DataColumnCollection);

                var expectation = new DataTable().Columns;

                // Act
                Action action =
                    () => subject.Should().NotHaveSameCount(expectation, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected * to not have the same count as * because we care, but found <null>.*");
            }

            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable();

                dataTable.Columns.Add(CreateTestDataColumn(0));
                dataTable.Columns.Add(CreateTestDataColumn(1));
                dataTable.Columns.Add(CreateTestDataColumn(2));

                var nullReference = default(DataColumnCollection);

                // Act
                Action action =
                    () => dataTable.Columns.Should().NotHaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataColumnCollectionAssertions
            {
                [Fact]
                public void When_two_collections_have_different_number_of_columns_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    // Act & Assert
                    firstDataTable.Columns.Should().NotHaveSameCount(secondDataTable.Columns);
                }

                [Fact]
                public void When_two_collections_have_the_same_number_of_columns_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(11));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    // Act
                    Action action =
                        () => firstDataTable.Columns.Should().NotHaveSameCount(secondDataTable.Columns,
                            because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataTable.Columns to not have 3 column(s) because we care, but found 3.");
                }
            }

            public class GenericCollectionAssertions
            {
                [Fact]
                public void When_collection_is_compared_with_null_it_should_fail()
                {
                    // Arrange
                    var dataTable = new DataTable();

                    dataTable.Columns.Add(CreateTestDataColumn(0));
                    dataTable.Columns.Add(CreateTestDataColumn(1));
                    dataTable.Columns.Add(CreateTestDataColumn(2));

                    List<DataColumn> nullDataColumns = null;

                    // Act
                    Action action =
                        () => nullDataColumns.Should().NotHaveSameCount(dataTable.Columns, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataColumns to not have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_columns_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(11));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act
                    Action action =
                        () => genericDataColumnCollection.Should().NotHaveSameCount(secondDataTable.Columns,
                            because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataColumnCollection to not have 3 column(s) because we care, but found 3.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_columns_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    firstDataTable.Columns.Add(CreateTestDataColumn(0));
                    firstDataTable.Columns.Add(CreateTestDataColumn(1));
                    firstDataTable.Columns.Add(CreateTestDataColumn(2));

                    secondDataTable.Columns.Add(CreateTestDataColumn(10));
                    secondDataTable.Columns.Add(CreateTestDataColumn(12));

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act & Assert
                    genericDataColumnCollection.Should().NotHaveSameCount(secondDataTable.Columns);
                }
            }
        }
    }
}
