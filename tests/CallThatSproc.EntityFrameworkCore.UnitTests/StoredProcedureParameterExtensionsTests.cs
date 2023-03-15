namespace CallThatSproc.EntityFrameworkCore.UnitTests;

public class StoredProcedureParameterExtensionsTests
{
    [Fact]
    public void ToSqlString_WithoutAtPrefix_ShouldReturnValidString()
    {
        //  Arrange
        var parameterName = "nameWithoutPrefix";
        var parameter = new StoredProcedureParameter(parameterName, "some arbitrary value");
        var expectedString = "@nameWithoutPrefix=@nameWithoutPrefix";

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.Equal(expectedString, sqlString);
    }

    [Fact]
    public void ToSqlString_WithAtPrefix_ShouldReturnValidString()
    {
        //  Arrange
        var parameterName = "@nameWithPrefix";
        var parameter = new StoredProcedureParameter(parameterName, "some arbitrary value");
        var expectedString = "@nameWithPrefix=@nameWithPrefix";

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.Equal(expectedString, sqlString);
    }

    [Fact]
    public void ToSqlString_InputParameter_ShouldNotHaveOutSuffix()
    {
        // Arrange
        var parameter = new StoredProcedureParameter("arbitraryName", "some arbitrary value", direction: System.Data.ParameterDirection.Input);

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.DoesNotContain(" OUT", sqlString);
    }

    [Fact]
    public void ToSqlString_OutputParameter_ShouldHaveOutSuffix()
    {
        // Arrange
        var parameter = new StoredProcedureParameter("arbitraryName", "some arbitrary value", direction: System.Data.ParameterDirection.Output);

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.EndsWith(" OUT", sqlString);
    }

    [Fact]
    public void ToSqlString_InOutParameter_ShouldHaveOutSuffix()
    {
        // Arrange
        var parameter = new StoredProcedureParameter("arbitraryName", "some arbitrary value", direction: System.Data.ParameterDirection.InputOutput);

        // Act
        var sqlString = parameter.ToSqlString();

        // Assert
        Assert.EndsWith(" OUT", sqlString);
    }
}
