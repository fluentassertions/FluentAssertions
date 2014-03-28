using System.Diagnostics;
using System.Xml.Linq;
using FluentAssertions.Execution;

using FluentAssertions.Common;
using FluentAssertions.Primitives;
using System.Text;
using System.IO;

namespace FluentAssertions.Xml
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XDocument"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class XDocumentAssertions : ReferenceTypeAssertions<XDocument, XDocumentAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XDocumentAssertions" /> class.
        /// </summary>
        public XDocumentAssertions(XDocument document)
        {
            Subject = document;
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> equals the <paramref name="expected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected document</param>
        public AndConstraint<XDocumentAssertions> Be(XDocument expected)
        {
            return Be(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> equals the <paramref name="expected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected document</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> Be(XDocument expected, string reason, params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.IsSameOrEqualTo(expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document to be {0}{reason}, but found {1}", expected, Subject);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> does not equal the <paramref name="unexpected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected document</param>
        public AndConstraint<XDocumentAssertions> NotBe(XDocument unexpected)
        {
            return NotBe(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> does not equal the <paramref name="unexpected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected document</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> NotBe(XDocument unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect XML document to be {0}, but found <null>.", unexpected);

            Execute.Assertion
                .ForCondition(!Subject.Equals(unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect XML document to be {0}{reason}.", unexpected);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> is equivalent to the <paramref name="expected"/> document,
        /// using its <see cref="XNode.DeepEquals()" /> implementation.
        /// </summary>
        /// <param name="expected">The expected document</param>
        public AndConstraint<XDocumentAssertions> BeEquivalentTo(XDocument expected)
        {
            return BeEquivalentTo(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> is equivalent to the <paramref name="expected"/> document,
        /// using its <see cref="XNode.DeepEquals()" /> implementation.
        /// </summary>
        /// <param name="expected">The expected document</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> BeEquivalentTo(XDocument expected, string reason, params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(XNode.DeepEquals(Subject, expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document {0} to be equivalent to {1}{reason}.",
                    Subject, expected);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> is not equivalent to the <paramref name="unexpected"/> document,
        /// using its <see cref="XNode.DeepEquals()" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected document</param>
        public AndConstraint<XDocumentAssertions> NotBeEquivalentTo(XDocument unexpected)
        {
            return NotBeEquivalentTo(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> is not equivalent to the <paramref name="unexpected"/> document,
        /// using its <see cref="XNode.DeepEquals()" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected document</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> NotBeEquivalentTo(XDocument unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!XNode.DeepEquals(Subject, unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect XML document {0} to be equivalent to {1}{reason}.",
                    Subject, unexpected);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name of the expected root element of the current document.</param>
        public AndConstraint<XDocumentAssertions> HaveRoot(string expected)
        {
            return HaveRoot(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name of the expected root element of the current document.</param>
        public AndConstraint<XDocumentAssertions> HaveRoot(XName expected)
        {
            return HaveRoot(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name of the expected root element of the current document.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> HaveRoot(string expected, string reason, params object[] reasonArgs)
        {
            return HaveRoot(XNamespace.None + expected, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The full name <see cref="XName"/> of the expected root element of the current document.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> HaveRoot(XName expected, string reason, params object[] reasonArgs)
        {
            XElement root = Subject.Root;

            Execute.Assertion
                .ForCondition((root != null) && (root.Name == expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document to have root element \"" + expected.ToString().Escape() + "\"{reason}" +
                    ", but found {0}.", Subject);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
        /// child element with the specified <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">
        /// The name of the expected child element of the current document's Root <see cref="XDocument.Root"/> element.
        /// </param>
        public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(string expected)
        {
            return HaveElement(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
        /// child element with the specified <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">
        /// The full name <see cref="XName"/> of the expected child element of the current document's Root <see cref="XDocument.Root"/> element.
        /// </param>
        public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(XName expected)
        {
            return HaveElement(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
        /// child element with the specified <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">
        /// The name of the expected child element of the current document's Root <see cref="XDocument.Root"/> element.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(string expected, string reason, params object[] reasonArgs)
        {
            return HaveElement(XNamespace.None + expected, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
        /// child element with the specified <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">
        /// The full name <see cref="XName"/> of the expected child element of the current document's Root <see cref="XDocument.Root"/> element.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndWhichConstraint<XDocumentAssertions, XElement> HaveElement(XName expected, string reason, params object[] reasonArgs)
        {
            string expectedText = expected.ToString().Escape();
            Execute.Assertion
                .ForCondition(Subject.Root != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document {0} to have root element with child \"" + expectedText + "\"{reason}" +
                    ", but XML document has no Root element.", Subject);

            XElement xElement = Subject.Root.Element(expected);
            Execute.Assertion
                .ForCondition(xElement != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document {0} to have root element with child \"" + expectedText + "\"{reason}" +
                    ", but no such child element was found.", Subject);

            return new AndWhichConstraint<XDocumentAssertions, XElement>(this, xElement);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "XML document"; }
        }
    }
}
