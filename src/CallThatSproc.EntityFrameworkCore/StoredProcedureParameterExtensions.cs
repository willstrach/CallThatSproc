namespace CallThatSproc.EntityFrameworkCore;

public static class StoredProcedureParameterExtensions
{
    public static string ToSqlString(this IStoredProcedureParameter parameter)
    {
        var cleanName = parameter.Name.StartsWith("@") ? parameter.Name : $"@{parameter.Name}";
        var directionSuffix = parameter.Direction == System.Data.ParameterDirection.Input ? "" : " OUT";
        return $"{cleanName}={cleanName}{directionSuffix}";
    }
}
