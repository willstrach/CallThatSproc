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
        var sqlParameter = new SqlParameter(parameter.Name, parameter.Value);
        sqlParameter.Direction = parameter.Direction;

        if (parameter.Value is DataTable)
        {
            sqlParameter.SqlDbType = SqlDbType.Structured;
            return sqlParameter;
        }

        if (parameter.DbType is not null)
        {
            sqlParameter.DbType = parameter.DbType ?? DbType.Binary;
        }

        return sqlParameter;
    }
}
