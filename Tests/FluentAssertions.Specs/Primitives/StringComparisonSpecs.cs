﻿using System;
using System.Collections.Generic;
using FluentAssertions.Specs.CultureAwareTesting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives
{
    [Collection(nameof(StringComparisonSpecs))]
    public class StringComparisonSpecs
    {
        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_the_Turkish_letter_i_it_should_differ_by_dottedness(string subject, string expected)
        {
            // Act
            bool ordinal = string.Equals(subject, expected, StringComparison.OrdinalIgnoreCase);
#pragma warning disable CA1309 // Verifies that test data behaves differently in current vs invariant culture
            bool currentCulture = string.Equals(subject, expected, StringComparison.CurrentCultureIgnoreCase);
#pragma warning restore CA1309

            // Assert
            ordinal.Should().NotBe(currentCulture, "Turkish distinguishes between a dotted and a non-dotted 'i'");
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_the_same_digit_from_different_cultures_they_should_be_equal(string subject, string expected)
        {
            // Act
            bool ordinal = string.Equals(subject, expected, StringComparison.Ordinal);
#pragma warning disable CA1309 // Verifies that test data behaves differently in current vs invariant culture
            bool currentCulture = string.Equals(subject, expected, StringComparison.CurrentCulture);
#pragma warning restore CA1309

            // Assert
            ordinal.Should().NotBe(currentCulture,
                "These two symbols happened to be culturewise identical on both ICU (net5.0, linux, macOS) and NLS (netfx and netcoreapp on windows)");
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_equivalency_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_equality_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_having_prefix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().StartWith(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_not_having_prefix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotStartWith(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_having_equivalent_prefix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().StartWithEquivalentOf(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_not_having_equivalent_prefix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotStartWithEquivalentOf(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_having_suffix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().EndWith(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_not_having_suffix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotEndWith(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_having_equivalent_suffix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().EndWithEquivalentOf(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_not_having_equivalent_suffix_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotEndWithEquivalentOf(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_containing_equivalent_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().ContainEquivalentOf(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_not_containing_equivalent_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotContainEquivalentOf(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_containing_equal_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().Contain(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_containing_all_equals_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().ContainAll(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_containing_any_equals_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().ContainAny(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_containing_one_equal_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().Contain(expected, Exactly.Once());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EquivalencyData))]
        public void When_comparing_strings_for_containing_one_equivalent_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().ContainEquivalentOf(expected, Exactly.Once());

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_not_containing_equal_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotContain(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_not_containing_all_equals_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotContainAll(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [CulturedTheory("tr-TR")]
        [MemberData(nameof(EqualityData))]
        public void When_comparing_strings_for_not_containing_any_equals_it_should_ignore_culture(string subject, string expected)
        {
            // Act
            Action act = () => subject.Should().NotContainAny(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        public static IEnumerable<object[]> EquivalencyData
        {
            get
            {
                const string LowerCaseI = "i";
                const string UpperCaseI = "I";

                return new List<object[]> { new object[] { LowerCaseI, UpperCaseI } };
            }
        }

        public static IEnumerable<object[]> EqualityData
        {
            get
            {
                const string SinhalaLithDigitEight = "෮";
                const string MyanmarTaiLaingDigitEight = "꧸";

                return new List<object[]> { new object[] { SinhalaLithDigitEight, MyanmarTaiLaingDigitEight } };
            }
        }
    }

    // Due to CulturedTheory changing CultureInfo
    [CollectionDefinition(nameof(StringComparisonSpecs), DisableParallelization = true)]
    public class StringComparisonDefinition { }
}
