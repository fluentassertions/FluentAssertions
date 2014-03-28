namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Defines a rule that is used to determine whether the order of items in collections is relevant or not.
    /// </summary>
    public interface IOrderingRule
    {
        /// <summary>
        /// Determines if ordering of the property refered to by the current <paramref name="subjectInfo"/> is relevant.
        /// </summary>
        bool AppliesTo(ISubjectInfo subjectInfo);
    }
}