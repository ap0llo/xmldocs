using System.Diagnostics.CodeAnalysis;

namespace Grynwald.XmlDocs;

public sealed class MemberId : IEquatable<MemberId>
{
    public MemberType Type { get; }

    public string Name { get; }

    public string Id { get; }


    private MemberId(string id, MemberType type, string name)
    {
        Type = type;
        Id = id;
        Name = name;
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Id);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as MemberId);

    /// <inheritdoc />
    public bool Equals(MemberId? other) => other is not null && StringComparer.Ordinal.Equals(Id, other.Id);

    /// <inheritdoc />
    public override string ToString() => Id;


    public static bool TryParse(string id, [NotNullWhen(true)] out MemberId? parsed)
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            parsed = null;
            return false;
        }

        MemberType? type = null;

        if (id.Length > 2)
        {
            var firstChar = id[0];
            var secondChar = id[1];
            if (secondChar == ':')
            {
                switch (firstChar)
                {
                    case 'N':
                        type = MemberType.Namespace;
                        break;
                    case 'T':
                        type = MemberType.Type;
                        break;
                    case 'F':
                        type = MemberType.Field;
                        break;
                    case 'P':
                        type = MemberType.Property;
                        break;
                    case 'M':
                        type = MemberType.Method;
                        break;
                    case 'E':
                        type = MemberType.Event;
                        break;
                    default:
                        type = null;
                        break;
                }
            }
        }

        if (type is null)
        {
            parsed = null;
            return false;
        }

        parsed = new MemberId(id, type.Value, id.Substring(2));
        return true;
    }

    public static MemberId Parse(string id)
    {
        return (TryParse(id, out var parsed))
            ? parsed
            : throw new FormatException($"'{id}' is not a valid member id");
    }

}
