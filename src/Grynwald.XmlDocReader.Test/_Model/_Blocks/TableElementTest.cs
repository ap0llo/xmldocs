using Xunit;
namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="TableElement"/>
/// </summary>
public class TableElementTest
{
    [Fact]
    public void Rows_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new TableElement(header: null!, rows: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("rows", argumentNullException.ParamName);
    }

    [Fact]
    public void GetHashCode_returns_0_for_empty_table()
    {
        // ARRANGE
        var sut = new TableElement(null, Array.Empty<TableRow>());

        // ACT 
        var hashCode = sut.GetHashCode();

        // ASSERT
        Assert.Equal(0, hashCode);
    }


    [Fact]
    public void Two_instances_are_equal_if_their_rows_are_equal_01()
    {
        // ARRANGE
        var instance1 = new TableElement(
            header: null,
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                }),
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 2, Column 1")),
                    new TextBlock(new PlainTextElement("Row 2, Column 2")),
                }),
            }
        );
        var instance2 = new TableElement(
            header: null,
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                }),
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 2, Column 1")),
                    new TextBlock(new PlainTextElement("Row 2, Column 2")),
                }),
            }
        );

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_equal_if_their_rows_are_equal_02()
    {
        // ARRANGE
        var instance1 = new TableElement(
            header: new TableRow(new[]
            {
                new TextBlock(new PlainTextElement("Header, Column 1")),
                new TextBlock(new PlainTextElement("Header, Column 2")),
            }),
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                }),
            }
        );
        var instance2 = new TableElement(
            header: new TableRow(new[]
            {
                new TextBlock(new PlainTextElement("Header, Column 1")),
                new TextBlock(new PlainTextElement("Header, Column 2")),
            }),
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                }),
            }
        );

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_rows_are_not_equal_01()
    {
        // ARRANGE
        var instance1 = new TableElement(
            header: null,
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                }),
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 2, Column 1")),
                    new TextBlock(new PlainTextElement("Row 2, Column 2")),
                }),
            }
        );
        var instance2 = new TableElement(
            header: null,
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                })
            }
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_rows_are_not_equal_02()
    {
        // ARRANGE
        // ARRANGE
        var instance1 = new TableElement(
            header: null,
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                })
            }
        );
        var instance2 = new TableElement(
            header: null,
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 2, Column 1")),
                    new TextBlock(new PlainTextElement("Row 2, Column 2")),
                })
            }
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_rows_are_not_equal_03()
    {
        // ARRANGE
        // ARRANGE
        var instance1 = new TableElement(
            header: new TableRow(new[]
            {
                new TextBlock(new PlainTextElement("Header, Column 1")),
                new TextBlock(new PlainTextElement("Header, Column 2")),
            }),
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                })
            }
        );
        var instance2 = new TableElement(
            header: null,
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                })
            }
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_rows_are_not_equal_04()
    {
        // ARRANGE
        // ARRANGE
        var instance1 = new TableElement(
            header: new TableRow(new[]
            {
                new TextBlock(new PlainTextElement("Header, Column 1")),
                new TextBlock(new PlainTextElement("Header, Column 2")),
            }),
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                })
            }
        );
        var instance2 = new TableElement(
            header: new TableRow(new[]
            {
                new TextBlock(new PlainTextElement("Some other Header, Column 1")),
                new TextBlock(new PlainTextElement("Some other Header, Column 2")),
            }),
            rows: new[]
            {
                new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                })
            }
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Equals_returns_false_when_comparing_to_null()
    {
        // ARRANGE
        var sut = new TableElement(null, Array.Empty<TableRow>());

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut.Equals(null));
    }
}
