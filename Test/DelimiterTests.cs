namespace Test;

public class DelimiterTests
{
    [Fact]
    public void Parse_DelimiterWithinQuotations_IgnoresDelimiter()
    {
        // Arrange
        string csv = @"Id, Name, Description
1, Cat, ""name, Orange""
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
                Description = $"name, Orange"
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
}
