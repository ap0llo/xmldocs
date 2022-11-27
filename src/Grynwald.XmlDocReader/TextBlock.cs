
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

public class TextBlock : IEquatable<TextBlock>
{
    public static readonly TextBlock Empty = new TextBlock(Array.Empty<TextElement>());

    /// <summary>
    /// Gets whether the text block contains any elements
    /// </summary>
    public bool IsEmpty => Elements.Count == 0;

    /// <summary>
    /// Gets the text block's text elements.
    /// </summary>
    public IReadOnlyList<TextElement> Elements { get; init; } = Array.Empty<TextElement>();


    public TextBlock()
    {

    }

    public TextBlock(params TextElement[] elements)
    {
        Elements = elements;
    }


    /// <inheritdoc />
    public override int GetHashCode()
    {
        if(Elements.Count== 0)
            return 0;

        unchecked
        {
            var hash = Elements[0].GetHashCode() * 397;
            for(int i = 1; i < Elements.Count; i++)
            {
                hash ^= Elements[i].GetHashCode();
            }
            return hash;
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as TextBlock);

    /// <inheritdoc />
    public bool Equals(TextBlock? other)
    {
        if (other is null)
            return false;

        if(Elements.Count != other.Elements.Count)
            return false;


        for(int i = 0; i < Elements.Count; i++)
        {
            if (!Elements[i].Equals(other.Elements[i]))
                return false;
        }


        return true;
    }


    public static TextBlock FromXml(XElement xml)
    {
        return new TextBlock()
        {
            Elements = ReadElements(xml).ToList()
        };
    }


    private static IEnumerable<TextElement> ReadElements(XElement xml)
    {
        var indent = 0;
        if(xml.Nodes().OfType<XText>().FirstOrDefault() is XText textElement)
        {
            indent = XmlContentHelper.GetIndentation(textElement.Value);
        }

        foreach (var node in xml.Nodes())
        {
            if(node is XText textNode)
            {
                var text = XmlContentHelper.TrimText(textNode.Value, indent);
                if(!String.IsNullOrEmpty(text))
                    yield return new PlainTextElement(text);
            }
            else if(node is XElement element)
            {
                if (element.Name.LocalName switch
                {
                    "para" => ParaElement.FromXml(element),
                    "paramref" => ParamRefElement.FromXml(element),
                    "typeparamref" => TypeParamRefElement.FromXml(element),
                    "code" => CodeElement.FromXml(element),
                    "c" => CElement.FromXml(element),
                    "see" => SeeElement.FromXml(element),
                    "list" => ListElement.FromXml(element),

                    //TODO: Handle unknown elements

                    _ => default(TextElement)

                } is TextElement parsed)
                    yield return parsed;
            }
        }
    }



}
