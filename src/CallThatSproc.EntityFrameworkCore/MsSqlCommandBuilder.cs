namespace CallThatSproc;

public class MsSqlCommandBuilder : ISqlCommandBuilder
{
    public string BuildExecProcedureCommand(IStoredProcedureCall procedureCall)
    {
        var procedureName = $"{procedureCall.Schema}.{procedureCall.Name}";
        if (!procedureCall.Parameters.Any()) return $"exec {procedureName}";

        var parameterStrings = procedureCall.Parameters
            .Select(parameter => $"{parameter.ParameterName}={parameter.ParameterName}{(parameter.Direction == System.Data.ParameterDirection.Output ? " out": "")}");
        return $"exec {procedureName} {string.Join(',', parameterStrings)}";
    }
}
