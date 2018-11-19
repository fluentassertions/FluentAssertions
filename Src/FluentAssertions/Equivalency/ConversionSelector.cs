using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Collects the members that need to be converted by the <see cref="TryConversionStep"/>.
    /// </summary>
    public class ConversionSelector
    {
        private class ConversionSelectorRule
        {
            public Func<IMemberInfo, bool> Predicate { get; }
            public string Description { get; }

            public ConversionSelectorRule(Func<IMemberInfo, bool> predicate, string description)
            {
                Predicate = predicate;
                Description = description;
            }
        }
        private List<ConversionSelectorRule> inclusions = new List<ConversionSelectorRule>();
        private List<ConversionSelectorRule> exclusions = new List<ConversionSelectorRule>();

        public void IncludeAll()
        {
            inclusions.Add(new ConversionSelectorRule(_ => true, "Try conversion of all members. "));
        }

        public void Include(Expression<Func<IMemberInfo, bool>> predicate)
        {
            inclusions.Add(new ConversionSelectorRule(
                predicate.Compile(),
                $"Try conversion of member {predicate.Body}. "));
        }

        public void Exclude(Expression<Func<IMemberInfo, bool>> predicate)
        {
            exclusions.Add(new ConversionSelectorRule(
                predicate.Compile(),
                $"Do not convert member {predicate.Body}."));
        }

        public bool RequiresConversion(IMemberInfo info)
        {
            return inclusions.Any(p => p.Predicate(info)) && !exclusions.Any(p => p.Predicate(info));
        }

        public override string ToString()
        {
            if (inclusions.Count == 0 && exclusions.Count == 0)
            {
                return "Without automatic conversion.";
            }

            StringBuilder descriptionBuilder = new StringBuilder();

            foreach (var inclusion in inclusions)
            {
                descriptionBuilder.Append(inclusion.Description);
            }

            foreach (var exclusion in exclusions)
            {
                descriptionBuilder.Append(exclusion.Description);
            }

            return descriptionBuilder.ToString();
        }

        public ConversionSelector Clone()
        {
            return new ConversionSelector
            {
                inclusions = new List<ConversionSelectorRule>(inclusions),
                exclusions = new List<ConversionSelectorRule>(exclusions),
            };
        }
    }
}
