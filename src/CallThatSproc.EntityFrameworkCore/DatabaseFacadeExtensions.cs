using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CallThatSproc;

public static class DatabaseFacadeExtensions
{
    public static int ExecuteStoredProcedureCall(this DatabaseFacade databaseFacade, IStoredProcedureCall storedProcedureCall, ISqlCommandBuilder? commandBuilder = default)
    {
        var builder = commandBuilder is null ? new MsSqlCommandBuilder() : commandBuilder;

        var commandString = builder.BuildExecProcedureCommand(storedProcedureCall);
        return databaseFacade.ExecuteSqlRaw(commandString, storedProcedureCall.Parameters);
    }

    public static async Task<int> ExecuteStoredProcedureCallAsync(this DatabaseFacade databaseFacade, IStoredProcedureCall storedProcedureCall, ISqlCommandBuilder? commandBuilder = default)
    {
        var builder = commandBuilder is null ? new MsSqlCommandBuilder() : commandBuilder;

        var commandString = builder.BuildExecProcedureCommand(storedProcedureCall);
        return await databaseFacade.ExecuteSqlRawAsync(commandString, storedProcedureCall.Parameters);

    }
}
