using NuGet.Frameworks;
using System.Data;

namespace CallThatSproc.UnitTests;

public class UserDefinedTableTypeParameterTests
{
    private class DemoUDT : UserDefinedTableType
    {
        public int Property1 { get; set; } = 1;
    }

    [Fact]
    public void Constructor_WithSingleValue_ShouldHaveDataTableValueWithOneRow()
    {
        // Arrange
        var udt = new DemoUDT();

        // Act
        var parameter = new UserDefinedTableTypeParameter("arbitraryName", udt);

        // Assert
        Assert.Single(((DataTable)parameter.Value).Rows);
    }

    [Fact]
    public void Constructor_WithListOfValues_ShouldHaveDataTableWithMultipleRows()
    {
        // Arrange
        var numberOfRows = 100;
        var udts = Enumerable.Range(0, numberOfRows).Select(number => new DemoUDT() { Property1 = number }).ToList();

        // Act
        var parameter = new UserDefinedTableTypeParameter("arbitraryName", udts);

        // Assert
        Assert.Equal(numberOfRows, ((DataTable)parameter.Value).Rows.Count);

    }
}
