using System.Data;
using System.Text;

namespace CallThatSproc.EntityFrameworkCore;

public static class StoredProcedureCallExtensions
{
    public static string ToSqlString(this IStoredProcedureCall procedureCall)
    {   
        var procedureName = $"[{procedureCall.Schema}].[{procedureCall.Name}]";

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("EXEC ");
        stringBuilder.Append(procedureName);

        var parameterStrings = procedureCall.Parameters.Select(parameter => parameter.ToSqlString()).ToList();
        if (parameterStrings.Count == 0) return stringBuilder.ToString();

        stringBuilder.AppendJoin(",", parameterStrings);

        return stringBuilder.ToString();
    }
}
