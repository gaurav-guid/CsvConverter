namespace Test;

public class TrimValuesTests
{
    [Fact]
    public void Parse_TrimValuesFalse_DoesNotTrimValues()
    {
        // Arrange
        string csv = @"Id, Name, Description
1, , Orange cat ";

        var parser = new CsvParser(o => o.TrimValues = false);

        // Act
        var parsed = parser.Parse(csv, true);

        // Assert
        Assert.Collection(parsed, (obj) =>
        {
            Assert.False(Utils.ObjectsHaveDifferences(obj, new
            {
                Id = 1,
                Name = " ",
                Description = " Orange cat "
            }));
        });
    }
}
