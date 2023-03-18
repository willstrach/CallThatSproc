using CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

namespace CallThatSproc.EntityFrameworkCore.IntegrationTests;

[Collection("Database collection")]
public class DatabaseFacadeExtensionsTests
{
    public DatabaseFacadeExtensionsTests(DatabaseFixture databaseFixture)
    {
        _context = databaseFixture.DatabaseContext;
    }

    private readonly DatabaseContext _context;

    [Fact]
    public void ExecuteStoredProcedure_EmptyProcedureCall_ShouldBeAbleToExecuteProcedure()
    {
        // Arrange
        var procedureCall = new BasicCall();

        // Act
        _context.Database.ExecuteStoredProcedureCall(procedureCall);
    }

    [Fact]
    public async Task ExecuteStoredProcedureAsync_EmptyProcedureCall_ShouldBeAbleToExecuteProcedure()
    {
        // Arrange
        var procedureCall = new BasicCall();

        // Act
        await _context.Database.ExecuteStoredProcedureCallAsync(procedureCall);
    }

    [Fact]
    public void ExecuteStoredProcedure_EmptyProcedureCallWithParameters_ShouldBeAbleToExecuteProcedure()
    {
        // Arrange
        var procedureCall = new BasicWithParameters(10, "some string");

        // Act
        _context.Database.ExecuteStoredProcedureCall(procedureCall);
    }

    [Fact]
    public async Task ExecuteStoredProcedureAsync_EmptyProcedureCallWithParameters_ShouldBeAbleToExecuteProcedure()
    {
        // Arrange
        var procedureCall = new BasicWithParameters(10, "some string");

        // Act
        await _context.Database.ExecuteStoredProcedureCallAsync(procedureCall);
    }

    [Fact]
    public void ExecuteStoredProcedure_SumValuesCall_ShouldBeAbleToExecute()
    {
        // Arrange
        var procedureCall = new SumValuesCall(11, 14);

        // Act
        var response = _context.Database.ExecuteStoredProcedureCall(procedureCall);
        var sumParameter = response.OutParameters.First(parameter => parameter.Name == "Sum");

        // Assert
        Assert.IsType<int>(sumParameter.Value);
        Assert.Equal(25, sumParameter.Value);
    }

    [Fact]
    public async Task ExecuteStoredProcedureAsync_SumValuesCall_ShouldBeAbleToExecute()
    {
        // Arrange
        var procedureCall = new SumValuesCall(11, 14);

        // Act
        var response = await _context.Database.ExecuteStoredProcedureCallAsync(procedureCall);
        var sumParameter = response.OutParameters.First(parameter => parameter.Name == "Sum");

        // Assert
        Assert.IsType<int>(sumParameter.Value);
        Assert.Equal(25, sumParameter.Value);
    }

    [Fact]
    public void ExecuteStoredProcedure_BasicWithUDTTCall_ShouldBeAbleToExecute()
    {
        // Arrange
        var procedureCall = new ConcatenateUDTT("value1", "value2");

        // Act
        var response = _context.Database.ExecuteStoredProcedureCall(procedureCall);
        var concatenatedParameter = response.OutParameters.First(parameter => parameter.Name == "Concatenated");

        // Assert
        Assert.IsType<string>(concatenatedParameter.Value);
        Assert.Equal("value1value2", concatenatedParameter.Value);
    }

    [Fact]
    public async Task ExecuteStoredProcedureAsync_BasicWithUDTTCall_ShouldBeAbleToExecute()
    {
        // Arrange
        var procedureCall = new ConcatenateUDTT("value1", "value2");

        // Act
        var response = await _context.Database.ExecuteStoredProcedureCallAsync(procedureCall);
        var concatenatedParameter = response.OutParameters.First(parameter => parameter.Name == "Concatenated");

        // Assert
        Assert.IsType<string>(concatenatedParameter.Value);
        Assert.Equal("value1value2", concatenatedParameter.Value);
    }
}
