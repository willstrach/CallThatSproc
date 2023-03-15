using System.Data;

namespace CallThatSproc.UnitTests;

public class StoredProcedureParameterTests
{
    private class DemoUDT : UserDefinedTableType
    {
        public int Property1 { get; set; } = 1;
    }

    [Fact]
    public void Constructor_WithIntValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = 86;

        // Act
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Assert
        Assert.Equal(expectedValue, (int)parameter.Value);
    }

    [Fact]
    public void Constructor_WithStringValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = "arbitrary value";

        // Act
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Assert
        Assert.Equal(expectedValue, (string)parameter.Value);
    }

    [Fact]
    public void Constructor_WithBooleanValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = false;

        // Act
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Assert
        Assert.Equal(expectedValue, (bool)parameter.Value);
    }

    [Fact]
    public void Constructor_WithCharValue_ShouldHaveCorrectValue()
    {
        // Arrange
        var expectedValue = 'a';

        // Act
        var parameter = new StoredProcedureParameter("arbitraryName", expectedValue);

        // Assert
        Assert.Equal(expectedValue, (char)parameter.Value);
    }

    [Fact]
    public void Constructor_WithSingleUDTValue_ShouldHaveDataTableValueWithOneRow()
    {
        // Arrange
        var udt = new DemoUDT();

        // Act
        var parameter = new StoredProcedureParameter("arbitraryName", udt);

        // Assert
        Assert.Single(((DataTable)parameter.Value).Rows);
    }

    [Fact]
    public void Constructor_WithListOfUDTValues_ShouldHaveDataTableWithMultipleRows()
    {
        // Arrange
        var numberOfRows = 100;
        var udts = Enumerable.Range(0, numberOfRows).Select(number => new DemoUDT() { Property1 = number }).ToList();

        // Act
        var parameter = new StoredProcedureParameter("arbitraryName", udts);

        // Assert
        Assert.Equal(numberOfRows, ((DataTable)parameter.Value).Rows.Count);
    }
}
