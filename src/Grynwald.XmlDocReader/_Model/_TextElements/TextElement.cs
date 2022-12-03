﻿namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a inline text element in a <see cref="TextBlock"/>
/// </summary>
public abstract class TextElement
{
    /// <inheritdoc />
    public abstract override int GetHashCode();

    /// <inheritdoc />
    public abstract override bool Equals(object? obj);
}