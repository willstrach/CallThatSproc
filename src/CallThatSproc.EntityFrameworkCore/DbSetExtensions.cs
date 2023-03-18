using Microsoft.EntityFrameworkCore;

namespace CallThatSproc.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static IEnumerable<TEntity> GetFromStoredProcedureCall<TEntity>(this DbSet<TEntity> dbSet, IStoredProcedureCall storedProcedureCall) where TEntity : class
    {
        var sqlString = storedProcedureCall.ToSqlString();
        var sqlParameters = storedProcedureCall.Parameters.Select(parameter => parameter.ToSqlParameter()).ToArray();

        var entities = dbSet.FromSqlRaw(sqlString, sqlParameters).ToList();

        return entities;
    }

    public static async Task<IEnumerable<TEntity>> GetFromStoredProcedureCallAsync<TEntity>(this DbSet<TEntity> dbSet, IStoredProcedureCall storedProcedureCall) where TEntity : class
    {
        var sqlString = storedProcedureCall.ToSqlString();
        var sqlParameters = storedProcedureCall.Parameters.Select(parameter => parameter.ToSqlParameter()).ToArray();

        var entities = await dbSet.FromSqlRaw(sqlString, sqlParameters).ToListAsync();

        return entities;
    }
}
