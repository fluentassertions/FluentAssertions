using System;
using System.Collections;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class EnumerableEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        if (!IsCollection(comparands.GetExpectedType(context.Options)))
        {
            return EquivalencyResult.ContinueWithNext;
        }

        if (AssertSubjectIsCollection(comparands.Subject))
        {
            var validator = new EnumerableEquivalencyValidator(valueChildNodes, context)
            {
                Recursive = context.CurrentNode.IsRoot || context.Options.IsRecursive,
                OrderingRules = context.Options.OrderingRules
            };

            validator.Execute(ToArray(comparands.Subject), ToArray(comparands.Expectation));
        }

        return EquivalencyResult.EquivalencyProven;
    }

    private static bool AssertSubjectIsCollection(object subject)
    {
        bool conditionMet = AssertionScope.Current
            .ForCondition(subject is not null)
            .FailWith("Expected a collection, but {context:Subject} is <null>.");

        if (conditionMet)
        {
            conditionMet = AssertionScope.Current
                .ForCondition(IsCollection(subject.GetType()))
                .FailWith("Expected a collection, but {context:Subject} is of a non-collection type.");
        }

        return conditionMet;
    }

    private static bool IsCollection(Type type)
    {
        return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    internal static object[] ToArray(object value)
    {
        if (value == null)
        {
            return null;
        }

        try
        {
            return ((IEnumerable)value).Cast<object>().ToArray();
        }
        catch (InvalidOperationException) when (IsIgnorableArrayLikeType(value))
        {
            // This is probably a default ImmutableArray<T> or an empty ArraySegment.
            return [];
        }
    }

    private static bool IsIgnorableArrayLikeType(object value)
    {
        var type = value.GetType();
        return type.Name.Equals("ImmutableArray`1", StringComparison.Ordinal) ||
            (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ArraySegment<>));
    }
}
