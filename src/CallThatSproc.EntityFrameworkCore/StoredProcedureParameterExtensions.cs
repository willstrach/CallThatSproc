using Microsoft.Data.SqlClient;
using System.Data;

namespace CallThatSproc.EntityFrameworkCore;

public static class StoredProcedureParameterExtensions
{
    public static string ToSqlString(this IStoredProcedureParameter parameter)
    {
        var cleanName = parameter.Name.StartsWith("@") ? parameter.Name : $"@{parameter.Name}";
        var directionSuffix = parameter.Direction == System.Data.ParameterDirection.Input ? "" : " OUT";
        return $"{cleanName}={cleanName}{directionSuffix}";
    }

    public static SqlParameter ToSqlParameter(this IStoredProcedureParameter parameter)
    {
        var cleanName = parameter.Name.StartsWith("@") ? parameter.Name : $"@{parameter.Name}";

        var sqlParameter = new SqlParameter(cleanName, parameter.Value);
        sqlParameter.Size = -1;
        sqlParameter.Direction = parameter.Direction;

        if (parameter.Value is DataTable)
        {
            sqlParameter.SqlDbType = SqlDbType.Structured;
            sqlParameter.TypeName = parameter.TypeName;
            return sqlParameter;
        }

        if (parameter.DbType is not null)
        {
            sqlParameter.DbType = parameter.DbType ?? DbType.Binary;
        }

        return sqlParameter;
    }
}
