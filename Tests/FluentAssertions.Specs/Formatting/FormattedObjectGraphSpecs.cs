using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs.Formatting;

public class FormattedObjectGraphSpecs
{
    [Fact]
    public void Indention_always_happens_for_newlines()
    {
        var formatter = new FormattedObjectGraph(100);
        using var _ = formatter.WithIndentation();
        formatter.AddFragmentOnNewLine("A");
        formatter.AddFragment(", B");
        formatter.AddFragmentOnNewLine("C");

        var s = formatter.ToString();
        s.Should().Be(
            """
                A, B
                C
            """);
    }

    [Fact]
    public void Indentation_never_happens_within_a_line()
    {
        var formatter = new FormattedObjectGraph(100);
        using var _ = formatter.WithIndentation();
        formatter.AddFragment("A");
        formatter.AddFragment(", ");

        using var __ = formatter.WithIndentation();
        formatter.AddFragment("B");

        var s = formatter.ToString();
        s.Should().Be("A, B");
    }

    [Fact]
    public void Can_add_opening_fragment_at_the_beginning_of_the_existing_line()
    {
        var formatter = new FormattedObjectGraph(100);
        using var _ = formatter.WithIndentation();

        var temp = formatter.GetAnchor();

        formatter.AddFragment("A");
        formatter.AddFragment("B");

        temp.InsertLineOrFragment("{");

        var s = formatter.ToString();
        s.Should().Be("{AB");
    }

    [Fact]
    public void Can_add_opening_fragment_even_if_new_lines_were_added()
    {
        var formatter = new FormattedObjectGraph(100);
        using var _ = formatter.WithIndentation();
        formatter.AddFragment("A");

        var anchor = formatter.GetAnchor();
        formatter.AddFragmentOnNewLine("B");

        anchor.InsertLineOrFragment("{");

        var s = formatter.ToString();
        s.Should().Be(
            """
                {
                A
                B
            """);
    }

    [Fact]
    public void Can_insert_fragment_if_new_lines_were_added()
    {
        var formatter = new FormattedObjectGraph(100);
        using var _ = formatter.WithIndentation();
        formatter.AddFragment("A");

        var anchor = formatter.GetAnchor();
        formatter.AddFragmentOnNewLine("B");
        anchor.InsertLineOrFragment("C");

        var s = formatter.ToString();
        s.Should().Be(
            """
                C
                A
                B
            """);
    }

    [Fact]
    public void Can_insert_fragment()
    {
        var formatter = new FormattedObjectGraph(100);
        using var _ = formatter.WithIndentation();
        formatter.AddFragment("A");

        var anchor = formatter.GetAnchor();
        formatter.AddFragment("B");
        anchor.InsertLineOrFragment("C");

        var s = formatter.ToString();
        s.Should().Be("ACB");
    }

    [Fact]
    public void Can_insert_content_at_the_anchor_point()
    {
        var formatter = new FormattedObjectGraph(100);
        var anchor = formatter.GetAnchor();
        formatter.AddFragment("A");

        formatter.AddLineOrFragment("B");
        formatter.AddLineOrFragment("C");
        formatter.AddLineOrFragment("D");

        // Should be place at the point we created the anchor
        anchor.InsertLineOrFragment("{");

        formatter.AddLineOrFragment("}");

        var s = formatter.ToString();
        s.Should().Be("{ABCD}");
    }

    [Fact]
    public void Character_anchors_move_with_inserts()
    {
        var formatter = new FormattedObjectGraph(100);
        var anchor1 = formatter.GetAnchor();
        formatter.AddFragment("A");

        var anchor2 = formatter.GetAnchor();
        anchor1.AddLineOrFragment("B");
        anchor1.AddLineOrFragment("C");

        // This should affect anchor2, since it was created after A was added
        // and anchor1 was created before that.
        anchor1.InsertLineOrFragment("D");

        anchor2.InsertLineOrFragment("E");

        var s = formatter.ToString();
        s.Should().Be("DECBA");
    }

    [Fact]
    public void Multiple_anchors_can_be_conbined()
    {
        var formatter = new FormattedObjectGraph(100);
        var anchor = formatter.GetAnchor();

        formatter.AddLineOrFragment("A");

        var commaAnchor = formatter.GetAnchor();

        formatter.AddLineOrFragment("B");

        commaAnchor.InsertLineOrFragment(",");

        commaAnchor = formatter.GetAnchor();

        formatter.AddLineOrFragment("C");

        commaAnchor.InsertLineOrFragment(",");

        anchor.InsertLineOrFragment("{");
        formatter.AddLineOrFragment("}");

        var s = formatter.ToString();
        s.Should().Be("{A,B,C}");
    }
}
