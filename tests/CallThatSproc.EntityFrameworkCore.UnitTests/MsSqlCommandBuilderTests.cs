using System.Net;

namespace CallThatSproc.EntityFrameworkCore.UnitTests;

public class MsSqlCommandBuilderTests
{
    class NoParametersProcedureCall : StoredProcedureCall
    {
        public override string Name => "NoParametersProcedure";
    }

    [Fact]
    public void BuildExecProcedureCommand_WithNoParameters_ShouldBuildCorrectCommand()
    {
        // Arrange
        var procedureCall = new NoParametersProcedureCall();
        var builder = new MsSqlCommandBuilder();

        // Act
        var sqlString = builder.BuildExecProcedureCommand(procedureCall);

        // Assert
        Assert.Matches("(EXEC|exec)\\s\\w*.\\w*", sqlString);
        Assert.EndsWith("dbo.NoParametersProcedure", sqlString);
    }

    class ParametersProcedureCall : StoredProcedureCall
    {
        public override string Name => "ParametersProcedure";
        public override string Schema => "notDbo";

        public ParametersProcedureCall()
        {
            Parameters.Add("Param1", "value");
            Parameters.Add("Param2", "value");
            Parameters.Add("Param3", "value");
        }
    }

    private readonly string _parametersRegex = """(EXEC|exec)\s\w*.\w*\s(@\w*=@\w*\s?,\s?)*(@\w*=@\w*)""";

    [Fact]
    public void BuildExecProcedureCommand_WithParameters_ShouldBuildCorrectCommand()
    {
        // Arrange
        var procedureCall = new ParametersProcedureCall();
        var builder = new MsSqlCommandBuilder();

        // Act
        var sqlString = builder.BuildExecProcedureCommand(procedureCall);

        // Assert
        Assert.Matches(_parametersRegex, sqlString);
        Assert.Contains("notDbo.ParametersProcedure", sqlString);
        Assert.Contains("@Param1=@Param1", sqlString);
        Assert.Contains("@Param2=@Param2", sqlString);
        Assert.Contains("@Param3=@Param3", sqlString);
    }

    class OutParametersProcedureCall : StoredProcedureCall
    {
        public override string Name => "OutParametersProcedure";
        public override string Schema => base.Schema;

        public OutParametersProcedureCall()
        {
            Parameters.Add("Param1", "value");
            Parameters.Add("Param2", "value", true);
            Parameters.Add("Param3", "value", true);
        }
    }

    private readonly string _outParametersRegex = """(EXEC|exec)\s\w*.\w*\s(@\w*=@\w*(\s(out|OUT|output|OUTPUT))?\s?,\s?)*(@\w*=@\w*)(\s(out|OUT|output|OUTPUT))?""";

    [Fact]
    public void BuildExecProcedureCommand_WithOutParameter_ShouldBuildCorrectCommand()
    {
        // Arrange
        var procedureCall = new OutParametersProcedureCall();
        var builder = new MsSqlCommandBuilder();

        // Act
        var sqlString = builder.BuildExecProcedureCommand(procedureCall);

        // Assert
        Assert.Matches(_outParametersRegex, sqlString);
        Assert.Contains("dbo.OutParametersProcedure", sqlString);
        Assert.Matches("""@Param1=@Param1\s?,""", sqlString);
        Assert.Matches("""@Param2=@Param2\s(OUT|OUTPUT|out|output)""", sqlString);
        Assert.Matches("""@Param3=@Param3\s(OUT|OUTPUT|out|output)""", sqlString);
    }
}
