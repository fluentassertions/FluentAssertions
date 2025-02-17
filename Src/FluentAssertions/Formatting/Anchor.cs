using System;

namespace FluentAssertions.Formatting;

/// <summary>
/// Write fragments that may be on a single line or span multiple lines,
/// and this is not known until later parts of the fragment are written.
/// </summary>
internal record Anchor
{
    private readonly FormattedObjectGraph parentGraph;
    private Line line;

    public int CharacterIndex { get; set; }

    public Anchor(FormattedObjectGraph parentGraph, Line line)
    {
        this.parentGraph = parentGraph;
        this.line = line;

        // Track the point in the graph where this instance was created.
        CharacterIndex = line.Length;
    }

    private bool RenderOnSingleLine => !parentGraph.HasLinesBeyond(line);

    public void InsertLineOrFragment(string fragment)
    {
        if (RenderOnSingleLine)
        {
            line.Insert(CharacterIndex, fragment);
        }
        else
        {
            // This should also inject whitespace in front of the first existing line
            parentGraph.InsertBefore(line, fragment);
        }
    }

    internal void AddLineOrFragment(string fragment)
    {
        if (RenderOnSingleLine)
        {
            line.Insert(CharacterIndex, fragment);
        }
        else if (line.Length > (CharacterIndex + 1))
        {
            line = parentGraph.AddLineAfter(line, fragment);
        }
        else
        {
            line.Insert(CharacterIndex, fragment);
        }
    }
}
