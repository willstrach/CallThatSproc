using System.Data;
using System.Text;

namespace CallThatSproc.EntityFrameworkCore;

public static class StoredProcedureCallExtensions
{
    public static string ToSqlString(this StoredProcedureCall procedureCall)
    {   
        var procedureName = $"[{procedureCall.Schema}].[{procedureCall.Name}]";

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("EXEC ");
        stringBuilder.Append(procedureName);

        return stringBuilder.ToString();
    }
}
