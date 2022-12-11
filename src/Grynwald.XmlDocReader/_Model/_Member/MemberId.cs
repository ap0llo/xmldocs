using System.Diagnostics.CodeAnalysis;

namespace Grynwald.XmlDocReader;

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

        MemberType? type = id switch
        {
            ['N', ':', .. { Length: > 0 }] => MemberType.Namespace,
            ['T', ':', .. { Length: > 0 }] => MemberType.Type,
            ['F', ':', .. { Length: > 0 }] => MemberType.Field,
            ['P', ':', .. { Length: > 0 }] => MemberType.Property,
            ['M', ':', .. { Length: > 0 }] => MemberType.Method,
            ['E', ':', .. { Length: > 0 }] => MemberType.Event,
            _ => null
        };

        if (type is null)
        {
            parsed = null;
            return false;
        }

        parsed = new MemberId(id, type.Value, id[2..]);
        return true;
    }

    public static MemberId Parse(string id)
    {
        return TryParse(id, out var parsed)
            ? parsed
            : throw new FormatException($"'{id}' is not a valid member id");
    }

}
