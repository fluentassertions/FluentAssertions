using System.Text;
using System.Text.RegularExpressions;

using FluentAssertions.Common;

namespace FluentAssertions.Primitives
{
    internal class StringWildcardMatchingValidator : StringValidator
    {
        public StringWildcardMatchingValidator(string subject, string expected, string because, object[] becauseArgs)
            : base(subject, expected, because, becauseArgs)
        {
        }

        protected override void ValidateAgainstMismatch()
        {
            bool isMatch = IsMatch();

            if (!isMatch && !Negate)
            {
                assertion.FailWith(ExpectationDescription + Resources.String_ButYDoesNotFormat,
                    expected, subject);
            }

            if (isMatch && Negate)
            {
                assertion.FailWith(ExpectationDescription + Resources.String_ButYMatchesFormat,
                    expected, subject);
            }
        }

        private bool IsMatch()
        {
            var options = IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

            string input = CleanNewLines(subject);
            string pattern = ConvertWildcardToRegEx(CleanNewLines(expected));

            return Regex.IsMatch(input, pattern, options | RegexOptions.Singleline);
        }

        private string ConvertWildcardToRegEx(string wildcardExpression)
        {
            return "^" + Regex.Escape(wildcardExpression).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }

        private string CleanNewLines(string input)
        {
            if (input is null)
            {
                return null;
            }

            return IgnoreNewLineDifferences ? input.RemoveNewLines() : input;
        }

        protected override string ExpectationDescription
        {
            get
            {
                if (Negate && IgnoreCase)
                {
                    return Resources.String_DidNotExpectStringToMatchEquivalentOfXFormat;
                }

                if (Negate)
                {
                    return Resources.String_DidNotExpectStringToMatchXFormat;
                }

                if (IgnoreCase)
                {
                    return Resources.String_ExpectedStringToMatchEquivalentOfXFormat;
                }

                return Resources.String_ExpectedStringToMatchXFormat;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the subject should not match the pattern.
        /// </summary>
        public bool Negate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the matching process should ignore any casing difference.
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Ignores the difference between environment newline differences
        /// </summary>
        public bool IgnoreNewLineDifferences { get; set; }
    }
}
