using CsvConverter;

namespace Test;

public class BasicTests
{
    [Fact]
    public void Parse_BasicCsv_Parses()
    {
        // Arrange
        string csv = @"Id, Name, Description
1, Cat, Orange cat
2, Dog, Black dog";

        var parser = new CsvParser();

        // Act
        var parsed = parser.Parse(csv, true);

        // Assert
        Assert.Collection(parsed, (obj) =>
        {
            Assert.False(Utils.ObjectsHaveDifferences(obj, new
            {
                Id = 1,
                Name = "Cat",
                Description = "Orange cat"
            }));
        }, (obj) =>
        {
            Assert.False(Utils.ObjectsHaveDifferences(obj, new
            {
                Id = 2,
                Name = "Dog",
                Description = "Black dog"
            }));
        });
    }

    [Fact]
    public void Parse_EmptyValues_Parses()
    {
        // Arrange
        string csv = @"Id, Name, Description
1, , Orange cat";

        var parser = new CsvParser();

        // Act
        var parsed = parser.Parse(csv, true);

        // Assert
        Assert.Collection(parsed, (obj) =>
        {
            Assert.False(Utils.ObjectsHaveDifferences(obj, new
            {
                Id = 1,
                Name = "",
                Description = "Orange cat"
            }));
        });
    }
}