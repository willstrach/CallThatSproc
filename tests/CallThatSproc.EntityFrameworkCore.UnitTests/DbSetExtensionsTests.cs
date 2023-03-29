using Microsoft.EntityFrameworkCore;

namespace CallThatSproc.EntityFrameworkCore.UnitTests;

public class DbSetExtensionsTests
{
    class TestProcedureCall : StoredProcedureCall
    {
        public override string Name => "Name";
    }

    [Fact]
    public void GetFromStoredProcedureCall_WithCustomCommandBuilder_ShouldUseProvidedBuilder()
    {
        // Arrange
        var dbSet = TestDatabaseBuilder.CreateDbSet();
        var sqlBuilderMock = new Mock<ISqlCommandBuilder>();
        var arbitrarySqlCommand = "select 1";
        sqlBuilderMock.Setup(builder
            => builder.BuildExecProcedureCommand(It.IsAny<StoredProcedureCall>())).Returns(arbitrarySqlCommand);
        var procedureCall = new TestProcedureCall();

        // Act
        dbSet.GetFromStoredProcedureCall(procedureCall, sqlBuilderMock.Object);

        // Assert
        sqlBuilderMock.Verify(builder => builder.BuildExecProcedureCommand(procedureCall));
    }

    [Fact]
    public async Task GetFromStoredProcedureCallAsync_WithCustomCommandBuilder_ShouldUseProvidedBuilderAsync()
    {
        // Arrange
        var dbSet = TestDatabaseBuilder.CreateDbSet();
        var sqlBuilderMock = new Mock<ISqlCommandBuilder>();
        var arbitrarySqlCommand = "select 1";
        sqlBuilderMock.Setup(builder
            => builder.BuildExecProcedureCommand(It.IsAny<StoredProcedureCall>())).Returns(arbitrarySqlCommand);
        var procedureCall = new TestProcedureCall();

        // Act
        await dbSet.GetFromStoredProcedureCallAsync(procedureCall, sqlBuilderMock.Object);

        // Assert
        sqlBuilderMock.Verify(builder => builder.BuildExecProcedureCommand(procedureCall));
    }
}
