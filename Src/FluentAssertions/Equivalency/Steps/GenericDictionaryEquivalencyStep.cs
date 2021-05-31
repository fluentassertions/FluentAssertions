using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class GenericDictionaryEquivalencyStep : IEquivalencyStep
    {
#pragma warning disable SA1110 // Allow opening parenthesis on new line to reduce line length
        private static readonly MethodInfo AssertSameLengthMethod =
            new Func<IDictionary<object, object>, IDictionary<object, object>, bool>
                (AssertSameLength).GetMethodInfo().GetGenericMethodDefinition();

        private static readonly MethodInfo AssertDictionaryEquivalenceMethod =
            new Action<EquivalencyValidationContext, IEquivalencyValidator, IEquivalencyAssertionOptions,
                    IDictionary<object, object>, IDictionary<object, object>>
                (AssertDictionaryEquivalence).GetMethodInfo().GetGenericMethodDefinition();
#pragma warning restore SA1110

        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator)
        {
            Type expectationType = comparands.GetExpectedType(context.Options);

            if (comparands.Expectation is null || !GetIDictionaryInterfaces(expectationType).Any())
            {
                return EquivalencyResult.ContinueWithNext;
            }

            if (PreconditionsAreMet(comparands, expectationType))
            {
                AssertDictionaryEquivalence(comparands, context, nestedValidator, expectationType);
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static Type[] GetIDictionaryInterfaces(Type type)
        {
            // Avoid expensive calculation when the type in question can't possibly implement IDictionary<,>.
            if (Type.GetTypeCode(type) != TypeCode.Object)
            {
                return Array.Empty<Type>();
            }

            return Common.TypeExtensions.GetClosedGenericInterfaces(
                type,
                typeof(IDictionary<,>));
        }

        private static bool PreconditionsAreMet(Comparands comparands, Type expectedType)
        {
            return AssertImplementsOnlyOneDictionaryInterface(comparands.Expectation)
                   && AssertSubjectIsNotNull(comparands.Subject)
                   && AssertExpectationIsNotNull(comparands.Subject, comparands.Expectation)
                   && AssertIsCompatiblyTypedDictionary(expectedType, comparands.Subject)
                   && AssertSameLength(comparands.Subject, expectedType, comparands.Expectation);
        }

        private static bool AssertSubjectIsNotNull(object subject)
        {
            return AssertionScope.Current
                .ForCondition(subject is not null)
                .FailWith("Expected {context:Subject} not to be {0}{reason}.", new object[] { null });
        }

        private static bool AssertExpectationIsNotNull(object subject, object expectation)
        {
            return AssertionScope.Current
                .ForCondition(expectation is not null)
                .FailWith("Expected {context:Subject} to be {0}{reason}, but found {1}.", null, subject);
        }

        private static bool AssertImplementsOnlyOneDictionaryInterface(object expectation)
        {
            Type[] interfaces = GetIDictionaryInterfaces(expectation.GetType());
            bool multipleInterfaces = interfaces.Length > 1;
            if (!multipleInterfaces)
            {
                return true;
            }

            AssertionScope.Current.FailWith(
                "{context:Expectation} implements multiple dictionary types.  "
                + $"It is not known which type should be use for equivalence.{Environment.NewLine}"
                + $"The following IDictionary interfaces are implemented: {string.Join(", ", (IEnumerable<Type>)interfaces)}");

            return false;
        }

        private static bool AssertIsCompatiblyTypedDictionary(Type expectedType, object subject)
        {
            Type expectedDictionaryType = GetIDictionaryInterface(expectedType);
            Type expectedKeyType = GetDictionaryKeyType(expectedDictionaryType);

            Type subjectType = subject.GetType();
            Type[] subjectDictionaryInterfaces = GetIDictionaryInterfaces(subjectType);
            if (!subjectDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    "Expected {context:subject} to be a {0}{reason}, but found a {1}.", expectedDictionaryType, subjectType);

                return false;
            }

            Type[] suitableDictionaryInterfaces = subjectDictionaryInterfaces.Where(
                @interface => GetDictionaryKeyType(@interface).IsAssignableFrom(expectedKeyType)).ToArray();

            if (suitableDictionaryInterfaces.Length > 1)
            {
                // SMELL: Code could be written to handle this better, but is it really worth the effort?
                AssertionScope.Current.FailWith(
                    "The subject implements multiple IDictionary interfaces. ");

                return false;
            }

            if (!suitableDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    $"The {{context:subject}} dictionary has keys of type {expectedKeyType}; "
                    + $"however, the expectation is not keyed with any compatible types.{Environment.NewLine}"
                    + $"The subject implements: {string.Join(",", (IEnumerable<Type>)subjectDictionaryInterfaces)}");

                return false;
            }

            return true;
        }

        private static Type GetDictionaryKeyType(Type expectedType)
        {
            return expectedType.GetGenericArguments()[0];
        }

        private static bool AssertSameLength(object subject, Type expectationType, object expectation)
        {
            if (subject is ICollection subjectCollection
                && expectation is ICollection expectationCollection
                && subjectCollection.Count == expectationCollection.Count)
            {
                return true;
            }

            Type subjectType = subject.GetType();
            Type[] subjectTypeArguments = GetDictionaryTypeArguments(subjectType);
            Type[] expectationTypeArguments = GetDictionaryTypeArguments(expectationType);
            Type[] typeArguments = subjectTypeArguments.Concat(expectationTypeArguments).ToArray();

            return (bool)AssertSameLengthMethod.MakeGenericMethod(typeArguments).Invoke(null, new[] { subject, expectation });
        }

        private static Type[] GetDictionaryTypeArguments(Type type)
        {
            Type dictionaryType = GetIDictionaryInterface(type);

            return dictionaryType.GetGenericArguments();
        }

        private static Type GetIDictionaryInterface(Type expectedType)
        {
            return GetIDictionaryInterfaces(expectedType).Single();
        }

        private static bool AssertSameLength<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            IDictionary<TSubjectKey, TSubjectValue> subject, IDictionary<TExpectedKey, TExpectedValue> expectation)
            where TExpectedKey : TSubjectKey

        // Type constraint of TExpectedKey is asymmetric in regards to TSubjectKey
        // but it is valid. This constraint is implicitly enforced by the
        // AssertIsCompatiblyTypedDictionary method which is called before
        // the AssertSameLength method.
        {
            if (expectation.Count == subject.Count)
            {
                return true;
            }

            KeyDifference<TSubjectKey, TExpectedKey> keyDifference = CalculateKeyDifference(subject, expectation);

            bool hasMissingKeys = keyDifference.MissingKeys.Count > 0;
            bool hasAdditionalKeys = keyDifference.AdditionalKeys.Any();

            return Execute.Assertion
                .WithExpectation("Expected {context:subject} to be a dictionary with {0} item(s){reason}, ", expectation.Count)
                .ForCondition(!hasMissingKeys || hasAdditionalKeys)
                .FailWith("but it misses key(s) {0}", keyDifference.MissingKeys)
                .Then
                .ForCondition(hasMissingKeys || !hasAdditionalKeys)
                .FailWith("but has additional key(s) {0}", keyDifference.AdditionalKeys)
                .Then
                .ForCondition(!hasMissingKeys || !hasAdditionalKeys)
                .FailWith("but it misses key(s) {0} and has additional key(s) {1}", keyDifference.MissingKeys,
                    keyDifference.AdditionalKeys)
                .Then
                .ClearExpectation();
        }

        private static KeyDifference<TSubjectKey, TExpectedKey> CalculateKeyDifference<TSubjectKey, TSubjectValue, TExpectedKey,
            TExpectedValue>(IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectedKey, TExpectedValue> expectation)
            where TExpectedKey : TSubjectKey
        {
            var missingKeys = new List<TExpectedKey>();
            var presentKeys = new HashSet<TSubjectKey>();

            foreach (TExpectedKey expectationKey in expectation.Keys)
            {
                if (subject.ContainsKey(expectationKey))
                {
                    presentKeys.Add(expectationKey);
                }
                else
                {
                    missingKeys.Add(expectationKey);
                }
            }

            var additionalKeys = new List<TSubjectKey>();
            foreach (TSubjectKey subjectKey in subject.Keys)
            {
                if (!presentKeys.Contains(subjectKey))
                {
                    additionalKeys.Add(subjectKey);
                }
            }

            return new KeyDifference<TSubjectKey, TExpectedKey>(missingKeys, additionalKeys);
        }

        private static void AssertDictionaryEquivalence(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator parent, Type expectedType)
        {
            Type subjectType = comparands.Subject.GetType();
            Type[] subjectTypeArguments = GetDictionaryTypeArguments(subjectType);
            Type[] expectationTypeArguments = GetDictionaryTypeArguments(expectedType);
            Type[] typeArguments = subjectTypeArguments.Concat(expectationTypeArguments).ToArray();

            AssertDictionaryEquivalenceMethod.MakeGenericMethod(typeArguments).Invoke(null,
                new[] { context, parent, context.Options, comparands.Subject, comparands.Expectation });
        }

        private static void AssertDictionaryEquivalence<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            EquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions options,
            IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectedKey, TExpectedValue> expectation)
            where TExpectedKey : TSubjectKey
        {
            foreach (TExpectedKey key in expectation.Keys)
            {
                if (subject.TryGetValue(key, out TSubjectValue subjectValue))
                {
                    if (options.IsRecursive)
                    {
                        // Run the child assertion without affecting the current context
                        using (new AssertionScope())
                        {
                            var nestedComparands = new Comparands(subject[key], expectation[key], typeof(TExpectedValue));

                            parent.RecursivelyAssertEquality(nestedComparands,
                                context.AsDictionaryItem<TExpectedKey, TExpectedValue>(key));
                        }
                    }
                    else
                    {
                        subjectValue.Should().Be(expectation[key], context.Reason.FormattedMessage, context.Reason.Arguments);
                    }
                }
                else
                {
                    AssertionScope.Current
                        .BecauseOf(context.Reason)
                        .FailWith("Expected {context:subject} to contain key {0}{reason}.", key);
                }
            }
        }

        private class KeyDifference<TSubjectKey, TExpectedKey>
        {
            public KeyDifference(List<TExpectedKey> missingKeys, List<TSubjectKey> additionalKeys)
            {
                MissingKeys = missingKeys;
                AdditionalKeys = additionalKeys;
            }

            public List<TExpectedKey> MissingKeys { get; }

            public List<TSubjectKey> AdditionalKeys { get; }
        }
    }
}
