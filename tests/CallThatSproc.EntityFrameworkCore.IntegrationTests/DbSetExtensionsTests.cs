using CallThatSproc.EntityFrameworkCore.IntegrationTests.ProcedureCalls;

namespace CallThatSproc.EntityFrameworkCore.IntegrationTests;

[Collection("Database collection")]
public class DbSetExtensionsTests
{
    public DbSetExtensionsTests(DatabaseFixture databaseFixture)
    {
        _context = databaseFixture.DatabaseContext;
    }

    private readonly DatabaseContext _context;

    [Fact]
    public void FromStoredProcedure_SelectWithNoParametersCall_ShouldBeAbleToReadData()
    {
        // Arrange
        var procedure = new SelectWithNoParametersCall();

        // Act
        var animals = _context.Animals.GetFromStoredProcedureCall(procedure);

        // Assert
        Assert.NotEmpty(animals);
        Assert.Equal(2, animals.First(animal => animal.Name == "Human").NumberOfLegs);
    }

    [Fact]
    public async Task FromStoredProcedureAsync_SelectWithNoParametersCall_ShouldBeAbleToReadData()
    {
        // Arrange
        var procedure = new SelectWithNoParametersCall();

        // Act
        var animals = await _context.Animals.GetFromStoredProcedureCallAsync(procedure);

        // Assert
        Assert.NotEmpty(animals);
        Assert.Equal(2, animals.First(animal => animal.Name == "Human").NumberOfLegs);
    }

    [Fact]
    public void FromStoredProcedure_SelectWithParametersCall_ShouldBeAbleToReadData()
    {
        // Arrange
        var procedure = new SelectWithParametersCall(2);

        // Act
        var animals = _context.Animals.GetFromStoredProcedureCall(procedure);

        // Assert
        Assert.NotEmpty(animals);
        Assert.All(animals, animal => Assert.Equal(2, animal.NumberOfLegs));
    }

    [Fact]
    public async Task FromStoredProcedureAsync_SelectWithParametersCall_ShouldBeAbleToReadData()
    {
        // Arrange
        var procedure = new SelectWithParametersCall(2);

        // Act
        var animals = await _context.Animals.GetFromStoredProcedureCallAsync(procedure);

        // Assert
        Assert.NotEmpty(animals);
        Assert.All(animals, animal => Assert.Equal(2, animal.NumberOfLegs));
    }

    [Fact]
    public async Task FromStoredProcedureAsync_SelectWithOutParameterCall_ShouldBeAbleToReadData()
    {
        // Arrange
        var procedure = new SelectWithOutParameterCall();

        // Act
        var animals = await _context.Animals.GetFromStoredProcedureCallAsync(procedure);

        // Assert
        Assert.NotEmpty(animals);
    }

    [Fact]
    public async Task FromStoredProcedureAsync_SelectWithOutParameterCall_ShouldBeAbleToReadOutParameter()
    {
        // Arrange
        var procedure = new SelectWithOutParameterCall();

        // Act
        var animals = await _context.Animals.GetFromStoredProcedureCallAsync(procedure);

        // Assert
        Assert.IsType<int>(procedure.Parameters.GetValueOf("RandomNumber"));
        Assert.InRange((int)procedure.Parameters.GetValueOf("RandomNumber"), 0, 100);
    }
}
