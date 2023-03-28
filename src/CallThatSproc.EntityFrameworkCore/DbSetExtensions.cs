using Microsoft.EntityFrameworkCore;

namespace CallThatSproc.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static IEnumerable<TEntity> GetFromStoredProcedureCall<TEntity>(this DbSet<TEntity> dbSet, IStoredProcedureCall storedProcedureCall, ISqlCommandBuilder? commandBuilder = default) where TEntity : class
    {
        var builder = commandBuilder is null ? new MsSqlCommandBuilder() : commandBuilder;
        var commandString = builder.BuildExecProcedureCommand(storedProcedureCall);
        var parameterArray = storedProcedureCall.Parameters.ToArray();

        var entities = dbSet.FromSqlRaw(commandString, parameterArray).ToList();

        foreach (var parameter in parameterArray)
        {
            storedProcedureCall.Parameters.First(param => param.ParameterName == parameter.ParameterName).Value = parameter.Value;
        }

        return entities;
    }

    public static async Task<IEnumerable<TEntity>> GetFromStoredProcedureCallAsync<TEntity>(this DbSet<TEntity> dbSet, IStoredProcedureCall storedProcedureCall, ISqlCommandBuilder? commandBuilder = default) where TEntity : class
    {
        var builder = commandBuilder is null ? new MsSqlCommandBuilder() : commandBuilder;
        var commandString = builder.BuildExecProcedureCommand(storedProcedureCall);
        var parameterArray = storedProcedureCall.Parameters.ToArray();

        var entities = await dbSet.FromSqlRaw(commandString, parameterArray).ToListAsync();

        foreach (var parameter in parameterArray)
        {
            storedProcedureCall.Parameters.First(param => param.ParameterName == parameter.ParameterName).Value = parameter.Value;
        }

        return entities;
    }
}
