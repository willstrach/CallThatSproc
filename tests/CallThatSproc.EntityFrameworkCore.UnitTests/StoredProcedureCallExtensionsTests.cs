using Moq;

namespace CallThatSproc.EntityFrameworkCore.UnitTests;

public class StoredProcedureCallExtensionsTests
{
    [Fact]
    public void ToSqlString_NoParameters_ShouldStartWithExec()
    {
        // Arrange
        var mockStoredProcedureCall = new Mock<StoredProcedureCall>();
        mockStoredProcedureCall.Setup(spc => spc.Schema).Returns("schema");
        mockStoredProcedureCall.Setup(spc => spc.Name).Returns("noparams");
        mockStoredProcedureCall.Setup(spc => spc.Parameters).Returns(new List<IStoredProcedureParameter>());

        var procedureCall = mockStoredProcedureCall.Object;

        // Act
        var sqlString = procedureCall.ToSqlString();

        // Assert
        Assert.StartsWith("EXEC [schema].[noparams]", sqlString);
    }

    [Fact]
    public void ToSqlString_WithParameters_ShouldStartWithExec()
    {
        // Arrange
        var mockStoredProcedureCall = new Mock<StoredProcedureCall>();
        mockStoredProcedureCall.Setup(spc => spc.Schema).Returns("schema");
        mockStoredProcedureCall.Setup(spc => spc.Name).Returns("withparams");

        var parameters = new List<IStoredProcedureParameter>()
        {
            new StoredProcedureParameter<string>("parameter1", "value"),
            new StoredProcedureParameter<string>("parameter2", "value"),
        };
        mockStoredProcedureCall.Setup(spc => spc.Parameters).Returns(parameters);

        var procedureCall = mockStoredProcedureCall.Object;

        // Act
        var sqlString = procedureCall.ToSqlString();

        // Assert
        Assert.StartsWith("EXEC [schema].[withparams]", sqlString);
    }
}
