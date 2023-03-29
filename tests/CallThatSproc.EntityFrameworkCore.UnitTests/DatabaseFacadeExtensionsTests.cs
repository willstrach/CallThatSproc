using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CallThatSproc.EntityFrameworkCore.UnitTests;

public class DatabaseFacadeExtensionsTests
{
    class TestProcedureCall : StoredProcedureCall
    {
        public override string Name => "Name";
    }

    [Fact]
    public void ExecuteStoredProcedureCall_WithCustomCommandBuilder_ShouldUseProvidedBuilder()
    {
        // Arrange
        var databaseFacade = TestDatabaseBuilder.CreateDatabaseFacade();
        var sqlBuilderMock = new Mock<ISqlCommandBuilder>();
        var arbitrarySqlCommand = "select 1";
        sqlBuilderMock.Setup(builder
            => builder.BuildExecProcedureCommand(It.IsAny<StoredProcedureCall>())).Returns(arbitrarySqlCommand);
        var procedureCall = new TestProcedureCall();

        // Act
        databaseFacade.ExecuteStoredProcedureCall(procedureCall, sqlBuilderMock.Object);

        // Assert
        sqlBuilderMock.Verify(builder => builder.BuildExecProcedureCommand(procedureCall));
    }

    [Fact]
    public async Task ExecuteStoredProcedureCallAsync_WithCustomCommandBuilder_ShouldUseProvidedBuilderAsync()
    {
        // Arrange
        var databaseFacade = TestDatabaseBuilder.CreateDatabaseFacade();
        var sqlBuilderMock = new Mock<ISqlCommandBuilder>();
        var arbitrarySqlCommand = "select 1";
        sqlBuilderMock.Setup(builder
            => builder.BuildExecProcedureCommand(It.IsAny<StoredProcedureCall>())).Returns(arbitrarySqlCommand);
        var procedureCall = new TestProcedureCall();

        // Act
        await databaseFacade.ExecuteStoredProcedureCallAsync(procedureCall, sqlBuilderMock.Object);

        // Assert
        sqlBuilderMock.Verify(builder => builder.BuildExecProcedureCommand(procedureCall));
    }
}
