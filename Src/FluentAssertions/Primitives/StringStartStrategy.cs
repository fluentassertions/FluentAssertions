using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringStartStrategy : IStringComparisonStrategy
{
    private readonly StringComparison stringComparison;

    public StringStartStrategy(StringComparison stringComparison)
    {
        this.stringComparison = stringComparison;
    }

    public string ExpectationDescription
    {
        get
        {
            string predicateDescription = IgnoreCase ? "start with equivalent of" : "start with";
            return "Expected {context:string} to " + predicateDescription + " ";
        }
    }

    private bool IgnoreCase
        => stringComparison == StringComparison.OrdinalIgnoreCase;

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        assertion
            .ForCondition(subject.Length >= expected.Length)
            .FailWith(ExpectationDescription + "{0}{reason}, but {1} is too short.", expected, subject)
            .Then
            .ForCondition(subject.StartsWith(expected, stringComparison))
            .FailWith(ExpectationDescription + "{0}{reason}, but {1} differs near " +
                subject.IndexedSegmentAt(subject.IndexOfFirstMismatch(expected, stringComparison)) +
                ".",
                expected, subject);
    }
}
