using System.Xml.Linq;

using FluentAssertions.Common;
using Xunit;

namespace FluentAssertions.Specs
{
    public class TypeExtensionsSpecs
    {
        [Fact]
        public void When_comparing_types_and_types_are_same_it_should_return_true()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var type1 = typeof(InheritedType);
            var type2 = typeof(InheritedType);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            bool result = type1.IsSameOrInherits(type2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().BeTrue();
        }

        [Fact]
        public void When_comparing_types_and_first_type_inherits_second_it_should_return_true()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var type1 = typeof(InheritingType);
            var type2 = typeof(InheritedType);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            bool result = type1.IsSameOrInherits(type2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().BeTrue();
        }

        [Fact]
        public void When_comparing_types_and_second_type_inherits_first_it_should_return_false()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var type1 = typeof(InheritedType);
            var type2 = typeof(InheritingType);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            bool result = type1.IsSameOrInherits(type2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().BeFalse();
        }

        [Fact]
        public void When_comparing_types_and_types_are_different_it_should_return_false()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var type1 = typeof(string);
            var type2 = typeof(InheritedType);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            bool result = type1.IsSameOrInherits(type2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().BeFalse();
        }

        class InheritedType { }

        class InheritingType : InheritedType { }
    }
}