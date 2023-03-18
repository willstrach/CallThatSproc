using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace CallThatSproc.EntityFrameworkCore;

public static class DatabaseFacadeExtensions
{
    public static IStoredProcedureCallResult ExecuteStoredProcedureCall(this DatabaseFacade databaseFacade, IStoredProcedureCall storedProcedureCall)
    {
        var sqlString = storedProcedureCall.ToSqlString();
        var sqlParameters = storedProcedureCall.Parameters.Select(parameter => parameter.ToSqlParameter()).ToArray();

        var rowsAffected = databaseFacade.ExecuteSqlRaw(sqlString, sqlParameters);
        
        var anotherOutParameters = storedProcedureCall.Parameters
            .Where(parameter => parameter.Direction != ParameterDirection.Input).ToList()
            ?? new List<IStoredProcedureParameter>();

        foreach (var parameter in anotherOutParameters)
        {
            var relevantParameter = sqlParameters
                .First(sqlParameter => sqlParameter.ParameterName == parameter.Name || sqlParameter.ParameterName == $"@{parameter.Name}");
            parameter.Value = relevantParameter.Value;
        }

        var outParameters = sqlParameters.Where(sqlParameter => sqlParameter.Direction != ParameterDirection.Input).ToArray();

        var result = new StoredProcedureCallResult()
        { 
            RowsAffected = rowsAffected,
            OutParameters = anotherOutParameters.ToArray()
        };

        return result;
    }

    public static async Task<IStoredProcedureCallResult> ExecuteStoredProcedureCallAsync(this DatabaseFacade databaseFacade, IStoredProcedureCall storedProcedureCall)
    {
        var sqlString = storedProcedureCall.ToSqlString();
        var sqlParameters = storedProcedureCall.Parameters.Select(parameter => parameter.ToSqlParameter()).ToArray();

        var rowsAffected = await databaseFacade.ExecuteSqlRawAsync(sqlString, sqlParameters);

        var anotherOutParameters = storedProcedureCall.Parameters
            .Where(parameter => parameter.Direction != ParameterDirection.Input).ToList()
            ?? new List<IStoredProcedureParameter>();

        foreach (var parameter in anotherOutParameters)
        {
            var relevantParameter = sqlParameters
                .First(sqlParameter => sqlParameter.ParameterName == parameter.Name || sqlParameter.ParameterName == $"@{parameter.Name}");
            parameter.Value = relevantParameter.Value;
        }

        var outParameters = sqlParameters.Where(sqlParameter => sqlParameter.Direction != ParameterDirection.Input).ToArray();

        var result = new StoredProcedureCallResult()
        {
            RowsAffected = rowsAffected,
            OutParameters = anotherOutParameters.ToArray()
        };

        return result;
    }
}
