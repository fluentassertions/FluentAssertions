using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Formatting;

/// <summary>
/// This class is used by the <see cref="Formatter"/> class to collect all the output of the (nested calls of an) <see cref="IValueFormatter"/> into
/// a the final representation.
/// </summary>
/// <remarks>
/// The <see cref="FormattedObjectGraph"/> will ensure that the number of lines will be limited
/// to the maximum number of lines provided through its constructor. It will throw
/// a <see cref="MaxLinesExceededException"/> if the number of lines exceeds the maximum.
/// </remarks>
public class FormattedObjectGraph
{
    private readonly int maxLines;
    private readonly List<Line> lines = [new()];
    private Line currentLine;

    public FormattedObjectGraph(int maxLines)
    {
        this.maxLines = maxLines;
        currentLine = lines[0];
    }

    internal int Indentation { get; private set; }

    /// <summary>
    /// The number of spaces that should be used by every indentation level.
    /// </summary>
    public static int SpacesPerIndentation => 4;

    /// <summary>
    /// Returns the number of lines of text currently in the graph.
    /// </summary>
    public int LineCount => lines.Count;

    /// <summary>
    /// Starts a new line with the provided text fragment. Additional text can be added to
    /// that same line through <see cref="AddFragment"/>.
    /// </summary>
    public void AddFragmentOnNewLine(string fragment)
    {
        FlushCurrentLine();

        AddFragment(fragment);
    }

    /// <summary>
    /// Starts a new line with the provided line of text that does not allow
    /// adding more fragments of text.
    /// </summary>
    public void AddLine(string content)
    {
        FlushCurrentLine();

        AddFragment(Whitespace + content);
    }

    /// <summary>
    /// Adds a new fragment of text to the current line.
    /// </summary>
    public void AddFragment(string fragment)
    {
        if (lines.Count > 1 && Whitespace.Length > 0 && currentLine.IsEmpty)
        {
            currentLine.Append(Whitespace);
        }

        currentLine.Append(fragment);
    }

    /// <summary>
    /// Adds a new line if there are no lines and no fragment that would cause a new line.
    /// </summary>
    internal void EnsureInitialNewLine()
    {
        if (!lines[0].IsEmpty)
        {
            lines.Insert(0, new Line(string.Empty));
        }
    }

    private void FlushCurrentLine()
    {
        if (currentLine.Flush())
        {
            AppendWithoutExceedingMaximumLines(new Line());
        }
        else if (Whitespace.Length > 0)
        {
            currentLine.Append(Whitespace);
        }
    }

    private void AppendWithoutExceedingMaximumLines(Line line)
    {
        if (lines.Count > maxLines)
        {
            lines.Add(new Line());

            lines.Add(new Line(
                $"(Output has exceeded the maximum of {maxLines} lines. " +
                $"Increase {nameof(FormattingOptions)}.{nameof(FormattingOptions.MaxLines)} on {nameof(AssertionScope)} or {nameof(AssertionConfiguration)} to include more lines.)"));

            throw new MaxLinesExceededException();
        }

        lines.Add(line);
        currentLine = line;
    }

    /// <summary>
    /// Increases the indentation of every line written into this text block until the returned disposable is disposed.
    /// </summary>
    /// <remarks>
    /// The amount of spacing used for each indentation level is determined by <see cref="SpacesPerIndentation"/>.
    /// </remarks>
    public IDisposable WithIndentation()
    {
        Indentation++;

        return new Disposable(() =>
        {
            if (Indentation > 0)
            {
                Indentation--;
            }
        });
    }

    internal Anchor GetAnchor()
    {
        return new Anchor(this, lines[^1]);
    }

    private string Whitespace => MakeWhitespace(Indentation);

    private static string MakeWhitespace(int indent) => new(' ', indent * SpacesPerIndentation);

    /// <summary>
    /// Returns the final textual multi-line representation of the object graph.
    /// </summary>
    public override string ToString()
    {
        return string.Join(Environment.NewLine, lines.Select(line => line.ToString()));
    }

    internal bool HasLinesBeyond(Line line)
    {
        return lines.IndexOf(line) < (lines.Count - 1);
    }

    internal void InsertBefore(Line line, string fragment)
    {
        int index = lines.IndexOf(line);
        lines.Insert(index, new Line(Whitespace + fragment));

        line.EnsureWhitespace(Whitespace);
    }

    internal Line AddLineAfter(Line line, string fragment)
    {
        int index = lines.IndexOf(line);

        Line item = new(Whitespace + fragment);
        lines.Insert(index + 1, item);

        return item;

        // move currentLine one line up
    }

    public void AddLineOrFragment(string fragment)
    {
        if (lines.Count == 1)
        {
            AddFragment(fragment);
        }
        else
        {
            AddLine(fragment);
        }
    }
}
