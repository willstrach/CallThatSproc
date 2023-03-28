namespace CallThatSproc.EntityFrameworkCore;

public interface ISqlCommandBuilder
{
    string BuildExecProcedureCommand(IStoredProcedureCall procedureCall);
}
