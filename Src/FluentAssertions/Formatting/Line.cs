using System;
using System.Text;

namespace FluentAssertions.Formatting;

internal class Line
{
    private string content = string.Empty;
    private StringBuilder builder = new();

    public Line()
    {
    }

    public Line(string content)
    {
        this.content = content.TrimEnd();
    }

    public bool Flush()
    {
        content = builder.ToString();

        if (content.Length > 0)
        {
            builder = null;
            return true;
        }

        return false;
    }

    public bool IsEmpty => content.Length == 0 && builder?.Length == 0;

    public int Length => builder?.Length ?? content.Length;

    public override string ToString() => content.Length > 0 ? content.TrimEnd() : builder.ToString().TrimEnd();

    public void Append(string fragment)
    {
        if (builder is not null)
        {
            builder.Append(fragment);
        }
        else
        {
            content += fragment;
        }
    }

    public void Insert(int characterIndex, string fragment)
    {
        if (builder is null)
        {
            content = content.Insert(characterIndex, fragment);
        }
        else
        {
            builder.Insert(characterIndex, fragment);
        }
    }

    public void EnsureWhitespace(string whitespace)
    {
        if (content.Length > 0 && !content.StartsWith(whitespace, StringComparison.OrdinalIgnoreCase))
        {
            content = whitespace + content;
        }

        if (builder?.Length > 0 && !builder.ToString().StartsWith(whitespace, StringComparison.OrdinalIgnoreCase))
        {
            builder.Insert(0, whitespace);
        }
    }
}
